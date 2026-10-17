using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 TrainingDummy extends Monster（src/classes/scenes/camp/TrainingDummy.hx）
    // 只移植类本体；defeated/won 依赖 TrainingDummyScene（未移植，原版 TrainingDummy.hx:10-16）。
    [Register("coc:training_dummy")]
    public class TrainingDummyEnemy : Enemy
    {
        // 原版 dummyGender 来自 game.camp.saveContent.dummyGender（TrainingDummy.hx:137）。
        // C# 由 TrainingDummyCamp 把同名字段存进 camp 场景存档（见 Contents/Scenes/Vanilla/Camp/TrainingDummyCamp.cs）。
        private int _dummyGender;

        // 原版 react(TurnStart)（TrainingDummy.hx:116-128）：每回合清空欲望并回满血。
        // C# 的 TurnStart 没有派发点，改在 CombatRoundUpdate（敌人回合开始）里做。
        public override void CombatRoundUpdate(CombatContext context)
        {
            if (Lust < MaxLust)
                Lust = 0;

            if (Health > 0)
                Health = MaxHealth;
        }

        // 原版 outputDefaultTeaseReaction（TrainingDummy.hx:18-54）：10 段假人吐槽，并占用本回合行动。
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
                    context.Append("Combat.TrainingDummy.OutputDefaultTeaseReaction.WhileMayBe".Localized()
                        _dummyGender % 2 == 1 ? " Well, almost, but that doesn't count." : "");

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

        // 原版 runCheck（TrainingDummy.hx:130-133）：专属逃跑台词，然后默认跑走。
        public override string? RunCheck(CombatContext context) =>
            "Combat.TrainingDummy.RunCheck.YouStepBack".Localized();

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            _dummyGender = MonoCoC.Contents.Scenes.Vanilla.TrainingDummyCamp.DummyGender;

            // 原版 short = game.camp.saveContent.dummyName（TrainingDummy.hx:139）
            var dummyName = MonoCoC.Contents.Scenes.Vanilla.TrainingDummyCamp.DummyName;

            if (!string.IsNullOrEmpty(dummyName))
                ShortName = dummyName;

            // 原版 createBreastRow(dummyGender >= 2 ? breastCupInverse("D") : 0)
            CreateBreastRow(_dummyGender >= 2 ? VanillaBreastCups.D : 0);

            // 原版 dummyGender % 2 == 1 时 createCock()
            if (_dummyGender % 2 == 1)
                CreateCock();

            // 原版 ass.analLooseness = LOOSENESS_VIRGIN(0); ass.analWetness = WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;
            anal.Wetness = 0;

            // 原版 tallness = 5 * 12 + 4
            Tallness = 5 * 12 + 4;

            // 原版 hips.rating = RATING_BOYISH(0); butt.rating = RATING_BUTTLESS(0)
            Body.SetHipsRating("boyish");
            Body.SetButtRating("buttless");

            // 原版 skin.tone = "wood"; hair.length = 0
            Body.Get<SkinPart>().Tone = "wood";
            Body.Get<HairPart>().Length = 0;

            // 原版 weaponName = "rusty frying pans"; weaponVerb = "smack"; weaponAttack = 0
            WeaponName = "rusty frying pans";
            WeaponVerb = "smack";
            WeaponAttack = 0;

            // 原版 armorName = "a few planks of wood"; armorDef = 0
            ArmorName = "a few planks of wood";
            ArmorDefense = 0;

            // 原版 createPerk(Immovable) / createStatusEffect(NoLoot)
            if (!HasPerk(VanillaPerks.Immovable))
                CreatePerk(VanillaPerks.Immovable);

            if (!HasStatusEffect(VanillaStatusEffects.NoLoot))
                CreateStatusEffect(VanillaStatusEffects.NoLoot);

            // 原版 temperment = TEMPERMENT_RANDOM_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
            // 原版 drop = new WeightedDrop()（空表，TrainingDummy.hx:168），所以数据表 loot 留空。
        }

        // 原版 long 按 dummyGender 四选一（TrainingDummy.hx:140）。
        public override string Description => _dummyGender switch
        {
            1 => "Npcs.TrainingDummy.Desc.Dick".Localized(),
            2 => "Npcs.TrainingDummy.Desc.Tits".Localized(),
            3 => "Npcs.TrainingDummy.Desc.Both".Localized(),
            _ => base.Description
        };

        // 原版 defeated() → game.camp.trainingDummyScene.dummyLost()（TrainingDummy.hx:10-12）
        public override bool Defeated(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.TrainingDummyScene>
                .Get().DummyLost();

            return true;
        }

        // 原版 won() → game.camp.trainingDummyScene.dummyWon(hpVictory)（TrainingDummy.hx:14-16）
        public override bool Won(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.TrainingDummyScene>
                .Get().DummyWon(hpVictory);

            return true;
        }
    }
}
