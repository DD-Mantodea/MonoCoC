using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes.Vanilla.Hel;
using MonoCoC.Extensions;
using MonoCoC.Configuration;

namespace MonoCoC.Contents.Scenes.Vanilla.Dungeons.HelDungeon
{
    [Register<BrigidScene>("coc:brigid_scene")]
    public class BrigidScene : Scene
    {
        private const string K = "Scenes.Brigidscene.";

        public override void Entry()
        {
            PcDefeatedByBrigid();
        }

        public void PcDefeatsBrigid(bool hpVictory)
        {
            ClearOutput();

            OutputText((K + "PcDefeatsBrigid.JailerCollapses").Localized());

            if (hpVictory)
                OutputText((K + "PcDefeatsBrigid.TooBadlyBeaten").Localized());
            else
                OutputText((K + "PcDefeatsBrigid.TooTurnedOn").Localized());

            OutputText((K + "PcDefeatsBrigid.SnatchesWeapon").Localized());

            Player.CreateKeyItem("Harpy Key A", 0, 0, 0, 0);

            Flags.Set(GameFlagIds.HelBrigidDefeated, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void PcDefeatedByBrigid()
        {
            ClearOutput();

            if (Flags.Get(GameFlagIds.HelHarpyQueenDefeated) == 0)
            {
                OutputText((K + "PcDefeatedByBrigid.TskTskCroon").Localized());

                DoNext(() => SceneInstance<HelDungeonScene>.Get().HarpyQueenBeatsUpPCBadEnd());
            }
            else
            {
                OutputText((K + "PcDefeatedByBrigid.ThatsItScream").Localized());

                OutputText((K + "PcDefeatedByBrigid.HelCalls").Localized());

                OutputText((K + "PcDefeatedByBrigid.HelLeaps").Localized());

                OutputText((K + "PcDefeatedByBrigid.PatheticRant").Localized());

                OutputText((K + "PcDefeatedByBrigid.WhereTheFu").Localized());

                OutputText((K + "PcDefeatedByBrigid.AlarmBlares").Localized());

                DoNext(BrigitPostQueenDeathBadEndII);
            }
        }

        private void BrigitPostQueenDeathBadEndII()
        {
            ClearOutput();

            if (Player.Gender <= Gender.Male || Player.Pregnancy.Incubation > 0)
            {
                OutputText((K + "BrigitPostQueenDeathBadEndII.TicklishChest").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.KiriWhispers").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.OthersAsk").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.HarpyFrowns").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.FirstPick").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.SharpCrack").Localized());

                if (!Player.HasCock())
                {
                    OutputText((K + "BrigitPostQueenDeathBadEndII.EyesWiden").Localized());

                    if (Player.Pregnancy.Incubation > 0)
                        OutputText((K + "BrigitPostQueenDeathBadEndII.DisgustingThing").Localized());

                    OutputText((K + "BrigitPostQueenDeathBadEndII.ObviousMessage").Localized());

                    OutputText((K + "BrigitPostQueenDeathBadEndII.SqueezesAss").Localized());

                    Player.CreateCock();
                }

                OutputText((K + "BrigitPostQueenDeathBadEndII.SlapsHalfBreed").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.StrokesPrick").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.EyesRollBack").Localized());

                if (Player.CockTotal() > 1 || Player.CumQ() > 1000)
                    OutputText((K + "BrigitPostQueenDeathBadEndII.HoldsForth").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.SalamanderBeside").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.KiriWhimpers").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.NotBadQuip").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.FrownsUnhappily").Localized());

                GameOver();

                if (OptionStore.Instance.Get("hardcore", 0) == 0)
                    AddButton(1, "Common.Button.Retry".Localized(), () => SceneInstance<HelDungeonScene>.Get().RetryDungeonFromBadEndPrompt());
            }
            else
            {
                Player.CuntChange(15, false, false, false);

                OutputText((K + "BrigitPostQueenDeathBadEndII.HeatFirst").Localized());

                if (Player.HasCock())
                    OutputText((K + "BrigitPostQueenDeathBadEndII.BobbingDrooling").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.PussyPounded").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.RealizesShift").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.TalkLessHowl").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.KiriScowls").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.MuffledCry").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.DickTwitches").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.DidntComeTaunt").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.ShovePinkJailer").Localized());

                if (Player.HasCock())
                    OutputText((K + "BrigitPostQueenDeathBadEndII.PhoenixCunt").Localized());

                OutputText((K + "BrigitPostQueenDeathBadEndII.BrigidLeaves").Localized());

                GameOver();

                if (OptionStore.Instance.Get("hardcore", 0) == 0)
                    AddButton(1, "Common.Button.Retry".Localized(), () => SceneInstance<HelDungeonScene>.Get().RetryDungeonFromBadEndPrompt());
            }
        }
    }
}