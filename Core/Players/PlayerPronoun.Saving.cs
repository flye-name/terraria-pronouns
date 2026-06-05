using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PronounsMod.Core.Players;

public partial class PlayerPronoun : ModPlayer
{
	public override void SaveData(TagCompound tag)
	{
		tag.Set("SPronoun", Pronoun.RawSubject);
		tag.Set("OPronoun", Pronoun.RawObject);
		tag.Set("PPronoun", Pronoun.RawPossessive);
		tag.Set("PronounMode", (byte)Mode);
	}

	public override void LoadData(TagCompound tag)
	{
		Pronoun = new Pronoun(tag.GetString("SPronoun"), tag.GetString("OPronoun"), tag.GetString("PPronoun"));
		Mode = (PronounMode)tag.GetByte("PronounMode");
	}
}