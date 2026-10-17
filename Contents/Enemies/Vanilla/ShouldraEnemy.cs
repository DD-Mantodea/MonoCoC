using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<ShouldraEnemy>("coc:shouldra")]
    public class ShouldraEnemy : Enemy
    {

        public bool GhostLoli => GameSettings.AllowChild == true && GameSettings.ShouldraChild;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 2);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 40);

            CreateBreastRow(GhostLoli ? BreastCups.A : BreastCups.C);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 0;

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 40);

            Tallness = GhostLoli ? 57 : 65;

            Body.Hips = GhostLoli ? 0 : 6;

            Body.Butt = GhostLoli ? 2 : 5;

            Body.Get<SkinPart>().Tone = "white";

            var hair = Body.Get<HairPart>();

            hair.Color = "white";

            hair.Length = 3;

            Strength = 45;

            Toughness = 30;

            Speed = 5;

            Intelligence = 110;

            Libido = 100;

            Sensitivity = 0;

            Corruption = 33;

            WeaponName = "fists";

            WeaponVerb = "punches";

            ArmorName = "comfortable clothes";

            Lust = 10;

            Level = 4;

            Gems = 0;

            CreatePerk(PerkIds.BlindImmune);
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.Blinded)
                return true;

            context.Append("Combat.Shouldra.React.RemarkablyItSeems".Localized());

            return false;
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Npcs.ShouldraScene>.Get().DefeatDannyPhantom();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Npcs.ShouldraScene>.Get().LoseToShouldra();

            return true;
        }
    }
}