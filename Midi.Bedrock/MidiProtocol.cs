namespace Midi.Bedrock;

/// <summary>
/// Types of MIDI bytes:
/// - Status byte: The first byte of a MIDI message. The most significant bit is always 1.
/// - Data byte: The second and third bytes of a MIDI message. The most significant bit is always 0.
///
/// Types of MIDI messages:
/// - Channel message: A MIDI message that is sent to a specific channel.
/// - System common message: A MIDI message that is sent to all channels.
/// - System real-time message:
/// 		A MIDI message that is sent to all channels. These messages are not queued and are also
/// 		not affected by the MIDI clock. They are sent immediately.
/// - System exclusive message: A MIDI message that is sent to all channels.
///
/// Structure of a MIDI message:
/// - Status byte: The first byte of a MIDI message. The most significant bit is always 1.
/// - Data byte: The second and third bytes of a MIDI message. The most significant bit is always 0.
///
/// Structure of a channel message:
/// - Status byte: The first byte of a MIDI message. The most significant bit is always 1.
/// - Data byte: The second and third bytes of a MIDI message. The most significant bit is always 0.
///
/// Structure of a system common message:
/// - Status byte: The first byte of a MIDI message. The most significant bit is always 1.
/// - Data byte: The second and third bytes of a MIDI message. The most significant bit is always 0.
///
/// Structure of a system real-time message:
/// - Status byte: The first byte of a MIDI message. The most significant bit is always 1.
/// - Data byte: The second and third bytes of a MIDI message. The most significant bit is always 0.
///
/// Structure of a system exclusive message:
/// - Status byte: The first byte of a MIDI message. The most significant bit is always 1.
/// - Data byte: The second and third bytes of a MIDI message. The most significant bit is always 0.
/// </summary>
internal class MidiProtocol : IMessageReader<MidiMessage>, IMessageWriter<MidiMessage> {
	public bool TryParseMessage(
		in ReadOnlySequence<byte> input,
		ref SequencePosition consumed,
		ref SequencePosition examined,
		out MidiMessage message) {
		message = MidiMessage.Empty;

		if (input.Length < 1) {
			return false;
		}

		var reader = new SequenceReader<byte>(input);

		if (!reader.TryRead(out var firstByte) || firstByte < (int)MidiStatusByteType.Max) {
			return false;
		}

		var statusByte = firstByte;
		// TODO: Replace with GetHighNibble() extension method.
		var statusByteType = (MidiStatusByteType)(statusByte & 0xF0);

		switch (statusByteType) {
			//FIXME: I don't think all these are 3 bytes long. I think some are 2 bytes long.
			case MidiStatusByteType.NoteOff:
			case MidiStatusByteType.NoteOn:
			case MidiStatusByteType.PolyphonicKeyPressure:
			case MidiStatusByteType.ControlChange:
			case MidiStatusByteType.ProgramChange:
			case MidiStatusByteType.ChannelPressure:
			case MidiStatusByteType.PitchBendChange:
				if (input.Length < 3) {
					return false;
				}

				if (!reader.TryRead(out var dataByte1) || !reader.TryRead(out var dataByte2)) {
					return false;
				}

				var channel = statusByte & 0x0F;
				var commandType = statusByte & 0xF0;
				message = new MidiChannelMessage {
					Status = statusByte,
					Command = MidiChannelCommandType.Parse((byte)commandType),
					Channel = (byte)channel,
					DataByte1 = dataByte1,
					DataByte2 = dataByte2
				};
				break;

			case MidiStatusByteType.SystemExclusive:
				if (input.Length < 2) {
					return false;
				}

				var systemData = input.Slice(reader.Position).ToArray();

				message = MidiByte.IsSystemCommonByte(statusByte)
					? new MidiSystemCommonMessage(statusByteType, systemData)
					: new MidiSystemExclusiveMessage(systemData);
				break;

			case MidiStatusByteType.SystemRealTime:
				message = new MidiSystemRealTimeMessage(statusByteType);
				break;
			case MidiStatusByteType.Max:
				break;
			default:
				return false;
		}

		consumed = reader.Position;
		examined = consumed;
		return true;
	}

	public void WriteMessage(MidiMessage message, IBufferWriter<byte> output) {
		throw new NotImplementedException();
	}
}
