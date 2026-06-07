using Microsoft.Xna.Framework;
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
	public UICharacterCreationPronounButton(Pronoun pronoun, PlayerPronoun player, UICharacterCreationPronounsPanel parent, PronounMode mode = PronounMode.Specific)
	{
		Width = StyleDimension.FromPixelsAndPercent(-5f, 0.5f);
		Height = StyleDimension.FromPixelsAndPercent(-5f, 0.175f);

		string text = pronoun.FullFormat;
		if (mode == PronounMode.Any)
			text = Pronouns.Any.Value;
		if (mode == PronounMode.PlayerName)
			text = Language.GetTextValue("Mods.PronounsMod.UI.PlayerNameOnly");
		
		UIPanelLabeledButton panel = new UIPanelLabeledButton(Assets.EmptyPanel.Asset, text, UICharacterCreationPronounsPanel.PanelColor, UICharacterCreationPronounsPanel.PanelHoverColor)
		{
			Width = StyleDimension.FromPercent(1),
			Height = StyleDimension.FromPercent(1)
		};

		panel.OnLeftClick += (evt, element) =>
		{
			bool shouldTick = (!player.Pronoun.Equals(pronoun) || mode != player.Mode);
			if (mode == PronounMode.Any || mode == PronounMode.PlayerName)
				shouldTick = player.Mode != mode;
			
			if (shouldTick)
				SoundEngine.PlaySound(SoundID.MenuTick);
			
			player.Pronoun = pronoun;
			player.Mode = mode;
		};

		panel.OnUpdate += element =>
		{
			bool shouldBeSelected = player.Pronoun.Equals(pronoun) && player.Mode == PronounMode.Specific;
			if (mode == PronounMode.Any || mode == PronounMode.PlayerName)
				shouldBeSelected = player.Mode == mode;
			
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
		panel.SetBackgroundAsset(Assets.EmptyPanel.Asset);
		if (panel.BorderColor.Equals(Main.OurFavoriteColor))
			panel.BorderColor = Color.Transparent;
	}
}