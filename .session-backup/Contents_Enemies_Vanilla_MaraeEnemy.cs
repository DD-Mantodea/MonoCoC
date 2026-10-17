using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 Marae extends Monster（src/classes/scenes/places/boat/Marae.hx）
    // 形态由 FACTORY_SHUTDOWN 决定：1 = 未腐蚀（纯净）/ 2 = 腐蚀 / 其它 = 未腐蚀。
    // 只移植类本体；defeated/won 依赖 MaraeScene（未移植，原版 Marae.hx:62-68）。
    [Register("coc:marae")]
    public class MaraeEnemy : Enemy
    {
        private int _factoryShutdown;

        private static readonly Identifier AttackId = new("coc", "marae_attack");
        private static readonly Identifier SmiteId = new("coc", "marae_smite");
        private static readonly Identifier SmiteHitId = new("coc", "marae_smite_hit");
        private static readonly Identifier TentacleAttackId = new("coc", "marae_tentacle_attack");
        private static readonly Identifier TentacleRapeId = new("coc", "marae_tentacle_rape");

        private bool Corrupted => _factoryShutdown == 2;

        // 原版 long：腐蚀 / 未腐蚀两套措辞（Marae.hx:105-113）。
        public override string Description => (Corrupted
            ? "Npcs.Marae.Desc.Corrupted"
            : "Npcs.Marae.Desc").Localized();

        // 原版 performCombatAction（Marae.hx:76-87）：
        //   Uber 状态时必定 smiteHit；
        //   否则 eAttack 3；FACTORY_SHUTDOWN==1 时 smite 3；==2 时 tentacleAttack 3 / tentacleRape 3。
        public override Identifier? ForcedAbility(CombatContext context) =>
            HasStatusEffect(VanillaStatusEffects.Uber) ? SmiteHitId : null;

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var choices = new List<(Identifier Id, float Weight)> { (AttackId, 3f) };

            if (_factoryShutdown == 1)
                choices.Add((SmiteId, 3f));

            if (_factoryShutdown == 2)
            {
                choices.Add((TentacleAttackId, 3f));
                choices.Add((TentacleRapeId, 3f));
            }

            var total = 0f;

            foreach (var (_, weight) in choices)
                total += weight;

            var roll = (float)context.Random.NextDouble() * total;

            foreach (var (id, weight) in choices)
            {
                roll -= weight;

                if (roll < 0)
                    return new CombatAiAction { Ability = id };
            }

            return new CombatAiAction { Ability = choices[^1].Id };
        }

        // 原版 handleFear（Marae.hx:70-74）：Marae 不怕恐惧，直接清掉并出专属台词。
        public override bool HandleFear(CombatContext context)
        {
            context.Append("Combat.Marae.HandleFear.MaraeSnarls".Localized());

            RemoveStatusEffect(VanillaStatusEffects.Fear);

            return true;
        }

        // 原版 runCheck（Marae.hx:89-98）：船被触手堵住，跑不掉（startMonsterTurn）。
        // C# 用 OnPlayerRunAttempt 接管逃跑尝试。
        public override bool OnPlayerRunAttempt(CombatContext context)
        {
            context.Append("Combat.Marae.RunCheck.YourBoatIs".Localized());
            context.Append((context.Player.CanFly()
                ? "Combat.Marae.RunCheck.YouGritYour"
                : "Combat.Marae.RunCheck.YouMayNot").Localized());
            context.Append("Combat.Marae.RunCheck.ItLooksLike".Localized());

            return true;
        }

        public override void Configure(EnemyDefinition definition)
        {
            _factoryShutdown = CoC.Instance.Flags.Get(VanillaFlags.FactoryShutdown);

            base.Configure(definition);

            // 原版 long/身体在 FACTORY_SHUTDOWN==2 与否则两套（Marae.hx:105-113）
            if (Corrupted)
            {
                CreateVagina(false, 4, 1); // WETNESS_DROOLING=4, LOOSENESS_NORMAL=1
                CreateBreastRow(VanillaBreastCups.G);

                Strength = 150;
                Toughness = 150;
                Speed = 70;
                Intelligence = 110;
                Libido = 60;
                Sensitivity = 25;
                Corruption = 100;

                WeaponName = "tentacles";
                WeaponVerb = "slap";
                WeaponAttack = 40;

                NormalAttackAvoid.DoParry = false;
                NormalAttackAvoid.DoCounter = false;
            }
            else
            {
                CreateVagina(false, 2, 1); // WETNESS_WET=2, LOOSENESS_NORMAL=1
                CreateBreastRow(VanillaBreastCups.DD);

                Strength = 200;
                Toughness = 150;
                Speed = 100;
                Intelligence = 150;
                Libido = 25;
                Sensitivity = 25;
                Corruption = 0;

                WeaponName = "fists";
                WeaponVerb = "wrathful punch";
                WeaponAttack = 50;

                NormalAttackAvoid.DoParry = false;
                NormalAttackAvoid.DoCounter = false;
            }

            // 原版 ass.analLooseness = 1; ass.analWetness = 1
            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;
            anal.Wetness = 1;

            // 原版 tallness = 10 * 12
            Tallness = 120;

            // 原版 hips.rating = 10; butt.rating = 8
            Body.SetHipsRating("curvy");
            Body.SetButtRating("large");

            // 原版 skin.tone = "white"; skin.setType(Skin.PLAIN)
            var skin = Body.Get<SkinPart>();

            skin.Tone = "white";
            skin.Id = VanillaBodyPartTypes.Plain;

            // 原版 hair.color = "green"; hair.length = 36
            var hair = Body.Get<HairPart>();

            hair.Color = "green";
            hair.Length = 36;

            // 原版 armorName = "bark"; armorDef = 30
            ArmorName = "bark";
            ArmorDefense = 30;

            // 原版 bonusHP = 4750；FACTORY_SHUTDOWN==1 时 +2700，MINERVA_TOWER_TREE>0 再 +1000。
            // 数据表 bonusHealth 为 0，这里按形态现算。
            var bonusHealth = 4750f;

            if (_factoryShutdown == 1)
            {
                bonusHealth += 2700;

                if (CoC.Instance.Flags.Get(VanillaFlags.MinervaTowerTree) > 0)
                    bonusHealth += 1000;
            }

            Stats.AddModifier(VanillaStats.MaxHealth, ModifierOperation.Addition,
                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            // 原版构造时血量就是满的（bonusHP 加上后 maxHP() 提升）
            Health = MaxHealth;

            // 原版 lustVuln = .07
            LustVulnerability = 0.07f;

            // 原版 additionalXP = 2500；FACTORY_SHUTDOWN==1 时 +500
            AdditionalExperience = _factoryShutdown == 1 ? 3000 : 2500;

            // 原版 gems = 1000
            Gems = 1000;

            // 原版 createPerk(Tank) / Tank2 / ImprovedSelfControl
            if (!HasPerk(VanillaPerks.Tank))
                CreatePerk(VanillaPerks.Tank);

            if (!HasPerk(VanillaPerks.Tank2))
                CreatePerk(VanillaPerks.Tank2);

            if (!HasPerk(VanillaPerks.ImprovedSelfControl))
                CreatePerk(VanillaPerks.ImprovedSelfControl);

            // 原版 bonusLust = 80（Marae.hx:144）：MaxLust 现在走 VanillaStats.MaxLust（Creature.cs:198），
            // 用 AddModifier 叠加到 100 + 80 = 180。
            Stats.AddModifier(VanillaStats.MaxLust, ModifierOperation.Addition,
                () => 80f, new TemporarySource(Guid.NewGuid()));

            // 原版 temperment = TEMPERMENT_RANDOM_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
            // 原版 drop = NO_DROP（Marae.hx:159），所以数据表 loot 留空。
            //
            // 原版 defeated()：Marae.hx:62-64（game.boat.marae.winAgainstMarae）
            // 原版 won()：Marae.hx:66-68（game.boat.marae.loseAgainstMarae）
        }

        // 原版 defeated()：Marae.hx:62-64
        public override bool Defeated(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.Boat.MaraeScene>.Get().WinAgainstMarae();

            return true;
        }

        // 原版 won()：Marae.hx:66-68（原版函数体只用 loseAgainstMarae()；C# 的 Won 没有 pcCameWorms 参数）
        public override bool Won(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.Boat.MaraeScene>.Get().LoseAgainstMarae();

            return true;
        }
    }
}
