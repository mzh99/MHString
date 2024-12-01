using System;
using OCSS.StringUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MHStringTest {
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
