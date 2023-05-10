namespace Midi.Messages;

public class MidiSysCommonMessageType : TypedEnum<byte> {
	/// <summary>
	/// Not a midi system common type.
	/// </summary>
	public static readonly MidiSysCommonMessageType Invalid = new(0x00);

	/// <summary>
	/// Some master device that controls sequence playback sends this
	/// timing message to keep a slave device in sync with the master.
	/// </summary>
	public static readonly MidiSysCommonMessageType MtcQuarterFrame = new(0xF1);

	/// <summary>
	/// Some master device that controls sequence playback sends this message to
	/// force a slave device to cue the playback to a certain point in the song/sequence.
	/// In other words, this message sets the device's "Song Position". This message
	/// doesn't actually start the playback. It just sets up the device to be
	/// "ready to play" at a particular point in the song.
	/// </summary>
	public static readonly MidiSysCommonMessageType SongPositionPointer = new(0xF2);

	/// <summary>
	/// Some master device that controls sequence playback sends this message to
	/// force a slave device to set a certain song for playback (ie, sequencing).
	/// </summary>
	public static readonly MidiSysCommonMessageType SongSelect = new(0xF3);

	/// <summary>
	/// The device receiving this should perform a tuning calibration.
	/// </summary>
	public static readonly MidiSysCommonMessageType TuneRequest = new(0xF4);

	public MidiSysCommonMessageType(byte value) : base(value) { }
}
