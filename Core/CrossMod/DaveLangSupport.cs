using System.Collections.Generic;
using System.IO;
using System.Text;
using Hjson;
using Newtonsoft.Json.Linq;
using PronounsMod.Core.Utils;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PronounsMod.Core.CrossMod;

public class DaveLangSupport : ModSystem
{	
	public override void OnLocalizationsLoaded()
	{
		if (!ModLoader.HasMod("GMT") && Language.GetTextValue("LoadingTips_Default.1").Equals("pee pee poo poo"))
		{
			LocalizationUtils.ReplaceLocalization("da-VE");
		}
	}
}