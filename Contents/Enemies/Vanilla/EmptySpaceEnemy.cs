using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    /// <summary>
    /// 原版 EmptySpace.hx:9：占位用的「无敌」怪物（40 级、15 万额外生命、
    /// 永久 immovable / 毒免 / 晕免 / 无敌，且 doAI 为空 → 永远不行动）。
    /// </summary>
    [Register<EmptySpaceEnemy>("coc:empty_space")]
    public class EmptySpaceEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 :13-20 体征与胸部
            CreateBreastRow(4);

            CreateVagina(false, 2, 2);

            // 原版 :21 永不掉落
            Gems = 2500;

            // 原版 :22 ignoreLust：永不因欲望落败
            IgnoreLust = true;

            // 原版 :23-37 基础数值与抗性
            Level = 40;

            Stats.AddModifier(GameStats.MaxHealth, Contents.Stats.ModifierOperation.Addition,

                () => 150000f, new Stats.ModifierSources.TemporarySource(System.Guid.NewGuid()));

            WeaponName = "spectral sword";

            WeaponVerb = "slash";

            WeaponAttack = 0;

            ArmorName = "";

            Lust = 30;

            AdditionalExperience = 3200;

            // 原版 :26 永不被击退、毒免、晕免、无敌
            CreatePerk(PerkIds.Immovable);

            CreatePerk(PerkIds.PoisonImmune);

            CreatePerk(PerkIds.StunImmune);

            CreatePerk(PerkIds.Invincible);

            // 原版 :31 neverAct = true

        }

        /// <summary>原版 EmptySpace.hx:41 doAI() 为空实现——这个占位怪物从不行动。</summary>
        public override void PerformCombatAction()
        {
        }
    }
}