using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Desert
{
    [Register<OasisShamanScene>("coc:oasis_shaman")]
    public class OasisShamanScene : Scene
    {

        public bool didEncounter = false;

        public bool questioned = false;

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
{
    ["nude"] = "bare, exposed body",
    ["straps"] = "solid black stockings and leather chest-wraps",
    ["leaf"] = "leafy wrappings",
    ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
};

        public static SaveContent Save => SaveContent(SceneIds.Desert.OasisShaman);

        public override void Entry()
        {
            OasisShamanEncounter();
        }

        public void OasisShamanEncounter()
        {
            ClearOutput();

            questioned = false;

            Save.SetBool("didEncounter", true);

            RegisterMacro("beautifulsword", Player.Weapon.Id == WeaponIds.BeautifulSword);

            RegisterMacro("question", questioned);

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            OutputText("Scenes.Oasisshamanscene.OasisShamanEncounter.ScorchingSand".Localized());

            OutputText("Scenes.Oasisshamanscene.OasisShamanEncounter.MirageStill".Localized());

            OutputText("Scenes.Oasisshamanscene.OasisShamanEncounter.ManSoaking".Localized());

            OutputText("Scenes.Oasisshamanscene.OasisShamanEncounter.NoticesNudity".Localized());

            Player.HasGottenWashed();

            ClearButtons();

            if (Player.SexOrientation <= 10)
                AddNextButton("Common.Button.Reject".Localized(), RejectOasisShaman);

            AddNextButton("Common.Button.Question".Localized(), QuestionOasisShaman);

            AddNextButton("Common.Button.Flirt".Localized(), () => FlirtOasisShaman(false));
        }

        public void RejectOasisShaman()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            OutputText("Scenes.Oasisshamanscene.RejectOasisShaman.FindsAttractive".Localized());

            OutputText("Scenes.Oasisshamanscene.RejectOasisShaman.NodsUnderstanding".Localized());

            ClearButtons();

            if (Player.Lust > 50)
                AddNextButton("Common.Button.Swoon".Localized(), () => FlirtOasisShaman(true));

            AddNextButton("Common.Button.Question".Localized(), QuestionOasisShaman);

            AddNextButton("Scenes.Akbal.RepeatAkbalPostSubmission.ButtonDeny".Localized(), DenyOasisShaman);
        }

        public void QuestionOasisShaman()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            OutputText("Scenes.Oasisshamanscene.QuestionOasisShaman.NotOnlyOne".Localized());

            OutputText("Scenes.Oasisshamanscene.QuestionOasisShaman.PiquesCuriosity".Localized());

            OutputText("Scenes.Oasisshamanscene.QuestionOasisShaman.FitBody".Localized());

            questioned = true;

            ClearButtons();

            AddNextButton("Common.Button.Flirt".Localized(), () => FlirtOasisShaman(false));

            AddNextButton("Common.Button.Spirit".Localized(), SpiritOasisShaman);

            AddNextButton("Scenes.Akbal.RepeatAkbalPostSubmission.ButtonDeny".Localized(), DenyOasisShaman);
        }

        public void FlirtOasisShaman(bool swoon)
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            RegisterMacro("swoon",swoon);

            if (swoon)
                OutputText("Scenes.Oasisshamanscene.FlirtOasisShaman.WilesMelt".Localized());
            else
                if (questioned)
                    OutputText("Scenes.Oasisshamanscene.FlirtOasisShaman.SpiritOasis".Localized());
                else
                    OutputText("Scenes.Oasisshamanscene.FlirtOasisShaman.Unanticipated".Localized());

            OutputText("Scenes.Oasisshamanscene.FlirtOasisShaman.TurnsFlirtatious".Localized());

            DoNext(() => SubmitOasisShaman());
        }

        public void SubmitOasisShaman()
        {
            ClearOutput();

            OutputText("Scenes.Oasisshamanscene.SubmitOasisShaman.LecherousGrins".Localized());

            OutputText("Scenes.Oasisshamanscene.SubmitOasisShaman.GlamourWord".Localized());

            OutputText("Scenes.Oasisshamanscene.SubmitOasisShaman.CockPulsates".Localized());

            OutputText("Scenes.Oasisshamanscene.SubmitOasisShaman.SlidesDown".Localized());

            ClearButtons();

            AddNextButton("Common.Button.GetFucked".Localized(), () => FuckedByOasisShaman(0));

            AddNextButton("Common.Button.Passive".Localized(), () => FuckedByOasisShaman(1));

            AddNextButton("Common.Button.Active".Localized(), () => FuckedByOasisShaman(2));
        }

        public void FuckedByOasisShaman(int type)
        {
            ClearOutput();

            if (type == 0)
                OutputText("Scenes.Oasisshamanscene.FuckedByOasisShaman.WringsDick".Localized());
            else
            {
                if (type == 1)
                    OutputText("Scenes.Oasisshamanscene.FuckedByOasisShaman.FlexibleFlesh".Localized());
                else
                {
                    OutputText("Scenes.Oasisshamanscene.FuckedByOasisShaman.PullsTail".Localized());

                    OutputText("Scenes.Oasisshamanscene.FuckedByOasisShaman.WrapsArm".Localized());
                }

                OutputText("Scenes.Oasisshamanscene.FuckedByOasisShaman.WithdrawsLeaving".Localized());
            }

            OutputText("Scenes.Oasisshamanscene.FuckedByOasisShaman.NeckGlows".Localized());

            OutputText("Scenes.Oasisshamanscene.FuckedByOasisShaman.WaterLungs".Localized());

            if (Player.HasVagina())
                Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);
            else
                Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            OutputText("Scenes.Oasisshamanscene.FuckedByOasisShaman.EverythingNumb".Localized());

            OutputText("Scenes.Oasisshamanscene.FuckedByOasisShaman.BlearyFloating".Localized());

            OutputText("Scenes.Oasisshamanscene.FuckedByOasisShaman.SlidesSlowly".Localized());

            OutputText("Scenes.Oasisshamanscene.FuckedByOasisShaman.FloodCum".Localized());

            DoNext(() => FuckedByOasisShamanNext());
        }

        public void FuckedByOasisShamanNext()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f), new DynStat(GameStats.Libido, 5f));

            Player.SetUndergarment(Undergarment.Nothing, Undergarment.TypeUpperwear);

            OutputText("Scenes.Oasisshamanscene.FuckedByOasisShamanNext.BlinkingLooksUp".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SpiritOasisShaman()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            OutputText("Scenes.Oasisshamanscene.SpiritOasisShaman.StaysOnTopic".Localized());

            OutputText("Scenes.Oasisshamanscene.SpiritOasisShaman.HandTouches".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Submit".Localized(), SubmitOasisShaman);

            AddNextButton("Common.Button.Resist".Localized(), ResistOasisShaman);
        }

        public void ResistOasisShaman()
        {
            ClearOutput();

            OutputText("Scenes.Oasisshamanscene.ResistOasisShaman.MusteringWill".Localized());

            OutputText("Scenes.Oasisshamanscene.ResistOasisShaman.GrimacesFrustration".Localized());

            OutputText("Scenes.Oasisshamanscene.ResistOasisShaman.StayInWater".Localized());

            var enemy = EnemyRegistry.Create(EnemyIds.OasisShaman);

            enemy.OnDefeated = hpVictory => WinCombatOasisShaman(hpVictory);

            enemy.OnWon = hpVictory => LostToOasisShaman();

            CombatScene.Start(enemy, () => Go<CampScene>());
        }

        public void DenyOasisShaman()
        {
            ClearOutput();

            OutputText("Scenes.Oasisshamanscene.DenyOasisShaman.WhateverFantasies".Localized());

            OutputText("Scenes.Oasisshamanscene.DenyOasisShaman.IllusionFades".Localized());

            var enemy = EnemyRegistry.Create(EnemyIds.OasisShaman);

            enemy.OnDefeated = hpVictory => WinCombatOasisShaman(hpVictory);

            enemy.OnWon = hpVictory => LostToOasisShaman();

            CombatScene.Start(enemy, () => Go<CampScene>());
        }

        public void WinCombatOasisShaman(bool hpVictory)
        {
            ClearOutput();

            RegisterMacro("lustwon", !hpVictory);

            Player.SetUndergarment(Undergarment.Nothing, Undergarment.TypeUpperwear);

            OutputText("Scenes.Oasisshamanscene.WinCombatOasisShaman.DemonGivesIn".Localized());

            OutputText("Scenes.Oasisshamanscene.WinCombatOasisShaman.AlteredState".Localized());

            ClearButtons();

            AddNextButton("Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonRideHim".Localized(), RideOasisShaman);

            AddNextButton("Common.Button.FuckHim".Localized(), FuckOasisShaman);

            AddNextButton("Common.Button.Kill".Localized(), KillOasisShaman);

            AddButton(14, "Common.Button.Leave".Localized(), () => LeaveOasisShamanAfterCombat());

        }

        public void KillOasisShaman()
        {
            ClearOutput();

            if (Player.Weapon.Id == WeaponIds.DullScimitar)
                OutputText("Scenes.Oasisshamanscene.KillOasisShaman.HoldsScythe".Localized());
            else
                if (Player.Weapon.IsKatana())
                    OutputText("Scenes.Oasisshamanscene.KillOasisShaman.CalmsBreath".Localized());
                else
                    if (Player.Weapon.IsChanneling(Player))
                        OutputText("Scenes.Oasisshamanscene.KillOasisShaman.MindClearer".Localized());
                    else
                        if (Player.Weapon.IsAxe() || Player.Weapon.IsScythe() || Player.Weapon.IsBlunt())
                            OutputText("Scenes.Oasisshamanscene.KillOasisShaman.TwirlsTail".Localized((Player.Weapon.IsBlunt() ? "cracking the demon's skull" : "cleaving into the demon's neck")));
                        else
                            if (Player.Weapon.IsStabby())
                                OutputText("Scenes.Oasisshamanscene.KillOasisShaman.DashesWater".Localized());
                            else
                                OutputText("Scenes.Oasisshamanscene.KillOasisShaman.WellAimed".Localized());

            OutputText("Scenes.Oasisshamanscene.KillOasisShaman.LessHazard".Localized());

            Player.UpgradeBeautifulSword();

            CombatScene.CleanupAfterCombat();
        }

        public void LeaveOasisShamanAfterCombat()
        {
            ClearOutput();

            OutputText("Scenes.Oasisshamanscene.LeaveOasisShamanAfterCombat.DragsSelf".Localized());

            OutputText("Scenes.Oasisshamanscene.LeaveOasisShamanAfterCombat.ClaspsSide".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void RideOasisShaman()
        {
            ClearOutput();

            OutputText("Scenes.Oasisshamanscene.RideOasisShaman.FloatsDazed".Localized());

            OutputText("Scenes.Oasisshamanscene.RideOasisShaman.GlidesCloser".Localized());

            OutputText("Scenes.Oasisshamanscene.RideOasisShaman.TrailsDownward".Localized());

            OutputText("Scenes.Oasisshamanscene.RideOasisShaman.UnthinkingPurr".Localized());

            OutputText("Scenes.Oasisshamanscene.RideOasisShaman.AnticipationCrackles".Localized());

            if (Player.HasVagina())
                Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);
            else
                Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            OutputText("Scenes.Oasisshamanscene.RideOasisShaman.PulsateWater".Localized());

            OutputText("Scenes.Oasisshamanscene.RideOasisShaman.FeedingInto".Localized());

            OutputText("Scenes.Oasisshamanscene.RideOasisShaman.TensionHeat".Localized());

            OutputText("Scenes.Oasisshamanscene.RideOasisShaman.WaterCrashes".Localized());

            OutputText("Scenes.Oasisshamanscene.RideOasisShaman.AftershocksFade".Localized());

            OutputText("Scenes.Oasisshamanscene.RideOasisShaman.UnfurlsTail".Localized());

            Player.SlimeFeed();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f), new DynStat(GameStats.Libido, 5f));

            if (Player.HasVagina())
            {
                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation, 90);

                Player.Orgasm("VaginalAndDick");
            }
            else
                Player.Orgasm("DickAndAnal");

            CombatScene.CleanupAfterCombat();
        }

        public void FuckOasisShaman()
        {
            ClearOutput();

            OutputText("Scenes.Oasisshamanscene.FuckOasisShaman.FloatsDazed".Localized());

            OutputText("Scenes.Oasisshamanscene.FuckOasisShaman.GlidesCloser".Localized());

            OutputText("Scenes.Oasisshamanscene.FuckOasisShaman.ExploresBody".Localized());

            OutputText("Scenes.Oasisshamanscene.FuckOasisShaman.BitesLip".Localized());

            OutputText("Scenes.Oasisshamanscene.FuckOasisShaman.SlowPace".Localized());

            OutputText("Scenes.Oasisshamanscene.FuckOasisShaman.LeansIn".Localized());

            OutputText("Scenes.Oasisshamanscene.FuckOasisShaman.TremblingTaut".Localized());

            OutputText("Scenes.Oasisshamanscene.FuckOasisShaman.AftershocksFade".Localized());

            OutputText("Scenes.Oasisshamanscene.FuckOasisShaman.UnfurlsTail".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f), new DynStat(GameStats.Libido, 2f));

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void LostToOasisShaman()
        {
            ClearOutput();

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.WornFloats".Localized());

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.KnowingSmile".Localized());

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.HotTonedBody".Localized());

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.BreathGhosting".Localized());

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.WithoutWord".Localized());

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.GroansLaced".Localized());

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.PunishingPace".Localized());

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.HeatPools".Localized());

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.ThereItIs".Localized());

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.GutTwists".Localized());

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.LetsGo".Localized());

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.MercilessConquest".Localized());

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.MurmursPossessive".Localized());

            OutputText("Scenes.Oasisshamanscene.LostToOasisShaman.AwakesHoursLater".Localized());

            Player.SlimeFeed();

            Player.SetUndergarment(Undergarment.Nothing, Undergarment.TypeUpperwear);

            if (Player.HasVagina())
            {
                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation, 90);

                Player.Orgasm("VaginalAndDick");
            }
            else
                Player.Orgasm("DickAndAnal");

            CombatScene.CleanupAfterCombat();
        }

    }
}