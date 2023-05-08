using FluentAssertions;

namespace Midi.Tests;

[TestClass]
public class MidiStatusByteTests {
	[TestMethod]
	public void IsStatusByte_returns_false_when_between_0x00_and_0x7F() {
		for (var i = 0x00; i <= 0x7F; i++) {
			MidiStatusByte.IsStatusByte((byte)i).Should().BeFalse();
		}
	}

	[TestMethod]
	public void IsStatusByte_returns_true_when_between_0x80_and_0xFF() {
		for (var i = 0x80; i <= 0xFF; i++) {
			MidiStatusByte.IsStatusByte((byte)i).Should().BeTrue();
		}
	}
}
