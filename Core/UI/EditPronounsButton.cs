using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
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
		uIImageButton.OnLeftClick += ButtonClick;
		uIImageButton.OnMouseOver += (_, __) => self._buttonLabel.SetText(Language.GetTextValue("Mods.PronounsMod.UI.EditPronouns"));
		uIImageButton.OnMouseOut += (_, __) => self._buttonLabel.SetText("");
		self.Append(uIImageButton);
		num += 24f;
		
		uIImageButton.SetSnapPoint("EditPronouns", orderInList);
	}

	private void ButtonClick(UIMouseEvent evt, UIElement element)
	{
		SoundEngine.PlaySound(SoundID.MenuTick);
	}
}