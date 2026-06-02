using Microsoft.Xna.Framework;
using MonoMod.Cil;
using Terraria.GameContent.UI.States;
using Terraria.ModLoader;
using Terraria.UI;

namespace PronounsMod.Core.UI;

public class CharacterCreationEdit : ModSystem
{
	public static readonly UICharacterCreation.CategoryId PronounsCategoryId = UICharacterCreation.CategoryId.Count; 
	public override void Load()
	{
		IL_UICharacterCreation.BuildPage += ResizeMainPanel;
		IL_UICharacterCreation.MakeCategoriesBar += AppendPronounsCategoryButton;
	}

	#region resize panel
	void ResizeMainPanel(ILContext il)
	{
		ILCursor c = new(il);

		c.GotoNext(MoveType.After, i => i.MatchStloc1()); // set uIElement

		c.EmitLdloca(1); // uIElement

		c.EmitDelegate(InnerResizeMainPanel);
	}

	void InnerResizeMainPanel(ref UIElement element)
	{
		element.Width.Set(550, 0);
	}
	#endregion
	
	#region tab button
	void AppendPronounsCategoryButton(ILContext il)
	{
		ILCursor c = new(il);

		c.GotoNext(MoveType.After, i => i.MatchStloc1()); // xPositionPerId initialized to 48f 

		c.EmitLdcR4(-267.5f);
		c.EmitStloc0(); // set xPositionStart to -265f

		c.EmitLdcR4(49f);
		c.EmitStloc1(); // set xPositionPerId to 49f
		
		c.GotoNext(MoveType.Before, i => i.MatchCall<UICharacterCreation>(nameof(UICharacterCreation.UpdateColorPickers)));
		c.GotoPrev(MoveType.Before, i => i.MatchLdarg0()); // move to where the last tab is appended

		c.EmitLdarg0();
		c.EmitLdarg1(); // categoryContainer
		c.EmitLdloc0(); // xPositionStart
		c.EmitLdloc1(); // xPositionPerId

		c.EmitDelegate(InnerAppendPronounsCategoryButton);
	}

	void InnerAppendPronounsCategoryButton(UICharacterCreation self, UIElement categoryContainer, float xPositionStart, float xPositionPerId)
	{
		UIElement element = self.CreatePickerWithoutClick(PronounsCategoryId, "Images/UI/CharCreation/HairStyle_Hair", xPositionStart, xPositionPerId);
		categoryContainer.Append(element);
	}
	#endregion
}

public class CharacterCreationPronouns
{
	
}