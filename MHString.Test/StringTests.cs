using System;
using System.IO;
using OCSS.StringUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MHStringTest {

   [TestClass]
   public class StringTests {

      [TestMethod]
      public void ByteToHexWorks() {
         Assert.AreEqual("1020ff", MHString.ByteToHex(new byte[] { 16, 32, 255 }, false), "t1");
         Assert.AreEqual(string.Empty, MHString.ByteToHex(new byte[] { }, false), "t2");
         Assert.AreEqual("0001FF", MHString.ByteToHex(new byte[] { 0, 1, 255 }, true), "t3");
      }

      [TestMethod]
      public void CharCountWorks() {
         Assert.AreEqual(2, MHString.CharCount("1,2,3", ','), "t1");
         Assert.AreEqual(0, MHString.CharCount("123", ','), "t2");
         Assert.AreEqual(3, MHString.CharCount("13933838, 3030337300338, 3 ,23", ','), "t3");
         Assert.AreEqual(0, MHString.CharCount("", ','), "t4");
         Assert.AreEqual(3, MHString.CharCount("1,,,99", ','), "t5");
      }

      [TestMethod]
      public void CharCleanFileNameWorks() {
         string IllegalNameSet = new string(Path.GetInvalidFileNameChars());

         Assert.AreEqual(string.Empty, MHString.CleanFileName(@"/\/\/"), "t1");
         Assert.AreEqual(".", MHString.CleanFileName(@"\\\.\\\"), "t2");
         Assert.AreEqual("test.txt", MHString.CleanFileName("test.txt"), "t3");
         Assert.AreEqual("ctest.txt", MHString.CleanFileName(@"c:\test.txt"), "t4");
         Assert.AreEqual("test.txt", MHString.CleanFileName(@"\\\\tes\\t.txt"), "t5");
         Assert.AreEqual("test.txt", MHString.CleanFileName(@"test::.txt"), "t6");
         Assert.AreEqual("test.txt", MHString.CleanFileName(@"/test.txt"), "t7");
         Assert.AreEqual("test.txt", MHString.CleanFileName(@"/test.txt"), "t8");
         Assert.AreEqual(string.Empty, MHString.CleanFileName(IllegalNameSet), "t9");
         Assert.AreEqual("abcdef", MHString.CleanFileName("abc" + IllegalNameSet + "def"), "t10");
      }

      [TestMethod]
      public void NumericsOnlyWorks() {
         Assert.AreEqual(string.Empty, MHString.NumericsOnly("abcdef"), "t1");
         Assert.AreEqual("123", MHString.NumericsOnly("abc123"), "t2");
         Assert.AreEqual("123", MHString.NumericsOnly("abcdefghijklmnopqrstuv123"), "t3");
         Assert.AreEqual(string.Empty, MHString.NumericsOnly("adkhfkekhqhbeebfeb*@#&#^#^   dki"), "t4");
      }

      [TestMethod]
      public void NumericsPlusExtraWorks() {
         Assert.AreEqual(string.Empty, MHString.NumericsPlusExtra("abcdef", ".,"), "t1");
         Assert.AreEqual("123", MHString.NumericsPlusExtra("abc123", ".,"), "t2");
         Assert.AreEqual("999,282,282", MHString.NumericsPlusExtra("999,282,282", ".,"), "t3");
         Assert.AreEqual("abc123", MHString.NumericsPlusExtra("abc123", "cba"), "t4");
         Assert.AreEqual("12345678.90", MHString.NumericsPlusExtra("12,345,678.90", "1234567890."), "t5");
      }

      [TestMethod]
      public void ReplaceCCWorks() {
         Assert.AreEqual("abc", MHString.ReplaceControlChars("abc", '.'), "t1");
         string Test = string.Empty;
         for (int z = 0; z < 32; z++)
            Test += Convert.ToChar(z);
         Assert.AreEqual("................................", MHString.ReplaceControlChars(Test, '.'), "t1");
         Assert.AreEqual("................................A", MHString.ReplaceControlChars(Test + "A", '.'), "t2");
      }

      [TestMethod]
      public void StrSegWorks() {

         Assert.AreEqual("123", MHString.StrSeg("123,456,789", 1, ','), "t1");
         Assert.AreEqual("456", MHString.StrSeg("123,456,789", 2, ','), "t2");
         Assert.AreEqual("789", MHString.StrSeg("123,456,789", 3, ','), "t3");
         Assert.AreEqual(string.Empty, MHString.StrSeg("123,456,789", 4, ','), "t4");
         Assert.AreEqual(string.Empty, MHString.StrSeg("123,456,789", 0, ','), "t5");
         Assert.AreEqual(string.Empty, MHString.StrSeg("123,456,789", -1, ','), "t6");
         Assert.AreEqual(string.Empty, MHString.StrSeg(",456,789", 1, ','), "t7");
         Assert.AreEqual(string.Empty, MHString.StrSeg("", 2, ','), "t8");
         Assert.AreEqual("456", MHString.StrSeg(",456,", 2, ','), "t9");
         Assert.AreEqual("456", MHString.StrSeg(",456", 2, ','), "t10");
         Assert.AreEqual("3456",MHString.StrSeg(",,3456", 3, ','), "t11");
         Assert.AreEqual("123", MHString.StrSeg("123", 1, ','), "t12");
         Assert.AreEqual("456", "123,456,789".StrSeg(2, 1, ','), "t13");
      }

      [TestMethod]
      public void StrSegLocateWorks() {
         Assert.AreEqual(0, MHString.StrSegLocate("ABC", "", 1, 1, ',', false), 0, "t1");
         Assert.AreEqual(0, MHString.StrSegLocate("", "123,456,789", 1, 1, ',', false), "t1");
         Assert.AreEqual(2, MHString.StrSegLocate("ABC", "def,abc", 1, 1, ',', false), "t2");
         Assert.AreEqual(2, MHString.StrSegLocate("ABC", "def,abc", 2, 1, ',', false), "t3");
         Assert.AreEqual(0, MHString.StrSegLocate("abc", "def,abc", 3, 1, ',', false), "t4");
         Assert.AreEqual(2, MHString.StrSegLocate("456", "123,456,789", 1, 1, ',', true), "t5");
         Assert.AreEqual(0, MHString.StrSegLocate("xyz", "123,456,789", 1, 1, ',', true), "t6");
         Assert.AreEqual(0, MHString.StrSegLocate("45", "123,456,789", 1, 1, ',', true), "t7");
         Assert.AreEqual(0, MHString.StrSegLocate("456", "123,456,789", 1, 2, ',', true), "t8");   // IncBy is 2, so it should skip the 2nd value during checking
         Assert.AreEqual(3, MHString.StrSegLocate("789", "123,456,789", 1, 1, ',', true), "t9");
         Assert.AreEqual(0, MHString.StrSegLocate("9", "123,456,789", 1, 1, ',', true), "t10");
         Assert.AreEqual(0, MHString.StrSegLocate("1", "123,456,789", 1, 1, ',', true), "t11");
         Assert.AreEqual(0, MHString.StrSegLocate("123", "123,456,789", 2, 1, ',', true), "t12");   // we are starting at segment 2, so no hit
         Assert.AreEqual(2, MHString.StrSegLocate("456", "123,456,789", 2, 1, ',', true), "t13");   // we are starting at segment 2, so it should hit
         Assert.AreEqual(3, MHString.StrSegLocate("789", "123,456,789", 1, 2, ',', true), "t14");
         Assert.AreEqual(2, MHString.StrSegLocate("AB" + "c", "def,abc", 1, 1, ',', false), "t15");
      }

      [TestMethod]
      public void TruncateWorksWithPositiveNumbers() {
         Assert.AreEqual(string.Empty, MHString.Truncate("", 5), string.Empty, "t1");
         Assert.AreEqual("1", MHString.Truncate("123456789", 1), "t2");
         Assert.AreEqual("123456789", MHString.Truncate("123456789", 9), "t3");
         Assert.AreEqual("123456789", MHString.Truncate("123456789", 99), "t4");
         Assert.AreEqual(string.Empty, MHString.Truncate("123456789", 0), "t5");
      }

      [TestMethod]
      [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
      public void TruncateWithNegRaisesException() {
         string test = MHString.Truncate("123456789", -1);
      }

      [TestMethod]
      public void LetterOnlyWorks() {
         Assert.AreEqual(string.Empty, MHString.LetterOnly(""), "t1");
         Assert.AreEqual("ABC", MHString.LetterOnly(@"123ABC#*#  -^"), "t2");
         Assert.AreEqual(string.Empty, MHString.LetterOnly(@"1232&*#^#@(   ~"), "t3");
      }

      [TestMethod]
      public void StrRightWorks() {
         Assert.AreEqual("EFG", MHString.StrRight("ABCDEFG", 3), "t1");
         Assert.AreEqual("ABCDEFG", MHString.StrRight("ABCDEFG", 99), "t2");
         Assert.AreEqual(string.Empty, MHString.StrRight("ABCDEFG", 0), "t3");
         Assert.AreEqual(string.Empty, MHString.StrRight("ABCDEFG", -1), "t4");
      }

      [TestMethod]
      public void SubStrSafeWorks() {
         Assert.AreEqual("A", MHString.SubstringSafe("ABCDEFGH", 0, 1), "t1");
         Assert.AreEqual(string.Empty, MHString.SubstringSafe("ABCDEFGH", -1, 1), "t2");
         Assert.AreEqual(string.Empty, MHString.SubstringSafe("ABCDEFGH", 99, 0), "t3");
         Assert.AreEqual(string.Empty, MHString.SubstringSafe("ABCDEFGH", 99, 99), "t4");
         Assert.AreEqual("ABCDEFGH", MHString.SubstringSafe("ABCDEFGH", 0, 99), "t5");
         Assert.AreEqual(string.Empty, MHString.SubstringSafe("", 0, 99), "t6");
         Assert.AreEqual(string.Empty, MHString.SubstringSafe("", 1, 1), "t7");
      }

      [TestMethod]
      public void UnquoteWorks() {
         char[] SINGLE_ONLY = new char[] { '\'' };
         char[] DOUBLE_ONLY = new char[] { '"' };
         char[] SINGLE_AND_DOUBLE = new char[] { '\'', '"' };
         char[] TRUE_QUOTES = new char[] { '“', '”' };
         // “test”
         Assert.AreEqual("test", MHString.Unquote("'test'", SINGLE_ONLY), "test 1 failed");
         Assert.AreEqual(" 'test' ", MHString.Unquote(" 'test' ", SINGLE_ONLY), "test 2 failed");
         Assert.AreEqual("test", MHString.Unquote("'test'", SINGLE_AND_DOUBLE), "test 3 failed");
         Assert.AreEqual("test", MHString.Unquote("\"test\"", SINGLE_AND_DOUBLE), "test 4 failed");
         Assert.AreEqual("\"test\"", MHString.Unquote("'\"test\"'", SINGLE_AND_DOUBLE), "test 5 failed");
         Assert.AreEqual("test", MHString.Unquote("'test\"", SINGLE_AND_DOUBLE), "test 6 failed");    // mixed quotes
         Assert.AreEqual("'test'", MHString.Unquote("'test'", new char[] { }), "test 7 failed");   // empty quote set
         Assert.AreEqual("test", MHString.Unquote("“test”", TRUE_QUOTES), "test 8 failed");
      }

      [TestMethod]
      [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
      public void SubStrSafeRaisesException() {
         string x = MHString.SubstringSafe("ABCDEFGH", 0, -1);
      }


      [TestMethod]
      public void InsertCharWithTrailingWorks() {
         Assert.AreEqual(string.Empty, MHString.InsertCharEvery(null, 5, ' ', true));
         Assert.AreEqual(string.Empty, MHString.InsertCharEvery(string.Empty, 5, ' ', true));
         Assert.AreEqual("ABC", MHString.InsertCharEvery("ABC", 5, ' ', true));
         Assert.AreEqual("ABC ", MHString.InsertCharEvery("ABC", 3, ' ', true));
         Assert.AreEqual("ABC", MHString.InsertCharEvery("ABC", 4, ' ', true));
         Assert.AreEqual("ABC DE", MHString.InsertCharEvery("ABCDE", 3, ' ', true));
         Assert.AreEqual("ABC DEF ", MHString.InsertCharEvery("ABCDEF", 3, ' ', true));
         Assert.AreEqual("A B C ", MHString.InsertCharEvery("ABC", 1, ' ', true));
         Assert.AreEqual("A ", MHString.InsertCharEvery("A", 1, ' ', true));
      }

      [TestMethod]
      public void InsertCharWithoutTrailingWorks() {
         Assert.AreEqual(string.Empty, MHString.InsertCharEvery(null, 5, ' ', false));
         Assert.AreEqual(string.Empty, MHString.InsertCharEvery(string.Empty, 5, ' ', false));
         Assert.AreEqual("ABC", MHString.InsertCharEvery("ABC", 5, ' ', false));
         Assert.AreEqual("ABC", MHString.InsertCharEvery("ABC", 3, ' ', false));
         Assert.AreEqual("ABC", MHString.InsertCharEvery("ABC", 4, ' ', false));
         Assert.AreEqual("ABC DE", MHString.InsertCharEvery("ABCDE", 3, ' ', false));
         Assert.AreEqual("ABC DEF", MHString.InsertCharEvery("ABCDEF", 3, ' ', false));
         Assert.AreEqual("A B C", MHString.InsertCharEvery("ABC", 1, ' ', false));
         Assert.AreEqual("A", MHString.InsertCharEvery("A", 1, ' ', false));
      }

      [TestMethod]
      public void CenteredWorks() {
         const string data1 = "DATA";

         // edge cases
         // null string passed
         Assert.AreEqual(string.Empty, MHString.CenterJustify(null, 20));
         // negative output width
         Assert.AreEqual(string.Empty, MHString.CenterJustify(data1, -1));
         // zero output width
         Assert.AreEqual(string.Empty, MHString.CenterJustify(data1, 0));
         // empty string passed
         Assert.AreEqual("    ", MHString.CenterJustify(string.Empty, 4));
         // other tests
         Assert.AreEqual("D", MHString.CenterJustify(data1, 1));
         Assert.AreEqual("DA", MHString.CenterJustify(data1, 2));
         Assert.AreEqual("DAT", MHString.CenterJustify(data1, 3));
         Assert.AreEqual("DATA", MHString.CenterJustify(data1, 4));
         Assert.AreEqual("DATA ", MHString.CenterJustify(data1, 5));
         Assert.AreEqual(" DATA ", MHString.CenterJustify(data1, 6));
         Assert.AreEqual(" DATA  ", MHString.CenterJustify(data1, 7));
      }


   }

}
