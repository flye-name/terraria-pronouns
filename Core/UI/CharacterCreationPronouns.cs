using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.ModLoader;
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
				if (child is UICharacterCreationPronouns)
				{
					self.RemoveChild(child);
					break;
				}
			}
		};
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
		UICharacterCreationPronouns pronounPanel = new UICharacterCreationPronouns()
		{
			Width = StyleDimension.FromPixels(350f),
			Height = StyleDimension.FromPixels(172),
			Top = StyleDimension.FromPixels(328f),
			HAlign = .5f,
			Left = StyleDimension.FromPixelsAndPercent(10f, 0.25f)
		};
		
		UICharacterNameButton pronounsButton = new UICharacterNameButton(Language.GetText("Mods.PronounsMod.UI.Pronouns"), Language.GetText("Mods.PronounsMod.UI.Blank"));
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

public class UICharacterCreationPronouns : UIElement
{
	public UICharacterCreationPronouns()
	{
		UIPanel panel = new UIPanel()
		{
			Width = StyleDimension.FromPercent(1),
			Height = StyleDimension.FromPercent(1),
			BackgroundColor = new Color(33, 43, 79) * 0.8f
		};
		Append(panel);	
	}
}