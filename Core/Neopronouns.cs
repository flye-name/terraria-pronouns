using System;
using System.IO;
using System.Linq;
using System.Text;
using log4net.Core;
using Terraria.ModLoader;

namespace PronounsMod.Core;

public class Neopronouns : ModSystem
{
	private const string HEADER = "DO NOT REMOVE ANY ENTRIES OR INSERT NEW ONES. Existing entries can be edited as long as they stay in the same format of subject/object/possessive and don't exceed 60 characters in total (including the slashes).";
	public static string DirPath => Path.Combine(ModLoader.ModPath, "PronounsMod");
	public static string FilePath => Path.Combine(DirPath, "neopronouns.txt");
	
	public override void Load()
	{
		if (Path.Exists(FilePath))
		{
			FillNeopronouns();
		}
		else
		{
			Directory.CreateDirectory(DirPath);
			WriteNeopronouns();
		}
	}

	private void WriteNeopronouns()
	{
		try
		{
			using StreamWriter writer = new StreamWriter(FilePath, false, new UTF8Encoding(true));
			writer.WriteLine(HEADER);
			foreach (var pronouns in Pronouns.Neo)
			{
				string[] pronoun = [pronouns.Subject, pronouns.Object, pronouns.Possessive];
				if (pronoun.Any((_) => String.IsNullOrEmpty(_)))
					continue;
				
				string line = String.Join("/", pronoun);
				
				writer.WriteLine(line);
			}
		}
		catch (Exception e)
		{
			Mod.Logger.Logger.Log(typeof(Neopronouns), Level.Error, "File could not be written to: ", e);
			throw;
		}
	}

	private void FillNeopronouns()
	{
		Pronouns.Neo.Clear();
		
		try
		{
			using StreamReader reader = new StreamReader(FilePath);
			reader.ReadLine(); // Skip header
			
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				if (line.Length < 6 || line.Length > 60)
					continue;
				
				string[] pronoun = line.Split("/");

				if (pronoun.Length == 3)
					Pronouns.Neo.Add(new Pronoun(pronoun));
			}
		}
		catch (Exception e)
		{
			Mod.Logger.Logger.Log(typeof(Neopronouns), Level.Error, "File could not be read: ", e);
			throw;
		}
	}
}