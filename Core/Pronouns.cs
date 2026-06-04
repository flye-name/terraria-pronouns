using System.Collections.Generic;
using Terraria.Localization;

namespace PronounsMod.Core;

public static class Pronouns
{
	public static readonly LocalizedText Any = Language.GetText("Mods.PronounsMod.Any");
	
	public static readonly Pronoun None = new("  ", "  ", "  ");
	public static readonly Pronoun They = new("they", "them", "their");
	public static readonly Pronoun She = new("she", "her", "her");
	public static readonly Pronoun He = new("he", "him", "his");
	public static readonly Pronoun It = new("it", "it", "its");
	public static readonly string AnyPreview = "(any)";

	public static readonly List<Pronoun> Neo = new();
}