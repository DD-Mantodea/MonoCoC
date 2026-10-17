using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Internals;
using System.Collections.Generic;
using MonoCoC.Utils;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<AikoScene>("coc:aiko")]
    public class AikoScene : Scene
    {

        public static bool talkAndSex;

        public static bool playerchoice = true;

        public static int aikoCorruption
        {
            get => CoC.Instance.Flags.Get(GameFlagIds.AikoCorruption);

            set => CoC.Instance.Flags.Set(GameFlagIds.AikoCorruption, Math.Clamp(value, 0, 100));
        }

        public void StartYamataFight()
        {
            var yamata = EnemyRegistry.Create(EnemyIds.Yamata);

            yamata.OnDefeated = (hpVictory) => YamataLoses();

            yamata.OnWon = (hpVictory) => YamataWins();

            CombatScene.Start(yamata, () => Go<CampScene>());
        }

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
{
    ["nude"] = "bare, exposed body",
    ["straps"] = "solid black stockings and leather chest-wraps",
    ["leaf"] = "leafy wrappings",
    ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
};

        public static SaveContent Save => SaveContent(SceneIds.Forest.Aiko);

        public override void Entry()
        {
            ExecEncounter();
        }

        public void ExecEncounter()
        {

            EncounterAiko();
        }

        public bool PlayerJerk()
        {

            return !Player.IsPureEnough(50) || Player.HasPerk(PerkIds.Sadist);
        }

        public void EncounterAiko()
        {
            ClearOutput();

            ShowImage("aiko-intro");

            talkAndSex = false;

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoTimesMet) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, 50);

            CoC.Instance.Flags.Set(GameFlagIds.AikoTimesMet, CoC.Instance.Flags.Get(GameFlagIds.AikoTimesMet) + 1);

            ShowImage("Aiko-intro");

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoTimesMet) == 1)
            {
                OutputText("Scenes.Aiko.EncounterAiko.DeeperFoliage".Localized());

                OutputText("Scenes.Aiko.EncounterAiko.NeverExplored".Localized());

                OutputText("Scenes.Aiko.EncounterAiko.FaintLight".Localized());

                OutputText("Scenes.Aiko.EncounterAiko.PathGone".Localized());

                OutputText("Scenes.Aiko.EncounterAiko.LostInUndergrowth".Localized());

                OutputText("Scenes.Aiko.EncounterAiko.PaleFlameDescends".Localized());

                OutputText("Scenes.Aiko.EncounterAiko.BrilliantFlash".Localized());

                OutputText("Scenes.Aiko.EncounterAiko.ReadyWeapon".Localized());

                OutputText("Scenes.Aiko.EncounterAiko.EveryoneConfused".Localized());

                if (Player.KitsuneScore() < 4)
                    OutputText("Scenes.Aiko.EncounterAiko.ExplainTrespassing".Localized());
                else
                    if (Player.IsNineTails())
                        OutputText("Scenes.Aiko.EncounterAiko.StopsWidensEyes".Localized(
                             Player.Mf("lord", "lady"),

                             Player.Mf("Lord", "Lady"),

                             Player.Mf("Priest", "Priestess")
                        ));
                    else
                        OutputText("Scenes.Aiko.EncounterAiko.ExplainSameKind".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), AikoAggroGreeting);

                AddButton(1, "Common.Button.No".Localized(), AikoPeaceGreeeting);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AikoHotBlood) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AikoTimesMet) > 3 && silly && aikoCorruption < 50 && Random.Shared.Next(15) == 0)
                {
                    OutputText("Scenes.Aiko.EncounterAiko.FamiliarTrail".Localized());

                    OutputText("Scenes.Aiko.EncounterAiko.InvestigateSound".Localized());

                    OutputText("Scenes.Aiko.EncounterAiko.HugeTentacleBeast".Localized());

                    OutputText("Scenes.Aiko.EncounterAiko.BlueProjectiles".Localized());

                    OutputText("Scenes.Aiko.EncounterAiko.ShoutsDontTakeLightly".Localized());

                    OutputText("Scenes.Aiko.EncounterAiko.LassHoldsBeast".Localized());

                    OutputText("Scenes.Aiko.EncounterAiko.BitOverTheTop".Localized());

                    OutputText("Scenes.Aiko.EncounterAiko.BeastSmoldering".Localized());

                    OutputText("Scenes.Aiko.EncounterAiko.ChantsBirthday".Localized());

                    OutputText("Scenes.Aiko.EncounterAiko.WonderingAbsurdity".Localized());

                    OutputText("Scenes.Aiko.EncounterAiko.LeftStoveOn".Localized());

                    OutputText("Scenes.Aiko.EncounterAiko.DashesOff".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.AikoHotBlood, CoC.Instance.Flags.Get(GameFlagIds.AikoHotBlood) + 1);

                    DoNext(() => ReturnToCamp(1));
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AikoTimesMet) == 2)
                    {
                        OutputText("Scenes.Aiko.EncounterAiko.DejaVu".Localized());

                        OutputText("Scenes.Aiko.EncounterAiko.NearlyJump".Localized(
                             CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 55 ? "fondness" : (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) > 50 ? "familiarity" : "contempt")
                        ));

                        if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 55)
                        {
                            OutputText("Scenes.Aiko.EncounterAiko.PleasantlySurprised".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.Talk".Localized(), AikoTalkE2);

                            AddButton(1, "Common.Button.Sex".Localized(), AikoSexE2);

                            AddButton(2, "Common.Button.Fight".Localized(), AikoFight);

                            AddButton(14, "Common.Button.Leave".Localized(), Leave);
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) > 50)
                            {
                                OutputText("Scenes.Aiko.EncounterAiko.EnjoyPresent".Localized());

                                ClearButtons();

                                AddButton(0, "Common.Button.Talk".Localized(), AikoTalkE2);

                                AddButton(1, "Common.Button.Sex".Localized(), AikoSexE2);

                                AddButton(2, "Common.Button.Fight".Localized(), AikoFight);

                                AddButton(14, "Common.Button.Leave".Localized(), Leave);
                            }
                            else
                            {
                                OutputText("Scenes.Aiko.EncounterAiko.GotSomeNerve".Localized(
                                     CoC.Instance.Flags.Get(GameFlagIds.AikoFightWon) > 0 ? "You tell her that she seems pretty high and mighty for someone who lost so handily the last time. " + (silly ? "Maybe, you suggest, she should check herself, before--and you quote--she wrecks herself. " : "") : "She seems pretty confident that she can take you again, after her previous victory. "
                                ));

                                ClearButtons();

                                AddButton(0, "Common.Button.Apologize".Localized(), AikoApologize1);

                                AddButton(1, "Common.Button.Sex".Localized(), AikoSexE2);

                                AddButton(2, "Common.Button.Fight".Localized(), AikoFight);

                                AddButton(14, "Common.Button.Leave".Localized(), Leave);
                            }
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.AikoTimesMet) == 3)
                        {
                            OutputText("Scenes.Aiko.EncounterAiko.AncientTreeClearing".Localized(
                                 !Player.HasMastery(new Identifier("coc", "Bow")) ? "She's quite good!" : (Player.MasteryLevel(new Identifier("coc", "Bow")) < 4 ? "She's quite good, " + (Player.HasStatusEffect(StatusEffectIds.Kelt) ? "though perhaps not as good as Kelt. Still, you might stand to learn a thing or two from her, and she's probably better company than the arrogant centaur." : "and you could probably learn a thing or two from her. If nothing else, it'd be more enjoyable than practicing by yourself.") : "She's quite good, but not as good as " + (Player.HasStatusEffect(StatusEffectIds.Kelt) ? "Kelt or yourself" : "you") + ". She might make a decent practice partner, but you probably wouldn't learn anything new from her.")
                            ));

                            if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
                            {
                                OutputText("Scenes.Aiko.EncounterAiko.WasJustPracticing".Localized());

                                OutputText("Scenes.Aiko.EncounterAiko.TellHerItsFine".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.AikoRape) < 1)
                                {
                                    OutputText("Scenes.Aiko.EncounterAiko.SmilesDismissesBow".Localized());

                                    ClearButtons();

                                    AddButton(0, "Common.Button.Talk".Localized(), AikoTalkE3);

                                    AddButton(1, "Common.Button.Sex".Localized(), AikoSex);

                                    AddButton(2, "Common.Button.Spar".Localized(), AikoSpar);

                                    AddButton(3, "Common.Button.Fight".Localized(), AikoFight);

                                    AddButton(14, "Common.Button.Leave".Localized(), Leave);
                                }
                                else
                                {
                                    OutputText("Scenes.Aiko.EncounterAiko.NodsShrinksBack".Localized());

                                    OutputText("Scenes.Aiko.EncounterAiko.WontRapeAgain".Localized());

                                    OutputText("Scenes.Aiko.EncounterAiko.WhatWillYouDo".Localized());

                                    ClearButtons();

                                    AddButton(0, "Common.Button.Talk".Localized(), AikoTalkE3);

                                    AddButton(1, "Common.Button.Sex".Localized(), AikoSex);

                                    AddButton(2, "Common.Button.Fight".Localized(), AikoFight);

                                    AddButton(14, "Common.Button.Leave".Localized(), Leave);
                                }
                            }
                            else
                            {
                                OutputText("Scenes.Aiko.EncounterAiko.WontTakeHint".Localized());

                                ClearButtons();

                                AddButton(0, "Common.Button.Talk".Localized(), AikoTalkE3);

                                AddButton(1, "Common.Button.Sex".Localized(), AikoSex);

                                AddButton(2, "Common.Button.Fight".Localized(), AikoFight);

                                AddButton(14, "Common.Button.Leave".Localized(), Leave);
                            }
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.AikoBossComplete) > 0 && CoC.Instance.Flags.Get(GameFlagIds.AikoBossOutro) == 0)
                            {
                                OutputText("Scenes.Aiko.EncounterAiko.DeepwoodsBioluminescent".Localized());

                                CoC.Instance.Flags.Set(GameFlagIds.AikoBossOutro, 1);

                                DoNext(() => ReturnToCamp(1));
                            }
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.AikoBossComplete) == 0 && (Player.Level >= 25 || View.Time.Days >= 25 * 6) && CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) > 90 && CoC.Instance.Flags.Get(GameFlagIds.AikoTimesMet) > 3)
                                {
                                    if (CoC.Instance.Flags.Get(GameFlagIds.AikoBossIntro) > 0)
                                    {
                                        if (aikoCorruption < 50)
                                        {
                                            OutputText("Scenes.Aiko.EncounterAiko.CallOutReturn".Localized());

                                            ClearButtons();

                                            AddButton(0, "Common.Button.Talk".Localized(), YamataTalk);

                                            AddButton(1, "Common.Button.GoNow".Localized(), YamataStart);

                                            AddButton(2, "Common.Button.Prepare".Localized(), YamataPrepare);
                                        }
                                        else
                                        {
                                            OutputText("Scenes.Aiko.EncounterAiko.EnterClearingCall".Localized());

                                            OutputText("Scenes.Aiko.EncounterAiko.HalfSisterReady".Localized());

                                            ClearButtons();

                                            AddButton(0, "Common.Button.Talk".Localized(), YamataTalk);

                                            AddButton(1, "Common.Button.GoNow".Localized(), YamataStart);

                                            AddButton(2, "Common.Button.Prepare".Localized(), YamataPrepare);
                                        }
                                    }
                                    else
                                        if (aikoCorruption < 50)
                                        {
                                            OutputText("Scenes.Aiko.EncounterAiko.HearsBall".Localized());

                                            OutputText("Scenes.Aiko.EncounterAiko.ItsMyBall".Localized());

                                            OutputText("Scenes.Aiko.EncounterAiko.LooksUpTearful".Localized());

                                            OutputText("Scenes.Aiko.EncounterAiko.WhatWillYouDoAgain".Localized());

                                            CoC.Instance.Flags.Set(GameFlagIds.AikoBossIntro, CoC.Instance.Flags.Get(GameFlagIds.AikoBossIntro) + 1);

                                            ClearButtons();

                                            AddButton(0, "Common.Button.GoNow".Localized(), YamataStart);

                                            AddButton(1, "Common.Button.Prepare".Localized(), YamataPrepare);
                                        }
                                        else
                                        {
                                            OutputText("Scenes.Aiko.EncounterAiko.HackingAway".Localized());

                                            OutputText("Scenes.Aiko.EncounterAiko.DontSneakUp".Localized());

                                            OutputText("Scenes.Aiko.EncounterAiko.NoDoubtSheWould".Localized());

                                            CoC.Instance.Flags.Set(GameFlagIds.AikoBossIntro, CoC.Instance.Flags.Get(GameFlagIds.AikoBossIntro) + 1);

                                            ClearButtons();

                                            AddButton(0, "Common.Button.GoNow".Localized(), YamataStart);

                                            AddButton(1, "Common.Button.Prepare".Localized(), YamataPrepare);
                                        }
                                }
                                else
                                    AikoMenu();
        }

        public void AikoMenu()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoMenu.EnterAncientTree".Localized());

            int temp = 0;

            if (aikoCorruption < 50)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
                {
                    temp = (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50 ? Random.Shared.Next(3) : Random.Shared.Next(2) + 1);

                    if (temp == 0)
                        OutputText("Scenes.Aiko.AikoMenu.DynamicEntry".Localized(
                             Player.Strength > 60 ? "The impact startles you, but you maintain your balance and help her down to the ground." : (Player.Strength >= 30 ? "The impact knocks the wind out of you, but thankfully Aiko is pretty light, and you manage to get her to the ground safely." : "You buckle under the force of the impact, and wind up on your face with Aiko sitting on your back. [say: Heheh, oops. Sorry!] she says as she helps you up.")
                        ));

                    if (temp == 1)
                        OutputText("Scenes.Aiko.AikoMenu.PriestessEmerges".Localized());

                    if (temp == 2)
                        OutputText("Scenes.Aiko.AikoMenu.SheSwingsDown".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 75)
                        OutputText("Scenes.Aiko.AikoMenu.QuickHug".Localized());

                    OutputText("Scenes.Aiko.AikoMenu.ClearingGlow".Localized(
                         aikoCorruption <= 20 ? "She then walks over to the great tree and casually leans against it, her arms behind her back as she smiles at you cutely. She's every bit as peppy and effervescent as always." : (aikoCorruption <= 40 ? "She then walks up to the tree, seeming a bit less energetic than usual. The old spark in her eyes has gone dull, but in its place there is a glimmer of something... else. You wonder if you're starting to rub off on her a little." : "She has an odd sway in her step, and something about her just seems... off. You're beginning to suspect that hanging around you has started to seriously affect her personality.")
                    ));
                }
                else
                    OutputText("Scenes.Aiko.AikoMenu.DuckArrow".Localized(
                         aikoCorruption <= 20 ? "You can see that same spark of life and pride in her eyes as always." : (aikoCorruption <= 40 ? "The spark of pride and life is gone from her eyes, replaced with something colder. You have your suspicions that she has you to thank for that." : "She seems to have an unsettling sway in her gait, like she's just a little off balance. There is a look in her eye that suggests her mind might be going slightly unhinged as well.")
                    ));

                if (CoC.Instance.Flags.Get(GameFlagIds.AikoTimesMet) == 4)
                {
                    OutputText("Scenes.Aiko.AikoMenu.ConfidentNow".Localized());

                    OutputText("Scenes.Aiko.AikoMenu.GreatTreeAdded".Localized());
                }

                ClearButtons();

                AddButton(0, "Common.Button.Appearance".Localized(), AikoAppearance);

                AddButton(1, "Common.Button.Talk".Localized(), AikoTalk);

                AddButton(2, "Common.Button.Sex".Localized(), AikoSex);

                AddButton(3, "Common.Button.Spar".Localized(), AikoSpar);

                AddButton(5, "Common.Button.TalkSex".Localized(), AikoTalkAndSex);

                AddButton(6, "Common.Button.Fight".Localized(), AikoFight);

                AddButton(14, "Common.Button.Leave".Localized(), Leave);
            }
            else
            {
                temp = Random.Shared.Next(3);

                if (temp == 0)
                    OutputText("Scenes.Aiko.AikoMenu.DerangedAppears".Localized());

                if (temp == 1)
                    OutputText("Scenes.Aiko.AikoMenu.CrazedDragsOut".Localized());

                if (temp == 2)
                    OutputText("Scenes.Aiko.AikoMenu.BillhookScythes".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 60)
                    OutputText("Scenes.Aiko.AikoMenu.BackToPlay".Localized());
                else
                    OutputText("Scenes.Aiko.AikoMenu.BackAgain".Localized());

                OutputText("Scenes.Aiko.AikoMenu.LeansOnTree".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Appearance".Localized(), AikoAppearance);

                AddButton(1, "Common.Button.Talk".Localized(), AikoTalk);

                AddButton(2, "Common.Button.Submit".Localized(), SubmitToAiko);

                AddButton(3, "Scenes.Aiko.AikoMenu.ButtonFight4Rape".Localized(), AikoFight);

                AddButton(5, "Common.Button.TalkSex".Localized(), AikoTalkAndSex);

                AddButton(14, "Common.Button.Leave".Localized(), Leave);
            }
        }

        public void AikoAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoAppearance.AikoSmiles".Localized());

            OutputText("Scenes.Aiko.AikoAppearance.TattoosLine".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoSexed) != 0 || CoC.Instance.Flags.Get(GameFlagIds.AikoRape) != 0)
                OutputText("Scenes.Aiko.AikoAppearance.GirlishCurves".Localized());

            OutputText("Scenes.Aiko.AikoAppearance.AssuresCanFight".Localized());

            DoNext(() => AikoMenu());
        }

        public void Leave()
        {

            Player.RemoveStatusEffect(StatusEffectIds.Spar);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void AikoAggroGreeting()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoAggroGreeting.ThreatenBack".Localized());

            OutputText("Scenes.Aiko.AikoAggroGreeting.CoyMaybeHasty".Localized());

            OutputText("Scenes.Aiko.AikoAggroGreeting.BallImportant".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Riches".Localized(), AikoE1Riches);

            AddButton(1, "Common.Button.Power".Localized(), AikoE1Power);

            AddButton(2, "Common.Button.Sex".Localized(), AikoE1SexPart1);

            AddButton(3, "Common.Button.Nothing".Localized(), AikoE1Nothing);

            AddButton(4, "Common.Button.Fight".Localized(), AikoE1Fight);

            AddButton(5, "Common.Button.FluffyTail".Localized(), AikoTouchFluffTail);
        }

        public void AikoPeaceGreeeting()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoPeaceGreeeting.WonderThreat".Localized());

            OutputText("Scenes.Aiko.AikoPeaceGreeeting.BallImportant".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Riches".Localized(), AikoE1Riches);

            AddButton(1, "Common.Button.Power".Localized(), AikoE1Power);

            AddButton(2, "Common.Button.Sex".Localized(), AikoE1SexPart1);

            AddButton(3, "Common.Button.Nothing".Localized(), AikoE1Nothing);

            AddButton(4, "Common.Button.FluffyTail".Localized(), AikoTouchFluffTail);
        }

        public void AikoE1Riches()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoE1Riches.ConsiderOffer".Localized());

            OutputText("Scenes.Aiko.AikoE1Riches.AvariciousScamp".Localized());

            OutputText("Scenes.Aiko.AikoE1Riches.CountChest".Localized());

            OutputText("Scenes.Aiko.AikoE1Riches.BeenHad".Localized());

            if (!Player.HasKeyItem("Camp - Chest"))
            {
                Player.CreateKeyItem("Camp - Chest", 0, 0, 0, 0);

                OutputText("Scenes.Aiko.AikoE1Riches.UseChestStorage".Localized());

                OutputText("Scenes.Aiko.AikoE1Riches.StorageSlots".Localized(
                     NumberUtils.Num2Text(Player.Inventory.MaxSlots)
                ));
            }
            else
                OutputText("Scenes.Aiko.AikoE1Riches.ChestTransforms".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoBallReturned, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AikoFirstChoice, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void AikoE1Power()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoE1Power.TellMorePower".Localized());

            OutputText("Scenes.Aiko.AikoE1Power.TeeheeHero".Localized(
                 Player.Mf("Mr.", "Mrs.")
            ));

            OutputText("Scenes.Aiko.AikoE1Power.BeenHad".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoBallReturned, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AikoFirstChoice, 2);

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 1);

            Player.Inventory.Add(ItemRegistry.Create(WeaponIds.Pipe), 1);

ReturnToCamp(1);
        }

        public void AikoE1SexPart1()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoE1SexPart.EyeHerBody".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoBallReturned, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AikoFirstChoice, 3);

            if (Player.HasCock() && Player.HasVagina())
            {
                OutputText("Scenes.Aiko.AikoE1SexPart.ExaminesLoins".Localized());

                ClearButtons();

                AddButton(0, "Scenes.Aiko.AikoE1SexPart.ButtonMale".Localized(), AikoSexBJ);

                AddButton(1, "Scenes.Aiko.AikoE1SexPart.ButtonFemale".Localized(), AikoSexCunni);
            }
            else
                if (Player.HasCock())
                    DoNext(() => AikoSexBJ());
                else
                    if (Player.HasVagina())
                        DoNext(() => AikoSexCunni());
                    else
                    {
                        OutputText("Scenes.Aiko.AikoE1SexPart.NoUsualEquipment".Localized());

                        DoNext(() => AikoE1SexPart2());
                    }
        }

        public void AikoE1SexPart2()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoE1SexPart.BlinkConfused".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoBallReturned, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 2);

            DoNext(() => ApplyAikoLustPrankEffect());
        }

        public void ApplyAikoLustPrankEffect()
        {

            Player.CreateStatusEffect(StatusEffectIds.AikoLustPrank, Random.Shared.Next(48) + 24, 0, 0, 0);

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 35f));

            DoNext(() => ReturnToCamp(1));
        }

        public void AikoE1Nothing()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoE1Nothing.DontKnowWhy".Localized());

            OutputText("Scenes.Aiko.AikoE1Nothing.HandItToHer".Localized());

            OutputText("Scenes.Aiko.AikoE1Nothing.JustGivingIt".Localized());

            OutputText("Scenes.Aiko.AikoE1Nothing.WalksAwaySmiling".Localized());

            OutputText("Scenes.Aiko.AikoE1Nothing.TurnBackCamp".Localized());

            OutputText("Scenes.Aiko.AikoE1Nothing.FieryKiss".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f), new DynStat(GameStats.Corruption, -5f));

            var temp= Random.Shared.Next(4);

            if (temp == 0)
            {
                OutputText("Scenes.Aiko.AikoE1Nothing.CouldArmWrestle".Localized());

                Player.ApplyStats(new DynStat(GameStats.Strength, Random.Shared.Next(4) + 1));
            }

            if (temp == 1)
            {
                OutputText("Scenes.Aiko.AikoE1Nothing.CouldBrushOff".Localized());

                Player.ApplyStats(new DynStat(GameStats.Toughness, Random.Shared.Next(4) + 1));
            }

            if (temp == 2)
            {
                OutputText("Scenes.Aiko.AikoE1Nothing.CouldOutrun".Localized());

                Player.ApplyStats(new DynStat(GameStats.Speed, Random.Shared.Next(4) + 1));
            }

            if (temp == 3)
            {
                OutputText("Scenes.Aiko.AikoE1Nothing.CouldMemorize".Localized());

                Player.ApplyStats(new DynStat(GameStats.Intelligence, Random.Shared.Next(4) + 1));
            }

            CoC.Instance.Flags.Set(GameFlagIds.AikoBallReturned, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AikoFirstChoice, 4);

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 5);

            DoNext(() => ReturnToCamp(1));
        }

        public void AikoTouchFluffTail()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoTouchFluffTail.ShoutsBlush".Localized());

            OutputText("Scenes.Aiko.AikoTouchFluffTail.RepeatPetTails".Localized());

            OutputText("Scenes.Aiko.AikoTouchFluffTail.TryingToHide".Localized());

            OutputText("Scenes.Aiko.AikoTouchFluffTail.UnsureWhatPrompted".Localized());

            OutputText("Scenes.Aiko.AikoTouchFluffTail.PleaseBeGentle".Localized());

            OutputText("Scenes.Aiko.AikoTouchFluffTail.NoIdeaWhat".Localized());

            OutputText("Scenes.Aiko.AikoTouchFluffTail.LookAtAiko".Localized());

            OutputText("Scenes.Aiko.AikoTouchFluffTail.UnderstandingDawns".Localized());

            OutputText("Scenes.Aiko.AikoTouchFluffTail.ComeBackSoon".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoBallReturned, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AikoFirstChoice, 6);

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 15);

            Player.ApplyStats(new DynStat(GameStats.Lust, 10 + Random.Shared.Next(11)), new DynStat(GameStats.Corruption, -5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void AikoE1Fight()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoE1Fight.ApparentWontBackDown".Localized());

            OutputText("Scenes.Aiko.AikoE1Fight.HopedToAvoidFight".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoFirstChoice, 5);

            CoC.Instance.Flags.Set(GameFlagIds.AikoHasFought, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) - 2);

            CombatScene.Start(EnemyRegistry.Create((CoC.Instance.Flags.Get(GameFlagIds.AikoCorruption) < 50 ? EnemyIds.AikoPure : EnemyIds.AikoCorrupt)), () => Go<CampScene>());
        }

        public void AikoLosesIntro()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoLosesIntro.DropsToOneKnee".Localized());

            OutputText("Scenes.Aiko.AikoLosesIntro.StrongerThanThought".Localized());

            OutputText("Scenes.Aiko.AikoLosesIntro.WillYouRapeHer".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoFightWon, 1);

            AikoRapeSex();
        }

        public void AikoWinsIntro()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoWinsIntro.SlumpToGround".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoTimesMet) == 1)
            {
                if (Player.KitsuneScore() >= 4)
                {
                    if (Player.IsNineTails())
                        OutputText("Scenes.Aiko.AikoWinsIntro.DismayDefeatedNineTails".Localized());
                    else
                        OutputText("Scenes.Aiko.AikoWinsIntro.DidntWantTo".Localized());
                }
                else
                {
                    OutputText("Scenes.Aiko.AikoWinsIntro.BroughtOnYourself".Localized());

                    OutputText("Scenes.Aiko.AikoWinsIntro.EightHoursPass".Localized());
                }
            }
            else
                OutputText("Scenes.Aiko.AikoWinsIntro.StandsOverIncantation".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoBallReturned, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void AikoRequestsBallAfterLose()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoRequestsBallAfterLose.TurnAwayBut".Localized());

            OutputText("Scenes.Aiko.AikoRequestsBallAfterLose.NeedMyBall".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Aiko.AikoRequestsBallAfterLose.ButtonGive".Localized(), AikoLoseGiveBall);

            AddButton(1, "Scenes.Aiko.AikoRequestsBallAfterLose.ButtonDontGive".Localized(), AikoLoseRefuseBall);
        }

        public void AikoLoseGiveBall()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoLoseGiveBall.LookDownPleading".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoBallReturned, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 2);

            CombatScene.CleanupAfterCombat();
        }

        public void AikoLoseRefuseBall()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoLoseRefuseBall.MadeItSomeDistance".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoBallReturned, 2);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            CombatScene.CleanupAfterCombat();
        }

        public void AikoTalkE2()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoTalkE2.JustWantToTalk".Localized(
                 Player.IsPureEnough(33) ? (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 57 ? "but you don't want to be impolite, so you carefully pick out a few pieces that look tasty and give her a smile. It's surprisingly good, and she seems pleased that you liked it." : "but you don't want to be impolite, so you carefully take a piece and put it in your mouth and... SPICY Aiko giggles at your unfortunate circumstances as you fan your mouth desperately to no avail, and you can't help feeling she may have planned it from the start. At least she's nice enough to offer you a drink.") : "so you give her a slightly disgusted look and push the box back over to her. She seems a little insulted, but doesn't bring it up again."
            ));

            Player.RefillHunger(10);

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 3);

            aikoCorruption -= 5;

            DoNext(() => ReturnToCamp(1));
        }

        public void AikoSexE2()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoSexE.AskIfInterested".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 55)
            {
                OutputText("Scenes.Aiko.AikoSexE.SmilesGiggles".Localized());

                OutputText("Scenes.Aiko.AikoSexE.WhatKindOfGirl".Localized());

                AikoConsSex();
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
                {
                    OutputText("Scenes.Aiko.AikoSexE.StraightToPoint".Localized());

                    AikoConsSex();
                }
                else
                {
                    OutputText("Scenes.Aiko.AikoSexE.SneersAfterWhatYouDid".Localized(
                         Player.Mf("bastard", "bitch")
                    ));

                    OutputText("Scenes.Aiko.AikoSexE.DidntAppreciate".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Force".Localized(), AikoFight);

                    AddButton(14, "Common.Button.Leave".Localized(), Leave);
                }
        }

        public void AikoApologize1()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoApologize.HandsUpNonThreatening".Localized());

            OutputText("Scenes.Aiko.AikoApologize1.ArrowTrainedSoftens".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Aiko.AikoApologize.ButtonSincere".Localized(), AikoApologySincere);

            AddButton(1, "Scenes.Aiko.AikoApologize.ButtonTrickHer".Localized(), AikoApologyTrick);
        }

        public void AikoApologySincere()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoApologySincere.OffersHand".Localized());

            OutputText("Scenes.Aiko.AikoApologySincere.NeedsTime".Localized());

            OutputText("Scenes.Aiko.AikoApologySincere.TellHerFine".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -5f));

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, 50);

            aikoCorruption -= 5;

            DoNext(() => ReturnToCamp(1));
        }

        public void AikoApologyTrick()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoApologyTrick.OffersHand".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) - 8);

            aikoCorruption += 10;

            if (Player.Strength > 35)
            {
                OutputText("Scenes.Aiko.AikoApologyTrick.ScreamsStruggles".Localized());

                OutputText("Scenes.Aiko.AikoApologyTrick.InTears".Localized(
                     Player.Mf("bastard", "cunt")
                ));

                OutputText("Scenes.Aiko.AikoApologyTrick.SwiftSmack".Localized());

                AikoRapeSex();
            }
            else
            {
                OutputText("Scenes.Aiko.AikoApologyTrick.FlamingSlap".Localized());

                OutputText("Scenes.Aiko.AikoApologyTrick.PsychoSpits".Localized());

                var me = EnemyRegistry.Create((CoC.Instance.Flags.Get(GameFlagIds.AikoCorruption) < 50 ? EnemyIds.AikoPure : EnemyIds.AikoCorrupt));

                me.OnDefeated = (hpVictory) => PcWinsDomFight(hpVictory);

                me.OnWon = (hpVictory) => PcLosesDomFight(hpVictory);

                CombatScene.Start(me, () => Go<CampScene>());
            }
        }

        public void AikoTalkE3()
        {
            ClearOutput();

            var bowSkill= Player.MasteryLevel(new Identifier("coc", "Bow"));

            var hasBow= (Player.HasKeyItem("Bow") || Player.HasKeyItem("Kelt's Bow"));

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
            {
                OutputText("Scenes.Aiko.AikoTalkE3.JustWantToTalk".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.TakingSeat".Localized());

                OutputText("Scenes.Aiko.AikoTalkE3.NoticedHandyBow".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.BlushesCompliment".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.LearnedFromMother".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.PressForMore".Localized());

                OutputText("Scenes.Aiko.AikoTalkE3.SomethingInCommon".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.ApologizePrying".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.SorryVillage".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.NotComfortable".Localized());

                if (hasBow)
                {
                    OutputText("Scenes.Aiko.AikoTalkE.PullOutBow".Localized());

                    OutputText("Scenes.Aiko.AikoTalkE.HourShooting".Localized());

                    if (bowSkill < 2)
                    {
                        OutputText("Scenes.Aiko.AikoTalkE.ThrashesYou".Localized());

                        Player.MasteryXP(new Identifier("coc", "Bow"), 10 + Random.Shared.Next(21));
                    }
                    else
                        if (bowSkill < 4)
                        {
                            OutputText("Scenes.Aiko.AikoTalkE.GoodRun".Localized());

                            Player.MasteryXP(new Identifier("coc", "Bow"), 5 + Random.Shared.Next(11));
                        }
                        else
                        {
                            OutputText("Scenes.Aiko.AikoTalkE.StiffCompetition".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 5);
                        }
                }
                else
                    OutputText("Scenes.Aiko.AikoTalkE.ConversationDrifts".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.HeadBackCamp".Localized());
            }
            else
            {
                OutputText("Scenes.Aiko.AikoTalkE.JustWantToTalk".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AikoApologySincere) == 2)
                {
                    OutputText("Scenes.Aiko.AikoTalkE.SquintsBowTrained".Localized());

                    OutputText("Scenes.Aiko.AikoTalkE.AssureDifferent".Localized());

                    OutputText("Scenes.Aiko.AikoTalkE.WhyBelieveYou".Localized());

                    if (silly)
                    {
                        OutputText("Scenes.Aiko.AikoTalkE.HoldFingerUp".Localized());

                        OutputText("Scenes.Aiko.AikoTalkE.WhomevOhOkay".Localized());

                        OutputText("Scenes.Aiko.AikoTalkE.ShutUp".Localized());

                        OutputText("Scenes.Aiko.AikoTalkE.SwearNothing".Localized());

                        OutputText("Scenes.Aiko.AikoTalkE3.FineStayThere".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Aiko.AikoTalkE.OneGoodReason".Localized());

                    OutputText("Scenes.Aiko.AikoTalkE.HoldUpHands".Localized());

                    OutputText("Scenes.Aiko.AikoTalkE.RelaxesABit".Localized());

                    OutputText("Scenes.Aiko.AikoTalkE3.FineStayThereAgain".Localized());
                }

                OutputText("Scenes.Aiko.AikoTalkE.AgreeToTerms".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.MotherTaughtMe".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.MeantNothingSort".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.InferFromTone".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.NotSoDifferent".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.SeeWhatYouveDone".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.WordsStingPress".Localized());

                OutputText("Scenes.Aiko.AikoTalkE3.DidntThinkMineOnly".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.AskWhyBigSecret".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.LikeOurPrivacy".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.ScathingAimedAtYou".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.DoesntBuyIt".Localized());

                OutputText("Scenes.Aiko.AikoTalkE.BackToCamp".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 3);

            aikoCorruption -= 5;

            DoNext(() => ReturnToCamp(1));
        }

        public void AikoApologize2()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoApologize.ShesRightSurprises".Localized());

            OutputText("Scenes.Aiko.AikoApologize.WrestlesCompromise".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 5);

            AikoConsSex();
        }

        public void AikoTalkAndSex()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoTalkAndSex.CameToTalkAndSex".Localized());

            OutputText("Scenes.Aiko.AikoTalkAndSex.SayFragment".Localized(
                 CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 65 ? "She smiles cheerfully and approaches you, leaning in to give you a light peck on the cheek. [say: Sounds like a plan to me,] she says, grinning and backing up a bit. [say: What did you want to talk about?]" : (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50 ? "She smiles cheerfully, nodding a little. [say: Sure, we can talk. Play your cards right and I just might take you up on that second part,] she says, but from her playful tone you can tell she's just teasing. [say: What did you have in mind?]" : "[say: Alright, we'll talk. We'll see if I feel like that second part after,] she says, crossing her arms. [say: Well, what did you want to talk about?]")
            ));

            talkAndSex = true;

            AikoTalk();
        }

        public void AikoTalk()
        {
            ClearOutput();

            if (!talkAndSex)
                OutputText("Scenes.Aiko.AikoTalk.JustCameToTalk".Localized(
                     CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 65 ? "She smiles cheerfully and approaches you, leaning in to give you a light peck on the cheek. [say: What did you want to talk about?] she says, grinning and backing up a bit." : (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50 ? "She smiles cheerfully, nodding a little. [say: Sure, we can talk. What did you have in mind?]" : "[say: Alright, we'll talk. Well, what did you want to talk about?]")
                ));

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 3);

            aikoCorruption -= 5;

            ClearButtons();

            AddButton(0, "Scenes.Aiko.AikoTalk.ButtonGiantBees".Localized(), TalkBees);

            AddButton(1, "Scenes.Aiko.AikoTalk.ButtonTentacleBeasts".Localized(), TalkTentacles);

            AddButton(2, "Scenes.Aiko.AikoTalk.ButtonGoblins".Localized(), TalkGoblins);

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionState) != 0)
                AddButton(3, "Common.Button.Akbal".Localized(), TalkAkbal);

            AddButton(4, "Scenes.Aiko.AikoTalk.ButtonKitsuneCulture".Localized(), TalkCulture);

            AddButton(5, "Common.Button.Aiko".Localized(), TalkAiko);

            AddButton(6, "Scenes.Aiko.AikoTalk.ButtonMansionSisters".Localized(), TalkMansionSisters);

            AddButton(7, "Scenes.Aiko.AikoTalk.ButtonNineTails".Localized(), TalkNineTails);

            AddButton(8, "Scenes.Aiko.AikoTalk.ButtonAikoFamily".Localized(), TalkFamily);

            AddButton(9, "Scenes.Aiko.AikoTalk.ButtonArchery".Localized(), TalkArchery);

            AddButton(14, "Common.Button.Back".Localized(), () => AikoMenu());
        }

        public void TalkBees()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.TalkBees.AskLocalFauna".Localized());

            OutputText("Scenes.Aiko.TalkBees.GiantBees".Localized());

            if (Player.BeeScore() >= 4)
            {
                OutputText("Scenes.Aiko.TalkBees.NotWhatYouWanted".Localized(
                     Player.Mf("If you're looking for a mate, you've gone too far.", "N-No eggs, please... not again.")
                ));

                OutputText("Scenes.Aiko.TalkBees.CuttingOffApologizing".Localized(
                     Player.Mf("visibly confused", "more than a little relieved")
                ));
            }
            else
                if (SaveContent(SceneIds.Forest.BeeGirl).GetInt("timesEgged", 0) > 0)
                    OutputText("Scenes.Aiko.TalkBees.FamiliarWithThem".Localized());
                else
                    if (Player.Corruption > 50)
                        OutputText("Scenes.Aiko.TalkBees.LittleWarmImagine".Localized());
                    else
                        OutputText("Scenes.Aiko.TalkBees.ShudderThinking".Localized());

            OutputText("Scenes.Aiko.TalkBees.VillagersBusiness".Localized());

            OutputText("Scenes.Aiko.TalkBees.DontKnowMuchElse".Localized());

            if (talkAndSex)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
                {
                    OutputText("Scenes.Aiko.TalkBees.ThatsAlrightDrift".Localized());

                    DoNext(() => AikoSex());
                }
                else
                {
                    OutputText("Scenes.Aiko.TalkBees.ThatsAlrightDriftAgain".Localized());

                    OutputText("Scenes.Aiko.TalkBees.MaybeFindBeeGirl".Localized());

                    DoNext(() => ReturnToCamp(1));
                }
            }
            else
            {
                OutputText("Scenes.Aiko.TalkBees.ThatsAlrightThanks".Localized());

                OutputText("Scenes.Aiko.TalkBees.BeCarefulOutThere".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void TalkTentacles()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.TalkTentacles.AskLocalFlora".Localized());

            OutputText("Scenes.Aiko.TalkTentacles.TheyreAlright".Localized());

            OutputText("Scenes.Aiko.TalkTentacles.WaitHadOne".Localized());

            OutputText("Scenes.Aiko.TalkTentacles.WildOnes".Localized());

            OutputText("Scenes.Aiko.TalkTentacles.ConsideringTemperaments".Localized());

            OutputText("Scenes.Aiko.TalkTentacles.HardToBelieve".Localized());

            OutputText("Scenes.Aiko.TalkTentacles.PeopleAssume".Localized());

            if (SaveContent(SceneIds.Forest.TentacleBeast).GetInt("timesLost", 0) > 0)
            {
                OutputText("Scenes.Aiko.TalkTentacles.FeelRisingWarmth".Localized());

                OutputText("Scenes.Aiko.TalkTentacles.GrinWider".Localized());
            }
            else
                OutputText("Scenes.Aiko.TalkTentacles.DontKnowHowItIs".Localized());

            OutputText("Scenes.Aiko.TalkTentacles.GetsWeirdSometimes".Localized());

            OutputText("Scenes.Aiko.TalkTentacles.AgreeDiscuss".Localized());

            if (talkAndSex)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
                {
                    OutputText("Scenes.Aiko.TalkTentacles.BeforeTellHer".Localized(
                         RandomUtils.Choice(["bloodiest fights you've had", "vicious monsters you've battled", "terrible demons you've encountered"])
                    ));

                    DoNext(() => AikoSex());
                }
                else
                {
                    OutputText("Scenes.Aiko.TalkTentacles.TellHerAllAbout".Localized(
                         RandomUtils.Choice(["bloodiest fights you've had", "vicious monsters you've battled", "terrible demons you've encountered"])
                    ));

                    OutputText("Scenes.Aiko.TalkTentacles.DontSeeMuchElse".Localized());

                    ReturnToCamp(1);
                }
            }
            else
                ReturnToCamp(1);
        }

        public void TalkGoblins()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.TalkGoblins.AskAboutGoblins".Localized());

            OutputText("Scenes.Aiko.TalkGoblins.ShrugsSadStory".Localized());

            OutputText("Scenes.Aiko.TalkGoblins.DontBotherUsMuch".Localized());

            OutputText("Scenes.Aiko.TalkGoblins.DiscussGoblinRunins".Localized());

            if (talkAndSex && CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
            {
                OutputText("Scenes.Aiko.TalkGoblins.WaitReallyAll".Localized());

                DoNext(() => AikoSex());
            }
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void TalkAkbal()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.TalkAkbal.AskAboutAkbal".Localized());

            OutputText("Scenes.Aiko.TalkAkbal.SpitsAkbalName".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) & (int)AkbalEvent.QuestDone) > 0)
            {
                OutputText("Scenes.Aiko.TalkAkbal.CouldTellHowDealt".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Yes".Localized(), () => TalkAkbalAnswer(true));

                AddNextButton("Common.Button.No".Localized(), () => TalkAkbalAnswer(false));
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionState) == 2)
                    OutputText("Scenes.Aiko.TalkAkbal.PainsToHearLies".Localized());
                else
                    OutputText("Scenes.Aiko.TalkAkbal.SeemsPredatory".Localized());

                OutputText("Scenes.Aiko.TalkAkbal.EnemyOfForest".Localized());

                OutputText("Scenes.Aiko.TalkAkbal.NodSoakIn".Localized());

                TalkAkbal2();
            }
        }

        public void TalkAkbalAnswer(bool choice)
        {

            if (choice)
            {
                OutputText("Scenes.Aiko.TalkAkbalAnswer.ListensEveryWord".Localized(
                     CoC.Instance.Flags.Get(GameFlagIds.AkkyName) == "" ? "how you've slain the beast" : "his new form: a cute, fuzzy kitten " + (CoC.Instance.Flags.Get(GameFlagIds.AkkyName) != "Akbal" ? "named [akky]" : "whose name is the only thing that ties him to his old life")
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.AikoBossComplete) == 1)
                    OutputText("Scenes.Aiko.TalkAkbalAnswer.GlancesNeverFree".Localized());
                else
                    OutputText("Scenes.Aiko.TalkAkbalAnswer.WantToBelieve".Localized());

                Save.SetBool("akbalDone", true);
            }
            else
            {
                OutputText("Scenes.Aiko.TalkAkbalAnswer.DoesntMatterWhile".Localized());

                OutputText("Scenes.Aiko.TalkAkbalAnswer.StillCuriousWhy".Localized());

                OutputText("Scenes.Aiko.TalkAkbalAnswer.EnemyOfForest".Localized());

                OutputText("Scenes.Aiko.TalkAkbalAnswer.NodSoakIn".Localized());
            }

            TalkAkbal2();
        }

        public void TalkAkbal2()
        {

            if (talkAndSex && CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
            {
                if ((CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) & (int)AkbalEvent.QuestDone) > 0)
                    OutputText("Scenes.Aiko.TalkAkbal.TailsWindingAround".Localized());
                else
                    OutputText("Scenes.Aiko.TalkAkbal.SorryForGloom".Localized());

                DoNext(() => AikoSex());
            }
            else
            {
                OutputText("Scenes.Aiko.TalkAkbal.ThankingHaveToGo".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void TalkCulture()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.TalkCulture.InterestedLearning".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoTalkCulture) < 1 || Player.HasPerk(PerkIds.CorruptedNinetails) || Player.HasPerk(PerkIds.EnlightenedNinetails))
            {
                OutputText("Scenes.Aiko.TalkCulture.SayFragment".Localized());

                OutputText("Scenes.Aiko.TalkCulture.JustCurious".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AikoTalkCulture) >= 1)
                    OutputText("Scenes.Aiko.TalkCulture.Again".Localized());

            OutputText("Scenes.Aiko.TalkCulture.WhereToStart".Localized());

            OutputText("Scenes.Aiko.TalkCulture.BeMoreSpecific".Localized());

            OutputText("Scenes.Aiko.TalkCulture.SatisfyingExperience".Localized());

            OutputText("Scenes.Aiko.TalkCulture.AskIfMeans".Localized());

            OutputText("Scenes.Aiko.TalkCulture.SexuallyYes".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
                OutputText("Scenes.Aiko.TalkCulture.ThinkingBackPranks".Localized(
                     CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 65 ? "A blush creeps across her face as she says, [say: That's one possibility...]" : "[say: Could be,] she says, her grin as unreadable as ever. [say: Or maybe" + (Player.HasPerk(PerkIds.CorruptedNinetails) || Player.HasPerk(PerkIds.EnlightenedNinetails) ? "..." : " you're just gullible.") + "]"
                ));

            OutputText("Scenes.Aiko.TalkCulture.AnotherThought".Localized(
                 Player.Mf(Player.KitsuneScore() >= 4 ? "another" : "a", "a"),

                 Player.Mf(Player.KitsuneScore() >= 4 ? " almost" : "", "")
            ));

            if (Player.KitsuneScore() >= 4)
            {
                OutputText("Scenes.Aiko.TalkCulture.GlancesContinues".Localized());

                switch ((int)Player.Gender)
                {
                    case 0:
                    {
                    OutputText("Scenes.Aiko.TalkCulture.CuriousSmile".Localized());
                    }

                    break;

                    case 1:
                    {
                    OutputText("Scenes.Aiko.TalkCulture.KnowingPredatorySmile".Localized());
                    }

                    break;

                    case 2:
                    {
                    OutputText("Scenes.Aiko.TalkCulture.SaysWithShrug".Localized());
                    }

                    break;

                    case 3:
                    {
                    OutputText("Scenes.Aiko.TalkCulture.KnowingPredatorySmileAgain".Localized());
                    }

                    break;
                }

                if (true )
                    OutputText("Scenes.Aiko.TalkCulture.ErrorInvalidGender".Localized());

                OutputText("Scenes.Aiko.TalkCulture.NodUnderstandBetter".Localized());
            }
            else
            {
                OutputText("Scenes.Aiko.TalkCulture.YesNoBoth".Localized());

                if (silly && Player.IsGoo())
                    OutputText("Scenes.Aiko.TalkCulture.CanRelateFluid".Localized());

                OutputText("Scenes.Aiko.TalkCulture.ThankHerHelp".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.AikoTalkCulture, CoC.Instance.Flags.Get(GameFlagIds.AikoTalkCulture) + 1);

            if (talkAndSex)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
                {
                    OutputText("Scenes.Aiko.TalkCulture.DriftsOtherTopics".Localized());

                    DoNext(() => AikoSex());
                }
                else
                {
                    OutputText("Scenes.Aiko.TalkCulture.DriftsOtherTopicsAgain".Localized());

                    DoNext(() => ReturnToCamp(1));
                }
            }
            else
            {
                OutputText("Scenes.Aiko.TalkCulture.ShouldBeGoing".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void TalkAiko()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.TalkAiko.PersonalQuestions".Localized());

            OutputText("Scenes.Aiko.TalkAiko.RepliesCrossesArms".Localized(
                 Player.KitsuneScore() >= 4 ? "[say: My Ball? You are a kitsune yourself, shouldn't you know how our spirit vessel works?] she says, widening her eyes. You tell her that you forgot how it works. She giggles a bit and then she playfully and lightly hits your forehead like she's knocking on a door. [say: Forgetful, aren't we? Hehehe. It's very important for us, let's see, where should I begin?[pg]" : "[say: Oh, my Ball... " + (CoC.Instance.Flags.Get(GameFlagIds.AikoTalkAiko) > 0 ? "I thought I explained that already? " : "") + "Every kitsune has one,] she explains. [say: It's difficult to explain to a non-kitsune, but... Let me see if this works.[pg]"
            ));

            OutputText("Scenes.Aiko.TalkAiko.DoesntSeemKeen".Localized());

            OutputText("Scenes.Aiko.TalkAiko.LotOfThem".Localized());

            if (Player.KitsuneScore() >= 4)
            {
                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox && Player.Body.Get<TailPart>().Venom > (CoC.Instance.Flags.Get(GameFlagIds.AikoBossComplete) > 0 ? 7 : 8))
                    OutputText("Scenes.Aiko.TalkAiko.NotAsStrong".Localized(
                         CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 65 ? "She blushes and looks away." : (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50 ? "Aiko mumbles something you can't make out." : "Aiko glares at you.")
                    ));
                else
                    OutputText("Scenes.Aiko.TalkAiko.SayFragment".Localized(
                         CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50 ? "She nods. [say: If you keep working at it, I'm sure you'll be " + (Player.Body.Get<TailPart>().Venom == 8 ? "an " : "a ") + NumberUtils.Num2Text(Player.Body.Get<TailPart>().Venom + 1) + "-tails one day.]" : "[say: Even someone like you can rise up the ranks,] Aiko says with a sigh. [say: Though we'd rather you didn't.]"
                    ));
            }
            else
                OutputText("Scenes.Aiko.TalkAiko.ExplainsNTails".Localized());

            OutputText("Scenes.Aiko.TalkAiko.PauseWondering".Localized());

            if (Player.KitsuneScore() >= 4)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) < 50)
                    OutputText("Scenes.Aiko.TalkAiko.GazeBores".Localized());
                else
                {
                    OutputText("Scenes.Aiko.TalkAiko.BacksAway".Localized());

                    OutputText("Scenes.Aiko.TalkAiko.JustCuriousAfterAll".Localized());

                    OutputText("Scenes.Aiko.TalkAiko.GlancesAroundNormal".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Aiko.TalkAiko.BlurtsNo".Localized());

                OutputText("Scenes.Aiko.TalkAiko.NodUnderstandingCheers".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.AikoTalkAiko, CoC.Instance.Flags.Get(GameFlagIds.AikoTalkAiko) + 1);

            if (talkAndSex && CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
            {
                if (Player.KitsuneScore() >= 4)
                    OutputText("Scenes.Aiko.TalkAiko.WrapUpConversation".Localized());
                else
                    OutputText("Scenes.Aiko.TalkAiko.WrapUpConversationAgain".Localized());

                DoNext(() => AikoSex());
            }
            else
            {
                OutputText("Scenes.Aiko.TalkAiko.WrapUpConversationThird".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void TalkMansionSisters()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.TalkMansionSisters.AskOtherKitsune".Localized());

            OutputText("Scenes.Aiko.TalkMansionSisters.ThoseThree".Localized());

            OutputText("Scenes.Aiko.TalkMansionSisters.PointOutHypocrisy".Localized());

            OutputText("Scenes.Aiko.TalkMansionSisters.ThatsDifferentGuardian".Localized());

            OutputText("Scenes.Aiko.TalkMansionSisters.DontGoOverboard".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MansionVisited) > 0)
            {
                OutputText("Scenes.Aiko.TalkMansionSisters.NotSureOverboard".Localized());

                OutputText("Scenes.Aiko.TalkMansionSisters.ShakesHead".Localized());
            }
            else
                OutputText("Scenes.Aiko.TalkMansionSisters.CuriousWhatSheMeans".Localized());

            OutputText("Scenes.Aiko.TalkMansionSisters.PastThreshold".Localized());

            OutputText("Scenes.Aiko.TalkMansionSisters.ThankForWarning".Localized());

            if (talkAndSex && CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
            {
                OutputText("Scenes.Aiko.TalkMansionSisters.MoodForMischief".Localized());

                DoNext(() => AikoSex());
            }
            else
            {
                OutputText("Scenes.Aiko.TalkMansionSisters.AwayFromCampTooLong".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void TalkNineTails()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.TalkNineTails.FoldsArms".Localized(
                 Player.HasPerk(PerkIds.CorruptedNinetails) || Player.HasPerk(PerkIds.EnlightenedNinetails) ? " and smiles" : (Player.KitsuneScore() >= 4 ? ", seemingly a little shocked you don't already know" : "")
            ));

            OutputText("Scenes.Aiko.TalkNineTails.OldestWisest".Localized());

            if (Player.HasPerk(PerkIds.CorruptedNinetails))
            {
                OutputText("Scenes.Aiko.TalkNineTails.AtLastSomeone".Localized());

                OutputText("Scenes.Aiko.TalkNineTails.MemoriesMakeTingle".Localized());

                OutputText("Scenes.Aiko.TalkNineTails.AllWorldIsYours".Localized());

                OutputText("Scenes.Aiko.TalkNineTails.LastNineTailsVillage".Localized());

                OutputText("Scenes.Aiko.TalkNineTails.GodsStillGoing".Localized());
            }
            else
            {
                OutputText("Scenes.Aiko.TalkNineTails.LastTailCeremony".Localized());

                OutputText("Scenes.Aiko.TalkNineTails.WhatAboutHer".Localized());

                OutputText("Scenes.Aiko.TalkNineTails.WasRetainer".Localized());

                OutputText("Scenes.Aiko.TalkNineTails.GiggleAtDistaste".Localized(
                     Player.HasPerk(PerkIds.CorruptedNinetails) || Player.HasPerk(PerkIds.EnlightenedNinetails) ? " Maybe she'd have rather been with you? " + (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 80 ? "[say: Definitely. But I can't, remember?]" : (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50 ? "She shrugs. [say: Who knows. But my guardian duties would have prevented it anyway.]" : "Aiko glares at you. [say: I'd rather be with her for a hundred years than with you for one.]")) : ""
                ));
            }

            if (talkAndSex && CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
            {
                if (Player.HasPerk(PerkIds.EnlightenedNinetails))
                    OutputText("Scenes.Aiko.TalkNineTails.ThankingInformation".Localized());
                else
                    OutputText("Scenes.Aiko.TalkNineTails.TurnToLeave".Localized());

                DoNext(() => AikoSex());
            }
            else
            {
                OutputText("Scenes.Aiko.TalkNineTails.HaveToGo".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void TalkFamily()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.TalkFamily.PersonalQuestions".Localized());

            OutputText("Scenes.Aiko.TalkFamily.RepliesCrossesArms".Localized());

            OutputText("Scenes.Aiko.TalkFamily.StartSimple".Localized());

            OutputText("Scenes.Aiko.TalkFamily.FamilyMotherElder".Localized());

            OutputText("Scenes.Aiko.TalkFamily.PuzzledLook".Localized());

            OutputText("Scenes.Aiko.TalkFamily.AskSiblings".Localized());

            OutputText("Scenes.Aiko.TalkFamily.IWellYes".Localized());

            OutputText("Scenes.Aiko.TalkFamily.DoesntNeedToTell".Localized(
                 CoC.Instance.Flags.Get(GameFlagIds.AikoBossComplete) > 0 ? "After [if (cor > 40) {half-heartedly}] apologizing for bringing up memories of her " + (CoC.Instance.Flags.Get(GameFlagIds.AikoBossComplete) == 1 ? "slain" : "murderous") + " sister, you" : (CoC.Instance.Flags.Get(GameFlagIds.AikoBossIntro) > 0 ? "[if (cor > 66) { You've long since forgotten about her problems, but you|It must have slipped your mind, and you}]" : "You")
            ));

            OutputText("Scenes.Aiko.TalkFamily.PreferItThatWay".Localized());

            if (talkAndSex && CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
            {
                OutputText("Scenes.Aiko.TalkFamily.IdeaToCheer".Localized());

                DoNext(() => AikoSex());
            }
            else
            {
                OutputText("Scenes.Aiko.TalkFamily.ChanceForExit".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void TalkArchery()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.TalkArchery.BringUpArchery".Localized());

            if (Player.HasKeyItem("Bow") || Player.HasKeyItem("Kelt's Bow"))
            {
                CoC.Instance.Flags.Set(GameFlagIds.AikoTalkArchery, CoC.Instance.Flags.Get(GameFlagIds.AikoTalkArchery) + 1);

                if (Player.MasteryLevel(new Identifier("coc", "Bow")) >= 4)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.AikoTalkArchery) == 1)
                    {
                        OutputText("Scenes.Aiko.TalkArchery.BeenTryingToLearn".Localized());

                        OutputText("Scenes.Aiko.TalkArchery.ExplainToHer".Localized(
                             Player.HasStatusEffect(StatusEffectIds.Kelt) ? "your dealings with Kelt, " + (Player.StatusEffectValue(StatusEffectIds.Kelt, 1) < 75 ? "remarking on the arrogant centaur's brash attitude and how awful it is putting up with him." : "sighing almost dreamily as you describe his rugged good looks and no-nonsense teaching methods.") : "You tell her [if (silly){it just materialized in your pack|you just found it}] one day, and you've been trying to teach yourself how to use it ever since."
                        ));

                        OutputText("Scenes.Aiko.TalkArchery.UseTargetRange".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Aiko.TalkArchery.WonderingGiveLessons".Localized());

                        OutputText("Scenes.Aiko.TalkArchery.NothingLeftToTeach".Localized());
                    }
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AikoTalkArchery) == 1)
                    {
                        OutputText("Scenes.Aiko.TalkArchery.BeenTryingToLearnAgain".Localized());

                        OutputText("Scenes.Aiko.TalkArchery.ExplainDealings".Localized(
                             CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50 ? "[say: I'd be happy to give you some pointers, [name].]" : (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 35 ? "[say: Okay, I'll teach you... Just don't make me regret this.][pg]" : "[say: There's no way I'm teaching [i: you] anything. Maybe you should try being nicer if you want my help.][pg]")
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Aiko.TalkArchery.WonderingGiveLessonsAgain".Localized());

                        OutputText("Scenes.Aiko.TalkArchery.SayFragment".Localized(
                             CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50 ? "She nods happily and gives you a cheerful smile, saying, [say: Of course, I'll go set up the targets!][pg]" : "She thinks about it for a second and then finally says, " + (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 35 ? "[say: Okay, I'll teach you some more.][pg]" : "[say: No, not today. You want free archery lessons, maybe you should try being nicer.][pg]")
                        ));
                    }

                if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 35)
                {
                    OutputText("Scenes.Aiko.TalkArchery.TakeTurnsShooting".Localized(
                         Player.StatusEffectValue(StatusEffectIds.Kelt, 1) >= 80 ? "Aiko is obviously less skilled than Kelt, and you find yourself anxious to be back under the true master's guidance." : (Player.HasStatusEffect(StatusEffectIds.Kelt) ? "Even if Kelt is a little more skilled, Aiko's cheery disposition more than makes up for it, and you find yourself grateful to have found an archery partner that isn't as arrogant, crude, and snide as he." : "")
                    ));

                    if (Player.MasteryLevel(new Identifier("coc", "Bow")) < 2)
                    {
                        OutputText("Scenes.Aiko.TalkArchery.SheStillBetter".Localized());

                        Player.MasteryXP(new Identifier("coc", "Bow"), 15 + Random.Shared.Next(31));
                    }
                    else
                        if (Player.MasteryLevel(new Identifier("coc", "Bow")) < 4)
                        {
                            OutputText("Scenes.Aiko.TalkArchery.YouGiveRunMoney".Localized());

                            Player.MasteryXP(new Identifier("coc", "Bow"), 5 + Random.Shared.Next(11));
                        }
                        else
                            OutputText("Scenes.Aiko.TalkArchery.SheWasRight".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Aiko.TalkArchery.InterestedLearningBow".Localized());

                OutputText("Scenes.Aiko.TalkArchery.NodApologize".Localized());
            }

            if (talkAndSex && CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
            {
                OutputText("Scenes.Aiko.TalkArchery.WaitSays".Localized());

                DoNext(() => AikoSex());
            }
            else
            {
                OutputText("Scenes.Aiko.TalkArchery.ShouldBeGettingBack".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void AikoSex()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoSex.LikeToHaveSex".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
            {
                OutputText("Scenes.Aiko.AikoSex.HappyToHear".Localized());

                OutputText("Scenes.Aiko.AikoSex.GladToHear".Localized());

                OutputText("Scenes.Aiko.AikoSex.ChuckleEager".Localized());

                AikoConsSex();
            }
            else
            {
                OutputText("Scenes.Aiko.AikoSex.ThickSkull".Localized());

                OutputText("Scenes.Aiko.AikoSex.StarvedEyes".Localized());

                OutputText("Scenes.Aiko.AikoSex.GotNeedsToo".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Apologize".Localized(), AikoApologize2);

                AddButton(1, "Common.Button.Force".Localized(), AikoFight);

                AddButton(14, "Common.Button.Leave".Localized(), Leave);
            }
        }

        public void AikoConsSex()
        {

            ClearButtons();

            if (Player.HasCock())
            {
                AddButton(0, "Scenes.Aiko.AikoConsSex.ButtonMissionary".Localized(), AikoSexMissionary);

                AddButton(1, "Scenes.Aiko.AikoConsSex.ButtonDoggyStyle".Localized(), AikoSexDoggy);

                AddButton(2, "Scenes.Aiko.AikoConsSex.ButtonBJ".Localized(), AikoSexBJ);
            }

            if (Player.HasVagina())
                AddButton(3, "Common.Button.GetLicked".Localized(), AikoSexCunni);

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 75 && Player.HasCock())
                AddButton(4, "Common.Button.Tailjob".Localized(), AikoSexTailjob);

            if (aikoCorruption >= 50)
                AddButton(5, "Common.Button.Submit".Localized(), SubmitToAiko);

            if (Player.HasStatusEffect(StatusEffectIds.Spar))
                AddButton(14, "Common.Button.Leave".Localized(), () => Leave());
            else
                AddButton(14, "Common.Button.Back".Localized(), () => AikoMenu());
        }

        public void AikoSpar()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoSpar.SuggestFriendlyCompetition".Localized());

            OutputText("Scenes.Aiko.AikoSpar.CompetitionEh".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 2);

            Player.CreateOrFindStatusEffect(StatusEffectIds.Spar);

            CombatScene.Start(EnemyRegistry.Create((CoC.Instance.Flags.Get(GameFlagIds.AikoCorruption) < 50 ? EnemyIds.AikoPure : EnemyIds.AikoCorrupt)), () => Go<CampScene>());
        }

        public void SparWithAikoWin()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.SparWithAikoWin.DropsOneKnee".Localized());

            OutputText("Scenes.Aiko.SparWithAikoWin.AdmitYoureGood".Localized());

            playerchoice = true;

            if (Player.HasStatusEffect(StatusEffectIds.Spar))
                CoC.Instance.Flags.Set(GameFlagIds.AikoSparVictories, CoC.Instance.Flags.Get(GameFlagIds.AikoSparVictories) + 1);

            AikoConsSex();
        }

        public void SparWithAikoLose(int aikoLust)
        {
            ClearOutput();

            OutputText("Scenes.Aiko.SparWithAikoLose.FallInHeap".Localized());

            playerchoice = false;

            var options = new List<Action>();

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 60 && aikoLust >= 60)
            {
                if (Player.HasCock())
                {
                    options.Add(() => AikoSexMissionary());

                    if (Player.CockThatFits(50) != -1)
                        options.Add(() => AikoSexDoggy());

                    options.Add(() => AikoSexBJ());
                }

                if (Player.HasVagina())
                    options.Add(() => AikoSexCunni());

                if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 75 && Player.HasCock())
                    options.Add(() => AikoSexTailjob());
            }

            if (options.Count > 0)
                options[Random.Shared.Next(options.Count)]();
            else
            {
                OutputText("Scenes.Aiko.SparWithAikoLose.NotInTheMood".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.Spar))
                    Player.RemoveStatusEffect(StatusEffectIds.Spar);

                CombatScene.CleanupAfterCombat();
            }
        }

        public void AikoFight()
        {
            ClearOutput();

            if (aikoCorruption >= 50)
                OutputText("Scenes.Aiko.AikoFight.FuckWhetherYouLike".Localized());
            else
            {
                OutputText("Scenes.Aiko.AikoFight.LungeWithoutWarning".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
                    OutputText("Scenes.Aiko.AikoFight.WhatTheHell".Localized());
                else
                    OutputText("Scenes.Aiko.AikoFight.DoItYourWay".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) - 2);
            }

            var me = EnemyRegistry.Create((CoC.Instance.Flags.Get(GameFlagIds.AikoCorruption) < 50 ? EnemyIds.AikoPure : EnemyIds.AikoCorrupt));

            me.OnDefeated = (hpVictory) => PcWinsDomFight(hpVictory);

            me.OnWon = (hpVictory) => PcLosesDomFight(hpVictory);

            CombatScene.Start(me, () => Go<CampScene>());
        }

        public void PcWinsDomFight(bool hpVictory)
        {
            ClearOutput();

            if (aikoCorruption >= 50)
            {
                OutputText("Scenes.Aiko.PcWinsDomFight.StumblesFalls".Localized());

                OutputText("Scenes.Aiko.PcWinsDomFight.KnowHowToShowGirl".Localized(
                     Player.Mf("limp-dicked choir boy", "washed-up whore")
                ));

                OutputText("Scenes.Aiko.PcWinsDomFight.KnowsJustToPiss".Localized());
            }
            else
            {
                OutputText("Scenes.Aiko.PcWinsDomFight.DropsOneKnee".Localized());

                OutputText("Scenes.Aiko.PcWinsDomFight.WhatWillYouDo".Localized());
            }

            AikoRapeSex();
        }

        public void PcLosesDomFight(bool hpVictory, bool pcCameWorms = false)
        {
            ClearOutput();

            if (aikoCorruption >= 50)
            {
                var x= Random.Shared.Next(Player.Cocks.Count);

                OutputText("Scenes.Aiko.PcLosesDomFight.FallToGround".Localized());

                OutputText("Scenes.Aiko.PcLosesDomFight.CracksKnuckles".Localized());

                OutputText("Scenes.Aiko.PcLosesDomFight.NotFunAsBitch".Localized());

                OutputText("Scenes.Aiko.PcLosesDomFight.ContinuesHaveWay".Localized());

                OutputText("Scenes.Aiko.PcLosesDomFight.WinceEarPull".Localized());

                OutputText("Scenes.Aiko.PcLosesDomFight.PuttyInHands".Localized());

                OutputText("Scenes.Aiko.PcLosesDomFight.FoxyDominatrixHands".Localized());

                OutputText("Scenes.Aiko.PcLosesDomFight.GaspViolently".Localized());

                Player.CuntChange(5, true, true, false);

                OutputText("Scenes.Aiko.PcLosesDomFight.DidYouThinkStrangle".Localized());

                OutputText("Scenes.Aiko.PcLosesDomFight.MindScreams".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Aiko.PcLosesDomFight.VisionDarkens".Localized(
                         x + 1
                    ));

                if (Player.HasVagina())
                    OutputText("Scenes.Aiko.PcLosesDomFight.TailsDoubleEffort".Localized());

                OutputText("Scenes.Aiko.PcLosesDomFight.HandsReleaseGrip".Localized());

                OutputText("Scenes.Aiko.PcLosesDomFight.ReleasedTorment".Localized());

                if (Player.HasCock())
                    Player.Orgasm("Dick");

                if (Player.HasVagina())
                    Player.Orgasm("vagina");
            }
            else
                OutputText("Scenes.Aiko.PcLosesDomFight.FallInHeap".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void AikoRapeSex()
        {

            ClearButtons();

            AddButton(0, "Scenes.Aiko.AikoRapeSex.ButtonFuckVag".Localized(), AikoRapeFuckVag);

            AddButton(1, "Common.Button.FuckAss".Localized(), AikoRapeFuckAss);

            AddButton(2, "Scenes.Aiko.AikoRapeSex.ButtonHumiliate".Localized(), AikoRapeHumiliate);

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoBallReturned) == 1)
            {
                AddButton(3, "Scenes.Aiko.AikoRapeSex.ButtonFistHer".Localized(), AikoFistHer);

                AddButton(4, "Common.Button.GetLicked".Localized(), AikoRapeGetLicked);
            }
            else
                AddButton(14, "Common.Button.Leave".Localized(), AikoRequestsBallAfterLose);

            if (!(CoC.Instance.Flags.Get(GameFlagIds.AikoBallReturned) != 1))
                AddButton(14, "Common.Button.Leave".Localized(), Leave);
        }

        public void PostRapeUpdate()
        {

            if (aikoCorruption >= 50)
                CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 5);
            else
                CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) - 5);

            CoC.Instance.Flags.Set(GameFlagIds.AikoRape, CoC.Instance.Flags.Get(GameFlagIds.AikoRape) + 1);

            if (Player.Corruption < 50)
                Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            if (Player.Corruption > aikoCorruption || Player.HasPerk(PerkIds.Sadist))
                aikoCorruption += 12;
            else
                aikoCorruption += 8;
        }

        public void PostSexUpdate()
        {

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) + 4);

            if (Player.Corruption >= 50)
                aikoCorruption += 4;

            if (aikoCorruption >= 50 && Player.Corruption < aikoCorruption)
            {
                Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

                aikoCorruption -= 2;
            }
        }

        public void AikoRapeFuckVag()
        {
            ClearOutput();

            var x= Random.Shared.Next(Player.Cocks.Count);

            OutputText("Scenes.Aiko.AikoRapeFuckVag.TemptationTakeAdvantage".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoTimesMet) == 1)
                OutputText("Scenes.Aiko.AikoRapeFuckVag.PleadsJustWantedBall".Localized());
            else
                OutputText("Scenes.Aiko.AikoRapeFuckVag.PleadsDidntWantFight".Localized());

            OutputText("Scenes.Aiko.AikoRapeFuckVag.CornersOfEyes".Localized(
                 PlayerJerk() ? "You smack her around a bit more, telling her to shut up" + (CoC.Instance.Flags.Get(GameFlagIds.AikoTimesMet) == 1 ? " about her stupid ball" : "") + ". You could have sworn you told this bitch to keep her damn trap shut! " : "You do your best to shrug off the pangs of guilt, knowing that you're in it too deep to turn back now."
            ));

            OutputText("Scenes.Aiko.AikoRapeFuckVag.TipAgainstQuivering".Localized(
                 x + 1,

                 Player.HasKnot(x) ? "When you feel your knot pushing against her lips, you shift your weight to lean over her and begin to pound her pussy with your [cock " + (x + 1) + "]." : "When you feel your hips rocking against hers, you shift your weight to lean over her and begin to pound her pussy with your [cock " + (x + 1) + "]."
            ));

            if (PlayerJerk())
                OutputText("Scenes.Aiko.AikoRapeFuckVag.GraspHairYank".Localized());

            OutputText("Scenes.Aiko.AikoRapeFuckVag.OneThingInHerFavor".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Aiko.AikoRapeFuckVag.NearingEndKnot".Localized());

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Aiko.AikoRapeFuckVag.LoudMoanClimax".Localized());
            else
                if (Player.CumQ() <= 350)
                    OutputText("Scenes.Aiko.AikoRapeFuckVag.LoudMoanDistend".Localized());
                else
                    OutputText("Scenes.Aiko.AikoRapeFuckVag.LoudMoanHits".Localized());

            OutputText("Scenes.Aiko.AikoRapeFuckVag.GetUpBrushOff".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoBallReturned) == 0)
            {
                OutputText("Scenes.Aiko.AikoRapeFuckVag.GotWhatYouWanted".Localized());

                OutputText("Scenes.Aiko.AikoRapeFuckVag.NoUseForIt".Localized());
            }

            OutputText("Scenes.Aiko.AikoRapeFuckVag.LeaveToUntie".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoBallReturned, 1);

            PostRapeUpdate();

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void AikoRapeFuckAss()
        {
            ClearOutput();

            var x = Player.CockThatFits(50);

            OutputText("Scenes.Aiko.AikoRapeFuckAss.EyesTrailDown".Localized());

            OutputText("Scenes.Aiko.AikoRapeFuckAss.WhaWaitNo".Localized());

            OutputText("Scenes.Aiko.AikoRapeFuckAss.ShrinksRaisesPleas".Localized());

            OutputText("Scenes.Aiko.AikoRapeFuckAss.SpiralTattoos".Localized(
                 x + 1
            ));

            OutputText("Scenes.Aiko.AikoRapeFuckAss.PleaseBeGentle".Localized(
                 x + 1
            ));

            if (Player.CockArea(x) >= 40)
                OutputText("Scenes.Aiko.AikoRapeFuckAss.AHNoMore".Localized());

            OutputText("Scenes.Aiko.AikoRapeFuckAss.OneLastShove".Localized());

            OutputText("Scenes.Aiko.AikoRapeFuckAss.SquealsPainPleasure".Localized(
                 x + 1
            ));

            OutputText("Scenes.Aiko.AikoRapeFuckAss.LoseYourself".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Aiko.AikoRapeFuckAss.HeldTogether".Localized());

            OutputText("Scenes.Aiko.AikoRapeFuckAss.PullOutSlurp".Localized(
                 CoC.Instance.Flags.Get(GameFlagIds.AikoBallReturned) == 0 ? ", and " + (PlayerJerk() ? "pull out her ball, smirking evilly as a despicable idea comes into your mind. You place the ball up against Aiko's brutalized anus and push it in, eliciting another shameful groan from her throat. Well, she said she wanted it back, and now she has it, you think to yourself with a terrible grin as you set off for camp. " : "gently drop her ball on the ground next to her, setting off for camp.") : " and set off back to camp."
            ));

            CoC.Instance.Flags.Set(GameFlagIds.AikoBallReturned, 1);

            PostRapeUpdate();

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void AikoRapeHumiliate()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoRapeHumiliate.StandOverEyesStray".Localized());

            OutputText("Scenes.Aiko.AikoRapeHumiliate.WWhatAreYouGoingToDo".Localized());

            OutputText("Scenes.Aiko.AikoRapeHumiliate.CutsOffFingers".Localized());

            OutputText("Scenes.Aiko.AikoRapeHumiliate.GraduallyDisrobe".Localized());

            OutputText("Scenes.Aiko.AikoRapeHumiliate.TeasingHand".Localized());

            OutputText("Scenes.Aiko.AikoRapeHumiliate.SilencingDefiance".Localized());

            OutputText("Scenes.Aiko.AikoRapeHumiliate.PleaseBeGentleWithMe".Localized());

            OutputText("Scenes.Aiko.AikoRapeHumiliate.RequestDulyNoted".Localized());

            OutputText("Scenes.Aiko.AikoRapeHumiliate.BodyStopsTwitching".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Aiko.AikoRapeHumiliate.PullHeadBack".Localized());

                OutputText("Scenes.Aiko.AikoRapeHumiliate.ShamesPriestessTongue".Localized());

                Player.Orgasm("Vagina");
            }

            if (!Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Aiko.AikoRapeHumiliate.ShiverDismount".Localized());
            else
                OutputText("Scenes.Aiko.AikoRapeHumiliate.ShiverLightly".Localized());

            if (Player.HasCock())
            {
                var x= Random.Shared.Next(Player.Cocks.Count);

                if (!Player.HasVagina())
                    OutputText("Scenes.Aiko.AikoRapeHumiliate.DragOntoBack".Localized());
                else
                    OutputText("Scenes.Aiko.AikoRapeHumiliate.RaiseHipsAngle".Localized(
                         x + 1
                    ));

                if (Player.CockArea(x) <= 50)
                    OutputText("Scenes.Aiko.AikoRapeHumiliate.RideHerThroat".Localized(
                         x + 1,

                         Player.CumQ() <= 150 ? "you fire a few sticky streamers down the convulsing passage, making her cough and sputter." : (Player.CumQ() > 150 && Player.CumQ() <= 350 ? "you unleash a thick wave of jism into her throat, making her choke and sputter as it begins to back up and foam around the corners of her mouth" : "you flood her gullet with your thick, virile spunk, forcing so much down her throat that her belly begins to distend from the sheer volume of it.")
                    ));
                else
                    OutputText("Scenes.Aiko.AikoRapeHumiliate.BearDownForcefully".Localized(
                         Player.CumQ() <= 150 ? " finally releasing your load onto the fox-girl's belly, smearing it with your scent" : (Player.CumQ() <= 350 ? "pumping out a thick load onto her stomach that spills down her sides and soaks the ground" : "shooting stream after stream of cum all across the fox-girl's lower body, painting her with a thick and thorough layer of jism")
                    ));

                Player.Orgasm("Dick");
            }

            OutputText("Scenes.Aiko.AikoRapeHumiliate.StandBackHandiwork".Localized(
                 CoC.Instance.Flags.Get(GameFlagIds.AikoBallReturned) == 0 ? "You decide that you should reward her obedience, pulling the ball from your [inv] and smearing it in some of your leftover essences before " + (PlayerJerk() ? "shoving it" : "placing it gently") + " in her mouth, then petting and praising her as if she was a dog. You spot a few of her tails wag limply in response to your degrading treatment, her spirit broken, at least for now." : "You pet and praise her as if she was a dog, and spot a few of her tails wag limply in response to your degrading treatment. Her spirit is broken, at least for now."
            ));

            CoC.Instance.Flags.Set(GameFlagIds.AikoBallReturned, 1);

            PostRapeUpdate();

            CombatScene.CleanupAfterCombat();
        }

        public void AikoFistHer()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoFistHer.CrackKnuckles".Localized());

            OutputText("Scenes.Aiko.AikoFistHer.SlideTouchPullAway".Localized());

            OutputText("Scenes.Aiko.AikoFistHer.ReachUpGrope".Localized());

            OutputText("Scenes.Aiko.AikoFistHer.CantHaveThat".Localized());

            PostRapeUpdate();

            CombatScene.CleanupAfterCombat();
        }

        public void AikoRapeGetLicked()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoRapeGetLicked.GrinStrip".Localized(
                 Player.HasCock() ? ", lifting your [cocks] " + (Player.Balls > 0 ? "and [balls]" : "") + " out of the way" : ""
            ));

            if (Player.HasCock())
            {
                var x= Random.Shared.Next(Player.Cocks.Count);

                OutputText("Scenes.Aiko.AikoRapeGetLicked.HoldArmLift".Localized(
                     x + 1
                ));

                OutputText("Scenes.Aiko.AikoRapeGetLicked.PullBackHair".Localized());

                Player.Orgasm("Dick");
            }

            OutputText("Scenes.Aiko.AikoRapeGetLicked.GlaresTonguing".Localized(
                 Player.HasCock() ? (Player.Cocks.Count > 1 ? "between" : "around") + " your [cocks]" : ""
            ));

            PostRapeUpdate();

            Player.Orgasm("Vagina");

            CombatScene.CleanupAfterCombat();
        }

        public void AikoSexMissionary()
        {
            ClearOutput();

            var x= Random.Shared.Next(Player.Cocks.Count);

            OutputText("Scenes.Aiko.AikoSexMissionary.YouTellAikoThat".Localized());

            OutputText("Scenes.Aiko.AikoSexMissionary.FullyNudeDisrobe".Localized());

            OutputText("Scenes.Aiko.AikoSexMissionary.BeautifulFormNude".Localized());

            OutputText("Scenes.Aiko.AikoSexMissionary.LegsSpreadGlistening".Localized(
                 x + 1
            ));

            OutputText("Scenes.Aiko.AikoSexMissionary.FirmerHoldShaft".Localized(
                 x + 1
            ));

            OutputText("Scenes.Aiko.AikoSexMissionary.SoftMoanMoveHips".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Aiko.AikoSexMissionary.HandsDownSides".Localized());

            OutputText("Scenes.Aiko.AikoSexMissionary.VigorousPace".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 65)
            {
                OutputText("Scenes.Aiko.AikoSexMissionary.WallsFinalContraction".Localized(
                     x + 1
                ));

                OutputText("Scenes.Aiko.AikoSexMissionary.MoaningHeavens".Localized());
            }

            if (Player.HasKnot(x))
                OutputText("Scenes.Aiko.AikoSexMissionary.RamKnotHome".Localized());

            OutputText("Scenes.Aiko.AikoSexMissionary.ShaftSwells".Localized());

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Aiko.AikoSexMissionary.LoudMoan".Localized());
            else
                if (Player.CumQ() <= 350)
                    OutputText("Scenes.Aiko.AikoSexMissionary.IncredibleLoad".Localized());
                else
                    OutputText("Scenes.Aiko.AikoSexMissionary.LoadAfterLoad".Localized());

            OutputText("Scenes.Aiko.AikoSexMissionary.MusclesNotGivenUp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 65)
            {
                OutputText("Scenes.Aiko.AikoSexMissionary.RollOffCollapse".Localized());

                OutputText("Scenes.Aiko.AikoSexMissionary.OkayLounging".Localized());

                OutputText("Scenes.Aiko.AikoSexMissionary.CertainlyWont".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
                {
                    OutputText("Scenes.Aiko.AikoSexMissionary.PushBackSatisfied".Localized());

                    OutputText("Scenes.Aiko.AikoSexMissionary.HateToLeave".Localized());

                    OutputText("Scenes.Aiko.AikoSexMissionary.SeeYaLater".Localized());
                }
                else
                {
                    OutputText("Scenes.Aiko.AikoSexMissionary.PullBackAikoSighs".Localized());

                    OutputText("Scenes.Aiko.AikoSexMissionary.SmallWave".Localized());
                }

            CoC.Instance.Flags.Set(GameFlagIds.AikoSexed, CoC.Instance.Flags.Get(GameFlagIds.AikoSexed) + 1);

            PostSexUpdate();

            Player.Orgasm("Dick");

            if (Player.HasStatusEffect(StatusEffectIds.Spar))
            {
                Player.RemoveStatusEffect(StatusEffectIds.Spar);

                CombatScene.CleanupAfterCombat();
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void AikoSexDoggy()
        {
            ClearOutput();

            var x = Player.CockThatFits(50, "area");

            var y= Player.CockThatFits2(50);

            OutputText("Scenes.Aiko.AikoSexDoggy.YouTellAikoThat".Localized());

            OutputText("Scenes.Aiko.AikoSexDoggy.StripEagerly".Localized(
                 x + 1
            ));

            OutputText("Scenes.Aiko.AikoSexDoggy.GrinLowerDown".Localized(
                 x + 1
            ));

            OutputText("Scenes.Aiko.AikoSexDoggy.WonderfulWarmth".Localized());

            OutputText("Scenes.Aiko.AikoSexDoggy.YellsPants".Localized());

            OutputText("Scenes.Aiko.AikoSexDoggy.GoGoYes".Localized());

            OutputText("Scenes.Aiko.AikoSexDoggy.WorryAttractPredator".Localized());

            if (y >= 0)
                OutputText("Scenes.Aiko.AikoSexDoggy.PassionatelyFuck".Localized(
                     y + 1,

                     y + 1
                ));

            OutputText("Scenes.Aiko.AikoSexDoggy.ImpaledBucksBack".Localized());

            if (Player.HasKnot(x) && Player.HasKnot(y))
                OutputText("Scenes.Aiko.AikoSexDoggy.SlamKnots".Localized());
            else
                if (Player.HasKnot(x))
                    OutputText("Scenes.Aiko.AikoSexDoggy.SlamKnot".Localized());
                else
                    if (Player.HasKnot(y))
                        OutputText("Scenes.Aiko.AikoSexDoggy.SlamKnotAss".Localized());

            OutputText("Scenes.Aiko.AikoSexDoggy.CumsHardEffect".Localized());

            if (Player.CumQ() > 350)
                OutputText("Scenes.Aiko.AikoSexDoggy.HoleGurgles".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50)
            {
                OutputText("Scenes.Aiko.AikoSexDoggy.ThatWasGreat".Localized());

                if (Player.HasKnot(x) || Player.HasKnot(y))
                {

                }
                else
                    OutputText("Scenes.Aiko.AikoSexDoggy.OffCockLaySide".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 65)
                    OutputText("Scenes.Aiko.AikoSexDoggy.TailsWrapAround".Localized());
                else
                    OutputText("Scenes.Aiko.AikoSexDoggy.TellShouldHeadBack".Localized());
            }
            else
            {
                OutputText("Scenes.Aiko.AikoSexDoggy.PullsAwayDrags".Localized());

                OutputText("Scenes.Aiko.AikoSexDoggy.AdmitsNotHalfBad".Localized());

                OutputText("Scenes.Aiko.AikoSexDoggy.TellHeadingBack".Localized());
            }

            OutputText("Scenes.Aiko.AikoSexDoggy.GatherThingsCamp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoSexed, CoC.Instance.Flags.Get(GameFlagIds.AikoSexed) + 1);

            PostSexUpdate();

            Player.Orgasm("Dick");

            if (y >= 0)
                Player.Orgasm("Dick");

            if (Player.HasStatusEffect(StatusEffectIds.Spar))
            {
                Player.RemoveStatusEffect(StatusEffectIds.Spar);

                CombatScene.CleanupAfterCombat();
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void AikoSexBJ()
        {
            ClearOutput();

            var x= Random.Shared.Next(Player.Cocks.Count);

            OutputText("Scenes.Aiko.AikoSexBJ.SmirksBobUp".Localized(
                 x + 1,

                 Player.Balls > 0 ? "Her other hand gently lifts your [balls], squeezing them softly as she runs her tongue up the underside of your [cock " + (x + 1) + "]." : ""
            ));

            if (Player.CockArea(x) < 40)
            {
                OutputText("Scenes.Aiko.AikoSexBJ.LipsSmackKisses".Localized(
                     x + 1
                ));

                OutputText("Scenes.Aiko.AikoSexBJ.KeepHandBase".Localized(
                     x + 1,

                     x + 1
                ));

                OutputText("Scenes.Aiko.AikoSexBJ.MoansSoftly".Localized());

                OutputText("Scenes.Aiko.AikoSexBJ.HoverHandsHesitate".Localized(
                     x + 1
                ));

                OutputText("Scenes.Aiko.AikoSexBJ.MakingGoodShow".Localized());

                OutputText("Scenes.Aiko.AikoSexBJ.ObsceneMoan".Localized(
                     x + 1,

                     Player.CumQ() > 150 ? "She puts up a good effort, but ultimately fails as jets of cum spurt from the corners of her mouth" + (Player.CumQ() > 350 ? ". As more and more of your semen cascades down her throat, her stomach begins to bulge a little, though far more of it gets on her than in her as it oozes messily down her chin" : "") : ""
                ));

                OutputText("Scenes.Aiko.AikoSexBJ.OnceOrgasmFinished".Localized(
                     x + 1
                ));
            }
            else
            {
                OutputText("Scenes.Aiko.AikoSexBJ.LipsSmackKissesAgain".Localized(
                     x + 1
                ));

                OutputText("Scenes.Aiko.AikoSexBJ.SqueezeBaseTightly".Localized(
                     x + 1
                ));

                OutputText("Scenes.Aiko.AikoSexBJ.GigglesAtLook".Localized());

                OutputText("Scenes.Aiko.AikoSexBJ.HandsRunUpDown".Localized(
                     x + 1,

                     x + 1
                ));

                OutputText("Scenes.Aiko.AikoSexBJ.SayFragment".Localized(
                     Player.Cocks.Count > 1 ? "The rest of your [cocks] spilling their spunk all over Aiko's chest and the ground below you" + (Player.HasVagina() ? ", and your" : "") : (Player.HasVagina() ? "Your" : "")
                ));

                OutputText("Scenes.Aiko.AikoSexBJ.SayFragmentAgain".Localized());

                OutputText("Scenes.Aiko.AikoSexBJ.CleanedOffBandages".Localized());
            }

            OutputText("Scenes.Aiko.AikoSexBJ.PushesToFeet".Localized());

            OutputText("Scenes.Aiko.AikoSexBJ.ThatWasFun".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoSexed, CoC.Instance.Flags.Get(GameFlagIds.AikoSexed) + 1);

            PostSexUpdate();

            Player.Orgasm("Dick");

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoTimesMet)  == 1)
                DoNext(() => AikoE1SexPart2());
            else
                OutputText("Scenes.Aiko.AikoSexBJ.RecoveredStupor".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Spar))
            {
                Player.RemoveStatusEffect(StatusEffectIds.Spar);

                CombatScene.CleanupAfterCombat();
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void AikoSexCunni()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoSexCunni.YouTellAikoYou".Localized());

            OutputText("Scenes.Aiko.AikoSexCunni.SmirksVaginaView".Localized());

            OutputText("Scenes.Aiko.AikoSexCunni.StandsOverUndoing".Localized());

            OutputText("Scenes.Aiko.AikoSexCunni.ShiversTongue".Localized());

            OutputText("Scenes.Aiko.AikoSexCunni.SpreadLipsFingers".Localized());

            OutputText("Scenes.Aiko.AikoSexCunni.MoaningPullsTongue".Localized());

            OutputText("Scenes.Aiko.AikoSexCunni.MoanInUnison".Localized(
                 Player.Cocks.Count > 0 ? ", your neglected [cocks] spilling " + (Player.Cocks.Count == 1 ? "its" : "their") + " spunk all over Aiko's chest and the ground below you" : ""
            ));

            OutputText("Scenes.Aiko.AikoSexCunni.PushesToFeet".Localized());

            OutputText("Scenes.Aiko.AikoSexCunni.ThatWasFun".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoSexed, CoC.Instance.Flags.Get(GameFlagIds.AikoSexed) + 1);

            PostSexUpdate();

            Player.Orgasm("Vagina");

            if (CoC.Instance.Flags.Get(GameFlagIds.AikoTimesMet)  == 1)
                DoNext(() => AikoE1SexPart2());

            if (Player.HasStatusEffect(StatusEffectIds.Spar))
            {
                Player.RemoveStatusEffect(StatusEffectIds.Spar);

                CombatScene.CleanupAfterCombat();
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void AikoSexTailjob()
        {
            ClearOutput();

            var x= Random.Shared.Next(Player.Cocks.Count);

            if (playerchoice)
            {
                OutputText("Scenes.Aiko.AikoSexTailjob.AskUseTails".Localized(
                     x + 1
                ));

                OutputText("Scenes.Aiko.AikoSexTailjob.OnlyWithSomeone".Localized());
            }
            else
                OutputText("Scenes.Aiko.AikoSexTailjob.ReallyLikeYou".Localized(
                     x + 1
                ));

            OutputText("Scenes.Aiko.AikoSexTailjob.CoyRelaxEnjoy".Localized());

            OutputText("Scenes.Aiko.AikoSexTailjob.ClimbsOntoLegs".Localized(
                 x + 1
            ));

            OutputText("Scenes.Aiko.AikoSexTailjob.GigglesPlayfully".Localized());

            OutputText("Scenes.Aiko.AikoSexTailjob.MoanMasterful".Localized());

            OutputText("Scenes.Aiko.AikoSexTailjob.ContinuesTorment".Localized(
                 x + 1
            ));

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Aiko.AikoSexTailjob.CocksEnveloped".Localized());
            else
                OutputText("Scenes.Aiko.AikoSexTailjob.Your".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Aiko.AikoSexTailjob.VaginaTickled".Localized());

            OutputText("Scenes.Aiko.AikoSexTailjob.BlushDeepMoan".Localized());

            OutputText("Scenes.Aiko.AikoSexTailjob.GigglesCoveredSperm".Localized());

            OutputText("Scenes.Aiko.AikoSexTailjob.FewPeopleEnjoy".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoSexed, CoC.Instance.Flags.Get(GameFlagIds.AikoSexed) + 1);

            PostSexUpdate();

            Player.Orgasm("Dick");

            if (Player.HasStatusEffect(StatusEffectIds.Spar))
            {
                Player.RemoveStatusEffect(StatusEffectIds.Spar);

                CombatScene.CleanupAfterCombat();
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void SubmitToAiko()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.SubmitToAiko.ItchingFor".Localized());

            switch ((int)Player.Gender)
            {
                case 1:
                {
                DoNext(() => AikoFootjob());
                }

                break;

                case 2:
                {
                DoNext(() => AikoWhipping());
                }

                break;

                case 3:
                {
                (Random.Shared.Next(2) == 0 ? (() => AikoFootjob()) : (Action)(() => AikoWhipping()))();
                }

                break;
            }

            var __sw1_0 = (int)Player.Gender;

            if (true )
            {
                OutputText("Scenes.Aiko.SubmitToAiko.WaitHowSupposed".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void AikoFootjob()
        {
            ClearOutput();

            var x= Random.Shared.Next(Player.Cocks.Count);

            OutputText("Scenes.Aiko.AikoFootjob.FallBackShoves".Localized());

            OutputText("Scenes.Aiko.AikoFootjob.ComfortableDripping".Localized());

            OutputText("Scenes.Aiko.AikoFootjob.StandsOverRaisesFoot".Localized());

            OutputText("Scenes.Aiko.AikoFootjob.PatheticLoser".Localized(
                 x + 1
            ));

            OutputText("Scenes.Aiko.AikoFootjob.IncreaseSpeed".Localized());

            OutputText("Scenes.Aiko.AikoFootjob.FuzzyBondsConstrict".Localized());

            OutputText("Scenes.Aiko.AikoFootjob.WonderfulPain".Localized());

            OutputText("Scenes.Aiko.AikoFootjob.BodyMovesOwnWill".Localized());

            if (Random.Shared.Next(4) == 0)
            {
                OutputText("Scenes.Aiko.AikoFootjob.NextTimeNotSelfish".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 50f));
            }
            else
            {
                OutputText("Scenes.Aiko.AikoFootjob.GoodLittleMasochist".Localized(
                     x + 1,

                     x + 1,

                     x + 1
                ));

                Player.Orgasm("Dick");

                PostSexUpdate();
            }

            CoC.Instance.Flags.Set(GameFlagIds.AikoSexed, CoC.Instance.Flags.Get(GameFlagIds.AikoSexed) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void AikoWhipping()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoWhipping.FallBackShoves".Localized());

            OutputText("Scenes.Aiko.AikoWhipping.ComfortableDripping".Localized());

            OutputText("Scenes.Aiko.AikoWhipping.RaisesHandClaws".Localized());

            OutputText("Scenes.Aiko.AikoWhipping.HandRisesRecoil".Localized());

            OutputText("Scenes.Aiko.AikoWhipping.IsntItWonderful".Localized());

            OutputText("Scenes.Aiko.AikoWhipping.TerrifiedNod".Localized());

            OutputText("Scenes.Aiko.AikoWhipping.MaliciousGrinTails".Localized());

            OutputText("Scenes.Aiko.AikoWhipping.HAhahaLikeIt".Localized());

            OutputText("Scenes.Aiko.AikoWhipping.SquealTwitchConvulse".Localized());

            OutputText("Scenes.Aiko.AikoWhipping.ShrillMoan".Localized());

            OutputText("Scenes.Aiko.AikoWhipping.WantToCum".Localized());

            OutputText("Scenes.Aiko.AikoWhipping.ShoutToHeavens".Localized());

            if (Random.Shared.Next(4) == 0)
            {
                OutputText("Scenes.Aiko.AikoWhipping.MoansDieOut".Localized());

                OutputText("Scenes.Aiko.AikoWhipping.ComeToHourLater".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

                Player.ApplyStats(new DynStat(GameStats.Toughness, 2f), new DynStat(GameStats.Sensitivity, -12f));

                Player.ChangeFatigue(30);
            }
            else
            {
                OutputText("Scenes.Aiko.AikoWhipping.GrabsThroat".Localized());

                Player.Orgasm("Vagina");

                Player.ApplyStats(new DynStat(GameStats.Toughness, 2f), new DynStat(GameStats.Sensitivity, -2f));

                Player.ChangeFatigue(30);

                PostSexUpdate();
            }

            CoC.Instance.Flags.Set(GameFlagIds.AikoSexed, CoC.Instance.Flags.Get(GameFlagIds.AikoSexed) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void YamataTalk()
        {
            ClearOutput();

            if (aikoCorruption < 50)
            {
                OutputText("Scenes.Aiko.YamataTalk.AskElseAboutHer".Localized());

                OutputText("Scenes.Aiko.YamataTalk.SniffsLooksUp".Localized());
            }
            else
                OutputText("Scenes.Aiko.YamataTalk.SighsShakesHead".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.GoNow".Localized(), YamataStart);

            AddButton(1, "Common.Button.Prepare".Localized(), YamataPrepare);
        }

        public void YamataPrepare()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.YamataPrepare.NeedTimePrepare".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void YamataStart()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.YamataStart.ReadyToGo".Localized());

            if (aikoCorruption < 50)
                OutputText("Scenes.Aiko.YamataStart.OkayFollowMe".Localized());
            else
                OutputText("Scenes.Aiko.YamataStart.TookYouLongEnough".Localized());

            DoNext(() => YamataIntro());
        }

        public void YamataIntro()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.YamataIntro.StreetsDeserted".Localized());

            OutputText("Scenes.Aiko.YamataIntro.FaceToFace".Localized());

            OutputText("Scenes.Aiko.YamataIntro.AikoCallsYamata".Localized());

            OutputText("Scenes.Aiko.YamataIntro.SayFragment".Localized());

            OutputText("Scenes.Aiko.YamataIntro.TurnAttention".Localized());

            OutputText("Scenes.Aiko.YamataIntro.MostStrikingFeature".Localized());

            OutputText("Scenes.Aiko.YamataIntro.LookingForThis".Localized());

            OutputText("Scenes.Aiko.YamataIntro.TakenAbackTitle".Localized());

            OutputText("Scenes.Aiko.YamataIntro.MasterKnowsAll".Localized());

            OutputText("Scenes.Aiko.YamataIntro.BitchReallyCrazy".Localized());

            OutputText("Scenes.Aiko.YamataIntro.PreyHasFight".Localized());

            OutputText("Scenes.Aiko.YamataIntro.SayFragmentAgain".Localized());

            StartYamataFight();
        }

        public void YamataWins()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.YamataWins.GritTeethKnees".Localized());

            OutputText("Scenes.Aiko.YamataWins.AikoShoutsRuns".Localized());

            OutputText("Scenes.Aiko.YamataWins.MutterLifeLeaves".Localized());
            // 原版 AikoScene.hx:1889：该分支的触发条件
            if (SceneInstance<Npcs.MarbleScene>.Get().MarbleFollower())
                OutputText("Scenes.Aiko.YamataWins.MarbleCries".Localized());

            CombatController.InCombat = false;

            GameOver();
        }

        public void YamataLoses()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.YamataLoses.RelaxShouldersFalling".Localized());

            OutputText("Scenes.Aiko.YamataLoses.ImpossibleStrength".Localized());

            OutputText("Scenes.Aiko.YamataLoses.ColdlyGiveItUp".Localized());

            OutputText("Scenes.Aiko.YamataLoses.FoolNineTails".Localized());

            OutputText("Scenes.Aiko.YamataLoses.AikoVoiceCuts".Localized());

            OutputText("Scenes.Aiko.YamataLoses.LowlyBeingsTrick".Localized());

            OutputText("Scenes.Aiko.YamataLoses.DontWorryAttack".Localized());

            OutputText("Scenes.Aiko.YamataLoses.InsolentBratPay".Localized());

            OutputText("Scenes.Aiko.YamataLoses.StandOverDefeated".Localized());

            OutputText("Scenes.Aiko.YamataLoses.DamnYouElders".Localized());

            OutputText("Scenes.Aiko.YamataLoses.DontListenKnowIt".Localized());

            if (!Player.HasPerk(PerkIds.Masochist))
                if (CoC.Instance.Flags.Get(GameFlagIds.YamataMasochist) >= 100)
                    OutputText("Scenes.Aiko.YamataLoses.RealizeFromTorture".Localized());
                else
                    OutputText("Scenes.Aiko.YamataLoses.AllThatPainMasochist".Localized());

            OutputText("Scenes.Aiko.YamataLoses.LookAtDefeated".Localized());

            if (!Player.HasPerk(PerkIds.Masochist) && CoC.Instance.Flags.Get(GameFlagIds.YamataMasochist) >= 100)
                Player.CreatePerk(PerkIds.Masochist, 0, 0, 0, 0);

            ClearButtons();

            AddButton(0, "Scenes.Aiko.YamataLoses.ButtonLetHerGo".Localized(), ReleaseYamata);

            AddButton(1, "Scenes.Aiko.YamataLoses.ButtonAikoFinish".Localized(), AikoKillSister);

            AddButton(2, "Scenes.Aiko.YamataLoses.ButtonFinishHer".Localized(), KillYamata);
        }

        public void FuckYamata()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.FuckYamata.PlaceholderToFuck".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Aiko.FuckYamata.ButtonLetHerGo".Localized(), ReleaseYamata);

            AddButton(1, "Scenes.Aiko.FuckYamata.ButtonAikoFinish".Localized(), AikoKillSister);

            AddButton(2, "Scenes.Aiko.FuckYamata.ButtonFinishHer".Localized(), KillYamata);
        }

        public void ReleaseYamata()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.ReleaseYamata.WhatLettingHerLeave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoAffection, CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) - 20);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            CoC.Instance.Flags.Set(GameFlagIds.AikoBossComplete, 3);

            DoNext(() => YamataAftermath());
        }

        public void AikoKillSister()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.AikoKillSister.LookAtAikoNod".Localized());

            OutputText("Scenes.Aiko.AikoKillSister.SevenTailsTrembling".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoBossComplete, 2);

            DoNext(() => YamataAftermath());
        }

        public void KillYamata()
        {
            ClearOutput();

            if (Player.Weapon.IsFirearm())
                OutputText("Scenes.Aiko.KillYamata.NarrowEyesRaise".Localized());
            else
                if (Player.Weapon.IsChanneling(Player))
                    OutputText("Scenes.Aiko.KillYamata.RaiseMagicBlast".Localized());
                else
                    if (Player.Weapon.IsStabby())
                        OutputText("Scenes.Aiko.KillYamata.NarrowEyesDash".Localized());
                    else
                        if (Player.Weapon.IsBlunt())
                            OutputText("Scenes.Aiko.KillYamata.SwingCrushRibs".Localized());
                        else
                            OutputText("Scenes.Aiko.KillYamata.NarrowEyesRaiseAgain".Localized());

            OutputText("Scenes.Aiko.KillYamata.CoughsBloodInsult".Localized());

            OutputText("Scenes.Aiko.KillYamata.BeforeActSevenTails".Localized());

            OutputText("Scenes.Aiko.KillYamata.NeedToGetStronger".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AikoBossComplete, 1);

            DoNext(() => YamataAftermath());
        }

        public void YamataAftermath()
        {
            ClearOutput();

            OutputText("Scenes.Aiko.YamataAftermath.LeaveToFree".Localized());

            OutputText("Scenes.Aiko.YamataAftermath.AikoArrivesSword".Localized());

            OutputText("Scenes.Aiko.YamataAftermath.BackInVillage".Localized());

            OutputText("Scenes.Aiko.YamataAftermath.ElderBowsThanks".Localized());

            OutputText("Scenes.Aiko.YamataAftermath.ReturnToCampExhausted".Localized());

            CombatScene.CleanupAfterCombat();
        }

    }
}