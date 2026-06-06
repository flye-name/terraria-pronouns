using PronounsMod.Core.Utils;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PronounsMod.Core.CrossMod;

[ExtendsFromMod("CalamityMod")]
public class CalamitySupport : ModSystem
{
	public override void OnLocalizationsLoaded()
	{
		if (LocalizationUtils.IsSupported("CalamityMod"))
			LocalizationUtils.ReplaceLocalization(Language.ActiveCulture.Name + "_Calamity");
	}
}