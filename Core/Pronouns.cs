using System.Collections.Generic;
using Terraria.Localization;

namespace PronounsMod.Core;

public static class Pronouns
{
	public static readonly string Key = "Mods.PronounsMod.Presets.";
	public static readonly LocalizedText Any = Language.GetText("Mods.PronounsMod.Any");
	
	public static readonly Pronoun None = new("  ", "  ", "  ");
	public static readonly Pronoun They = new(Key + "They.S", Key + "They.O", Key + "They.P", true);
	public static readonly Pronoun She = new(Key + "She.S", Key + "She.O", Key + "She.P", true);
	public static readonly Pronoun He = new(Key + "He.S", Key + "He.O", Key + "He.P", true);
	public static readonly Pronoun It = new(Key + "It.S", Key + "It.O", Key + "It.P", true);
	public static readonly string AnyPreview = $"{Language.GetText("Mods.PronounsMod.Any")}";
}