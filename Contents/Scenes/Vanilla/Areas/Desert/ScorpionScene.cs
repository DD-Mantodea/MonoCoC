using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using System.Collections.Generic;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Desert
{
    [Register<ScorpionScene>("coc:scorpion")]
    public class ScorpionScene : Scene
    {

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
{
    ["nude"] = "bare, exposed body",
    ["straps"] = "solid black stockings and leather chest-wraps",
    ["leaf"] = "leafy wrappings",
    ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
};

        public static SaveContent Save => SaveContent(SceneIds.Desert.Scorpion);

        public override void Entry()
        {
            ExecEncounter();
        }

        public void ExecEncounter()
        {
            ClearOutput();

            ClearButtons();

            if (Save.GetInt("state", 0) == 0)
            {
                Save.SetInt("state", 1);

                OutputText("Scenes.Scorpionscene.ExecEncounter.RhythmicCrunch".Localized());

                OutputText("Scenes.Scorpionscene.ExecEncounter.CloseEnough".Localized());

                OutputText("Scenes.Scorpionscene.ExecEncounter.DriedShrubbery".Localized());

                OutputText("Scenes.Scorpionscene.ExecEncounter.NoticesInscriptions".Localized());

                OutputText("Scenes.Scorpionscene.ExecEncounter.WhatLookedLikeAncient".Localized());

                OutputText("Scenes.Scorpionscene.ExecEncounter.YourMindIsCalled".Localized());

                OutputText("Scenes.Scorpionscene.ExecEncounter.SomeOptions".Localized());
            }
            else
            {
                OutputText("Scenes.Scorpionscene.ExecEncounter.TreadedPath".Localized());

                OutputText("Scenes.Scorpionscene.ExecEncounter.SameBushes".Localized((CoC.Instance.Flags.Get(GameFlagIds.CodexEntryNagas) > 0 ? "nagas" : "snake-people")));

                OutputText("Scenes.Scorpionscene.ExecEncounter.TooQuiet".Localized());

                OutputText("Scenes.Scorpionscene.ExecEncounter.GlanceOnly".Localized());

                AddButton(2, "Common.Button.NeverAgain".Localized(), Never);
            }

            AddNextButton("Common.Button.Investigate".Localized(), Investigate);

            AddNextButton("Common.Button.Leave".Localized(), Leave);
        }

        public void Leave()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Leave.WithDifficulty".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Never()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Never.NoValueExploring".Localized());

            Save.SetInt("state", -1);

            DoNext(() => ReturnToCamp(1));
        }

        public void Investigate()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Investigate.MoreToStones".Localized());

            OutputText("Scenes.Scorpionscene.Investigate.HopesNoose".Localized());

            OutputText("Scenes.Scorpionscene.Investigate.LetsRest".Localized());

            OutputText("Scenes.Scorpionscene.Investigate.JustTumbleweed".Localized());

            OutputText("Scenes.Scorpionscene.Investigate.DistantRumble".Localized());

            OutputText("Scenes.Scorpionscene.Investigate.ApproachingRapid".Localized());

            OutputText("Scenes.Scorpionscene.Investigate.ScramblesUp".Localized());

            OutputText("Scenes.Scorpionscene.Investigate.SandCascades".Localized());

            OutputText("Scenes.Scorpionscene.Investigate.SandTrap".Localized());

            OutputText("Scenes.Scorpionscene.Investigate.SteadiesGround".Localized());

            OutputText("Scenes.Scorpionscene.Investigate.CarvingsHunters".Localized((CoC.Instance.Flags.Get(GameFlagIds.CodexEntryNagas) > 0 ? "nagas" : "snake-human hybrids")));

            OutputText("Scenes.Scorpionscene.Investigate.SomethingCracks".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Attack".Localized(), Attack);

            AddNextButton("Common.Button.Talk".Localized(), Talk);
        }

        public void Attack()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Attack.NoSenseInTrying".Localized());

            OutputText("Scenes.Scorpionscene.Attack.RaisesPincers".Localized());

            CombatCalculator.AddCombatBuff(Player, CombatCalculator.StatByName("spe"), -Player.Speed / 3, "ScorpionScene");

            var scorpion = EnemyRegistry.Create(EnemyIds.Scorpion);

            CombatScene.Start(scorpion, () => Go<CampScene>());
        }

        public void Talk()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Talk.NagaNote".Localized());

            OutputText("Scenes.Scorpionscene.Talk.CallsOut".Localized());

            OutputText("Scenes.Scorpionscene.Talk.DoesntAttack".Localized());

            OutputText("Scenes.Scorpionscene.Talk.AgitatedIt".Localized());

            Save.SetBool("talked", true);

            CombatCalculator.AddCombatBuff(Player, CombatCalculator.StatByName("spe"), -Player.Speed / 3, "ScorpionScene");

            var scorpion = EnemyRegistry.Create(EnemyIds.Scorpion);

            CombatScene.Start(scorpion, () => Go<CampScene>());
        }

        public void BadEnd(bool hpVictory)
        {
            ClearOutput();

            if (CombatController.Instance!.CurrAbilityUsed == CombatController.FantasizeAbilityId && silly)
            {
                OutputText("Scenes.Scorpionscene.BadEnd.CantKeepIn".Localized());

                OutputText("Scenes.Scorpionscene.BadEnd.NeverComes".Localized());

                OutputText("Scenes.Scorpionscene.BadEnd.SlightTap".Localized());

                OutputText("Scenes.Scorpionscene.BadEnd.NotDeath".Localized());

                OutputText("Scenes.Scorpionscene.BadEnd.YouBlush".Localized());

                OutputText("Scenes.Scorpionscene.BadEnd.BlushesBack".Localized());

                OutputText("Scenes.Scorpionscene.BadEnd.PicksUpAgain".Localized());

                OutputText("Scenes.Scorpionscene.BadEnd.GuidesStep".Localized());

                OutputText("Scenes.Scorpionscene.BadEnd.FeetFly".Localized());

                OutputText("Scenes.Scorpionscene.BadEnd.WateringUp".Localized());

                OutputText("Scenes.Scorpionscene.BadEnd.RaisesYouUp".Localized());

                OutputText("Scenes.Scorpionscene.BadEnd.ThatsMyArm".Localized());

                OutputText("Scenes.Scorpionscene.BadEnd.FantasyKeeps".Localized());

                OutputText("Scenes.Scorpionscene.BadEnd.DashingDances".Localized());
            }
            else
                if (Player.HasStatusEffect(StatusEffectIds.Grappled))
                {
                    OutputText("Scenes.Scorpionscene.BadEnd.CantBreakFree".Localized());

                    OutputText("Scenes.Scorpionscene.BadEnd.SeesRibcage".Localized());

                    OutputText("Scenes.Scorpionscene.BadEnd.PowerlessFlailing".Localized());

                    OutputText("Scenes.Scorpionscene.BadEnd.LastThingSee".Localized());
                }
                else
                {
                    if (hpVictory)
                        OutputText("Scenes.Scorpionscene.BadEnd.LurchesVision".Localized());
                    else
                        OutputText("Scenes.Scorpionscene.BadEnd.LurchesLoins".Localized());

                    OutputText("Scenes.Scorpionscene.BadEnd.EarthSpins".Localized());

                    OutputText("Scenes.Scorpionscene.BadEnd.NumbPain".Localized());

                    OutputText("Scenes.Scorpionscene.BadEnd.BileRises".Localized((!hpVictory ? "This isn't the kind of S&M play you had in mind. You" : "The world only rolls faster. And you")));

                    OutputText("Scenes.Scorpionscene.BadEnd.HurtsSoMuch".Localized());

                    OutputText("Scenes.Scorpionscene.BadEnd.LiftedAir".Localized());

                    OutputText("Scenes.Scorpionscene.BadEnd.LetsGo".Localized());
                }

            GameOver();
        }

        public void Defeated()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Defeated.VisiblyWounded".Localized());

            OutputText("Scenes.Scorpionscene.Defeated.ClearsAway".Localized());

            OutputText("Scenes.Scorpionscene.Defeated.StillAlive".Localized());

            OutputText("Scenes.Scorpionscene.Defeated.MindfulOpponent".Localized());

            OutputText("Scenes.Scorpionscene.Defeated.DoubtThreat".Localized());

            ClearButtons();

            AddNextButton("Scenes.Lumberjack.Button.Spare".Localized(), Spare);

            AddNextButton("Common.Button.Kill".Localized(), Kill);

            AddNextButton("Scenes.Faerie.GladesScene.ButtonHelp".Localized(), Help);

            AddNextButton("Common.Button.Fuck".Localized(), Fuck);
        }

        public void Spare()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Spare.NoBloodshed".Localized());

            OutputText("Scenes.Scorpionscene.Spare.NotFarPincer".Localized());

            OutputText("Scenes.Scorpionscene.Spare.PickingTwigs".Localized());

            OutputText("Scenes.Scorpionscene.Spare.TimeItTakes".Localized());

            OutputText("Scenes.Scorpionscene.Spare.NothingLeft".Localized());

            Save.SetInt("fate", 2);

            Player.ChangeFatigue(35);

            InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.DesertRose), () => { CombatScene.CleanupAfterCombat(); ReturnToCamp(3); });
        }

        public void Kill()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Kill.MonsterMustDie".Localized());

            OutputText("Scenes.Scorpionscene.Kill.StridesClose".Localized());

            if (Player.Weapon.IsSpear())
                OutputText("Scenes.Scorpionscene.Kill.ThrustMaw".Localized());
            else
                if (Player.Weapon.IsKnife())
                    OutputText("Scenes.Scorpionscene.Kill.ClimbOnTop".Localized());
                else
                    if (Player.Weapon.IsAxe())
                        OutputText("Scenes.Scorpionscene.Kill.SwingGap".Localized());
                    else
                        if (Player.Weapon.IsScythe())
                            OutputText("Scenes.Scorpionscene.Kill.SwingBlade".Localized());
                        else
                            if (Player.Weapon.IsBladed())
                                OutputText("Scenes.Scorpionscene.Kill.ClambersOnTop".Localized());
                            else
                                if (Player.Weapon.IsBlunt())
                                    OutputText("Scenes.Scorpionscene.Kill.ClimbOnTopAgain".Localized());
                                else
                                    if (Player.Weapon.IsFirearm())
                                        OutputText("Scenes.Scorpionscene.Kill.PressBarrel".Localized());
                                    else
                                        if (Player.Weapon.IsChanneling(Player))
                                            OutputText("Scenes.Scorpionscene.Kill.ChargeEnergy".Localized());
                                        else
                                            OutputText("Scenes.Scorpionscene.Kill.GrabMouthparts".Localized());

            OutputText("Scenes.Scorpionscene.Kill.StepBackAppraise".Localized());

            OutputText("Scenes.Scorpionscene.Kill.PickingTwigs".Localized());

            OutputText("Scenes.Scorpionscene.Kill.TimeItTakes".Localized());

            OutputText("Scenes.Scorpionscene.Kill.WatchYourStep".Localized());

            Save.SetInt("fate", 1);

            Player.ChangeFatigue(45);

            InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.DesertRose), () => { CombatScene.CleanupAfterCombat(); ReturnToCamp(3); });
        }

        public void Help()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Help.IllAdvised".Localized());

            OutputText("Scenes.Scorpionscene.Help.RattlesAtYou".Localized());

            if (Player.Strength > 100)
                OutputText("Scenes.Scorpionscene.Help.TinyBit".Localized());
            else
            {
                OutputText("Scenes.Scorpionscene.Help.NothingCanDo".Localized((Player.Weapon.IsSpear() ? " Your [weapon] comes to mind, but you'd rather not risk it." : "")));

                OutputText("Scenes.Scorpionscene.Help.SpaceWon".Localized());

                OutputText("Scenes.Scorpionscene.Help.PushLever".Localized());
            }

            OutputText("Scenes.Scorpionscene.Help.DustSettles".Localized());

            OutputText("Scenes.Scorpionscene.Help.SpotsSomething".Localized());

            OutputText("Scenes.Scorpionscene.Help.TheMagic".Localized());

            OutputText("Scenes.Scorpionscene.Help.MoreInterested".Localized());

            OutputText("Scenes.Scorpionscene.Help.ConversationEnds".Localized());

            OutputText("Scenes.Scorpionscene.Help.FiveObelisks".Localized());

            OutputText("Scenes.Scorpionscene.Help.SighLeaves".Localized());

            OutputText("Scenes.Scorpionscene.Help.FlowingSand".Localized());

            OutputText("Scenes.Scorpionscene.Help.VoicesThanks".Localized());

            OutputText("Scenes.Scorpionscene.Help.LotsOfGems".Localized());

            OutputText("Scenes.Scorpionscene.Help.GettingOut".Localized());

            OutputText("Scenes.Scorpionscene.Help.TestsGrip".Localized());

            Player.ChangeFatigue(25);

            ClearButtons();

            AddNextButton("Common.Button.DoIt".Localized(), Ride);

            AddNextButton("Scenes.ErlKing.HowDareYou.ButtonDont".Localized(), Rude);
        }

        public void Ride()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Ride.TrustColossus".Localized());

            OutputText("Scenes.Scorpionscene.Ride.SmoothPlates".Localized());

            OutputText("Scenes.Scorpionscene.Ride.ClackingSounds".Localized());

            OutputText("Scenes.Scorpionscene.Ride.PressingFeeling".Localized());

            OutputText("Scenes.Scorpionscene.Ride.HoldingOnEasy".Localized());

            OutputText("Scenes.Scorpionscene.Ride.ShakesYouOff".Localized());

            OutputText("Scenes.Scorpionscene.Ride.AnxietyWearsOff".Localized());

            OutputText("Scenes.Scorpionscene.Ride.CoatsCarapace".Localized(
                 View.Time.IsTimeBetween(18, 21) ? "Night has fallen, and the moon's light" : (View.Time.IsTimeBetween(21, 3) ? "The moonlight" : "The sunlight")
            ));

            Save.SetInt("fate", 4);

            Player.Gems += 713;

            InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.DesertRose), () => { CombatScene.CleanupAfterCombat(); ReturnToCamp(3); });
        }

        public void Rude()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Rude.DontTrustThing".Localized());

            OutputText("Scenes.Scorpionscene.Rude.ArduousWay".Localized());

            OutputText("Scenes.Scorpionscene.Rude.TideOfSand".Localized());

            OutputText("Scenes.Scorpionscene.Rude.NotSureHow".Localized());

            Save.SetInt("fate", 3);

            Player.Gems += 713;

            Player.ChangeFatigue(35);

            InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.DesertRose), () => { CombatScene.CleanupAfterCombat(); ReturnToCamp(3); });
        }

        public void Fuck()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Fuck.ThoughtSeeps".Localized());

            OutputText("Scenes.Scorpionscene.Fuck.CastsGear".Localized());

            OutputText("Scenes.Scorpionscene.Fuck.TiredlyRattles".Localized());

            OutputText("Scenes.Scorpionscene.Fuck.ChitinWarm".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Scorpionscene.Fuck.TurgidCock".Localized());

                OutputText("Scenes.Scorpionscene.Fuck.ReliefMandibles".Localized());

                OutputText("Scenes.Scorpionscene.Fuck.GruntsPants".Localized());

                OutputText("Scenes.Scorpionscene.Fuck.LapseControl".Localized());

                OutputText("Scenes.Scorpionscene.Fuck.PronouncesClimax".Localized());
            }
            else
            {
                OutputText("Scenes.Scorpionscene.Fuck.VaginaDrools".Localized());

                OutputText("Scenes.Scorpionscene.Fuck.ChitinGlides".Localized());

                OutputText("Scenes.Scorpionscene.Fuck.MoansPants".Localized());

                OutputText("Scenes.Scorpionscene.Fuck.LapseControlAgain".Localized());

                OutputText("Scenes.Scorpionscene.Fuck.PronouncesPleasure".Localized());
            }

            OutputText("Scenes.Scorpionscene.Fuck.HungersMore".Localized());

            OutputText("Scenes.Scorpionscene.Fuck.LotOfBlood".Localized());

            OutputText("Scenes.Scorpionscene.Fuck.ForgetItAll".Localized());

            OutputText("Scenes.Scorpionscene.Fuck.ChuckleDribbles".Localized());

            OutputText("Scenes.Scorpionscene.Fuck.MoreMandibles".Localized());

            OutputText("Scenes.Scorpionscene.Fuck.LoudCrack".Localized());

            OutputText("Scenes.Scorpionscene.Fuck.GrandFinale".Localized());

            OutputText("Scenes.Scorpionscene.Fuck.MandiblesAbove".Localized());

            Player.Orgasm("Generic");

            GameOver();
        }

        public void Special()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Special.PincersClose".Localized((CombatController.Instance!.CurrAbilityUsed == CombatController.TeaseAbilityId ? "You just wanted to seduce it, but this is fine by you, too." : "Does it understand you mean it no harm?")));

            OutputText("Scenes.Scorpionscene.Special.LowersWeapon".Localized((Save.GetBool("talked", false) ? " this time" : "")));

            OutputText("Scenes.Scorpionscene.Special.ItSSmoothAnd".Localized());

            OutputText("Scenes.Scorpionscene.Special.ConversationDies".Localized());

            OutputText("Scenes.Scorpionscene.Special.FiveObelisks".Localized());

            OutputText("Scenes.Scorpionscene.Special.SighLeaves".Localized());

            OutputText("Scenes.Scorpionscene.Special.GrindingChitin".Localized());

            OutputText("Scenes.Scorpionscene.Special.VoicesThanks".Localized());

            OutputText("Scenes.Scorpionscene.Special.LotsOfGems".Localized());

            OutputText("Scenes.Scorpionscene.Special.WhateverFascinated".Localized());

            OutputText("Scenes.Scorpionscene.Special.AllForTreasure".Localized());

            OutputText("Scenes.Scorpionscene.Special.TestsGrip".Localized());

            CombatScene.CleanupAfterCombat();

            ClearButtons();

            AddNextButton("Common.Button.Mount".Localized(), Mount);

            AddNextButton("Common.Button.ClimbAlone".Localized(), Climb);
        }

        public void Mount()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Mount.IllIntentions".Localized());

            OutputText("Scenes.Scorpionscene.Mount.SmoothPlates".Localized());

            OutputText("Scenes.Scorpionscene.Mount.ClackingSounds".Localized());

            OutputText("Scenes.Scorpionscene.Mount.PressingFeeling".Localized());

            OutputText("Scenes.Scorpionscene.Mount.HoldingOnEasy".Localized());

            OutputText("Scenes.Scorpionscene.Mount.NotMuchToSee".Localized());

            OutputText("Scenes.Scorpionscene.Mount.CrestsDunes".Localized());

            OutputText("Scenes.Scorpionscene.Mount.BlackCarapace".Localized(
                 View.Time.IsTimeBetween(19, 20) ? "Night has fallen, and in the moon's light" : (View.Time.IsTimeBetween(21, 3) ? "In the moonlight" : "In the sunlight")
            ));

            OutputText("Scenes.Scorpionscene.Mount.CutsFromIt".Localized());

            OutputText("Scenes.Scorpionscene.Mount.DoesntFollow".Localized());

            Save.SetInt("fate", 4);

            Player.Gems += 713;

            InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.DesertRose), () => ReturnToCamp(2));
        }

        public void Climb()
        {
            ClearOutput();

            OutputText("Scenes.Scorpionscene.Climb.HelpfulDocile".Localized());

            OutputText("Scenes.Scorpionscene.Climb.WallToScale".Localized());

            OutputText("Scenes.Scorpionscene.Climb.ArduousWay".Localized());

            OutputText("Scenes.Scorpionscene.Climb.TideComes".Localized());

            OutputText("Scenes.Scorpionscene.Climb.KnowsWhatNext".Localized());

            Save.SetInt("fate", 3);

            Player.Gems += 713;

            Player.ChangeFatigue(35);

            InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.DesertRose), () => ReturnToCamp(3));
        }

    }
}