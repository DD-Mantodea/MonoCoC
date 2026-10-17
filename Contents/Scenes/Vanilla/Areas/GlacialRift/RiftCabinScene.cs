using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.GlacialRift
{
    [Register<RiftCabinScene>("coc:rift_cabin")]
    public class RiftCabinScene : Scene
    {

        public bool encountered = false;

        public bool pictureBurned = false;

        public bool bloomersBurned = false;

        public bool tookBloomers = false;

        public string orgasmType = "";

        public bool loliVersion = false;

        public static SaveContent Save => SaveContent(SceneIds.GlacialRift.RiftCabin);

        public bool loli => Save.GetBool("loliVersion");

        public bool masturbated => Save.GetString("orgasmType") == "" ? false : true;

        public string debugName => "Rift Cabin";

        public string debugHint => "The abandoned cabin in the glacial rift.";

        public override void Entry()
        {
            ExecEncounter();
        }

        public void Reset()
        {

            Save.SetBool("encountered", false);

            Save.SetBool("pictureBurned", false);

            Save.SetBool("bloomersBurned", false);

            Save.SetBool("tookBloomers", false);

            Save.SetString("orgasmType", "");

            Save.SetBool("loliVersion", false);
        }

        public void ExecEncounter()
        {

            FindCabin();
        }

        public void FindCabin()
        {
            ClearOutput();

            Reset();

            Save.SetBool("loliVersion", OptionStore.Instance.Get("genericLoliShota", 0) > 0 && false);

            Save.SetBool("encountered", true);

            OutputText("Scenes.Riftcabinscene.FindCabin.TrudgesSnow".Localized());

            OutputText("Scenes.Riftcabinscene.FindCabin.WanderedOnto".Localized());

            OutputText("Scenes.Riftcabinscene.FindCabin.FightsWay".Localized());

            OutputText("Scenes.Riftcabinscene.FindCabin.LimbsFeelLike".Localized());

            OutputText("Scenes.Riftcabinscene.FindCabin.JustAsSmall".Localized());

            OutputText("Scenes.Riftcabinscene.FindCabin.SparseFurniture".Localized());

            OutputText("Scenes.Riftcabinscene.FindCabin.PreviousInhabitant".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), SearchDrawer);

            AddButton(1, "Common.Button.No".Localized(), RespectPrivacy);

            AddButton(2, "Common.Button.BurnIt".Localized(), BurnPicture);
        }

        public void LeaveCabin(bool rested = false)
        {

            var restBonus = 0;

            var timeSpent = 1;

            if (Save.GetBool("bloomersBurned"))
            {
                restBonus += 50;

                Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));
            }

            if (Save.GetBool("pictureBurned"))
                restBonus += 100;

            if (masturbated)
            {
                timeSpent += 1;

                Player.Orgasm(Save.GetString("orgasmType"));
            }

            if (rested)
            {
                timeSpent += 2;

                OutputText("Scenes.Riftcabinscene.LeaveCabin.ParagraphBreakMark".Localized());

                CampScene.SleepRecoveryApply(timeSpent, 0, true, restBonus);
            }

            if (Save.GetBool("tookBloomers"))
                InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.Bloomers), () => ReturnToCamp(timeSpent));
            else
                ReturnToCamp(timeSpent);
        }

        public void BurnPicture()
        {
            ClearOutput();

            Save.SetBool("pictureBurned", true);

            OutputText("Scenes.Riftcabinscene.BurnPicture.OpensStove".Localized());

            if (silly)
                OutputText("Scenes.Riftcabinscene.BurnPicture.DidntBurnDrawer".Localized());
            else
                OutputText("Scenes.Riftcabinscene.BurnPicture.DrawerStillThere".Localized());

            OutputText("Scenes.Riftcabinscene.BurnPicture.WantToOpenQuestion".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), SearchDrawer);

            AddButton(1, "Common.Button.No".Localized(), RespectPrivacy);
        }

        public void RespectPrivacy()
        {
            ClearOutput();

            if (Save.GetBool("pictureBurned"))
                OutputText("Scenes.Riftcabinscene.RespectPrivacy.GlancingDrawer".Localized());
            else
                OutputText("Scenes.Riftcabinscene.RespectPrivacy.SetsPictureUpright".Localized());

            OutputText("Scenes.Riftcabinscene.RespectPrivacy.NoInterestDigging".Localized());

            OutputText("Scenes.Riftcabinscene.RespectPrivacy.DontHaveTime".Localized());

            OutputText("Scenes.Riftcabinscene.RespectPrivacy.AwakensFeeling".Localized());

            LeaveCabin(true);
        }

        public void SearchDrawer()
        {
            ClearOutput();

            if (Save.GetBool("pictureBurned"))
                OutputText("Scenes.Riftcabinscene.SearchDrawer.WalksOverDrawer".Localized());
            else
                OutputText("Scenes.Riftcabinscene.SearchDrawer.SetsPictureUpright".Localized());

            OutputText("Scenes.Riftcabinscene.SearchDrawer.SeemsStuck".Localized());

            if (loli)
            {
                OutputText("Scenes.Riftcabinscene.SearchDrawer.WhatGreetsYou".Localized());

                OutputText("Scenes.Riftcabinscene.SearchDrawer.ContinueSearch".Localized());

                OutputText("Scenes.Riftcabinscene.SearchDrawer.WhatDoWant".Localized());
            }
            else
            {
                OutputText("Scenes.Riftcabinscene.SearchDrawer.AreGreetedBy".Localized());

                OutputText("Scenes.Riftcabinscene.SearchDrawer.SoWhatDoWant".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Take".Localized(), TakeBloomers);

            AddButton(1, "Common.Button.LeaveThem".Localized(), LeaveBloomers);

            AddButton(2, "Common.Button.BurnThem".Localized(), BurnBloomers);

            AddButton(3, "Common.Button.Pantifap".Localized(), FapBloomers);

            AddButton(4, "Common.Button.Pantischlick".Localized(), SchlickBloomers);
        }

        public void TakeBloomers()
        {
            ClearOutput();

            Save.SetBool("tookBloomers", true);

            OutputText("Scenes.Riftcabinscene.TakeBloomers.PocketingDead".Localized((loli ? "girl's" : "mother's"), (Player.IsCorruptEnough(20) ? "" : " Or would normally even consider.")));

            ClearButtons();

            AddButton(0, "Scenes.Camp.Rest".Localized(), Rest);

            AddButton(1, "Common.Button.Leave".Localized(), Leave);
        }

        public void Rest()
        {
            ClearOutput();

            OutputText("Scenes.Riftcabinscene.Rest.StillTired".Localized());

            OutputText("Scenes.Riftcabinscene.Rest.AwakensFeeling".Localized());

            LeaveCabin(true);
        }

        public void Leave()
        {
            ClearOutput();

            OutputText("Scenes.Riftcabinscene.Leave.DidntGetRest".Localized());

            OutputText("Scenes.Riftcabinscene.Leave.BloomersInPocket".Localized());

            LeaveCabin(false);
        }

        public void LeaveBloomers()
        {
            ClearOutput();

            OutputText("Scenes.Riftcabinscene.LeaveBloomers.DiggingDead".Localized((loli ? "girl's" : "woman's")));

            OutputText("Scenes.Riftcabinscene.LeaveBloomers.WakeUpSomeTime".Localized());

            LeaveCabin(true);
        }

        public void BurnBloomers()
        {
            ClearOutput();

            Save.SetBool("bloomersBurned", true);

            OutputText("Scenes.Riftcabinscene.BurnBloomers.WalksOverStove".Localized((silly ? " Maybe you should try setting yourself on fire some time." : "")));

            OutputText("Scenes.Riftcabinscene.BurnBloomers.WakeUpSomeTime".Localized());

            LeaveCabin(true);
        }

        public void FapBloomers()
        {
            ClearOutput();

            Save.SetBool("tookBloomers", true);

            Save.SetString("orgasmType", "Dick");

            OutputText("Scenes.Riftcabinscene.FapBloomers.IdeaComes".Localized());

            OutputText("Scenes.Riftcabinscene.FapBloomers.ImaginesAbove".Localized((loli ? " loli" : ""), (loli ? "sweet aroma of a child" : "musky aroma of a mother")));

            OutputText("Scenes.Riftcabinscene.FapBloomers.ShovesBloomers".Localized((loli ? " small" : ""), (loli ? "barely developed" : "voluptuous")));

            if (loli)
            {
                OutputText("Scenes.Riftcabinscene.FapBloomers.GivesTimeRest".Localized(
                    Player.IsPureEnough(50) ? "some" : "little",

                     Player.Cocks[Player.SmallestCockIndex()].Length <= 5 ? "gives it a few experimental strokes. Her tiny hands make even your modest size look big." : (Player.Cocks[Player.SmallestCockIndex()].Length <= 10 ? "pokes your impressive member before giving it a few inexperienced strokes." : "simply stares in awe at your length. You hear her gulp and you reach to stroke the back of her head and tell her she'll be fine. She nods, somewhat unsure, and wraps her tiny hands around your [cock smallest], stroking it slowly up and down.")
                ));

                OutputText("Scenes.Riftcabinscene.FapBloomers.AlmostPushes".Localized());

                OutputText("Scenes.Riftcabinscene.FapBloomers.MouthDivine".Localized(
                    Player.Cocks[Player.SmallestCockIndex()].Length <= 6 ? "reaches your base. She blinks and looks up to you, her eyes sparkling in triumph." : "reaches her limit a good six inches down your [cock smallest]. She looks up at you, her eyes glossy as she does her best not to gag."
                ));

                OutputText("Scenes.Riftcabinscene.FapBloomers.ImaginesLittleGirl".Localized((Player.IsPureEnough(50) ? "gently" : "roughly")));

                OutputText("Scenes.Riftcabinscene.FapBloomers.DickSmearing".Localized(
                    Player.Cocks[Player.SmallestCockIndex()].Length >= 12 ? " Though she steals worried glances down at your impressive length." : ""
                ));

                if (Player.IsPureEnough(75))
                {
                    OutputText("Scenes.Riftcabinscene.FapBloomers.ReassuresHer".Localized((Player.IsPureEnough(50) ? " gently" : "")));

                    OutputText("Scenes.Riftcabinscene.FapBloomers.BreathingRapid".Localized(
                        Player.Cocks[Player.SmallestCockIndex()].Length >= 10 ? "She is far more spacious than she looks. You can see the outline of your dick deforming her stomach, the mental image making you almost nut, but you calm yourself at the last moment." : ""
                    ));

                    OutputText("Scenes.Riftcabinscene.FapBloomers.FeelsConvulsing".Localized((silly ? "like a little girl." : "wantonly.")));

                    OutputText("Scenes.Riftcabinscene.FapBloomers.UnableHold".Localized());

                    OutputText("Scenes.Riftcabinscene.FapBloomers.CollapseOnTop".Localized());

                    OutputText("Scenes.Riftcabinscene.FapBloomers.HangsBloomers".Localized());
                }
                else
                {
                    OutputText("Scenes.Riftcabinscene.FapBloomers.GrabbingHips".Localized(
                        Player.Cocks[Player.SmallestCockIndex()].Length >= 10 ? ", marveling at the sight of your dick distending her belly, the outline perfectly visibly through her small form" : ""
                    ));

                    OutputText("Scenes.Riftcabinscene.FapBloomers.RedMarks".Localized((silly ? " Lolis just look great in that freshly-raped look." : "")));

                    OutputText("Scenes.Riftcabinscene.FapBloomers.TiltsNeck".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Riftcabinscene.FapBloomers.RecoversFast".Localized((Player.Cocks[0].Length > 14 ? " with surprising ease" : "")));

                OutputText("Scenes.Riftcabinscene.FapBloomers.GripsTight".Localized());

                OutputText("Scenes.Riftcabinscene.FapBloomers.CollapsesOnYou".Localized());

                if (Player.IsPureEnough(75))
                {
                    OutputText("Scenes.Riftcabinscene.FapBloomers.SpendsEntangled".Localized());

                    OutputText("Scenes.Riftcabinscene.FapBloomers.HandTravels".Localized());

                    OutputText("Scenes.Riftcabinscene.FapBloomers.GivesLittleTime".Localized((Player.IsPureEnough(50) ? " a" : "")));

                    OutputText("Scenes.Riftcabinscene.FapBloomers.UtterlySpent".Localized());
                }
                else
                {
                    OutputText("Scenes.Riftcabinscene.FapBloomers.MayBeDone".Localized());

                    OutputText("Scenes.Riftcabinscene.FapBloomers.TitsVictims".Localized());

                    OutputText("Scenes.Riftcabinscene.FapBloomers.HandLetsGo".Localized());

                    OutputText("Scenes.Riftcabinscene.FapBloomers.ClosesEyes".Localized());

                    OutputText("Scenes.Riftcabinscene.FapBloomers.TossesBloomers".Localized());
                }
            }

            OutputText("Scenes.Riftcabinscene.FapBloomers.WakesLater".Localized());

            OutputText("Scenes.Riftcabinscene.FapBloomers.DoorOpens".Localized());

            LeaveCabin(true);
        }

        public void SchlickBloomers()
        {
            ClearOutput();

            Save.SetBool("tookBloomers", true);

            Save.SetString("orgasmType", "Vaginal");

            if (loli)
            {
                OutputText("Scenes.Riftcabinscene.SchlickBloomers.TurnsBloomers".Localized());

                OutputText("Scenes.Riftcabinscene.SchlickBloomers.SeesHerBefore".Localized());

                OutputText("Scenes.Riftcabinscene.SchlickBloomers.CrawlsAtop".Localized());

                OutputText("Scenes.Riftcabinscene.SchlickBloomers.BreaksKiss".Localized());

                OutputText("Scenes.Riftcabinscene.SchlickBloomers.SmilesLeansIn".Localized());

                OutputText("Scenes.Riftcabinscene.SchlickBloomers.NotKeepingWaiting".Localized());

                OutputText("Scenes.Riftcabinscene.SchlickBloomers.GetsIdea".Localized());

                OutputText("Scenes.Riftcabinscene.SchlickBloomers.NotGoingDeny".Localized());

                OutputText("Scenes.Riftcabinscene.SchlickBloomers.BeenMoaning".Localized((Player.IsPureEnough(50) ? "gently" : "roughly"), (Player.IsPureEnough(75) ? "But you don't mind, she deserves her reward." : "You'll have to punish her for that, but first things first.")));

                OutputText("Scenes.Riftcabinscene.SchlickBloomers.SeparatesSlit".Localized());

                if (Player.IsPureEnough(75))
                {
                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.CrawlsUpLiesDown".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.GrindingUp".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.SeparatesLips".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.HugsTighter".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.LettingGoTells".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.IntertwinedEmbrace".Localized());
                }
                else
                {
                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.PlaythingMight".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.ClimbsOver".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.PrettyMediocre".Localized((Player.Cocks[Player.SmallestCockIndex()].Length >= 7 ? " grab your [cock smallest] and" : "")));

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.GrinAsTears".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.ReachesBack".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.LeftHerFace".Localized((Player.IsPureEnough(50) ? "gently" : "roughly"), (Player.IsPureEnough(75) ? "But you don't mind, she deserves her reward." : "You'll have to punish her for that, but first things first.")));

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.OrgasmRising".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.RaisesHips".Localized((Player.IsCorruptEnough(100) ? " Maybe you should find her frozen corpse out there and have some fun with the real thing. Stiff fingers go a long way. Though that would be pretty cold. Literally." : "")));
                }
            }
            else
            {
                OutputText("Scenes.Riftcabinscene.SchlickBloomers.StaresUnderwear".Localized());

                OutputText("Scenes.Riftcabinscene.SchlickBloomers.ImaginesStanding".Localized());

                if (Player.IsPureEnough(75))
                {
                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.WorldOfPleasure".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.SashaysCloser".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.HandWanders".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.WontLetHer".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.FingersPumping".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.LeansCups".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.ContinuesWhile".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.ShinyTrail".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.DivesFolds".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.CirclingBody".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.NeckAches".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.LieIntertwined".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.ComesDown".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.ComesTo".Localized());
                }
                else
                {
                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.WorldOfPain".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.GrinSashay".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.MassagesShoulders".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.GrowsBored".Localized((Player.IsCorruptEnough(100) ? " degenerate," : "")));

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.DidSheCome".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.ExtendsTongue".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.GettingIntoIt".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.SpreadingLegs".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.DipsBetween".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.ByTheTime".Localized());

                    OutputText("Scenes.Riftcabinscene.SchlickBloomers.SlidesOut".Localized());
                }
            }

            OutputText("Scenes.Riftcabinscene.SchlickBloomers.WakesLater".Localized());

            OutputText("Scenes.Riftcabinscene.SchlickBloomers.DoorOpens".Localized());

            LeaveCabin(true);
        }

    }
}