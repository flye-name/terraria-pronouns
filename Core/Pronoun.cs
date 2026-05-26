using System;

namespace PronounsMod.Core;

public readonly struct Pronoun
{
	public readonly string Subject { get; }
	public readonly string Object { get; }
	public readonly string Possessive { get; }

	public readonly string FullFormat => String.Join("/", [Subject, Object, Possessive]); 

	public Pronoun(string s, string o, string p)
	{
		Subject = s;
		Object = o;
		Possessive = p;
	}
	
	public Pronoun(string[] packed)
	{
		Subject = packed[0];
		Object = packed[1];
		Possessive = packed[2];
	}
}