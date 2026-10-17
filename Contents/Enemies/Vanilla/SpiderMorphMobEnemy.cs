using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<SpiderMorphMobEnemy>("coc:spider_morph_mob")]
    public class SpiderMorphMobEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(9, 2, BodyPartTypes.Human);

            Balls = 2;

            BallSize = 1;

            CumMultiplier = 3;

            CreateVagina(false, 3, 3);

            CreateBreastRow(0);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 4;

            anal.Wetness = 5;

            Tallness = Random.Shared.Next(8) + 70;

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 15;

            LustVulnerability = 0.2f;

            WeaponName = "claws";

            WeaponVerb = "claws";

            ArmorName = "chitin";

            Body.Get<TailPart>().Id = BodyPartTypes.SpiderAbdomen;

            Gems = Random.Shared.Next(25) + 40;

        }

        public override string Description =>

            (CoC.Instance?.Player?.Level ?? 0) <= 13

                ? "Npcs.SpiderMorphMob.DescHorde".Localized()

                : base.Description;

        public override bool BeforeCombatAction(CombatContext context)
        {
            if (context.Random.Next(2) == 0 || context.Player.HasStatusEffect(StatusEffectIds.UberWeb))
                SpiderStandardAttack(context);
            else
                WebLaunch(context);

            return true;
        }

        public void SpiderStandardAttack(CombatContext context)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = false
            });

            if (MissFirstRound || avoid.AttackFailed)
            {
                MissFirstRound = false;

                context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.ANumberOf".Localized());

                KihaSpidahai(context);

                return;
            }

            context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.ANumberOf2".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, Strength + WeaponAttack) + 20;

            var capitalA = Utils.Utils.CapitalizeFirstLetter(Article);

            if (damage <= 0)
            {
                damage = 0;

                if (context.Random.Next(Math.Max(1, (int)(player.ArmorDefense + player.Toughness)))
                    < player.ArmorDefense)

                    context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.YouAbsorbAnd".Localized(WeaponVerb));
                else
                    context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.YouDeflectAnd".Localized(
                        WeaponVerb, Article, ShortName));
            }
            else if (damage < 6)
                context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.YouAreStruck".Localized(
                    Article, ShortName));
            else if (damage < 11)
                context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.OneWounds".Localized(
                    capitalA, ShortName));
            else if (damage < 21)
                context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.OneStaggers".Localized(
                    capitalA, ShortName, Pronoun3, WeaponVerb));
            else
                context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.OneMutilate".Localized(
                    capitalA, ShortName, Pronoun3, WeaponVerb));

            if (damage > 0)
            {
                player.TakeDamage(damage);

                if (LustVulnerability > 0 && player.Armor.Id.Path == "bondage_straps")
                {
                    context.Append((Plural

                        ? "Combat.SpiderMorphMob.SpiderStandardAttack.OneBrush"

                        : "Combat.SpiderMorphMob.SpiderStandardAttack.OneBrushes")
                        .Localized(capitalA, ShortName));

                    Lust += 10 * LustVulnerability;
                }
            }

            KihaSpidahai(context);
        }

        public void KihaSpidahai(CombatContext context)
        {
            context.Append("Combat.SpiderMorphMob.KihaSPOIDAHAI.Text".Localized());

            context.Append("Combat.SpiderMorphMob.KihaSPOIDAHAI.WhileTheyRe".Localized());

            HealthChange(-50);
        }

        public void WebLaunch(CombatContext context)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (MissFirstRound || avoid.AttackFailed)
            {
                MissFirstRound = false;

                context.Append("Combat.SpiderMorphMob.SpoidahHordeWebLaunchahs.OneOfThe".Localized());

                return;
            }

            context.Append("Combat.SpiderMorphMob.SpoidahHordeWebLaunchahs.SomeOfThe".Localized());

            player.CreateStatusEffect(StatusEffectIds.UberWeb, 0, 0, 0, 0);

            HealthChange(-250);
        }

        public override void Struggle(CombatContext context)
        {
            if (!context.Player.HasStatusEffect(StatusEffectIds.UberWeb))
                return;

            context.Clear();

            context.Append("Combat.SpiderMorphMob.Struggle.YouClawYour".Localized());

            context.Player.RemoveStatusEffect(StatusEffectIds.UberWeb);
        }

        /// <summary>原版 SpiderMorphMob.hx:101 defeated()：打赢蛛群后走 Kiha 的「蛛群撤退」剧情。</summary>
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<KihaFollowerScene>.Get().BeatSpiderMob();

            return true;
        }

        /// <summary>
        /// 原版 SpiderMorphMob.hx:105 won()：被蛛群抓住。
        /// 若玩家刚自慰过（CameWorms），改为输出蛛群围观的段落并以 lust 失败结束。
        /// </summary>
        public override bool Won(bool hpVictory)
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.CameWormsAfterCombat) > 0)
            {
                Scene.OutputText("Combat.SpiderMorphMob.Won.TheSpidersSmile".Localized());

                CombatController.Instance!.EndLustLoss();

                return true;
            }

            SceneInstance<KihaFollowerScene>.Get().LoseToSpiderMob();

            return true;
        }

    }
}
