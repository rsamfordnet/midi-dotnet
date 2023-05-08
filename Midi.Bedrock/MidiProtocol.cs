using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using Bedrock.Framework.Protocols;
using Midi.Messages;

namespace Midi.Bedrock;

public class MidiProtocol : IMessageReader<MidiMessage>, IMessageWriter<MidiMessage> {
	public bool TryParseMessage(
		in ReadOnlySequence<byte> input,
		ref SequencePosition consumed,
		ref SequencePosition examined,
		[UnscopedRef] out MidiMessage message
	) {
		var reader = new SequenceReader<byte>(input);

		if (reader.TryPeek(out var val)) {
			if (!MidiStatusByte.IsStatusByte(val)) {

			}
		}

		if (!reader.TryReadToAny(
			sequence: out var line,
			delimiters: MidiStatusByte.AllStatusBytes,
			advancePastDelimiter: false
		)) {
			message = MidiMessage.Empty;
			consumed = input.Start;
			examined = input.End;
			return false;
		}

		message = new(line);
		consumed = input.GetPosition(line.Length);
		examined = input.GetPosition(reader.Consumed);
		return true;
	}

	public void WriteMessage(MidiMessage message, IBufferWriter<byte> output) {
		foreach (var segment in message.RawBytes) {
			output.Write(segment.Span);
		}

		var memory = output.GetMemory(1);
		memory.Span[0] = MidiStatusByte.SystemExclusiveEnd.Value;
		output.Advance(1);
	}
}
