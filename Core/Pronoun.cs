using System;
using System.Diagnostics.CodeAnalysis;
using Humanizer;
using Terraria.Localization;

namespace PronounsMod.Core;

public struct Pronoun : IEquatable<Pronoun>
{
	public bool Equals(Pronoun pronoun)
	{
		return Subject.Equals(pronoun.Subject) && Object.Equals(pronoun.Object) && Possessive.Equals(pronoun.Possessive);
	}

	public void Edit(int type, string newPronoun)
	{
		switch (type)
		{
			case 0: RawSubject = newPronoun; break;
			case 1: RawObject = newPronoun; break;
			case 2: RawPossessive = newPronoun; break;
		}
	}

	public string RawSubject;
	public string RawObject;
	public string RawPossessive;

	public readonly string Subject => Language.Exists(RawSubject) ? Language.GetTextValue(RawSubject) : RawSubject;
	public readonly string Object => Language.Exists(RawObject) ? Language.GetTextValue(RawObject) : RawObject;
	public readonly string Possessive => Language.Exists(RawPossessive) ? Language.GetTextValue(RawPossessive) : RawPossessive;

	public readonly string FullFormat => String.Join("/", [Subject, Object, Possessive]); 
	public readonly string ChatFormat => String.Join("/", [Subject, Object]); 

	public Pronoun(string s, string o, string p, bool localized = false)
	{
		RawSubject = s;
		RawObject = o;
		RawPossessive = p;
	}
}