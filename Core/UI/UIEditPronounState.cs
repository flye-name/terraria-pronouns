using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PronounsMod.Core.Players;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.IO;
using Terraria.UI;

namespace PronounsMod.Core.UI;

public class UIEditPronounState : UIState
{
	public PlayerFileData data;
	public Pronoun oldPronoun;
	public PronounMode oldMode;
	public int gracePeriod;
	public UIEditPronounState(PlayerFileData data, Pronoun oldPronoun, PronounMode oldMode)
	{
		this.data = data;
		this.oldPronoun = oldPronoun;
		this.oldMode = oldMode;
		
		Width = StyleDimension.FromPercent(1);
		Height = StyleDimension.FromPercent(1);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		
		if (!Main.keyState.IsKeyDown(Keys.Enter) && !Main.keyState.IsKeyDown(Keys.Escape))
			gracePeriod--;

		if (gracePeriod > 0)
			return;

		if (Main.keyState.IsKeyDown(Keys.Enter))
		{
			SavePronouns(data);
		}

		if (Main.keyState.IsKeyDown(Keys.Escape))
		{
			RevertPronouns(data._player, oldPronoun, oldMode);
		}
	}

	public static void RevertPronouns(Player player, Pronoun oldPronouns, PronounMode oldMode)
	{
		SoundEngine.PlaySound(SoundID.MenuClose);
		
		player.GetModPlayer<PlayerPronoun>().Pronoun = oldPronouns;
		player.GetModPlayer<PlayerPronoun>().Mode = oldMode;
		
		Main.MenuUI.SetState(Main._characterSelectMenu); 
	}

	public static void SavePronouns(PlayerFileData data)
	{
		SoundEngine.PlaySound(SoundID.MenuTick);
		
		Player.SavePlayer(data);
		
		SoundEngine.PlaySound(SoundID.ResearchComplete);
		
		Main.MenuUI.SetState(Main._characterSelectMenu); 
	}
}