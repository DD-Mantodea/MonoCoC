using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Dungeons
{

    // 房间描述与本轮已接的剧情：Sean 三选一/商店、Vala 三分支、Zetaz 房间、虫群大厅、门禁检查。
    public static class DeepCaveRooms
    {

        [Register("coc:zetaz_door_unlocked")]
        public static bool ZetazDoorUnlocked(DungeonContext context) =>

            CoC.Instance.Flags.Get(GameFlagIds.ZetazDoorUnlocked) > 0;

        [Register("coc:deep_cave_defeat")]
        public static void Defeat(DungeonContext context)
        {
            context.Write("Dungeons.Defeat.DeepCave".Localized());

            context.LeaveDungeon();
        }

        [Register("coc:deep_cave_entrance")]
        public static void Entrance(DungeonContext context)
        {
            context.Write("Scenes.DeepCave.RoomEntrance.CaveEntranceHeading".Localized());

            context.Write("Scenes.DeepCave.RoomEntrance.EntranceFarBigger".Localized());

            var vendor = CoC.Instance.Flags.Get(GameFlagIds.ZetazLairDemonVendorPresent);

            if (vendor == 0)
                Scenes.Scene.AddButton(0, "Common.Button.Investigate".Localized(), () => SeanIntro(context));
            else if (vendor > 0)
                Scenes.Scene.AddButton(0, "Common.Button.Shop".Localized(), () => SeanShop(context));
        }

        public static void SeanIntro(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DeepCave.Sean.Intro".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Deal".Localized(), () =>
            {
                CoC.Instance.Flags.Set(GameFlagIds.ZetazLairDemonVendorPresent, 1);

                context.Write("Scenes.DeepCave.Sean.Deal".Localized());

                Scenes.Scene.ClearButtons();

                Scenes.Scene.AddButton(14, "Common.Button.Continue".Localized(), () => context.Refresh());
            });

            Scenes.Scene.AddButton(1, "Common.Button.NoDeal".Localized(), () =>
            {
                CoC.Instance.Flags.Set(GameFlagIds.ZetazLairDemonVendorPresent, -1);

                context.Write("Scenes.DeepCave.Sean.NoDeal".Localized());

                Scenes.Scene.ClearButtons();

                Scenes.Scene.AddButton(14, "Common.Button.Continue".Localized(), () => context.Refresh());
            });

            Scenes.Scene.AddButton(2, "Common.Button.NotNow".Localized(), () => context.Refresh());
        }

        public static void SeanShop(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DeepCave.Sean.ShopGreeting".Localized());

            Identifier[] stock =

            [
                ConsumableIds.SensitivityDraft,

                ItemIds.Reducto,

                ConsumableIds.Liddellium
            ];

            for (var index = 0; index < stock.Length; index++)
            {
                var item = ItemRegistry.Create(stock[index]);

                // 原版 DeepCave.hx incubusTransact()：Liddellium 按 2 倍价，其它按 3 倍价
                var multiplier = stock[index] == ConsumableIds.Liddellium ? 2f : 3f;

                var price = Math.Max(1, (int)Math.Round(item.Value * multiplier));

                Scenes.Scene.AddButton(index, $"{item.Name} - {price}g",

                    () => SeanBuy(context, item, price));
            }

            Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => context.Refresh());
        }

        public static void SeanBuy(DungeonContext context, Item item, int price)
        {
            var player = CoC.Instance.Player;

            if (player.Gems < price)
            {
                context.Write("Scenes.DeepCave.Sean.NoGems".Localized());

                return;
            }

            player.Gems -= price;

            player.Inventory.Add(item, 1);

            context.Write("Scenes.DeepCave.Sean.Bought".Localized(item.Name));

            SeanShop(context);
        }

        [Register("coc:deep_cave_tunnel")]
        public static void Tunnel(DungeonContext context)
        {
            context.Write("Scenes.DeepCave.RoomTunnel.CaveTunnelHeading".Localized());

            context.Write("Scenes.DeepCave.RoomTunnel.TunnelSlantsDown".Localized());
        }

        [Register("coc:deep_cave_gathering_hall")]
        public static void GatheringHall(DungeonContext context)
        {
            context.Write("Scenes.DeepCave.RoomGatheringHall.GatheringHallHeading".Localized());

            context.Write("Scenes.DeepCave.RoomGatheringHall.DiningHall".Localized());

            var flag = CoC.Instance.Flags.Get(GameFlagIds.ZetazImpHordeDefeated);

            if (flag == 0)
            {
                context.Write("Scenes.DeepCave.RoomGatheringHall.TwoDozenImps".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.Fight".Localized(),

                    () => context.StartCombat(EnemyRegistry.Create(EnemyIds.ImpHorde)));
            }
            else
                context.Write("Scenes.DeepCave.RoomGatheringHall.ImpsCleared".Localized());
        }

        [Register("coc:deep_cave_fungus_cavern")]
        public static void FungusCavern(DungeonContext context)
        {
            Scenes.Scene.Go<DeepCaveFungusScene>();
        }

        [Register("coc:deep_cave_torture_room")]
        public static void TortureRoom(DungeonContext context)
        {
            context.Write("Scenes.DeepCave.RoomTortureRoom.FilthyTortureRoomHeading".Localized());

            context.Write("Scenes.DeepCave.RoomTortureRoom.DankRoom".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FreedVala) == 0)
            {
                context.Write("Scenes.DeepCave.RoomTortureRoom.ValaChainedIntro".Localized());

                Scenes.Scene.AddButton(0, "Scenes.Lumberjack.Button.Free".Localized(), () => FreeVala(context));
            }
            else
                context.Write("Scenes.DeepCave.RoomTortureRoom.ValaGone".Localized());
        }

        public static void FreeVala(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DeepCave.Vala.FreeIntro1".Localized());

            context.Write("Scenes.DeepCave.Vala.FreeIntro2".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.FixHer".Localized(), () => HealVala(context));

            Scenes.Scene.AddButton(3, "Common.Button.Reject".Localized(), () => RejectVala(context));
        }

        public static void HealVala(DungeonContext context)
        {
            var player = CoC.Instance.Player;

            Identifier[] cures = [ConsumableIds.PureHoney, ConsumableIds.PurityPhilter, UseableIds.PurePearl];

            Item? cure = null;

            foreach (var id in cures)
            {
                var item = ItemRegistry.Create(id);

                if (player.HasItem(item))
                {
                    cure = item;

                    break;
                }
            }

            if (cure == null)
            {
                context.Write("Scenes.DeepCave.Vala.NoCure".Localized());

                return;
            }

            player.ConsumeItem(cure, 1);

            CoC.Instance.Flags.Set(GameFlagIds.FreedVala, 1);

            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DeepCave.Vala.HealText".Localized());

            context.Write("Scenes.DeepCave.Vala.HealExit".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Continue".Localized(), () => context.Refresh());
        }

        public static void RejectVala(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            context.Write("Scenes.DeepCave.Vala.RejectText".Localized());

            context.StartCombat(EnemyRegistry.Create(EnemyIds.Vala));
        }

        [Register("coc:deep_cave_secret_passage")]
        public static void SecretPassage(DungeonContext context)
        {
            context.Write("Scenes.DeepCave.RoomSecretPassage.SecretTunnelHeading".Localized());

            context.Write("Scenes.DeepCave.RoomSecretPassage.LeastLivablePassage".Localized());

            context.Write("Scenes.DeepCave.RoomSecretPassage.GoingToFindZetaz".Localized());
        }

        [Register("coc:deep_cave_zetaz_chamber")]
        public static void ZetazChamber(DungeonContext context)
        {
            context.Write("Scenes.DeepCave.RoomZetazChamber.ZetazChambersHeading".Localized());

            context.Write("Scenes.DeepCave.RoomZetazChamber.MostLavishRoom".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ZetazDoorUnlocked) == 0)
            {
                context.Write("Scenes.DeepCave.RoomZetazChamber.BoltHoldingDoor".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.ZetazDoorUnlocked, 1);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.DefeatedZetaz) == 0)
            {
                context.Write("Scenes.DeepCave.RoomZetazChamber.ZetazConfronts".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.Fight".Localized(),

                    () => context.StartCombat(EnemyRegistry.Create(EnemyIds.Zetaz)));
            }
            else
                context.Write("Scenes.DeepCave.RoomZetazChamber.ZetazGone".Localized());
        }

        /// <summary>
        /// 原版 DeepCave.hx:115 takeBondageStraps()：拿走束缚皮带，计数 +1 并放入背包。
        /// </summary>
        public static void TakeBondageStraps(DungeonContext context)
        {
            var flags = CoC.Instance.Flags;

            flags.Set(GameFlagIds.ZetazLairTookBondageStraps, flags.Get(GameFlagIds.ZetazLairTookBondageStraps) + 1);

            Scenes.Vanilla.InventoryScene.TakeItem(ItemRegistry.Create(ArmorIds.BondageStraps), () => context.Refresh());
        }

        /// <summary>原版 DeepCave.hx:95 checkDoor1()：门未解锁则退回大厅。</summary>
        public static void CheckDoor1(DungeonContext context)
        {
            Scenes.Scene.ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.ZetazDoorUnlocked) <= 0)
            {
                context.Write("Scenes.DeepCave.Door.WontBudge".Localized());

                GatheringHall(context);
            }
            else
                ZetazChamber(context);
        }

        /// <summary>原版 DeepCave.hx:105 checkExit()：NG+3 且未击杀 Zetaz 时出口被魔法封闭。</summary>
        public static void CheckExit(DungeonContext context)
        {
            var flags = CoC.Instance.Flags;

            Scenes.Scene.ClearOutput();

            if (flags.Get(GameFlagIds.NewGamePlusLevel) >= 3 && flags.Get(GameFlagIds.ZetazDefeatedAndKilled) <= 0)
            {
                context.Write("Scenes.DeepCave.Door.LockedByMagic".Localized());

                GatheringHall(context);
            }
            else
                Tunnel(context);
        }
    }
}