namespace Midi.IO;

using System;
using System.Buffers;
using Midi.Utils;

public class MidiReader {
	public void ProcessMidiData(SequenceReader<byte> reader) {
		while (reader.TryPeek(out var nextByte)) {
			if (MidiByte.IsSystemRealTimeByte(nextByte)) {
				HandleSystemRealTimeMessage(reader);
				return;
			}

			if (MidiByte.IsStatusByte(nextByte)) {
				HandleStatusMessage(reader);
				return;
			}

			if (MidiByte.IsSystemCommonByte(nextByte)) {
				HandleSystemCommonMessage(reader);
				return;
			}

			if (!MidiByte.IsDataByte(nextByte)) {
				//TODO: Log a warning
				return;
			}

			if (MidiByte.IsProgramChange(nextByte)) {
				HandleProgramChangeMessage(reader);
				return;
			}

			if (MidiByte.IsChannelPressure(nextByte)) {
				HandleChannelPressureMessage(reader);
				return;
			}
		}
	}

	private void HandleSystemRealTimeMessage(SequenceReader<byte> reader) {
		throw new NotImplementedException();
	}

	private void HandleStatusMessage(SequenceReader<byte> reader) {
		throw new NotImplementedException();
	}

	private void HandleSystemCommonMessage(SequenceReader<byte> reader) {
		throw new NotImplementedException();
	}

	private void HandleProgramChangeMessage(SequenceReader<byte> reader) {
		throw new NotImplementedException();
	}

	private void HandleChannelPressureMessage(SequenceReader<byte> reader) {
		throw new NotImplementedException();
	}
}
