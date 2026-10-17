using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<RathazulScene>("coc:rathazul")]
    public class RathazulScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Npcs.Rathazul);

        public Action? shopReturn;

        public string debugName => "Rathazul";

        public string debugHint => "";

        public int mixologyXP { get => Save.GetInt("mixologyXP", 0); set => Save.SetInt("mixologyXP", value); }

        public bool offeredDebimbo => Save.GetBool("offeredDebimboPlayer") || Save.GetBool("offeredDebimboSophie");

        public int reductoCost => CoC.Instance.Flags.Get(GameFlagIds.AmilyMetRathazul) >= 2 ? 50 : 100;

        public override void Entry()
        {
            ExecEncounter();
        }

        public void Reset()
        {

            Save.SetBool("metRathazul", false);

            Save.SetBool("campFollower", false);

            Save.SetBool("campOffer", false);

            Save.SetInt("mixologyXP", 0);

            Save.SetBool("offeredGel", false);

            Save.SetBool("offeredChitin", false);

            Save.SetBool("offeredSilk", false);

            Save.SetBool("offeredScale", false);

            Save.SetBool("offeredEbonbloom", false);

            Save.SetBool("offeredDye", false);

            Save.SetBool("offeredPhilter", false);

            Save.SetBool("offeredHoney", false);

            Save.SetBool("offeredReducto", false);

            Save.SetBool("offeredMarae", false);

            Save.SetBool("offeredGolemHeart", false);

            Save.SetBool("offeredTBark", false);

            Save.SetBool("offeredDBark", false);

            Save.SetBool("offeredTrice", false);

            Save.SetBool("offeredOculum", false);

            Save.SetBool("offeredPurify", false);

            Save.SetBool("offeredDemonTF", false);

            Save.SetBool("offeredDelight", false);

            Save.SetBool("offeredMinoCum", false);

            Save.SetBool("offeredLaBova", false);

            Save.SetBool("offeredLactaidTaurinum", false);

            Save.SetBool("offeredDebimboPlayer", false);

            Save.SetBool("offeredDebimboSophie", false);

            Save.SetBool("giftedBear", false);
        }

        public void ReturnToRathazulMenu()
        {

            if (FollowerRathazul())
                CampRathazul();
            else
                ExecEncounter();
        }

        public void ReturnToShopMenu()
        {

            if (shopReturn == null)
                ReturnToRathazulMenu();
            else
                shopReturn?.Invoke();
        }

        public void ExecEncounter()
        {
            ClearOutput();

            ShowImage("encounter-rathazul");

            if (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) == 2 && Save.GetBool("metRathazul"))
            {
                SceneInstance<MarblePurification>.Get().VisitRathazulToPurifyMarbleAfterLaBovaStopsWorkin();

                return ;
            }

            if (Save.GetBool("metRathazul"))
            {
                OutputText("Scenes.Rathazul.ExecEncounter.SpyFamiliarSight".Localized());

                Player.UnlockCodexEntry(GameFlagIds.CodexEntryRatMice);
            }
            else
            {
                OutputText("Scenes.Rathazul.ExecEncounter.HunchedFigure".Localized((noFur ? "rat-like whiskers" : "a rat-like muzzle"), (noFur ? "; entirely human aside from the whiskers and small rat ears" : "")));

                Save.SetBool("metRathazul", true);

                Player.UnlockCodexEntry(GameFlagIds.CodexEntryRatMice);
            }

            if (mixologyXP >= 16 && !Save.GetBool("campOffer") && Player.IsPureEnough(75))
            {
                OutputText("Scenes.Rathazul.ExecEncounter.AsksTheRat".Localized());

                OutputText("Scenes.Rathazul.ExecEncounter.MoveOption".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), RathazulMoveToCamp);

                AddButton(1, "Common.Button.No".Localized(), RathazulMoveDecline);

                Save.SetBool("campOffer", true);

                return ;
            }

            RathazulWorkOffer();
        }

        public bool FollowerRathazul()
        {

            return Save.GetBool("campFollower");
        }

        public void RathazulMoveToCamp()
        {
            ClearOutput();

            ShowImage("encounter-rathazul");

            OutputText("Scenes.Rathazul.RathazulMoveToCamp.SmilesHappily".Localized());

            OutputText("Scenes.Rathazul.RathazulMoveToCamp.LookOfSomeone".Localized());

            Save.SetBool("campFollower", true);

            DoNext(() => ReturnToCamp(1));
        }

        public void RathazulMoveDecline()
        {
            ClearOutput();

            ShowImage("encounter-rathazul");

            OutputText("Scenes.Rathazul.RathazulMoveDecline.WheezesSigh".Localized());

            OutputText("Scenes.Rathazul.RathazulMoveDecline.MuttersPacksUp".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void CampRathazul()
        {
            ClearOutput();

            ShowImage("encounter-rathazul");

            if (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) == 2 && Save.GetBool("metRathazul"))
            {
                SceneInstance<MarblePurification>.Get().VisitRathazulToPurifyMarbleAfterLaBovaStopsWorkin();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RathazulSilkArmorCountdown) == 1 && CoC.Instance.Flags.Get(GameFlagIds.RathazulSilkArmorType) > 0)
            {
                CollectSilkArmor();

                return ;
            }

            if ((!Save.GetBool("offeredDebimboPlayer") && Player.Intelligence < 20) || (!Save.GetBool("offeredDebimboSophie") && CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0))
            {
                RathazulDebimboOffer();

                return ;
            }

            if (Random.Shared.Next(6) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RathazulCampInteractionCountdown) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.RathazulCampInteractionCountdown, 3);

                if (CoC.Instance.Flags.Get(GameFlagIds.JojoRathazulInteractionCounter) == 0 && CoC.Instance.Flags.Get(GameFlagIds.JojoFixedStatus) > 0)
                {
                    SceneInstance<FollowerInteractionsScene>.Get().JojoOffersRathazulMeditation();

                    return ;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyMetRathazul) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) > 0)
                {
                    SceneInstance<FollowerInteractionsScene>.Get().AmilyIntroducesSelfToRathazul();

                    return ;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyMetRathazul) == 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) > 0)
                {
                    SceneInstance<FollowerInteractionsScene>.Get().AmilyIngredientDelivery();

                    return ;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyMetRathazul) == 2 && CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) > 0)
                {
                    SceneInstance<FollowerInteractionsScene>.Get().AmilyAsksAboutRathazulsVillage();

                    return ;
                }
            }

            ShowImage("rathazul-himself");
// 原版 RathazulScene.hx:40：Akky 若在 Rathazul 处则追加它的位置描述
            SceneInstance<AkkyScene>.Get().LocationDesc("Rathazul");

            OutputText("Scenes.Rathazul.CampRathazul.LooksUpEquipment".Localized());

            OutputText("Scenes.Rathazul.CampRathazul.HeSays".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryRatMice);

            RathazulWorkOffer();
        }

        public void RathazulWorkOffer()
        {

            var lethiciteDefense = Player.KeyItemV1("Marae's Lethicite") > 0 && !Player.HasStatusEffect(StatusEffectIds.DefenseCanopy);

            var seaShirt = Undergarments.SEA_SHIRT;

            if (CoC.Instance.Flags.Get(GameFlagIds.RathazulSilkArmorCountdown) == 1 && CoC.Instance.Flags.Get(GameFlagIds.RathazulSilkArmorType) > 0)
            {
                CollectSilkArmor();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaPurificationRathazulTalked) == 1 && CoC.Instance.Flags.Get(GameFlagIds.MinervaPurificationProgress) < 10)
            {
                PurificationByRathazulBegin();

                return ;
            }

            if (Player.Gems >= 50 && !Save.GetBool("offeredDye"))
            {
                Save.SetBool("offeredDye", true);

                OutputText("Scenes.Rathazul.RathazulWorkOffer.OffersSinceEnough".Localized());

                if (mixologyXP >= 32)
                    OutputText("Scenes.Rathazul.RathazulWorkOffer.AbleToMake".Localized());

                OutputText("Scenes.Rathazul.RathazulWorkOffer.OrWantChanges".Localized());
            }

            if (Player.Gems >= 500 && mixologyXP >= 100 && !Save.GetBool("offeredPhilter"))
            {
                Save.SetBool("offeredPhilter", true);

                OutputText("Scenes.Rathazul.RathazulWorkOffer.RathazulOffers".Localized());
            }

            if ((Player.HasItem(ItemRegistry.Create(ConsumableIds.EggBlack)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.EggLargeBlack))) && CoC.Instance.Flags.Get(GameFlagIds.PcKnowsAboutBlackEggs) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.PcKnowsAboutBlackEggs, 1);

                OutputText("Scenes.Rathazul.RathazulWorkOffer.EyesOnyxEgg".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaPurificationRathazulTalked) == 2 && CoC.Instance.Flags.Get(GameFlagIds.MinervaPurificationProgress) < 10 && !Player.HasKeyItem("Rathazul's Purity Potion"))
                OutputText("Scenes.Rathazul.RathazulWorkOffer.RodentLooksAt".Localized());

            if (mixologyXP >= 30 && !Save.GetBool("offeredLactaidTaurinum"))
            {
                Save.SetBool("offeredLactaidTaurinum", true);

                OutputText("Scenes.Rathazul.RathazulWorkOffer.RatMentions".Localized());
            }

            if (FollowerRathazul() && mixologyXP >= 20 && !Save.GetBool("offeredReducto"))
            {
                Save.SetBool("offeredReducto", true);

                OutputText("Scenes.Rathazul.RathazulWorkOffer.HurriesToBench".Localized(
                    reductoCost
                ));
            }

            if (lethiciteDefense && FollowerRathazul() && !Save.GetBool("offeredMarae"))
            {
                Save.SetBool("offeredMarae", true);

                OutputText("Scenes.Rathazul.RathazulWorkOffer.EyesWidenApproaching".Localized());
            }

            ClearButtons();

            AddNextButton("Common.Button.Appearance".Localized(), RathazulAppearance);

            AddNextButton("Common.Button.Talk".Localized(), RathazulTalkMenu);

            AddNextButton("Common.Button.Services".Localized(), RathazulServicesMenu);

            if (silly && FollowerRathazul())
                AddNextButton("Common.Button.Flirt".Localized(), GetThatRatAss);

            if (lethiciteDefense && Save.GetBool("offeredMarae"))
                AddButton(5, "Items.Lethicite.Short".Localized(), GrowLethiciteDefense);

            if (Player.HasItem(UseableIds.TeddyBear) && !Save.GetBool("giftedBear"))
                AddButton(7, "Common.Button.GiftBear".Localized(), RathazulBear);

            if (Player.Armor.Id == ArmorIds.VineArmorBehavior && !((0 & 0) != 0))
                AddButton(10, "Common.Button.Vines".Localized(), () => { });

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Liddellium)) && (CoC.Instance.Flags.Get(GameFlagIds.LiddelliumFlag) >= 0))
                AddButton(11, "Items.Liddellium.Short".Localized(), IdLiddellium);

            if (Player.UpperGarment == seaShirt && true && Save.GetInt("rathazulAsked", 0) == 0)
                AddRowButton(2, "Cursed Shirt", AskRathazulAboutNobleShirt);

            SetExitButton("Common.Button.Leave".Localized(), () => { if (FollowerRathazul()) Go<CampScene>(); else ReturnToCamp(1); });
        }

        public void RathazulAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Rathazul.RathazulAppearance.AgingRat".Localized());

            DoNext(() => ReturnToRathazulMenu());
        }

        public void RathazulTalkMenu()
        {
            ClearOutput();

            OutputText("Scenes.Rathazul.RathazulTalkMenu.TalkPrompt".Localized());

            ClearButtons();

            AddNextButton("CharCreation.ChooseHistory.Button2".Localized(), RathazulTalkAlchemy);

            AddNextButton("Common.Button.HisPast".Localized(), RathazulTalkPast);

            AddNextButton("Common.Button.Hope".Localized(), RathazulTalkHope);

            AddNextButton("Common.Button.Youth".Localized(), RathazulTalkYouth);

            AddButton(14, "Common.Button.Back".Localized(), () => ReturnToRathazulMenu());
        }

        public void RathazulTalkAlchemy()
        {
            ClearOutput();

            OutputText("Scenes.Rathazul.RathazulTalkAlchemy.AsksAboutInterest".Localized());

            OutputText("Scenes.Rathazul.RathazulTalkAlchemy.TapsClaw".Localized());

            OutputText("Scenes.Rathazul.RathazulTalkAlchemy.SeemsToRealize".Localized());

            OutputText("Scenes.Rathazul.RathazulTalkAlchemy.ShakesHeadContinuing".Localized());

            OutputText("Scenes.Rathazul.RathazulTalkAlchemy.SmilesOnceAgain".Localized());

            DoNext(() => RathazulTalkMenu());
        }

        public void RathazulTalkPast()
        {
            ClearOutput();

            OutputText("Scenes.Rathazul.RathazulTalkPast.WondersWouldnt".Localized());

            OutputText("Scenes.Rathazul.RathazulTalkPast.GlancesUp".Localized());

            OutputText("Scenes.Rathazul.RathazulTalkPast.AsksIfCould".Localized());

            OutputText("Scenes.Rathazul.RathazulTalkPast.HeSaysWhyNot".Localized());

            OutputText("Scenes.Rathazul.RathazulTalkPast.GlowersGround".Localized());

            DoNext(() => RathazulTalkMenu());
        }

        public void RathazulTalkHope()
        {
            ClearOutput();

            OutputText("Scenes.Rathazul.RathazulTalkHope.DespiteAge".Localized());

            OutputText("Scenes.Rathazul.RathazulTalkHope.QuestionResonates".Localized());

            OutputText("Scenes.Rathazul.RathazulTalkHope.LooksDirectly".Localized());

            DoNext(() => RathazulTalkMenu());
        }

        public void RathazulTalkYouth()
        {
            ClearOutput();

            OutputText("Scenes.Rathazul.RathazulTalkYouth.GivenOlder".Localized());

            OutputText("Scenes.Rathazul.RathazulTalkYouth.YouthsMess".Localized());

            OutputText("Scenes.Rathazul.RathazulTalkYouth.HopefulException".Localized((CoC.Instance.Flags.Get(GameFlagIds.AmilyMetRathazul) != 0 ? "And that young mouse-girl you've brought here is quite diligent, too. " : "")));

            OutputText("Scenes.Rathazul.RathazulTalkYouth.TurnsBackEquipment".Localized());

            DoNext(() => RathazulTalkMenu());
        }

        public void RathazulServicesMenu()
        {
            ClearOutput();

            OutputText("Scenes.Rathazul.RathazulServicesMenu.AsksServices".Localized());

            OutputText("Scenes.Rathazul.RathazulServicesMenu.LosesTrack".Localized());

            OutputText("Scenes.Rathazul.RathazulServicesMenu.RiflesProducing".Localized());

            var offered = false;

            if (Player.HasItem(ItemRegistry.Create(ItemIds.GreenGel)) && !Save.GetBool("offeredGel"))
            {
                Save.SetBool("offeredGel", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.PipesUp".Localized());

                offered = true;
            }

            if (Player.HasItem(UseableIds.BeeChitin) && !Save.GetBool("offeredChitin"))
            {
                Save.SetBool("offeredChitin", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.ElderlyRatLooks".Localized());

                offered = true;
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.ToughSilk)) && !Save.GetBool("offeredSilk"))
            {
                Save.SetBool("offeredSilk", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.OffersRathazul".Localized());

                offered = true;
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.D_SCALE)) && !Save.GetBool("offeredScale"))
            {
                Save.SetBool("offeredScale", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.FurrowsBrow".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.EmberHatched) > 0 || CoC.Instance.Flags.Get(GameFlagIds.KihaFollower) > 0 ? " Quarreling with " + (CoC.Instance.Flags.Get(GameFlagIds.KihaFollower) > 0 && CoC.Instance.Flags.Get(GameFlagIds.EmberHatched) > 0 ? "those draconic hot-heads" : "that draconic hot-head") + "?" : ""
                ));

                offered = true;
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EBNFLWR)) && !Save.GetBool("offeredEbonbloom"))
            {
                Save.SetBool("offeredEbonbloom", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.LooksAtFlower".Localized());

                offered = true;
            }

            if (!Save.GetBool("offeredPurify"))
            {
                var purify = new[] { ConsumableIds.IncubiDraft, ConsumableIds.SuccubiMilk, ConsumableIds.SuccubusDelight, ConsumableIds.LaBova, ConsumableIds.MinotaurCum };

                foreach (var tainted in purify)
                    if (Player.HasItem(tainted))
                    {
                        Save.SetBool("offeredPurify", true);

                        break;
                    }
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.BeeHoney)) && !Save.GetBool("offeredHoney"))
            {
                Save.SetBool("offeredHoney", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.RathazulOffersAgain".Localized());

                offered = true;
            }

            if (Player.HasKeyItem("Golem's Heart") && !Save.GetBool("offeredGolemHeart"))
            {
                Save.SetBool("offeredGolemHeart", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.EyesAlight".Localized());

                offered = true;
            }

            if (Player.HasKeyItem("Tentacled Bark Plates") && !Save.GetBool("offeredTBark"))
            {
                Save.SetBool("offeredTBark", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.TentacledPiece".Localized());

                OutputText("Scenes.Rathazul.RathazulServicesMenu.ItsMarae".Localized());

                OutputText("Scenes.Rathazul.RathazulServicesMenu.SilentOverMinute".Localized());

                offered = true;
            }

            if (Player.HasKeyItem("Divine Bark Plates") && !Save.GetBool("offeredDBark"))
            {
                Save.SetBool("offeredDBark", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.SoftGlowDivine".Localized());

                OutputText("Scenes.Rathazul.RathazulServicesMenu.TellsDetails".Localized());

                OutputText("Scenes.Rathazul.RathazulServicesMenu.BreaksOutGrin".Localized());

                offered = true;
            }

            if (Player.HasItem(ItemIds.TonOTrice) && !Save.GetBool("offeredTrice"))
            {
                Save.SetBool("offeredTrice", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.EyesSweepItems".Localized());

                OutputText("Scenes.Rathazul.RathazulServicesMenu.AsksYouDescribe".Localized());

                OutputText("Scenes.Rathazul.RathazulServicesMenu.SuddenlyShouts".Localized());

                offered = true;
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SweetGossamer)) && !Save.GetBool("offeredOculum"))
            {
                Save.SetBool("offeredOculum", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.HoldsSweetBundle".Localized());

                OutputText("Scenes.Rathazul.RathazulServicesMenu.CouldDoSomething".Localized());

                offered = true;
            }

            if ((Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.IncubiDraft))) && !Save.GetBool("offeredDemonTF"))
            {
                Save.SetBool("offeredDemonTF", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.GlancesProffered".Localized());

                OutputText("Scenes.Rathazul.RathazulServicesMenu.StartsOutPauses".Localized());

                offered = true;
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubusDelight)) && !Save.GetBool("offeredDelight"))
            {
                Save.SetBool("offeredDelight", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.FindsSuccubiBottle".Localized());

                OutputText("Scenes.Rathazul.RathazulServicesMenu.SighsWord".Localized());

                offered = true;
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.MinotaurCum)) && !Save.GetBool("offeredMinoCum"))
            {
                Save.SetBool("offeredMinoCum", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.EyesShootWide".Localized());

                OutputText("Scenes.Rathazul.RathazulServicesMenu.HowDangerous".Localized());

                OutputText("Scenes.Rathazul.RathazulServicesMenu.HeStarts".Localized());

                offered = true;
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LaBova)) && !Save.GetBool("offeredLaBova"))
            {
                Save.SetBool("offeredLaBova", true);

                OutputText("Scenes.Rathazul.RathazulServicesMenu.PicksUpBottle".Localized());

                OutputText("Scenes.Rathazul.RathazulServicesMenu.LooksBackUp".Localized());

                offered = true;
            }

            if (!offered)
                OutputText("Scenes.Rathazul.RathazulServicesMenu.NodsSelection".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Shop".Localized(), RathazulShopMenu);

            AddNextButton("Common.Button.Craft".Localized(), RathazulArmorMenu);

            AddNextButton("CharCreation.ChooseHistory.Button2".Localized(), RathazulAlchemyMenu);

            if (Save.GetBool("offeredPurify"))
                AddNextButton("Common.Button.Purify".Localized(), PurifySomething);

            AddButton(14, "Common.Button.Back".Localized(), ReturnToRathazulMenu);
        }

        public bool CraftingInProgress()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.RathazulSilkArmorCountdown) > 0;
        }

        public void RathazulArmorMenu()
        {
            ClearOutput();

            ClearButtons();

            if (Save.GetBool("offeredGel"))
                AddNextButton("Common.Button.Gelarmor".Localized(), CraftOozeArmor);

            if (Save.GetBool("offeredChitin"))
                AddNextButton("Common.Button.Beearmor".Localized(), CraftCarapace);

            if (Save.GetBool("offeredSilk"))
                AddNextButton("Common.Button.SpiderSilk".Localized(), CraftSilkArmor);

            if (Save.GetBool("offeredScale"))
                AddNextButton("Common.Button.Dragonscale".Localized(), CraftDragonscaleArmor);

            if (Save.GetBool("offeredEbonbloom"))
                AddNextButton("Items.Ebonbloom.Short".Localized(), CraftEbonweaveArmor);

            if (Player.HasKeyItem("Golem's Heart"))
                AddNextButton("Common.Button.VolcanicArmor".Localized(), CraftGolemArmor);

            if (Player.HasKeyItem("Tentacled Bark Plates"))
                AddNextButton("Common.Button.TBarkArmor".Localized(), () => CraftMaraeArmor(false));

            if (Player.HasKeyItem("Divine Bark Plates"))
                AddNextButton("Common.Button.DBarkArmor".Localized(), () => CraftMaraeArmor(true));

            if (Buttons.All(b => b is null))
                OutputText("Scenes.Rathazul.RathazulArmorMenu.DontHaveMaterials".Localized());
            else
                OutputText("Scenes.Rathazul.RathazulArmorMenu.WhichProject".Localized());

            AddButton(14, "Common.Button.Back".Localized(), () => RathazulServicesMenu());
        }

        public void CraftOozeArmor()
        {
            ClearOutput();

            ShowImage("item-gelArmor");

            OutputText("Scenes.Rathazul.CraftOozeArmor.TakesGreenGel".Localized());

            Player.DestroyItems(ItemIds.GreenGel, 5);

            mixologyXP += 8;

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.GelArmor), () => ReturnToRathazulMenu());
        }

        public void CraftCarapace()
        {
            ClearOutput();

            ShowImage("item-chitinArmor");

            OutputText("Scenes.Rathazul.CraftCarapace.TakesScales".Localized());

            OutputText("Scenes.Rathazul.CraftCarapace.PlatesShimmer".Localized((silly ? ", even bragging a little bit. [say: Let me show you the different lengths of string I used.]" : ".")));

            if (Player.HasCock() && Player.BiggestCockArea() >= 40)
                OutputText("Scenes.Rathazul.CraftCarapace.SilkenDoesLittle".Localized());

            if (Player.BiggestBreastSize() >= 8)
                OutputText("Scenes.Rathazul.CraftCarapace.TightBreastplate".Localized(
                    Appearance.BiggestBreastSizeDescript(Player)
                ));

            Player.DestroyItems(UseableIds.BeeChitin, 5);

            mixologyXP += 8;

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.BeeArmor), () => ReturnToRathazulMenu());
        }

        public void CraftSilkArmor()
        {
            ClearOutput();

            ShowImage("rathazul-lab");

            OutputText("Scenes.Rathazul.CraftSilkArmor.HandsWebbing".Localized());

            if (!Player.HasItem(ItemRegistry.Create(ConsumableIds.ToughSilk), 2))
            {
                OutputText("Scenes.Rathazul.CraftSilkArmor.ShakesHead".Localized());

                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.SpiderAbdomen)
                {
                    OutputText("Scenes.Rathazul.CraftSilkArmor.ShowsSpiderLike".Localized());

                    OutputText("Scenes.Rathazul.CraftSilkArmor.ShakesHeadAgain".Localized());
                }

                DoNext(() => ReturnToRathazulMenu());

                return ;
            }

            OutputText("Scenes.Rathazul.CraftSilkArmor.LimpsToBench".Localized());

            OutputText("Scenes.Rathazul.CraftSilkArmor.CantHelpSigh".Localized());

            if (Player.Gems < 500)
            {
                OutputText("Scenes.Rathazul.CraftSilkArmor.WaitDontEven".Localized());

                DoNext(() => ReturnToRathazulMenu());

                return ;
            }

            AddButton(0, "Common.Button.Yes".Localized(), CommissionSilkArmorForReal);

            AddButton(1, "Common.Button.No".Localized(), DeclineSilkArmorCommish);
        }

        public void CommissionSilkArmorForReal()
        {
            ClearOutput();

            ShowImage("rathazul-lab");

            var hasFive = Player.HasItem(ItemRegistry.Create(ConsumableIds.ToughSilk), 5);

            OutputText("Scenes.Rathazul.CommissionSilkArmorForReal.SortsGemsPayment".Localized());

            if (hasFive)
                OutputText("Scenes.Rathazul.CommissionSilkArmorForReal.MageRobeOrArmor".Localized());
            else
                OutputText("Scenes.Rathazul.CommissionSilkArmorForReal.NotEnoughSilk".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Armor".Localized(), () => ChooseArmorOrRobes(1));

            AddNextButton("Common.Button.Robes".Localized(), () => ChooseArmorOrRobes(2));

            AddNextButton("Common.Button.Bra".Localized(), () => ChooseArmorOrRobes(3));

            AddNextButton("Common.Button.Panties".Localized(), () => ChooseArmorOrRobes(4));

            AddNextButton("Common.Button.Loincloth".Localized(), () => ChooseArmorOrRobes(5));

            AddButton(14, "Scenes.CorruptedGlade.NeverMind".Localized(), () => DeclineSilkArmorCommish());
        }

        public void DeclineSilkArmorCommish()
        {
            ClearOutput();

            ShowImage("encounter-rathazul");

            OutputText("Scenes.Rathazul.DeclineSilkArmorCommish.TakesSilkBack".Localized((noFur ? "head." : "muzzle.")));

            DoNext(() => ReturnToRathazulMenu());
        }

        public void ChooseArmorOrRobes(int robeType)
        {
            ClearOutput();

            if (robeType == 1 || robeType == 2)
                Player.DestroyItems(ConsumableIds.ToughSilk, 5);
            else
                Player.DestroyItems(ConsumableIds.ToughSilk, 2);

            Player.Gems -= 500;

            OutputText("Scenes.Rathazul.ChooseArmorOrRobes.GruntsResponse".Localized());

            if (FollowerRathazul())
                OutputText("Scenes.Rathazul.ChooseArmorOrRobes.TurnsBackCenter".Localized());
            else
                OutputText("Scenes.Rathazul.ChooseArmorOrRobes.HeadsBackCamp".Localized());

            OutputText("Scenes.Rathazul.ChooseArmorOrRobes.WonderingRodent".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RathazulSilkArmorType, robeType);

            CoC.Instance.Flags.Set(GameFlagIds.RathazulSilkArmorCountdown, 24);

            DoNext(() => ReturnToCamp(1));
        }

        public void CollectSilkArmor()
        {
            ClearOutput();

            OutputText("Scenes.Rathazul.CollectSilkArmor.BeamsAnnounces".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RathazulSilkArmorType) == 1)
                OutputText("Scenes.Rathazul.CollectSilkArmor.ArmorWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RathazulSilkArmorType) == 2)
                    OutputText("Scenes.Rathazul.CollectSilkArmor.RobeWord".Localized());
                else
                    OutputText("Scenes.Rathazul.CollectSilkArmor.UndergarmentWord".Localized());

            OutputText("Scenes.Rathazul.CollectSilkArmor.IsFinished".Localized());

            Identifier itype = default;

            switch (CoC.Instance.Flags.Get(GameFlagIds.RathazulSilkArmorType))
            {
                case 1:
                {
                ShowImage("item-silkArmor");

                OutputText("Scenes.Rathazul.CollectSilkArmor.GlitteringArmor".Localized());

                OutputText("Scenes.Rathazul.CollectSilkArmor.Marvels".Localized());

                itype = ArmorIds.SilkArmor;
                }

                break;

                case 2:
                {
                ShowImage("item-silkRobes");

                OutputText("Scenes.Rathazul.CollectSilkArmor.HangingOnRack".Localized());

                OutputText("Scenes.Rathazul.CollectSilkArmor.TakesDownGarment".Localized());

                itype = ArmorIds.SS_ROBE;
                }

                break;

                case 3:
                {
                ShowImage("item-silkBra");

                OutputText("Scenes.Rathazul.CollectSilkArmor.WhiteOnTable".Localized());

                OutputText("Scenes.Rathazul.CollectSilkArmor.TakesGarment".Localized());

                itype = ItemIds.SilkBra;
                }

                break;

                case 4:
                {
                ShowImage("item-silkPanties");

                OutputText("Scenes.Rathazul.CollectSilkArmor.PairOnTable".Localized());

                OutputText("Scenes.Rathazul.CollectSilkArmor.TakesGarmentAgain".Localized());

                itype = ItemIds.SilkPanties;
                }

                break;

                case 5:
                {
                ShowImage("item-silkLoincloth");

                OutputText("Scenes.Rathazul.CollectSilkArmor.WhiteOnTableAgain".Localized());

                OutputText("Scenes.Rathazul.CollectSilkArmor.TakesGarmentThird".Localized());

                itype = ItemIds.SilkLoincloth;
                }

                break;
            }

            if (true )
            {
                OutputText("Scenes.Rathazul.CollectSilkArmor.BugReportFallback".Localized());

                itype = ArmorIds.SS_ROBE;
            }

            mixologyXP += 8;

            CoC.Instance.Flags.Set(GameFlagIds.RathazulSilkArmorType, 0);

            CoC.Instance.Flags.Set(GameFlagIds.RathazulSilkArmorCountdown, 0);

            InventoryScene.TakeItem(ItemRegistry.Create(itype), () => ReturnToRathazulMenu());
        }

        public void CraftDragonscaleArmor()
        {
            ClearOutput();

            var hasFive = Player.HasItem(ItemRegistry.Create(ConsumableIds.D_SCALE), 5);

            ShowImage("rathazul-lab");

            OutputText("Scenes.Rathazul.CraftDragonscaleArmor.LooksAtSheets".Localized());

            if (!hasFive)
                OutputText("Scenes.Rathazul.CraftDragonscaleArmor.RealizesStill".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Armor".Localized(), () => CraftDragonscaleArmorForReal(0));

            AddNextButton("Common.Button.Robe".Localized(), () => CraftDragonscaleArmorForReal(1));

            AddNextButton("Common.Button.Bra".Localized(), () => CraftDragonscaleArmorForReal(2));

            AddNextButton("Common.Button.Thong".Localized(), () => CraftDragonscaleArmorForReal(3));

            AddNextButton("Common.Button.Loincloth".Localized(), () => CraftDragonscaleArmorForReal(4));

            AddButton(14, "Scenes.CorruptedGlade.NeverMind".Localized(), () => RathazulArmorMenu());
        }

        public void CraftDragonscaleArmorForReal(int type = 0)
        {
            ClearOutput();

            if (type == 0 || type == 1)
                Player.DestroyItems(ConsumableIds.D_SCALE, 5);
            else
                Player.DestroyItems(ConsumableIds.D_SCALE, 2);

            OutputText("Scenes.Rathazul.CraftDragonscaleArmorForReal.TakesScales".Localized());

            Identifier itype = default;

            switch (type)
            {
                case 0:
                {
                ShowImage("item-dragonscalearmor");

                OutputText("Scenes.Rathazul.CraftDragonscaleArmorForReal.RedArmor".Localized());

                itype = ArmorIds.DragonScaleArmor;
                }

                break;

                case 1:
                {
                ShowImage("item-dragonscalerobes");

                OutputText("Scenes.Rathazul.CraftDragonscaleArmorForReal.RedRobe".Localized());

                itype = ArmorIds.DragonScaleRobes;
                }

                break;

                case 2:
                {
                ShowImage("item-dragonscalebra");

                OutputText("Scenes.Rathazul.CraftDragonscaleArmorForReal.TexturedBra".Localized());

                itype = ItemIds.DragonScaleBra;
                }

                break;

                case 3:
                {
                ShowImage("item-dragonscalethong");

                OutputText("Scenes.Rathazul.CraftDragonscaleArmorForReal.TexturedThong".Localized());

                itype = Undergarments.DSTHONG.Id;
                }

                break;

                case 4:
                {
                ShowImage("item-dragonscaleloincloth");

                OutputText("Scenes.Rathazul.CraftDragonscaleArmorForReal.TexturedLoincloth".Localized());

                itype = ItemIds.DragonScaleLoincloth;
                }

                break;
            }

            if (true )
            {
                OutputText("Scenes.Rathazul.CraftDragonscaleArmorForReal.BugReportFallback".Localized());

                itype = ArmorIds.DragonScaleArmor;
            }

            mixologyXP += 8;

            InventoryScene.TakeItem(ItemRegistry.Create(itype), () => ReturnToRathazulMenu());
        }

        public void CraftEbonweaveArmor()
        {
            ClearOutput();

            ShowImage("rathazul-lab");

            var ebonCount = Player.ItemCount(ConsumableIds.EBNFLWR);

            OutputText("Scenes.Rathazul.CraftEbonweaveArmor.LooksAtEbonbloom".Localized());

            if (Player.Gems < 500)
            {
                OutputText("Scenes.Rathazul.CraftEbonweaveArmor.WaitDontHave".Localized());

                DoNext(() => ReturnToRathazulMenu());
            }
            else
            {
                if (ebonCount < 8)
                    OutputText("Scenes.Rathazul.CraftEbonweaveArmor.RealizesStill".Localized());
                else
                    if (ebonCount < 10)
                        OutputText("Scenes.Rathazul.CraftEbonweaveArmor.RealizesStillAgain".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Armor".Localized(), () => CraftEbonweaveArmorForReal(0));

                AddNextButton("Common.Button.Jacket".Localized(), () => CraftEbonweaveArmorForReal(1));

                AddNextButton("Common.Button.Robe".Localized(), () => CraftEbonweaveArmorForReal(2));

                AddNextButton("Common.Button.Indecro".Localized(), () => CraftEbonweaveArmorForReal(3));

                AddNextButton("Common.Button.Vest".Localized(), () => CraftEbonweaveArmorForReal(4));

                AddNextButton("Common.Button.Corset".Localized(), () => CraftEbonweaveArmorForReal(5));

                AddNextButton("Common.Button.Jockstrap".Localized(), () => CraftEbonweaveArmorForReal(6));

                AddNextButton("Common.Button.Thong".Localized(), () => CraftEbonweaveArmorForReal(7));

                AddNextButton("Common.Button.Loincloth".Localized(), () => CraftEbonweaveArmorForReal(8));

                AddNextButton("Common.Button.Runestrap".Localized(), () => CraftEbonweaveArmorForReal(9));

                AddNextButton("Common.Button.Runethong".Localized(), () => CraftEbonweaveArmorForReal(10));

                AddNextButton("Common.Button.Runecloth".Localized(), () => CraftEbonweaveArmorForReal(11));

                AddButton(14, "Scenes.CorruptedGlade.NeverMind".Localized(), () => RathazulArmorMenu());
            }
        }

        public void CraftEbonweaveArmorForReal(int type = 0)
        {
            ClearOutput();

            if (type == 0 || type == 1 || type == 2)
                Player.DestroyItems(ConsumableIds.EBNFLWR, 10);
            else
                if (type == 3)
                    Player.DestroyItems(ConsumableIds.EBNFLWR, 8);
                else
                    Player.DestroyItems(ConsumableIds.EBNFLWR, 3);

            Player.Gems -= 500;

            OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.TakesFlowers".Localized());

            OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.WhenYouReturn".Localized());

            Identifier itype = default;

            switch (type)
            {
                case 0:
                {
                ShowImage("item-ebonArmor");

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.LaidNearBench".Localized());

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.CuriousStrength".Localized());

                itype = ArmorIds.EBNARMR;
                }

                break;

                case 1:
                {
                ShowImage("item-ebonJacket");

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.LaidBesideBench".Localized());

                itype = ArmorIds.EBNJACK;
                }

                break;

                case 2:
                {
                ShowImage("item-ebonRobe");

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.HangingOnRack".Localized());

                itype = ArmorIds.EBNROBE;
                }

                break;

                case 3:
                {
                ShowImage("item-ebonRobent");

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.HangingOnRackAgain".Localized());

                itype = ArmorIds.EBNIROB;
                }

                break;

                case 4:
                {
                ShowImage("item-ebonVest");

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.HangingOnRackThird".Localized());

                itype = Undergarments.EBNVEST.Id;
                }

                break;

                case 5:
                {
                ShowImage("item-ebonCorset");

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.HangingOnRackFourth".Localized());

                itype = Undergarments.EBNCRST.Id;
                }

                break;

                case 6:
                {
                ShowImage("item-ebonJockstrap");

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.HangingOnRackFifth".Localized());

                itype = Undergarments.EBNJOCK.Id;
                }

                break;

                case 7:
                {
                ShowImage("item-ebonThong");

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.HangingOnRackSixth".Localized());

                itype = Undergarments.EBNTHNG.Id;
                }

                break;

                case 8:
                {
                ShowImage("item-ebonLoincloth");

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.HangingOnRackSeventh".Localized());

                itype = Undergarments.EBNCLTH.Id;
                }

                break;

                case 9:
                {
                ShowImage("item-ebonJockstrapune");

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.HangingOnRackEighth".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.KnowsArouse))
                    OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.BlushRecognizing".Localized());

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.RunsHandOver".Localized());

                itype = Undergarments.EBNRJCK.Id;
                }

                break;

                case 10:
                {
                ShowImage("item-ebonThongune");

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.HangingOnRackNinth".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.KnowsArouse))
                    OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.BlushRecognizingAgain".Localized());

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.RunsHandOverAgain".Localized());

                itype = Undergarments.EBNRTNG.Id;
                }

                break;

                case 11:
                {
                ShowImage("item-ebonLoinclothune");

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.HangingOnRackTenth".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.KnowsArouse))
                    OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.BlushRecognizingThird".Localized());

                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.RunsHandOverThird".Localized());

                itype = Undergarments.EBNRLNC.Id;
                }

                break;
            }

            if (true )
            {
                OutputText("Scenes.Rathazul.CraftEbonweaveArmorForReal.BugReportFallback".Localized());

                itype = ArmorIds.EBNARMR;
            }

            mixologyXP += 8;

            InventoryScene.TakeItem(ItemRegistry.Create(itype), () => ReturnToRathazulMenu());
        }

        public void CraftMaraeArmor(bool divine = false)
        {
            ClearOutput();

            ShowImage("rathazul-lab");

            if (!divine)
            {
                OutputText("Scenes.Rathazul.CraftMaraeArmor.ShowsPieces".Localized());

                OutputText("Scenes.Rathazul.CraftMaraeArmor.RatSaysTakes".Localized());

                OutputText("Scenes.Rathazul.CraftMaraeArmor.FinishedIs".Localized());

                OutputText("Scenes.Rathazul.CraftMaraeArmor.HandsArmor".Localized());

                DoNext(() => TakethatMarmorC());
            }
            else
            {
                OutputText("Scenes.Rathazul.CraftMaraeArmor.ShowsPiecesAgain".Localized());

                OutputText("Scenes.Rathazul.CraftMaraeArmor.RatSaysTakesAgain".Localized());

                OutputText("Scenes.Rathazul.CraftMaraeArmor.FinishedIsAgain".Localized());

                OutputText("Scenes.Rathazul.CraftMaraeArmor.HandsArmorAgain".Localized());

                DoNext(() => TakethatMarmorD());
            }
        }

        public void TakethatMarmorC()
        {
            ClearOutput();

            ShowImage("item-bArmor-corrupt");

            OutputText("Scenes.Rathazul.TakethatMarmorC.PlatesLikeSnow".Localized());

            OutputText("Scenes.Rathazul.TakethatMarmorC.HandsLoincloth".Localized());

            if (Player.HasCock() && Player.BiggestCockArea() >= 40)
                OutputText("Scenes.Rathazul.TakethatMarmorC.SilkenDoesLittle".Localized());

            if (Player.BiggestBreastSize() >= 8)
                OutputText("Scenes.Rathazul.TakethatMarmorC.TightBreastplate".Localized(
                    Appearance.BiggestBreastSizeDescript(Player)
                ));

            Player.RemoveKeyItem("Tentacled Bark Plates");

            mixologyXP += 8;

            InventoryScene.TakeItem(ItemRegistry.Create(ArmorIds.TentacleBarkArmor), () => ReturnToRathazulMenu());
        }

        public void TakethatMarmorD()
        {
            ClearOutput();

            ShowImage("item-bArmor-pure");

            OutputText("Scenes.Rathazul.TakethatMarmorD.PlatesLikeSnow".Localized((silly ? ", even bragging a little bit. [say: Let me show you the different lengths of string I used.]" : ".")));

            if (Player.HasCock() && Player.BiggestCockArea() >= 40)
                OutputText("Scenes.Rathazul.TakethatMarmorD.SilkenDoesLittle".Localized());

            if (Player.BiggestBreastSize() >= 8)
                OutputText("Scenes.Rathazul.TakethatMarmorD.TightBreastplate".Localized(
                    Appearance.BiggestBreastSizeDescript(Player)
                ));

            Player.RemoveKeyItem("Divine Bark Plates");

            mixologyXP += 8;

            InventoryScene.TakeItem(ItemRegistry.Create(ArmorIds.DivineArmor), () => ReturnToRathazulMenu());
        }

        public void CraftGolemArmor()
        {
            ClearOutput();

            OutputText("Scenes.Rathazul.CraftGolemArmor.ShowsGem".Localized());

            OutputText("Scenes.Rathazul.CraftGolemArmor.OhImpressive".Localized());

            OutputText("Scenes.Rathazul.CraftGolemArmor.EitherWayFashion".Localized());

            OutputText("Scenes.Rathazul.CraftGolemArmor.FinishedIs".Localized());

            OutputText("Scenes.Rathazul.CraftGolemArmor.RegalArmor".Localized());

            OutputText("Scenes.Rathazul.CraftGolemArmor.ThanksHim".Localized());

            Player.RemoveKeyItem("Golem's Heart");

            mixologyXP += 8;

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.GolemArmor), () => ReturnToRathazulMenu());
        }

        public void RathazulShopMenu()
        {

            shopReturn = () => RathazulShopMenu();

            ClearButtons();

            AddNextButton("Common.Button.HairDyes".Localized(), BuyDyes);

            AddNextButton("Common.Button.SkinOils".Localized(), BuyOils);

            AddNextButton("Common.Button.BodyLotions".Localized(), BuyLotions);

            if (Save.GetBool("offeredReducto"))
            {
                AddShopItem(ItemIds.Reducto, reductoCost, "item-reducto");

                AddShopItem(ItemIds.GroPlus, reductoCost, "item-gro-plus");
            }

            AddShopItem(ConsumableIds.HealingPill, 100);

            if (Save.GetBool("offeredPhilter"))
            {
                AddShopItem(ConsumableIds.PurityPhilter, 500);

                AddShopItem(ConsumableIds.NumbingOil, 500);
            }

            AddButton(14, "Common.Button.Back".Localized(), () => RathazulServicesMenu());
        }

        public void BuyDyes()
        {
            ClearOutput();

            shopReturn = BuyDyes;

            ShowImage("rathazul-vials");

            OutputText("Scenes.Rathazul.BuyDyes.PullsForthDyes".Localized());

            ClearButtons();

            AddDye("Auburn", ConsumableIds.HairDyeAuburn);

            AddDye("Black", ConsumableIds.HairDyeBlack);

            AddDye("Blond", ConsumableIds.HairDyeBlond);

            AddDye("Brown", ConsumableIds.HairDyeBrown);

            AddDye("Red", ConsumableIds.HairDyeRed);

            AddDye("White", ConsumableIds.HairDyeWhite);

            AddDye("Gray", ConsumableIds.HairDyeGray);

            if (mixologyXP >= 32)
            {
                AddDye("Blue", ConsumableIds.HairDyeBlue);

                AddDye("Green", ConsumableIds.HairDyeGreen);

                AddDye("Orange", ConsumableIds.HairDyeOrange);

                AddDye("Yellow", ConsumableIds.HairDyeYellow);

                AddDye("Purple", ConsumableIds.HairDyePurple);

                AddDye("Pink", ConsumableIds.HairDyePink);

                AddDye("Russet", ConsumableIds.HairDyeRusset);
            }

            if (mixologyXP >= 48)
                AddDye("Rainbow", ConsumableIds.HairDyeRainbow);

            AddButton(14, "Common.Button.Back".Localized(), () => RathazulShopMenu());
        }

        public void AddDye(string name, Identifier item)
        {

            AddNamedShopItem(name, item, 50, "item-dye");
        }

        public void BuyOils()
        {
            ClearOutput();

            shopReturn = BuyOils;

            ShowImage("rathazul-oils");

            OutputText("Scenes.Rathazul.BuyOils.PullsForthOils".Localized());

            ClearButtons();

            AddOil("Dark", ConsumableIds.SkinOilDark);

            AddOil("Ebony", ConsumableIds.SkinOilEbony);

            AddOil("Fair", ConsumableIds.SkinOilFair);

            AddOil("Light", ConsumableIds.SkinOilLight);

            AddOil("Mahogany", ConsumableIds.SkinOilMahogany);

            AddOil("Olive", ConsumableIds.SkinOilOlive);

            AddOil("Russet", ConsumableIds.SkinOilRusset);

            if (mixologyXP >= 50)
            {
                AddOil("Red", ConsumableIds.SkinOilRed);

                AddOil("Green", ConsumableIds.SkinOilGreen);

                AddOil("White", ConsumableIds.SkinOilWhite);

                AddOil("Blue", ConsumableIds.SkinOilBlue);

                AddOil("Black", ConsumableIds.SkinOilBlack);

                AddOil("Purple", ConsumableIds.SkinOilPurple);

                AddOil("Silver", ConsumableIds.SkinOilSilver);
            }

            if (mixologyXP >= 100)
            {
                AddOil("Orange", ConsumableIds.SkinOilOrange);

                AddOil("Yellow", ConsumableIds.SkinOilYellow);
            }

            if (mixologyXP >= 120)
            {
                AddOil("Yellow Green", ConsumableIds.SkinOilYellowGreen);

                AddOil("Spring Green", ConsumableIds.SkinOilSpringGreen);

                AddOil("Cyan", ConsumableIds.SkinOilCyan);

                AddOil("Ocean Blue", ConsumableIds.SkinOilOceanBlue);
            }

            if (mixologyXP >= 150)
            {
                AddOil("Electric Violet", ConsumableIds.SkinOilElectricViolet);

                AddOil("Magenta", ConsumableIds.SkinOilMagenta);

                AddOil("Deep Pink", ConsumableIds.SkinOilDeepPink);

                AddOil("Pink", ConsumableIds.SkinOilPink);
            }

            AddButton(14, "Common.Button.Back".Localized(), () => RathazulShopMenu());
        }

        public void AddOil(string name, Identifier item)
        {

            AddNamedShopItem(name, item, 50, "item-oil");
        }

        public void BuyLotions()
        {
            ClearOutput();

            shopReturn = BuyLotions;

            ShowImage("rathazul-vials");

            OutputText("Scenes.Rathazul.BuyLotions.PullsForthLotions".Localized());

            ClearButtons();

            AddLotion("Clear", ConsumableIds.ClearLotion);

            AddLotion("Rough", ConsumableIds.RoughLotion);

            AddLotion("Sexy", ConsumableIds.SexyLotion);

            AddLotion("Smooth", ConsumableIds.SmoothLotion);

            AddButton(14, "Common.Button.Back".Localized(), () => RathazulShopMenu());
        }

        public void AddLotion(string name, Identifier item)
        {

            AddNamedShopItem(name, item, 50, "item-lotion");
        }

        public void AddShopItem(Identifier item, int cost, string image = "")
        {

            AddNextButton(ItemRegistry.Create(item).Name, () => BuyItem(item, cost, image));
        }

        public void AddNamedShopItem(string name, Identifier item, int cost, string image = "")
        {

            AddNextButton(name, () => BuyItem(item, cost, image));
        }

        public void BuyItem(Identifier item, int cost, string image = "")
        {
            ClearOutput();

            image = image != "" ? image : "item-box";

            Player.Gems -= cost;

            ShowImage(image);

            InventoryScene.TakeItem(ItemRegistry.Create(item), () => ReturnToShopMenu());

            mixologyXP += 4;
        }

        public void PurifySomething()
        {
            ClearOutput();

            shopReturn = PurifySomething;

            OutputText("Scenes.Rathazul.PurifySomething.RathazulAsks".Localized());

            ClearButtons();

            AddPurify(ConsumableIds.IncubiDraft, ConsumableIds.PurifiedIncubiDraft);

            AddPurify(ConsumableIds.SuccubiMilk, ConsumableIds.PurifiedSuccubiMilk);

            AddPurify(ConsumableIds.SuccubusDelight, ConsumableIds.PurifiedSuccubusDelight);

            AddPurify(ConsumableIds.LaBova, ConsumableIds.PurifiedLaBova);

            AddPurify(ConsumableIds.MinotaurCum, ConsumableIds.PurifiedMinotaurCum);

            AddButton(14, "Common.Button.Back".Localized(), () => RathazulServicesMenu());
        }

        public void AddPurify(Identifier item, Identifier result, int cost = 20)
        {

            AddNextButton(ItemRegistry.Create(item).Name, () => RathazulPurifyItem(item, result, cost));
        }

        public void RathazulPurifyItem(Identifier item, Identifier result, int cost = 20)
        {

            Player.Gems -= cost;

            Player.DestroyItems(item, 1);

            ShowImage("item-box");

            InventoryScene.TakeItem(ItemRegistry.Create(result), () => ReturnToShopMenu());

            mixologyXP += 8;
        }

        public void PurificationByRathazulBegin()
        {
            ClearOutput();

            ShowImage("rathazul-lab");

            OutputText("Scenes.Rathazul.PurificationByRathazulBegin.HopingRodent".Localized());

            OutputText("Scenes.Rathazul.PurificationByRathazulBegin.NodsAndAsks".Localized());

            OutputText("Scenes.Rathazul.PurificationByRathazulBegin.ExplainsSadly".Localized());

            OutputText("Scenes.Rathazul.PurificationByRathazulBegin.WithThatInMind".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationRathazulTalked, 2);

            DoNext(() => ReturnToCamp(1));
        }

        public void RathazulMakesPurifyPotion()
        {
            ClearOutput();

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney)))
                Player.DestroyItems(ConsumableIds.PureHoney, 1);
            else
                Player.DestroyItems(ConsumableIds.PurityPhilter, 1);

            Player.DestroyItems(ConsumableIds.CinnamonMint, 1);

            Player.DestroyItems(ItemIds.PurePeach, 1);

            ShowImage("item-pPotion");

            OutputText("Scenes.Rathazul.RathazulMakesPurifyPotion.HurriesOver".Localized());

            OutputText("Scenes.Rathazul.RathazulMakesPurifyPotion.HaveHerSwallow".Localized());

            OutputText("Scenes.Rathazul.RathazulMakesPurifyPotion.TakesGratefully".Localized());

            OutputText("Scenes.Rathazul.RathazulMakesPurifyPotion.ShrugsHelplessly".Localized());

            OutputText("Scenes.Rathazul.RathazulMakesPurifyPotion.ConcedesPoint".Localized());

            Player.CreateKeyItem("Rathazul's Purity Potion", 0, 0, 0, 0);

            DoNext(() => CampRathazul());
        }

        public void RathazulAlchemyMenu()
        {
            ClearOutput();

            shopReturn = RathazulAlchemyMenu;

            bool hasIngredients = false;

            ClearButtons();

            if (Save.GetBool("offeredHoney"))
                AddNextButton("Common.Button.DistillHoney".Localized(), RathazulMakesPureHoney);

            if (offeredDebimbo)
                AddNextButton("Common.Button.Debimbo".Localized(), MakeADeBimboDraft);

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaPurificationRathazulTalked) == 2 && CoC.Instance.Flags.Get(GameFlagIds.MinervaPurificationProgress) < 10)
            {
                hasIngredients = Player.HasItemArrayAny([ConsumableIds.PureHoney, ConsumableIds.PurityPhilter]) && Player.HasItemArray([ConsumableIds.CinnamonMint, ItemIds.PurePeach]);

                AddNextButton("Common.Button.PurePotion".Localized(), RathazulMakesPurifyPotion);
            }

            if (Save.GetBool("offeredLactaidTaurinum"))
            {
                hasIngredients = Player.HasItemArray([ConsumableIds.Lactaid, ConsumableIds.PurifiedLaBova], [5, 2]);

                AddNextButton("Items.ProLactaid.Short".Localized(), RathazulMakesMilkPotion);

                hasIngredients = Player.HasItemArray([ItemIds.Equinum, ConsumableIds.MinotaurBlood], [2, 1]);

                AddNextButton("Items.Taurinum.Short".Localized(), RathazulMakesTaurPotion);
            }

            if (mixologyXP >= 50 && CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredCockatrices) > 0)
            {
                hasIngredients = Player.HasItemArray([ConsumableIds.Reptlum, ConsumableIds.GoldenSeed]);

                AddNextButton("Items.TonOTrice.Short".Localized(), RathazulMakesCockatricePotion);
            }

            if (mixologyXP >= 50)
            {
                hasIngredients = Player.HasItemArray([ConsumableIds.SweetGossamer, ConsumableIds.HUMMUS_], [2, 1]);

                AddNextButton("Common.Button.OculumA".Localized(), RathazulMakesEyePotion);
            }

            if (Buttons.All(b => b is null))
                OutputText("Scenes.Rathazul.RathazulAlchemyMenu.NothingToMake".Localized());

            AddButton(14, "Common.Button.Back".Localized(), () => RathazulServicesMenu());
        }

        public void RathazulDebimboOffer()
        {
            ClearOutput();

            ShowImage("rathazul-lab");

            if ((Player.HasPerk(PerkIds.BimboBrains) || Player.HasPerk(PerkIds.FutaFaculties)) && !Save.GetBool("offeredDebimboPlayer"))
            {
                Save.SetBool("offeredDebimboPlayer", true);

                OutputText("Scenes.Rathazul.RathazulDebimboOffer.GlancesYourWay".Localized());

                OutputText("Scenes.Rathazul.RathazulDebimboOffer.TwirlsHair".Localized());

                OutputText("Scenes.Rathazul.RathazulDebimboOffer.PleadsCounting".Localized());
            }
            else
                if (Player.HasPerk(PerkIds.BroBrains) && !Save.GetBool("offeredDebimboPlayer"))
                {
                    Save.SetBool("offeredDebimboPlayer", true);

                    OutputText("Scenes.Rathazul.RathazulDebimboOffer.GlancesYourWayAgain".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0 && !Save.GetBool("offeredDebimboSophie"))
                    {
                        Save.SetBool("offeredDebimboSophie", true);

                        OutputText("Scenes.Rathazul.RathazulDebimboOffer.GlancesYourWayThird".Localized());

                        OutputText("Scenes.Rathazul.RathazulDebimboOffer.AwaitsClarification".Localized());

                        OutputText("Scenes.Rathazul.RathazulDebimboOffer.ChucklesWiping".Localized());
                    }

            DoNext(() => CampRathazul());
        }

        public void MakeADeBimboDraft()
        {
            ClearOutput();

            OutputText("Scenes.Rathazul.MakeADeBimboDraft.TakesTeas".Localized());

            OutputText("Scenes.Rathazul.MakeADeBimboDraft.MuttersTurns".Localized());

            Player.Gems -= 250;

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.ScholarTea), 5);

            mixologyXP += 8;

            ShowImage("item-debimbo");

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.DeBimbo), 1);

ReturnToShopMenu();
        }

        public void RathazulMakesPureHoney()
        {
            ClearOutput();

            Player.DestroyItems(ConsumableIds.BeeHoney, 1);

            Player.Gems -= 25;

            ShowImage("rathazul-lab");

            OutputText("Scenes.Rathazul.RathazulMakesPureHoney.HandsVial".Localized());

            OutputText("Scenes.Rathazul.RathazulMakesPureHoney.SaysAsTakes".Localized());

            DoNext(() => TakethatHoney());
        }

        public void TakethatHoney()
        {

            ShowImage("item-pHoney");

            OutputText("Scenes.Rathazul.TakethatHoney.ReturnsWithHoney".Localized());

            mixologyXP += 8;

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.PureHoney), 1);

ReturnToShopMenu();
        }

        public void RathazulMakesMilkPotion()
        {
            ClearOutput();

            Player.DestroyItems(ConsumableIds.Lactaid, 5);

            Player.DestroyItems(ConsumableIds.PurifiedLaBova, 2);

            Player.Gems -= 250;

            ShowImage("rathazul-lab");

            OutputText("Scenes.Rathazul.RathazulMakesMilkPotion.HandsIngredients".Localized());

            OutputText("Scenes.Rathazul.RathazulMakesMilkPotion.SaysAsTakes".Localized());

            DoNext(() => TakethatMotion());
        }

        public void TakethatMotion()
        {

            ShowImage("item-lactaid-pro");

            OutputText("Scenes.Rathazul.TakethatMotion.ReturnsWithPotion".Localized());

            mixologyXP += 8;

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.ProLactaid), 1);

ReturnToShopMenu();
        }

        public void RathazulMakesTaurPotion()
        {
            ClearOutput();

            Player.DestroyItems(ItemIds.Equinum, 2);

            Player.DestroyItems(ConsumableIds.MinotaurBlood, 1);

            Player.Gems -= 100;

            ShowImage("rathazul-lab");

            OutputText("Scenes.Rathazul.RathazulMakesTaurPotion.HandsTwoVials".Localized());

            DoNext(() => TakethatTaurico());
        }

        public void TakethatTaurico()
        {

            ShowImage("item-taurico");

            OutputText("Scenes.Rathazul.TakethatTaurico.HandsOverTaurico".Localized());

            mixologyXP += 8;

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.Taurinum), () => ReturnToShopMenu());
        }

        public void RathazulMakesEyePotion()
        {
            ClearOutput();

            ShowImage("rathazul-lab");

            Player.DestroyItems(ConsumableIds.SweetGossamer, 2);

            Player.DestroyItems(ConsumableIds.HUMMUS_, 1);

            Player.Gems -= 100;

            OutputText("Scenes.Rathazul.RathazulMakesEyePotion.HandsTwoStrands".Localized());

            DoNext(() => TakethatOculum());
        }

        public void TakethatOculum()
        {

            ShowImage("item-taurico");

            OutputText("Scenes.Rathazul.TakethatOculum.HandsOverOculum".Localized());

            mixologyXP += 8;

            InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.OculumArachnae), () => ReturnToShopMenu());
        }

        public void RathazulMakesCockatricePotion()
        {
            ClearOutput();

            Player.DestroyItems(ConsumableIds.Reptlum, 1);

            Player.DestroyItems(ConsumableIds.GoldenSeed, 1);

            Player.Gems -= 100;

            ShowImage("rathazul-lab");

            OutputText("Scenes.Rathazul.RathazulMakesCockatricePotion.HandsOneVial".Localized());

            DoNext(() => TakethatTotrice());
        }

        public void TakethatTotrice()
        {

            ShowImage("item-totrice");

            OutputText("Scenes.Rathazul.TakethatTotrice.HandsOverTotrice".Localized());

            mixologyXP += 8;

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.TonOTrice), () => ReturnToShopMenu());
        }

        public void GrowLethiciteDefense()
        {
            ClearOutput();

            ShowImage("encounter-rathazul");

            OutputText("Scenes.Rathazul.GrowLethiciteDefense.RathazulAsks".Localized());

            OutputText("Scenes.Rathazul.GrowLethiciteDefense.UseTheMallet".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), GrowLethiciteDefenseYesYesYes);

            AddButton(1, "Common.Button.No".Localized(), GrowLethiciteDefenseGuessNot);
        }

        public void GrowLethiciteDefenseYesYesYes()
        {
            ClearOutput();

            ShowImage("rathazul-canopy");

            OutputText("Scenes.Rathazul.GrowLethiciteDefenseYesYesYes.ProducesMallet".Localized());

            OutputText("Scenes.Rathazul.GrowLethiciteDefenseYesYesYes.WarnsBefore".Localized());

            Player.CreateStatusEffect(StatusEffectIds.DefenseCanopy, 0, 0, 0, 0);

            Player.AddKeyValue("Marae's Lethicite", 1, -1);

            DoNext(PlayerMenu);
        }

        public void GrowLethiciteDefenseGuessNot()
        {
            ClearOutput();

            ShowImage("encounter-rathazul");

            OutputText("Scenes.Rathazul.GrowLethiciteDefenseGuessNot.NodsSagely".Localized());

            DoNext(() => ReturnToRathazulMenu());
        }

        public void GetThatRatAss()
        {
            ClearOutput();

            ShowImage("rathazul-accident");

            OutputText("Scenes.Rathazul.GetThatRatAss.SlidesOver".Localized());

            OutputText("Scenes.Rathazul.GetThatRatAss.LooksUpWhatever".Localized());

            OutputText("Scenes.Rathazul.GetThatRatAss.SoftLaugh".Localized());

            OutputText("Scenes.Rathazul.GetThatRatAss.MomentSilence".Localized());

            OutputText("Scenes.Rathazul.GetThatRatAss.NoWay".Localized());

            OutputText("Scenes.Rathazul.GetThatRatAss.StepsBack".Localized());

            OutputText("Scenes.Rathazul.GetThatRatAss.StepsInBowl".Localized());

            OutputText("Scenes.Rathazul.GetThatRatAss.GetsUpSlowly".Localized());

            OutputText("Scenes.Rathazul.GetThatRatAss.HourLaterMuster".Localized());

            OutputText("Scenes.Rathazul.GetThatRatAss.NoAndDontAsk".Localized());

            OutputText("Scenes.Rathazul.GetThatRatAss.DramaQueen".Localized());

            OutputText("Scenes.Rathazul.GetThatRatAss.ThinkingAboutRat".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void IdLiddellium()
        {
            ClearOutput();

            OutputText("Scenes.Rathazul.IdLiddellium.PresentsPhial".Localized());

            OutputText("Scenes.Rathazul.IdLiddellium.MuttersFiddling".Localized());

            if (Player.HasPerk(PerkIds.HistoryAlchemist))
                OutputText("Scenes.Rathazul.IdLiddellium.AlchemyBackHome".Localized());

            OutputText("Scenes.Rathazul.IdLiddellium.DoesShortTests".Localized());

            OutputText("Scenes.Rathazul.IdLiddellium.AnyWayFinding".Localized());

            OutputText("Scenes.Rathazul.IdLiddellium.LiveSubject".Localized());

            ReturnToRathazulMenu();
        }

        public void RathazulBear()
        {
            ClearOutput();

            OutputText("Scenes.Rathazul.RathazulBear.DigsInBag".Localized());

            OutputText("Scenes.Rathazul.RathazulBear.TakesBear".Localized());

            OutputText("Scenes.Rathazul.RathazulBear.NeedsWantsOthers".Localized());

            if (Player.IsChild())
                OutputText("Scenes.Rathazul.RathazulBear.PlacesHand".Localized());
            else
                OutputText("Scenes.Rathazul.RathazulBear.SitsBack".Localized());

            OutputText("Scenes.Rathazul.RathazulBear.CoughsReadjusts".Localized());

            Player.ConsumeItem(UseableIds.TeddyBear);

            Save.SetBool("giftedBear", true);

            DoNext(PlayerMenu);
        }

        public void AskRathazulAboutNobleShirt()
        {
            ClearOutput();

            RegisterMacro("shirtStage", Undergarments.SEA_SHIRT.Save.GetInt("armorStage", 0));

            OutputText("Scenes.Rathazul.AskRathazulAboutNobleShirt.BeingDirect".Localized());

            OutputText("Scenes.Rathazul.AskRathazulAboutNobleShirt.TouchesShirt".Localized());

            OutputText("Scenes.Rathazul.AskRathazulAboutNobleShirt.MinutesPass".Localized());

            OutputText("Scenes.Rathazul.AskRathazulAboutNobleShirt.BrowsRaise".Localized());

            OutputText("Scenes.Rathazul.AskRathazulAboutNobleShirt.AllTheTime".Localized());

            Save.SetInt("rathazulAsked", 1);

            DoNext(() => ReturnToCamp(1));
        }

    }
}