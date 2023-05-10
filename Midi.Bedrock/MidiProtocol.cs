namespace Midi.Bedrock;

public class MidiProtocol : IMessageReader<MidiMessage>, IMessageWriter<MidiMessage> {
	public bool TryParseMessage(
		in ReadOnlySequence<byte> input,
		ref SequencePosition consumed,
		ref SequencePosition examined,
		[UnscopedRef] out MidiMessage message
	) {
		var reader = new SequenceReader<byte>(input);
		message = MidiMessageBuilder.Empty;

		if (!reader.TryPeek(out var nextByte)) {
			message = MidiMessageBuilder.Empty;
			consumed = input.Start;
			examined = input.End;
			return false;
		}

		if (MidiByte.IsSystemRealTimeByte(nextByte)) {
			message = HandleSystemRealTimeMessage(reader);
		} else if (MidiByte.IsStatusByte(nextByte)) {
			message = HandleStatusMessage(reader);
		} else if (MidiByte.IsSystemCommonByte(nextByte)) {
			message = HandleSystemCommonMessage(reader);
		}

		// At this point the only remaining possibility is that the byte is a data byte
		// If it isn't, then we have an invalid message. Per the MIDI spec, you should skip
		// any bytes that aren't valid data bytes.
		if (!MidiByte.IsDataByte(nextByte)) {
			//TODO: Log a warning
			consumed = input.GetPosition(1);
			examined = input.GetPosition(1);
			return false;
		}

		if (MidiByte.IsProgramChange(nextByte)) {
			message = HandleProgramChangeMessage(reader);
		} else if (MidiByte.IsChannelPressure(nextByte)) {
			message = HandleChannelPressureMessage(reader);
		}

		consumed = input.GetPosition(message.Payload.Length);
		examined = input.GetPosition(reader.Consumed);
		return true;
	}

	public void WriteMessage(MidiMessage message, IBufferWriter<byte> output) {
		output.Write(message.Payload.Span);
	}

	private static MidiSysRealtimeMessage HandleSystemRealTimeMessage(SequenceReader<byte> reader) {
		return !reader.TryRead(out var statusByte)
			? throw new InvalidOperationException("Expected a status byte, but none was found.")
			: new MidiSysRealtimeMessage {
				Status = statusByte,
				Payload = Array.Empty<byte>(),
			};
	}

	private static MidiMessage HandleStatusMessage(SequenceReader<byte> reader) {
		// if (!reader.TryRead(out var statusByte)) {
		// 	throw new InvalidOperationException("Expected a status byte, but none was found.");
		// }

		// if (!MidiStatusByte.TryGet(statusByte, out _)) {
		// }

		throw new NotImplementedException();
	}

	private MidiMessage HandleSystemCommonMessage(SequenceReader<byte> reader) {
		// if (!reader.TryReadToAny(
		// 	sequence: out var line,
		// 	delimiters: MidiStatusByte.AllStatusBytes,
		// 	advancePastDelimiter: false
		// )) {
		// 	message = MidiMessageBuilder.Empty;
		// 	consumed = input.Start;
		// 	examined = input.End;
		// 	return false;
		// }

		// message = new RawMidiMessage {
		// 	DataBytes = line.ToArray()
		// };
		// consumed = input.GetPosition(line.Length);
		// examined = input.GetPosition(reader.Consumed);
		// return true;

		throw new NotImplementedException();
	}

	private MidiMessage HandleProgramChangeMessage(SequenceReader<byte> reader) {
		throw new NotImplementedException();
	}

	private MidiMessage HandleChannelPressureMessage(SequenceReader<byte> reader) {
		throw new NotImplementedException();
	}
}
