using MonoMod.Cil;
using PronounsMod.Core.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PronounsMod.Core.Chat;

// TODO:
// - Respect other mods that may add more arguments to vanilla death messages
// - Support for other languages
public sealed class DeathMessages : ModSystem
{
    private static int targetPlayer;

    public override void Load()
    {
        On_Player.KillMe += KillMe_CapturePlayer;
        IL_Lang.CreateDeathMessage += CreateDeathMessage_Pronouns;
    }

    private void KillMe_CapturePlayer(On_Player.orig_KillMe orig, Player self, PlayerDeathReason damageSource, double dmg, int hitDirection, bool pvp)
    {
        targetPlayer = self.whoAmI;

        orig(self, damageSource, dmg, hitDirection, pvp);
    }

    private void CreateDeathMessage_Pronouns(ILContext il)
    {
        var c = new ILCursor(il);

#region DeathTextGeneric
        c.GotoNext(
            MoveType.After,
            i => i.MatchLdstr("DeathTextGeneric"),
            i => i.MatchLdnull(),
            i => i.MatchCall(typeof(Language), nameof(Language.RandomFromCategory)),
            i => i.MatchLdfld<LocalizedText>(nameof(LocalizedText.Key))
        );

        c.EmitDelegate(AppendPlayerNameKey);

        c.GotoNext(
            MoveType.After,
            i => i.MatchLdcI4(2)
        );

        c.EmitPop();

        c.EmitLdcI4(5);

        c.GotoNext(
            MoveType.After,
            i => i.MatchLdsfld<Main>(nameof(Main.worldName)),
            i => i.MatchStelemRef()
        );

        c.EmitDelegate(
            static (object[] parameters) =>
            {
                Player player = Main.player[targetPlayer];
                var modPlayer = player.GetModPlayer<PlayerPronoun>();

                parameters[2] = modPlayer.Pronoun.Subject;
                parameters[3] = modPlayer.Pronoun.Object;
                parameters[4] = modPlayer.Pronoun.Possessive;

                return parameters;
            }
        );
#endregion

#region DeathText
        c.GotoNext(
            MoveType.After,
            i => i.MatchLdstr("DeathText.Fell_")
        );

        c.GotoNext(
            MoveType.After,
            i => i.MatchCall<string>(nameof(string.Concat))
        );

        c.EmitDelegate(AppendPlayerNameKey);

        c.GotoNext(
            MoveType.After,
            i => i.MatchLdcI4(1)
        );

        c.EmitPop();

        c.EmitLdcI4(4);

        c.GotoNext(
            MoveType.After,
            i => i.MatchStelemRef()
        );

        c.EmitDelegate(
            static (object[] parameters) =>
            {
                Player player = Main.player[targetPlayer];
                var modPlayer = player.GetModPlayer<PlayerPronoun>();

                parameters[1] = modPlayer.Pronoun.Subject;
                parameters[2] = modPlayer.Pronoun.Object;
                parameters[3] = modPlayer.Pronoun.Possessive;

                return parameters;
            }
        );

        string[] keys = [
            "DeathText.Teleport_2_Male",
            "DeathText.Teleport_2_Female"
        ];

        foreach (string key in keys)
        {
            c.GotoNext(
                MoveType.After,
                i => i.MatchLdstr(key)
            );

            c.EmitDelegate(AppendPlayerNameKey);

            c.GotoNext(
                MoveType.After,
                i => i.MatchLdcI4(1)
            );

            c.EmitPop();

            c.EmitLdcI4(4);

            c.GotoNext(
                MoveType.After,
                i => i.MatchStelemRef()
            );

            c.EmitDelegate(
                static (object[] parameters) =>
                {
                    Player player = Main.player[targetPlayer];
                    var modPlayer = player.GetModPlayer<PlayerPronoun>();

                    parameters[1] = modPlayer.Pronoun.Subject;
                    parameters[2] = modPlayer.Pronoun.Object;
                    parameters[3] = modPlayer.Pronoun.Possessive;

                    return parameters;
                }
            );
        }
#endregion

        return;

        static string AppendPlayerNameKey(string key)
        {
            const string player_name_key = "PlayerName";

            Player player = Main.player[targetPlayer];

            if (!player.TryGetModPlayer<PlayerPronoun>(out var modPlayer)
             || modPlayer.Mode != PronounMode.PlayerName
             || !Language.Exists($"{key}.{player_name_key}"))
            {
                return key;
            }

            return $"{key}.{player_name_key}";
        }
    }
}
