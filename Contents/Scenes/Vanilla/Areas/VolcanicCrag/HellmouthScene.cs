using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.VolcanicCrag
{
    [Register<HellmouthScene>("coc:hellmouth")]
    public class HellmouthScene : Scene
    {

        public bool ambushed = false;

        public bool Ambushed => Save.GetBool("ambushed");

        public static SaveContent Save => SaveContent(SceneIds.VolcanicCrag.Hellmouth);

        public string debugName => "Hellmouth";

        public string debugHint => "";

        public override void Entry()
        {

        }

        public void Reset()
        {

            Save.SetBool("ambushed", false);
        }

        public void EncounterHellmouth()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MetHellmouth) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.MetHellmouth, 1);

                OutputText("Scenes.Hellmouthscene.EncounterHellmouth.JourneyingCrag".Localized());

                OutputText("Scenes.Hellmouthscene.EncounterHellmouth.GlancingUp".Localized());

                OutputText("Scenes.Hellmouthscene.EncounterHellmouth.FacesDemon".Localized());

                OutputText("Scenes.Hellmouthscene.EncounterHellmouth.PaleSkin".Localized());

                OutputText("Scenes.Hellmouthscene.EncounterHellmouth.ShortStackClears".Localized());

                OutputText("Scenes.Hellmouthscene.EncounterHellmouth.LiftsWeapon".Localized());

                OutputText("Scenes.Hellmouthscene.EncounterHellmouth.FightingHellmouth".Localized());
            }
            else
                OutputText("Scenes.Hellmouthscene.EncounterHellmouth.TrekEnds".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryHellmouth);

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.Hellmouth), () => Go<CampScene>());
        }

        public void BeatHellmouth(bool output = true)
        {
            ClearOutput();

            if (output)
                if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                    OutputText("Scenes.Hellmouthscene.BeatHellmouth.ShakingTwitching".Localized());
                else
                    OutputText("Scenes.Hellmouthscene.BeatHellmouth.BatteredBroken".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Lick".Localized(), GetLickedByHellmouth);

            AddButton(1, "Common.Button.Sixtynine".Localized(), HellmouthSixtyNine);

            AddButton(2, "Common.Button.Hairjob".Localized(), HellmouthHairjob);

            AddNextButton("Scenes.PlayerAppearance.Label.Ears".Localized(), Ears);

            AddNextButton("Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), HellmouthCuddling);

            if (OptionStore.Instance.Get("watersports", 0) > 0)
                AddNextButton("Common.Button.Watersports".Localized(), HellmouthPiss);

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(10, "Common.Button.Kill".Localized(), KillHellmouth);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void HellmouthHairjob()
        {
            ClearOutput();

            if (Player.HasCock())
            {
                Player.Orgasm("Dick");

                OutputText("Scenes.Hellmouthscene.HellmouthHairjob.NoTrustingMaw".Localized());

                OutputText("Scenes.Hellmouthscene.HellmouthHairjob.LooksOnDazed".Localized());

                OutputText("Scenes.Hellmouthscene.HellmouthHairjob.GrabsCock".Localized());

                OutputText("Scenes.Hellmouthscene.HellmouthHairjob.SighsContented".Localized());
            }
            else
            {
                Player.Orgasm("Vaginal");

                OutputText("Scenes.Hellmouthscene.HellmouthHairjob.NoTrustingMawAgain".Localized());

                OutputText("Scenes.Hellmouthscene.HellmouthHairjob.LooksOnDazedAgain".Localized());

                OutputText("Scenes.Hellmouthscene.HellmouthHairjob.ThrownOff".Localized());

                OutputText("Scenes.Hellmouthscene.HellmouthHairjob.GrabsPseudoCock".Localized());

                OutputText("Scenes.Hellmouthscene.HellmouthHairjob.SighsContentedAgain".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));

            CombatScene.CleanupAfterCombat();
        }

        public void GetLickedByHellmouth()
        {
            ClearOutput();

            OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.HellishMaw".Localized());

            OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.WalksUp".Localized((!Player.IsNakedLower() ? "relieve yourself of your garments" : "get started")));

            if (Player.HasCock())
            {
                OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.ThickTongue".Localized());

                OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.TongueWraps".Localized());

                OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.DistractSelf".Localized());

                OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.ReachingLimit".Localized());

                Player.Orgasm("Dick");
            }
            else
            {
                Player.Orgasm("Vaginal");

                OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.LongHotTongue".Localized((Player.HasTailInsteadOfLegs() ? "You lower your snake coils to put your pussy as close to her face as you're comfortably able." : "You splay your legs out, lowering your hips to put your pussy in better reach of the crawling demoness.")));

                OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.CorruptedStack".Localized());

                if (Player.Vaginas[0].Looseness < 2)
                {
                    OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.GentlestStretching".Localized());

                    if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin)
                    {
                        OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.ItStings".Localized());

                        Player.CuntChange(5, true);
                    }
                    else
                        OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.IntoPlaces".Localized());
                }
                else
                    OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.IntoPlacesAgain".Localized());

                OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.InsidesShudder".Localized((Player.Body.Get<BodyParts.Vanilla.TonguePart>().Id == BodyPartTypes.Demonic ? ", you would know" : ", you think to yourself")));

                OutputText("Scenes.Hellmouthscene.GetLickedByHellmouth.StretchesUnwinds".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void KillHellmouth()
        {
            ClearOutput();

            if (Player.Weapon.IsHolySword())
                OutputText("Scenes.Hellmouthscene.KillHellmouth.WrenchesEar".Localized());
            else
                if (Player.Weapon.IsScythe())
                    OutputText("Scenes.Hellmouthscene.KillHellmouth.EvenMawHell".Localized());
                else
                    if (Player.Weapon.IsStaff() && Player.Weapon.IsChanneling(Player))
                        OutputText("Scenes.Hellmouthscene.KillHellmouth.WeaponShimmers".Localized());
                    else
                        if (Player.Weapon.IsBlunt())
                            OutputText("Scenes.Hellmouthscene.KillHellmouth.StretchesLimbs".Localized());
                        else
                            if (Player.Weapon.IsKnife())
                                OutputText("Scenes.Hellmouthscene.KillHellmouth.ShortWork".Localized());
                            else
                                if ((Player.Weapon.IsAxe() || Player.Weapon.IsLarge()) && Player.Weapon.IsBladed())
                                    OutputText("Scenes.Hellmouthscene.KillHellmouth.HeavesWeapon".Localized());
                                else
                                    if (Player.Weapon.IsStabby())
                                        OutputText("Scenes.Hellmouthscene.KillHellmouth.TwistsForward".Localized());
                                    else
                                        if (Player.Weapon.IsSpear())
                                            OutputText("Scenes.Hellmouthscene.KillHellmouth.YanksEar".Localized());
                                        else
                                            OutputText("Scenes.Hellmouthscene.KillHellmouth.FocusesLine".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HellmouthsKilled, CoC.Instance.Flags.Get(GameFlagIds.HellmouthsKilled) + 1);

            if (Player.Corruption < 25)
                Player.ApplyStats(new DynStat(GameStats.Corruption, -0.5f));

            Player.UpgradeDeusVult();

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToHellmouth()
        {
            ClearOutput();

            OutputText("Scenes.Hellmouthscene.LoseToHellmouth.BetweenTeeth".Localized());

            OutputText("Scenes.Hellmouthscene.LoseToHellmouth.VisionDarkens".Localized());

            OutputText("Scenes.Hellmouthscene.LoseToHellmouth.ShutsMaw".Localized());

            GameOver();
        }

        public void HellmouthCuddling()
        {
            ClearOutput();

            OutputText("Scenes.Hellmouthscene.HellmouthCuddling.DespiteDemonic".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthCuddling.NodsNervously".Localized((Player.Race != "human" && !(Player.DemonScore() < 4) && Player.GoblinScore() < 4 ? "un" : "")));

            OutputText("Scenes.Hellmouthscene.HellmouthCuddling.NotWhyHere".Localized((Player.Race != "human" && !(Player.DemonScore() < 4) && Player.GoblinScore() < 4 ? "un" : "")));

            OutputText("Scenes.Hellmouthscene.HellmouthCuddling.TremblesTouch".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthCuddling.MustLikeIt".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthCuddling.ShakesHead".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthCuddling.AshenSkin".Localized((Player.Race != "human" && !(Player.DemonScore() < 4) && Player.GoblinScore() < 4 ? "un" : "")));

            OutputText("Scenes.Hellmouthscene.HellmouthCuddling.RightNow".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthCuddling.BurblesDisappointment".Localized(
                 Player.HasChildren() ? "family" : (CampScene.FollowersCount() + CampScene.LoversCount() > 0 ? "friends" : "home")
            ));

            CombatScene.CleanupAfterCombat();
        }

        public void HellmouthPiss()
        {
            ClearOutput();

            OutputText("Scenes.Hellmouthscene.HellmouthPiss.AbominableMaw".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthPiss.StrollsOver".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthPiss.StaringUp".Localized((!Player.IsNaked() ? " while removing your [armor]" : "")));

            OutputText("Scenes.Hellmouthscene.HellmouthPiss.MovesTongue".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthPiss.HumiliationFun".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthPiss.TremblesPowerful".Localized((!Player.IsNaked() ? "re-dress" : "gather your things")));

            OutputText("Scenes.Hellmouthscene.HellmouthPiss.HeadBack".Localized((Player.IsNaked() ? "You take a moment to catch your breath" : "You finish re-dressing, take a moment to catch your breath,")));

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void HellmouthSixtyNine()
        {
            ClearOutput();

            OutputText("Scenes.Hellmouthscene.HellmouthSixtyNine.VictorSpoils".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthSixtyNine.SitsAtopFace".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthSixtyNine.TeasingFun".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthSixtyNine.SurprisinglyDexterous".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthSixtyNine.OnceFinished".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void HellmouthAmbush()
        {
            ClearOutput();

            OutputText("Scenes.Hellmouthscene.HellmouthAmbush.SomethingChurns".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthAmbush.RockTurns".Localized());

            Save.SetBool("ambushed", true);

            ClearButtons();

            AddNextButton("Common.Button.Dodge".Localized(), HellmouthAmbushDodge);

            AddNextButton("Common.Button.Shut".Localized(), HellmouthAmbushShut);
        }

        public void HellmouthAmbushDodge()
        {
            ClearOutput();

            OutputText("Scenes.Hellmouthscene.HellmouthAmbushDodge.LeapsAside".Localized());

            var desc = "This demon stands at about the height of a goblin, however instead of a gaping pussy, it's her mouth that seems capable of widening to accommodate anything and everything. Her teeth are generally sharp and imposing, with two pairs of longer canines standing out. Her skin is pale gray with visible and thin veins showing through. Her chest is quite modest, opposite to her wide hips. Her eyes are large, glowing crimson around her pitch-black pupils. If she has irises, they are equally as pitch as the pupils themselves. Her elfin ears are at least a foot long each, drooping toward the sides as they poke out from her long flowing locks of black hair. She'd be cute, perhaps, if not for the impending death you sense from that hellish maw.";

            var hellmouth = EnemyRegistry.Create(EnemyIds.Hellmouth);

            var imp = EnemyRegistry.Create(EnemyIds.Imp);

            imp.OnWon = _ => LoseToHellmouth();

            CombatScene.StartMultiple(new[] { hellmouth, imp }, () => BeatHellmouth(), false, desc);
        }

        public void HellmouthAmbushShut()
        {
            ClearOutput();

            OutputText("Scenes.Hellmouthscene.HellmouthAmbushShut.LungesAt".Localized());

            ClearButtons();

            AddNextButton("Scenes.Faerie.SexMenu.ButtonRelease".Localized(), HellmouthAmbushRelease);

            AddNextButton("Common.Button.Kill".Localized(), HellmouthAmbushKill);
        }

        public void HellmouthAmbushKill()
        {
            ClearOutput();

            OutputText("Scenes.Hellmouthscene.HellmouthAmbushKill.EyesGoBlank".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HellmouthsKilled, CoC.Instance.Flags.Get(GameFlagIds.HellmouthsKilled) + 1);

            if (Player.Corruption < 25)
                Player.ApplyStats(new DynStat(GameStats.Corruption, -0.5f));

            Player.UpgradeDeusVult();

            DoNext(() => ReturnToCamp(1));
        }

        public void HellmouthAmbushRelease()
        {
            ClearOutput();

            OutputText("Scenes.Hellmouthscene.HellmouthAmbushRelease.SadEyesPity".Localized());

            OutputText("Scenes.Hellmouthscene.HellmouthAmbushRelease.CretinExclaims".Localized());

            BeatHellmouth(false);

            AddButton(14, "Common.Button.Leave".Localized(), HellmouthAmbushLeave);
        }

        public void HellmouthAmbushLeave()
        {
            ClearOutput();

            OutputText("Scenes.Hellmouthscene.HellmouthAmbushLeave.ExperienceOver".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Ears()
        {
            ClearOutput();

            OutputText("Scenes.Hellmouthscene.Ears.LustAsIs".Localized());

            OutputText("Scenes.Hellmouthscene.Ears.MurmursQuietly".Localized());

            OutputText("Scenes.Hellmouthscene.Ears.PullsHead".Localized());

            OutputText("Scenes.Hellmouthscene.Ears.EnoughExploratory".Localized());

            OutputText("Scenes.Hellmouthscene.Ears.SpurredThrust".Localized());

            OutputText("Scenes.Hellmouthscene.Ears.RapesEar".Localized());

            OutputText("Scenes.Hellmouthscene.Ears.WaveClimax".Localized());

            OutputText("Scenes.Hellmouthscene.Ears.LiftsHead".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));

            CombatScene.CleanupAfterCombat();
        }

    }
}