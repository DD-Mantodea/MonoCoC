using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Lake
{
    [Register<FetishZealotScene>("coc:fetish_zealot")]
    public class FetishZealotScene : Scene
    {

        public FetishZealotEnemy currentZealot = null!;

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
{
    ["nude"] = "bare, exposed body",
    ["straps"] = "solid black stockings and leather chest-wraps",
    ["leaf"] = "leafy wrappings",
    ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
};

        public override void Entry()
        {
            GenerateZealot();
        }

        public void GenerateZealot()
        {

            currentZealot = (FetishZealotEnemy)EnemyRegistry.Create(EnemyIds.FetishZealot);
        }

        public void RegisterMacros()
        {

            RegisterMacro("shotazealot", currentZealot.ShotaZealot);

            RegisterMacro("zealotskin", () => currentZealot.Body.Get<SkinPart>().Tone);

            RegisterMacro("zealothair", () => currentZealot.Body.Get<HairPart>().Color);
        }

        public void ZealotBoat()
        {
            ClearOutput();

            GenerateZealot();

            RegisterMacros();

            if (Player.StatusEffectValue(StatusEffectIds.FetishOn, 0) == 1)
                OutputText("Scenes.Fetishzealotscene.ZealotBoat.NoTrespassing".Localized());
            else
            {
                OutputText("Scenes.Fetishzealotscene.ZealotBoat.SpotsBoat".Localized());

                Player.ChangeStatusValue(StatusEffectIds.FetishOn, 1, 1);
            }

            currentZealot.OnDefeated = hpVictory => ZealotDefeated();

            currentZealot.OnWon = hpVictory => ZealotLossRape();

            CombatScene.Start(currentZealot, () => Go<CampScene>());
        }

        public void RapeDrain()
        {

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -1f), new DynStat(GameStats.Corruption, 2f));
        }

        public bool BadEndCheck()
        {

            return Player.Intelligence < 10 && Random.Shared.Next(100) < (50);
        }

        public void ZealotLossRape()
        {
            ClearOutput();

            if (Player.Health < 1)
                OutputText("Scenes.Fetishzealotscene.ZealotLossRape.CollapsesPain".Localized());
            else
                OutputText("Scenes.Fetishzealotscene.ZealotLossRape.FetishImages".Localized());

            switch (currentZealot.ArmorName)
            {
                case FetishZealotEnemy.StudentsClothes:
                {
                StudentRape();
                }

                break;
            }

            if (true )
                ReligiousRape();

            RapeDrain();

            bool badEnd = BadEndCheck();

            switch (currentZealot.ArmorName)
            {
                case FetishZealotEnemy.StudentsClothes:
                {
                if (badEnd) StudentRapeFinishBad(); else StudentRapeFinishGood();
                }

                break;
            }

            if (true )
                if (badEnd) ReligiousRapeFinishBad(); else ReligiousRapeFinishGood();
            if (badEnd)
                Go<FetishCultistScene>();
            else
            {
                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Corruption / 20 + Player.Libido / 10)));

                CombatScene.CleanupAfterCombat();
            }
        }

        public void ReligiousRape()
        {

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.StumblesCollide".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.MyChild".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.Politeness".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.StillCleansed".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.StoneMarble".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.SurfaceWord".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.StoneCool".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.PriestNods".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.GhostingShape".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.HandWraps".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.PouringSound".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.SacredOils".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.LoosensRelaxes".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.ZealotWhispers".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.NoWait".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.NoResist".Localized());

            if (Player.HasVagina())
                Player.CuntChange(currentZealot.CockArea(0), true);
            else
                Player.ButtChange(currentZealot.CockArea(0), true);

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.NotLetEvil".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.ZealotPace".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.HeTries".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.PrayerStrength".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.CriesHilting".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRape.StillNeedMore".Localized());

            Player.Orgasm("VaginalAnal");

            Player.SlimeFeed();
        }

        public void ReligiousRapeFinishGood()
        {

            OutputText("Scenes.Fetishzealotscene.ReligiousRapeFinishGood.SlippingShut".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRapeFinishGood.CathedralGone".Localized());
        }

        public void ReligiousRapeFinishBad()
        {

            OutputText("Scenes.Fetishzealotscene.ReligiousRapeFinishBad.AflutterResplendence".Localized());

            OutputText("Scenes.Fetishzealotscene.ReligiousRapeFinishBad.DoesntWake".Localized());
        }

        public void StudentRape()
        {

            if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                Player.ModArmorName = Player.Mf("formal vest, tie, and crotchless pants", "backless female teacher's clothes");

            OutputText("Scenes.Fetishzealotscene.StudentRape.SnapsAttention".Localized());

            OutputText("Scenes.Fetishzealotscene.StudentRape.SmoothsClothes".Localized());

            OutputText("Scenes.Fetishzealotscene.StudentRape.Nervousness".Localized());

            OutputText("Scenes.Fetishzealotscene.StudentRape.DisbeliefCovers".Localized());

            if (Player.IsFeminine())
            {
                OutputText("Scenes.Fetishzealotscene.StudentRape.TakesBreath".Localized());

                OutputText("Scenes.Fetishzealotscene.StudentRape.StudentBlushes".Localized());

                OutputText("Scenes.Fetishzealotscene.StudentRape.FlinchesCup".Localized());

                OutputText("Scenes.Fetishzealotscene.StudentRape.DizzyWithLust".Localized());

                OutputText("Scenes.Fetishzealotscene.StudentRape.FlushHeat".Localized());
            }
            else
            {
                OutputText("Scenes.Fetishzealotscene.StudentRape.TakesBreathAgain".Localized());

                OutputText("Scenes.Fetishzealotscene.StudentRape.FlinchesReach".Localized());

                OutputText("Scenes.Fetishzealotscene.StudentRape.CouldITry".Localized());

                if (Player.HasCock() || Player.GetClitLength() > 2.5f)
                    OutputText("Scenes.Fetishzealotscene.StudentRape.NormallyMan".Localized());
                else
                    OutputText("Scenes.Fetishzealotscene.StudentRape.DespiteSeem".Localized());

                OutputText("Scenes.Fetishzealotscene.StudentRape.WouldBeLying".Localized());
            }

            OutputText("Scenes.Fetishzealotscene.StudentRape.GreatRelief".Localized());

            OutputText("Scenes.Fetishzealotscene.StudentRape.DazeWorsens".Localized());

            Player.Orgasm(Player.HasCock() ? "Dick" : "VaginalAnal");
        }

        public void StudentRapeFinishGood()
        {

            OutputText("Scenes.Fetishzealotscene.StudentRapeFinishGood.HoursLater".Localized());

            if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                OutputText("Scenes.Fetishzealotscene.StudentRapeFinishGood.StillHave".Localized());
            else
                OutputText("Scenes.Fetishzealotscene.StudentRapeFinishGood.ArmorBack".Localized());

            OutputText("Scenes.Fetishzealotscene.StudentRapeFinishGood.OrdealLeft".Localized());
        }

        public void StudentRapeFinishBad()
        {

            OutputText("Scenes.Fetishzealotscene.StudentRapeFinishBad.MindUnable".Localized());
        }

        public void ZealotDefeated()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Fetishzealotscene.ZealotDefeated.CollapsesWounds".Localized());
            else
                OutputText("Scenes.Fetishzealotscene.ZealotDefeated.QuiversMoment".Localized());

            if (Player.Lust >= 33)
                OutputText("Scenes.Fetishzealotscene.ZealotDefeated.TakeAdvantage".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Penance".Localized(), ZealotWinReligious);

            if (Player.HasVagina() && Player.BiggestBreastSize() >= 4 && Player.Armor?.Id == ArmorIds.LightMaidenArmor)
                AddButton(1, "Common.Button.BikiniTitfuck".Localized(), () => Contents.Items.Behaviors.Vanilla.LustyMaidensArmorBehavior.Paizuri(CombatController.Instance!.Context.Enemy));

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void ZealotWinReligious()
        {
            ClearOutput();

            OutputText("Scenes.Fetishzealotscene.ZealotWinReligious.ShimmerFades".Localized());

            OutputText("Scenes.Fetishzealotscene.ZealotWinReligious.MysteryBrief".Localized());

            var __sw0_0 = (int)Player.Gender;

            if (__sw0_0 == (int)Gender.Hermaphrodite)
            {
                ClearButtons();

                AddNextButton("Common.Button.FuckHim".Localized(), DickZealotReligious);

                AddNextButton("Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonRideHim".Localized(), RideZealotReligious);
            }

            if (__sw0_0 == (int)Gender.Male)
                DickZealotReligious();

            if (true )
                RideZealotReligious();
        }

        public void DickZealotReligious()
        {

            OutputText("Scenes.Fetishzealotscene.DickZealotReligious.ClaspsHands".Localized());

            OutputText("Scenes.Fetishzealotscene.DickZealotReligious.NotDissuaded".Localized());

            OutputText("Scenes.Fetishzealotscene.DickZealotReligious.PushPrompt".Localized());

            OutputText("Scenes.Fetishzealotscene.DickZealotReligious.PrayersDistracting".Localized());

            OutputText("Scenes.Fetishzealotscene.DickZealotReligious.GlassyEyed".Localized());

            OutputText("Scenes.Fetishzealotscene.DickZealotReligious.ClergyRumor".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void RideZealotReligious()
        {

            OutputText("Scenes.Fetishzealotscene.RideZealotReligious.SitsOnButt".Localized());

            OutputText("Scenes.Fetishzealotscene.RideZealotReligious.SinksLevel".Localized());

            OutputText("Scenes.Fetishzealotscene.RideZealotReligious.UnveilsBody".Localized());

            OutputText("Scenes.Fetishzealotscene.RideZealotReligious.HeCompliant".Localized());

            if (Player.HasVagina())
                Player.CuntChange(currentZealot.CockArea(0), true);
            else
                Player.ButtChange(currentZealot.CockArea(0), true);

            OutputText("Scenes.Fetishzealotscene.RideZealotReligious.RushPleasure".Localized());

            OutputText("Scenes.Fetishzealotscene.RideZealotReligious.GiddinessBubbles".Localized());

            OutputText("Scenes.Fetishzealotscene.RideZealotReligious.CatchesBreath".Localized());

            Player.SlimeFeed();

            Player.Orgasm("VaginalAnal");

            CombatScene.CleanupAfterCombat();
        }

    }
}