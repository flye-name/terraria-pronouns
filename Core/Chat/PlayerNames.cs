using MonoMod.Cil;
using PronounsMod.Core.Players;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;
using Terraria.ModLoader;
using PronounsMod.Core.Config;

namespace PronounsMod.Core;

public sealed class PlayerNames : ModSystem
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
		PronounFormat format = ModContent.GetInstance<FormatConfig>().Format;
		
		if (format == PronounFormat.None || !player.TryGetModPlayer<PlayerPronoun>(out var modPlayer))
			return;

		PronounMode mode = modPlayer.Mode;
		if (mode == PronounMode.None || mode == PronounMode.PlayerName)
			return;
		
		string tag = NameTagHandler.GenerateTag(player.name);
		text2 = text2.Remove(0, tag.Length);

		string pronoun = format switch // switch redundant but more formats may be added.
		{
			PronounFormat.Normal => modPlayer.Pronoun.ChatFormat,
			PronounFormat.Short => modPlayer.Pronoun.Subject
		};
		
		if (mode == PronounMode.Any)
			pronoun = Pronouns.Any.Value;
		
		string pronounTag = $" - [c/b2aacc:{pronoun}]";
		tag = $"<{player.name + pronounTag}>";
		text2 = tag + text2;
	}
}