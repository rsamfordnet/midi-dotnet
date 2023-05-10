namespace Midi.Messages;

public enum MidiSystemCommonMessageType {
	/// <summary>
	/// 	An SOX should always be sent at the beginning of a System Exclusive message.
	/// </summary>
	SystemExclusive = 0xF0,

	/// <summary>
	/// 	Some master device that controls sequence playback sends this
	/// 	timing message to keep a slave device in sync with the master.
	/// </summary>
	MidiTimeCodeQuarterFrame = 0xF1,

	/// <summary>
	/// 	Some master device that controls sequence playback sends this message to
	/// 	force a slave device to cue the playback to a certain point in the song/sequence.
	/// 	In other words, this message sets the device's "Song Position". This message
	/// 	doesn't actually start the playback. It just sets up the device to be
	/// 	"ready to play" at a particular point in the song.
	/// </summary>
	SongPositionPointer = 0xF2,

	/// <summary>
	/// 	Some master device that controls sequence playback sends this message to
	/// 	force a slave device to set a certain song for playback (ie, sequencing).
	/// </summary>
	SongSelect = 0xF3,

	// Undefined1 = 0xF4,
	// Undefined2 = 0xF5,

	/// <summary>
	/// 	The device receiving this should perform a tuning calibration.
	/// </summary>
	TuneRequest = 0xF6,

	/// <summary>
	/// 	An EOX should always be sent at the end of a System Exclusive message.
	/// </summary>
	EndOfExclusive = 0xF7,
}



public class MidiSysCommonMessageInfo : TypedEnum<byte> {

	/// <summary>
	/// Some master device that controls sequence playback sends this message to
	/// force a slave device to set a certain song for playback (ie, sequencing).
	/// </summary>
	public static readonly MidiSystemCommonMessageType SongSelect = new(0xF3);

	/// <summary>
	/// The device receiving this should perform a tuning calibration.
	/// </summary>
	public static readonly MidiSystemCommonMessageType TuneRequest = new(0xF4);

	public MidiSysCommonMessageType(byte value) : base(value) { }
}
