using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using PronounsMod.Core.Players;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace PronounsMod.Core.UI;

public class CharacterCreationEdit : ModSystem
{
	public static readonly UICharacterCreation.CategoryId PronounsCategoryId = UICharacterCreation.CategoryId.Count;

	public override void Load()
	{
		IL_UICharacterCreation.MakeInfoMenu += AddPronounsField;

		On_UICharacterCreation.UnselectAllCategories += (orig, self) =>
		{
			orig(self);
			foreach (var child in self.Children)
			{
				if (child is UICharacterCreationPronounsPanel)
				{
					self.RemoveChild(child);
					break;
				}
			}
		};
		
		Assets.EmptyPanel.Asset.SetToLoadingState();
		Assets.FullPanel.Asset.SetToLoadingState();
		Assets.PanelOutline.Asset.SetToLoadingState();
	}

	void AddPronounsField(ILContext il)
	{
		ILCursor c = new(il);

		c.GotoNext(MoveType.After, i => i.MatchNewobj<UICharacterNameButton>()); 
		
		c.GotoNext(MoveType.After, i => i.MatchStfld<UIElement>(nameof(UIElement.Width)));
		c.GotoPrev(MoveType.Before, i => i.MatchLdcR4(1)); // Width percentage

		c.Remove();
		c.EmitLdcR4(0.75f);
		
		c.GotoNext(MoveType.After, i => i.MatchStfld<UIElement>(nameof(UIElement.HAlign))); 
		c.GotoPrev(MoveType.Before, i => i.MatchLdcR4(0.5f)); // HAlign

		c.Remove();
		c.EmitLdcR4(0);

		c.GotoNext(MoveType.After, i => i.MatchCallvirt<UIElement>(nameof(UIElement.Append)));

		c.EmitLdarg0();
		c.EmitLdloc0(); // uIElement

		c.EmitDelegate(InnerAddPronounsField);
	}

	void InnerAddPronounsField(UICharacterCreation self, UIElement uIElement)
	{
		UICharacterCreationPronounsPanel pronounPanel = new UICharacterCreationPronounsPanel(self._player, self)
		{
			Width = StyleDimension.FromPixels(350f),
			Height = StyleDimension.FromPixels(310),
			Top = StyleDimension.FromPixels(268),
			HAlign = .5f,
			VAlign = 0f,
			Left = StyleDimension.FromPixelsAndPercent(10f, 0.25f)
		};
		
		UICharacterNameButton pronounsButton = new UICharacterNameButton(Language.GetText("Mods.PronounsMod.UI.Pronouns").WithFormatArgs(Pronouns.Color), Language.GetText("Mods.PronounsMod.UI.Blank"));
		pronounsButton.Width = StyleDimension.FromPixelsAndPercent(-5f, 0.25f);
		pronounsButton.HAlign = 1f;
		foreach (var child in pronounsButton.Children)
		{
			if (child is UIText)
				child.HAlign = 0.25f;
		}
		pronounsButton.RecalculateChildren();
		uIElement.Append(pronounsButton);
		pronounsButton.OnLeftMouseDown += (evt, element) =>
		{
			SoundEngine.PlaySound(10);
			Main.clrInput();
			
			if (self.HasChild(pronounPanel))
			{
				self.RemoveChild(pronounPanel);
			}
			else
			{
				self.Append(pronounPanel);
			}
		};
	}
}


