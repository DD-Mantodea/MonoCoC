using MonoCoC.Attributes;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.World
{
    /// <summary>
    /// 原版 scenes/dungeons/DungeonCore.hx:118-156 的地牢「已通关」判定与 Deep Cave 发现条件。
    /// 原版调用点：Camp.hx:3379/3391/4030（营地/地图显示）、GooArmor.hx:35、
    /// HighMountains.hx:93 与 DeepWoods.hx:113 的 when 条件。
    /// </summary>
    public static class DungeonClearChecks
    {
        /// <summary>原版 DungeonCore.hx:118 checkFactoryClear()。</summary>
        public static bool FactoryClear()
        {
            var flags = CoC.Instance.Flags;

            return flags.Get(GameFlagIds.FactoryShutdown) > 0

                && flags.Get(GameFlagIds.FactorySuccubusDefeated) > 0

                && (flags.Get(GameFlagIds.FactoryIncubusDefeated) > 0 || flags.Get(GameFlagIds.D3Discovered) > 0)

                && flags.Get(GameFlagIds.FactoryOmnibusDefeated) > 0;
        }

        /// <summary>原版 DungeonCore.hx:122 checkDeepCaveClear()。</summary>
        public static bool DeepCaveClear()
        {
            var flags = CoC.Instance.Flags;

            var player = CoC.Instance.Player;

            return flags.Get(GameFlagIds.ZetazImpHordeDefeated) > 0

                && flags.Get(GameFlagIds.ZetazFungusRoomDefeated) > 0

                && flags.Get(GameFlagIds.FreedVala) == 1

                && player.HasKeyItem("Zetaz's Map");
        }

        /// <summary>原版 DungeonCore.hx:126 checkSandCaveClear()。</summary>
        public static bool SandCaveClear()
        {
            var flags = CoC.Instance.Flags;

            return (flags.Get(GameFlagIds.EssrayleEscapedDungeon) > 0 || flags.Get(GameFlagIds.MetEssy) == 0)

                && (flags.Get(GameFlagIds.SandWitchesFriendly) > 0 || flags.Get(GameFlagIds.SandWitchesCowed) > 0);
        }

        /// <summary>原版 DungeonCore.hx:130 checkPhoenixTowerClear()。</summary>
        public static bool PhoenixTowerClear()
        {
            var flags = CoC.Instance.Flags;

            return flags.Get(GameFlagIds.HarpyQueenExecuted) != 0

                && flags.Get(GameFlagIds.HelHarpiesDefeated) > 0

                && flags.Get(GameFlagIds.HelPhoenixesDefeated) > 0

                && flags.Get(GameFlagIds.HelBrigidDefeated) > 0;
        }

        /// <summary>原版 DungeonCore.hx:134 checkLethiceStrongholdClear()。</summary>
        public static bool LethiceStrongholdClear()
        {
            var flags = CoC.Instance.Flags;

            return flags.Get(GameFlagIds.D3MirrorsShattered) > 0

                && flags.Get(GameFlagIds.D3JeanClaudeDefeated) > 0

                && flags.Get(GameFlagIds.D3GardenerDefeated) > 0

                && flags.Get(GameFlagIds.D3CentaurDefeated) > 0

                && flags.Get(GameFlagIds.LethiceDefeated) > 0;
        }

        /// <summary>原版 DungeonCore.hx:138 checkManorClear()：MANOR_PROGRESS 第 7 位。</summary>
        public static bool ManorClear() => (CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 128) > 0;

        /// <summary>原版 DungeonCore.hx:142 checkTowerDeceptionClear()：WIZARD_TOWER_PROGRESS 第 8 位。</summary>
        public static bool TowerDeceptionClear() =>

            (CoC.Instance.Flags.Get(GameFlagIds.WizardTowerProgress) & 256) > 0;

        /// <summary>原版 DungeonCore.hx:150 canFindDeepCave()。</summary>
        public static bool CanFindDeepCave()
        {
            var flags = CoC.Instance.Flags;

            return flags.Get(GameFlagIds.DiscoveredDungeon2Zetaz) == 0

                && flags.Get(GameFlagIds.FactoryShutdown) > 0;
        }

        // ---- 供地牢房间 lockedBy 条件使用的注册项 ----
        [Register("coc:deep_cave_clear")]
        public static bool DeepCaveClearCondition(DungeonContext context) => DeepCaveClear();

        [Register("coc:can_find_deep_cave")]
        public static bool CanFindDeepCaveCondition(DungeonContext context) => CanFindDeepCave();

        [Register("coc:phoenix_tower_clear")]
        public static bool PhoenixTowerClearCondition(DungeonContext context) => PhoenixTowerClear();

        [Register("coc:manor_clear")]
        public static bool ManorClearCondition(DungeonContext context) => ManorClear();

        [Register("coc:sand_cave_clear")]
        public static bool SandCaveClearCondition(DungeonContext context) => SandCaveClear();

        [Register("coc:lethice_stronghold_clear")]
        public static bool LethiceClearCondition(DungeonContext context) => LethiceStrongholdClear();

        [Register("coc:tower_deception_clear")]
        public static bool TowerClearCondition(DungeonContext context) => TowerDeceptionClear();

        [Register("coc:factory_clear")]
        public static bool FactoryClearCondition(DungeonContext context) => FactoryClear();
    }
}