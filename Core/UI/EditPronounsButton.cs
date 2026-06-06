using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using PronounsMod.Core.Players;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace PronounsMod.Core.UI;

public class EditPronounsButton : ModSystem
{
	public static Asset<Texture2D> ButtonAsset = Assets.ButtonPronouns.Asset;
	
	public override void Load()
	{
		IL_UICharacterListItem.ctor += IL_ctor;
		ButtonAsset.SetToLoadingState();
	}

	private void IL_ctor(ILContext il)
	{
		ILCursor c = new(il);

		int numIndex = -1; // loc - horizontal offset
		
		c.GotoNext(MoveType.After, i => i.MatchLdfld<UICharacterListItem>(nameof(UICharacterListItem._buttonRenameTexture)) );
		c.GotoNext(MoveType.After, i => i.MatchCall<UIElement>(nameof(UIElement.Append)));
		c.GotoNext(MoveType.After, i => i.MatchStloc(out numIndex));
		
		c.EmitLdarg0();
		c.EmitLdarg2(); // orderInList
		c.EmitLdloca(numIndex);

		c.EmitDelegate(AppendButton);
	}

	private void AppendButton(UICharacterListItem self, int orderInList, ref float num)
	{
		UIImageButton uIImageButton = new UIImageButton(ButtonAsset);
		uIImageButton.VAlign = 1f;
		uIImageButton.Left.Set(num, 0f);
		uIImageButton.OnLeftClick += (evt, element) => SetUIState(self._data);
		uIImageButton.OnMouseOver += (_, __) => self._buttonLabel.SetText(Language.GetTextValue("Mods.PronounsMod.UI.EditPronouns"));
		uIImageButton.OnMouseOut += (_, __) => self._buttonLabel.SetText("");
		self.Append(uIImageButton);
		num += 24f;
		
		uIImageButton.SetSnapPoint("EditPronouns", orderInList);
	}

	private void SetUIState(PlayerFileData data)
	{
		SoundEngine.PlaySound(SoundID.MenuOpen);

		Player player = data._player;
		Pronoun oldPronouns = player.GetModPlayer<PlayerPronoun>().Pronoun;
		PronounMode oldMode = player.GetModPlayer<PlayerPronoun>().Mode;

		UIText header = new(Language.GetTextValue("Mods.PronounsMod.UI.EditPronounsLarge", player.name), 1, true)
		{
			VAlign = .28f,
			Width = StyleDimension.FromPercent(1)
		};
			
		UICharacterCreationPronounsPanel pronounPanel = new(player, null)
		{
			Width = StyleDimension.FromPixels(550f),
			Height = StyleDimension.FromPixels(380),
			HAlign = .5f,
			VAlign = .57f,
		};
		
		UIButton<string> backButton = new(Language.GetTextValue("UI.Back"),0.7f, true)
		{
			Width = StyleDimension.FromPixels(260f),
			Height = StyleDimension.FromPixels(50f),
			Left = StyleDimension.FromPixels(-140f),
			Top = StyleDimension.FromPixels(250f),
			HAlign = .5f,
			VAlign = .52f,
		};
		backButton.OnLeftClick += (evt, element) => UIEditPronounState.RevertPronouns(player, oldPronouns, oldMode); 
		
		UIButton<string> saveButton = new(Language.GetTextValue("UI.Save"),0.7f, true)
		{
			Width = StyleDimension.FromPixels(260f),
			Height = StyleDimension.FromPixels(50f),
			Left = StyleDimension.FromPixels(140f),
			Top = StyleDimension.FromPixels(250f),
			HAlign = .5f,
			VAlign = .52f,
		};
		saveButton.OnLeftClick += (evt, element) => UIEditPronounState.SavePronouns(data);

		UIState state = new UIEditPronounState(data, oldPronouns, oldMode);
		state.Append(header);
		state.Append(pronounPanel);
		state.Append(backButton);
		state.Append(saveButton);
		Main.MenuUI.SetState(state);
	}
}