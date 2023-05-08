namespace Midi.Extensions;

internal static class ByteExtensions {
	public static byte GetHighNibble(this byte x) {
		return (byte)((x >> 4) & 0xF);
	}

	public static byte GetLowNibble(this byte x) {
		return (byte)(x & 0xF);
	}
}
