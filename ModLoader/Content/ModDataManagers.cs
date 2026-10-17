using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Ensures all vanilla content managers exist before mod data is applied.
    /// Lazy ManagerInstance singletons would otherwise miss data for managers
    /// that are only constructed later by gameplay code.
    /// </summary>
    public static class ModDataManagers
    {
        private static bool _initialized;

        public static void EnsureLoaded()
        {
            if (_initialized)
                return;

            _initialized = true;

            _ = ManagerInstance<AchievementManager>.Instance;
            _ = ManagerInstance<AreaManager>.Instance;
            _ = ManagerInstance<BodyPartManager>.Instance;
            _ = ManagerInstance<CabinFurnitureManager>.Instance;
            _ = ManagerInstance<CabinResourceManager>.Instance;
            _ = ManagerInstance<CabinStageManager>.Instance;
            _ = ManagerInstance<CharCreationRulesManager>.Instance;
            _ = ManagerInstance<CodexManager>.Instance;
            _ = ManagerInstance<CombatAbilityManager>.Instance;
            _ = ManagerInstance<ConsumableEffectManager>.Instance;
            _ = ManagerInstance<DamageTypeManager>.Instance;
            _ = ManagerInstance<DungeonManager>.Instance;
            _ = ManagerInstance<EffectManager>.Instance;
            _ = ManagerInstance<EnemyManager>.Instance;
            _ = ManagerInstance<ItemBehaviorManager>.Instance;
            _ = ManagerInstance<ItemManager>.Instance;
            _ = ManagerInstance<NpcManager>.Instance;
            _ = ManagerInstance<PerkManager>.Instance;
            _ = ManagerInstance<PregnancyTypeManager>.Instance;
            _ = ManagerInstance<RaceColorManager>.Instance;
            _ = ManagerInstance<RaceManager>.Instance;
            _ = ManagerInstance<StatTypeManager>.Instance;
            _ = ManagerInstance<StorageManager>.Instance;
            _ = ManagerInstance<TagRuleManager>.Instance;
        }
    }
}
