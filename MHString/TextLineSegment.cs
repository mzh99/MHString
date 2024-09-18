using System;

namespace OCSS.StringUtil {

   public struct TextLineSegment: IEquatable<TextLineSegment> {
      public int StartPos { get; set; }
      public int Length { get; set; }
      public bool HyphenNeeded { get; set; }

      public TextLineSegment(int startPos, int len, bool hyphen) {
         StartPos = startPos;
         Length = len;
         HyphenNeeded = hyphen;
      }

      public override bool Equals(object obj) {
         return obj is TextLineSegment segment && Equals(segment);
      }

      public bool Equals(TextLineSegment other) {
         return StartPos == other.StartPos && Length == other.Length && HyphenNeeded == other.HyphenNeeded;
      }

      public override int GetHashCode() {
         int hashCode = -1658384116;
         hashCode = hashCode * -1521134295 + StartPos.GetHashCode();
         hashCode = hashCode * -1521134295 + Length.GetHashCode();
         hashCode = hashCode * -1521134295 + HyphenNeeded.GetHashCode();
         return hashCode;
      }

      public static bool operator ==(TextLineSegment left, TextLineSegment right) {
         return left.Equals(right);
      }

      public static bool operator !=(TextLineSegment left, TextLineSegment right) {
         return !(left == right);
      }

   }

}
