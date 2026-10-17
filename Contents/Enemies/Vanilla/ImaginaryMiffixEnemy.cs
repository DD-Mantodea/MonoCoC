using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<ImaginaryMiffixEnemy>("coc:imaginary_miffix")]
    public class ImaginaryMiffixEnemy : Enemy
    {

        // 原版 ImaginaryMiffix.hx:13/64 的 prevHP：构造时记录玩家血量，战斗结束还原（梦境战斗不真正扣血）
        public float PrevHP;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            PrevHP = CoC.Instance.Player.Health;

            CreateCock(Random.Shared.Next(2) + 15, 2.5f, BodyPartTypes.Demon);

            Balls = 2;

            BallSize = 2;

            CreateBreastRow(0);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 1;

            Tallness = Random.Shared.Next(24) + 25;

            Body.SetHipsRating("boyish");

            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 5;

            WeaponName = "claws";

            WeaponVerb = "claw-slash";

            ArmorName = "leathery skin";

            Gems = Random.Shared.Next(5) + 5;

            Body.Get<WingsPart>().Id = BodyPartTypes.Imp;

            CreatePerk(PerkIds.PoisonImmune);

            CreatePerk(PerkIds.BleedImmune);

            CreatePerk(PerkIds.PhysicalResistance, 0.6f);

            CreatePerk(PerkIds.Immovable);

        }

        public override int HandleCombatLossText(CombatContext context, bool inDungeon, int gemsLost) => 0;

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            switch (reaction)
            {
                case CombatReactionContext.BeforeAttacked:
                {
                    var weapon = context.Player.Weapon;

                    if (weapon.IsBladed())
                        context.WriteAction("Combat.ImaginaryMiffix.React.YouSwingYour".Localized());
                    else if (weapon.IsRanged())
                        context.WriteAction("Combat.ImaginaryMiffix.React.YouTakeAim".Localized());
                    else if (weapon.IsBlunt())
                    {
                        context.WriteAction("Combat.ImaginaryMiffix.React.YouSwingYour2".Localized());

                        context.WriteAction("Combat.ImaginaryMiffix.React.ToYourAbsolute".Localized());
                    }
                    else
                    {
                        context.WriteAction("Combat.ImaginaryMiffix.React.TheImpRemains".Localized());

                        context.WriteAction("Combat.ImaginaryMiffix.React.YourAttackLands".Localized());

                        context.WriteAction("Combat.ImaginaryMiffix.React.YouReStill".Localized());
                    }

                    return false;
                }

                default:
                    return true;
            }
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Player.Health = PrevHP;

            Scenes.SceneInstance<Scenes.Vanilla.Places.Bazaar.MiffixScene>.Get().ThatCantBeHowItHappened();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            CoC.Instance.Player.Health = PrevHP;

            Scenes.SceneInstance<Scenes.Vanilla.Places.Bazaar.MiffixScene>.Get().MiffixPlan4();

            return true;
        }
    }
}