using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PronounsMod.Core.Players;

public partial class PlayerPronoun : ModPlayer
{
	public Pronoun Pronoun = new("UNLOADED", "UNLOADED", "UNLOADED");
}