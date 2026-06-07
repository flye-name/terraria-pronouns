using System.Linq;
using MonoMod.Cil;
using PronounsMod.Core.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PronounsMod.Core.CrossMod;

public class CrossModDeathMessages : ModSystem
{
	public static readonly string[] SupportedKeysVariant1 = 
	[
		"Mods.CalamityMod.Status.Death."
	];
	
	public override void Load()
	{
		IL_Player.KillMe += IL_KillMe;
	}

	void IL_KillMe(ILContext il)
	{
		ILCursor c = new(il);

		int deathTextIndex = -1; // loc
		
		c.GotoNext(MoveType.After, i => i.MatchCallvirt<PlayerDeathReason>(nameof(PlayerDeathReason.GetDeathText)));
		c.GotoNext(MoveType.After, i => i.MatchStloc(out deathTextIndex));

		c.EmitLdarg0();
		c.EmitLdarg1(); // damageSource
		c.EmitLdloca(deathTextIndex);

		c.EmitDelegate(ReplaceGetDeathTextCall);
	}

	void ReplaceGetDeathTextCall(Player self, PlayerDeathReason damageSource, ref NetworkText deathText)
	{
		if (damageSource.CustomReason == null || damageSource.CustomReason._mode == NetworkText.Mode.Literal)
			return;

		string originalKey = damageSource.CustomReason._text;
		
		PlayerPronoun player = self.GetModPlayer<PlayerPronoun>();
		Pronoun pronoun = player.Pronoun;
		
		string selfPronoun = pronoun.Equals(Pronouns.They)
			? Language.GetTextValue("Mods.PronounsMod.Common.Themselves")
			: Language.GetText("Mods.PronounsMod.Common.Self").Format(pronoun.Object);

		if (SupportedKeysVariant1.Any(originalKey.StartsWith))
		{
			string key = player.ShouldUsePlayerNameDeathFormat() ? originalKey + ".PlayerName" : originalKey;
			LocalizedText localizedDeath = Language.GetText(key);
			
			NetworkText formattedDeathText = localizedDeath.ToNetworkText(self.name, pronoun.Subject, pronoun.Object, pronoun.Possessive, selfPronoun);
			PlayerDeathReason newReason = PlayerDeathReason.ByCustomReason(formattedDeathText);

			deathText = newReason.GetDeathText(self.name);
		}
	}
}