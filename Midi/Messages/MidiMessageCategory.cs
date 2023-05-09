namespace Midi.Messages;

public enum MidiMessageCategory {
	/// <summary>
	/// 	<p>
	/// 		A Channel message uses four bits in the Status byte to address the message to one of sixteen MIDI
	/// 		channels and four bits to define the message. Channel messages are thereby intended for the receivers
	/// 		in a system whose channel number matches the channel number encoded into the Status byte.
	/// 	</p>
	/// 	<p>
	/// 		An instrument can receive MIDI messages on more than one channel. The channel in which it receives
	/// 		its main instructions, such as which program number to be on and what mode to be in, is referred to as
	/// 		its "Basic Channel".
	/// 	</p>
	/// </summary>
	Channel,

	/// <summary>
	/// 	System messages are not encoded with channel numbers.
	/// </summary>
	System,
}
