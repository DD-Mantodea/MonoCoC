using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Places.Farm;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<KeltEnemy>("coc:kelt")]
    public class KeltEnemy : Enemy
    {

        public bool _breakLevel2;

        public int _bowCooldown;

        public static readonly Identifier ShootBowId = new("coc", "kelt_shoot_bow");

        public static readonly Identifier DayDreamId = new("coc", "kelt_day_dream_kelly");

        public static readonly Identifier AttackId = new("coc", "kelt_attack");

        public static readonly Identifier KellyuraId = new("coc", "kelt_kellyura_attack");

        public static readonly Identifier TramplesId = new("coc", "kelt_tramples_joo");

        public void SetBowCooldown(int value) => _bowCooldown = value;

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (_bowCooldown > 0)
                _bowCooldown -= 1;

            var choices = new List<(Identifier Id, float Weight)>();

            if (_bowCooldown == 0)
                choices.Add((ShootBowId, 2f));

            if (CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) >= 2)
                choices.Add((DayDreamId, 1f));

            choices.Add((AttackId, 2f));

            choices.Add((KellyuraId, 1.5f));

            choices.Add((TramplesId, 1.5f));

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

        public override string Description =>

            base.Description + (_breakLevel2 ? "Npcs.Kelt.Desc.Broken".Localized() : "");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            var player = CoC.Instance.Player;

            _breakLevel2 = CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) == 2;

            CreateCock(_breakLevel2 ? 12 : 24, 3.5f, BodyPartTypes.Horse);

            Balls = 2;

            BallSize = 2 + Random.Shared.Next(13);

            CumMultiplier = 1.5f;

            HoursSinceCum = player.BallSize * 10;

            CreateBreastRow(_breakLevel2 ? BreastCups.B : BreastCups.A);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 2;

            anal.Wetness = 0;

            if (!HasStatusEffect(StatusEffectIds.BonusACapacity))
                CreateStatusEffect(StatusEffectIds.BonusACapacity, 50);

            Tallness = 84;

            Body.SetHipsRating("average");

            Body.SetButtRating("average", 1);

            var lowerBody = Body.Get<LowerBodyPart>();

            lowerBody.Id = BodyPartTypes.Hoofed;

            lowerBody.LegCount = 4;

            Body.Get<SkinPart>().Tone = "tan";

            var hair = Body.Get<HairPart>();

            hair.Color = Random.Shared.Next(2) == 0 ? "black" : "brown";

            hair.Length = 3;

            Body.Get<TailPart>().Id = BodyPartTypes.Horse;

            WeaponName = "fist";

            WeaponVerb = "punch";

            WeaponAttack = 10;

            ArmorName = "tough skin";

            ArmorDefense = 4;

            LustVulnerability = 0.83f;

            Gems = Random.Shared.Next(5) + 5;

        }

        public override bool Defeated(bool hpVictory)
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.KeltKillPlan) == 1)
            {
                if (hpVictory)
                    SceneInstance<KeltScene>.Get().FightToBeatKeltVictoryHP();
                else
                    SceneInstance<KeltScene>.Get().FightToBeatKeltVictoryLust();
            }
            else if (CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) == 1)
                SceneInstance<KellyScene>.Get().DefeatKellyNDBREAKHIM();
            else
                SceneInstance<KellyScene>.Get().BreakingKeltNumeroThree();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.CameWormsAfterCombat) > 0)
            {
                Scene.OutputText("Combat.Kelt.Won.KeltRecoilsFor".Localized());

                return false;
            }

            SceneInstance<KellyScene>.Get().KeltFucksShitUp();

            return true;
        }
    }
}