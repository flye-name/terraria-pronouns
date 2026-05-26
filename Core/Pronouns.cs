using System.Collections.Generic;

namespace PronounsMod.Core;

public static class Pronouns
{
	public static readonly Pronoun TheyThem = new("they", "them", "their");
	public static readonly Pronoun SheHer = new("she", "her", "her");
	public static readonly Pronoun HeHim = new("he", "him", "his");

	public static readonly List<Pronoun> Neo = new();
}