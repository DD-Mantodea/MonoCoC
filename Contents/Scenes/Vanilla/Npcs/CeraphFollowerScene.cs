using MonoCoC.Attributes;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<CeraphFollowerScene>("coc:ceraph_follower")]
    public class CeraphFollowerScene : Scene, ITimeAware
    {

        public bool TimeChange()
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.LiddelliumCeraphFlag) > 1)
                CoC.Instance.Flags.Set(GameFlagIds.LiddelliumCeraphFlag,

                    CoC.Instance.Flags.Get(GameFlagIds.LiddelliumCeraphFlag) - 1);

            return false;
        }

        public bool TimeChangeLarge() => false;

        public static SaveContent Save => SaveContent(SceneIds.Npcs.CeraphFollower);

        public string debugName => "Ceraph";

        public string debugHint => "";

        public override void Entry()
        {
            CeraphFollowerEncounter();
        }

        public void Reset()
        {

            Save.SetInt("rapePlay", 0);

            Save.SetInt("roleplayAsTellyCount", 0);
        }

        public bool CeraphIsFollower()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.CeraphBellybuttonPiercing) > 0 || CoC.Instance.Flags.Get(GameFlagIds.CeraphToken) > 0;
        }

        public string CeraphBus()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                return "omnibus";
            else
                return "succubus";
        }

        public void CeraphFollowerEncounter(bool forceCeraph = false)
        {

            if (forceCeraph)
            {
                CeraphFollowerAppearance();

                return ;
            }

            if (Random.Shared.Next(24) == 0 && Player.HasCock())
                CatgirlEncounter();
            else
                if (Random.Shared.Next(24) == 0 && CoC.Instance.Flags.Get(GameFlagIds.CeraphFauxCorruptionDisabled) == 0)
                    CarephCorruptionSlaves();
                else
                    if (Random.Shared.Next(24) <= 1 && (int)Player.Gender > 0)
                        EncounterZetsuko();
                    else
                        CeraphFollowerAppearance();
        }

        public void CeraphSprite(bool nude = false)
        {

        }

        public void CeraphFollowerAppearance(bool output = true)
        {
            ClearOutput();

            var seaShirt = Undergarments.SEA_SHIRT;

            if (output)
            {
                OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerAppearance.SummonCeraphFollower".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerAppearance.CeraphBoulderArrival".Localized(
                    Player.Mf("Master", "Mistress")
                ));
            }

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), CeraphAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), CeraphTalkMenu);

            AddButton(2, "Common.Button.Sex".Localized(), CeraphSexMenu);

            AddButton(3, "Common.Button.Roleplay".Localized(), FollowerCeraphRoleplay);

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                AddButton(5, "Common.Button.GoFemale".Localized(), CawkTawgle);
            else
                AddButton(5, "Common.Button.GoHerm".Localized(), CawkTawgle);

            AddButton(6, "Common.Button.Partswap".Localized(), GiveFollowerBodyBits);

            AddButton(7, "Common.Button.Fetishes".Localized(), CeraphFetishMenu);

            if (Player.UpperGarment == seaShirt && seaShirt.knownBound())
            {
                if (seaShirt.Save.GetInt("ceraphAsked", 0) == 0)
                    AddButton(8, "Common.Button.CursedShirt".Localized(), AskCeraphAboutNobleShirt);
                else
                    if (seaShirt.Save.GetInt("ceraphLeftIt", 0) != 0)
                        AddButton(8, "Common.Button.RemoveShirt".Localized(), CeraphRemoveNobleShirt);
            }
            else
                if (Player.HasItem(seaShirt) && !seaShirt.IsNormalShirt())
                    AddButton(8, "Common.Button.Disenchant".Localized(), CeraphDisenchantNobleShirt);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmCeraph) == 0 && CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 1)
                AddButton(10, "Common.Button.FarmWork".Localized(), HelpWithFarm);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Liddellium)) && (CoC.Instance.Flags.Get(GameFlagIds.LiddelliumFlag) >= 0))
                AddButton(11, "Items.Liddellium.Short".Localized(), IdLiddellium);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.LiddelliumFlag) < 0)
                    AddButton(11, "Common.Button.Liddellium".Localized(), GetLiddellium);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
        }

        public void CeraphAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphAppearance.CeraphWearsRedCollar".Localized((CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0 ? "Common.Text.HerDemonicPrickFormsARidge".Localized() : "Common.Text.ThankfullySheSeemsToHaveRemoved".Localized())));

            OutputText("Scenes.Ceraphfollowerscene.CeraphAppearance.CeraphBlackEyesTwist".Localized());

            DoNext(() => CeraphFollowerAppearance());
        }

        public void CeraphTalkMenu()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkMenu.ChooseTalkTopic".Localized());

            ClearButtons();

            AddNextButton("Codex.Category.Magic".Localized(), CeraphTalkMagic);

            AddNextButton("Common.Button.Slave".Localized(), CeraphTalkSlave);

            AddNextButton("Common.Button.Lethice".Localized(), CeraphTalkLethice);

            AddNextButton("Common.Button.Partswap".Localized(), CeraphTalkPartswap);

            AddButton(14, "Common.Button.Back".Localized(), () => CeraphFollowerAppearance());
        }

        public void CeraphTalkMagic()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkMagic.WantingToKnowMore".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkMagic.CeraphMagicOffer".Localized((SceneInstance<CeraphScene>.Get().HadFetish() ? "Is [Master] interested in receiving a fetish after all? " : "")));

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkMagic.UnfamiliarMagicPower".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkMagic.CeraphMagicRarity".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkMagic.ExplanationLacking".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkMagic.CeraphRegainsComposure".Localized());

            DoNext(() => CeraphTalkMenu());
        }

        public void CeraphTalkSlave()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkSlave.CeraphPersonaShift".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkSlave.CeraphBattingEyes".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkSlave.WantTrueAssurance".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkSlave.CeraphUncertainPause".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkSlave.CeraphNodsSatisfied".Localized());

            DoNext(() => CeraphTalkMenu());
        }

        public void CeraphTalkLethice()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkLethice.AskAboutLethice".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkLethice.CeraphLowVoiceRefusal".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkLethice.MasterInsistsAnswer".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkLethice.CeraphLethiceDisdain".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkLethice.CeraphUnsettlingGleam".Localized());

            DoNext(() => CeraphTalkMenu());
        }

        public void CeraphTalkPartswap()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkPartswap.AskMagicServices".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkPartswap.CeraphPartswapOffer".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTalkPartswap.PartswapOnRequest".Localized());

            DoNext(() => CeraphTalkMenu());
        }

        public void HelpWithFarm()
        {
            ClearOutput();

            CeraphSprite();

            OutputText("Scenes.Ceraphfollowerscene.HelpWithFarm.AskFarmHelp".Localized());

            OutputText("Scenes.Ceraphfollowerscene.HelpWithFarm.CeraphFarmMockery".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.DoIt".Localized(), DoFarmShit);

            AddButton(1, "Common.Button.Later".Localized(), NoFarmShitYet);
        }

        public void DoFarmShit()
        {
            ClearOutput();

            CeraphSprite();

            OutputText("Scenes.Ceraphfollowerscene.DoFarmShit.CeraphFarmExcitement".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmCeraph, 1);

            CeraphFollowerAppearance(false);
        }

        public void NoFarmShitYet()
        {
            ClearOutput();

            CeraphSprite();

            OutputText("Scenes.Ceraphfollowerscene.NoFarmShitYet.CeraphSmirkAgreement".Localized());

            CeraphFollowerAppearance(false);
        }

        public void CeraphSexMenu()
        {
            ClearOutput();

            ClearButtons();

            AddButton(0, "Common.Button.FuckPussy".Localized(), FuckFollowerCeraphsVagoo);

            AddButton(1, "Common.Button.GetTongued".Localized(), FollowerCeraphTongueFucking);

            AddButton(2, "Common.Button.PleaseAll".Localized(), CeraphTentacleGrape);

            AddButton(3, "Common.Button.NippleFuck".Localized(), StuffSomeNippleCunts);

            AddButton(4, "Common.Button.PenisMagic".Localized(), PortalFuckWithFollowerCeraph);

            AddButton(5, "Common.Button.LayEggs".Localized(), LayEggsInSlaveCeraph);

            AddNextButton("Common.Button.RapePlay".Localized(), CeraphRapePlay);

            AddNextButton("Common.Button.Trot".Localized(), GoOnATrotWithCeraph);

            AddButton(14, "Common.Button.Back".Localized(), () => CeraphFollowerAppearance(false));
        }

        public void FollowerCeraphRoleplay()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphRoleplay.ProposeRoleplay".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphRoleplay.CeraphMountainIllusion".Localized());

            Action? urta = null;

            Action? marbles = null;

            Action? dominika = null;

            Action? Telly = null;

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) > 0 && (Player.HasCock() || Player.HasVagina()) && Player.Lust >= 33)
                urta = CeraphUrtaRoleplay;

            if (Player.HasCockThatFits(70) && Player.HasStatusEffect(StatusEffectIds.Marble) && Player.Lust >= 33)
                marbles = SweetieNOOOO;

            if (CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) > 0 && Player.Lust >= 33 && Player.HasCock())
                dominika = Cerminika;
            // 原版 CeraphFollowerScene.hx:299 — CeraphFollower.hx：PC 淫欲低于 33
            if (Player.Lust < 33)
                Telly = TellyRolepay;

            if (Player.Lust < 33)
                OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphRoleplay.InsufficientArousal".Localized());

            ClearButtons();

            if ((int)Player.Gender > 0)
                AddButton(8, "Common.Button.BeAPet".Localized(), SumissivenessToCeraphFollower);

            AddButton(0, "Common.Button.DefeatHer".Localized(), SceneInstance<CeraphScene>.Get().WinRapeChoices);

            AddButton(1, "Common.Button.LoseToHer".Localized(), SceneInstance<CeraphScene>.Get().CeraphRapesYouBADDAWGYODIGGITY);

            AddButton(5, "Common.Button.Dominika".Localized(), dominika);

            AddButton(6, "Common.Button.MarblePlay".Localized(), marbles);

            AddButton(7, "Common.Button.UrtaPlay".Localized(), urta);

            AddButton(9, "Common.Button.Telly".Localized(), Telly);

            AddButton(14, "Common.Button.Back".Localized(), () => CeraphFollowerAppearance(false));
        }

        public void SubmissiveCeraphOffer()
        {
            ClearOutput();

            CeraphSprite(true);

            OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.OnceAgainCeraphPrefix".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.DropsBeatenFragment".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.MasturbatingFragment".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.SheWord".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.LooksUpAsksFragment".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.StopsFappingToAsk".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.CommaWhyBotherFragment".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.SurprisedByDefeat".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.NervousChuckle".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.SurprisedLaugh".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.BemusedSmirk".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.PeriodSpaceCeraphPresses".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.TemptedByOffer".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.SubmissiveCeraphOffer.FuckHerPrompt".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.FuckHer".Localized(), null);

            AddButton(2, "Common.Button.RideHer".Localized(), null);

            AddButton(3, "Common.Button.Fuckherass".Localized(), null);

            AddButton(0, "Common.Button.CollarHer".Localized(), CollarCeraph);

            if (Player.Lust >= 33)
            {
                if (Player.HasCock())
                {
                    AddButton(1, "Common.Button.FuckHer".Localized(), SceneInstance<CeraphScene>.Get().MaleFuckCeraphsPussy);

                    if (Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()) != -1)
                        AddButton(3, "Common.Button.Fuckherass".Localized(), SceneInstance<CeraphScene>.Get().ButtRapeCeraph);
                    else
                        AddButton(3, "Common.Button.Fuckherass".Localized(), null);
                }

                if (Player.HasVagina())
                {
                    AddButton(2, "Common.Button.RideHer".Localized(), SceneInstance<CeraphScene>.Get().RideCeraphsCockLikeaBAWSSexclamation11eleven);

                    if (Player.BiggestBreastSize() >= 4 && Player.Armor?.Id == ArmorIds.LightMaidenArmor)
                        AddButton(4, "Common.Button.Titfuck".Localized(), () => Contents.Items.Behaviors.Vanilla.LustyMaidensArmorBehavior.Paizuri(CombatController.Instance!.Context.Enemy));
                }
            }

            if (Player.Armor?.Id == ArmorIds.NaughtyNunsHabit)
                AddButton(5, "Scenes.Akbal.AkbalDefeated.ButtonWorshipCock".Localized(), () => Contents.Items.Behaviors.Vanilla.NaughtyNunsHabitBehavior.CockWorship());

            if (!CeraphIsFollower())
                AddButton(6, "Common.Button.Enough".Localized(), SceneInstance<CeraphScene>.Get().KillCeraph);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void CollarCeraph()
        {
            ClearOutput();

            CeraphSprite(true);

            OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.SnatchCollar".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.FastenCollarSlave".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.NeverExpectedSlaveClause".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.DemonSlaveAcceptance".Localized());
                else
                {
                    OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.EnjoyUsingFormerDom".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.CumDump".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.TongueSlave".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.SentencePeriod".Localized());
                }

            OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.CeraphKneelsKissesFeet".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.DefeatedDemonExplains".Localized(
                Player.Mf("Master", "Mistress"),

                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.HaremLairLogic".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.ResumeStroking".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.StrokeWord".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CollarCeraph.NoduleDemonDickFragment".Localized(
                Player.Mf("Master", "Mistress")
            ));

            ClearButtons();

            AddButton(0, "Common.Button.Carry".Localized(), CarryCarephsToken);

            AddButton(1, "Common.Button.Pierce".Localized(), GetCeraphFollowerPiercing);
        }

        public void CarryCarephsToken()
        {
            ClearOutput();

            CeraphSprite(true);

            OutputText("Scenes.Ceraphfollowerscene.CarryCarephsToken.SnatchOnyxToken".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CarryCarephsToken.CeraphFinalFuckAsk".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.CarryCarephsToken.OnyxTokenReceived".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CeraphToken, 1);

            Player.CreateKeyItem("Onyx Token - Ceraph's", 0, 0, 0, 0);

            ClearButtons();

            AddButton(0, "Common.Button.FuckHer".Localized(), null);

            AddButton(1, "Common.Button.RideHer".Localized(), null);

            AddButton(2, "Common.Button.Fuckherass".Localized(), null);

            if (Player.Lust >= 33)
            {
                OutputText("Scenes.Ceraphfollowerscene.CarryCarephsToken.HaveWayWithDemon".Localized());

                if (Player.HasCock())
                {
                    AddButton(0, "Common.Button.FuckHer".Localized(), SceneInstance<CeraphScene>.Get().MaleFuckCeraphsPussy);

                    if (Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()) != -1)
                        AddButton(2, "Common.Button.Fuckherass".Localized(), SceneInstance<CeraphScene>.Get().ButtRapeCeraph);
                    else
                        AddButton(2, "Common.Button.Fuckherass".Localized(), null);
                }

                if (Player.HasVagina())
                    AddButton(1, "Common.Button.RideHer".Localized(), SceneInstance<CeraphScene>.Get().RideCeraphsCockLikeaBAWSSexclamation11eleven);
            }

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void GetCeraphFollowerPiercing()
        {
            ClearOutput();

            CeraphSprite(true);

            CoC.Instance.Flags.Set(GameFlagIds.CeraphBellybuttonPiercing, 1);

            OutputText("Scenes.Ceraphfollowerscene.GetCeraphFollowerPiercing.BareMidriffToSlut".Localized());

            if (Player.Corruption < 40)
                OutputText("Scenes.Ceraphfollowerscene.GetCeraphFollowerPiercing.LittleHesitation".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.GetCeraphFollowerPiercing.SmirkSecureKnowledge".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GetCeraphFollowerPiercing.CeraphPressesStud".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GetCeraphFollowerPiercing.AwestruckNewPiercing".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GetCeraphFollowerPiercing.CeraphFuckAsk".Localized(
                Player.Mf("Master", "Mistress")
            ));

            ClearButtons();

            AddButton(0, "Common.Button.FuckHer".Localized(), null);

            AddButton(1, "Common.Button.RideHer".Localized(), null);

            AddButton(2, "Common.Button.Fuckherass".Localized(), null);

            if (Player.Lust >= 33)
            {
                OutputText("Scenes.Ceraphfollowerscene.GetCeraphFollowerPiercing.HerBodyPartsPrompt".Localized());

                if (Player.HasCock())
                {
                    AddButton(0, "Common.Button.FuckHer".Localized(), SceneInstance<CeraphScene>.Get().MaleFuckCeraphsPussy);

                    if (Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()) != -1)
                        AddButton(2, "Common.Button.Fuckherass".Localized(), SceneInstance<CeraphScene>.Get().ButtRapeCeraph);
                    else
                        AddButton(2, "Common.Button.Fuckherass".Localized(), null);
                }

                if (Player.HasVagina())
                    AddButton(1, "Common.Button.RideHer".Localized(), SceneInstance<CeraphScene>.Get().RideCeraphsCockLikeaBAWSSexclamation11eleven);
            }

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void CawkTawgle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
            {
                OutputText("Scenes.Ceraphfollowerscene.CawkTawgle.AskCeraphHideCock".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CawkTawgle.CeraphHumArching".Localized(
                    Player.Mf("Master", "Mistress")
                ));

                CoC.Instance.Flags.Set(GameFlagIds.CeraphHidingDick, 1);
            }
            else
            {
                OutputText("Scenes.Ceraphfollowerscene.CawkTawgle.AskCeraphShowCock".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CawkTawgle.MoanBulgeEmerge".Localized(
                    Player.Mf("Master", "Mistress")
                ));

                CoC.Instance.Flags.Set(GameFlagIds.CeraphHidingDick, 0);
            }

            DoNext(() => CeraphFollowerAppearance());
        }

        public void CeraphFetishMenu()
        {

            ClearButtons();

            if (!SceneInstance<CeraphScene>.Get().HasPacifism())
                AddButton(0, "Common.Button.GetFetish".Localized(), CeraphHandsOutNewFetishesLikePervCandy);
            else
                AddButton(0, "Common.Button.GetFetish".Localized(), null);

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                AddButton(1, "Common.Button.Removefetish".Localized(), UnfetishifyYourselfWithFollowerCeraph);
            else
                AddButton(1, "Common.Button.Removefetish".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), () => CeraphFollowerAppearance());
        }

        public void CeraphHandsOutNewFetishesLikePervCandy()
        {
            ClearOutput();

            if (!SceneInstance<CeraphScene>.Get().HasExhibition())
            {
                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.CeraphAdvancesCalmly".Localized());

                if (Player.Body.Get<EarsPart>().Pierced == 0)
                    OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.RevealsGreenPiercings".Localized(
                        Player.Mf("Master", "Mistress")
                    ));
                else
                    OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.HoldsPiercedEars".Localized(
                        Player.Mf("Master", "Mistress")
                    ));

                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.ParagraphBreak".Localized());

                if (Player.Body.Get<EarsPart>().Pierced == 0)
                    OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.RubsYourTemples".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.NumbingEarMassage".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.FearsArmorRemoval".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.WatchesYourBody".Localized());

                if (Player.CockTotal() > 0)
                    OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.CockPulseWithHeartbeat".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.VaginaGrowsWetter".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.PartedLipsConfusion".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.NewDesiresSettleIn".Localized(
                    Player.Mf("Master", "Mistress")
                ));

                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.DeclaresYouExhibitionist".Localized());

                if (Player.Body.Get<EarsPart>().Pierced == 0)
                {
                    Player.Body.Get<EarsPart>().Pierced = 1;

                    Player.EarsPShort = "green gem-stone ear-studs";
                }

                Player.ApplyStats(new DynStat(GameStats.Lust, 25f), new DynStat(GameStats.Corruption, 1f));
            }
            else
                if (!SceneInstance<CeraphScene>.Get().HasBondage())
                {
                    OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.GigglesForAnotherMassage".Localized());

                    if (Player.Body.Get<BreastRowPart>().NipplesPierced == 0)
                    {
                        OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.BuildsSuspenseWithStuds".Localized());

                        if (Player.HasFuckableNipples())
                            OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.SluttyWord".Localized());
                        else
                            if (Player.NippleLength < 1)
                                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.CuteWord".Localized());
                            else
                                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.TightWord".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.TellMeYourFetishes".Localized(
                            Player.Mf("Master", "Mistress")
                        ));
                    }

                    if (Player.Body.Get<BreastRowPart>().NipplesPierced > 0)
                        OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.BuildsSuspenseWithNipples".Localized(
                            Player.Mf("Master", "Mistress")
                        ));

                    OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.NoChanceToReply".Localized());

                    if (Player.Body.Get<BreastRowPart>().NipplesPierced > 0)
                        OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.FingersCircleYourNipples".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.AlignsPiercingsOnNipple".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.CuriousAboutNewFetish".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.SmilesHopingYourePleased".Localized(
                        Player.Mf("Master", "Mistress")
                    ));

                    OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.BodyStrugglesAgainstMagic".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 25f), new DynStat(GameStats.Corruption, 1f));

                    if (Player.Body.Get<BreastRowPart>().NipplesPierced == 0)
                    {
                        Player.Body.Get<BreastRowPart>().NipplesPierced = 1;

                        Player.NipplesPShort = "seamless black nipple-studs";

                        Player.NipplesPLong = "Seamless black nipple-studs";
                    }
                }
                else
                    if (!SceneInstance<CeraphScene>.Get().HasPacifism())
                    {
                        OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.DemonessRevealsDiamondStud".Localized(
                            Player.Mf("Master", "Mistress")
                        ));

                        OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.ArousedByFacingDenizens".Localized());

                        if (Player.Cocks.Count > 0)
                        {
                            OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.PreCumOozesFrom".Localized());

                            if (Player.Cocks.Count > 1)
                                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.EachOfPrefix".Localized());

                            OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.YourCocksPhrase".Localized());
                        }
                        else
                            if (Player.HasVagina())
                                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.FeminineMoistureDrools".Localized());
                            else
                                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.BodyAchesForRelease".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.YanksDownYourGear".Localized());

                        if (Player.Cocks.Count > 0)
                        {
                            OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.GrabsYourCock".Localized());

                            if (Player.Cocks[0].isPierced)
                                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.OldPiercingClatters".Localized());

                            OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.SnapsStudThroughFlesh".Localized());

                            Player.Cocks[0].Pierced = 1;

                            Player.Cocks[0].PShortDesc = "diamond cock-stud";

                            Player.Cocks[0].PLongDesc = "Diamond cock-stud";
                        }
                        else
                            if (Player.HasVagina())
                            {
                                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.SpreadsYourLips".Localized());

                                if (Player.Vaginas[0].clitPierced > 0)
                                    OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.OldPiercingClattersFree".Localized());

                                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.GripsFleshAroundBase".Localized());

                                Player.Vaginas[0].clitPierced = 1;

                                Player.Vaginas[0].clitPShort = "diamond clit-stud";

                                Player.Vaginas[0].clitPLong = "Diamond clit-stud";
                            }
                            else
                            {
                                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.PiercesYourEyebrow".Localized());

                                if (Player.eyebrowPierced > 0)
                                    OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.DiscardsOldJewelry".Localized());

                                OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.HurtsMoreThanOught".Localized());

                                Player.eyebrowPierced = 1;

                                Player.EyebrowPShort = "diamond eyebrow-stud";

                                Player.EyebrowPLong = "Diamond eyebrow-stud";
                            }

                        OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.TestsSlutsWork".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.RubsDemonsHair".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.CeraphHandsOutNewFetishesLikePervCandy.SqueaksAndHoldsStill".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 25f), new DynStat(GameStats.Corruption, 2f));
                    }

            SceneInstance<CeraphScene>.Get().GiveFetish();

            DoNext(() => CeraphFollowerAppearance());
        }

        public void UnfetishifyYourselfWithFollowerCeraph()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.UnfetishifyYourselfWithFollowerCeraph.AskCeraphToRemove".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.UnfetishifyYourselfWithFollowerCeraph.InsistsOnRemoval".Localized());

            OutputText("Scenes.Ceraphfollowerscene.UnfetishifyYourselfWithFollowerCeraph.ExplainsReverseOrder".Localized());

            OutputText("Scenes.Ceraphfollowerscene.UnfetishifyYourselfWithFollowerCeraph.ConfirmProceedQuestion".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), GoThroughWithCeraphUnfetishification);

            AddButton(14, "Common.Button.Leave".Localized(), () => CeraphFollowerAppearance());
        }

        public void GoThroughWithCeraphUnfetishification()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.GoThroughWithCeraphUnfetishification.StepsCloserTouchesForehead".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GoThroughWithCeraphUnfetishification.RecoversAsksForTask".Localized(
                Player.Mf("Master", "Mistress")
            ));

            if (Player.Corruption < 33)
                OutputText("Scenes.Ceraphfollowerscene.GoThroughWithCeraphUnfetishification.SparkOfDefianceRemains".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Ceraphfollowerscene.GoThroughWithCeraphUnfetishification.WeighsPunishingHer".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.GoThroughWithCeraphUnfetishification.SlapsHerForTemerity".Localized());

            Player.ApplyStats(new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Corruption, 1f));

            SceneInstance<CeraphScene>.Get().RemoveFetish();

            DoNext(() => CeraphFollowerAppearance());
        }

        public void GoOnATrotWithCeraph()
        {
            ClearOutput();

            CeraphSprite(true);

            RegisterMacro("ceraphHasDick", CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0);

            OutputText("Scenes.Ceraphfollowerscene.GoOnATrotWithCeraph.ShouldSpendMoreTime".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GoOnATrotWithCeraph.FeignsIgnoranceKindly".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GoOnATrotWithCeraph.CrawlsUnderYourHarness".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GoOnATrotWithCeraph.WrigglesSecondSpine".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GoOnATrotWithCeraph.DelightedGroanStepForward".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GoOnATrotWithCeraph.HopesStrollPleasesYou".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GoOnATrotWithCeraph.LongerRideBurnsHotter".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GoOnATrotWithCeraph.TailFlicksAndMilks".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, .25f));

            DoNext(() => FinishTheTrot());
        }

        public void FinishTheTrot()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.FinishTheTrot.SatisfiedUnbindsHarness".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckFollowerCeraphsVagoo()
        {
            ClearOutput();

            int x = Player.CockThatFits(115);

            if (x < 0)
                x = 0;

            var y = Player.CockThatFits2(115);

            CeraphSprite(true);

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.CeraphPussyUseAnnounced".Localized(
                Player.Mf("Master", "Mistress")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.HalfErectCock".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.PeriodSpace".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.AllFoursArmsBound".Localized(
                    Player.Mf("Master", "Mistress")
                ));
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.AllFoursArmsPinned".Localized());
                else
                {
                    OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.PushOntoAllFours".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                        OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.DickReleasingPreCum".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.RoughPlayArousal".Localized());
                }

            if (Player.Corruption < 33)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.UnexpectedRequestIdea".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.BoldRequestAccepted".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.PredatoryInviteThrill".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.GrabCeraphThighs".Localized());

            if (Player.Strength < 60)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.LiftHerOffGround".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.EasilyLiftHerStraddle".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.DragDemonOntoCock".Localized(
                Player.CockDescript(x)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.IgnoringDanglingPreCum".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.HeatWashingOffMons".Localized(
                    Player.CockHead(x)
                ));

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.BuryAllInchesDeep".Localized(
                NumberUtils.Num2Text(Player.Cocks[x].Length),

                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) > 150)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.DistortBodyAroundBulk".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.ImmerseInDecadentWetness".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.BuriedToHiltMoment".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.SlutMoansSlidingHome".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.CockDanglingPreSpunk".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.TightAsVirginWet".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.DemonPantsLegsCrossed".Localized(
                Player.Mf("Master", "Mistress")
            ));

            if (y >= 0)
            {
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.WickedIdeaTargetsAsshole".Localized(
                    Player.CockDescript(y)
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.SquealSprayThickJizz".Localized());

                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.SlaveWigglesBackside".Localized(
                    Player.Mf("chuckle", "giggle")
                ));

                if (Player.CockTotal() >= 3)
                {
                    OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.SadlyExtraPenis".Localized());

                    if (Player.CockTotal() > 3)
                        OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.ExtraPenisesSlideCheeks".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.ExtraPenisSlidesCheeks".Localized());
                }

                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.ParagraphBreak".Localized());
            }
            else
            {
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.WorkingHarderAndHarder".Localized(
                    Player.CockDescript(x)
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.CockSpewingPreCum".Localized());

                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.SuckingSqueezingEmbrace".Localized());
            }

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.ShudderingExplosionWarmth".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.AssholeDevoursSeed".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.HoldDeepInsideWanton".Localized());

            if (y >= 0)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.PluralSuffix".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.BastingCorruptSlaveTunnel".Localized());

            if (y >= 0)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.PluralSuffixAgain".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.CeraphScreamsSloppySpooge".Localized(
                Player.Mf("Master", "Mistress")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.CockPumpingDemonicSpunk".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.TrailsOffPussyTeasing".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.CeraphSighs".Localized());

            if (Player.CumQ() >= 700)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.RubbingHer".Localized());

            if (Player.CumQ() >= 1400)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.BloatedWord".Localized());

            if (Player.CumQ() >= 700)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.BulgingBellyPurrs".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.PurringWord".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.GoodDomGratitude".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.SmirkDropIntoMud".Localized());

            if (Player.Tallness >= 80)
                OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.ChestWord".Localized());
            else
                if (Player.Tallness >= 60)
                    OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.LipsWord".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.ForeheadWord".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.AndMouthsThanks".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckFollowerCeraphsVagoo.NodAndSlapAss".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, .25f));

            DoNext(() => ReturnToCamp(1));
        }

        public void FollowerCeraphTongueFucking()
        {
            ClearOutput();

            CeraphSprite(true);

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.SpreadLegsForSlave".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.AndCocksFragment".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.CommandServeTongue".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.CeraphNodsKneels".Localized());

            if (Player.Lust < 50)
                OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.StillHoodedWord".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.EngorgedWord".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.ClitOverPubicSkin".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.SharpIntakeReelsTongue".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.SlutWinksLicksCrown".Localized());

            if (Player.Wetness() < 3)
                OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.MoistWord".Localized());
            else
                if (Player.Wetness() < 4)
                    OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.WetWord".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.SoakingWord".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.TwistFlexibleMouthDick".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.HipsTremblingBreathless".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.JoltOfPleasureBlasts".Localized());

            if (Player.GetClitLength() >= 3)
                OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.FellatingWord".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.LickingWord".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.ShudderAtClit".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.GrabCeraphHeadMash".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.CeraphHumsEyesClosed".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.CheeksBulgingThroatStruggle".Localized());
            else
                if (Player.Wetness() >= 4)
                    OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.CheeksSlightlyBulged".Localized());
                else
                    if (Player.Wetness() >= 3)
                        OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.SheGulpsDownSometimes".Localized());
                    else
                        if (Player.Wetness() >= 2)
                            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.ProduceEnoughToGulp".Localized());
                        else
                            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.PussyNotGushing".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.ShakingClenchingComeDown".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphLickedYouOut) == 0)
                OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.WondersIfSheCame".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.TongueReturnsToNormal".Localized(
                    Player.Mf("Master", "Mistress")
                ));

            OutputText("Scenes.Ceraphfollowerscene.FollowerCeraphTongueFucking.PullBackToLick".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CeraphLickedYouOut, CoC.Instance.Flags.Get(GameFlagIds.CeraphLickedYouOut) + 1);

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, .25f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CeraphTentacleGrape()
        {
            ClearOutput();

            CeraphSprite(true);

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.TearOffArmorInstruct".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.GlimpseCeraphDemonicPrick".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.ParagraphBreak".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.AmethystCocksLiftYou".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.WarmOrificeAligns".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.AllYourGirth".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.AllMembersAtOnce".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.ArchBackInPleasure".Localized());

            if (Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.SheathWord".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.BaseWord".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.CiliaSquirmInTunnel".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.DistractedFromVagina".Localized());

            Player.CuntChange(24, true, true, false);

            Player.ButtChange(24, true, true, false);

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.HumidAirSuspension".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.SLetter".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.CeraphAsksKiss".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.SmileNodLickLips".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0 && CoC.Instance.Flags.Get(GameFlagIds.CeraphPunished) == 0)
                OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.CeraphSpearsHerPrick".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.CeraphThrowsHeadBack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.CeraphCockSpraysCum".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.BodySeizesExplodes".Localized());

            if (Player.CumQ() >= 800)
                OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.BulgesFromSheerSize".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.SucksWord".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.TentacleSwallowsCum".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.WakeInCumPuddle".Localized(
                Player.Mf("Master", "Mistress")
            ));

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, .25f));

            Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 32, 61);

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0 && CoC.Instance.Flags.Get(GameFlagIds.CeraphPunished) == 0)
            {
                OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.SmirkWonderPunish".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Punish".Localized(), PunishCeraphForSurpriseThroatFuck);

                SetExitButton("Common.Button.Back".Localized());
            }
            else
            {
                OutputText("Scenes.Ceraphfollowerscene.CeraphTentacleGrape.NodAndCleanUp".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void PunishCeraphForSurpriseThroatFuck()
        {
            ClearOutput();

            CeraphSprite(true);

            CoC.Instance.Flags.Set(GameFlagIds.CeraphPunished, 1);

            OutputText("Scenes.Ceraphfollowerscene.PunishCeraphForSurpriseThroatFuck.BendOverPunishAss".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.PunishCeraphForSurpriseThroatFuck.ReleaseBruisedDemon".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void CatgirlEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CatgirlEncounter.CeraphLateSummon".Localized());

            if (Random.Shared.Next(50) <= Player.GetEvasionChance())
                OutputText("Scenes.Ceraphfollowerscene.CatgirlEncounter.CatgirlTacklesYou".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.CatgirlEncounter.MissesAndCrashes".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CatgirlEncounter.ParagraphBreak".Localized());

            if (noFur)
                OutputText("Scenes.Ceraphfollowerscene.CatgirlEncounter.StudyHumanSisters".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.CatgirlEncounter.StudyFurSisters".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CatgirlEncounter.ChorusSistersSpeak".Localized((noFur ? "" : " furry,"), (noFur ? "Common.Text.Eared".Localized() : "Common.Text.Faced".Localized())));

            ClearButtons();

            AddButton(0, "Common.Button.Sex".Localized(), FuckCeraphsCatgirls);

            AddButton(14, "Common.Button.Leave".Localized(), DeclineCeraphsCatgirls);
        }

        public void DeclineCeraphsCatgirls()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.DeclineCeraphsCatgirls.DeclineKittySisters".Localized());

            DoNext(PlayerMenu);
        }

        public void FuckCeraphsCatgirls()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.AgreeToPleasure".Localized((noFur ? "" : " furry")));

            OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.ScratchBehindEars".Localized(Player.CockDescript(x), (noFur ? "" : "-furred")));

            OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.SisterRubsOnYou".Localized());

            if (Player.BiggestLactation() > 1)
                OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.MilkSeepsOut".Localized());
            else
            {
                OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.DelightsInBreasts".Localized((noFur ? "" : " fuzzy")));

                if (Random.Shared.Next(5) == 0 && !noFur)
                    OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.FurTicklesSneeze".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.VibratingWildly".Localized());
            }

            OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.SisterLaughsOnCock".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.CatOnYourFace".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.HandBetweenTits".Localized((noFur ? "Common.Text.Large".Localized() : "Scenes.Ivorysuccubusscene.AnalTail.FurryWord".Localized()), Player.CockDescript(x)));
            else
                OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.GrabNeglectedDickflesh".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.ParagraphBreak".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.StimulationBuildUp".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.TorrentCoatsGirls".Localized());
            else
                if (Player.CumQ() >= 300)
                    OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.CreamLoadForGirls".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.ImmodestSquirts".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.RelaxSpentOnBack".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.LickRemnantsClean".Localized((noFur ? "Common.Text.Themselves".Localized() : "Common.Text.TheirFur".Localized())));

            OutputText("Scenes.Ceraphfollowerscene.FuckCeraphsCatgirls.GirlsThankForCream".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CeraphUrtaRoleplay()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplay.CeraphPurrsRoleplay".Localized(
                Player.Mf("Master", "Mistress"),

                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplay.ShuddersBreathingDeep".Localized((noFur ? "Common.Text.HerLavenderSkinHasChangedTo".Localized() : "Common.Text.YouSeeThatHerLavenderSkin".Localized()), (noFur ? "" : " Placing her fingers at the bridge of her nose and her thumb under her jaw, she cocks her head to one side and yanks forward, her skull deforming as the front of her face is pulled into a vulpine muzzle, lips thickening into a glistening black pucker as she blows you a kiss.")));

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplay.CurtseyInDeference".Localized(Player.Mf("Master", "Mistress"), (noFur ? "" : " furred"), Player.Mf("Master", "Mistress"), (noFur ? "n" : " fuzzy,")));

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplay.UrtaFlusteredGreeting".Localized());

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.Sober".Localized(), CeraphUrtaRoleplaySober);
            else
                AddButton(0, "Common.Button.Sober".Localized(), null);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.Drunk".Localized(), CeraphUrtaRoleplayDrunk);
            else
                AddButton(1, "Common.Button.Drunk".Localized(), null);
        }

        public void CeraphUrtaRoleplayDrunk()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk.BottomsUpToast".Localized((noFur ? "" : " She looks closely at the bottle and blinks several times. [say: Wh- what did you put in this?]")));

            if (!noFur)
            {
                OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk.AdmitHerAwareness".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk.UrtaFurTransforms".Localized());
            }

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk.HoldShouldersGaze".Localized((noFur ? "Common.Text.You".Localized() : "Common.Text.WithASighingBreathYouWhisper".Localized()), (noFur ? "" : "twists her head too far to the side, trying to compensate for a muzzle that's no longer there before giggling into your mouth and turning back too far, bumping her nose against yours. She lets out a brief bark of laughter and "), Player.ButtDescript()));

            Player.ApplyStats(new DynStat(GameStats.Lust, 125f));

            DoNext(() => CeraphUrtaRoleplayDrunk2());
        }

        public void CeraphUrtaRoleplayDrunk2()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk2.CockThrobsBackside".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk2.UrtaChantsDamnit".Localized());

            if (Player.IsGoo() || Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk2.WriggleWiderStance".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk2.SpreadYourLegs".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk2.TrappedBetweenKnees".Localized());

            Player.CuntChange(60, true, true, false);

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk2.ParagraphBreak".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk2.CockSwellsDanger".Localized((noFur ? "" : "No condoms for sneaky bitches who spike drinks,] her husky voice right on the edge. [say: Fur isn't the only thing I've lost. ")));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) > 0 || false || false)
                OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk2.KnockYouUpTaunt".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk2.BraceOnYourShoulder".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk2.ThirdCumOnCervix".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk2.SympatheticClimax".Localized(Player.SMultiCockDesc(), (noFur ? "" : " [say: Hey, I can finally take a shower without smelling like a wet dog afterwards,] she realizes, happily.")));

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk2.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplayDrunk2.WakeCleanedSticky".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 2f));

            if (Player.HasVagina() && Player.TotalFertility() >= Random.Shared.Next(45) && Player.Pregnancy.Incubation == 0)
                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 32, 61);

            DoNext(() => ReturnToCamp(1));
        }

        public void CeraphUrtaRoleplaySober()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplaySober.PutBottleDown".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplaySober.UrtaShockedTears".Localized((noFur ? "" : " the fine, ebony fuzz of")));

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplaySober.RestoredToWoman".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplaySober.LedBackToCot".Localized((noFur ? "Common.Text.Skin".Localized() : "Common.Text.Fur".Localized())));

            if (Player.Horns.Id.Path != "none")
                OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplaySober.HandsAtHerSides".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 200f));

            DoNext(() => CeraphUrtaRoleplaySober2());
        }

        public void CeraphUrtaRoleplaySober2()
        {
            ClearOutput();

            HideUpDown();

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplaySober2.HandsAlongAbs".Localized((noFur ? "" : " the lighter fur of")));

            if (Player.CockArea(0) > 150)
                OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplaySober2.SizeNoImpediment".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplaySober2.ParagraphBreak".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplaySober2.FinallyBottomOut".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplaySober2.SharedClimaxCrest".Localized(
                Player.CockDescript(Player.BiggestCockIndex())
            ));

            OutputText("Scenes.Ceraphfollowerscene.CeraphUrtaRoleplaySober2.UrtaGoneOnReturn".Localized(
                Player.Mf("Master", "Mistress")
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CarephCorruptionSlaves()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CarephCorruptionSlaves.HumanGirlInstead".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CarephCorruptionSlaves.ImpReadsLetter".Localized(
                Player.Mf("Master", "Mistress"),

                Player.Mf("Master", "Mistress"),

                Player.Mf("Master", "Mistress")
            ));

            if (Player.Corruption >= 75)
            {
                OutputText("Scenes.Ceraphfollowerscene.CarephCorruptionSlaves.GlareAtImpInsult".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CarephCorruptionSlaves.ImpRecoilsApology".Localized(
                    Player.Mf("Master", "Mistress")
                ));

                OutputText("Scenes.Ceraphfollowerscene.CarephCorruptionSlaves.DismissImpWave".Localized());
            }

            OutputText("Scenes.Ceraphfollowerscene.CarephCorruptionSlaves.ScrollVialOffering".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CarephCorruptionSlaves.AcceptOfferingPrompt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), CeraphLackeyCorruption);

            AddButton(14, "Common.Button.Leave".Localized(), MakeCarephsLackeysLeave);
        }

        public void MakeCarephsLackeysLeave()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.MakeCarephsLackeysLeave.WaveImpsAway".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.MakeCarephsLackeysLeave.ImpsReplyMeekly".Localized(
                Player.Mf("Master", "Mistress")
            ));

            Go<CampScene>();
        }

        public void CeraphLackeyCorruption()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.AcceptCeraphOffering".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.AppraiseGirlAppearance".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.GazeAtHerBreasts".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.ProbeHerPussy".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.TasteBlackFluidVial".Localized());

            if (!Player.HasCock())
            {
                OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.BloodRushPleasure".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.ClitSwells".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.SmallBumpForms".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.HugeErectionDevelops".Localized());
            }

            var x = Player.BiggestCockIndex();

            var demon = false;

            if ((Player.HasCock()) && (Player.Cocks[x].Id == BodyPartTypes.Demon))
                    demon = true;

            if (Player.CockArea(Player.BiggestCockIndex()) < 48 && Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.CockThrobsLarger".Localized(
                    Player.CockDescript(x)
                ));

            if (!Player.HasCock() || !demon)
                OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.MuskyScentEmanates".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.StrokeDemonicPrick".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.AdmirePulsatingMember".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.OrderRemoveBindings".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.TeaseGirlWithShaft".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.ScrotumAnd".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.ImpsLaughAtGirl".Localized());

            if (Player.CockArea(x) >= 60)
                OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.FearRippingHerApart".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.PlungeIntoHerDepths".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.EmptyFragment".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.BallsFragment".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.YourselfFragment".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.HipsPoundWildly".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.FuckHerPowerfully".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.UnableToStopPounding".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.SerpentineTongueKiss".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.SheGigglesAfterTryst".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.SuccubusReadsThoughts".Localized());

            if (!demon)
                OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.LiftOffAndSit".Localized(
                    Player.Mf("Master", "Mistress")
                ));

            if (!demon && !Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.PhallusShrinksAway".Localized());

            if (!demon)
                OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.SentencePeriod".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.SeductiveSmileSlappingSound".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLackeyCorruption.DoYouQuestion".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -3f), new DynStat(GameStats.Corruption, 5f));

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), AcceptMoreCeraphFauxCorruption);

            AddButton(1, "Common.Button.No".Localized(), DeclineCeraphFauxCorruption);

            AddButton(4, "Common.Button.NeverAgain".Localized(), IQuitCeraphCorruptionDemons);
        }

        public void IQuitCeraphCorruptionDemons()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.IQuitCeraphCorruptionDemons.RefuseHerGameLoudly".Localized());

            OutputText("Scenes.Ceraphfollowerscene.IQuitCeraphCorruptionDemons.SheUnfurlsWings".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CeraphFauxCorruptionDisabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void DeclineCeraphFauxCorruption()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.DeclineCeraphFauxCorruption.RefuseGrantingImpsRelease".Localized());

            OutputText("Scenes.Ceraphfollowerscene.DeclineCeraphFauxCorruption.SheFliesAway".Localized(
                Player.Mf("Master's", "Mistress")
            ));

            DoNext(() => ReturnToCamp(1));
        }

        public void AcceptMoreCeraphFauxCorruption()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.AcceptMoreCeraphFauxCorruption.SmirkOrderImpsPleasure".Localized());

            OutputText("Scenes.Ceraphfollowerscene.AcceptMoreCeraphFauxCorruption.ImpsEyesGlow".Localized());

            OutputText("Scenes.Ceraphfollowerscene.AcceptMoreCeraphFauxCorruption.ImpsBrutallyFuckSuccubus".Localized());

            OutputText("Scenes.Ceraphfollowerscene.AcceptMoreCeraphFauxCorruption.ImpsClimaxQuickly".Localized());

            OutputText("Scenes.Ceraphfollowerscene.AcceptMoreCeraphFauxCorruption.LewdSmileUsedTransformed".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.AcceptMoreCeraphFauxCorruption.LickCumOffBody".Localized(
                Player.Mf("Master", "Mistress")
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f), new DynStat(GameStats.Corruption, 2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void SweetieNOOOO()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.CeraphHuffsFeigning".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.DescribeCountryGirl".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.GestureChangesScenery".Localized());

            if (Player.HasKeyItem("Breast Milker - Installed At Whitney's Farm"))
                OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.SimilarToFactoryOne".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.LikeIngnamModified".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.ExplorationPutOnHold".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphSweetieNoRpDone) == 0)
                OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.ConfusedStareAnswer".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.ChuckleAtSizeDisparity".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.MoldsHerHorns".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.WinkSyringeFingers".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.CeraphPlopsDown".Localized((noFur ? "Common.Text.Knee".Localized() : "Scenes.Anemonescene.KidADreams.Thigh".Localized())));

            OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.CowLikeGrowsHips".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) > 0 && CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.NubblyMemberColorShift".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.HairColorationShift".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.LastTouchFrameJostles".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphSweetieNoRpUtterMooDisabled) == 1)
                DoNext(() => PostUdderChoice());
            else
            {
                OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.SultryTonesTurnEarthy".Localized(
                    Player.Mf("Master", "Mistress")
                ));

                OutputText("Scenes.Ceraphfollowerscene.SweetieNOOOO.UdderQuestionPosed".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Udder".Localized(), YesUdderPWEASE);

                AddButton(1, "Common.Button.NoUdder".Localized(), () => NoUdderPlz());

                AddButton(2, "Common.Button.NeverUdder".Localized(), () => NoUdderPlz());
            }
        }

        public void NoUdderPlz(bool perm = false)
        {
            ClearOutput();

            if (perm)
                CoC.Instance.Flags.Set(GameFlagIds.CeraphSweetieNoRpUtterMooDisabled, 1);

            OutputText("Scenes.Ceraphfollowerscene.NoUdderPlz.SharpHeadShakeRefusal".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CeraphSweetieNoRpUtterMooDisabled, 0);

            PostUdderChoice();
        }

        public void YesUdderPWEASE()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.YesUdderPWEASE.BrightEyesConsent".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CeraphSweetieNoRpUtterMooDisabled, 1);

            PostUdderChoice();
        }

        public void PostUdderChoice(bool newl = false)
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.TurnBackWaiting".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.MarbleTripsOverBucket".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphSweetieNoRpUtterMooDisabled) == 1)
                OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.PrepFourExtraTubes".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.RipStrainedBlouse".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphSweetieNoRpUtterMooDisabled) == 1)
                OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.FourCupsSuckNubs".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.ParagraphBreak".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.MarbleJostledByMilkers".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.KneadMarbleButt".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.SheBecomesSlave".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.EyesBulgeCumFunnel".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.NipplesReddenedMilked".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.BallsTightening".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.BodyReadiness".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.WindUpLastThrust".Localized());

            var cum = Player.CumQ();

            if (cum < 50)
                OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.AssTakesPackage".Localized());
            else
                if (cum < 250)
                    OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.GroansAtSeedAmount".Localized());
                else
                    if (cum < 750)
                        OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.BellyBulgesMore".Localized());
                    else
                        if (cum < 2000)
                            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.NormalGirlsCannot".Localized());
                        else
                            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.BodyWrithesEcstatic".Localized());

            if (cum >= 10000)
                OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.ConvulsionsDislodgeFunnel".Localized());

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.OtherCockSprays".Localized());
            else
                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.OtherCocksSpray".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.ArticleWord".Localized());

                if (cum < 50)
                    OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.GentleWord".Localized());
                else
                    if (cum < 250)
                        OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.GenerousWord".Localized());
                    else
                        if (cum < 750)
                            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.HeadyWord".Localized());
                        else
                            if (cum < 1500)
                                OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.WildWord".Localized());
                            else
                                OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.MassiveWord".Localized());

                OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.SemenOverStalls".Localized());

                if (cum >= 250 && cum < 1000)
                    OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.SpatteringEntireArea".Localized());
                else
                    if (cum >= 1000 && cum < 3000)
                        OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.DrenchingAreaWhite".Localized());
                    else
                        if (cum >= 3000)
                            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.CoatingThickAlabaster".Localized());

                OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.SentencePeriod".Localized());
            }

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.SpentFreeYourself".Localized());

            if (cum >= 2000 && Player.Corruption >= 66)
                OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.ChuckleAtDrizzle".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.TurnAwayVictorious".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.PostUdderChoice.GripFadesSheGone".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -5f), new DynStat(GameStats.Corruption, 3f));

            DoNext(() => ReturnToCamp(1));
        }

        public void Cerminika()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphRoleplayAsDominikaCount) == 0)
            {
                OutputText("Scenes.Ceraphfollowerscene.Cerminika.CeraphAsks".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.ExplainDarkLippedWitch".Localized());
            }

            OutputText("Scenes.Ceraphfollowerscene.Cerminika.CeraphEyesReveal".Localized());

            OutputText("Scenes.Ceraphfollowerscene.Cerminika.FigureShrinksSlighter".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.Cerminika.PrickShrinksToInches".Localized());

            OutputText("Scenes.Ceraphfollowerscene.Cerminika.SentencePeriodBreak".Localized());

            OutputText("Scenes.Ceraphfollowerscene.Cerminika.BodyShrinksLipsGrow".Localized());

            OutputText("Scenes.Ceraphfollowerscene.Cerminika.DominikaStandsNude".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Ceraphfollowerscene.Cerminika.DarkLipsPurrChampion".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.DickPressesCrotch".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.PurrsBetweenKiss".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.HandsEaseYouDown".Localized());

                if (Player.BiggestBreastSize() >= 0)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.PlayfulChestSqueeze".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.PullsArmorAway".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.PrickRemainsFirmWord".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.TongueInkyLipsWord".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.PurrsHowLong".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.SlidesBodyBack".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.PrickBaseTeasesVeins".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.GrinsReachesTop".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.GrabHipsPullLap".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.CockShootsStraightUp".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.SmirkBounceHer".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.PrickHeatBouncingWord".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.GaspsSavorItWord".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.SlapHerTit".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.FramingExpressionInjustice".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.CraftedRetortSuperiority".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.SingleBeadPrecum".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.HairBackBehindEar".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.SurpriseHerTwistRoll".Localized(
                    Player.Mf("bastard", "bitch")
                ));

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.YetUndecidedMouth".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.DarkLipsStammerFuck".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.PrickBouncesBellies".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.HipsPoundingAway".Localized());

                if (Player.BiggestBreastSize() >= 1)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.SlapOwnChestWord".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.HissingBloodyMurderWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.LightSpurtWarmsHips".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.FingersTrailShoulders".Localized(
                    Player.Mf("handsome", "gorgeous")
                ));

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.NoIntentControlPace".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.HearCockKickGround".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.BeginsThenShouts".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.GrindDuskyCheeks".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.DenyRequest".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.SpearPlushPosterior".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.StammersPleaseClench".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.OrgasmDenialException".Localized(
                    Player.Mf("bastard", "bitch")
                ));

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.CalmAffirm".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.WailsFrustrationPleasure".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.LiftFucktoyUpright".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.PrickEngorgedBumps".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.GaspsTakeItAll".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.RuinedChastityEmbrace".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.DumpLoadsAddict".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.ShuddersGutsWarmth".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.CuntWaterfallsJuices".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.DemonicPrickSpews".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.ParagraphBreak".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.HandsDragChest".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.JizzLickedClean".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.DemonessLapsClean".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.CeraphOwnVoiceAsks".Localized());

                if (Player.CockArea(0) >= 75)
                    OutputText("Scenes.Ceraphfollowerscene.Cerminika.DoubtAssPerfection".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.HopeSlutAppreciates".Localized());

                OutputText("Scenes.Ceraphfollowerscene.Cerminika.WhispersInviteEar".Localized());
            }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 2f));

            CoC.Instance.Flags.Set(GameFlagIds.CeraphRoleplayAsDominikaCount, CoC.Instance.Flags.Get(GameFlagIds.CeraphRoleplayAsDominikaCount) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void TellyRolepay()
        {
            ClearOutput();

            if (Save.GetInt("roleplayAsTellyCount", 0) == 0)
                OutputText("Scenes.Ceraphfollowerscene.TellyRolepay.DemonPondersChin".Localized());

            Save.SetInt("roleplayAsTellyCount", Save.GetInt("roleplayAsTellyCount", 0) + 1);

            OutputText("Scenes.Ceraphfollowerscene.TellyRolepay.DemonessShimmersSetting".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRolepay.TellyIntroduction".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Kiss".Localized(), TellyRpKiss);

            AddButton(1, "Common.Button.Sowing".Localized(), TellyRpSowing);
        }

        public void TellyRpKiss()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.TellyRpKiss.KissRequestBlush".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpKiss.PresentGemsPayment".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpKiss.WetSmoochDisengage".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpKiss.SilenceBrokenThanks".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpKiss.TellySkirtPanties".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            ClearButtons();

            AddButton(0, "Common.Button.KissHerLips".Localized(), TellyRpLick);

            AddButton(1, "Common.Button.Pantyjob".Localized(), TellyRpPanty);
        }

        public void TellyRpLick()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.TellyRpLick.PushAliceCounter".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpLick.TellyGulpsOntoCounter".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpLick.ShopkeepChargeLicking".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpLick.WantsServicePleasure".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpLick.HesitatesSaveSpecial".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            ClearButtons();

            AddButton(0, "Common.Button.Fuck".Localized(), TellyRpFuck);

            AddButton(1, "Common.Button.ThighGrind".Localized(), TellyRpGrind);

            AddButton(2, "Scenes.CorruptedGlade.NeverMind".Localized(), TellyRpEarlyEnd);
        }

        public void TellyRpFuck()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.TellyRpFuck.ReadyYourselfAction".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpFuck.LabiaSpreadEntry".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpFuck.RelaxThenWithdraw".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpFuck.PumpFasterHarder".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpFuck.GroanOrgasmRhythm".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, .25f));

            DoNext(() => TellyRpEnd());
        }

        public void TellyRpGrind()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.TellyRpGrind.ReadyYourselfGrind".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpGrind.MerchantWhinesHolding".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpGrind.AliceCuddlesShaking".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpGrind.OncomingOrgasmNuzzle".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, .25f));

            DoNext(() => TellyRpEnd());
        }

        public void TellyRpEarlyEnd()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.TellyRpEarlyEnd.RecklessSpendingDawn".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpEarlyEnd.TellyBlinkingCredit".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpEarlyEnd.DebtTemptingOffer".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpEarlyEnd.DemonessCoughsIllusion".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpEarlyEnd.PracticeSmartSpending".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TellyRpPanty()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.TellyRpPanty.FutureSalesHandling".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpPanty.HandsStrokePanties".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpPanty.MoneysWorthHarder".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpPanty.BlushesKeepsStroking".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpPanty.ComeByTreats".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpPanty.ContentedSighFade".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, .25f));

            DoNext(() => ReturnToCamp(1));
        }

        public void TellyRpEnd()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.TellyRpEnd.IllusionCrumblesCeraph".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpEnd.FineJobUnwound".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TellyRpSowing()
        {
            ClearOutput();

            var smallestCock = Player.Cocks[Player.SmallestCockIndex()];

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.OfferingToSew".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.BrightSquintingGrin".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.CrawlsOntoBed".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.CrawlingCloserOverDemon".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.AttractionBeyondCurves".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.SeesBuildingArousal".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.SPrefixUndressing".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.TellyUndressesBeside".Localized(Player.MultiCockDescriptLight(Player.SmallestCockIndex())));

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.RaisingHerPelvis".Localized(
                Player.MultiCockDescriptLight(Player.SmallestCockIndex())
            ));

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.HandsAlongHerThighs".Localized(
                smallestCock.Length > 8 || smallestCock.Thickness >= 3 ? "Common.Text.ItsAMiracleSheCanFit".Localized() : "Common.Text.SheHoldsYouLikeAVice".Localized() + (smallestCock.Length < 5 ? ", perfectly sized for your equipment" : "")
            ));

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.SwitchesToBouncing".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.CuddlingWhilePumping".Localized(Player.MultiCockDescriptLight(Player.SmallestCockIndex())));

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.QuickeningPace".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowing.DizzyingAfterglow".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, .25f));

            DoNext(() => TellyRpSowingEnd());
        }

        public void TellyRpSowingEnd()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowingEnd.MayHaveImpregnatedYou".Localized());

            OutputText("Scenes.Ceraphfollowerscene.TellyRpSowingEnd.GetsUpAndRedresses".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void EncounterZetsuko()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ZetsukoMet, CoC.Instance.Flags.Get(GameFlagIds.ZetsukoMet) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.ZetsukoMet) == 1)
            {
                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.UnexpectedZetsuko".Localized());

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.ZetsukoApologizes".Localized(
                    Player.Mf("Master", "other Mistress")
                ));

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.CreepyCanineGrin".Localized());

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.AsksJacketLoosened".Localized(
                    Player.Mf("Master", "Mistress")
                ));

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.FindsPinnedNote".Localized());

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.SayStartWithName".Localized(
                    Player.Mf("Master", "Mistress")
                ));

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.LetterApologizesAbsence".Localized());

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.LetterSignature".Localized());

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.SafetyWordPostscript".Localized());

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.ConfusedBlink".Localized());

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.MockingOutsmarted".Localized(
                    Player.Mf("Master", "Mistress"),

                    Player.Mf("Master", "Mistress")
                ));

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.LegsSplayedLewdly".Localized());

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.AsksPussyLiking".Localized(
                    Player.Mf("Master", "Mistress")
                ));

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.ChoiceUseZetsuko".Localized());
            }
            else
            {
                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.ZetsukoWashesIn".Localized());

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.ZetsukoApologizesAgain".Localized(
                    Player.Mf("Master", "other Mistress"),

                    Player.Mf("Master", "Mistress")
                ));

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.RefusesToFreeHer".Localized());

                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.ChoiceUseZetsukoAgain".Localized());
            }

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.FuckVagina".Localized(), FuckZetsukosTonguepussy);
            else
                AddButton(0, "Common.Button.FuckVagina".Localized(), null);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.GetTongued".Localized(), GetTonguedByZetsuko);
            else
                AddButton(1, "Common.Button.GetTongued".Localized(), null);

            if (silly)
            {
                OutputText("Scenes.Ceraphfollowerscene.EncounterZetsuko.RecallsSafetyWord".Localized());

                AddButton(2, "Common.Button.Banana".Localized(), ZetsukoBanana);
            }

            AddButton(14, "Common.Button.Leave".Localized(), NoZetsukoLoveToday);
        }

        public void FuckZetsukosTonguepussy()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.CuriosityAtTongue".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.MarvelsTongueControl".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.EagerForYourDick".Localized(
                Player.Mf("Master's", "Mistress's"),

                Player.Mf("Master", "Mistress")
            ));

            if (Player.CockArea(x) >= 100)
                OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.WondersHowItFits".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.GropingWhileWatching".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.InNoTimeWord".Localized());

            if (Player.CockArea(x) >= 100)
                OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.SomehowWord".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.HiltedInsideOrifice".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.SlitheringAcrossWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.LickingOuterLabia".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.TongueMovesBackward".Localized());

            if (Player.Balls > 0 || Player.HasVagina())
                OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.AndWord".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.TeasingWhileGathering".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.MoansRollingEyesBack".Localized(
                Player.Mf("Master's", "Mistress's"),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.PaybackForInterruption".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.BegsForPunishment".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.TorturingNipples".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.ImpossibleToResist".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() <= 100)
                OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.OrgasmEndsQuickly".Localized());
            else
                if (Player.CumQ() <= 500)
                    OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.FiringCumRibbons".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.CuntMilksShaft".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.ParagraphBreak".Localized());

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.PussySucksWhilePulling".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Ceraphfollowerscene.FuckZetsukosTonguepussy.HopesToTasteAgain".Localized(
                Player.Mf("Master", "Mistress")
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, .25f), new DynStat(GameStats.Sensitivity, -5f), new DynStat(GameStats.Corruption, 2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void GetTonguedByZetsuko()
        {
            ClearOutput();

            int x = -1;

            if (Player.HasCock())
                x = Player.BiggestCockIndex();

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.HeatRushesToGroin".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.BegsToTaste".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.TongueSlithersCloser".Localized(
                Player.VaginaDescript(0)
            ));

            if (x >= 0)
                OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.WrappingTongueWord".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.HeadingForSlit".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.TipProbesDepths".Localized());

            if (x >= 0)
            {
                OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.FeetContortionFeat".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.ItWord".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.ThemWord".Localized());

                OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.ToesStrokingShaft".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.SLetter".Localized());

                OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.DexterousToesGrip".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.SLetterAgain".Localized());

                OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.AlmostLikeHands".Localized());
            }

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.HipsForwardCurling".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.DeliciousPraise".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.RockingOnTongue".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.ClenchingAroundTongue".Localized(
                Player.VaginaDescript(0)
            ));

            if (x >= 0)
            {
                OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.TwitchingBetweenFeet".Localized(
                    Player.SMultiCockDesc()
                ));

                if (Player.CumQ() <= 100)
                    OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.HealthyCumSpray".Localized());
                else
                    if (Player.CumQ() <= 500)
                        OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.DistendingWithSpunk".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.HeavyCumDownpour".Localized());

                OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.RevealsSecondTongue".Localized());
            }

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.ParagraphBreak".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.JuicesPoolInside".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.OverflowDribblesDown".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.OddGurglingDrain".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.PullsTongueOut".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GetTonguedByZetsuko.HopesToTasteAgain".Localized(
                Player.Mf("Master", "Mistress")
            ));

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, .25f), new DynStat(GameStats.Sensitivity, -5f), new DynStat(GameStats.Corruption, 2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ZetsukoBanana()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.ZetsukoBanana.WhatIsABanana".Localized());

            OutputText("Scenes.Ceraphfollowerscene.ZetsukoBanana.SaysBananaPlain".Localized());

            OutputText("Scenes.Ceraphfollowerscene.ZetsukoBanana.SaysBananaRepeatedly".Localized());

            OutputText("Scenes.Ceraphfollowerscene.ZetsukoBanana.MaraeProtestsBanana".Localized());

            OutputText("Scenes.Ceraphfollowerscene.ZetsukoBanana.CollapsesFaceDown".Localized());

            OutputText("Scenes.Ceraphfollowerscene.ZetsukoBanana.SaysBananaBreak".Localized());

            OutputText("Scenes.Ceraphfollowerscene.ZetsukoBanana.CursesLoudly".Localized());

            OutputText("Scenes.Ceraphfollowerscene.ZetsukoBanana.CollarStudsGlow".Localized());

            OutputText("Scenes.Ceraphfollowerscene.ZetsukoBanana.AvoidsSayingFruit".Localized());

            OutputText("Scenes.Ceraphfollowerscene.ZetsukoBanana.BananaIsAFruit".Localized());

            OutputText("Scenes.Ceraphfollowerscene.ZetsukoBanana.CursesLoudlyAgain".Localized());

            OutputText("Scenes.Ceraphfollowerscene.ZetsukoBanana.TakesAsConfirmation".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void NoZetsukoLoveToday()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.NoZetsukoLoveToday.DismissesZetsuko".Localized());

            OutputText("Scenes.Ceraphfollowerscene.NoZetsukoLoveToday.WarnsOfFury".Localized());

            Go<CampScene>();
        }

        public void GiveFollowerBodyBits()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.GiveFollowerBodyBits.AskBodyPartToll".Localized());

            if (Player.HasCock() || Player.HasVagina() || Player.HasBreasts())
            {
                if (Player.HasCock())
                    OutputText("Scenes.Ceraphfollowerscene.GiveFollowerBodyBits.OfferTakeCock".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Ceraphfollowerscene.GiveFollowerBodyBits.OfferRemoveSnatch".Localized());

                if (Player.HasBreasts())
                    OutputText("Scenes.Ceraphfollowerscene.GiveFollowerBodyBits.OfferTakeTits".Localized());
            }
            else
                OutputText("Scenes.Ceraphfollowerscene.GiveFollowerBodyBits.NothingWorthTaking".Localized());

            OutputText("Scenes.Ceraphfollowerscene.GiveFollowerBodyBits.SayEnd".Localized());

            ClearButtons();

            if ((int)Player.Gender > 0 || Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Ceraphfollowerscene.GiveFollowerBodyBits.ScratchHeadAtPitch".Localized());

                OutputText("Scenes.Ceraphfollowerscene.GiveFollowerBodyBits.ConsiderThePitch".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Penis".Localized(), null);

                AddButton(1, "Common.Button.SmallestPenis".Localized(), null);

                AddButton(2, "Common.Button.Vagina".Localized(), null);

                AddButton(3, "Common.Button.Breasts".Localized(), null);

                if (Player.HasCock())
                {
                    OutputText("Scenes.Ceraphfollowerscene.GiveFollowerBodyBits.OfferYourPenis".Localized());

                    AddButton(0, "Common.Button.Penis".Localized(), () => CeraphFollowerCockTaking());

                    if (Player.CockTotal() > 1)
                        AddButton(1, "Common.Button.SmallestPenis".Localized(), () => CeraphFollowerCockTaking(true));
                }

                if (Player.HasVagina())
                {
                    OutputText("Scenes.Ceraphfollowerscene.GiveFollowerBodyBits.VaginaOrgyPitch".Localized());

                    AddButton(2, "Common.Button.Vagina".Localized(), CeraphFollowerCuntTaking);
                }

                if (Player.BiggestBreastSize() >= 1)
                {
                    OutputText("Scenes.Ceraphfollowerscene.GiveFollowerBodyBits.NeedBoobsQuestion".Localized());

                    AddButton(3, "Common.Button.Breasts".Localized(), () => CeraphFollowerTitTaking());

                    if (Player.BreastRows.Count > 1)
                    {
                        var i = 1;

                        while (i < Player.BreastRows.Count && i < 10)
                        {
                            if (Player.BreastRows[i].BreastRating >= 1)
                                AddButton(3 + i, "Common.Text.Breastsrow".Localized() + i, () => CeraphFollowerTitTaking(i));

                            i += 1;
                        }
                    }
                }

                AddButton(14, "Common.Button.Back".Localized(), () => CeraphFollowerAppearance());
            }
            else
                DoNext(() => CeraphFollowerAppearance());
        }

        public void CeraphFollowerCockTaking(bool smallest = false)
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            if (smallest)
                x = Player.SmallestCockIndex();

            var y = x + 1;

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerCockTaking.SighAndOfferCock".Localized(
                y
            ));

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerCockTaking.HotFingersOnCock".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Thickness >= 4)
                OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerCockTaking.CompressGirthShaft".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerCockTaking.ShaftClosingWarm".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0 && Player.CockTotal() == 1)
            {
                OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerCockTaking.AndBallsSuffix".Localized());

                Player.Balls = 0;

                Player.BallSize = 1;
            }

            if (Player.CockTotal() == 1)
                Player.RemoveStatusEffect(StatusEffectIds.Infested);

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerCockTaking.HeldCockArcaneMark".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerCockTaking.FingerUpTheLength".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerCockTaking.PirouetteWithCock".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 5f));

            CoC.Instance.Flags.Set(GameFlagIds.CeraphDicksOwned, CoC.Instance.Flags.Get(GameFlagIds.CeraphDicksOwned) + 1);

            Player.RemoveCock(x, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void CeraphFollowerCuntTaking()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerCuntTaking.UndressOfferVagina".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerCuntTaking.FingernailsStabSkin".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerCuntTaking.HeldPussyFingered".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerCuntTaking.FeelingsWillFade".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerCuntTaking.WorkJawStayUpright".Localized());

            Player.RemoveVagina(0, 1);

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 5f));

            CoC.Instance.Flags.Set(GameFlagIds.CeraphPussiesOwned, CoC.Instance.Flags.Get(GameFlagIds.CeraphPussiesOwned) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void CeraphFollowerTitTaking(int rowNum = 0)
        {
            ClearOutput();

            int x = rowNum;

            if (Player.BreastRows[x].Fuckable)
                CoC.Instance.Flags.Set(GameFlagIds.CeraphGenitalStealChoice, 4);
            else
                if (Player.LactationQ() >= 100)
                    CoC.Instance.Flags.Set(GameFlagIds.CeraphGenitalStealChoice, 5);
                else
                    CoC.Instance.Flags.Set(GameFlagIds.CeraphGenitalStealChoice, 3);

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.BareBosomOffer".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.BounceTitsInHands".Localized(
                Player.BreastDescript(x)
            ));

            if (x < Player.BreastRows.Count - 1 && Player.BreastRows.Count > 1)
            {
                if (x >= Player.BreastRows.Count - 2)
                    OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.TorsoShiftTitsUp".Localized(
                        Player.BreastDescript(x + 1)
                    ));
                else
                    OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.TorsoShiftOtherTits".Localized());

                Player.RemoveBreastRow(x, 1);
            }
            else
                if (x == 0)
                {
                    OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.FlatChestGainsNipples".Localized());

                    Player.BreastRows[0].BreastRating = 0;

                    Player.NippleLength = .25f;

                    Player.BreastRows[0].Breasts = 2;

                    Player.BreastRows[0].NipplesPerBreast = 1;

                    Player.BreastRows[0].Fuckable = false;

                    Player.BreastRows[0].Lactation = 0;
                }
                else
                {
                    OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.TorsoLeavesFlatVoid".Localized());

                    Player.RemoveBreastRow(x, 1);
                }

            if (Player.BreastRows.Count == 0)
                OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.SaveErrorWarning".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.ParagraphBreak".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.GaspAtCeraphPrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphGenitalStealChoice) == 3)
                OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.RollNipplesHands".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphGenitalStealChoice) == 4)
                    OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.PushFingersLeakyNipple".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.SqueezeMilkSquirt".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.WeakInKneesToken".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.TauntCockInTits".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphFollowerTitTaking.PranceAwayTweaking".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f), new DynStat(GameStats.Corruption, 5f));

            CoC.Instance.Flags.Set(GameFlagIds.CeraphTitsOwned, CoC.Instance.Flags.Get(GameFlagIds.CeraphTitsOwned) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void StuffSomeNippleCunts()
        {
            ClearOutput();

            CeraphSprite(true);

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.ExploitExtremeChanges".Localized());

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.IncredulousStareAtChest".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.ProudDemonDickToken".Localized());

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.GrabChestExamine".Localized());

            if (Player.TotalNipples() > 2)
                OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.TwoOfPrefix".Localized());

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.TitPussiesPlural".Localized());

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.DigitsEnterKnuckleDeep".Localized());

            if (Player.TotalNipples() > 2)
                OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.ExploreMyriadHoles".Localized());

            if (Player.LactationQ() > 0)
                OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.MilkSquirtInEye".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.CockSmearsBelly".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.VaginaWetEnough".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.NeglectedCockDribbles".Localized());

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.SatisfiedInspection".Localized());

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.DistasteAtShifting".Localized());

            if (Player.BreastRows.Count > 1)
            {
                OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.SecondLaterPrefix".Localized());

                if (Player.BreastRows.Count == 2)
                    OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.AnotherRowBegins".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.MoreRowsBegin".Localized());

                OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.SproutBeneathTopPair".Localized());
            }

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.AreolaLookUdder".Localized());

            if (Player.AverageNipplesPerBreast() > 1)
                OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.StrokeIntoPenises".Localized());

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.GapeAtNewForm".Localized());

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.EndowedMinionStepsUp".Localized(
                NumberUtils.Num2Text(Player.TotalNipples())
            ));

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.DemonGivesInInstinct".Localized());

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.ChewLowerLipRiding".Localized());

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.BiteLipCommandCum".Localized(
                NumberUtils.Num2Text(Player.TotalNipples())
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.HerCockSpoutsIvory".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.HerPussyDripsWeak".Localized());

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.HandsThroughHairHold".Localized());

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.DuelingClimaxesFade".Localized(
                NumberUtils.Num2Text(Player.TotalNipples())
            ));

            OutputText("Scenes.Ceraphfollowerscene.StuffSomeNippleCunts.CoosWingsUnfurl".Localized());

            Player.Orgasm("Nipples");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void PortalFuckWithFollowerCeraph()
        {
            ClearOutput();

            CeraphSprite(true);

            var x = Player.CockThatFits(100);

            var y = x + 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesCeraphPortalFucked) == 0)
            {
                OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.AskCeraphForIdeas".Localized());

                OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.KnowingSmileNoAnswer".Localized());

                OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.SkepticalAtRings".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 1)
                    OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.RingUnusedToday".Localized());

                OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.SayEnd".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 1)
                    OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.TwirlRingToPalm".Localized());

                OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.MauveTossesRing".Localized());
            }
            else
            {
                OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.SuggestCockRings".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 1)
                    OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.SadlyTeleportsRing".Localized());

                OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.MauveTossesYourRing".Localized());
            }

            OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.ObeyLaySpreadThighs".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.EagerCockToken".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.ScootAssToChair".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.MomentOfTruthPush".Localized(
                y,

                y
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesCeraphPortalFucked) == 0)
                OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.NoVisibleChange".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.ImaginesSpearingHer".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.WhiteLightTingles".Localized(
                y,

                y
            ));

            OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.SlideRingExperiment".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.PetForcesOwnRing".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.SlideRingUpDown".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.SurgingPassionRelease".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.GurglesThenSwallows".Localized());

            OutputText("Scenes.Ceraphfollowerscene.PortalFuckWithFollowerCeraph.StandAndStretch".Localized(
                y
            ));

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            AwardAchievement(AchievementIds.FuckWithPortals);

            CoC.Instance.Flags.Set(GameFlagIds.TimesCeraphPortalFucked, CoC.Instance.Flags.Get(GameFlagIds.TimesCeraphPortalFucked) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void LayEggsInSlaveCeraph()
        {
            ClearOutput();

            CeraphSprite(true);

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.YouPrefix".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.RubSensitiveBulge".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.RegardSlaveDemand".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.CeraphGlancesBulge".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.CeraphFrownsCrotch".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.CommandTurnAround".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.GarmentCreaksStrains".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.FrownEggDumpScold".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.CockSpillsOut".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.SentencePeriod".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.CeraphCriesJerking".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.RollEyesDropHer".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.OvipositorSlipsFree".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.FingersTrembleTracing".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.OrderSlutPresentPussy".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.SurelyDoBetter".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.BetterDemandWider".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.VaginaCervixGlow".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.PositionBehindSlave".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.EggsTravelToTip".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.FirstEggSpurt".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.OrgasmSpattersRear".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.EggsFollowFirst".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.EmptyAfterEternity".Localized());

            OutputText("Scenes.Ceraphfollowerscene.LayEggsInSlaveCeraph.HummingHappyTune".Localized());

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            DoNext(() => ReturnToCamp(1));
        }

        public void SumissivenessToCeraphFollower()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.BroachSubmissionIdea".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.BetterPlaceInDirt".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.NodMeeklyAwed".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.BendOverCommand".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.StandWhipLash".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.ScowlPetsRollOver".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.MewlUnderBlows".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.BothHardnessManhood".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.SLetter".Localized());

                OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.MoistureLeakingUnder".Localized());
            }
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.BeadsMoistureForming".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.HardnessEasilyVisible".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.SentencePeriod".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.CeraphSmirksAsks".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.PurpleDomCoos".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.OneCockToken".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.RigidClit".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.TicklishSoftTouches".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.GentlySqueezeSack".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.SavorStateGroan".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.PreCumForming".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.GirlCumForming".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.CrystalDropletDangles".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.CeraphWondersDripping".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.WordChokedOff".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.HumiliatingMilking".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.SumissivenessToCeraphFollower.HumiliatingPegging".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

            ClearButtons();

            AddButton(0, "Common.Button.MewlOnce".Localized(), () => BarkToCeraphOnce(false));

            AddButton(1, "Common.Button.BarkOnce".Localized(), () => BarkToCeraphOnce(true));

            AddButton(2, "Common.Button.MeowTwice".Localized(), () => BarkOrMeowTwiceToCeraph(false));

            AddButton(3, "Common.Button.BarkTwice".Localized(), () => BarkOrMeowTwiceToCeraph(true));

            AddButton(4, "Common.Button.Apple".Localized(), SayAppleToCeraph);
        }

        public void SayAppleToCeraph()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.SayAppleToCeraph.AppleSafeWord".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SayAppleToCeraph.CeraphShockedUp".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SayAppleToCeraph.SayNotFunLeave".Localized());

            OutputText("Scenes.Ceraphfollowerscene.SayAppleToCeraph.GrumblesAccepting".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void BarkToCeraphOnce(bool dog = true)
        {
            ClearOutput();

            if (dog)
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.BarkOnceLine".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.MewOnceLine".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.LoudAlmostProudly".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.WaggingTailExcitedly".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.SentencePeriod".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.PantiesPulledLower".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphHidingDick) == 0)
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.EaseTightPhallusPressure".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.AllowPenisToGrow".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.CockStalkDrips".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.ParagraphBreak".Localized());

            if (dog)
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.DoubleBarkSay".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.LoudMewlSay".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.YouWord".Localized());

            if (dog)
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.BarkWord".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.MeowWord".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.CrawlAwayInDistress".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.Milked".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.Fucked".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.WhipSnapsTaut".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.SwatHandClaps".Localized());

            if (dog)
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.Doggie".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.KittyCat".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.HipsPressHotSpear".Localized());

            if (Player.Anal.Looseness == 0)
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.VirginRectumEntry".Localized());
            else
                if (Player.AnalCapacity() < 20)
                    OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.TightRectumEntry".Localized());
                else
                    if (Player.Anal.Looseness >= 4)
                        OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.UsedAssholeEntry".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.CapacitiveAnusSlide".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.GroanAsTaken".Localized());

            Player.ButtChange(16, true, true, false);

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.GlansRingBumps".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.TwitchingMaleness".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.AchingEmptyPussy".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.EsSuffix".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.SquirtOutDollop".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.PreCum".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.LadySpunk".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.DanglesHeavily".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.MakeCockBobBelow".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.TantalizeTeaseClit".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.AnotherPleasureGrunt".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.CeraphHandsOnWaist".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.InnerCockReserveRam".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.QuiveringVaginalWalls".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.BrainFireworks".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.ParagraphBreakAgain".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.DickMilk".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.LoveJuice".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.DrizzlesFrom".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.EachCockToken".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.SlightlyPartedTwat".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.PoolOnFloor".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.FemPrefix".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.CumIsJust".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.Sliding".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.Pouring".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.SlidesOutSmoothly".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.InnerReservoirSqueezed".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.CuntWallsStimulated".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.PushDicktipSlips".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.Prostate".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.Walls".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.PeriodSpace".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.CockLosingStiffness".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.FemcumLeaksShivers".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.SentencePeriodAgain".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.MostAmazinglyThough".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.QuotedPressure".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.NeedWithinYou".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.ReleasedStillHorny".Localized());

            if (Player.HasCock())
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.EvenWithSoftCock".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.EvenWithSoftCocks".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.CeraphPurrs".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.RapidPenisPumping".Localized());

            if (dog)
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.CumSlutDoggyBark".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.CockLovingKittyMewl".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.PushBackAgainst".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.Semen".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.Juices".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.FromYourPrefix".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.LimpCock".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.SLetter".Localized());
            }
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.Snatch".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.TinglingEcstaticTime".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.ProstatePushedAgainst".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.EmptyCuntWallsTeased".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.SentencePeriodThird".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.LoudMoanRamsHard".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.ProstateIs".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.VaginalWallsAre".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.WarmthExplodesInside".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.OntoSoreProstate".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.SensitiveWallPressure".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.ScrunchEyesClosed".Localized());

            if (Player.HasCock())
            {
                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.EachOfYourCocks".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.YourCock".Localized());

                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.LastStickyPetCum".Localized());
            }
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.PussyLastSecretions".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.SentencePeriodThirdAgain".Localized());

            if (Player.CumQ() >= 700)
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.FloodsOutEverywhere".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.MistressPullsOut".Localized());

            if (dog)
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.BoneShapedBiscuit".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.FishySmellingTreat".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.GobbleDownReward".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkToCeraphOnce.FadingIntoWind".Localized());

            Player.SlimeFeed();

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void BarkOrMeowTwiceToCeraph(bool dog = true)
        {
            ClearOutput();

            if (dog)
                OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.DoubleBarkLine".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.MeowTwicePlead".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.WigglingYour".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Tail".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ButtToken".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.HappilyDeniedRelease".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ImprovisedCollar".Localized());

            if (dog)
                OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Whining".Localized());
            else
                OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Mewling".Localized());

            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.LikeNeedyAnimal".Localized());

            var choices = new List<int>();
            // 原版 CeraphFollowerScene.hx:2536 — Holli 是跟随者
            if (SceneInstance<HolliScene>.Get().HolliFollower())
                choices.Add(0);
            // 原版 CeraphFollowerScene.hx:2539 — Amily 是腐化跟随者
            if (SceneInstance<AmilyScene>.Get().AmilyFollowerCorrupt())
                choices.Add(1);
            else

                if (CoC.Instance.Flags.Get(GameFlagIds.HolliSubmissive) == 0)
                    choices.Add(1); // 原版 CeraphFollower.hx — Holli 尚未屈服

            choices.Add(2);

            int select = choices[Random.Shared.Next(choices.Count)];

            if (select == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.HolliSubmissive) == 0)
                {
                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.CorruptedDryadPeeps".Localized());

                    if (dog)
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.DoggieThird".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.KittyCat".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.MotherMaraeComplaint".Localized());
                }
                else
                {
                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.CorruptedDryadQuestions".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.CeraphSmilesExpectantly".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ParagraphBreak".Localized());

                    if (dog)
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.BarkQuestion".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.MeowQuestion".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.HolliCrudePetting".Localized());

                    if (dog)
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.DoggieAgain".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.KittyAgain".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ThrilledSheSays".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.HolliSubmissive, 0);
                }

                OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.CeraphWalkContinues".Localized());

                if (Player.HasCock())
                {
                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.OneOf".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.RebelliousBoner".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.PluralSuffix".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.UnintendedSideEffect".Localized());
                }
                else
                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.RebelliousClitoris".Localized());

                OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.PeriodSpace".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.BouncesAgainstTummy".Localized());
                else
                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.VaginalMusclesClench".Localized());

                OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.UnfilledNeed".Localized());
            }
            else
                if (select == 1)
                {
                    // 原版 CeraphFollowerScene.hx:2623 — CeraphFollower.hx：腐化 Amily 与腐化 Jojo 同在营地
                    if (SceneInstance<AmilyScene>.Get().AmilyFollowerCorrupt() && SceneInstance<JojoScene>.Get().CampCorruptJojo())
                    {
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ComeAcrossBothSlaves".Localized((noFur ? "Common.Text.Eared".Localized() : "Scenes.Urtascene.TellUrtaToStayTheSame.LikeWord".Localized())));

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.CeraphHoldsLeash".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ParagraphBreakAgain".Localized());

                        if (dog)
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.BarkUtterance".Localized());
                        else
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.MeowUtterance".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.AmilySpeaksForBoth".Localized());
                    }
                    else
                    {
                        var jojo = false;

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.MouseSlaveSeesYou".Localized((noFur ? "Common.Text.Eared".Localized() : "Scenes.Urtascene.TellUrtaToStayTheSame.LikeWord".Localized())));

                        if (jojo)
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.HePronoun".Localized());
                        else
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ShePronoun".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.SlaveAsksMaster".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.CeraphHoldsLeashAgain".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ParagraphBreakThird".Localized());

                        if (dog)
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.BarkUtteranceAgain".Localized());
                        else
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.MeowUtteranceAgain".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.AmilyBeginsPetsToo".Localized());

                        if (!jojo)
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.CumSlutIs".Localized());
                        else
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.IGuessImPet".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.HerPetToo".Localized());

                        if (!jojo)
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.AmilyThird".Localized());
                        else
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.JojoThird".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.SaysDroppingAllFours".Localized());

                        if (!jojo)
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.AmilyAgain".Localized());
                        else
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.JojoAgain".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.RubAgainstEachOther".Localized());

                        if (!jojo)
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Sister".Localized());
                        else
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Brother".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.PetComfortingExposure".Localized());

                        if (jojo)
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.HisPossessive".Localized());
                        else
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.HerPossessive".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.SexualScentDistracting".Localized());
                    }

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ExhaustingWalk".Localized());
                    // 原版 CeraphFollowerScene.hx:2684 — 腐化 Amily 与腐化 Jojo 同在
                    if (SceneInstance<AmilyScene>.Get().AmilyFollowerCorrupt() && SceneInstance<JojoScene>.Get().CampCorruptJojo())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.PluralSuffixAgain".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.DegradingEnjoyment".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.EndlessFeedbackLoop".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ParagraphBreakFourth".Localized());

                    if (SceneInstance<JojoScene>.Get().CampCorruptJojo())
                    {
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.JojoThird".Localized());
                        // 原版 CeraphFollowerScene.hx:2692 — 腐化 Amily 与腐化 Jojo 同在时才加「and」
                        if (SceneInstance<AmilyScene>.Get().AmilyFollowerCorrupt() && SceneInstance<JojoScene>.Get().CampCorruptJojo())
                            OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.AndConjunction".Localized());
                    }

                    if (SceneInstance<AmilyScene>.Get().AmilyFollowerCorrupt())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.AmilyThird".Localized());

                    if (SceneInstance<HolliScene>.Get().HolliFollower())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.LookAroundDartBack".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.LooksAroundDartBack".Localized());
                }
                else
                {
                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.CeraphLeadsIntoWasteland".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.GoblinsAgain".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ImpsAgain".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.IntriguedBySight".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ErectMaleness".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.QuiveringQuim".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.EsSuffix".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.GemsBountyOffer".Localized());

                    if (dog)
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.DoggieThird".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.KittyAgain".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.GetAllNeedObedience".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.CockAgain".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Cunt".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.HoldStillQuaking".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.CockAgain".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Nethers".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.PluralSuffixThird".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.SentencePeriod".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.OneOfThe".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.GoblinsAgain".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ImpsAgain".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.NoticesStateComments".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.HerPossessiveAgain".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.HisPossessiveAgain".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.HandBeneathDrooling".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Erection".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Pussy".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.PluralSuffixFourth".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.CollectPalmfulSlippery".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Boy".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Girl".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Honey".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.PronounShe".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.PronounHe".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.HoldsUpInFront".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.PronounHer".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.PronounHis".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.SniffSighTamedRemark".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.MilkingHimDry".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.FuckEveryChance".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.MaybeChangeMind".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.ShakesHeadSmiles".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.PreCumRockets".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.FemJuiceSpurts".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.WhineGroundCommand".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Goblin".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Imp".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.HomeMessLickUp".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.LickUpLeavings".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.PreEjaculate".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.LadyJizz".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.GoodEnoughComeOn".Localized());

                    if (dog)
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Doggy".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Cat".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.FinishOurWalk".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.DragsCollarHome".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Boner".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Twat".Localized());

                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.LetterS".Localized());

                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.FeelLikeThey".Localized());
                    }
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.FeelsLikeIt".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.GoOffAnySecond".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.OverInflatedMaleness".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.PuffyAchingCleft".Localized());

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.LetterEs".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.MakeWholeWayHome".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Popping".Localized());
                    else
                        OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.TippingOverEdge".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.TrailOfMuskySlime".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Click".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.CollarRemovedAlone".Localized());
                }

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f), new DynStat(GameStats.Lust, 100f));

            DoNext(() => ReturnToCamp(1));
        }

        public void IdLiddellium()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.IdLiddellium.FoundOddPotion".Localized());

            OutputText("Scenes.Ceraphfollowerscene.IdLiddellium.SlaveExaminesPotion".Localized());

            OutputText("Scenes.Ceraphfollowerscene.IdLiddellium.ChuckleHaremChaos".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LiddelliumFlag, -1);

            DoNext(() => CeraphFollowerAppearance());
        }

        public void GetLiddellium()
        {
            ClearOutput();

            var got1 = false;

            if (CoC.Instance.Flags.Get(GameFlagIds.LiddelliumCeraphFlag) == 0)
            {
                OutputText("Scenes.Ceraphfollowerscene.GetLiddellium.LiddelliumHouseholdAsk".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.LiddelliumCeraphFlag, (7 * 24));

                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.Liddellium), 1);

CeraphFollowerAppearance();
            }
            else
            {
                var outText = "You tell Ceraph you're looking for more Liddellium.\n\n";

                got1 = false;

                if (CoC.Instance.Flags.Get(GameFlagIds.LiddelliumCeraphFlag) == 1)
                {
                    outText += "Ceraph pulls a bottle from behind her, mystifying you as to her non-Euclidean control of space.[pg][say: Hope you have fun using it, [Master].][pg]";

                    CoC.Instance.Flags.Set(GameFlagIds.LiddelliumCeraphFlag, (7 * 24));

                    got1 = true;
                }
                else
                    outText += "Ceraph apologizes and admits she doesn't have any Liddellium right now.";

                OutputText(outText);

                if (got1)
                {
                    Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.Liddellium), 1);

CeraphFollowerAppearance();
                }
            }

            if (!got1)
                DoNext(() => CeraphFollowerAppearance());
        }

        public void CeraphRapePlay()
        {
            ClearOutput();

            if (Save.GetInt("rapePlay", 0) != 0)
            {
                OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.HeatRisingRequest".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.EffortToBringMe".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.BarelyStartedAbuse".Localized());
            }
            else
            {
                OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.AsksToBeSub".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.FoolishMortal".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.StepCloserGaze".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.CagedBirdSpeech".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.StartsToWorry".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.WhispersBreedYou".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.StomachLurches".Localized());
            }

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.FaceFirstInDirt".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.HandOnThroat".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.FirmAssSqueeze".Localized());

            Player.CuntChange(20, true, true);

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.FuckmeatPraise".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.MemberSwells".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.DemonessRavages".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.SingleTearSlips".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.KnotDeepInside".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.OrgasmComingOn".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.NoticesTremblingInsides".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.PunctuatedDeepThrust".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.StrangeFeeling".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.EyesShootWide".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.LastBestialHowl".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.ClimaxLastsLong".Localized());

            Player.Orgasm("Vaginal");

            if (Save.GetInt("rapePlay", 0) == 0)
                DoNext(() => CeraphRapePlayChoice());
            else
                if (Save.GetInt("rapePlay", 0) == 1)
                {
                    OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.MinutesToSinkIn".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.AsksIfSatisfied".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.ResidualPleasure".Localized());

                    Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 32, 61);

                    DoNext(() => ReturnToCamp(1));
                }
                else
                {
                    OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.ScramblesOutAngry".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.AsksDemurely".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.FindsNothingThere".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.LadyHasTricks".Localized());

                    OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlay.EnjoyedSomeParts".Localized());

                    DoNext(() => ReturnToCamp(1));
                }
        }

        public void CeraphRapePlayChoice()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlayChoice.SlumpsBackSatisfied".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlayChoice.ExpressShock".Localized());

            ClearButtons();

            AddNextButton("Common.Button.LikedIt".Localized(), () => CeraphRapePlayChoice2(0));

            AddNextButton("Common.Button.SheDid".Localized(), () => CeraphRapePlayChoice2(1));

            AddNextButton("Common.Button.SheDidnt".Localized(), () => CeraphRapePlayChoice2(2));
        }

        public void CeraphRapePlayChoice2(int choice)
        {
            ClearOutput();

            switch (choice)
            {
                case 0:
                {
                OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlayChoice2.BlushAtPregnancy".Localized());

                Save.SetInt("rapePlay", 1);
                }

                break;

                case 1:
                {
                OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlayChoice2.NotVerySpecific".Localized());

                Save.SetInt("rapePlay", 2);
                }

                break;

                case 2:
                {
                OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlayChoice2.ShouldKnowBetter".Localized());

                Save.SetInt("rapePlay", 2);
                }

                break;
            }

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlayChoice2.TakesMomentToThink".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRapePlayChoice2.TakeYourLeave".Localized());

            Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 32, 61);

            DoNext(() => ReturnToCamp(1));
        }

        public void AskCeraphAboutNobleShirt()
        {
            ClearOutput();

            Undergarments.SEA_SHIRT.Save.SetInt("ceraphAsked", 1);

            RegisterMacro("shirtStage", Undergarments.SEA_SHIRT.Save.GetInt("armorStage", 0));

            OutputText("Scenes.Ceraphfollowerscene.AskCeraphAboutNobleShirt.PilferedCursedShirt".Localized());

            OutputText("Scenes.Ceraphfollowerscene.AskCeraphAboutNobleShirt.AskedForCurses".Localized());

            OutputText("Scenes.Ceraphfollowerscene.AskCeraphAboutNobleShirt.CutThroughTeasing".Localized());

            if (Undergarments.SEA_SHIRT.Save.GetInt("armorStage", 0) > 2)
            {
                OutputText("Scenes.Ceraphfollowerscene.AskCeraphAboutNobleShirt.ReleasesAndSighs".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            OutputText("Scenes.Ceraphfollowerscene.AskCeraphAboutNobleShirt.SharpNailRuffles".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.RemoveShirt".Localized(), CeraphRemoveNobleShirt);

            AddButton(1, "Common.Button.LeaveIt".Localized(), CeraphLeaveNobleShirt);
        }

        public void CeraphLeaveNobleShirt()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphfollowerscene.CeraphLeaveNobleShirt.ComeBackLater".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphLeaveNobleShirt.AsYouWish".Localized());

            Undergarments.SEA_SHIRT.Save.SetInt("ceraphLeftIt", 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void CeraphRemoveNobleShirt()
        {
            ClearOutput();

            if (Undergarments.SEA_SHIRT.Save.GetInt("armorStage", 0) > 2)
            {
                OutputText("Scenes.Ceraphfollowerscene.CeraphRemoveNobleShirt.StaresAndFurrows".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphRemoveNobleShirt.ShiverDownSpine".Localized());
            }
            else
            {
                OutputText("Scenes.Ceraphfollowerscene.CeraphRemoveNobleShirt.PushesYourSternum".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphRemoveNobleShirt.ClothSettles".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphRemoveNobleShirt.NotQuiteDelicate".Localized());

                OutputText("Scenes.Ceraphfollowerscene.CeraphRemoveNobleShirt.CursedShirtOff".Localized());

                Player.SetUndergarment(Undergarment.Nothing, Undergarment.TypeUpperwear);
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void CeraphDisenchantNobleShirt()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Ceraphfollowerscene.CeraphDisenchantNobleShirt.FoundFancyShirt".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphDisenchantNobleShirt.GrinsAndPonders".Localized());

            AddButton(0, "Common.Button.Binding".Localized(), CeraphRemoveBindingNobleShirt);

            AddButton(1, "Common.Button.Dispel".Localized(), CeraphDispelNobleShirt);
        }

        public void CeraphRemoveBindingNobleShirt()
        {

            OutputText("Scenes.Ceraphfollowerscene.CeraphRemoveBindingNobleShirt.BindingDownside".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphRemoveBindingNobleShirt.DevilishGrin".Localized());

            Undergarments.SEA_SHIRT.Save.SetInt("bound", 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void CeraphDispelNobleShirt()
        {

            OutputText("Scenes.Ceraphfollowerscene.CeraphDispelNobleShirt.KnowingLookConfirm".Localized());

            OutputText("Scenes.Ceraphfollowerscene.CeraphDispelNobleShirt.HarmlessNobleShirt".Localized());

            Undergarments.SEA_SHIRT.Save.SetInt("enchanted", 0);

            Undergarments.SEA_SHIRT.Save.SetInt("bound", 0);

            DoNext(() => ReturnToCamp(1));
        }

    }
}