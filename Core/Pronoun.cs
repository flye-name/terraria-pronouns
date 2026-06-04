using System;
using System.Diagnostics.CodeAnalysis;
using Humanizer;

namespace PronounsMod.Core;

public struct Pronoun : IEquatable<Pronoun>
{
	public bool Equals(Pronoun pronoun)
	{
		return Subject.Equals(pronoun.Subject) && Object.Equals(pronoun.Object) && Possessive.Equals(pronoun.Possessive);
	}

	public string Subject;
	public string Object;
	public string Possessive;

	public readonly string FullFormat => String.Join("/", [Subject, Object, Possessive]); 
	public readonly string ChatFormat => String.Join("/", [Subject, Object]); 

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