namespace Midi.Tests;

using FluentAssertions;

[TestClass]
public class ByteUtilsTests {
	[DataTestMethod]
	[DataRow((byte)0b1000_0000, (byte)0b0000_1000)]
	[DataRow((byte)0b0100_0000, (byte)0b0000_0100)]
	[DataRow((byte)0b0010_0000, (byte)0b0000_0010)]
	[DataRow((byte)0b0001_0000, (byte)0b0000_0001)]
	[DataRow((byte)0b0000_1000, (byte)0b0000_0000)]
	[DataRow((byte)0b0000_0100, (byte)0b0000_0000)]
	[DataRow((byte)0b0000_0010, (byte)0b0000_0000)]
	[DataRow((byte)0b0000_0001, (byte)0b0000_0000)]
	public void GetHighNibble_should_return_the_first_4_bits_as_byte(byte value, byte expected) {
		value.GetHighNibble().Should().Be(expected);
	}

	[DataTestMethod]
	[DataRow((byte)0b1000_0000, (byte)0b0000_0000)]
	[DataRow((byte)0b0100_0000, (byte)0b0000_0000)]
	[DataRow((byte)0b0010_0000, (byte)0b0000_0000)]
	[DataRow((byte)0b0001_0000, (byte)0b0000_0000)]
	[DataRow((byte)0b0000_1000, (byte)0b0000_1000)]
	[DataRow((byte)0b0000_0100, (byte)0b0000_0100)]
	[DataRow((byte)0b0000_0010, (byte)0b0000_0010)]
	[DataRow((byte)0b0000_0001, (byte)0b0000_0001)]
	public void GetLowNibble_should_return_the_last_4_bits_as_byte(byte value, byte expected) {
		value.GetLowNibble().Should().Be(expected);
	}
}
