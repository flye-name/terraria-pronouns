using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace PronounsMod.Core.Config;

public enum PronounFormat : byte
{
	Normal,
	Short,
	None
}

public class FormatConfig : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ClientSide;

	[CyclicalTextEnumAttribute<PronounFormat>, DrawTicks, DefaultValue(PronounFormat.Normal)]
	public PronounFormat Format;
}