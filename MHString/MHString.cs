using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace OCSS.StringUtil {

   /// <summary>String Utility functions</summary>
   public static class MHString {

      public static readonly string DoubleQuoteEN = "\"";
      public static readonly string SingleQuoteEN = "'";

      public static readonly char DoubleQuoteCharEN = DoubleQuoteEN[0];
      public static readonly char SingleQuoteCharEN = SingleQuoteEN[0];

      /// <summary>Returns a substring delimited by character Delim.</summary>
      /// <param name="str">the string to parse</param>
      /// <param name="segNum">segment number to return</param>
      /// <param name="numSegs">number of segments to return</param>
      /// <param name="delim">character delimiter</param>
      /// <returns>segment if found. Otherwise, an empty string</returns>
      /// <example>
      /// MHString.StrSeg("abc,def,gh,123",3,1,',') = "gh"
      /// MHString.StrSeg("abc,def,gh,123",2,2,',') = "def,gh"
      /// MHString.StrSeg("abc,def,gh,123",99,1,',') = ""
      /// </example>
      public static string StrSeg(string str, int segNum, int numSegs, char delim) {
         int delimCnt, charCnt, strNdx, startSegNdx;

         if ((numSegs < 1) || (segNum < 1) || (str.Length == 0))
            return String.Empty;

         segNum--;
         strNdx = 0;
         while (segNum > 0) {
            if (str[strNdx] == delim)
               segNum--;
            strNdx++;
            if (strNdx >= str.Length)
               break;
         }
         if (segNum == 0) {
            delimCnt = 0;
            charCnt = 0;
            for (startSegNdx = strNdx; startSegNdx < str.Length; startSegNdx++) {
               if (str[startSegNdx] == delim) {
                  delimCnt++;
                  if (delimCnt == numSegs)
                     break;
               }
               charCnt++;
            }
            if (charCnt > 0)
               return str.Substring(strNdx, charCnt);
         }
         return String.Empty;
      }

      /// <summary>Calls StrSeg with numSegs = 1</summary>
      public static string StrSeg(string str, int segNum, char delim) {
         return StrSeg(str, segNum, 1, delim);
      }

      /// <summary>Locates a search string within a delimited string</summary>
      /// <param name="searchStr">string to search for</param>
      /// <param name="searchIn">string to search in</param>
      /// <param name="startSegNum">starting segment number</param>
      /// <param name="incBy">1=process each segment. 2=every other, etc.</param>
      /// <param name="delim">delimiter for segments</param>
      /// <param name="isCaseSensitive">flag for case sensitive search</param>
      /// <returns>the integer segment number (1-x) matching the string or zero for not found</returns>
      /// <example>
      /// StrSegLocate("", "123,456,789", 1, 1, ',', false) = 0
      /// StrSegLocate("ABC", "def,abc", 2, 1, ',', false) = 2
      /// StrSegLocate("123", "123,456,789", 2, 1, ',', true) = 0 (since starting at 2 will skip the first match)
      /// StrSegLocate("456", "123,456,789", 1, 2, ',', true) = 0 (since incBy of 2 will pass by the 2nd entry)
      /// </example>
      public static int StrSegLocate(string searchStr, string searchIn, int startSegNum, int incBy, char delim, bool isCaseSensitive) {
         //if (incBy < 1)
         //   throw new ArgumentException("Increment by must be >= 1");
         if ((startSegNum <= 0) || (incBy <= 0) || (searchIn.Length == 0) || (searchStr.Length == 0))
            return 0;

         StringComparison comp = (isCaseSensitive) ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
         int numDelim = CharCount(searchIn, delim) + 1;
         while (startSegNum <= numDelim) {
            string seg = StrSeg(searchIn, startSegNum, 1, delim);
            if (!isCaseSensitive)
               seg = seg.ToUpper();
            if (seg.Equals(searchStr, comp))
               return startSegNum;
            startSegNum += incBy;
         }
         return 0;
      }

      /// <summary>Count all occurences of a delimiter in a string</summary>
      /// <param name="str">string to search in</param>
      /// <param name="searchChar">delimiter</param>
      /// <returns>count of searchChar</returns>
      public static int CharCount(string str, char searchChar) {
         int cnt = 0;
         for (int z = 0; z < str.Length; z++) {
            if (str[z] == searchChar)
               cnt++;
         }
         return cnt;
      }

      /// <summary>Returns a truncated string</summary>
      /// <param name="str">string to process</param>
      /// <param name="maxLength">one-based position to chop string</param>
      /// <returns>truncated string</returns>
      /// <remarks>This avoids "Out of Range" exceptions when using String.Substring(0,Maxlength) when string length is less than Maxlength</remarks>
      public static string Truncate(string str, int maxLength) {
         return str.Substring(0, Math.Min(maxLength, str.Length));
      }

      /// <summary>Returns letters only from a string</summary>
      /// <param name="str">string to process</param>
      /// <returns>string containing letters only</returns>
      /// <remarks>Uses char.IsLetter() internally</remarks>
      public static string LetterOnly(string str) {
         char[] arr = str.Where(c => char.IsLetter(c)).ToArray();
         return new string(arr);
      }

      /// <summary>Returns letters and numbers only from a string</summary>
      /// <param name="str">string to process</param>
      /// <returns>string containing only letters and numbers</returns>
      /// <remarks>Uses IsLetterOrDigit() internally</remarks>
      public static string LetterOrNumOnly(string str) {
         char[] arr = str.Where(c => char.IsLetterOrDigit(c)).ToArray();
         return new string(arr);
      }

      /// <summary>Returns numbers only from a string</summary>
      /// <param name="str">string to process</param>
      /// <returns>string containing numbers only</returns>
      /// <remarks>Uses char.IsDigit() internally</remarks>
      public static string NumericsOnly(string str) {
         char[] arr = str.Where(c => char.IsDigit(c)).ToArray();
         return new string(arr);
      }

      /// <summary>Returns numbers plus a user-defined set of other characters from a string</summary>
      /// <param name="str">string to process</param>
      /// <param name="extraAllowChars">string of extra characters allowed</param>
      /// <returns>string containing letters only</returns>
      /// <remarks>Uses char.IsDigit() internally for the numbers portion</remarks>
      public static string NumericsPlusExtra(string str, string extraAllowChars) {
         char[] arr = str.Where(c => char.IsDigit(c) || extraAllowChars.Contains(c.ToString())).ToArray();
         return new string(arr);
      }

      /// <summary>Formats a phone number into (000) 000-0000 [#optional ext] US format</summary>
      /// <param name="telephone">phone number to process</param>
      /// <param name="areaCode">area code to add if telephone is only 7 digits</param>
      /// <param name="prefix">prefix to add if telephone number is only 4 digits</param>
      /// <returns>formatted phone number</returns>
      public static string FmtPhoneLikePS(string telephone, string areaCode, string prefix) {
         const string INTERNATIONAL_DIAL = "1";
         const string LEFT_PARENS = "(";
         const string RIGHT_PARENS = ")";
         const string DASH = "-";
         const string HASHMARK = "#";

         // grab only numerics and an optional extension marker (#)
         string teleDigits = NumericsPlusExtra(telephone, HASHMARK);
         string ext = teleDigits.StrSeg(2, 1, HASHMARK[0]);
         teleDigits = teleDigits.StrSeg(1, 1, HASHMARK[0]);

         // strip leading INTERNATIONAL_DIAL if it's 11 digits and starts with INTERNATIONAL_DIAL
         if ((teleDigits.Length == 11) && (teleDigits.Substring(0, 1) == INTERNATIONAL_DIAL))
            teleDigits = teleDigits.Substring(1);
         // if full 10, format it
         if (teleDigits.Length == 10)
            teleDigits = LEFT_PARENS + teleDigits.Substring(0, 3) + RIGHT_PARENS + " " + teleDigits.Substring(3, 3) + DASH + teleDigits.Substring(6, 4);
         // if seven digits, add default area code and prefix
         if (teleDigits.Length == 7)
            teleDigits = LEFT_PARENS + areaCode + RIGHT_PARENS + " " + teleDigits.Substring(0, 3) + DASH + teleDigits.Substring(3, 4);
         // if four digits, add default prefix
         if (teleDigits.Length == 4)
            teleDigits = LEFT_PARENS + areaCode + RIGHT_PARENS + " " + prefix + DASH + teleDigits;
         // add extension back in if needed
         if (ext != String.Empty)
            teleDigits = teleDigits + " " + HASHMARK + ext;

         return teleDigits;
      }

      /// <summary>Replaces Control Characters in a string with another character</summary>
      /// <param name="str">Input value</param>
      /// <param name="replacementChar">character to replace constrol characters with</param>
      /// <returns>converted string</returns>
      /// <remarks>
      /// From MS:
      /// Control characters are formatting and other non-printing characters, such as ACK, BEL, CR, FF, LF, and VT.
      /// The Unicode standard assigns code points from \U0000 to \U001F, \U007F, and from \U0080 to \U009F to control characters.
      /// According to the Unicode standard, these values are to be interpreted as control characters unless their use is otherwise defined by an application.
      /// Valid control characters are members of the UnicodeCategory.Control category.
      /// </remarks>
      public static string ReplaceControlChars(string str, char replacementChar) {
         // Replace all control characters in a string with another character
         var charArray = str.ToCharArray();
         for (int z = 0; z < charArray.Length; z++) {
            if (Char.IsControl(charArray[z]))
               charArray[z] = replacementChar;
         }
         return new string(charArray);
      }

      /// <summary>Returns the rightmost segment of a string</summary>
      /// <param name="str">string to process</param>
      /// <param name="cnt">the number of characters from the right to extract</param>
      /// <returns>string containing the x right-most characters</returns>
      public static string StrRight(string str, int cnt) {
         if (cnt <= 0)
            return string.Empty;
         if (cnt < str.Length)
            return str.Substring(str.Length - cnt);
         return str;
      }

      /// <summary>calls overload as TrueFalseToWord(flag, "Yes", "No")</summary>
      public static string TrueFalseToWord(bool flag) {
         return TrueFalseToWord(flag, "Yes", "No");
      }

      /// <summary>Translates a boolean to true/false words</summary>
      /// <param name="flag">flag to test</param>
      /// <param name="trueVal">true value to return</param>
      /// <param name="falseVal">false value to return</param>
      /// <returns>trueVal or falseVal depending on flag</returns>
      public static string TrueFalseToWord(bool flag, string trueVal, string falseVal) {
         return (flag) ? trueVal : falseVal;
      }

      /// <summary>returns a file name with all illegal characters stripped out</summary>
      /// <param name="fileNameIn">file name to process</param>
      /// <returns>a clean file name</returns>
      /// <remarks>
      /// Uses Path.GetInvalidFileNameChars() internally, so not sure about cross-platform compatibility.
      /// Returned file name can be an empty string if all characters are illegal.
      /// </remarks>
      public static string CleanFileName(string fileNameIn) {
         string illegalNameSet = new string(Path.GetInvalidFileNameChars());
         char[] arr = fileNameIn.Where(c => (illegalNameSet.IndexOf(c) == -1)).ToArray();
         return new string(arr);
      }

      /// <summary>Returns a substring from a string without worrying about string bounds on startPos</summary>
      /// <param name="str">string to process</param>
      /// <param name="startPos">zero-based starting position</param>
      /// <param name="cnt">count of characters to extract starting at startPos</param>
      /// <returns>Returns the substring</returns>
      /// <remarks>No exceptions if startPos > string.length or startPos + cnt > string.length</remarks>
      public static string SubstringSafe(string str, int startPos, int cnt) {
         if ((startPos >= str.Length) || (startPos < 0))
            return String.Empty;
         if (startPos + cnt <= (str.Length))
            return str.Substring(startPos, cnt);
         return str.Substring(startPos, str.Length - startPos);
      }

      public static string HexToBase64(string hexStr) {
         return Convert.ToBase64String(HexToByte(hexStr));
      }

      public static byte[] HexToByte(string hexStr) {
         if (hexStr.Length % 2 > 0)
            throw new ArgumentException("Hex Input has invalid length.");
         byte[] buffer = new byte[hexStr.Length / 2];
         int cnt = 0;
         for (int z = 0; z < hexStr.Length; z += 2) {
            buffer[cnt++] = byte.Parse(hexStr.Substring(z, 2), NumberStyles.HexNumber);
         }

         return buffer;
      }

      public static string ByteToHex(byte[] buffer, bool convUpper = false) {
         char[] HEXCONST = { '0', '1', '2', '3', '4', '5', '6', '7', '8' ,'9', 'a', 'b', 'c', 'd', 'e', 'f',
                             '0', '1', '2', '3', '4', '5', '6', '7', '8' ,'9', 'A', 'B', 'C', 'D', 'E', 'F' };

         char[] hexBuffer = new char[buffer.Length * 2]; // size output buffer
         int charOffset = 0;  // lower case has no offset
         if (convUpper == true)
            charOffset = 16;     // upper needs to use the 2nd set

         int cnt = 0;
         for (int z = 0; z < buffer.Length; z++) {
            byte upperNibble = (byte) (((byte) (buffer[z] & 0xf0)) >> 4);
            hexBuffer[cnt++] = HEXCONST[upperNibble + charOffset];
            byte lowerNibble = (byte) (buffer[z] & 0x0f);
            hexBuffer[cnt++] = HEXCONST[lowerNibble + charOffset];
         }

         return new string(hexBuffer);
      }

      /// <summary>Calculates Hamming distance for two strings</summary>
      /// <param name="str1"></param>
      /// <param name="str2"></param>
      /// <returns>an unsigned integer of the Hamming distance</returns>
      /// <remarks>Uses Knuth's algorithm from: http://en.wikipedia.org/wiki/Hamming_distance </remarks>
      public static UInt32 HammingDistance(string str1, string str2) {
         if (str1.Length != str2.Length)
            throw new ArgumentException("Length of strings must be equal.");

         char[] c1 = str1.ToCharArray();
         char[] c2 = str2.ToCharArray();
         UInt32 cnt = 0;
         for (int z = 0; z < c1.Length; z++) {
            UInt32 val = (UInt32) c1[z] ^ (UInt32) c2[z];
            while (val > 0) {
               cnt++;
               val &= val - 1;
            }
         }

         return cnt;
      }

      public static UInt32 HammingDistance(byte[] s, int startPos1, int startPos2, int numBytes) {
         UInt32 cnt = 0;
         for (int z = 0; z < numBytes; z++) {
            UInt32 val = (UInt32) s[startPos1++] ^ (UInt32) s[startPos2++];
            while (val > 0) {
               cnt++;
               val &= val - 1;
            }
         }

         return cnt;
      }

      /// <summary>Tests for a valid email address using a regular expresion</summary>
      /// <param name="email">email address</param>
      /// <returns>true if valid</returns>
      /// <remarks>Uses regex of: "\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}"</remarks>
      public static bool IsValidEmail(string email) {
         if (string.IsNullOrWhiteSpace(email))
            return false;
         try {
            Regex expression = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
            return expression.IsMatch(email);
         }
         catch (RegexMatchTimeoutException) {
            return false;
         }
      }

      /// <summary>Changes a string to all lower-case and 1st character upper-case</summary>
      /// <param name="str">string to process</param>
      /// <returns>transformed string</returns>
      public static string UpperFirstLetter(string str) {
         if (string.IsNullOrWhiteSpace(str))
            return string.Empty;

         char[] strArray = str.ToLower().ToCharArray();
         strArray[0] = char.ToUpper(strArray[0]);

         return new string(strArray);
      }

      /// <summary>Performs initial capitalization of any letters following a non-alpha character</summary>
      /// <param name="str">string to process</param>
      /// <returns>transformed string</returns>
      public static string InitCaps(string str) {

         if (string.IsNullOrEmpty(str))
            return string.Empty;

         char[] strArray = str.ToLower().ToCharArray();
         bool lastWasAlpha = false;
         for (int z = 0; z < strArray.Length; z++) {
            if (lastWasAlpha == false)
               strArray[z] = char.ToUpper(strArray[z]);
            lastWasAlpha = char.IsLetter(strArray[z]);
         }

         return new string(strArray);
      }

      /// <summary>Unquotes a string</summary>
      /// <param name="str">string to unquote</param>
      /// <param name="quoteCharSet">set of quote characters to check for</param>
      /// <returns>unquoted string</returns>
      /// <remarks>
      /// 1) Only one set of quotes are stripped. It does not continuously unquote. For that use .Trim(quoteChar[])
      /// 2) Unbalanced quotes are not stripped. If only one is found, the string is returned as is.
      /// 2) The quote characters can be different. Example: “This is a test”
      ///    This function only tests if starting and ending characters are in the set quoteCharSet[]. You must pre-trim the string passed to the function, if needed.
      /// </remarks>
      public static string Unquote(string str, char[] quoteCharSet) {
         if (str.Length >= 2 && quoteCharSet.Length > 0) {  // return as-is if not at least 2 chars and a set of quote chars to match
            bool foundFirst = false;
            foreach (var c in quoteCharSet) {
               if (str[0] == c) {  // first char matches one of the quote set
                  foundFirst = true;
                  break;
               }
            }
            if (foundFirst) {
               foreach (var c in quoteCharSet) {
                  if (str[str.Length - 1] == c) {   // last char matches one of the quote set
                     return str.SubstringSafe(1, str.Length - 2);
                  }
               }
            }
         }

         return str;
      }

      /// <summary>Parse an IPv4 address</summary>
      /// <param name="ip">IPv4 address in string form</param>
      /// <returns>A "normalized" IPv4 Address with whitespace and any leading zeros removed.</returns>
      /// <remarks>
      /// Throws a FormatException if the IP Address is an invalid format or number (0-255).
      ///  To avoid exceptions, you can use TryParseIPv4 instead or trap it yourself.
      /// </remarks>
      public static string ParseIPv4(string ip) {
         if (string.IsNullOrWhiteSpace(ip))
            throw new FormatException("IPv4 Address is null or empty.");
         var parts = ip.Split(new char[] { '.' }, StringSplitOptions.None);
         if (parts.Length != 4)
            throw new FormatException("IPv4 Address does not contain 4 segments separated by a dot.");
         for (int z = 0; z < 4; z++) {
            var num = 0;
            // using overridden method so we can say we only want to consider integral digits
            if (int.TryParse(parts[z].Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out num) == false)
               throw new FormatException($"Segment {z + 1} of IPv4 Address ({parts[z]}) is not strictly numeric (digits only).");
            if (num < 0 || num > 255)
               throw new FormatException($"Segment {z + 1} of IPv4 Address ({parts[z]}) is not between 0 and 255.");
            parts[z] = num.ToString();
         }
         // success starts here
         return string.Join(".", parts);   // normalize segments into valid numbers (in case of whitespace or leading zeros)
      }

      /// <summary>Parse an IPv4 address</summary>
      /// <param name="ip">IPv4 address in string form</param>
      /// <param name="normalizedIp">Filled with a "normalized" IPv4 Address (whitespace and any leading zeros removed).</param>
      /// <returns>True if a valid IPv4 Address. Otherwise, False.</returns>
      /// <remarks>FormatException exceptions are trapped and not thrown. False will be returned instead. See ParseIPv4 for details on validations.</remarks>
      public static bool TryParseIPv4(string ip, out string normalizedIp) {
         normalizedIp = string.Empty;
         try {
            normalizedIp = ParseIPv4(ip);
            return true;
         }
         catch (FormatException) {
            return false;
         }
      }

      /// <summary>Semi-intelligent comma-separated splitter for CSV data</summary>
      /// <param name="str">string to split</param>
      /// <param name="opts">parse options</param>
      /// <returns>string array of split elements</returns>
      /// <remarks>an imbalance of quote delimiters will throw a FormatException</remarks>
      public static string[] CSVSplit(string str, CSVParseOpts opts) {

         string[] fields = new string[0];
         if (str == string.Empty)
            return fields;

         if (opts.AreFieldsEnclosed == false) {
            fields = str.Split(opts.FieldDelimiter);    // no embedded delimiters-just use .Split()
            if (opts.TrimOutside) {
               for (int z = 0; z < fields.Length; z++) {
                  fields[z] = TrimCSVField(fields[z], opts);
               }
            }
         }
         else {
            List<String> fieldlist = new List<string>();
            bool inEnclosure = false;
            int startPos = 0;
            int p = 0;
            while (p < str.Length) {
               if (str[p] == opts.FieldDelimiter) {
                  if (inEnclosure) {
                     p++;  // skip past delimiter as it's embedded inside an enclosure
                  }
                  else {
                     // process field
                     fieldlist.Add(TrimCSVField(ExtractCSVField(str, startPos, p, opts), opts));
                     p++;
                     startPos = p;
                  }
               }
               else {
                  if (str[p] == opts.EnclosureDelimiter) {
                     inEnclosure = !inEnclosure;   // toggle enclosure flag
                  }
                  p++;
               }
            }
            if (inEnclosure)
               throw new FormatException("Imbalance of delimiters in line");
            // process last field
            fieldlist.Add(TrimCSVField(ExtractCSVField(str, startPos, p, opts), opts));
            fields = fieldlist.ToArray();
         }
         return fields;
      }

      private static string ExtractCSVField(string str, int startPos, int currPos, CSVParseOpts opts) {
         if (currPos == startPos)
            return string.Empty;
         return str.Substring(startPos, (currPos - startPos));
      }

      /// <summary>Trim CSV element</summary>
      /// <param name="str"></param>
      /// <param name="opts"></param>
      /// <returns>Trimmed element</returns>
      /// <remarks>
      ///   1) trim outside if TrimOutside flag is set
      ///   2) remove enclosures if enclosures are defined
      ///   3) trim inside if enclosures are defined
      ///   4) unescape delimiters if enclosures are defined
      /// </remarks>
      private static string TrimCSVField(string str, CSVParseOpts opts) {
         if (opts.TrimOutside)
            str = str.Trim();
         if (opts.AreFieldsEnclosed) {
            if (str.StartsWith(opts.EnclosureDelimiter.ToString()) && str.EndsWith(opts.EnclosureDelimiter.ToString()))
               str = str.Substring(1, str.Length - 2);
            if (opts.TrimInEnclosure)
               str = str.Trim();
            // change double enclosure to single enclosure
            str = str.Replace(opts.EnclosureDelimiter.ToString() + opts.EnclosureDelimiter.ToString(), opts.EnclosureDelimiter.ToString());
         }
         return str;
      }
   }

   public class CSVParseOpts {
      public char FieldDelimiter { get; set; }
      public char EnclosureDelimiter { get; set; }
      public bool AreFieldsEnclosed { get; set; }
      public bool TrimOutside { get; set; }
      public bool TrimInEnclosure { get; set; }

      public CSVParseOpts() {
         FieldDelimiter = ',';  // default to comma
         EnclosureDelimiter = '\"';  // default to quote
         AreFieldsEnclosed = true;
         TrimOutside = true;
         TrimInEnclosure = false;
      }
   }

}
