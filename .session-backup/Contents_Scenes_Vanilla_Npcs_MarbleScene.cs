using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.World;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    // 原版 classes/scenes/npcs/MarbleScene.hx（5042 行）。
    //
    // 本文件只放「战斗收尾」（MarbleScene.hx:678 marbleFightWin / :701 marbleFightLose）；
    // A 块（1-897 行）在 MarbleScene.Core.cs，其余分段按任务分区清单分批移植。
    // 完整骨架由 scene-port.mjs 生成在 Tools/PortKit/out/MarbleScene.cs.txt。
    // 后续分段各开 partial 文件，避免多人改同一文件。
    [Register("coc:marble_scene")]
    public partial class MarbleScene : Scene, ITimeAware
    {
        // 原版 MarbleScene 是 NPCAwareContent，没有入口方法：由农场/营地事件直接调它的方法。
        // 同 CeraphScene 的处理方式。
        public override void Entry()
        {
        }

        // 原版 MarbleScene.hx:678 marbleFightWin()
        // HP 胜利 / 欲望胜利两套开场，然后给三个选项。
        public void MarbleFightWin()
        {
            // 原版 spriteSelect(SpriteDb.s_marble)：C# 没有贴图运行时（同 SpiderMorphMobEnemy 的处理），跳过。
            ClearOutput();

            if (CurrentMonster?.Health < 1)
            {
                OutputText("Scenes.Marblescene.MarbleFightWin.DefeatedLookingUp".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleFightWin.DesperateSkirtLift".Localized());
            }

            OutputText("Scenes.Marblescene.MarbleFightWin.CrowdOfOnlookers".Localized());

            ClearButtons();

            // 原版 MarbleScene.hx:691-695：有 Feeder 或奶量 > 200 才能「Feed Her」，
            // 否则按钮置灰并提示要高奶量。forceFeedMarble 已在 A 块落地（MarbleScene.hx:793）。
            if (Player.HasPerk(VanillaPerks.Feeder) || Player.LactationQ() > 200)
            {
                AddButton(0, "Feed Her", ForceFeedMarble);
            }
            else
            {
                AddButtonDisabled(0, "Feed Her", "Scenes.Marblescene.MarbleFightWin.FeedHerHint".Localized());
            }

            // 原版 MarbleScene.hx:693：rapeMarbleInHerRoom 已在 A 块落地（MarbleScene.hx:716）。
            AddButton(1, "RapeInRoom", RapeMarbleInHerRoom);

            AddButton(14, "Leave", CombatScene.CleanupAfterCombat);
        }

        // 原版 MarbleScene.hx:701 marbleFightLose()
        public void MarbleFightLose()
        {
            // 原版 spriteSelect(SpriteDb.s_marble)：同上，C# 无贴图运行时，跳过。
            ClearOutput();

            if (Player.Health < 1)
            {
                OutputText("Scenes.Marblescene.MarbleFightLose.KnockedToGround".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleFightLose.OvercomeByDesire".Localized());
            }

            OutputText("Scenes.Marblescene.MarbleFightLose.WhisperedThreat".Localized());

            CombatScene.CleanupAfterCombat();
        }
    }
}
