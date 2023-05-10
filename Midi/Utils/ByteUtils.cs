internal static class ByteUtils {
	public static byte GetHighNibble(this byte x) {
		return (byte)((x >> 4) & 0xF);
	}

	public static byte GetLowNibble(this byte x) {
		return (byte)(x & 0xF);
	}

	public static bool GetBit(this byte value, ByteBit bit) {
		return (value & (1 << (byte)bit)) != 0;
	}
}

internal enum ByteBit : byte {
	Bit1 = 1,
	Bit2 = 2,
	Bit3 = 3,
	Bit4 = 4,
	Bit5 = 5,
	Bit6 = 6,
	Bit7 = 7,
	Bit8 = 8
}
