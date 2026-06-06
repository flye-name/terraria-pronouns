using System.IO;
using MonoMod.Utils;
using Terraria;
using Terraria.ModLoader;

namespace PronounsMod.Core.Players;

public partial class PlayerPronoun
{
	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
	{
		ModPacket packet = Mod.GetPacket();
		packet.Write((byte)Message.SyncPronouns);
		packet.Write((byte)Player.whoAmI);
		packet.WriteNullTerminatedString(Pronoun.RawSubject);
		packet.WriteNullTerminatedString(Pronoun.RawObject);
		packet.WriteNullTerminatedString(Pronoun.RawPossessive);
		packet.Write((byte)Mode);
		packet.Send(toWho, fromWho);
	}

	public static void ReceiveChanges(BinaryReader reader, int whoAmI)
	{
		bool shouldDoAnything = true;

		byte msg = reader.ReadByte();
		if (msg != (byte)Message.SyncPronouns)
			shouldDoAnything = false;

		byte index = reader.ReadByte();
		Pronoun pronoun = new Pronoun(reader.ReadNullTerminatedString(), reader.ReadNullTerminatedString(), reader.ReadNullTerminatedString());
		PronounMode mode = (PronounMode)reader.ReadByte();

		if (index == Main.myPlayer)
			shouldDoAnything = false;

		PlayerPronoun player = Main.player[index].GetModPlayer<PlayerPronoun>();

		if (shouldDoAnything)
		{
			player.Pronoun = pronoun;
			player.Mode = mode;
		}
	}
}

public enum Message : byte
{
	SyncPronouns
}