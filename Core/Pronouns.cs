using System.Collections.Generic;
using Terraria.Localization;

namespace PronounsMod.Core;

public static class Pronouns
{
	public static readonly string Color = "b2aacc";
	public static readonly string Key = "Mods.PronounsMod.Presets.";
	public static readonly LocalizedText Any = Language.GetText("Mods.PronounsMod.Common.Any");
	
	public static readonly Pronoun None = new("", "", "");
	public static readonly Pronoun They = new(Key + "They.Subject", Key + "They.Object", Key + "They.Possessive");
	public static readonly Pronoun She = new(Key + "She.Subject", Key + "She.Object", Key + "She.Possessive");
	public static readonly Pronoun He = new(Key + "He.Subject", Key + "He.Object", Key + "He.Possessive");
	public static readonly Pronoun It = new(Key + "It.Subject", Key + "It.Object", Key + "It.Possessive");
}