using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<AmilyEnemy>("coc:amily")]
    public class AmilyEnemy : Enemy
    {

        public bool _teasedWhileConcentrated;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 1, 2);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 48);

            CreateBreastRow(BreastCups.C);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;

            anal.Wetness = 0;

            Tallness = 62;

            Body.Hips = 6;

            Body.Butt = 2;

            var noFur = !CoC.Instance.Player.HasFur();

            var skin = Body.Get<SkinPart>();

            skin.Tone = noFur ? "tan" : "tawny";

            skin.Id = noFur ? BodyPartTypes.Plain : BodyPartTypes.Fur;

            var hair = Body.Get<HairPart>();

            hair.Color = "brown";

            hair.Length = 5;

            Strength = 30;

            Toughness = 30;

            Speed = 85;

            Intelligence = 60;

            Libido = 45;

            Sensitivity = 45;

            Corruption = 10;

            WeaponName = "knife";

            WeaponVerb = "slash";

            WeaponAttack = 6;

            ArmorName = "rags";

            ArmorDefense = 1;

            Lust = 20;

            LustVulnerability = 0.85f;

            Level = 4;

            Gems = 2 + Random.Shared.Next(5);

        }

        public override void ApplyTease(float lustDelta)
        {
            if (HasStatusEffect(StatusEffectIds.Concentration))
            {
                _teasedWhileConcentrated = true;

                Lust = Math.Min(MaxLust, Lust + lustDelta * LustVulnerability * 2 + 25);

                RemoveStatusEffect(StatusEffectIds.Concentration);

                return;
            }

            base.ApplyTease(lustDelta);
        }

        public override void OnTeased(CombatContext context, float lustDelta)
        {
            if (_teasedWhileConcentrated)
            {
                _teasedWhileConcentrated = false;

                context.WriteAction("Combat.Amily.Teased.AmilyFlushesHotly".Localized());

                return;
            }

            base.OnTeased(context, lustDelta);
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction == CombatReactionContext.BeforeAttacked
                && HasStatusEffect(StatusEffectIds.Concentration)

                && !context.Player.Weapon.IsRanged()

                && context.Player.StatusEffectValue(StatusEffectIds.CounterAB, 0) != 1)
            {
                context.Append("Combat.Amily.BeforeAttacked.AmilyEasilyGlides".Localized());

                return false;
            }

            return true;
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Npcs.AmilyScene>.Get().ConquerThatMouseBitch();

            return true;
        }
    }
}