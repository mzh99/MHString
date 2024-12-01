namespace OCSS.StringUtil {
   /// <summary>MHString extension methods</summary>
   public static class MHStringExt {

      public static string StrSeg(this string str, int segNum, int numSegs, char delim) {
         return MHString.StrSeg(str, segNum, numSegs, delim);
      }
      public static string StrSeg(this string str, int segNum, char delim) {
         return MHString.StrSeg(str, segNum, 1, delim);
      }
      public static int StrSegLocate(this string searchStr, string searchIn, int startSeg, int incBy, char delim, bool isCaseSensitive) {
         return MHString.StrSegLocate(searchStr, searchIn, startSeg, incBy, delim, isCaseSensitive);
      }
      public static int CharCount(this string str, char searchChar) {
         return MHString.CharCount(str, searchChar);
      }
      public static string Truncate(this string str, int maxLength) {
         return MHString.Truncate(str, maxLength);
      }
      public static string LetterOnly(this string str) {
         return MHString.LetterOnly(str);
      }
      public static string LetterOrNumOnly(this string str) {
         return MHString.LetterOrNumOnly(str);
      }
      public static string NumericsOnly(this string str) {
         return MHString.NumericsOnly(str);
      }
      public static string NumericsPlusExtra(this string str, string extra) {
         return MHString.NumericsPlusExtra(str, extra);
      }
      public static string FmtPhoneLikePS(this string telephone, string areaCode, string Prefix) {
         return MHString.FmtPhoneLikePS(telephone, areaCode, Prefix);
      }
      public static string ReplaceControlChars(this string str, char replacementChar) {
         return MHString.ReplaceControlChars(str, replacementChar);
      }
      public static string StrRight(this string str, int cnt) {
         return MHString.StrRight(str, cnt);
      }
      public static string LogicalToEnglish(this bool flag) {
         return MHString.TrueFalseToWord(flag);
      }
      public static string CleanFileName(this string fileNameIn) {
         return MHString.CleanFileName(fileNameIn);
      }
      public static string SubstringSafe(this string str, int startPos, int cnt) {
         return MHString.SubstringSafe(str, startPos, cnt);
      }
      public static string HexToBase64(this string hexStr) {
         return MHString.HexToBase64(hexStr);
      }
      public static bool IsValidEmail(this string email) {
         return MHString.IsValidEmail(email);
      }
      public static string UpperFirstLetter(this string str) {
         return MHString.UpperFirstLetter(str);
      }
      public static string InitCaps(this string str) {
         return MHString.InitCaps(str);
      }

      public static string Unquote(this string str, char[] quoteCharSet) {
         return MHString.Unquote(str, quoteCharSet);
      }

      public static string ParseIPv4(this string ip) {
         return MHString.ParseIPv4(ip);
      }

      public static string InsertCharEvery(this string str, int everyNth, char charToInsert, bool includeTrailing = false) {
         return MHString.InsertCharEvery(str, everyNth, charToInsert, includeTrailing);
      }

      public static string CenterJustify(this string val, int outputWidth, char paddingChar = ' ') {
         return MHString.CenterJustify(val, outputWidth, paddingChar);
      }

   }

}
