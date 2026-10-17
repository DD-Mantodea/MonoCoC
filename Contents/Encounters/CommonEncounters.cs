using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Areas.DeepWoods;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Swamp;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Utils;
using System;
using System.Linq;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using System.Collections.Generic;

namespace MonoCoC.Contents.Encounters
{

    // 已接：goblin / twoimpsclash / imp（含多只变体）/ bigjunk

    public static class CommonEncounters
    {
        public static readonly IEncounter Table = IEncounter.Group(
            "common",

            IEncounter.Group(
                "common_imps",

                IEncounter.Create("twoimpsclash", _ => 0.1f, _ => true, TwoImpsClashing),
                // 原版 :26 imp 的出现率 = impEncounterBaseChance × lethiteMod
                IEncounter.Create("coc:imp", _ => ImpEncounterBaseChance() * LethiteMod(), _ => true, ImpCombat),
            // 原版 CommonEncounters.hx:37 alice：allowChild 时按等级出现（chance 0.1→0.3）
            IEncounter.Create("alice", _ => 0.1f, _ => GameSettings.AllowChild, _ => SceneInstance<AliceScene>.Get().ExecEncounter()),
            // 原版 CommonEncounters.hx:44 plaguerat：等级 ≥ 8
            IEncounter.Create("plaguerat", _ => 0.5f, _ => CoC.Instance.Player.Level >= 8, _ => SceneInstance<PlagueRatScene>.Get().PlagueEncounter()),
            // 原版 CommonEncounters.hx:51 ivorysuccubus：等级 ≥ 12
            IEncounter.Create("ivorysuccubus", _ => 0.25f, _ => CoC.Instance.Player.Level >= 12, _ => SceneInstance<IvorySuccubusScene>.Get().Encounter())),

            IEncounter.Group(
                "common_goblins",

                IEncounter.Create("coc:goblin", _ => 1f, _ => true, GoblinCombat),
            // 原版 CommonEncounters.hx:71 gobass：等级 ≥ 10
            IEncounter.Create("gobass", _ => 0.5f, _ => CoC.Instance.Player.Level >= 10, _ => SceneInstance<GoblinAssassinScene>.Get().GoblinAssassinEncounter()),

                IEncounter.Create("gobwar", _ => 1f, _ => true, _ => SceneInstance<GoblinWarriorScene>.Get().GoblinWarriorEncounter()),
            // 原版 CommonEncounters.hx:81 gobsha：等级 ≥ 12
            IEncounter.Create("gobsha", _ => 0.5f, _ => CoC.Instance.Player.Level >= 12, _ => SceneInstance<GoblinShamanScene>.Get().GoblinShamanEncounter())),

            // 原版 CommonEncounters.hx theCommonEncounters：helScene.helSexualAmbushEncounter
            IEncounter.Create("helia", _ => 1f, _ => true, _ => SceneInstance<HelScene>.Get().HelSexualAmbush()));

        public static void GoblinCombat(EncounterContext context)
        {
            var player = context.Player;

            Scene.ClearOutput();

            if (player.IsGenderless())
                context.Write("Combat.Common.Goblin.Genderless".Localized());
            else if (player.IsChild())
                context.Write("Combat.Common.Goblin.Child".Localized());
            else
                context.Write("Combat.Common.Goblin.Default".Localized(player.Mf("stud", "slut")));

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.Goblin), () => Scene.Go<CampScene>());
        }

        public static void TwoImpsClashing(EncounterContext context)
        {
            Scene.ClearOutput();

            context.Write("Combat.Common.TwoImps.Text".Localized());

            var items = new[] { ConsumableIds.SuccubiMilk, ConsumableIds.IncubiDraft, ConsumableIds.ImpFood };

            var pick = items[context.Random.Next(items.Length)];

            InventoryScene.TakeItem(ItemRegistry.Create(pick), context.ReturnToCamp);
        }

        /// <summary>原版 DungeonCore.hx:118 checkFactoryClear()：工厂是否已完全清空。</summary>
        public static bool CheckFactoryClear()
        {
            var flags = CoC.Instance.Flags;

            return flags.Get(GameFlagIds.FactoryShutdown) > 0

                && flags.Get(GameFlagIds.FactorySuccubusDefeated) > 0

                && (flags.Get(GameFlagIds.FactoryIncubusDefeated) > 0 || flags.Get(GameFlagIds.D3Discovered) > 0)

                && flags.Get(GameFlagIds.FactoryOmnibusDefeated) > 0;
        }

        /// <summary>
        /// 原版 CommonEncounters.hx:172 impMultiCombatEncounter()：
        /// 等级 / 出生过的小恶魔数量 / 工厂是否清空，共同决定出现只数与「带伤登场」的变体。
        /// </summary>
        public static void ImpCombat(EncounterContext context)
        {
            var player = CoC.Instance.Player;

            var flags = CoC.Instance.Flags;

            Scene.ClearOutput();

            var birthedImps = (int)player.StatusEffectValue(StatusEffectIds.BirthedImps, 0);

            var adjustedLevel = (int)Math.Max(player.Level, Scene.View.Time.Days / 6);

            var maxImps = 1;

            var extraImpChance = 5;

            var levelupChance = 0;

            if (CheckFactoryClear())
            {
                maxImps += 1;

                extraImpChance += 10;

                levelupChance += 20;
            }

            if (adjustedLevel >= 2 && birthedImps > 0)
            {
                if (birthedImps >= 20)
                    maxImps += 1;

                extraImpChance += birthedImps * 2;

                levelupChance += birthedImps * 3;
            }

            if (adjustedLevel >= 5)
            {
                maxImps += 1;

                extraImpChance += Math.Clamp(adjustedLevel * 2, 0, 50);

                levelupChance += Math.Clamp(adjustedLevel * 3, 0, 50);
            }

            var impAmount = 1;

            // 原版 :198：始终保留 1 只的合理概率
            if (extraImpChance <= 90 || context.Random.Next(100) < 90)
                for (var i = 1; i < maxImps; i++)
                {
                    var adjustedChance = extraImpChance - (i - 1) * 25;

                    if (context.Random.Next(100) < adjustedChance)
                        impAmount += 1;
                }

            // 原版 :207-241：按只数抽取开场描述与「带伤登场」变体
            string textKey;

            Action<List<Enemy>>? debuff = null;

            if (impAmount == 1)
                textKey = "Combat.Common.Imp.One";
            else if (impAmount == 2)
                switch (context.Random.Next(4))
                {
                    case 0:
                        textKey = "Combat.Common.Imp.Two.A";

                        break;

                    case 1:
                        textKey = "Combat.Common.Imp.Two.B";

                        break;

                    case 2:
                        textKey = "Combat.Common.Imp.Descend";

                        break;

                    default:
                        textKey = "Combat.Common.Imp.Two.C";
                        // 原版 :216：第二只已受伤 10 点、疲劳 +20
                        debuff = imps =>
                        {
                            if (imps.Count > 1)
                            {
                                imps[1].Health = Math.Max(1, imps[1].Health - 10);

                                imps[1].Fatigue += 20;
                            }
                        };

                        break;
                }
            else if (impAmount == 3)
                switch (context.Random.Next(4))
                {
                    case 0:
                        textKey = "Combat.Common.Imp.Three.A";

                        break;

                    case 1:
                        textKey = "Combat.Common.Imp.Descend";

                        break;

                    case 2:
                        textKey = "Combat.Common.Imp.Three.B";
                        // 原版 :229-231：全体受最大生命 10% 伤害、疲劳 +20
                        debuff = imps =>
                        {
                            foreach (var imp in imps)
                            {
                                imp.Health = Math.Max(1, imp.Health - imp.MaxHealth / 100 * 10);

                                imp.Fatigue += 20;
                            }
                        };

                        break;

                    default:
                        textKey = "Combat.Common.Imp.Three.C";
                        // 原版 :238-239：前两只各受 10 点伤害
                        debuff = imps =>
                        {
                            if (imps.Count > 0) imps[0].Health = Math.Max(1, imps[0].Health - 10);
                            if (imps.Count > 1) imps[1].Health = Math.Max(1, imps[1].Health - 10);
                        };

                        break;
                }
            else
                textKey = "Combat.Common.Imp.Descend";

            Scene.OutputText(textKey.Localized());

            // 原版 :248：解锁小恶魔图鉴
            player.UnlockCodexEntry(GameFlagIds.CodexEntryImps);

            // 原版 :249：按只数创建，每只独立判定是否升级
            var imps2 = new List<Enemy>();

            for (var i = 0; i < impAmount; i++)
            {
                var imp = EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "imp"));

                if (context.Random.Next(100) < levelupChance)
                    imp.ScaleToLevel(Math.Max(1, adjustedLevel + 1));

                imps2.Add(imp);
            }

            // 原版 :250-253：单只直接开战，多只走 StartMultiple
            if (imps2.Count == 1)
                CombatScene.Start(imps2[0], () => Scene.Go<CampScene>());
            else
                CombatScene.StartMultiple(imps2, () => Scene.Go<CampScene>(), true);

            debuff?.Invoke(imps2);

            Scene.SpriteSelect("coc:imp");
        }

        public static float BigJunkChance()
        {
            var player = CoC.Instance.Player;

            if (player.LongestCockLength() < player.Tallness)
                return 0f;

            if (TotalCockThickness(player) < 12f)
                return 0f;

            return 1f + (player.LongestCockLength() - player.Tallness) / 24f;
        }

        public static float TotalCockThickness(Player player) =>

            player.Cocks.Sum(cock => cock.Thickness);

        public static void BigJunkForestScene(bool lake = false)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            var x = player.BiggestCockIndex();

            Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.WalkingLead".Localized());

            if (lake)
                Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.MuddyShores".Localized());
            else
                Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.ForestPaths".Localized());

            Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.IncreasinglyImpeded".Localized(player.CockDescript(x)));

            if (lake)
                Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.WetGround".Localized());
            else
                Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.EarthBehind".Localized());

            if (player.Cocks.Count == 1)
            {
                if (lake)
                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.DragsLakeside".Localized(Appearance.CockNoun(player, x)));
                else
                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.DragsGrass".Localized(Appearance.CockNoun(player, x)));
            }
            else if (player.Cocks.Count >= 2)
                if (lake)
                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.CocksDragging".Localized(NumberUtils.Num2Text(player.CockTotal())));
                else
                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.CocksAcross".Localized(NumberUtils.Num2Text(player.CockTotal())));

            Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.ParagraphBreak".Localized());

            if (!player.IsTaur())
            {
                Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.ImpendingErection".Localized(player.MultiCockDescriptLight()));

                if (player.BiggestBreastSize() >= 35f)
                    if (lake)
                        Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.HangsLewdly".Localized());
                    else
                        Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.HangsLewdlyAgain".Localized());

                if (player.Balls > 0)
                {
                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.RestsBeneath".Localized());

                    if (lake)
                        Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.IntoWaters".Localized());
                    else
                        Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.OntoSoil".Localized());
                }

                if (player.Vaginas.Count >= 1)
                {
                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.SquashedBetween".Localized());

                    if (player.Vaginas[0].Wetness >= 4f)
                    {
                        Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.JuicesStream".Localized());

                        if (lake)
                            Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.DroolingSpunk".Localized());
                        else
                            Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.StickySpunk".Localized());
                    }
                }
            }

            else
            {
                Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.ImpendingErectionAgain".Localized(player.MultiCockDescriptLight()));

                if (player.BiggestBreastSize() >= 35f)
                    if (lake)
                        Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.PullsTorso".Localized());
                    else
                        Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.PullsTorsoAgain".Localized());

                if (player.Balls > 0)
                {

                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.RestsBeneathAgain".Localized(player.SkinTone, player.SackDescript()));

                    if (lake)
                        Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.IntoWatersAgain".Localized());
                    else
                        Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.OntoSoilAgain".Localized());
                }

                if (player.Vaginas.Count >= 1)
                {
                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.SquashedBetweenAgain".Localized());

                    if (player.Vaginas[0].Wetness >= 4f)
                        if (lake)
                            Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.LeafFalls".Localized());
                        else
                            Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.LeafFallsAgain".Localized());
                }
            }

            Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.ParagraphBreakTwo".Localized());

            Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.EffectivelyTrapped".Localized());

            if (player.Corruption < 33)
                Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.PanicSlips".Localized());
            else if (player.Corruption < 66)
                Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.IfDanger".Localized());
            else
                Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.EndowmentsHinder".Localized());

            if (lake)
            {
                if (player.CanFly())
                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.ExtendsWings".Localized());
                else if (player.IsTaur())
                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.StrugglesMultiple".Localized());
                else
                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.StrugglesPushes".Localized());
            }
            else
                if (player.CanFly())
                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.ExtendsWingsAgain".Localized());
                else if (player.IsTaur())
                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.StrugglesMultipleAgain".Localized());
                else
                    Scene.OutputText("Scenes.Commonencounters.BigJunkForestScene.StrugglesPushesAgain".Localized());

            FinishBigJunkScene(player);
        }

        public static void BigJunkDesertScene()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            var x = player.BiggestCockIndex();

            Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.WalksDunes".Localized(player.CockDescript(x)));

            if (player.Cocks.Count == 1)
                Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.DragsDunes".Localized(Appearance.CockNoun(player, x)));
            else if (player.Cocks.Count >= 2)
                Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.CocksDragging".Localized(NumberUtils.Num2Text(player.CockTotal())));

            Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.ParagraphBreak".Localized());

            if (!player.IsTaur())
            {
                Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.ImpendingErection".Localized(player.MultiCockDescriptLight()));

                if (player.BiggestBreastSize() >= 35f)
                    Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.HangsLewdly".Localized(player.AllBreastsDescript()));

                if (player.Balls > 0)
                    Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.RestsBeneath".Localized(player.SkinTone, player.SackDescript()));

                if (player.Vaginas.Count >= 1)
                {
                    Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.SquashedBetween".Localized());

                    if (player.Vaginas[0].Wetness >= 4f)
                        Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.JuicesStream".Localized());
                }
            }

            else
            {
                Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.ImpendingErectionAgain".Localized(player.MultiCockDescriptLight()));

                if (player.BiggestBreastSize() >= 35f)
                    Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.PullsTorso".Localized(player.AllBreastsDescript()));

                if (player.Balls > 0)
                    Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.RestsBeneathAgain".Localized(player.SkinTone, player.SackDescript()));

                if (player.Vaginas.Count >= 1)
                {
                    Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.SquashedBetweenAgain".Localized());

                    if (player.Vaginas[0].Wetness >= 4f)
                        Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.DesertSun".Localized());
                }
            }

            Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.ParagraphBreakTwo".Localized());

            Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.EffectivelyTrapped".Localized());

            if (player.Corruption < 33)
                Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.PanicSlips".Localized());
            else if (player.Corruption < 66)
                Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.IfDanger".Localized());
            else
                Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.EndowmentsHinder".Localized());

            if (player.CanFly())
                Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.ExtendsWings".Localized());
            else if (player.IsTaur())
                Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.StrugglesEquine".Localized());
            else
                Scene.OutputText("Scenes.Commonencounters.BigJunkDesertScene.StrugglesPushes".Localized());

            FinishBigJunkScene(player);
        }

        public static void FinishBigJunkScene(Player player)
        {
            var lust = 25 + (int)(Random.Shared.NextDouble() * (player.Corruption / 5f));

            player.ApplyStats(new DynStatsOptions(Scale: false), new DynStat(GameStats.Lust, lust));

            player.ChangeFatigue(5);

            Scene.DoNext(ReturnToCampAfterOneHour);
        }

        public static void ReturnToCampAfterOneHour()
        {
            Scene.View.Time.Minutes = 0;

            Scene.View.Time.PassTime(1);

            Scene.Go<CampScene>();
        }

        // 原版 :16 encounterName()
        public static string EncounterName() => "common";

        // 原版 :137 furriteMod()：穿孔毛石加成 3 倍
        public static float FurriteMod() =>

            CoC.Instance.Player.HasPerk(PerkIds.PiercedFurrite) ? 3f : 1f;

        // 原版 :141 lethiteMod()：穿孔勒石加成 3 倍
        public static float LethiteMod() =>

            CoC.Instance.Player.HasPerk(PerkIds.PiercedLethite) ? 3f : 1f;

        // 原版 :145 impEncounterBaseChance()
        public static float ImpEncounterBaseChance()
        {
            var player = CoC.Instance.Player;

            var impch = 5f;

            if (player.Cocks.Count > 0)
                impch -= 1;

            if (player.HasVagina())
                impch += 1;

            if (player.TotalFertility() >= 30)
                impch += 1;

            if (player.CumQ() >= 200)
                impch -= 1;

            return impch / 10;
        }
    }
}