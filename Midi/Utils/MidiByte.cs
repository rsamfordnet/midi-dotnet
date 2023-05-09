namespace Midi.Utils;

public static class MidiByte {
	public static readonly Range DataRange = new(0x00, 0x7F);
	public static readonly Range StatusRange = new(0x80, 0xFF);
	public static readonly Range RealTimeRange = new(0xF8, 0xFF);
	public static readonly Range SystemCommonRange = new(0xF0, 0xF7);

	public static bool IsDataByte(byte midiByte) {
		return DataRange.Contains(midiByte);
	}

	public static bool IsStatusByte(byte midiByte) {
		return StatusRange.Contains(midiByte);
	}

	public static bool IsSystemRealTimeByte(byte midiByte) {
		return RealTimeRange.Contains(midiByte);
	}

	public static bool IsSystemCommonByte(byte midiByte) {
		return SystemCommonRange.Contains(midiByte);
	}

	public static bool IsSystemExclusiveStart(byte midiByte) {
		return midiByte == 0xF0;
	}

	public static bool IsSystemExclusiveEnd(byte midiByte) {
		return midiByte == 0xF7;
	}

	public static bool IsProgramChange(byte midiByte) {
		return (midiByte & 0xF0) == 0xC0;
	}

	public static bool IsChannelPressure(byte midiByte) {
		return (midiByte & 0xF0) == 0xD0;
	}
}
