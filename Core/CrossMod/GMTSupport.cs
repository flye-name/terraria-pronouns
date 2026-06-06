using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using GMT.Common;
using GMT.Revamps.ForTheWorthy;
using Hjson;
using Newtonsoft.Json.Linq;
using PronounsMod.Core.Utils;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PronounsMod.Core.CrossMod;

[ExtendsFromMod("GMT")]
public class GMTSupport : ModSystem
{
	private delegate void orig_LoadLanguage(bool force);
	
	public override void Load()
	{
		MethodInfo loadLanguage = typeof(FTWSystem).GetMethod("LoadLanguage", BindingFlags.Static | BindingFlags.Public);
		
		MonoModHooks.Add(loadLanguage, OnLoadLanguage);
	}

	private void OnLoadLanguage(orig_LoadLanguage orig, bool force)
	{
		bool shouldReplace = (Main.getGoodWorld || force) && !CommonSystem.dave;
			
		orig(force);
		
		if (shouldReplace)
			LocalizationUtils.ReplaceLocalization("da-VE");
	}
}