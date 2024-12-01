using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCSS.StringUtil;

namespace MHStringTest {
   
   [TestClass]
   public class ParseIPTests {

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

}
