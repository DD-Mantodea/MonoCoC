using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Monsters;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 ImpGang extends Imp（src/classes/scenes/camp/ImpGang.hx）
    // 原版 super() 后把 Imp 的身体清空再重建，C# 侧直接按「重建后的净结果」配置，不继承 ImpEnemy。
    // 只移植类本体；defeated/won 依赖 ImpScene（未移植，原版 ImpGang.hx:9-16）。
    [Register("coc:imp_gang")]
    public class ImpGangEnemy : Enemy
    {
        // 原版 unitAmount（ImpGang.hx:68）：HP 每 100 算一只小鬼，maxHP() = unitAmount * 100。
        private int _unitAmount = 4;

        // 原版 maxHP()（ImpGang.hx:18-20）：随当前血量重算的「小鬼数量 × 100」。
        public int RecalculateUnits()
        {
            _unitAmount = (int)MathF.Ceiling(Health / 100f);

            return _unitAmount;
        }

        // 原版 long（ImpGang.hx:55）里拼了 cockDescript(0)，依赖实例，所以现算。
        public override string Description => "Npcs.ImpGang.Desc".Localized(CockDescript(0));

        public override void Configure(EnemyDefinition definition)
        {
            _unitAmount = 4;

            // 原版 maxHP() 覆盖了基类血量公式：直接写 unitAmount * 100。
            // 数据表 bonusHealth 必须为 0，否则会在这之上再叠加。
            Stats.SetProvider(VanillaStats.MaxHealth, () => _unitAmount * 100);

            base.Configure(definition);

            // 原版 createBreastRow()（默认 0 号）
            CreateBreastRow();

            // 原版 createCock(12, 1.5) / (25, 2.5) / (25, 2.5)
            CreateCock(12, 1.5f);
            CreateCock(25, 2.5f);
            CreateCock(25, 2.5f);

            // 原版 cocks[2].cockType = CockTypesEnum.DOG; cocks[2].knotMultiplier = 2
            if (Cocks.Count >= 3)
            {
                Cocks[2].Id = VanillaBodyPartTypes.Dog;
                Cocks[2].KnotMultiplier = 2;
            }

            // 原版 balls = 2; ballSize = 3
            Balls = 2;
            BallSize = 3;

            // 原版 tallness = 36
            Tallness = 36;

            // 原版 tail.type = Tail.DEMONIC; wings.type = Wings.IMP
            Body.Get<TailPart>().Id = VanillaBodyPartTypes.Demonic;
            Body.Get<WingsPart>().Id = VanillaBodyPartTypes.Imp;

            // 原版 skin.tone = "green"
            Body.Get<SkinPart>().Tone = "green";

            // 原版 createStatusEffect(GenericRunDisabled)
            if (!HasStatusEffect(VanillaStatusEffects.GenericRunDisabled))
                CreateStatusEffect(VanillaStatusEffects.GenericRunDisabled);

            // 原版 weaponName = "claws"; weaponVerb = "claws"; weaponAttack = 10
            WeaponName = "claws";
            WeaponVerb = "claws";
            WeaponAttack = 10;

            // 原版 armorName = "leathery skin"; armorDef = 3
            ArmorName = "leathery skin";
            ArmorDefense = 3;

            // 原版 lustVuln = .65
            LustVulnerability = 0.65f;

            // 原版 gems = Utils.rand(15) + 25：数据表里的 gems 只当占位，实际在这里摇
            Gems = Random.Shared.Next(15) + 25;

            // 原版 temperment = TEMPERMENT_LUSTY_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
        }

        // 原版 defeated(hpVictory)（ImpGang.hx:13-16）：DEMONS_DEFEATED += 1；impScene.impGangGetsWhooped()
        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(VanillaFlags.DemonsDefeated, CoC.Instance.Flags.Get(VanillaFlags.DemonsDefeated) + 1);
            SceneInstance<ImpScene>.Get().ImpGangGetsWhooped();

            return true;
        }

        // 原版 won(hpVictory, pcCameWorms)（ImpGang.hx:9-11）：impScene.impGangabangaEXPLOSIONS(true)
        public override bool Won(bool hpVictory)
        {
            SceneInstance<ImpScene>.Get().ImpGangabangaEXPLOSIONS(true);

            return true;
        }
    }
}
