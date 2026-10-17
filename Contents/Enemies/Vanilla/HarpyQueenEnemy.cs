using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<HarpyQueenEnemy>("coc:harpy_queen")]
    public class HarpyQueenEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 5, 0);

            CreateBreastRow(BreastCups.D);

            Tallness = Random.Shared.Next(8) + 70;

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 15;

            WeaponName = "eldritch staff";

            WeaponVerb = "thwack";

            ArmorName = "armor";

            LustVulnerability = 0.15f;
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.HelHarpyQueenDefeated, 1);

            CombatController.Instance?.Context.Append("Combat.Hel.HarpyQueenVictory".Localized());

            return false;
        }

        public override bool Won(bool hpVictory)
        {
            CombatController.Instance?.Context.Append("Combat.Hel.HarpyQueenDefeat".Localized());

            return false;
        }
    }
}