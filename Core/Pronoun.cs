using System;
using System.Diagnostics.CodeAnalysis;
using Humanizer;
using Terraria.Localization;

namespace PronounsMod.Core;

public struct Pronoun : IEquatable<Pronoun>
{
	public readonly bool Equals(Pronoun pronoun)
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

	public readonly string FullFormat
	{
		get
		{
			if (Subject.Equals(Object) && !Subject.Equals(Possessive))
				return string.Join("/", [Subject, Object, Possessive]);
			
			if (string.IsNullOrWhiteSpace(ChatFormat))
				return Possessive;

			if (string.IsNullOrWhiteSpace(Possessive))
				return ChatFormat;
			
			return String.Join("/", [ChatFormat, Possessive]);
		}
	}

	public readonly string ChatFormat
	{
		get
		{
			if (Subject.Equals(Object) && !Subject.Equals(Possessive))
				return string.Join("/", [Subject, Possessive]);
			
			if (string.IsNullOrWhiteSpace(Subject) && !string.IsNullOrWhiteSpace(Object))
				return Object;
			
			if (!string.IsNullOrWhiteSpace(Subject) && string.IsNullOrWhiteSpace(Object))
				return Subject;
			
			if (string.IsNullOrWhiteSpace(Subject) && string.IsNullOrWhiteSpace(Object))
				return string.Empty;
			
			return String.Join("/", [Subject, Object]);
		}
	}

	public Pronoun(string s, string o, string p)
	{
		RawSubject = s;
		RawObject = o;
		RawPossessive = p;
	}
}