using System;
using System.Collections.Generic;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Places.Boat;

namespace MonoCoC.Contents.World
{
    // 原版 TimeAwareInterface（classes/TimeAwareInterface.hx:3-25）的 C# 对应物。
    //
    // 时间语义约定（必须遵守）：
    //   C# 的 Time.HourPassed 在小时「回绕之后」触发（Time.cs:32-41），所以 TimeChange 里看到的
    //   View.Time.Hours 是 0（午夜），而不是原版 timeChange 看到的 24（EventParser.hx:231 先 +1、
    //   :240 才回绕）。因此本仓库禁止再出现 `hours > 23` 的直译；判断午夜一律用 `Hours == 0`。
    public interface ITimeAware
    {
        // 每个小时推进时调用；返回 true 表示产出了需要玩家确认的内容。
        // 对应原版 EventParser.hx:235：任一 timeChange 返回 true 时 needNext = true。
        bool TimeChange();

        // 紧跟在全部 TimeChange 之后调用；返回 true 表示该事件要求暂停等玩家点 Next。
        // 对应原版 EventParser.hx:268-274（timeAwareLargeLastEntry 的可恢复遍历）。
        bool TimeChangeLarge();
    }

    // 轻量静态注册表：只保存「取 ITimeAware 的工厂」，不保存实例。
    // 场景本身是懒加载单例（SceneInstance<T>.Get()），如果在这里直接存实例，启动时就会把所有场景
    // 提前构造出来；存工厂则由第一次小时派发按需创建。
    public static class TimeAwareRegistry
    {
        private static readonly Dictionary<string, Func<ITimeAware>> Sources = [];

        // key 用于幂等：同一个 key 重复注册只保留第一次。
        public static void Register(string key, Func<ITimeAware> source) => Sources[key] = source;

        public static void Clear() => Sources.Clear();

        public static int Count => Sources.Count;

        // NewGame / Load 的统一入口：先清空注册表，再丢弃场景单例缓存，最后重新注册原版实现者。
        //
        // 注意 SceneInstance<T> 自己不会在开局 / 读档时清理（SceneInstance.Reset 全仓库 0 处调用），
        // 所以这里对已注册的类型显式 Reset，避免读档后拿到带旧状态的场景实例。
        // 未注册的 SceneInstance<T> 缓存不在此处理：那是更广范围的历史问题，不在本任务内。
        public static void ReloadVanilla()
        {
            Clear();

            SceneInstance<KellyScene>.Reset();

            SceneInstance<TamanisDaughtersScene>.Reset();

            SceneInstance<SheilaScene>.Reset();

            SceneInstance<MaraeScene>.Reset();

            SceneInstance<MarbleScene>.Reset();

            Register("coc:kelly_scene", () => SceneInstance<KellyScene>.Get());

            Register("coc:tamanis_daughters_scene", () => SceneInstance<TamanisDaughtersScene>.Get());

            Register("coc:sheila", () => SceneInstance<SheilaScene>.Get());

            // 原版 MaraeScene.hx:65-79 timeChange：Holli 树成长计数器的日增量来源
            Register("coc:marae_scene", () => SceneInstance<MaraeScene>.Get());

            // 原版 MarbleScene.hx:62-237 timeChange / :281-495 timeChangeLarge（A 块）
            Register("coc:marble_scene", () => SceneInstance<MarbleScene>.Get());
        }

        // 每小时派发一次（由 GameView.OnHourPassed 调用）。
        // 返回值 = 是否有 timeChange / timeChangeLarge 产出需要 Next 的内容。
        //
        // 未实现（刻意留空，避免动 Scene 导航）：原版 timeChangeLarge 返回 true 时的「暂停等 Next」
        // 与可恢复遍历（EventParser.hx:212-219、:268-274）。C# 目前没有事件队列 / 续跑机制，
        // 所以这里只汇总返回值；接暂停队列时应在 TimeChangeLarge 返回 true 处停下并保存进度。
        public static bool DispatchHour(int hour)
        {
            var needNext = false;

            var aware = new List<ITimeAware>(Sources.Count);

            foreach (var source in Sources.Values)
                aware.Add(source());

            // 对应 EventParser.hx:235：全部 timeChange 先跑完。
            foreach (var entry in aware)
            {
                if (entry.TimeChange())
                    needNext = true;
            }

            // 对应 EventParser.hx:268-274：之后再跑 timeChangeLarge。
            foreach (var entry in aware)
            {
                if (entry.TimeChangeLarge())
                    needNext = true;
            }

            return needNext;
        }
    }
}
