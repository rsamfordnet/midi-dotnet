internal static class RangeExtensions {
	public static bool Contains(this Range range, Index index) {
		return index.Value >= range.Start.Value && index.Value <= range.End.Value;
	}
}
