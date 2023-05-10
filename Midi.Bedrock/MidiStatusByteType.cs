namespace Midi.Bedrock;

internal enum MidiStatusByteType {
	Min = 0x80,
	NoteOff = 0x80,
	NoteOn = 0x90,
	PolyphonicKeyPressure = 0xA0,
	ControlChange = 0xB0,
	ProgramChange = 0xC0,
	ChannelPressure = 0xD0,
	PitchBendChange = 0xE0,
	SystemExclusive = 0xF0,
	SystemCommon = 0xF0,
	SystemRealTime = 0xF8,
	Max = 0xFF,
}
