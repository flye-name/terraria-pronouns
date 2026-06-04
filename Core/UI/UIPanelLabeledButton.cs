using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PronounsMod.Core.UI;

public class UIPanelLabeledButton : UIPanel
{
	public UIText uIText;
	
	public UIPanelLabeledButton(Asset<Texture2D> asset, string text, Color defaultColor, Color hoverColor, Color? borderColor = null)
	{
		_backgroundTexture = asset;
		_borderTexture = Assets.PanelOutline.Asset;
		BorderColor = borderColor ?? Color.Transparent;
		
		OnMouseOut += (evt, element) =>
		{
			BackgroundColor = defaultColor;
		};
		OnMouseOver += (evt, element) =>
		{
			BackgroundColor = hoverColor;
		};
		
		uIText = new UIText(text)
		{
			Width = StyleDimension.FromPercent(1),
			Height = StyleDimension.FromPercent(1)
		};
		
		Append(uIText);
	}

	public void SetBackgroundAsset(Asset<Texture2D> asset) => _backgroundTexture = asset;
	
	public void SetText(string text) => uIText.SetText(text); 
}