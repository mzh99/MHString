using System;
using System.Collections.Generic;

namespace OCSS.StringUtil {

   /// <summary>CSV functions included in MHString class</summary>
   public static partial class MHString {
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
}
