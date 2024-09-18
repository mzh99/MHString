using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCSS.StringUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MHStringTest {

   [TestClass]
   public class TextWrapTests {

      /// <summary>Show detailed info about the TextLineSegment</summary>
      /// <param name="textBlock">the original text</param>
      /// <param name="segs">list of segments(already broken/wrapped)</param>
      /// <param name="useUnicodeHyphen">flag to use Unicode hyphen</param>
      public static void DumpTextSegments(string textBlock, IEnumerable<TextLineSegment> segs, bool useUnicodeHyphen = false) {
         int ndx = 0;
         int cnt = segs.ToList().Count;
         Debug.WriteLine($"Segments: {cnt}");
         foreach (TextLineSegment seg in segs) {
            string txt = seg.Length == 0 ? string.Empty : textBlock.Substring(seg.StartPos, seg.Length);
            if (seg.HyphenNeeded) {
               txt += (useUnicodeHyphen ? MHString.HyphenUnicode : MHString.HyphenEN);
            }
            Debug.WriteLine($"{ndx}: {txt} (offset: {seg.StartPos}, len: {seg.Length}, hyphen: {seg.HyphenNeeded})");
            ndx++;
         }
      }

      #region Tests with CR, LF, CRLF

      [TestMethod]
      public void BackToBackCRWorks() {
         var str = "ABCDE\r\rFGHI";
         var chunks = MHString.WrapText(str, 7).ToList();
         Assert.AreEqual(3, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(5, chunks[0].Length);

         Assert.AreEqual(6, chunks[1].StartPos);
         Assert.AreEqual(0, chunks[1].Length);

         Assert.AreEqual(7, chunks[2].StartPos);
         Assert.AreEqual(4, chunks[2].Length);

         for (int ndx = 0; ndx < chunks.Count; ndx++) {
            Assert.IsFalse(chunks[ndx].HyphenNeeded);
         }

      }

      [TestMethod]
      public void BackToBackLFWorks() {
         var str = "ABCDE\n\nFGHI";
         var chunks = MHString.WrapText(str, 7).ToList();
         Assert.AreEqual(3, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(5, chunks[0].Length);

         Assert.AreEqual(6, chunks[1].StartPos);
         Assert.AreEqual(0, chunks[1].Length);

         Assert.AreEqual(7, chunks[2].StartPos);
         Assert.AreEqual(4, chunks[2].Length);

         for (int ndx = 0; ndx < chunks.Count; ndx++) {
            Assert.IsFalse(chunks[ndx].HyphenNeeded);
         }
      }

      [TestMethod]
      public void BackToBackCRLFWorks() {
         var str = "ABCDE\r\n\r\nFGHI";
         var chunks = MHString.WrapText(str, 7).ToList();
         Assert.AreEqual(3, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(5, chunks[0].Length);

         Assert.AreEqual(7, chunks[1].StartPos);
         Assert.AreEqual(0, chunks[1].Length);

         Assert.AreEqual(9, chunks[2].StartPos);
         Assert.AreEqual(4, chunks[2].Length);

         for (int ndx = 0; ndx < chunks.Count; ndx++) {
            Assert.IsFalse(chunks[ndx].HyphenNeeded);
         }
      }

      [TestMethod]
      public void WrapWithEarlyCRLFWorks() {
         var str = "ABCDE\r\nFGHIJKLMNOPQ";
         var chunks = MHString.WrapText(str, 7).ToList();
         Assert.AreEqual(3, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(5, chunks[0].Length);
         Assert.IsFalse(chunks[0].HyphenNeeded);

         Assert.AreEqual(7, chunks[1].StartPos);
         // 6 because room for hyphen
         Assert.AreEqual(6, chunks[1].Length);
         Assert.IsTrue(chunks[1].HyphenNeeded);

         Assert.AreEqual(13, chunks[2].StartPos);
         Assert.AreEqual(6, chunks[2].Length);
         Assert.IsFalse(chunks[2].HyphenNeeded);
      }

      [TestMethod]
      public void WrapWithEarlyCRWorks() {
         var str = "ABCDE\rFGHIJKLMNOPQ";
         var chunks = MHString.WrapText(str, 7).ToList();
         Assert.AreEqual(3, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(5, chunks[0].Length);
         Assert.IsFalse(chunks[0].HyphenNeeded);

         Assert.AreEqual(6, chunks[1].StartPos);
         Assert.AreEqual(6, chunks[1].Length);
         Assert.IsTrue(chunks[1].HyphenNeeded);

         Assert.AreEqual(12, chunks[2].StartPos);
         Assert.AreEqual(6, chunks[2].Length);
         Assert.IsFalse(chunks[2].HyphenNeeded);
      }

      [TestMethod]
      public void WrapWithEarlyLFWorks() {
         var str = "ABCDE\nFGHIJKLMNOPQ";
         var chunks = MHString.WrapText(str, 7).ToList();
         Assert.AreEqual(3, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(5, chunks[0].Length);
         Assert.IsFalse(chunks[0].HyphenNeeded);

         Assert.AreEqual(6, chunks[1].StartPos);
         Assert.AreEqual(6, chunks[1].Length);
         Assert.IsTrue(chunks[1].HyphenNeeded);

         Assert.AreEqual(12, chunks[2].StartPos);
         Assert.AreEqual(6, chunks[2].Length);
         Assert.IsFalse(chunks[2].HyphenNeeded);
      }

      [TestMethod]
      public void WrapWithStartCRLFWorks() {
         var str = "\r\nABCDEFGHIJK";
         var chunks = MHString.WrapText(str, 7).ToList();
         Assert.AreEqual(3, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(0, chunks[0].Length);
         Assert.IsFalse(chunks[0].HyphenNeeded);

         Assert.AreEqual(2, chunks[1].StartPos);
         Assert.AreEqual(6, chunks[1].Length);
         Assert.IsTrue(chunks[1].HyphenNeeded);

         Assert.AreEqual(8, chunks[2].StartPos);
         Assert.AreEqual(5, chunks[2].Length);
         Assert.IsFalse(chunks[2].HyphenNeeded);
      }

      [TestMethod]
      public void WrapWithStartCRWorks() {
         var str = "\rABCDEFGHIJK";
         var chunks = MHString.WrapText(str, 7).ToList();
         Assert.AreEqual(3, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(0, chunks[0].Length);
         Assert.IsFalse(chunks[0].HyphenNeeded);

         Assert.AreEqual(1, chunks[1].StartPos);
         Assert.AreEqual(6, chunks[1].Length);
         Assert.IsTrue(chunks[1].HyphenNeeded);

         Assert.AreEqual(7, chunks[2].StartPos);
         Assert.AreEqual(5, chunks[2].Length);
         Assert.IsFalse(chunks[2].HyphenNeeded);
      }

      [TestMethod]
      public void WrapWithStartLFWorks() {
         var str = "\nABCDEFGHIJK";
         var chunks = MHString.WrapText(str, 7).ToList();
         Assert.AreEqual(3, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(0, chunks[0].Length);
         Assert.IsFalse(chunks[0].HyphenNeeded);

         Assert.AreEqual(1, chunks[1].StartPos);
         Assert.AreEqual(6, chunks[1].Length);
         Assert.IsTrue(chunks[1].HyphenNeeded);

         Assert.AreEqual(7, chunks[2].StartPos);
         Assert.AreEqual(5, chunks[2].Length);
         Assert.IsFalse(chunks[2].HyphenNeeded);
      }

      [TestMethod]
      public void WrapWithEndCRLFWorks() {
         var str = "ABCDEFGHIJK\r\n";
         var chunks = MHString.WrapText(str, 7).ToList();
         Assert.AreEqual(2, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(6, chunks[0].Length);
         Assert.IsTrue(chunks[0].HyphenNeeded);

         Assert.AreEqual(6, chunks[1].StartPos);
         Assert.AreEqual(5, chunks[1].Length);
         Assert.IsFalse(chunks[1].HyphenNeeded);
      }

      [TestMethod]
      public void WrapWithEndCRWorks() {
         var str = "ABCDEFGHIJK\r";
         var chunks = MHString.WrapText(str, 7).ToList();
         Assert.AreEqual(2, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(6, chunks[0].Length);
         Assert.IsTrue(chunks[0].HyphenNeeded);

         Assert.AreEqual(6, chunks[1].StartPos);
         Assert.AreEqual(5, chunks[1].Length);
         Assert.IsFalse(chunks[1].HyphenNeeded);
      }

      [TestMethod]
      public void WrapWithEndLFWorks() {
         var str = "ABCDEFGHIJK\r\n";
         var chunks = MHString.WrapText(str, 7).ToList();
         Assert.AreEqual(2, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(6, chunks[0].Length);
         Assert.IsTrue(chunks[0].HyphenNeeded);

         Assert.AreEqual(6, chunks[1].StartPos);
         Assert.AreEqual(5, chunks[1].Length);
         Assert.IsFalse(chunks[1].HyphenNeeded);
      }

      [TestMethod]
      public void WrapOneLenWithCRWorks() {
         var str = "AB\rDE";
         var chunks = MHString.WrapText(str, 1).ToList();
         Assert.AreEqual(str.Length, chunks.Count);
         for (int cnt = 0; cnt < str.Length; cnt++) {
            Assert.AreEqual(cnt, chunks[cnt].StartPos);
            Assert.AreEqual(cnt == 2 ? 0 : 1, chunks[cnt].Length);
            Assert.IsFalse(chunks[cnt].HyphenNeeded);
         }
      }

      [TestMethod]
      public void WrapOneLenWithLFWorks() {
         var str = "AB\nDE";
         var chunks = MHString.WrapText(str, 1).ToList();
         Assert.AreEqual(str.Length, chunks.Count);
         for (int cnt = 0; cnt < str.Length; cnt++) {
            Assert.AreEqual(cnt, chunks[cnt].StartPos);
            Assert.AreEqual(cnt == 2 ? 0 : 1, chunks[cnt].Length);
            Assert.IsFalse(chunks[cnt].HyphenNeeded);
         }
      }

      #endregion

      [TestMethod]
      public void WrapWithExtraSpaceAtBreakWorks() {
         var str = "This is a test    with spaces.";
         var chunks = MHString.WrapText(str, 15).ToList();
         DumpTextSegments(str, chunks);
         Assert.AreEqual(2, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(15, chunks[0].Length);
         Assert.IsFalse(chunks[0].HyphenNeeded);

         Assert.AreEqual(18, chunks[1].StartPos);
         Assert.AreEqual(12, chunks[1].Length);
         Assert.IsFalse(chunks[1].HyphenNeeded);
      }

      [TestMethod]
      public void WrapWithLongMiddleLineWorksWithSizeThreshold() {
         var str = "This is a sentence with a veryveryveryveryveryveryveryveryvery long word.";
         var chunks = MHString.WrapText(str, 20, 10).ToList();
         DumpTextSegments(str, chunks);
         Assert.AreEqual(4, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(19, chunks[0].Length);
         Assert.IsFalse(chunks[0].HyphenNeeded);

         Assert.AreEqual(19, chunks[1].StartPos);
         Assert.AreEqual(19, chunks[1].Length);
         Assert.IsTrue(chunks[1].HyphenNeeded);

         Assert.AreEqual(38, chunks[2].StartPos);
         Assert.AreEqual(19, chunks[2].Length);
         Assert.IsTrue(chunks[2].HyphenNeeded);

         Assert.AreEqual(57, chunks[3].StartPos);
         Assert.AreEqual(16, chunks[3].Length);
         Assert.IsFalse(chunks[3].HyphenNeeded);
      }

      [TestMethod]
      public void WrapWithLongMiddleLineWorksNoSizeThreshold() {
         var str = "This is a sentence with a veryveryveryveryveryveryveryveryveryveryvery long word.";
         var chunks = MHString.WrapText(str, 20).ToList();
         DumpTextSegments(str, chunks);
         Assert.AreEqual(5, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(19, chunks[0].Length);
         Assert.IsFalse(chunks[0].HyphenNeeded);

         Assert.AreEqual(19, chunks[1].StartPos);
         Assert.AreEqual(7, chunks[1].Length);
         Assert.IsFalse(chunks[1].HyphenNeeded);

         Assert.AreEqual(26, chunks[2].StartPos);
         Assert.AreEqual(19, chunks[2].Length);
         Assert.IsTrue(chunks[2].HyphenNeeded);

         Assert.AreEqual(45, chunks[3].StartPos);
         Assert.AreEqual(19, chunks[3].Length);
         Assert.IsTrue(chunks[3].HyphenNeeded);

         Assert.AreEqual(64, chunks[4].StartPos);
         Assert.AreEqual(17, chunks[4].Length);
         Assert.IsFalse(chunks[4].HyphenNeeded);
      }

      [TestMethod]
      public void WrapOneLenSimpleWorks() {
         // Note: when using a wrap count of 1, no hyphens will be generated
         var str = "ABCDE";
         var chunks = MHString.WrapText(str, 1).ToList();
         Assert.AreEqual(str.Length, chunks.Count);
         for (int cnt = 0; cnt < str.Length; cnt++) {
            Assert.AreEqual(cnt, chunks[cnt].StartPos);
            Assert.AreEqual(1, chunks[cnt].Length);
            Assert.IsFalse(chunks[cnt].HyphenNeeded);
         }
      }

      [TestMethod]
      public void SimplePunctWorks() {
         var str = "Now is the time for all good people to come to the aid of their country.";
         var chunks = MHString.WrapText(str, 20).ToList();
         DumpTextSegments(str, chunks);
         Assert.AreEqual(4, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(20, chunks[0].Length);

         Assert.AreEqual(20, chunks[1].StartPos);
         Assert.AreEqual(19, chunks[1].Length);

         Assert.AreEqual(39, chunks[2].StartPos);
         Assert.AreEqual(19, chunks[2].Length);

         Assert.AreEqual(58, chunks[3].StartPos);
         Assert.AreEqual(14, chunks[3].Length);

         for (int ndx = 0; ndx < chunks.Count; ndx++) {
            Assert.IsFalse(chunks[ndx].HyphenNeeded);
         }
      }
      [TestMethod]
      public void LongWordsWithMinSizeWorks() {
         // Note: a carefully crafted sentence that does not break on words due to min size of 12
         var str = "Quite fastidiously did he surreptitiously succeed.";
         var chunks = MHString.WrapText(str, 17, 12).ToList();
         DumpTextSegments(str, chunks);
         Assert.AreEqual(4, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(16, chunks[0].Length);

         Assert.AreEqual(16, chunks[1].StartPos);
         Assert.AreEqual(16, chunks[1].Length);

         Assert.AreEqual(32, chunks[2].StartPos);
         Assert.AreEqual(16, chunks[2].Length);

         Assert.AreEqual(48, chunks[3].StartPos);
         Assert.AreEqual(2, chunks[3].Length);

         for (int ndx = 0; ndx < chunks.Count; ndx++) {
            Assert.AreEqual(ndx != 3, chunks[ndx].HyphenNeeded);
         }
      }

      [TestMethod]
      public void MultiplePunctDefaultWorks() {
         var str = "I am good, although not excellent. I aim, sometimes, to excel.";
         var chunks = MHString.WrapText(str, 10, false).ToList();
         DumpTextSegments(str, chunks);
         Assert.AreEqual(7, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(10, chunks[0].Length);

         Assert.AreEqual(11, chunks[1].StartPos);
         Assert.AreEqual(9, chunks[1].Length);  // although + space

         Assert.AreEqual(20, chunks[2].StartPos);
         Assert.AreEqual(4, chunks[2].Length);  // not + space

         Assert.AreEqual(24, chunks[3].StartPos);
         Assert.AreEqual(10, chunks[3].Length);  // excellent.

         Assert.AreEqual(35, chunks[4].StartPos);
         Assert.AreEqual(7, chunks[4].Length);  // I aim,

         Assert.AreEqual(42, chunks[5].StartPos);
         Assert.AreEqual(10, chunks[5].Length);  // sometimes,

         Assert.AreEqual(53, chunks[6].StartPos);
         Assert.AreEqual(9, chunks[6].Length);  // to excel.

         for (int ndx = 0; ndx < chunks.Count; ndx++) {
            Assert.IsFalse(chunks[ndx].HyphenNeeded);
         }
      }

      [TestMethod]
      public void ExactLineSizeYieldsOneBlock() {
         var str = "ABCDE";
         var chunks = MHString.WrapText(str, 5).ToList();
         Assert.AreEqual(1, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(str.Length, chunks[0].Length);
         Assert.IsFalse(chunks[0].HyphenNeeded);
      }

      [TestMethod]
      public void ShortLineWorks() {
         var str = "ABCDEFGHIJKLMNOPQ";
         var chunks = MHString.WrapText(str, 50).ToList();
         Assert.AreEqual(1, chunks.Count);

         Assert.AreEqual(0, chunks[0].StartPos);
         Assert.AreEqual(str.Length, chunks[0].Length);
         Assert.IsFalse(chunks[0].HyphenNeeded);
      }

      [TestMethod]
      public void EmptyTextYieldsZeroBlocks() {
         var chunks = MHString.WrapText(string.Empty, 50).ToList();
         Assert.AreEqual(0, chunks.Count);
      }

      [TestMethod]
      public void NullTextYieldsZeroBlocks() {
         var chunks = MHString.WrapText(null, 50).ToList();
         Assert.AreEqual(0, chunks.Count);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void ZeroMaxLengthThrowsException() {
         var chunks = MHString.WrapText("This is a test", 0).ToList();
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void NegativeMaxLengthThrowsException() {
         var chunks = MHString.WrapText("This is a test", -1).ToList();
      }

      [TestMethod]
      public void GetWrappedTextRawWorks() {
         var str = "This is a test of the emergency broadcast";
         var chunks = MHString.WrapText(str, 25, 15).ToList();
         Assert.AreEqual(2, chunks.Count);
         var txtLines = MHString.GetWrappedText(chunks, str, false).ToList();
         Assert.AreEqual(chunks.Count, txtLines.Count);
         Assert.AreEqual("This is a test of the ", txtLines[0]);
         Assert.AreEqual("emergency broadcast", txtLines[1]);
      }
      
      [TestMethod]
      public void GetWrappedTextRawWithEmptyWorks() {
         var str = "";
         var chunks = MHString.WrapText(str, 25, 15).ToList();
         Assert.AreEqual(0, chunks.Count);
         var txtLines = MHString.GetWrappedText(chunks, str, false).ToList();
         Assert.AreEqual(chunks.Count, txtLines.Count);
      }

      [TestMethod]
      public void GetWrappedTextWorks() {
         var str = "This is a test of the emergency broadcast";
         var txtLines = MHString.GetWrappedText(str, 25, 15, false).ToList();
         Assert.AreEqual(2, txtLines.Count);
         Assert.AreEqual("This is a test of the ", txtLines[0]);
         Assert.AreEqual("emergency broadcast", txtLines[1]);
      }

      [TestMethod]
      public void GetWrappedTextWithEmptyWorks() {
         var txtLines = MHString.GetWrappedText("", 25, 15, false).ToList();
         Assert.AreEqual(0, txtLines.Count);
      }

   }
}
