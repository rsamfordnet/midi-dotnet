using System.Buffers;

namespace Midi.Messages;

public record MidiMessage(ReadOnlySequence<byte> RawBytes) {
	public static readonly MidiMessage Empty = new(ReadOnlySequence<byte>.Empty);
	public static readonly MidiMessage Heartbeat = new(
		RawBytes: new(new byte[] { 0xF0, 0x43, 0x10, 0x3E, 0x19, 0x7F })
	);
}
