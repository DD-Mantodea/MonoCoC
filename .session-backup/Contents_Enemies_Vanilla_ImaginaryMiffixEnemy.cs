using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/monsters/ImaginaryMiffix.hx（extends Monster）
    // 巴扎里「想象中的 Miffix」：属性全部 200，一击必杀，攻击会被他用两根手指接住。
    [Register("coc:imaginary_miffix")]
    public class ImaginaryMiffixEnemy : Enemy
    {
        // 原版 public var prevHP:Float = 0（ImaginaryMiffix.hx:13）在构造函数里记录 player.HP。
        // 只有 defeated()/won() 会用它把玩家血量还原（ImaginaryMiffix.hx:9/16），
        // 而这两个方法要跳 MiffixScene（未移植，原版 ImaginaryMiffix.hx:10/17），所以这里先不保存。

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new() 逐项照抄（ImaginaryMiffix.hx:59-101）
            // createCock(Utils.rand(2) + 15, 2.5, CockTypesEnum.DEMON)
            CreateCock(Random.Shared.Next(2) + 15, 2.5f, VanillaBodyPartTypes.Demon);

            Balls = 2;
            BallSize = 2;

            CreateBreastRow(0);

            // ass.analLooseness = LOOSENESS_TIGHT(1) / analWetness = WETNESS_NORMAL(1)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 1;

            // tallness = Utils.rand(24) + 25
            Tallness = Random.Shared.Next(24) + 25;

            // hips.rating = RATING_BOYISH(0) / butt.rating = RATING_TIGHT(2)
            Body.SetHipsRating("boyish");
            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();
            hair.Color = "black";
            hair.Length = 5;

            // weaponVerb = "claw-slash"
            WeaponName = "claws";
            WeaponVerb = "claw-slash";
            ArmorName = "leathery skin";

            // 原版 lust = 40 / level = 1 在数据表里；gems 是 Utils.rand(5) + 5
            Gems = Random.Shared.Next(5) + 5;

            // wings.type = Wings.IMP
            Body.Get<WingsPart>().Id = VanillaBodyPartTypes.Imp;

            // createPerk(PerkLib.PoisonImmune) / BleedImmune / PhysicalResistance(0.6) / Immovable
            CreatePerk(VanillaPerks.PoisonImmune);
            CreatePerk(VanillaPerks.BleedImmune);
            CreatePerk(VanillaPerks.PhysicalResistance, 0.6f);
            CreatePerk(VanillaPerks.Immovable);

            // 原版 drop = WeightedDrop(SUCMILK 3, INCUBID 3, IMPFOOD 4, WOODSHL 1)：见 ImaginaryMiffix.hjson loot
        }

        // 原版 handleCombatLossText（ImaginaryMiffix.hx:26-28）固定返回 0：
        // 这场战斗不扣宝石、不昏迷。
        public override int HandleCombatLossText(CombatContext context, bool inDungeon, int gemsLost) => 0;

        // 原版 react(BeforeAttacked)（ImaginaryMiffix.hx:30-49）：
        // 玩家出手前按武器类型选一段「被两根手指接住」的台词，并返回 false 取消这次攻击。
        // C# 侧由 CombatResolver.PlayerAttack 开头触发 BeforeAttacked（对应原版 Combat.hx:927，
        // 在闪避判定之前；返回 false = 整个攻击被挡下），所以这段逻辑是生效的。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            switch (reaction)
            {
                case CombatReactionContext.BeforeAttacked:
                {
                    var weapon = context.Player.Weapon;

                    if (weapon.IsBladed())
                    {
                        context.WriteAction("Combat.ImaginaryMiffix.React.YouSwingYour".Localized());
                    }
                    else if (weapon.IsRanged())
                    {
                        context.WriteAction("Combat.ImaginaryMiffix.React.YouTakeAim".Localized());
                    }
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

        // 原版 defeated() → game.bazaar.miffixScene.thatCantBeHowItHappened()（ImaginaryMiffix.hx:10）
        // 原版 won()      → game.bazaar.miffixScene.miffixPlan4()               （ImaginaryMiffix.hx:17）
        // 场景缺失：Contents/Scenes 全量搜索 MiffixScene = 0 命中（原版 ImaginaryMiffix.hx:10/17），先按默认收尾。
    }
}
