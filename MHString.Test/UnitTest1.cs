using System;
using System.IO;
using OCSS.StringUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MHStringTest {

   [TestClass]
   public class MHStringTest {

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
      public void ParseIPv4IsSuccessful() {
         Assert.AreEqual("1.1.1.1", MHString.ParseIPv4("1.1.1.1"), "IP Address Parse failed");
      }

      [TestMethod]
      public void ParseIPv4WithLeadingZerosIsSuccessful() {
         Assert.AreEqual("1.1.1.1", MHString.ParseIPv4("01.01.01.01"), "IP Address Parse with leading zeros failed");
      }

      [TestMethod]
      [ExpectedException(typeof(System.FormatException))]
      public void ParseIPv4WithTooFewSegmentsRaisesException() {
         MHString.ParseIPv4("1.1.1");
      }

      [TestMethod]
      [ExpectedException(typeof(System.FormatException))]
      public void ParseIPv4WithTooManySegmentsRaisesException() {
         MHString.ParseIPv4("1.1.1.1.1");
      }

      [TestMethod]
      [ExpectedException(typeof(System.FormatException))]
      public void ParseIPv4WithNullIPRaisesException() {
         MHString.ParseIPv4(null);
      }

      [TestMethod]
      [ExpectedException(typeof(System.FormatException))]
      public void ParseIPv4WithEmptyIPRaisesException() {
         MHString.ParseIPv4(string.Empty);
      }

      [TestMethod]
      [ExpectedException(typeof(System.FormatException))]
      public void ParseIPv4WithWhitespaceRaisesException() {
         MHString.ParseIPv4("   ");
      }

      [TestMethod]
      [ExpectedException(typeof(System.FormatException))]
      public void ParseIPv4WithInvalidSegmentRaisesException() {
         MHString.ParseIPv4("1.1.1.A");
      }

      [TestMethod]
      [ExpectedException(typeof(System.FormatException))]
      public void ParseIPv4WithOutOfRangeSegmentRaisesException() {
         MHString.ParseIPv4("1.1.256.1");
      }

      [TestMethod]
      public void TryParseIPv4IsSuccessful() {
         string outputIP = string.Empty;
         bool success;

         success = MHString.TryParseIPv4("1.1.1.1", out outputIP);
         Assert.IsTrue(success, "TryParse failed with 1.1.1.1");
         Assert.AreEqual("1.1.1.1", outputIP, "normalized ip not 1.1.1.1");

         success = MHString.TryParseIPv4("01.01.01.01", out outputIP);
         Assert.IsTrue(success, "TryParse failed with leading zeros");
         Assert.AreEqual("1.1.1.1", outputIP, "normalized ip not 1.1.1.1");

         success = MHString.TryParseIPv4("1.1.1", out outputIP);
         Assert.IsFalse(success, "TryParse succeeded with 1.1.1");
         Assert.AreEqual(string.Empty, outputIP, "normalized ip not empty with 1.1.1");

         success = MHString.TryParseIPv4("1.1.1.1.1", out outputIP);
         Assert.IsFalse(success, "TryParse succeeded with 1.1.1.1.1");
         Assert.AreEqual(string.Empty, outputIP, "normalized ip not empty with 1.1.1.1.1");

         success = MHString.TryParseIPv4(null, out outputIP);
         Assert.IsFalse(success, "TryParse succeeded with null");
         Assert.AreEqual(string.Empty, outputIP, "normalized ip not empty with null");

         success = MHString.TryParseIPv4(string.Empty, out outputIP);
         Assert.IsFalse(success, "TryParse succeeded with empty");
         Assert.AreEqual(string.Empty, outputIP, "normalized ip not empty with empty");

         success = MHString.TryParseIPv4("   ", out outputIP);
         Assert.IsFalse(success, "TryParse succeeded with whitespace");
         Assert.AreEqual(string.Empty, outputIP, "normalized ip not empty with whitespace");

         success = MHString.TryParseIPv4("1.1.1.A", out outputIP);
         Assert.IsFalse(success, "TryParse succeeded with 1.1.1.A");
         Assert.AreEqual(string.Empty, outputIP, "normalized ip not empty with 1.1.1.A");

         success = MHString.TryParseIPv4("1.1.256.1", out outputIP);
         Assert.IsFalse(success, "TryParse succeeded with 1.1.256.1");
         Assert.AreEqual(string.Empty, outputIP, "normalized ip not empty with 1.1.256.1");

      }


   }

   [TestClass]
   public class CSVTest {

      const string TESTSTR1 = "field1,field2,3,,5";
      const string TESTSTR2 = " field1   ,  field2  ,3,  ,5  ";
      const string TESTSTR3 = ",|2|,|3|,";
      const string TESTSTR4 = ",";
      const string TESTSTR5 = "||";
      const string TESTSTR6 = "||,";
      const string TESTSTR7 = ",||,||";
      const string TESTSTR8 = ",||,";
      const string TESTSTR9 = "|Field1|,|Field2|,|Field3|";
      const string TESTSTR10 = "    |Field1|   ,    |Field2|   ,  |Field3|  ";
      const string TESTSTR11 = "    |   Field1   |   ,    |   Field2 |   ,  |  Field3  |  ";
      const string TESTSTR12 = "  Field1   ";
      const string TESTSTR13 = "  Field1   ,123456789   ";
      const string TESTSTR14 = "|my||pipes|,|are|,|strong|";
      const string TESTSTR15 = "   |my||pipes|   ,   |are| ,  |strong| ";
      const string TESTSTR16 = "   |   my||pipes  |   ,   |  are | ,  |   strong  | ";
      const string TESTSTR17 = "|my||||pipes|,|are|,|strong|";

      const string TESTSTR_BAD1 = "|1|,|2|,|";

      [TestMethod]
      public void SplitWithDoubleEnclosureNoTrimSucceeds() {
         var opts = new CSVParseOpts { TrimOutside = false, AreFieldsEnclosed = true, EnclosureDelimiter = '|', TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR14, opts);
         Assert.AreEqual(2, output.GetUpperBound(0));
         Assert.AreEqual(3, output.Length);
         Assert.AreEqual("my|pipes", output[0]);
         Assert.AreEqual("are", output[1]);
         Assert.AreEqual("strong", output[2]);
      }
      [TestMethod]
      public void SplitWithDoubleDoubleEnclosureNoTrimSucceeds() {
         var opts = new CSVParseOpts { TrimOutside = false, AreFieldsEnclosed = true, EnclosureDelimiter = '|', TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR17, opts);
         Assert.AreEqual(2, output.GetUpperBound(0));
         Assert.AreEqual(3, output.Length);
         Assert.AreEqual("my||pipes", output[0]);
         Assert.AreEqual("are", output[1]);
         Assert.AreEqual("strong", output[2]);
      }

      [TestMethod]
      public void SplitWithDoubleEnclosureWithTrimOutsideSucceeds() {
         var opts = new CSVParseOpts { TrimOutside = true, AreFieldsEnclosed = true, EnclosureDelimiter = '|', TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR15, opts);
         Assert.AreEqual(2, output.GetUpperBound(0));
         Assert.AreEqual(3, output.Length);
         Assert.AreEqual("my|pipes", output[0]);
         Assert.AreEqual("are", output[1]);
         Assert.AreEqual("strong", output[2]);
      }

      [TestMethod]
      public void SplitWithDoubleEnclosureWithFullTrimSucceeds() {
         var opts = new CSVParseOpts { TrimOutside = true, AreFieldsEnclosed = true, EnclosureDelimiter = '|', TrimInEnclosure = true };
         string[] output = MHString.CSVSplit(TESTSTR16, opts);
         Assert.AreEqual(2, output.GetUpperBound(0));
         Assert.AreEqual(3, output.Length);
         Assert.AreEqual("my|pipes", output[0]);
         Assert.AreEqual("are", output[1]);
         Assert.AreEqual("strong", output[2]);
      }

      [TestMethod]
      public void SplitNoEnclosureNoTrimSucceeds() {
         var opts = new CSVParseOpts { AreFieldsEnclosed = false, TrimOutside = false };
         string[] output = MHString.CSVSplit(TESTSTR1, opts);
         Assert.AreEqual(4, output.GetUpperBound(0));
         Assert.AreEqual(5, output.Length);
         Assert.AreEqual("field1", output[0]);
         Assert.AreEqual("3", output[2]);
         Assert.AreEqual(string.Empty, output[3]);
         Assert.AreEqual("5", output[4]);
      }

      [TestMethod]
      public void SplitNoEnclosureWithTrimSucceeds() {
         var opts = new CSVParseOpts { TrimOutside = true, AreFieldsEnclosed = false };
         string[] output = MHString.CSVSplit(TESTSTR2, opts);
         Assert.AreEqual(4, output.GetUpperBound(0));
         Assert.AreEqual(5, output.Length);
         Assert.AreEqual("field1", output[0]);
         Assert.AreEqual("field2", output[1]);
         Assert.AreEqual("3", output[2]);
         Assert.AreEqual(string.Empty, output[3]);
         Assert.AreEqual("5", output[4]);
      }

      [TestMethod]
      public void SplitNoEnclosureWithTrimWithEmptySucceeds() {
         var opts = new CSVParseOpts { AreFieldsEnclosed = false, TrimOutside = true };
         opts.AreFieldsEnclosed = false;
         string[] output = MHString.CSVSplit(string.Empty, opts);
         Assert.AreEqual(0, output.Length, "Length of array not zero");
         Assert.AreEqual(-1, output.GetUpperBound(0), "Number of array elements not zero");
      }

      [TestMethod]
      public void SplitWithEnclosureNoTrimSucceeds() {
         var opts = new CSVParseOpts { EnclosureDelimiter = '|', AreFieldsEnclosed = true, TrimOutside = false, TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR3, opts);
         Assert.AreEqual(4, output.Length);
         Assert.AreEqual(3, output.GetUpperBound(0));
         Assert.AreEqual(String.Empty, output[0]);
         Assert.AreEqual("2", output[1]);
         Assert.AreEqual("3", output[2]);
         Assert.AreEqual(string.Empty, output[3]);
      }

      [TestMethod]
      public void SplitWithEnclosureNoTrimAndTwoEmptyFieldsSucceeds() {
         var opts = new CSVParseOpts { EnclosureDelimiter = '|', AreFieldsEnclosed = true, TrimOutside = false, TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR4, opts);
         Assert.AreEqual(2, output.Length);
         Assert.AreEqual(1, output.GetUpperBound(0));
      }

      [TestMethod]
      public void SplitWithEnclosureNoTrimAndOneEmptyFieldSucceeds() {
         var opts = new CSVParseOpts { EnclosureDelimiter = '|', AreFieldsEnclosed = true, TrimOutside = false,TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR5, opts);
         Assert.AreEqual(1, output.Length);
         Assert.AreEqual(0, output.GetUpperBound(0));
         Assert.AreEqual(string.Empty, output[0]);
      }

      [TestMethod]
      public void SplitWithEnclosureNoTrimAndEmptySucceeds() {
         var opts = new CSVParseOpts { EnclosureDelimiter = '|', AreFieldsEnclosed = true, TrimOutside = false, TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(string.Empty, opts);
         Assert.AreEqual(0, output.Length);
         Assert.AreEqual(-1, output.GetUpperBound(0));
      }

      [TestMethod]
      [ExpectedException(typeof(System.FormatException))]
      public void SplitWithImbalancedReturnsException() {
         var opts = new CSVParseOpts { EnclosureDelimiter = '|', AreFieldsEnclosed = true, TrimOutside = false, TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR_BAD1, opts);
      }

      [TestMethod]
      public void SplitWithEnclosureNoTrimAndTwoEmptyFields2Succeeds() {
         var opts = new CSVParseOpts { EnclosureDelimiter = '|', AreFieldsEnclosed = true, TrimOutside = false, TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR6, opts);
         Assert.AreEqual(2, output.Length);
         Assert.AreEqual(1, output.GetUpperBound(0));
         Assert.AreEqual(string.Empty, output[0]);
         Assert.AreEqual(string.Empty, output[1]);
      }

      [TestMethod]
      public void SplitWithEnclosureNoTrimAndThreeEmptyFieldsSucceeds() {
         var opts = new CSVParseOpts { EnclosureDelimiter = '|', AreFieldsEnclosed = true, TrimOutside = false,TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR7, opts);
         Assert.AreEqual(3, output.Length);
         Assert.AreEqual(2, output.GetUpperBound(0));
         Assert.AreEqual(string.Empty, output[0]);
         Assert.AreEqual(string.Empty, output[1]);
         Assert.AreEqual(string.Empty, output[2]);
      }

      [TestMethod]
      public void SplitWithEnclosureNoTrimAndThreeEmptyFields2Succeeds() {
         var opts = new CSVParseOpts { EnclosureDelimiter = '|', AreFieldsEnclosed = true, TrimOutside = false,TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR8, opts);
         Assert.AreEqual(3, output.Length);
         Assert.AreEqual(2, output.GetUpperBound(0));
      }

      [TestMethod]
      public void SplitWithEnclosureNoTrimAndThreeFieldsSucceeds() {
         var opts = new CSVParseOpts { EnclosureDelimiter = '|', AreFieldsEnclosed = true, TrimOutside = false, TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR9, opts);
         Assert.AreEqual(3, output.Length);
         Assert.AreEqual(2, output.GetUpperBound(0));
         Assert.AreEqual("Field1", output[0]);
         Assert.AreEqual("Field2", output[1]);
         Assert.AreEqual("Field3", output[2]);
      }

      [TestMethod]
      public void SplitWithEnclosureTrimOutsideAndThreeFieldsSucceeds() {
         var opts = new CSVParseOpts { EnclosureDelimiter = '|', AreFieldsEnclosed = true, TrimOutside = true, TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR10, opts);
         Assert.AreEqual(3, output.Length);
         Assert.AreEqual(2, output.GetUpperBound(0));
         Assert.AreEqual("Field1", output[0]);
         Assert.AreEqual("Field2", output[1]);
         Assert.AreEqual("Field3", output[2]);
      }

      [TestMethod]
      public void SplitWithEnclosureTrimOutsideAndInsideAndThreeFieldsSucceeds() {
         var opts = new CSVParseOpts { EnclosureDelimiter = '|', AreFieldsEnclosed = true, TrimOutside = true, TrimInEnclosure = true };
         string[] output = MHString.CSVSplit(TESTSTR11, opts);
         Assert.AreEqual(3, output.Length);
         Assert.AreEqual(2, output.GetUpperBound(0));
         Assert.AreEqual("Field1", output[0]);
         Assert.AreEqual("Field2", output[1]);
         Assert.AreEqual("Field3", output[2]);
      }

      [TestMethod]
      public void SplitNoEnclosureTrimOutsideAndOneFieldSucceeds() {
         var opts = new CSVParseOpts { EnclosureDelimiter = '|', AreFieldsEnclosed = false, TrimOutside = true, TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR12, opts);
         Assert.AreEqual(1, output.Length);
         Assert.AreEqual(0, output.GetUpperBound(0));
         Assert.AreEqual("Field1", output[0]);
      }

      [TestMethod]
      public void SplitNoEnclosureTrimOutsideAndTwoFieldsSucceeds() {
         var opts = new CSVParseOpts { EnclosureDelimiter = '|', AreFieldsEnclosed = false, TrimOutside = true, TrimInEnclosure = false };
         string[] output = MHString.CSVSplit(TESTSTR13, opts);
         Assert.AreEqual(2, output.Length);
         Assert.AreEqual(1, output.GetUpperBound(0));
         Assert.AreEqual("Field1", output[0]);
         Assert.AreEqual("123456789", output[1]);
      }


   }
}
