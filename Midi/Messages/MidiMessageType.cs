namespace Midi.Messages;

public enum MidiMessageType {
	/// <summary>
	/// 	To control an instrument's voices, Voice messages
	/// 	are sent over the Voice Channels.
	/// </summary>
	ChannelVoice,

	/// <summary>
	/// 	To define the instrument's response to Voice messages,
	/// 	Mode messages are sent over an instrument's Basic Channel.
	/// </summary>
	ChannelMode,
	SystemExclusive,
	SystemCommon,
	SystemRealTime
}
