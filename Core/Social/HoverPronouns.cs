using MonoMod.Cil;
using PronounsMod.Core.Players;
using PronounsMod.Core.Utils;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PronounsMod.Core;

public class HoverPronouns : ModSystem
{
	public override void Load()
	{
		IL_Main.DrawMouseOver += IL_DrawMouseOver;
	}

	void IL_DrawMouseOver(ILContext il)
	{
		ILCursor c = new(il);

		int text2Index = -1 ; // loc
		int numIndex = -1; // loc - current life
		int jIndex = -1; // loc - player index
		
		c.GotoNext(MoveType.After, i => i.MatchLdfld<Player>(nameof(Player.statLife)));
		c.GotoNext(MoveType.After, i => i.MatchStloc(out numIndex));
		
		c.GotoNext(MoveType.After, i => i.MatchLdflda<Player>(nameof(Player.statLifeMax2)));
		c.GotoPrev(MoveType.After, i => i.MatchLdloc(out jIndex));
		c.GotoNext(MoveType.After, i => i.MatchStloc(out text2Index));

		c.EmitLdloc(jIndex);
		c.EmitLdloc(numIndex);
		c.EmitLdloca(text2Index);

		c.EmitDelegate(AlterPlayerHoverText);
	}

	void AlterPlayerHoverText(int playerIndex, int currentLife, ref string text2)
	{
		Player player = Main.player[playerIndex];
		PlayerPronoun modPlayer = player.GetModPlayer<PlayerPronoun>();

		if (string.IsNullOrWhiteSpace(modPlayer.Pronoun.ChatFormat) || modPlayer.ShouldUsePlayerNameDeathFormat())
			return;
		
		string pronoun = modPlayer.Pronoun.ChatFormat;
		if (modPlayer.Mode == PronounMode.Any)
			pronoun = Pronouns.Any.Value;

		text2 = player.name + $" ({pronoun})".FormatWithChatColor() + ": " + currentLife + "/" + player.statLifeMax2;
	}
}