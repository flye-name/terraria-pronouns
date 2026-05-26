using System;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using PronounsMod.Core.Players;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace PronounsMod.Core;

public class ChatEdit : ModSystem
{
	public override void Load()
	{
		IL_ChatHelper.DisplayMessage += IL_DisplayMessage;
	}
	
	private void IL_DisplayMessage(ILContext il)
	{
		ILCursor c = new(il);

		int text2Index = -1; // loc
		int messageAuthorIndex = -1; // arg
		
		c.GotoNext(MoveType.After, i => i.MatchCall<NameTagHandler>(nameof(NameTagHandler.GenerateTag)));
		c.GotoNext(MoveType.After, i => i.MatchStloc(out text2Index));
		
		c.FindPrev(out _, i => i.MatchLdarg(out messageAuthorIndex)); // = 2

		c.EmitLdarg(messageAuthorIndex);
		c.EmitLdloca(text2Index);

		c.EmitDelegate(ReplaceTag);
	}

	private static void ReplaceTag(byte author, ref string text2)
	{
		Player player = Main.player[author];
		if (player.TryGetModPlayer<PlayerPronoun>(out var modPlayer))
		{
			string tag = NameTagHandler.GenerateTag(player.name);

			text2 = text2.Remove(0, tag.Length);
					
			string pronounTag = $" - [c/b2aacc:{modPlayer.Pronoun.ShortFormat}]";
			tag = $"<{player.name + pronounTag}>";
					
			text2 = tag + text2;
		}
	}
}