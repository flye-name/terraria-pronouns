using System.Collections.Generic;
using System.IO;
using System.Text;
using Hjson;
using Newtonsoft.Json.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PronounsMod.Core.CrossMod;

public class DaveLangSupport : ModSystem
{
	public static Dictionary<string, LocalizedText> FullLocalization => LanguageManager.Instance._localizedTexts;
	
	public override void OnLocalizationsLoaded()
	{
		if (!ModLoader.HasMod("GMT") && Language.GetTextValue("LoadingTips_Default.1").Equals("pee pee poo poo"))
		{
			ReplaceDaveLangEntries();
		}
	}

	public static void ReplaceDaveLangEntries()
	{
		using (Stream stream = ModContent.GetInstance<PronounsMod>().GetFileStream(Path.Combine("Localization", "da-VE")))
		{
			using StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
			JObject parsedLocalization = JObject.Parse(HjsonValue.Parse(reader.ReadToEnd()).ToString());

			foreach (JToken entry in parsedLocalization.SelectTokens("$..*"))
			{
				if (!entry.HasValues)
					UpdateLocalizationEntry(entry);
			}
		}
	}

	static void UpdateLocalizationEntry(JToken entry)
	{
		string key = string.Empty;
		JObject? entryObj = entry as JObject;
		JToken currentToken = entry;

		if (entryObj is not null || entryObj?.Count == 0)
			return; 
		
		for (JToken? parent = entry.Parent; parent != null; parent = parent.Parent)
		{
			JProperty? property = parent as JProperty;
			string keyInner;
			if (property is null)
			{
				JArray? array = parent as JArray;
				keyInner = ((array is null) ? key : (array.IndexOf(currentToken) + ((key == string.Empty) ? string.Empty : ("." + key))));
			}
			else
				keyInner = property.Name + ((key == string.Empty) ? string.Empty : ("." + key));

			key = keyInner;
			currentToken = parent;
		}
		
		if (FullLocalization.ContainsKey(key))
			FullLocalization[key].SetValue(entry.ToString());
		else
			FullLocalization.Add(key, new LocalizedText(key, entry.ToString()));
	}
}