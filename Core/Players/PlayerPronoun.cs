using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PronounsMod.Core.Players;

public partial class PlayerPronoun : ModPlayer
{
	public Pronoun Pronoun = Pronouns.None;
	public PronounMode Mode = PronounMode.None;

	public override void PostUpdate()
	{
		base.PostUpdate();
        Mode = PronounMode.None;
		Pronoun = Pronouns.She;
	}
}