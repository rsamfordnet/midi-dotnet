namespace Midi.Bedrock;

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using global::Bedrock.Framework.Protocols;
using Midi.Messages;

public class MidiProtocol : IMessageReader<MidiMessage>, IMessageWriter<MidiMessage> {
	public bool TryParseMessage(
		in ReadOnlySequence<byte> input,
		ref SequencePosition consumed,
		ref SequencePosition examined,
		[UnscopedRef] out MidiMessage message
	) {
		var reader = new SequenceReader<byte>(input);

		if (reader.TryPeek(out var val)) {
			if (!Midi.MidiStatusByte.IsStatusByte(val)) {

			}
		}

		if (!reader.TryReadToAny(
			sequence: out var line,
			delimiters: Midi.MidiStatusByte.AllStatusBytes,
			advancePastDelimiter: false
		)) {
			message = MidiMessageBuilder.Empty;
			consumed = input.Start;
			examined = input.End;
			return false;
		}

		message = new RawMidiMessage {
			DataBytes = line.ToArray()
		};
		consumed = input.GetPosition(line.Length);
		examined = input.GetPosition(reader.Consumed);
		return true;
	}

	public void WriteMessage(MidiMessage message, IBufferWriter<byte> output) {
		if (message is not RawMidiMessage rawMessage) {
			throw new ArgumentException($"Message must be of type {nameof(RawMidiMessage)}", nameof(message));
		}

		output.Write(rawMessage.DataBytes);
	}
}
