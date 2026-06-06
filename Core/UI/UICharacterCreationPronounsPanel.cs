using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PronounsMod.Core.Players;
using PronounsMod.Core.Utils;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace PronounsMod.Core.UI;

// TODO: CLEANUP
public class UICharacterCreationPronounsPanel : UIElement
{
	public UICharacterCreation? MainParent;
	public Player player;
	public PlayerPronoun ModPlayer => player.GetModPlayer<PlayerPronoun>();
	
	public readonly UIPanelLabeledButton[] inputs = new UIPanelLabeledButton[3];
	public UIText footer;
	public UIText footer2;
	public UIPanel panel;
	public UIElement container;

	public static Color PanelHoverColor => new Color(100, 103, 151);
	public static Color PanelColor => new Color(63, 82, 151) * 0.7f;
	
	public UICharacterCreationPronounsPanel(Player player, UICharacterCreation? parent)
	{
		MainParent = parent;
		this.player = player;
		
		panel = new UIPanel()
		{
			Width = StyleDimension.FromPercent(1),
			Height = StyleDimension.FromPercent(1),
			BackgroundColor = new Color(33, 43, 79) * 0.8f
		};

		container = new UIElement()
		{
			Width = StyleDimension.FromPercent(1),
			Height = StyleDimension.FromPercent(0.8f),
			VAlign = 0
		};
		
		panel.Append(container);
		
		CreateFooter();

		CreateHeaders();

		CreatePresetButtons();

		CreateInputs();
		
		Append(panel);	
	}

	void CreateInput(int i)
	{
		inputs[i] = new UIPanelLabeledButton(Assets.EmptyPanel.Asset, ModPlayer.Pronoun.FullFormat.Split('/')[i], PanelColor, PanelHoverColor)
		{
			Width = StyleDimension.FromPixelsAndPercent(-5f, .33f),
			Height = StyleDimension.FromPixelsAndPercent(-5f, 0.175f),
			HAlign = i / 2f,
			VAlign = 1f
		};
			
		inputs[i].OnLeftClick += (evt, element) =>
		{
			SoundEngine.PlaySound(SoundID.MenuTick);
			Main.clrInput();
		};
	}
	
	void CreateInputs()
	{
		for (int i = 0; i < 3; i++)
		{
			CreateInput(i);
			
			switch (i)
			{
				case 0:
					inputs[i].OnUpdate += element => inputs[0].SetText(ModPlayer.Mode == PronounMode.Any ? "" : ModPlayer.Pronoun.Subject);
					
					inputs[i].OnLeftClick += (evt, element) =>
					{
						Main.MenuUI.SetState(PronounInput(0, Language.GetTextValue("Mods.PronounsMod.UI.EnterSubject")));
					}; 
					break;
				
				case 1:
					inputs[i].OnUpdate += element => inputs[1].SetText(ModPlayer.Mode == PronounMode.Any ? "" : ModPlayer.Pronoun.Object);
					
					inputs[i].OnLeftClick += (evt, element) =>
					{
						Main.MenuUI.SetState(PronounInput(1, Language.GetTextValue("Mods.PronounsMod.UI.EnterObject")));
					}; 
					break;
				
				case 2: 
					inputs[i].OnUpdate += element => inputs[2].SetText(ModPlayer.Mode == PronounMode.Any ? "" : ModPlayer.Pronoun.Possessive);
					
					inputs[i].OnLeftClick += (evt, element) =>
					{
						Main.MenuUI.SetState(PronounInput(2, Language.GetTextValue("Mods.PronounsMod.UI.EnterPossessive")));
					}; 
					break;
			}
		}
		
		for (int i = 0; i < 3 ; i++)	
			container.Append(inputs[i]);
	}

	void CreatePresetButtons()
	{
		UICharacterCreationPronounButton heHim = new UICharacterCreationPronounButton(Pronouns.He, ModPlayer, this) 
		{
			HAlign = 1f,
			VAlign = 0.12f
		};
		UICharacterCreationPronounButton sheHer = new UICharacterCreationPronounButton(Pronouns.She, ModPlayer, this)
		{
			HAlign = 0f,
			VAlign = 0.12f
		};
		UICharacterCreationPronounButton theyThem = new UICharacterCreationPronounButton(Pronouns.They, ModPlayer, this) 
		{
			HAlign = 1f,
			VAlign = 0.32f
		};
		UICharacterCreationPronounButton itIts = new UICharacterCreationPronounButton(Pronouns.It, ModPlayer, this) 
		{
			HAlign = 0f,
			VAlign = 0.32f
		};
		
		UICharacterCreationPronounButton none = new UICharacterCreationPronounButton(Pronouns.None, ModPlayer, this, PronounMode.PlayerName)
		{
			HAlign = 0f,
			VAlign = 0.52f
		};
		UICharacterCreationPronounButton any = new UICharacterCreationPronounButton(Pronouns.They, ModPlayer, this, PronounMode.Any)
		{
			HAlign = 1f,
			VAlign = 0.52f
		};
		
		container.Append(none);
		container.Append(any);
		container.Append(heHim);
		container.Append(sheHer);
		container.Append(theyThem);
		container.Append(itIts);
	}
	
	void CreateHeaders()
	{
		UIText presets = new UIText(Language.GetText("Mods.PronounsMod.UI.Presets"))
		{
			Width = StyleDimension.FromPercent(1),
			VAlign = 0f,
			HAlign = 0.5f
		};

		UIText custom = new UIText(Language.GetText("Mods.PronounsMod.UI.EnterPronouns"))
		{
			Width = StyleDimension.FromPercent(1),
			VAlign = 0.78f,
			HAlign = 0.5f
		};

		UIHorizontalSeparator presetSeparator = new UIHorizontalSeparator()
		{
			Width = StyleDimension.FromPixelsAndPercent(-25f, 1f),
			Top = StyleDimension.FromPixelsAndPercent(6f, 0.04f),
			Left = new StyleDimension(-2.5f, 0f),
			VAlign = 0f,
			HAlign = 0.5f,
			Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
		};

		UIHorizontalSeparator customSeparator = new UIHorizontalSeparator()
		{
			Width = StyleDimension.FromPixelsAndPercent(-25f, 1f),
			Top = StyleDimension.FromPixelsAndPercent(6f, 0.78f),
			Left = new StyleDimension(-2.5f, 0f),
			VAlign = 0f,
			HAlign = 0.5f,
			Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
		};
		
		container.Append(presetSeparator);
		container.Append(customSeparator);
		container.Append(presets);
		container.Append(custom);
	}
	
	void CreateFooter()
	{
		UISlicedImage footerPanel = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight"))
		{
			Width = StyleDimension.FromPercent(1f),
			Height = StyleDimension.FromPixels(60f),
			VAlign = 1f,
			HAlign = 0.5f,
			Color = Color.LightGray * 0.7f
		};
		footerPanel.SetSliceDepths(10);
		
		footer = new UIText(Language.GetText("Mods.PronounsMod.UI.Blank"))
		{
			Width = StyleDimension.FromPercent(1f),
			Left = StyleDimension.FromPixels(15f),
			TextOriginX = 0f,
			HAlign = 0.5f,
			VAlign = 0.15f
		};
		footer.OnUpdate += element =>
		{
			string playerName = player.name.Length == 0 ? Language.GetTextValue("Mods.PronounsMod.UI.Player") : player.name;
			if (ModPlayer.Mode == PronounMode.PlayerName)
				footer.SetText($"[c/E11919:{Language.GetText("DeathTextGeneric.Brain.PlayerName").Format(playerName)}]");
			else if (ModPlayer.Mode == PronounMode.Any)
				footer.SetText($"[c/E11919:{Language.GetText("DeathTextGeneric.Brain").Format(playerName, "", "", "", Pronouns.They.Possessive)}]");
			else
				footer.SetText($"[c/E11919:{Language.GetText("DeathTextGeneric.Brain").Format(playerName, "", "", "", ModPlayer.Pronoun.Possessive)}]");
		};
		
		
		footer2 = new UIText(Language.GetText("Mods.PronounsMod.UI.Blank"))
		{
			Width = StyleDimension.FromPercent(1f),
			Left = StyleDimension.FromPixels(15f),
			TextOriginX = 0f,
			HAlign = 0.5f,
			VAlign = 0.8f
		};
		footer2.OnUpdate += element =>
		{
			string playerName = player.name.Length == 0 ? Language.GetTextValue("Mods.PronounsMod.UI.Player") : player.name;
			if (ModPlayer.Mode == PronounMode.PlayerName)
				footer2.SetText($"<{playerName}> {Language.GetTextValue("Mods.PronounsMod.UI.SampleMessage")}");
			else if (ModPlayer.Mode == PronounMode.Any)
				footer2.SetText($"<{playerName} - {Pronouns.Any.Value.FormatWithChatColor()}> {Language.GetTextValue("Mods.PronounsMod.UI.SampleMessage")}");
			else
				footer2.SetText($"<{playerName} - {ModPlayer.Pronoun.ChatFormat.FormatWithChatColor()}> {Language.GetTextValue("Mods.PronounsMod.UI.SampleMessage")}");
		};

		footerPanel.Append(footer);
		footerPanel.Append(footer2);
		panel.Append(footerPanel);
	}

	UIVirtualKeyboard PronounInput(int index, string label)
	{
		UIVirtualKeyboard state = new UIVirtualKeyboard(label, "",
			text =>
			{
				string pronoun = text;
				pronoun = text.Replace(" ", string.Empty);
				pronoun = text.Replace("/", string.Empty);

				ModPlayer.Pronoun.Edit(index, pronoun);

				ModPlayer.Mode = PronounMode.Specific;
				if (MainParent != null)
					Main.MenuUI.SetState(MainParent);
				else
					Main.MenuUI.SetState(Parent as UIState);
			},
			() =>
			{
				if (MainParent != null)
					Main.MenuUI.SetState(MainParent);
				else
					Main.MenuUI.SetState(Parent as UIState);
			}, 
			0, allowEmpty: false);
		state.SetMaxInputLength(10);

		return state;
	}
}