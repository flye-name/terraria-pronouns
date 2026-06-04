using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PronounsMod.Core.Players;

public partial class PlayerPronoun : ModPlayer
{
	public Pronoun Pronoun = Pronouns.They;
	public PronounMode Mode = PronounMode.Specific;

	public override void PostUpdate()
	{
		base.PostUpdate();
	}
}