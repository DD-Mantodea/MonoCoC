using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Internals;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Swim
{
    [Register<JellyFishGirlScene>("coc:jelly_fish_girl_scene")]
    public class JellyFishGirlScene : Scene
    {

        public string element = "blue";

        public string elementLong = "sapphire";

        public static bool isHalloween => DateTime.Now.Month == 10;

        public static SaveContent Save => SaveContent(SceneIds.Swim.JellyFishGirl);

        public string debugName => "JellyFishGirl";

        public string debugHint => "";

        public override void Entry()
        {
            JellyFishGirlEncounter();
        }

        public void Reset()
        {

            Save.SetBool("encountered", false);
        }

        public JellyFishGirlEnemy RandomJelly(bool firstTime = false)
        {
            var colorChoices = new WeightedRandom<string>()
                .Add("blue", 2)
                .Add("purple", 2)
                .Add("crystal", 1);

            if (isHalloween)
                colorChoices.Add("orange", 3);

            element = colorChoices.Choose();

            if (firstTime)
                element = "blue";

            var elements = new Dictionary<string, string[]>
            {
                ["blue"] = ["blue", "azure", "cerulean", "sapphire"],
                ["purple"] = ["purple", "violet", "plum", "amethyst"],
                ["crystal"] = ["glassy", "crystal", "prismatic"],
                ["orange"] = ["orange", "pumpkin", "tangerine", "sunstone"]
            };

            elementLong = RandomUtils.Choice(elements[element]);

            RegisterMacro("jellyColor", elementLong);

            return JellyFishGirlEnemy.Create(element, elementLong);
        }

        public void JellyFishGirlEncounter()
        {
            ClearOutput();

            if (!Save.GetBool("encountered"))
            {
                OutputText("Scenes.Jellyfishgirlscene.JellyFishGirlEncounter.SpotsJelly".Localized());

                OutputText("Scenes.Jellyfishgirlscene.JellyFishGirlEncounter.GelatinousStop".Localized());

                OutputText("Scenes.Jellyfishgirlscene.JellyFishGirlEncounter.FlailsBack".Localized());
            }
            else
                OutputText("Scenes.Jellyfishgirlscene.JellyFishGirlEncounter.YourAquaticAdventureYields".Localized(
                    RandomUtils.Choice([ "Your aquatic adventure yields a find--a bubble. On the lake floor. Inquisitive, you poke it, and it dislodges itself, revealing that it was a jelly-girl!",

            "The gleam of red catches your eye and you realize a jelly-girl is drifting along. The big, gelatinous bell wriggles as it pushes through the water, and the small, [if (allowBaby) {toddler|human}]-like body descending from it turns to look at you with her simple-minded expression. You are not caught off-guard though as she surges in your direction!",

            "Frilly, ribbon-like tendrils drift along as a jelly-girl floats overhead. The sight, lit by the [sun], is rather pretty, but the mucus-y tentacles tense up and begin to converge on you. She's on the offensive!" ])
                ));

            var firstTime = !Save.GetBool("encountered");

            CombatScene.Start(RandomJelly(firstTime), () => Go<CampScene>());

            Save.SetBool("encountered", true);
        }

        public void JellyLossChoices(bool hpVictory)
        {
            ClearOutput();

            RegisterMacro("lustwin", !hpVictory);

            if (Player.HasBreasts() && Player.HasCock())
            {
                if (Random.Shared.Next(100) < (50))
                    LoseYourMilkToTheJelly();
                else
                    JellyWantsYourPenis();
            }
            else
                if (Player.HasCock())
                    JellyWantsYourPenis();
                else
                    LoseYourMilkToTheJelly();
        }

        public void LoseYourMilkToTheJelly()
        {
            ClearOutput();

            OutputText("Scenes.Jellyfishgirlscene.LoseYourMilkToTheJelly.DizzinessFit".Localized());

            OutputText("Scenes.Jellyfishgirlscene.LoseYourMilkToTheJelly.EyesUnfocused".Localized());

            OutputText("Scenes.Jellyfishgirlscene.LoseYourMilkToTheJelly.TentacleGlides".Localized());

            OutputText("Scenes.Jellyfishgirlscene.LoseYourMilkToTheJelly.SucklesHarder".Localized());

            if (Player.HasVagina())
                Player.CuntChange(0, true, false);
            else
                Player.ButtChange(0, true, false);

            OutputText("Scenes.Jellyfishgirlscene.LoseYourMilkToTheJelly.GelatinTexture".Localized());

            OutputText("Scenes.Jellyfishgirlscene.LoseYourMilkToTheJelly.MuffledWhimper".Localized());

            Player.Orgasm("VaginalAnal");

            CombatScene.CleanupAfterCombat();
        }

        public void JellyWantsYourPenis()
        {
            ClearOutput();

            OutputText("Scenes.Jellyfishgirlscene.JellyWantsYourPenis.SeizesMoment".Localized());

            OutputText("Scenes.Jellyfishgirlscene.JellyWantsYourPenis.TendrilsEncircle".Localized());

            OutputText("Scenes.Jellyfishgirlscene.JellyWantsYourPenis.ShudderingHum".Localized());

            OutputText("Scenes.Jellyfishgirlscene.JellyWantsYourPenis.GrowlReverberates".Localized());

            OutputText("Scenes.Jellyfishgirlscene.JellyWantsYourPenis.MethodicalKneading".Localized());

            OutputText("Scenes.Jellyfishgirlscene.JellyWantsYourPenis.RelentlessSuction".Localized());

            OutputText("Scenes.Jellyfishgirlscene.JellyWantsYourPenis.ClimaxWanes".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void JellyWinChoices(bool hpVictory)
        {
            ClearOutput();

            RegisterMacro("lustwin", !hpVictory);

            OutputText("Scenes.Jellyfishgirlscene.JellyWinChoices.BubblesTrail".Localized());

            ClearButtons();

            SexButton(AddNextButton("Common.Button.TieUp".Localized(), TieUpJellyFishGirl), Gender.Male);

            SexButton(AddNextButton("Common.Button.TieDown".Localized(), TieDownJellyFishGirl), Gender.Female);

            SexButton(AddNextButton("Scenes.Faerie.SexMenu.ButtonOral".Localized(), OralTheJellyFishGirl), AnyGender)

                ?.DisableIf(Player.IsTaur(), "You need fewer legs to do this");

            if (silly)
                AddNextButton("Common.Button.Kill".Localized(), KillJellyFishGirl);

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void OralTheJellyFishGirl()
        {
            ClearOutput();

            OutputText("Scenes.Jellyfishgirlscene.OralTheJellyFishGirl.DriftsDizzily".Localized());

            OutputText("Scenes.Jellyfishgirlscene.OralTheJellyFishGirl.TongueLolls".Localized());

            OutputText("Scenes.Jellyfishgirlscene.OralTheJellyFishGirl.SlitheringWaist".Localized());

            OutputText("Scenes.Jellyfishgirlscene.OralTheJellyFishGirl.GrindsFervent".Localized());

            OutputText("Scenes.Jellyfishgirlscene.OralTheJellyFishGirl.SucklingSlows".Localized());

            Player.Orgasm("VaginalAndDick");

            CombatScene.CleanupAfterCombat();
        }

        public void TieDownJellyFishGirl()
        {
            ClearOutput();

            OutputText("Scenes.Jellyfishgirlscene.TieDownJellyFishGirl.QuiversGelatinous".Localized());

            OutputText("Scenes.Jellyfishgirlscene.TieDownJellyFishGirl.DragsTied".Localized());

            OutputText("Scenes.Jellyfishgirlscene.TieDownJellyFishGirl.TendrilsSluggish".Localized());

            OutputText("Scenes.Jellyfishgirlscene.TieDownJellyFishGirl.RocksSlowDeliberate".Localized(
                Player.CuntChange(0, true, false, false) ? "true" : "false"
            ));

            OutputText("Scenes.Jellyfishgirlscene.TieDownJellyFishGirl.BreathFaster".Localized());

            OutputText("Scenes.Jellyfishgirlscene.TieDownJellyFishGirl.CryBubbles".Localized());

            OutputText("Scenes.Jellyfishgirlscene.TieDownJellyFishGirl.RelaxesTremors".Localized());

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void TieUpJellyFishGirl()
        {
            ClearOutput();

            OutputText("Scenes.Jellyfishgirlscene.TieUpJellyFishGirl.QuiversGelatinous".Localized());

            OutputText("Scenes.Jellyfishgirlscene.TieUpJellyFishGirl.EyesBewilderment".Localized());

            OutputText("Scenes.Jellyfishgirlscene.TieUpJellyFishGirl.WrithesInstinct".Localized());

            OutputText("Scenes.Jellyfishgirlscene.TieUpJellyFishGirl.ClenchesPulsating".Localized());

            OutputText("Scenes.Jellyfishgirlscene.TieUpJellyFishGirl.CohesionWeakens".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void KillJellyFishGirl()
        {
            ClearOutput();

            OutputText("Scenes.Jellyfishgirlscene.KillJellyFishGirl.FullWeightStrike".Localized());

            OutputText("Scenes.Jellyfishgirlscene.KillJellyFishGirl.DriftsBack".Localized());

            CombatScene.CleanupAfterCombat();
        }

    }
}
