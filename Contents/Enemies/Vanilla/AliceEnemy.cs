using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.DeepWoods;
using System;
using System.Collections.Generic;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    [Register<AliceEnemy>("coc:alice")]
    public class AliceEnemy : Enemy
    {
        public readonly string _hairColor;

        public readonly string _skinTone;

        public readonly string _eyeColor;

        public readonly string _panties;

        // 注册表要求 public 无参构造；外观参数仅在 AliceScene 显式构造时传入，注册路径用中性默认值。
        public AliceEnemy() : this("black", "fair", "green", "white")
        {
        }

        public AliceEnemy(string hairColor, string skinTone, string eyeColor, string panties = "white")
        {
            _hairColor = hairColor;

            _skinTone = skinTone;

            _eyeColor = eyeColor;

            _panties = panties;
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 0);

            CreateBreastRow(1);

            Tallness = 50;

            Body.Get<SkinPart>().Tone = _skinTone;

            Body.Get<HairPart>().Color = _hairColor;

            Body.Get<HairPart>().Length = 22;

            WeaponName = "fists";

            WeaponVerb = "punch";

            ArmorName = "skin";
        }

        public override void PerformCombatAction()
        {
            var context = CombatController.Instance!.Context;

            var player = context.Player;

            var distanced = Distance == CombatDistance.Distant;

            var options = new List<int>();

            for (var i = 0; i < (distanced ? 3 : 1); i++) options.Add(0);
            options.Add(1);

            options.Add(1);

            for (var i = 0; i < (HasStatusEffect(StatusEffectIds.LustAura) ? 2 : 4); i++) options.Add(2);
            options.Add(3);

            switch (options[Random.Shared.Next(options.Count)])
            {
                case 0: Illusion(context, player); break;
                case 1: Tease(context, player); break;
                case 2: Arouse(context, player); break;
                default: CombatController.Instance!.Resolver.EnemyBasicAttack(); break;
            }
        }

        public void Illusion(CombatContext context, Player player)
        {
            context.Write("Common.Text.TheAliceAttemptsToMaintainA".Localized());

            var resist = player.Intelligence < 30 ? (int)Math.Round(player.Intelligence) : 30;

            if (player.HasPerk(PerkIds.Whispered)) resist += 20;
            if (player.IsReligious() && player.IsPureEnough(20)) resist += (int)(20 - player.CorAdjustedDown());

            if (Random.Shared.Next(100) <= resist)
            {
                context.Write("Scenes.Alice.AliceIllusion.JellySlowResisted".Localized());

                return;
            }

            if (!player.HasStatusEffect(StatusEffectIds.Web))
            {
                context.Write("Common.Text.YouChargeForwardToCatchHer".Localized());

                player.CreateStatusEffect(StatusEffectIds.Web, 3, 0, 0, 0);
            }
            else
            {
                context.Write("Scenes.Alice.AliceIllusion.IllusionSpeedMore".Localized());

                player.ChangeStatusValue(StatusEffectIds.Web, 0, 1);
            }

            CombatCalculator.AddCombatBuff(player, GameStats.Speed, -25, "alice_web", StatusEffectIds.Web);
        }

        public void Tease(CombatContext context, Player player)
        {
            switch (Random.Shared.Next(3))
            {
                case 0:
                    context.Write("Scenes.Alice.AliceTeases.SkirtLiftTease".Localized());

                    break;

                case 1:
                    context.Write("Common.Text.TheAliceTossesHerHandsUp".Localized());

                    break;

                default:
                    context.Write("Scenes.Alice.AliceTeases.TailFlickPanties".Localized());

                    break;
            }

            player.TakeLustDamage(5 + player.Sensitivity / 10);
        }

        public void Arouse(CombatContext context, Player player)
        {
            context.Write("Scenes.Alice.AliceArouse.ArcaneArousalWarmth".Localized());

            player.TakeLustDamage(15 + player.Sensitivity / 10);

            if (!HasStatusEffect(StatusEffectIds.LustAura))
                CreateStatusEffect(StatusEffectIds.LustAura, 0, 0, 0, 0);
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<AliceScene>.Get().AliceWin();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<AliceScene>.Get().AliceLoss();

            return true;
        }
    }
}