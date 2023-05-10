public static class HexUtils {
	public static string ToHexString(this ReadOnlyMemory<byte> bytes, char separator = '-') {
		var sb = new StringBuilder((bytes.Length * 3) - 1);

		for (var i = 0; i < bytes.Length; i++) {
			if (i > 0) {
				_ = sb.Append(separator);
			}
			_ = sb.Append(bytes.Span[i].ToString("X2"));
		}

		return sb.ToString();
	}

	public static string ToHexString(this ReadOnlySpan<byte> bytes, char separator = '-') {
		var sb = new StringBuilder((bytes.Length * 3) - 1);

		for (var i = 0; i < bytes.Length; i++) {
			if (i > 0) {
				_ = sb.Append(separator);
			}
			_ = sb.Append(bytes[i].ToString("X2"));
		}

		return sb.ToString();
	}

	internal static string ToHexStringFast(this ReadOnlySpan<byte> bytes, char separator = '-') {
		var outputLength = (bytes.Length * 3) - 1;
		var chars = outputLength < 1024 ? stackalloc char[outputLength] : new char[outputLength];

		for (int i = 0, j = 0; i < bytes.Length; i++, j += 3) {
			if (i > 0) {
				chars[j - 1] = separator;
			}

			WriteByteAsHexFast(chars[j..], bytes[i]);
		}

		return new string(chars);
	}

	private static void WriteByteAsHexFast(Span<char> buffer, byte value) {
		if (buffer.Length < 2) {
			throw new ArgumentException("Insufficient buffer size.", nameof(buffer));
		}

		var low = (byte)(value & 0x0F);
		var high = (byte)((value & 0xF0) >> 4);

		buffer[0] = (char)(high < 10 ? high + '0' : high - 10 + 'A');
		buffer[1] = (char)(low < 10 ? low + '0' : low - 10 + 'A');
	}
}
