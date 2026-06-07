using System.Reflection;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using MonoMod.Cil;
using PronounsMod.Core.Players;
using PronounsMod.Core.Utils;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PronounsMod.Core.UI;

public class PlayerListEdit : ModSystem
{
	public override void Load()
	{
		IL_UICharacterListItem.DrawSelf += IL_DrawSelf;
	}

	private void IL_DrawSelf(ILContext il)
	{
		ILCursor c = new ILCursor(il);

		int textIndex = -1; // loc

		c.GotoNext(MoveType.After, 
			i => i.MatchLdfld<FileData>(nameof(UICharacterListItem._data.Name)),
			i => i.MatchStloc(out textIndex)
		);

		c.EmitLdarg0();
		c.EmitLdloca(textIndex);

		c.EmitDelegate(AppendPronouns);
	}

	private static void AppendPronouns(UICharacterListItem self, ref string text)
	{
		Player player = self._data._player;
		if (player.TryGetModPlayer<PlayerPronoun>(out var modPlayer))
		{
			switch (modPlayer.Mode)
			{
				case PronounMode.Specific:
					if (!string.IsNullOrWhiteSpace(modPlayer.Pronoun.FullFormat))
						text = text + " " + $"({modPlayer.Pronoun.FullFormat})".FormatWithChatColor();
					break;
				
				case PronounMode.Any:
					text = text + " " + $"({Pronouns.Any.Value})".FormatWithChatColor();
					break;
				
				case PronounMode.None:
					text = text + " " + Language.GetTextValue("Mods.PronounsMod.UI.Unassigned").FormatWithChatColor();
					break;
			}
		}
	}
}