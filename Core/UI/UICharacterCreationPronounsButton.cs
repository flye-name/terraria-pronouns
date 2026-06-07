using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PronounsMod.Core.Players;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace PronounsMod.Core.UI;

public class UICharacterCreationPronounButton : UIElement
{
	public UICharacterCreationPronounButton(Pronoun pronoun, PlayerPronoun player, UICharacterCreationPronounsPanel parent, PronounMode mode = PronounMode.Specific, Color? MainColor = null, Color? HoverColor = null)
	{
		Width = StyleDimension.FromPixelsAndPercent(-5f, 0.5f);
		Height = StyleDimension.FromPixelsAndPercent(-5f, 0.175f);

		string text = pronoun.FullFormat;
		if (mode == PronounMode.Any)
			text = Pronouns.Any.Value.ApplyCase(LetterCasing.Sentence);
		if (mode == PronounMode.PlayerName)
			text = Language.GetTextValue("Mods.PronounsMod.UI.PlayerNameOnly");
		
		UIPanelLabeledButton panel = new UIPanelLabeledButton(Assets.EmptyPanelAlt.Asset, text, MainColor ?? UICharacterCreationPronounsPanel.PanelColor, HoverColor ?? UICharacterCreationPronounsPanel.PanelHoverColor)
		{
			Width = StyleDimension.FromPercent(1),
			Height = StyleDimension.FromPercent(1)
		};

		panel.OnLeftClick += (evt, element) =>
		{
			bool shouldTick = (!player.Pronoun.Equals(pronoun) || mode != player.Mode);
			
			if (mode == PronounMode.PlayerName)
				shouldTick = player.Mode != mode;
			
			if (mode == PronounMode.Any)
				shouldTick = true;
			
			if (shouldTick)
				SoundEngine.PlaySound(SoundID.MenuTick);
			

			if (mode == PronounMode.Any)
			{
				player.Mode = player.Mode == PronounMode.Any ? PronounMode.Specific : PronounMode.Any;
			}
			else
			{
				if (player.Mode != PronounMode.Any || mode == PronounMode.PlayerName)
					player.Mode = mode;
				
				player.Pronoun = pronoun;
			}
		};

		panel.OnUpdate += element =>
		{
			bool shouldBeSelected = player.Pronoun.Equals(pronoun) && player.Mode is PronounMode.Specific or PronounMode.Any;
			if (mode == PronounMode.Any)
				shouldBeSelected = player.Mode == mode;
			if (mode == PronounMode.PlayerName)
				shouldBeSelected = false;
			
			if (shouldBeSelected)
				SelectPanel(panel);
			else
				DeselectPanel(panel);
		};
		
		Append(panel);
	}

	void SelectPanel(UIPanelLabeledButton panel)
	{
		panel.SetBackgroundAsset(Assets.FullPanel.Asset);
		panel.BorderColor = Main.OurFavoriteColor;
	}

	void DeselectPanel(UIPanelLabeledButton panel)
	{
		panel.SetBackgroundAsset(Assets.EmptyPanelAlt.Asset);
		if (panel.BorderColor.Equals(Main.OurFavoriteColor))
			panel.BorderColor = Color.Transparent;
	}
}