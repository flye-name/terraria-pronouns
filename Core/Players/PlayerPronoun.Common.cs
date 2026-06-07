using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PronounsMod.Core.Players;

public partial class PlayerPronoun : ModPlayer
{
	public Pronoun Pronoun = Pronouns.None;
	public PronounMode Mode = PronounMode.None;
	
	public bool ShouldUsePlayerNameDeathFormat() => Mode == PronounMode.PlayerName || string.IsNullOrWhiteSpace(Pronoun.Possessive);
}