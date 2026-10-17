using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Monsters;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<ImpGangEnemy>("coc:imp_gang")]
    public class ImpGangEnemy : Enemy
    {

        public int _unitAmount = 4;

        public int RecalculateUnits()
        {
            _unitAmount = (int)MathF.Ceiling(Health / 100f);

            return _unitAmount;
        }

        public override string Description => "Npcs.ImpGang.Desc".Localized(CockDescript(0));

        public override void Configure(EnemyDefinition definition)
        {
            _unitAmount = 4;

            Stats.SetProvider(GameStats.MaxHealth, () => _unitAmount * 100);

            base.Configure(definition);

            CreateBreastRow();

            CreateCock(12, 1.5f);

            CreateCock(25, 2.5f);

            CreateCock(25, 2.5f);

            if (Cocks.Count >= 3)
            {
                Cocks[2].Id = BodyPartTypes.Dog;

                Cocks[2].KnotMultiplier = 2;
            }

            Balls = 2;

            BallSize = 3;

            Tallness = 36;

            Body.Get<TailPart>().Id = BodyPartTypes.Demonic;

            Body.Get<WingsPart>().Id = BodyPartTypes.Imp;

            Body.Get<SkinPart>().Tone = "green";

            if (!HasStatusEffect(StatusEffectIds.GenericRunDisabled))
                CreateStatusEffect(StatusEffectIds.GenericRunDisabled);

            WeaponName = "claws";

            WeaponVerb = "claws";

            WeaponAttack = 10;

            ArmorName = "leathery skin";

            ArmorDefense = 3;

            LustVulnerability = 0.65f;

            Gems = Random.Shared.Next(15) + 25;

        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.DemonsDefeated, CoC.Instance.Flags.Get(GameFlagIds.DemonsDefeated) + 1);

            SceneInstance<ImpScene>.Get().ImpGangGetsWhooped();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<ImpScene>.Get().ImpGangabangaEXPLOSIONS(true);

            return true;
        }
    }
}