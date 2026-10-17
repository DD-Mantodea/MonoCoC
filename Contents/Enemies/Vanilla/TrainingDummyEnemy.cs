using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<TrainingDummyEnemy>("coc:training_dummy")]
    public class TrainingDummyEnemy : Enemy
    {

        public int _dummyGender;

        public override void CombatRoundUpdate(CombatContext context)
        {
            if (Lust < MaxLust)
                Lust = 0;

            if (Health > 0)
                Health = MaxHealth;
        }

        public override void OnTeased(CombatContext context, float lustDelta)
        {
            context.Append("Combat.TrainingDummy.OutputDefaultTeaseReaction.Text".Localized());

            switch (context.Random.Next(10))
            {
                case 0:
                    context.Append("Combat.TrainingDummy.OutputDefaultTeaseReaction.SWoodenVisage".Localized());

                    break;

                case 1:
                    context.Append("Combat.TrainingDummy.OutputDefaultTeaseReaction.YourCharmsSeem".Localized());

                    break;

                case 2:
                    context.Append("Combat.TrainingDummy.OutputDefaultTeaseReaction.YouBeginTo".Localized());

                    break;

                case 3:
                    context.Append("Combat.TrainingDummy.OutputDefaultTeaseReaction.YouWonderIf".Localized());

                    break;

                case 4:
                    context.Append("Combat.TrainingDummy.OutputDefaultTeaseReaction.FailsToReact".Localized());

                    break;

                case 5:
                    context.Append("Combat.TrainingDummy.OutputDefaultTeaseReaction.SeemsUnimpressed".Localized());

                    break;

                case 6:
                    context.Append("Combat.TrainingDummy.OutputDefaultTeaseReaction.RemainsStillIt".Localized());

                    break;

                case 7:
                    context.Append("Combat.TrainingDummy.OutputDefaultTeaseReaction.WhileMayBe".Localized(
                        _dummyGender % 2 == 1 ? " Well, almost, but that doesn't count." : ""));

                    break;

                case 8:
                    context.Append((_dummyGender % 2 == 1

                        ? "Combat.TrainingDummy.OutputDefaultTeaseReaction.StiffReason"

                        : "Combat.TrainingDummy.OutputDefaultTeaseReaction.MainPole").Localized());

                    break;

                case 9:
                    context.Append("Combat.TrainingDummy.OutputDefaultTeaseReaction.YourDummyIsn".Localized());

                    break;

                default:
                    break;
            }

            TookAction = true;
        }

        public override string? RunCheck(CombatContext context) =>

            "Combat.TrainingDummy.RunCheck.YouStepBack".Localized();

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            _dummyGender = Scenes.Vanilla.TrainingDummyCamp.DummyGender;

            var dummyName = Scenes.Vanilla.TrainingDummyCamp.DummyName;

            if (!string.IsNullOrEmpty(dummyName))
                ShortName = dummyName;

            CreateBreastRow(_dummyGender >= 2 ? BreastCups.D : 0);

            if (_dummyGender % 2 == 1)
                CreateCock();

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;

            anal.Wetness = 0;

            Tallness = 5 * 12 + 4;

            Body.SetHipsRating("boyish");

            Body.SetButtRating("buttless");

            Body.Get<SkinPart>().Tone = "wood";

            Body.Get<HairPart>().Length = 0;

            WeaponName = "rusty frying pans";

            WeaponVerb = "smack";

            WeaponAttack = 0;

            ArmorName = "a few planks of wood";

            ArmorDefense = 0;

            if (!HasPerk(PerkIds.Immovable))
                CreatePerk(PerkIds.Immovable);

            if (!HasStatusEffect(StatusEffectIds.NoLoot))
                CreateStatusEffect(StatusEffectIds.NoLoot);

        }

        public override string Description => _dummyGender switch
        {
            1 => "Npcs.TrainingDummy.Desc.Dick".Localized(),

            2 => "Npcs.TrainingDummy.Desc.Tits".Localized(),

            3 => "Npcs.TrainingDummy.Desc.Both".Localized(),

            _ => base.Description
        };

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.TrainingDummyScene>
                .Get().DummyLost();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.TrainingDummyScene>
                .Get().DummyWon(hpVictory);

            return true;
        }
    }
}
