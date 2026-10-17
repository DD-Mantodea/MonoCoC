using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.Handlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<SharkManEnemy>("coc:shark_man")]
    public class SharkManEnemy : Enemy
    {

        public bool _isCharging;

        public static readonly Identifier AttackId = new("coc", "shark_man_attack");

        public static readonly Identifier ThrustGrabId = new("coc", "shark_man_thrust_grab");

        public static readonly Identifier ThrustFuckId = new("coc", "shark_man_thrust_fuck");

        public static readonly Identifier ChargePt1Id = new("coc", "shark_man_charge_pt1");

        public static readonly Identifier ChargePt2Id = new("coc", "shark_man_charge_pt2");

        public bool IsCharging
        {
            get => _isCharging;

            set => _isCharging = value;
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (_isCharging)
                return new CombatAiAction { Ability = ChargePt2Id };

            var grappled = context.Player.HasStatusEffect(StatusEffectIds.Grappled);

            var choices = new List<(Identifier Id, float Weight)> { (AttackId, 2f) };

            if (grappled)
                choices.Add((ThrustFuckId, 5f));
            else
            {
                choices.Add((ThrustGrabId, 2f));

                choices.Add((ChargePt1Id, 1f));
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

        public override void Struggle(CombatContext context) => SharkManCombatHandlers.Struggle(context);

        public override void OutputAttack(CombatContext context, float damage)
        {
            context.Append((IsUnarmed()

                ? "Combat.SharkMan.OutputAttack.TheSharkMan2"

                : "Combat.SharkMan.OutputAttack.TheSharkMan").Localized());

            if (CoC.Instance.Flags.Get(Flags.GameFlagIds.EnemyCritical) > 0)
                context.Append("Combat.SharkMan.OutputAttack.Text".Localized());

            context.Append("Combat.Monster.Attack.Damage".Localized((int)damage));
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(13, 2);

            CreateCock(13, 2);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 0;

            Tallness = 72;

            CreateBreastRow(0);

            Body.SetHipsRating("boyish");

            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "gray";

            var hair = Body.Get<HairPart>();

            hair.Color = "silver";

            hair.Length = 16;

            WeaponName = "broken chain";

            WeaponVerb = "slash";

            WeaponAttack = 10;

            ArmorName = "tough skin";

            ArmorDefense = 5;

            LustVulnerability = 0.3f;

            Gems = Random.Shared.Next(15) + 5;

        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Places.Swim.SharkManScene>.Get().SharkManLostCombat(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Places.Swim.SharkManScene>.Get().SharkManWonCombat(hpVictory);

            return true;
        }

        public override void RunSuccess(CombatContext context) =>

            Scenes.SceneInstance<Scenes.Vanilla.Places.Swim.SharkManScene>.Get().MarkPlayerEscaped();
    }
}