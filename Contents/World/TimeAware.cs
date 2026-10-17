using System;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;
using System.Collections.Generic;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains;
using MonoCoC.Contents.Scenes.Vanilla.Places.Boat;

namespace MonoCoC.Contents.World
{

    public interface ITimeAware
    {

        bool TimeChange();

        bool TimeChangeLarge();
    }

    public static class TimeAwareRegistry
    {
        public static readonly Dictionary<string, Func<ITimeAware>> Sources = [];

        public static void Register(string key, Func<ITimeAware> source) => Sources[key] = source;

        public static void Clear() => Sources.Clear();

        public static int Count => Sources.Count;

        public static void ReloadVanilla()
        {
            Clear();

            SceneInstance<KellyScene>.Reset();

            SceneInstance<TamanisDaughtersScene>.Reset();

            SceneInstance<SheilaScene>.Reset();

            SceneInstance<MaraeScene>.Reset();

            SceneInstance<MarbleScene>.Reset();

            Register("coc:kelly_scene", () => SceneInstance<KellyScene>.Get());

    Register("coc:raphael", () => SceneInstance<RaphaelScene>.Get());

    Register("coc:cotton", () => SceneInstance<CottonScene>.Get());

    Register("coc:edryn", () => SceneInstance<EdrynScene>.Get());

    Register("coc:miffix", () => SceneInstance<MiffixScene>.Get());

    Register("coc:arian", () => SceneInstance<ArianScene>.Get());

    Register("coc:kiha_follower", () => SceneInstance<KihaFollowerScene>.Get());

    Register("coc:giacomo", () => SceneInstance<GiacomoScene>.Get());

    Register("coc:vapula", () => SceneInstance<VapulaScene>.Get());

    Register("coc:anemone", () => SceneInstance<AnemoneScene>.Get());

    Register("coc:urta", () => SceneInstance<UrtaScene>.Get());

            SceneInstance<ShouldraScene>.Reset();

            Register("coc:shouldra_scene", () => SceneInstance<ShouldraScene>.Get());

            Register("coc:player_hunger", () => new PlayerEvents());

            // 原版 items/undergarments/NobleShirt.hx:38 —— 贵妇衬衫按小时检查夜变阶段
            Register("coc:noble_shirt", () => Items.Behaviors.Vanilla.NobleShirtBehavior.Instance);

            // 原版 PhoukaScene.hx:24 —— 威士忌酒劲按小时递减
            Register("coc:phouka_scene", () => SceneInstance<Scenes.Vanilla.Areas.Bog.PhoukaScene>.Get());

            Register("coc:marae_scene", () => SceneInstance<MaraeScene>.Get());

            Register("coc:marble_scene", () => SceneInstance<MarbleScene>.Get());

        // 原版 Niamh.hx 是 TimeAwareInterface：香槟/黑猫啤酒倒计时、酿酒计数
        Register("coc:niamh", () => SceneInstance<NiamhScene>.Get());

            // 原版 MinervaScene.hx:77 pregnancyAdvance + 生产
            Register("coc:minerva", () => SceneInstance<MinervaScene>.Get());
        }

        public static bool DispatchHour(int hour)
        {
            var needNext = false;

            var aware = new List<ITimeAware>(Sources.Count);

            foreach (var source in Sources.Values)
                aware.Add(source());

            foreach (var entry in aware)
                if (entry.TimeChange())
                    needNext = true;

            foreach (var entry in aware)
                if (entry.TimeChangeLarge())
                    needNext = true;

            return needNext;
        }
    }
}