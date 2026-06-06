namespace PronounsMod.Core.Utils;

public static class SocialUtils
{
	public static string FormatWithChatColor(this string str) => $"[c/{Pronouns.Color}:{str}";
}