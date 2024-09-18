using System;
using System.Collections.Generic;

namespace OCSS.StringUtil {

   /// <summary>Text-wrapping (breaking) functions included in MHString partial class</summary>
   /// <remarks>This is English specific, but may work for other Latin languages.</remarks>
   public static partial class MHString {

      public static readonly string HyphenEN = "-";
      public static readonly string HyphenUnicode = "\u2010";

      public static readonly TextLineSegment EmptySegment = new TextLineSegment(0, 0, false);

      /// <summary>Get a list of text from the segment(s) (TextLineSegment) passed to this method</summary>
      /// <param name="lines">IEnumerable of type TextLineSegment</param>
      /// <param name="origText">The original text that produced the list</param>
      /// <param name="useUnicodeHyphen">If true, the Unicode hyphen is used</param>
      /// <returns>IEnumerable of type string</returns>
      /// <remarks>
      ///   This function only needs to be used when caller wants to do something with TextLineSegments.
      ///   Otherwise, use the overloaded version that does not need TextLineSegments
      ///   </remarks>
      public static IEnumerable<string> GetWrappedText(IEnumerable<TextLineSegment> lines, string origText, bool useUnicodeHyphen = false) {
         foreach (var line in lines) {
            string txt = line.Length == 0 ? string.Empty : origText.Substring(line.StartPos, line.Length);
            if (line.HyphenNeeded) {
               txt += (useUnicodeHyphen ? HyphenUnicode : HyphenEN);
            }
            yield return txt;
         }
      }

      /// <summary>Word-wrap text with max length limit</summary>
      /// <param name="txt">text to wrap</param>
      /// <param name="maxLen">max size of line</param>
      /// <param name="minWrapSizeThreshold">If greater than zero and less maxLen-1, specifies the minimum number of characters to wrap the sentence (using space characters). If the criteria is not met, a hyphenated line will be used instead of a short line.</param>
      /// <param name="preserveWhitespace">Flag whether to preserve whitespace in between wrapped section; false is typically a simple/dumb wrapping</param>
      /// <returns>IEnumerable of type TextLineSegments describing segments</returns>
      /// <exception cref="ArgumentException">When maxLen is less than or equal to zero</exception>
      /// <remarks>
      /// This is used when the caller doesn't care about the TextLineSegments.
      /// This avoids wrapped functions where the text needs to passed twice and the caller has to save the intermediate TextLineSegments.
      ///   Example:
      ///      var lines = GetWrappedText(WrapText(myLongText, 50, 35), myLongText).ToList();
      ///   Better: 
      ///      var lines = GetWrappedText(myLongText, 50, 35);
      ///      
      ///   Tech Note: this doesn't need much testing as the the WrapText is extensively tested
      /// </remarks>
      public static IEnumerable<string> GetWrappedText(string txt, int maxLen, int minWrapSizeThreshold, bool preserveWhitespace = false, bool useUnicodeHyphen = false) {
         return GetWrappedText(WrapText(txt, maxLen, minWrapSizeThreshold, preserveWhitespace), txt, useUnicodeHyphen);
      }

      /// <summary>Overloaded method that doesn't require a minWrapSizeThreshold for wrapped lines</summary>
      /// <param name="txt">text to wrap</param>
      /// <param name="maxLen">max size of line</param>
      /// <param name="preserveWhitespace">Flag whether to preserve whitespace in between wrapped section; false is typically a simple/dumb wrapping</param>
      /// <returns>IEnumerable of type TextLineSegments describing segments</returns>
      public static IEnumerable<TextLineSegment> WrapText(string txt, int maxLen, bool preserveWhitespace = false) {
         return WrapText(txt, maxLen, 0, preserveWhitespace);
      }

      /// <summary>Word-wrap text with max length limit</summary>
      /// <param name="txt">text to wrap. If null, it's treated as an empty string</param>
      /// <param name="maxLen">max size of line</param>
      /// <param name="minWrapSizeThreshold">If greater than zero and less maxLen-1, specifies the minimum number line size in order to wrap a sentence using space characters. If the criteria is not met, a hyphened line will be used instead of a short line.</param>
      /// <param name="preserveWhitespace">Flag whether to preserve whitespace in between wrapped section; false is typically a simple/dumb wrapping</param>
      /// <returns>IEnumerable of type TextLineSegments describing segments</returns>
      /// <exception cref="ArgumentException">When maxLen is less than or equal to zero</exception>
      /// <remarks>
      ///   - This may not function as expected for non-English or non-Latin character sets.
      ///   - It is expected that the text only contains special characters CR, LF, or CRLF combos. Tabs should be replaced with desired spaces prior to calling the function.
      ///   - if the last block consists of just CR, LF, or CRLF, an empty TextLineSegment will not be generated.
      ///   - if null is passed for the text, it will be treated as an empty string (no exception)
      ///   - If max length is 1, no hyphens will be generated as there won't be room for the character itself. This is intended behaviour.
      ///   
      ///   Tech Notes: 
      ///      - Char.IsWhiteSpace includes Unicode Categories: SpaceSeparator, LineSeparator, ParagraphSeparator
      ///        and CHARACTER TABULATION (U+0009), LINE FEED (U+000A), LINE TABULATION (U+000B), FORM FEED (U+000C), CARRIAGE RETURN (U+000D), and NEXT LINE (U+0085).
      ///        
      ///      - Char.IsSeparator includes: SpaceSeparator, LineSeparator, and ParagraphSeparator but NOT LF, CR, FF
      ///      
      ///      - Char.IsControl includes: ACK, BEL, CR, FF, LF, VT, and Unicode code points from \U0000 to \U001F, \U007F, and from \U0080 to \U009F
      ///      
      /// </remarks>
      public static IEnumerable<TextLineSegment> WrapText(string txt, int maxLen, int minWrapSizeThreshold, bool preserveWhitespace = false) {
         if (maxLen <= 0)
            throw new ArgumentException("Max length must be greater than zero.");
         // if invalid min size, set to none
         if (minWrapSizeThreshold < 0 || minWrapSizeThreshold >= maxLen - 1)
            minWrapSizeThreshold = 0;
         if (string.IsNullOrEmpty(txt))
            yield break;
         int startPos = 0;
         int len = 0;
         int breakMarker = -1;
         int charNdx = startPos;
         while (charNdx < txt.Length) {
            // special case to look for Carriage Return
            bool triggerCR = txt[charNdx] == '\r';
            bool triggerLF = txt[charNdx] == '\n';
            if (triggerCR || triggerLF) {
               yield return new TextLineSegment(startPos, len, false);
               // reset starting position, length counter, and last punctuation for next block
               breakMarker = -1;
               len = 0;
               charNdx++;
               startPos = charNdx;
               // if processing a CR, check if next character is a LF. If so, skip past it
               if (triggerCR && charNdx < txt.Length) {
                  if (txt[charNdx] == '\n') {
                     charNdx++;  // skip past
                     startPos++;
                  }
               }
               if (preserveWhitespace == false) {
                  while (charNdx < txt.Length) {
                     if (txt[charNdx] == '\r' || txt[charNdx] == '\n' || txt[charNdx] != ' ')
                        break;
                     charNdx++;
                  }
                  startPos = charNdx;
               }
            }
            else {
               // handling of all other characters and whitespace
               if (char.IsPunctuation(txt, charNdx) || txt[charNdx] == ' ') {
                  breakMarker = charNdx;
               }
               len++;
               charNdx++;
               if (len >= maxLen) {
                  // if there's a possible break but it doesn't meet the min size threshold, set break as none
                  if (breakMarker >= 0) {
                     int tempSize = (breakMarker + 1) - startPos;
                     if (tempSize < minWrapSizeThreshold)
                        breakMarker = -1;    // set as no marker
                  }
                  // check for a recorded break marker 
                  if (breakMarker == -1) {
                     // no break marker, so use exact block with room for a hyphen, except when at end of text.
                     // Back up one character only if max length > 1 and not at end of text
                     bool needsHyphen = maxLen > 1 && charNdx < txt.Length;
                     if (needsHyphen) {
                        charNdx--;
                        len--;
                     }
                     yield return new TextLineSegment(startPos, len, needsHyphen);
                     // reset starting position, length counter, and last punctuation for next block
                     startPos = charNdx;
                  }
                  else {
                     yield return new TextLineSegment(startPos, (breakMarker + 1) - startPos, false);
                     charNdx = breakMarker + 1;
                     startPos = charNdx;
                  }
                  // reset length and last break marker
                  len = 0;
                  breakMarker = -1;
                  if (preserveWhitespace == false) {
                     while (charNdx < txt.Length) {
                        if (txt[charNdx] == '\r' || txt[charNdx] == '\n' || txt[charNdx] != ' ')
                           break;
                        charNdx++;
                     }
                     startPos = charNdx;
                  }
               }

            }
         }
         // handle any leftover text
         if (len > 0) {
            yield return new TextLineSegment(startPos, len, false);
         }
      }


   }

}
