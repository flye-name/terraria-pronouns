using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PronounsMod.Core.Players;

public partial class PlayerPronoun : ModPlayer
{
	public override void SaveData(TagCompound tag)
	{
		tag.Set("SPronoun", Pronoun.Subject);
		tag.Set("OPronoun", Pronoun.Object);
		tag.Set("PPronoun", Pronoun.Possessive);
		tag.Set("PronounMode", (byte)Mode);
	}

	public override void LoadData(TagCompound tag)
	{
		Pronoun = new Pronoun(tag.GetString("SPronoun"), tag.GetString("OPronoun"), tag.GetString("PPronoun"));
		Mode = (PronounMode)tag.GetByte("PronounMode");
	}
}