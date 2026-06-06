using Microsoft.Xna.Framework;
using PronounsMod.Core.Players;
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
	public UICharacterCreation MainParent;
	public Player player;
	public PlayerPronoun ModPlayer => player.GetModPlayer<PlayerPronoun>();
	
	public readonly UIPanelLabeledButton[] inputs = new UIPanelLabeledButton[3];
	public UIText footer;
	public UIPanel panel;

	public static Color PanelHoverColor => new Color(100, 103, 151);
	public static Color PanelColor => new Color(63, 82, 151) * 0.7f;
	
	public UICharacterCreationPronounsPanel(Player player, UICharacterCreation parent)
	{
		MainParent = parent;
		this.player = player;
		
		panel = new UIPanel()
		{
			Width = StyleDimension.FromPercent(1),
			Height = StyleDimension.FromPercent(1),
			BackgroundColor = new Color(33, 43, 79) * 0.8f
		};
		
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
					inputs[i].OnUpdate += element => inputs[0].SetText(ModPlayer.Mode == PronounMode.Any ? Pronouns.Any.Value : ModPlayer.Pronoun.Subject);
					
					inputs[i].OnLeftClick += (evt, element) =>
					{
						Main.MenuUI.SetState(PronounInput(0, Language.GetTextValue("Mods.PronounsMod.UI.EnterSubject")));
					}; 
					break;
				
				case 1:
					inputs[i].OnUpdate += element => inputs[1].SetText(ModPlayer.Mode == PronounMode.Any ? Pronouns.Any.Value : ModPlayer.Pronoun.Object);
					
					inputs[i].OnLeftClick += (evt, element) =>
					{
						Main.MenuUI.SetState(PronounInput(1, Language.GetTextValue("Mods.PronounsMod.UI.EnterObject")));
					}; 
					break;
				
				case 2: 
					inputs[i].OnUpdate += element => inputs[2].SetText(ModPlayer.Mode == PronounMode.Any ? Pronouns.Any.Value : ModPlayer.Pronoun.Possessive);
					
					inputs[i].OnLeftClick += (evt, element) =>
					{
						Main.MenuUI.SetState(PronounInput(2, Language.GetTextValue("Mods.PronounsMod.UI.EnterPossessive")));
					}; 
					break;
			}
		}
		
		for (int i = 0; i < 3 ; i++)	
			panel.Append(inputs[i]);
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
		
		panel.Append(none);
		panel.Append(any);
		panel.Append(heHim);
		panel.Append(sheHer);
		panel.Append(theyThem);
		panel.Append(itIts);
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
		
		panel.Append(presetSeparator);
		panel.Append(customSeparator);
		panel.Append(presets);
		panel.Append(custom);
	}
	
	void CreateFooter()
	{
		footer = new UIText(Language.GetText("Mods.PronounsMod.UI.Blank"))
		{
			Width = StyleDimension.FromPercent(1f),
			VAlign = 1.15f,
			HAlign = 0.5f
		};
		footer.OnUpdate += element =>
		{
			if (ModPlayer.Mode == PronounMode.PlayerName)
				footer.SetText($"[c/E11919:{Language.GetText("DeathTextGeneric.Brain.PlayerName").Format(player.name.Length == 0 ? "<player-name>" : player.name)}]");
			else if (ModPlayer.Mode == PronounMode.Any)
				footer.SetText(Language.GetText("Mods.PronounsMod.UI.AnyInChat"));
			else
				footer.SetText($"[c/E11919:{Language.GetText("DeathTextGeneric.Brain").Format(player.name.Length == 0 ? "<player-name>" : player.name, "", "", "", ModPlayer.Pronoun.Possessive)}]");
		};

		panel.Append(footer);
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
				Main.MenuUI.SetState(MainParent);
			},
			() =>
			{
				Main.MenuUI.SetState(MainParent);
			}, 
			0, allowEmpty: false);
		state.SetMaxInputLength(10);

		return state;
	}
}