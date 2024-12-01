namespace OCSS.StringUtil {

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
