using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Places.MothCave;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 Outsider extends Monster（src/classes/scenes/places/mothCave/Outsider.hx，132 行）
    // 机制特殊：自己从不出手（tookAction=true），第三回合自行 HP=0；玩家的攻击会被黑触手反噬。
    [Register("coc:outsider")]
    public class OutsiderEnemy : Enemy
    {
        private int _turnsPassed;
        private int _timesWaited;
        private bool _attacked;
        private int _damage;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new()（Outsider.hx:96-130）：没有 createCock，直接 initedGenitals
            CreateBreastRow(0);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1; // LOOSENESS_TIGHT
            anal.Wetness = 1;   // WETNESS_NORMAL

            Tallness = 60;

            Body.SetHipsRating("slender");
            Body.SetButtRating("average");

            WeaponName = "strange tendril";
            WeaponVerb = "strikes";
            Fatigue = 0;
            WeaponAttack = 0;

            ArmorName = "inky mist";
            ArmorDefense = 99;

            Lust = 0;
            LustVulnerability = 0.01f;
            Level = 99;

            CreatePerk(VanillaPerks.BleedImmune);
            CreateStatusEffect(VanillaStatusEffects.GenericRunDisabled, 0, 0, 0, 0);

            AdditionalExperience = 0;
        }

        // 原版 long（Outsider.hx:102）里按 metric 选项切换单位，数据表的 descriptionKey 只放英制；
        // 公制时用独立的 Desc.Metric 键。
        public override string Description =>
            (OptionSystem.OptionStore.Instance.Get("metric", 0) > 0
                ? "Npcs.Outsider.Desc.Metric"
                : "Npcs.Outsider.Desc").Localized();

        // 原版 react(TurnStart) 的「从不行动 / 第三回合消散」部分（Outsider.hx:26-33）。
        // C# 没有独立的 TurnStart 派发（见 Enemy.BeforeCombatAction 注释），每回合行动前的这个钩子语义等价。
        public override bool BeforeCombatAction(CombatContext context)
        {
            TookAction = true;

            _turnsPassed += 1;

            if (_turnsPassed == 3 && context.Player.Health > 0)
                Health = 0;

            return true;
        }

        // 原版 react(...)：PlayerWaited / BeforeAttacked 两个分支（Outsider.hx:36-49、81-90）
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction == CombatReactionContext.PlayerWaited)
            {
                // 原版 switch (++timesWaited)（Outsider.hx:37-47）
                switch (++_timesWaited)
                {
                    case 1:
                        context.WriteAction("Combat.Outsider.Wait.First".Localized());
                        break;
                    case 2:
                        context.WriteAction("Combat.Outsider.Wait.Second".Localized());
                        break;
                    case 3:
                        context.WriteAction("Combat.Outsider.Wait.Third".Localized());
                        break;
                }

                return false;
            }

            if (reaction == CombatReactionContext.BeforeAttacked)
            {
                if (_attacked)
                {
                    context.WriteAction("Combat.Outsider.React.Tendril".Localized());
                    context.Player.TakeDamage(_damage);
                }
                else
                {
                    context.WriteAction("Combat.Outsider.React.NoResistance".Localized());
                    _damage = (int)(context.Player.Health * 0.55f);
                    _attacked = true;
                }

                return false;
            }

            return true;
        }

        // 原版 defeated(hpVictory)（Outsider.hx:16-18）
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<DoloresScene>.Get().DoloresSummoningWin();

            return true;
        }

        // 原版 won(hpVictory, pcCameWorms)（Outsider.hx:20-22）
        public override bool Won(bool hpVictory)
        {
            SceneInstance<DoloresScene>.Get().DoloresSummoningLose();

            return true;
        }
    }
}
