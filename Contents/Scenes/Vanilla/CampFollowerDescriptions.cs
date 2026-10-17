using System;
using System.Collections.Generic;
using System.Linq;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Followers;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Seasonal;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    /// <summary>
    /// 原版 Camp.hx:1672 campFollowers(true) 的营地随从描述注册表。
    /// mod 可通过 Register 追加自己的随从描述，不必改 CampScene。
    /// </summary>
    public static class CampFollowerDescriptions
    {
        private sealed class Entry
        {
            public Identifier Id = default!;

            public Func<bool> Present = () => false;

            public Action Describe = () => { };

            public Action? Open;

            public int Order;
        }

        private static readonly List<Entry> Entries = [];

        private static readonly Dictionary<Identifier, Identifier> Aliases = [];

        public static int Count => Entries.Count;

        static CampFollowerDescriptions()
        {
            AddFollower("coc:anemone", () => SceneInstance<AnemoneScene>.Get().AnemoneFollower(),

                () => SceneInstance<AnemoneScene>.Get().AnemoneBarrelDescription(), 10,

                () => SceneInstance<AnemoneScene>.Get().ApproachAnemoneBarrel());

            AddFollower("coc:ember", () => SceneInstance<EmberScene>.Get().FollowerEmber(),

                () => SceneInstance<EmberScene>.Get().EmberCampDesc(), 20,

                () => SceneInstance<EmberScene>.Get().EmberCampMenu());

            AddFollower("coc:holli", () => SceneInstance<HolliScene>.Get().HolliFollower(),

                () => Scene.OutputText("Scenes.Camp.Desc.HolliTree".Localized()), 30,

                () => SceneInstance<HolliScene>.Get().TreeMenu());

            // 原版 Camp.hx:1672 campFollowers() 的顺序
            AddFollower("coc:jojo", () => FollowerRegistry.HasFollower(FollowerIds.JojoFollower),

                DescribeJojoCamp, 40, () => SceneInstance<JojoScene>.Get().JojoCamp());

            AddFollower("coc:rathazul", () => SceneInstance<RathazulScene>.Get().FollowerRathazul(),

                DescribeRathazulCamp, 50, () => SceneInstance<RathazulScene>.Get().ReturnToRathazulMenu());

            AddFollower("coc:sophie", () => SceneInstance<SophieScene>.Get().SophieAtCamp(),

                DescribeSophieCamp, 60, () => SceneInstance<SophieFollowerScene>.Get().FollowerSophieMainScreen());

            AddFollower("coc:valeria", () => CoC.Instance.Flags.Get(GameFlagIds.TookGooArmor) == 1,

                () => SceneInstance<ValeriaScene>.Get().ValeriaCampLines(), 65,

                () => SceneInstance<ValeriaScene>.Get().ValeriaFollowerMenu());

            // 原版 Camp.hx campLoversMenu() 的顺序
            AddFollower("coc:amily", () => SceneInstance<AmilyScene>.Get().AmilyFollowerPure(),

                DescribeAmilyCamp, 70);

            AddFollower("coc:arian", () => SceneInstance<ArianScene>.Get().ArianFollower(),

                DescribeArianCamp, 80, () => SceneInstance<ArianScene>.Get().ArianHomeMenu());

            AddFollower("coc:helia", () => SceneInstance<HelScene>.Get().FollowerHel(),

                DescribeHeliaCamp, 90, () => SceneInstance<HelFollowerScene>.Get().HeliaFollowerMenu());

            AddFollower("coc:helspawn", () => SceneInstance<HelSpawnScene>.Get().HelspawnLover(),

                DescribeHelspawnCamp, 100, () => SceneInstance<HelSpawnScene>.Get().HelspawnsMainMenu());

            AddFollower("coc:isabella", () => CoC.Instance.Flags.Get(GameFlagIds.IsabellaFollowerAccepted) == 1

                    && CoC.Instance.Flags.Get(GameFlagIds.IsabellaCampDisabled) == 0,

                DescribeIsabellaCamp, 110, () => SceneInstance<IsabellaFollowerScene>.Get().CallForFollowerIsabella());

            AddFollower("coc:izma", () => SceneInstance<IzmaScene>.Get().IzmaFollower(),

                DescribeIzmaCamp, 120, () => SceneInstance<IzmaScene>.Get().IzmaFollowerMenu());

            AddFollower("coc:kiha", () => SceneInstance<KihaFollowerScene>.Get().FollowerKiha(),

                DescribeKihaCamp, 130, () => SceneInstance<KihaScene>.Get().EncounterKiha());

            AddFollower("coc:marble", () => SceneInstance<MarbleScene>.Get().MarbleAtCamp(),

                DescribeMarbleCamp, 140, () => SceneInstance<MarbleScene>.Get().InteractWithMarbleAtCamp());

            AddFollower("coc:phylla", () => CoC.Instance.Flags.Get(GameFlagIds.AntWaifu) > 0,

                DescribePhyllaCamp, 150, () => SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.Desert.AntsScene>.Get().IntroductionToPhyllaFollower());

            // 原版 Camp.hx campSlavesMenu() 的顺序
            AddFollower("coc:latex_goo", () => SceneInstance<LatexGirlScene>.Get().LatexGooAtCamp(),

                DescribeLatexGooCamp, 160, () => SceneInstance<LatexGirlScene>.Get().ApproachLatexy());

            AddFollower("coc:milk_waifu", () => SceneInstance<MilkWaifuScene>.Get().MilkSlaveAtCamp(),

                DescribeMilkWaifuCamp, 170, () => SceneInstance<MilkWaifuScene>.Get().MilkyMenu());

            AddFollower("coc:ceraph", () => SceneInstance<CeraphFollowerScene>.Get().CeraphIsFollower(),

                () => Scene.OutputText("Scenes.Camp.Desc.Ceraph".Localized()), 180,

                () => SceneInstance<CeraphFollowerScene>.Get().CeraphFollowerEncounter());

            AddFollower("coc:vapula", () => SceneInstance<VapulaScene>.Get().VapulaSlave()

                    && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmVapula) == 0,

                () => SceneInstance<VapulaScene>.Get().VapulaSlaveFlavorText(), 190,

                () => SceneInstance<VapulaScene>.Get().CallSlaveVapula());

            AddFollower("coc:nieve", () => SceneInstance<NieveScene>.Get().NieveFollower(),

                () => SceneInstance<NieveScene>.Get().NieveCampDescs(), 200,

                () => SceneInstance<NieveScene>.Get().ApproachNieve(true));

            // 这些角色 ID 已由上面的条件描述覆盖，注册空项以抑制通用占位
            Alias("coc:amily_lover", "coc:amily");

            Alias("coc:amily_slave", "coc:amily");

            // 角色 ID 与 Camp 描述 ID 的别名映射
            Alias("coc:sophie_follower", "coc:sophie");

            Alias("coc:sophie_slave", "coc:sophie");

            Alias("coc:helspawn_follower", "coc:helspawn");

            Alias("coc:helspawn_lover", "coc:helspawn");

            Alias("coc:latex_girl", "coc:latex_goo");
        }

        public static void Alias(string alias, string target)
        {
            Aliases[Identifier.Parse(alias)] = Identifier.Parse(target);
        }

        private static Entry? FindEntry(Identifier id)
        {
            var entry = Entries.FirstOrDefault(candidate => candidate.Id == id);

            if (entry != null)
                return entry;

            return Aliases.TryGetValue(id, out var target)

                ? Entries.FirstOrDefault(candidate => candidate.Id == target)

                : null;
        }

        private static void DescribeAmilyCamp()
        {
            if (SceneInstance<HolliScene>.Get().HolliFollower())
            {
                Scene.OutputText("Scenes.Camp.Desc.AmilyBeddingHolli".Localized());

                return;
            }

            Scene.OutputText("Scenes.Camp.Desc.AmilyBedding".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) > 0

                    ? "Scenes.Camp.Desc.Cabin".Localized()

                    : "Scenes.Camp.Desc.Bedroll".Localized()));
        }

        private static void DescribeArianCamp()
        {
            var flags = CoC.Instance.Flags;

            if (CampSleep.IsForceSleepTime())
                Scene.OutputText(flags.Get(GameFlagIds.SleepWith) == "Arian"

                    ? "Scenes.Camp.Desc.ArianWait".Localized()

                    : "Scenes.Camp.Desc.ArianSleep".Localized());
            else
                Scene.OutputText("Scenes.Camp.Desc.ArianTent".Localized());
        }

        private static void DescribeHeliaCamp()
        {
            var flags = CoC.Instance.Flags;

            var level = flags.Get(GameFlagIds.HelFollowerLevel);

            if (level != 2)
            {
                Scene.OutputText("Scenes.Camp.Desc.HeliaAwait".Localized());

                return;
            }

            if (CampSleep.IsForceSleepTime())
            {
                Scene.OutputText(flags.Get(GameFlagIds.SleepWith) == "Helia"

                    ? "Scenes.Camp.Desc.HeliaWait".Localized()

                    : "Scenes.Camp.Desc.HeliaSleep".Localized());

                return;
            }

            var hour = Scene.View.Time.Hours;

            if (hour <= 7)
                Scene.OutputText("Scenes.Camp.Desc.HeliaVodka".Localized());
            else if (hour <= 17)
                Scene.OutputText("Scenes.Camp.Desc.HeliaPlains".Localized());
            else if (hour <= 19)
                Scene.OutputText("Scenes.Camp.Desc.HeliaFamily".Localized());
            else
                Scene.OutputText("Scenes.Camp.Desc.HeliaHammock".Localized());
        }

        private static void DescribeHelspawnCamp()
        {
            var flags = CoC.Instance.Flags;

            var name = flags.Get(GameFlagIds.HelspawnName);

            if (CampSleep.IsForceSleepTime())
            {
                if (flags.Get(GameFlagIds.SleepWith) == "Helspawn")
                    Scene.OutputText("Scenes.Camp.Desc.HelspawnWait".Localized(name));

                Scene.OutputText("Scenes.Camp.Desc.HelspawnSleep".Localized(name));

                return;
            }

            Scene.OutputText("Scenes.Camp.Desc.HelspawnCamp".Localized(name));
        }

        private static void DescribeIsabellaCamp()
        {
            if (CampSleep.IsForceSleepTime())
            {
                Scene.OutputText("Scenes.Camp.Desc.IsabellaSleep".Localized());

                return;
            }

            switch (Scene.View.Time.Hours)
            {
                case 6:
                    Scene.OutputText("Scenes.Camp.Desc.IsabellaBreakfast".Localized());

                    break;

                case 7:
                    Scene.OutputText("Scenes.Camp.Desc.IsabellaSewing".Localized());

                    break;

                case 8:
                    Scene.OutputText("Scenes.Camp.Desc.IsabellaCleaning".Localized());

                    break;

                default:
                    Scene.OutputText("Scenes.Camp.Desc.IsabellaAround".Localized());

                    break;
            }
        }

        private static void DescribePhyllaCamp()
        {
            var flags = CoC.Instance.Flags;

            var kids = flags.Get(GameFlagIds.AntKids);

            if (kids > 0 && kids <= 250)
                Scene.OutputText("Scenes.Camp.Desc.PhyllaChildrenOne".Localized());
            else if (kids > 250 && kids <= 1000)
                Scene.OutputText("Scenes.Camp.Desc.PhyllaChildrenFew".Localized());
            else if (kids > 1000)
                Scene.OutputText("Scenes.Camp.Desc.PhyllaChildrenMany".Localized());
            else
                Scene.OutputText("Scenes.Camp.Desc.PhyllaAlone".Localized());
        }

        private static void DescribeLatexGooCamp() => Scene.OutputText(
            CampSleep.IsForceSleepTime()

                ? "Scenes.Camp.Desc.LatexSleep".Localized()

                : "Scenes.Camp.Desc.LatexLurk".Localized());

        private static void DescribeMilkWaifuCamp() => Scene.OutputText(
            CampSleep.IsForceSleepTime()

                ? "Scenes.Camp.Desc.MilkSleep".Localized()

                : "Scenes.Camp.Desc.MilkAwake".Localized());

        private static bool IsFollowerGuard(Identifier id)
        {
            var definition = FollowerRegistry.Definitions.FirstOrDefault(candidate => candidate.Id == id);

            return definition?.IsPresent() == true && definition.IsGuard?.Invoke() == true;
        }

        private static void DescribeIzmaCamp()
        {
            var flags = CoC.Instance.Flags;

            if (CampSleep.IsForceSleepTime())
            {
                Scene.OutputText(flags.Get(GameFlagIds.SleepWith) == "Izma"

                    ? "Scenes.Camp.Desc.IzmaWait".Localized()

                    : "Scenes.Camp.Desc.IzmaSleep".Localized());

                return;
            }

            Scene.OutputText("Scenes.Camp.Desc.IzmaBedroll".Localized(
                flags.Get(GameFlagIds.CampBuiltCabin) > 0

                    ? "Scenes.Camp.Desc.NextToCabin".Localized()

                    : "Scenes.Camp.Desc.NearBedroll".Localized()));

            Scene.OutputText(Random.Shared.Next(3) switch
            {
                0 => "Scenes.Camp.Desc.IzmaReading".Localized(),

                1 => "Scenes.Camp.Desc.IzmaStream".Localized(),

                _ => "Scenes.Camp.Desc.IzmaWet".Localized(),
            });
        }

        private static void DescribeKihaCamp()
        {
            var flags = CoC.Instance.Flags;

            var kiha = SceneInstance<KihaFollowerScene>.Get();

            var children = kiha.TotalKihaChildren();

            var maturity = flags.Get(GameFlagIds.KihaChildMaturityCounter);

            if (IsFollowerGuard(FollowerIds.KihaLover))
            {
                Scene.OutputText("Scenes.Camp.Desc.KihaGuard".Localized());

                return;
            }

            if (CampSleep.IsForceSleepTime())
            {
                Scene.OutputText("Scenes.Camp.Desc.KihaSleep".Localized());

                return;
            }

            if (Scene.View.Time.Hours < 7)
            {
                Scene.OutputText("Scenes.Camp.Desc.KihaPolish".Localized());

                return;
            }

            if (children > 0 && maturity > 160)
            {
                Scene.OutputText("Scenes.Camp.Desc.KihaBreastfeed".Localized());

                return;
            }

            if (children > 0 && maturity > 80 && maturity <= 160)
            {
                Scene.OutputText("Scenes.Camp.Desc.KihaStories".Localized());

                return;
            }

            Scene.OutputText(Scene.View.Time.Hours < 19

                ? "Scenes.Camp.Desc.KihaPatrol".Localized()

                : "Scenes.Camp.Desc.KihaDummies".Localized());
        }

        private static void DescribeMarbleCamp()
        {
            var flags = CoC.Instance.Flags;

            var marble = SceneInstance<MarbleScene>.Get();

            if (CampSleep.IsForceSleepTime())
            {
                Scene.OutputText(flags.Get(GameFlagIds.SleepWith) == "Marble"

                    ? "Scenes.Camp.Desc.MarbleWait".Localized()

                    : "Scenes.Camp.Desc.MarbleSleep".Localized());

                return;
            }

            Scene.OutputText("Scenes.Camp.Desc.MarbleBedroll".Localized());

            if (flags.Get(GameFlagIds.MarblePurificationStage) == 4)
                Scene.OutputText("Scenes.Camp.Desc.MarbleFamily".Localized());
            else if (flags.Get(GameFlagIds.MarbleKids) >= 1
                     && (Scene.View.Time.Hours == 19 || Scene.View.Time.Hours == 20))

                Scene.OutputText("Scenes.Camp.Desc.MarbleNursery".Localized());
            else if (Scene.View.Time.Hours == 6 || Scene.View.Time.Hours == 7)
                Scene.OutputText("Scenes.Camp.Desc.MarbleTraining".Localized());
            else if (Scene.View.Time.Hours >= 21)
                Scene.OutputText("Scenes.Camp.Desc.MarbleBedWait".Localized());
            else
                Scene.OutputText("Scenes.Camp.Desc.MarbleAround".Localized());
        }

        private static void DescribeRathazulCamp()
        {
            var flags = CoC.Instance.Flags;

            var countdown = flags.Get(GameFlagIds.RathazulSilkArmorCountdown);

            if (countdown <= 1)
            {
                Scene.OutputText("Scenes.Camp.Desc.RathazulWork".Localized());

                if (CampSleep.IsForceSleepTime())
                    Scene.OutputText("Scenes.Camp.Desc.RathazulSleeping".Localized());
                else
                    Scene.OutputText("Scenes.Camp.Desc.RathazulChemicals".Localized());

                if (countdown == 1)
                    Scene.OutputText("Scenes.Camp.Desc.RathazulFinished".Localized());
            }
            else
                Scene.OutputText("Scenes.Camp.Desc.RathazulSilkWork".Localized());
        }

        private static void DescribeSophieCamp()
        {
            var flags = CoC.Instance.Flags;

            var children = SceneInstance<SophieBimboScene>.Get().SophieChildren();

            if (CampSleep.IsForceSleepTime())
            {
                if (flags.Get(GameFlagIds.SleepWith) == "Sophie")
                    Scene.OutputText("Scenes.Camp.Desc.SophieSleepWith".Localized());
                else if (children == 1)
                    Scene.OutputText("Scenes.Camp.Desc.SophieSleepOne".Localized());
                else if (children > 1)
                    Scene.OutputText("Scenes.Camp.Desc.SophieSleepMany".Localized());
                else
                    Scene.OutputText("Scenes.Camp.Desc.SophieSleepAlone".Localized());

                return;
            }

            var roll = Random.Shared.Next(5);

            if (roll == 0)
            {
                Scene.OutputText("Scenes.Camp.Desc.SophieGloss".Localized());

                return;
            }

            if (roll == 1)
            {
                Scene.OutputText("Scenes.Camp.Desc.SophieBrush".Localized());

                return;
            }

            if (roll == 2)
            {
                Scene.OutputText("Scenes.Camp.Desc.SophieFuss".Localized());

                return;
            }

            var adultKids = flags.Get(GameFlagIds.SophieAdultKidCount);

            if (roll == 3 || adultKids == 0)
            {
                Scene.OutputText(flags.Get(GameFlagIds.SophieBimbo) > 0

                    ? "Scenes.Camp.Desc.SophieReadBimbo".Localized()

                    : "Scenes.Camp.Desc.SophieRead".Localized());

                return;
            }

            var where = adultKids < 5

                ? "Scenes.Camp.Desc.SophieAcrossDaughters".Localized()

                : "Scenes.Camp.Desc.SophieSurroundedDaughters".Localized();

            var them = adultKids == 1

                ? "Scenes.Camp.Desc.Her".Localized()

                : "Scenes.Camp.Desc.Them".Localized();

            Scene.OutputText("Scenes.Camp.Desc.SophieTeaching".Localized(where, them));
        }

        private static void DescribeJojoCamp()
        {
            var jojo = FollowerRegistry.Definitions.FirstOrDefault(
                definition => definition.Id == FollowerIds.JojoFollower);

            if (jojo?.IsGuard?.Invoke() == true)
                Scene.OutputText("Scenes.Camp.Desc.JojoGuard".Localized());
            else
                Scene.OutputText("Scenes.Camp.Desc.JojoBedroll".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) > 0

                        ? "Scenes.Camp.Desc.Cabin".Localized()

                        : "Scenes.Camp.Desc.Bedroll".Localized()));
        }

        public static void AddFollower(string id, Func<bool> present, Action describe, int order = 100, Action? open = null)
        {
            if (!Identifier.TryParse(id, out var identifier))
                throw new FormatException($"Invalid camp follower description id '{id}'");

            Entries.RemoveAll(entry => entry.Id == identifier);

            Entries.Add(new Entry
            {
                Id = identifier,

                Present = present,

                Describe = describe,

                Open = open,

                Order = order,
            });

            Entries.Sort((left, right) => left.Order.CompareTo(right.Order));
        }

        public static void AppendAll()
        {
            foreach (var entry in Entries)
            {
                if (!entry.Present())
                    continue;

                entry.Describe();
            }
        }

        /// <summary>Followers 菜单等复用处：命中专用条件描述时输出并返回 true。</summary>
        public static bool TryDescribe(Identifier id)
        {
            var entry = FindEntry(id);

            if (entry == null || !entry.Present())
                return false;

            entry.Describe();

            return true;
        }

        public static Action? TryOpen(Identifier id)
        {
            var entry = FindEntry(id);

            return entry?.Present() == true ? entry.Open : null;
        }

        // 数据驱动的 Lover/Slave 营地描述：读取 FollowerDefinition.Description
        public static void AppendRole(Identifier role)
        {
            foreach (var definition in FollowerRegistry.Present(role))
            {
                // 已经有专门条件描述的 NPC 不再输出通用占位
                if (FindEntry(definition.Id) != null)
                    continue;

                if (!string.IsNullOrEmpty(definition.Description))
                    Scene.OutputText(definition.Description + "[pg]");
            }
        }
    }
}