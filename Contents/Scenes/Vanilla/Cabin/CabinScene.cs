using MonoCoC.Contents.Flags;
using MonoCoC.Contents.World.Cabin;
using MonoCoC.Extensions;
using System.Linq;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Cabin
{
    [Register<CabinProgressScene>("coc:progress")]
    public class CabinProgressScene : Scene
    {
        public override void Entry()
        {
            EnsureState();

            var stage = CurrentStage();

            ClearOutput();

            ClearButtons();

            DisplayHeader("Scenes.Cabin.Title".Localized());

            if (stage == null)
            {
                OutputText("Scenes.Cabin.Built".Localized());

                AddButton(0, "Scenes.Cabin.Enter".Localized(), () => Go<CabinScene>());

                AddButton(14, "Common.Back".Localized(), () => Go<CampScene>());

                return;
            }

            OutputText(stage.NameKey.Localized());

            OutputText(stage.DescriptionKey.Localized());

            foreach (var cost in stage.Costs)
                OutputText("Cabin.CostLine".Localized(cost.Resource.NameKey.Localized(), cost.Resource.Get(Player), cost.Amount));

            AddButton(0, "Scenes.Cabin.Build".Localized(), Build);

            if (Flags.Get(GameFlagIds.CampBuiltCabin) > 0)
                AddButton(1, "Scenes.Cabin.Enter".Localized(), () => Go<CabinScene>());

            AddButton(14, "Common.Back".Localized(), () => Go<CampScene>());
        }

        public void Build()
        {
            EnsureState();

            var stage = CurrentStage();

            if (stage == null)
            {
                Go<CabinProgressScene>();

                return;
            }

            var context = new CabinBuildContext
            {
                Player = Player,

                Flags = Flags,

                State = Player.Cabin,

                Stage = stage
            };

            if (stage.Requirements.Any(requirement => !requirement.IsMet(context)))
            {
                OutputText("Scenes.Cabin.Requirements".Localized());

                return;
            }

            if (stage.Costs.Any(cost => cost.Resource.Get(Player) < cost.Amount))
            {
                OutputText("Scenes.Cabin.NotEnough".Localized());

                return;
            }

            foreach (var cost in stage.Costs)
                cost.Resource.Add(Player, -cost.Amount);

            Player.Cabin.Complete(stage.Id);

            var next = CabinStageRegistry.Next(stage.Id);

            if (next == null)
            {
                Flags.Set(GameFlagIds.CampBuiltCabin, 1);

                Player.Cabin.CurrentStage = stage.Id;
            }
            else
            {
                Player.Cabin.CurrentStage = next.Id;

                Flags.Set(GameFlagIds.CampCabinProgress, next.Order);
            }

            CabinHandlerRegistry.Invoke(stage.Handler, context);

            Player.ChangeFatigue(stage.Fatigue);

            View.Time.Minutes = 0;

            if (stage.Hours > 0)
                View.Time.PassTime(stage.Hours);

            Go<CabinProgressScene>();
        }

        public void EnsureState()
        {
            var state = Player.Cabin;

            if (!string.IsNullOrEmpty(state.CurrentStage.Path) &&
                CabinStageRegistry.Stages.Has(state.CurrentStage))

                return;

            var ordered = CabinStageRegistry.Ordered();

            if (ordered.Count == 0)
                return;

            var progress = Flags.Get(GameFlagIds.CampCabinProgress);

            if (progress >= ordered.Count)
            {
                Flags.Set(GameFlagIds.CampBuiltCabin, 1);

                foreach (var stage in ordered)
                    state.Complete(stage.Id);

                state.CurrentStage = ordered[^1].Id;

                return;
            }

            var index = progress <= 0 ? 0 : progress - 1;

            state.CurrentStage = ordered[index].Id;

            Flags.Set(GameFlagIds.CampCabinProgress, ordered[index].Order);
        }

        public CabinStage? CurrentStage()
        {
            var state = Player.Cabin;

            if (!string.IsNullOrEmpty(state.CurrentStage.Path) &&
                CabinStageRegistry.Stages.TryGet(state.CurrentStage, out var stage))

                return stage;

            return CabinStageRegistry.First();
        }
    }

    [Register<CabinScene>("coc:cabin")]
    public class CabinScene : Scene
    {
        /// <summary>原版 Cabin.hx:143 bedBearDesc()：按床上熊的数量输出描述。</summary>
        public virtual void BedBearDesc()
        {
            var bears = Flags.Get(GameFlagIds.BedBears);

            var key = bears switch
            {
                0 => "Scenes.Cabin.CabinBear.DescNone",

                1 => "Scenes.Cabin.CabinBear.DescOne",

                2 => "Scenes.Cabin.CabinBear.DescTwo",

                <= 5 => "Scenes.Cabin.CabinBear.DescSeveral",

                <= 9 => "Scenes.Cabin.CabinBear.DescCovered",

                _ => "Scenes.Cabin.CabinBear.DescHarem",
            };

            OutputText(key.Localized());
        }

        /// <summary>原版 Cabin.hx:12 enterCabin()：木屋内部。</summary>
        public override void Entry()
        {
            ClearButtons();

            ClearOutput();

            OutputText("Scenes.Cabin.EnterCabin.CabinBanner".Localized());

            OutputText("Scenes.Cabin.EnterCabin.InYourCabin".Localized());

            if (World.CabinInterior.HasBed)
            {
                OutputText("Scenes.Cabin.EnterCabin.SWord".Localized());

                if (Flags.Get(GameFlagIds.BedBears) > 0) BedBearDesc();

                if (Player.HasKeyItem("Camp - Chest")
                    || Player.HasKeyItem("Camp - Murky Chest")

                    || Player.HasKeyItem("Camp - Ornate Chest"))

                    OutputText("Scenes.Cabin.EnterCabin.StorageChest".Localized());
            }

            if (World.CabinInterior.HasNightstand)
            {
                OutputText("Scenes.Cabin.EnterCabin.Nightstand".Localized());

                if (Flags.Get(GameFlagIds.BenoitClockBought) > 0)
                    OutputText("Scenes.Cabin.EnterCabin.AlarmClock".Localized());

                if (Player.HasKeyItem("Equipment Storage - Jewelry Box")
                    && !World.CabinInterior.HasDresser)

                    OutputText("Scenes.Cabin.EnterCabin.JewelryBox".Localized());
            }

            if (World.CabinInterior.HasDresser)
            {
                OutputText("Scenes.Cabin.EnterCabin.DresserWall".Localized());

                if (Player.HasKeyItem("Equipment Storage - Jewelry Box"))
                    OutputText("Scenes.Cabin.EnterCabin.JewelryStored".Localized());
            }

            if (World.CabinInterior.HasTable)
            {
                OutputText("Scenes.Cabin.EnterCabin.TableWindow".Localized());

                if (World.CabinInterior.HasTableChair1
                    && !World.CabinInterior.HasTableChair2)

                    OutputText("Scenes.Cabin.EnterCabin.ChairTable".Localized());

                if (World.CabinInterior.HasTableChair1
                    && World.CabinInterior.HasTableChair2)

                    OutputText("Scenes.Cabin.EnterCabin.TwoChairs".Localized());
            }

            if (World.CabinInterior.HasBookshelf)
                OutputText("Scenes.Cabin.EnterCabin.CornerOpposite".Localized());

            if (World.CabinInterior.HasDesk)
            {
                OutputText(World.CabinInterior.HasBookshelf

                    ? "Scenes.Cabin.EnterCabin.DeskBookshelf".Localized()

                    : "Scenes.Cabin.EnterCabin.CornerOppositeAgain".Localized());

                OutputText("Scenes.Cabin.EnterCabin.DrawerSupplies".Localized());
            }

            if (Areas.Forest.KitsuneScene.StatueLocation != "")
                OutputText("Scenes.Cabin.EnterCabin.GoldenStatue".Localized());

            OutputText("Scenes.Cabin.EnterCabin.WhatDoQuestion".Localized());

            // Pet 菜单按原版放在 Camp 主界面，不在 Cabin
            AddButton(2, "Common.Button.Inventory".Localized(),

                () => InventoryScene.InventoryMenu(), "Scenes.Cabin.Hint.Inventory".Localized());

            AddButton(3, "Common.Button.Storage".Localized(),

                () => StorageScene.StorageMenu(), "Scenes.Cabin.Hint.Storage".Localized());

            AddButton(4, "Common.Button.Furniture".Localized(), () => Go<CabinProgressScene>(),

                "Scenes.Cabin.Hint.Furniture".Localized());

            if (World.CabinInterior.HasBookshelf)
                AddButton(5, "Common.Button.Bookshelf".Localized(), MenuStudy,

                    "Scenes.Cabin.Hint.Bookshelf".Localized());

            if (CampScene.PetsCount() > 0)
                AddButton(10, "Common.Button.Pets".Localized(), CabinPetsMenu,

                    "Scenes.Cabin.Hint.Pets".Localized());

            if (World.CabinInterior.HasBed
                && (Player.HasItem(Datas.Vanilla.UseableIds.TeddyBear)

                    || Flags.Get(GameFlagIds.BedBears) > 0))

                AddButton(6, "Common.Button.TeddyBear".Localized(), CabinBearMenu);

            if (Areas.Forest.KitsuneScene.StatueLocation != "")
            {
                AddButton(7, "Common.Button.Meditate".Localized(),

                    () => SceneInstance<Areas.Forest.KitsuneScene>.Get().MeditateLikeAKitsuneEhQuestionMark(),

                    "Scenes.Cabin.Hint.Meditate".Localized());

                // 原版 Cabin.hx:110：把金雕像收回背包，并清空雕像位置
                AddButton(8, "Common.Button.TakeStatue".Localized(), TakeStatue);
            }

            // 原版 Cabin.hx:281 menuAlarm：床头柜上买了闹钟才能设起床时间
            if (World.CabinInterior.HasNightstand
                && Flags.Get(GameFlagIds.BenoitClockBought) > 0)

                AddButton(1, "Common.Button.SetAlarm".Localized(), MenuAlarm,

                    "Scenes.Cabin.Hint.Alarm".Localized());

            // 原版 Cabin.hx:112：拥有床品（蛾丝 / 蛛丝床单）时才提供切换入口
            if (World.CabinInterior.HasBedding())
                AddButton(13, "Common.Button.BeddingMenu".Localized(), BeddingMenu);

            WaitButton();

            AddButton(14, "Common.Button.ExitCabin".Localized(), () => Go<CampScene>(),

                "Scenes.Cabin.Hint.Exit".Localized());
        }

        /// <summary>
        /// 原版 Cabin.hx:110 Take Statue：把金雕像从木屋里收回背包。
        /// </summary>
        public virtual void TakeStatue()
        {
            ClearOutput();

            Player.AddItem(Contents.Items.ItemRegistry.Create(Datas.Vanilla.UseableIds.GoldenStatue));

            Areas.Forest.KitsuneScene.StatueLocation = "";

            Entry();
        }

        /// <summary>
        /// 原版 Cabin.hx:116 waitButton()：Wait / Rest / Sleep（同槽位，后者覆盖前者）。
        /// </summary>
        public virtual void WaitButton()
        {
            AddButton(9, "Common.Button.Wait".Localized(), CampScene.Wait, "Scenes.Cabin.Hint.Wait".Localized());

            if (Player.Fatigue > 30 || Player.HpPercent <= 90)
                AddButton(9, "Common.Button.Rest".Localized(), CampScene.Rest, "Scenes.Cabin.Hint.Rest".Localized());

            if (View.Time.Hours >= 21 || View.Time.Hours < 6)
                AddButton(9, "Common.Button.Sleep".Localized(), CampScene.Sleep, "Scenes.Cabin.Hint.Sleep".Localized());
        }

        /// <summary>原版 Cabin.hx:239 cabinPetsMenu()：木屋内的宠物菜单。</summary>
        public virtual void CabinPetsMenu()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.CabinPetsMenu.InteractPets".Localized());

            if (SceneInstance<Npcs.AkkyScene>.Get().IsOwned())
            {
                var akky = SceneInstance<Npcs.AkkyScene>.Get();

                akky.name = CoC.Instance.Flags.Get(GameFlagIds.AkkyName);

                akky.MenuButton(() => Go<CabinScene>(), 0, "Cabin");
            }

            AddButton(14, "Common.Back".Localized(), () => Go<CabinScene>());
        }

        /// <summary>原版 Cabin.hx:172 cabinBearMenu()：泰迪熊菜单。</summary>
        public virtual void CabinBearMenu()
        {
            ClearOutput();

            var bears = Flags.Get(GameFlagIds.BedBears);

            var hasBear = Player.HasItem(Datas.Vanilla.UseableIds.TeddyBear);

            if (bears > 0) BedBearDesc();

            if (hasBear)
                if (bears == 0)
                    OutputText("Scenes.Cabin.CabinBearMenu.WouldYouBear".Localized());
                else if (bears >= 10)
                    OutputText("Scenes.Cabin.CabinBearMenu.NoRoomMore".Localized());
                else
                    OutputText("Scenes.Cabin.CabinBearMenu.WouldYouGive".Localized(bears));

            if (hasBear && bears < 10)
            {
                AddButton(0, "Common.Button.Yes".Localized(), CabinBearAdd);

                AddButton(1, "Common.Button.No".Localized(), () => Go<CabinScene>());
            }

            if (bears > 0) AddButton(2, "Common.Button.Take".Localized(), CabinBearTake);

            AddButton(3, "Common.Button.Snuggle".Localized(), CabinBearSnuggle);

            AddButton(14, "Common.Back".Localized(), () => Go<CabinScene>());
        }

        /// <summary>原版 Cabin.hx:198 cabinBearAdd()：把熊放到床上。</summary>
        public virtual void CabinBearAdd()
        {
            ClearOutput();

            Player.ConsumeItem(Datas.Vanilla.UseableIds.TeddyBear);

            var bears = Flags.Get(GameFlagIds.BedBears) + 1;

            Flags.Set(GameFlagIds.BedBears, bears);

            OutputText("Scenes.Cabin.CabinBearAdd.PlacesBear".Localized());

            CabinBearMenu();
        }

        /// <summary>原版 Cabin.hx:206 cabinBearTake()：把熊收回背包。</summary>
        public virtual void CabinBearTake()
        {
            ClearOutput();

            var bears = Flags.Get(GameFlagIds.BedBears) - 1;

            Flags.Set(GameFlagIds.BedBears, bears);

            Player.AddItem(Contents.Items.ItemRegistry.Create(Datas.Vanilla.UseableIds.TeddyBear));

            OutputText("Scenes.Cabin.CabinBearTake.PicksBearUp".Localized(bears));

            CabinBearMenu();
        }

        /// <summary>原版 Cabin.hx:214 cabinBearSnuggle()：抱熊。</summary>
        public virtual void CabinBearSnuggle()
        {
            ClearOutput();

            var bears = Flags.Get(GameFlagIds.BedBears);

            var key = bears switch
            {
                <= 1 => "Scenes.Cabin.CabinBearSnuggle.GetsOntoBed",

                2 => "Scenes.Cabin.CabinBearSnuggle.GetsOntoBedAgain",

                <= 9 => "Scenes.Cabin.CabinBearSnuggle.ClimbsAmongst",

                _ => "Scenes.Cabin.CabinBearSnuggle.BedLoadedBears",
            };

            OutputText(key.Localized());

            Go<CabinScene>();
        }

        /// <summary>原版 Cabin.hx:849 beddingMenu()：切换床品（普通 / 蛾丝 / 蛛丝）。</summary>
        public virtual void BeddingMenu()
        {
            ClearOutput();

            var moth = Player.HasPerk(Datas.Vanilla.PerkIds.MothBedding);

            var spider = Player.HasPerk(Datas.Vanilla.PerkIds.SpiderBedding);

            // 原版 disableIf：当前就是普通床品时不可再选
            if (moth || spider)
                AddButton(0, "Common.Button.NormalBedding".Localized(), NormalBedding,

                    "Scenes.Cabin.Bedding.HintNormal".Localized());
            else
                AddButtonDisabled(0, "Common.Button.NormalBedding".Localized(),

                    "Scenes.Cabin.Bedding.HintNormal".Localized());

            if (Player.HasKeyItem("Moth Bedding"))
                if (moth)
                    AddButtonDisabled(1, "Common.Button.MothBedding".Localized(),

                        "Scenes.Cabin.Bedding.HintMoth".Localized());
                else
                    AddButton(1, "Common.Button.MothBedding".Localized(), MothBedding,

                        "Scenes.Cabin.Bedding.HintMoth".Localized());

            if (Player.HasKeyItem("Spider-Silk Bedding"))
                if (spider)
                    AddButtonDisabled(2, "Common.Button.SpiderSilkBedding".Localized(),

                        "Scenes.Cabin.Bedding.HintSpider".Localized());
                else
                    AddButton(2, "Common.Button.SpiderSilkBedding".Localized(), SpiderBedding,

                        "Scenes.Cabin.Bedding.HintSpider".Localized());

            AddButton(14, "Common.Back".Localized(), () => Go<CabinScene>());
        }

        /// <summary>原版 Cabin.hx:861 normalBedding()。</summary>
        public virtual void NormalBedding()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.NormalBedding.PlainSheets".Localized());

            World.CabinInterior.ResetBeddingPerks();

            Entry();
        }

        /// <summary>原版 Cabin.hx:868 mothBedding()。</summary>
        public virtual void MothBedding()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.MothBedding.SilkenSheets".Localized());

            World.CabinInterior.ResetBeddingPerks();

            Player.CreatePerk(Datas.Vanilla.PerkIds.MothBedding);

            Entry();
        }

        /// <summary>原版 Cabin.hx:876 spiderBedding()。</summary>
        public virtual void SpiderBedding()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.SpiderBedding.SpiderSheets".Localized());

            World.CabinInterior.ResetBeddingPerks();

            Player.CreatePerk(Datas.Vanilla.PerkIds.SpiderBedding);

            Entry();
        }

        /// <summary>原版 Cabin.hx:281 menuAlarm()：设置闹钟起床时间。</summary>
        public virtual void MenuAlarm()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.MenuAlarm.SetHour".Localized());

            OutputText("Scenes.Cabin.MenuAlarm.NoteCircumstances".Localized());

            AddButton(0, "Common.Button.Alarm6Am".Localized(), () => SetAlarm(6));

            AddButton(1, "Common.Button.Alarm7Am".Localized(), () => SetAlarm(7));

            AddButton(2, "Common.Button.Alarm8Am".Localized(), () => SetAlarm(8));

            AddButton(3, "Common.Button.Alarm9Am".Localized(), () => SetAlarm(9));

            AddButton(14, "Common.Back".Localized(), () => Go<CabinScene>());
        }

        /// <summary>原版 Cabin.hx:294 setAlarm()。</summary>
        public virtual void SetAlarm(int timeSet = 6)
        {
            ClearOutput();

            OutputText("Scenes.Cabin.SetAlarm.AlarmSet".Localized(timeSet));

            Flags.Set(GameFlagIds.BenoitClockAlarm, timeSet);

            Entry();
        }

        /// <summary>原版 Cabin.hx:300 menuStudy()：书架。</summary>
        public virtual void MenuStudy()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.MenuStudy.BookshelfHolds".Localized());

            OutputText("Scenes.Cabin.MenuStudy.CodexBook".Localized());

            AddButton(0, "Common.Button.Codex".Localized(), () => Go<CodexScene>());

            if (Player.HasKeyItem("Dangerous Plants"))
            {
                OutputText("Scenes.Cabin.MenuStudy.DangerousPlants".Localized());

                AddButton(1, "Common.Button.DangerousPlants".Localized(), ReadDPlants);
            }

            if (Player.HasKeyItem("Traveler's Guide"))
            {
                OutputText("Scenes.Cabin.MenuStudy.TravelersGuide".Localized());

                AddButton(2, "Common.Button.TravelersGuide".Localized(), ReadTGuide);
            }

            if (Player.HasKeyItem("Hentai Comic"))
            {
                OutputText("Scenes.Cabin.MenuStudy.HentaiComic".Localized());

                AddButton(3, "Common.Button.HentaiBook".Localized(), ReadHComic);
            }

            if (Player.HasKeyItem("Yoga Guide"))
            {
                OutputText("Scenes.Cabin.MenuStudy.YogaGuide".Localized());

                AddButton(4, "Common.Button.YogaGuide".Localized(), ReadYGuide);
            }

            if (Player.HasKeyItem("Carpenter's Toolbox"))
            {
                OutputText("Scenes.Cabin.MenuStudy.CarpentersManual".Localized());

                AddButtonDisabled(5, "Common.Button.CarpentersManual".Localized());
            }

            if (Player.HasKeyItem("Izma's Book - Combat Manual"))
            {
                OutputText("Scenes.Cabin.MenuStudy.CombatManual".Localized());

                AddButton(6, "Common.Button.CombatManual".Localized(), StudyCombatManual);
            }

            if (Player.HasKeyItem("Izma's Book - Etiquette Guide"))
            {
                OutputText("Scenes.Cabin.MenuStudy.EtiquetteGuide".Localized());

                AddButton(7, "Common.Button.EtiquetteGuide".Localized(), StudyEtiquetteGuide);
            }

            if (Player.HasKeyItem("Izma's Book - Porn"))
            {
                OutputText("Scenes.Cabin.MenuStudy.Porn".Localized());

                AddButton(8, "Common.Button.Porn".Localized(), StudyPorn);
            }

            OutputText("Scenes.Cabin.MenuStudy.WhichBookQuestion".Localized());

            AddButton(14, "Common.Back".Localized(), () => Go<CabinScene>());
        }

        /// <summary>原版 Cabin.hx:357 readDPlants()。</summary>
        public virtual void ReadDPlants()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.ReadDPlants.ProcuresBook".Localized());

            OutputText("Scenes.Cabin.ReadDPlants.FortunateQuite".Localized());

            OutputText("Scenes.Cabin.ReadDPlants.ClosesBook".Localized());

            Player.ApplyStats(new Stats.DynStat(Datas.Vanilla.GameStats.Intelligence, 0.1f));

            ReturnToCamp(1);
        }

        /// <summary>原版 Cabin.hx:368 readTGuide()：只读文本，不消耗时间。</summary>
        public virtual void ReadTGuide()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.ReadTGuide.AnotherLook".Localized());

            OutputText("Scenes.Cabin.ReadTGuide.CrazyMerchant".Localized());

            OutputText("Scenes.Cabin.ReadTGuide.SignsWonders".Localized());

            AddButton(0, "Common.Button.Next".Localized(), () => Go<CabinScene>());
        }

        /// <summary>原版 Cabin.hx:377 readHComic()。</summary>
        public virtual void ReadHComic()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.ReadHComic.ReachesShelf".Localized());

            OutputText("Scenes.Cabin.ReadHComic.PerusesBook".Localized());

            OutputText("Scenes.Cabin.ReadHComic.StillRemembered".Localized());

            OutputText("Scenes.Cabin.ReadHComic.PutsComicBack".Localized());

            Player.ApplyStats(
                new Stats.DynStat(Datas.Vanilla.GameStats.Libido, 1f),

                new Stats.DynStat(Datas.Vanilla.GameStats.Lust, 15f));

            ReturnToCamp(1);
        }

        /// <summary>原版 Cabin.hx:388 readYGuide()：按 Cotton 的瑜伽进度分支。</summary>
        public virtual void ReadYGuide()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.ReadYGuide.ReachesShelf".Localized());

            var tracker = Flags.Get(GameFlagIds.CottonUnusualYogaBookTracker);

            if (tracker == 1)
            {
                OutputText("Scenes.Cabin.ReadYGuide.FirstFewPages".Localized());

                OutputText("Scenes.Cabin.ReadYGuide.BringToCotton".Localized());
            }
            else if (tracker == 3)
            {
                var yoga = Flags.Get(GameFlagIds.TimesHadYoga);

                if (yoga == 0)
                    OutputText("Scenes.Cabin.ReadYGuide.CottonLearn".Localized());
                else if (yoga == 1)
                    OutputText("Scenes.Cabin.ReadYGuide.FirstYogaSession".Localized());
                else if (yoga is >= 2 and < 5)
                {
                    OutputText("Scenes.Cabin.ReadYGuide.FewLessons".Localized());

                    Player.ModTone(52, 1);

                    Player.ChangeFatigue(20);

                    Player.ApplyStats(new Stats.DynStat(Datas.Vanilla.GameStats.Speed, 0.5f));
                }
                else
                {
                    OutputText("Scenes.Cabin.ReadYGuide.ManySessions".Localized());

                    Player.ModTone(52, 1);

                    Player.ChangeFatigue(20);

                    Player.ApplyStats(new Stats.DynStat(Datas.Vanilla.GameStats.Speed, 1.1f));
                }
            }

            ReturnToCamp(1);
        }

        /// <summary>原版 Cabin.hx:421 studyCombatManual()：随机三分支属性成长。</summary>
        public virtual void StudyCombatManual()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.StudyCombatManual.TakesBook".Localized());

            var atDesk = World.CabinInterior.HasDesk

                         && World.CabinInterior.HasDeskChair;

            OutputText(atDesk

                ? "Scenes.Cabin.StudyCombatManual.SitsChair".Localized()

                : "Scenes.Cabin.StudyCombatManual.SitsBed".Localized());

            OutputText("Scenes.Cabin.StudyCombatManual.OpensStudies".Localized());

            var choice = System.Random.Shared.Next(3);

            if (choice == 0)
            {
                OutputText("Scenes.Cabin.StudyCombatManual.LearnsStances".Localized());

                Player.ApplyStats(new Stats.DynStat(Datas.Vanilla.GameStats.Toughness, 2f));
            }
            else if (choice == 1)
            {
                OutputText("Scenes.Cabin.StudyCombatManual.QuickSkim".Localized());

                Player.ApplyStats(new Stats.DynStat(Datas.Vanilla.GameStats.Intelligence, 2f));
            }
            else
            {
                OutputText("Scenes.Cabin.StudyCombatManual.ReadThrough".Localized());

                Player.ApplyStats(new Stats.DynStat(Datas.Vanilla.GameStats.Strength, 2f));
            }

            OutputText("Scenes.Cabin.StudyCombatManual.FinishedLearning".Localized());

            ReturnToCamp(1);
        }

        /// <summary>原版 Cabin.hx:452 studyEtiquetteGuide()。</summary>
        public virtual void StudyEtiquetteGuide()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.StudyEtiquetteGuide.TakesBook".Localized());

            var atDesk = World.CabinInterior.HasDesk

                         && World.CabinInterior.HasDeskChair;

            OutputText(atDesk

                ? "Scenes.Cabin.StudyEtiquetteGuide.SitsChair".Localized()

                : "Scenes.Cabin.StudyEtiquetteGuide.SitsBed".Localized());

            OutputText("Scenes.Cabin.StudyEtiquetteGuide.OpensStudies".Localized());

            OutputText("Scenes.Cabin.StudyEtiquetteGuide.PerusesStrange".Localized());

            OutputText("Scenes.Cabin.StudyEtiquetteGuide.FrillyBook".Localized());

            Player.ApplyStats(
                new Stats.DynStat(Datas.Vanilla.GameStats.Libido, -2f),

                new Stats.DynStat(Datas.Vanilla.GameStats.Corruption, -2f));

            ReturnToCamp(1);
        }

        /// <summary>原版 Cabin.hx:474 studyPorn()。</summary>
        public virtual void StudyPorn()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.StudyPorn.TakesPornBook".Localized());

            OutputText("Scenes.Cabin.StudyPorn.WetsLips".Localized());

            OutputText("Scenes.Cabin.StudyPorn.MindFilled".Localized());

            Player.ApplyStats(
                new Stats.DynStat(Datas.Vanilla.GameStats.Libido, 2f),

                new Stats.DynStat(Datas.Vanilla.GameStats.Lust, 20f + Player.Libido / 10f));

            ReturnToCamp(1);
        }
    }
}