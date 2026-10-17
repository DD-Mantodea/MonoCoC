using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Items.Effects.Vanilla;
using MonoCoC.Attributes;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Items.Effects
{
    public static class ConsumableEffectRegistry
    {
        public static readonly Dictionary<string, Func<ConsumableEffectDefinition, IConsumableEffect>> Types = new(StringComparer.OrdinalIgnoreCase);

        // 原版 Consumable.canUse()：特定物品在满足条件前不能使用（如 Bimbo 利口酒对有 FutaForm 的人）
        public static readonly Dictionary<string, Func<Creatures.Player, bool>> CanUseChecks = new(StringComparer.OrdinalIgnoreCase);

        static ConsumableEffectRegistry()
        {
            RegistrationScanner.OnTypes(RegisterTypes);

            RegisterType("spell_book", definition => new SpellBookEffect(definition.GetString("kind") switch
            {
                "black" => SpellBookKind.Black,

                "gray" => SpellBookKind.Gray,

                _ => SpellBookKind.White
            }));

            RegisterType("hair_dye", definition => new HairDyeEffect(definition.GetString("color")));

            RegisterType("skin_oil", definition => new SkinOilEffect(definition.GetString("color")));

            RegisterType("egg", definition => new EggEffect(definition.GetBool("large")));

            RegisterType("ovi_elixir", definition => new OviElixirEffect(definition.GetBool("ovimax")));

            RegisterType("bee_honey", definition => new BeeHoneyEffect(definition.GetBool("pure"), definition.GetBool("special")));

            RegisterType("canine_pepper", definition => new CaninePepperEffect(definition.GetString("flavor")));

            RegisterType("draft", definition => new DraftEffect(definition.GetBool("incubi"), definition.GetBool("purified"), definition.GetBool("fuck")));

            RegisterType("body_lotion", definition => new BodyLotionEffect(definition.GetString("adj")));

            RegisterType("stat_tonic", definition => new StatTonicEffect(
                definition.GetFloat("libido"),

                definition.GetFloat("sensitivity"),

                definition.GetFloat("corruption"),

                definition.GetFloat("lust"),

                definition.GetFloat("strength"),

                definition.GetFloat("toughness")));

            RegisterType("food", definition => new FoodEffect(
                definition.GetInt("hunger"),

                definition.GetFloat("strength"),

                definition.GetFloat("toughness"),

                definition.GetFloat("lust")));

            RegisterType("drink", definition => new DrinkEffect(
                definition.GetInt("hunger"),

                definition.GetFloat("strength"),

                definition.GetFloat("toughness"),

                definition.GetFloat("lust")));

            RegisterType("numbing", definition => new NumbingEffect(definition.GetBool("oil")));

            // 原版 Reducto.hx:27 —— 药膏的部位选择界面（数据条目 coc:reducto_menu）
            RegisterType("reducto_menu", _ => new Scenes.Vanilla.Items.ReductoEffect());

            // 原版 Lactaid.hx:14 —— 促乳剂
            RegisterType("lactaid", _ => new LactaidEffect());

            // 原版 UrtaCum.hx:9 —— 瓶装 Urta 精液
            RegisterType("urta_cum", _ => new UrtaCumEffect());

            // 原版 GroPlus.hx:19 —— Gro+ 注射剂（部位选择界面）
            RegisterType("gro_plus", _ => new Scenes.Vanilla.Items.GroPlusEffect());

            // 原版 Equinum.hx:13 —— Equinum 马用转化药水
            RegisterType("equinum", _ => new EquinumEffect());

            // 原版 Beondegi.hx:14 —— 蚕蛹（蝙蝠化）
            RegisterType("beondegi", _ => new BeondegiEffect());

            // 原版 Clovis.hx:13 —— Clovis（绵羊化）
            RegisterType("clovis", _ => new ClovisEffect());

            // 原版 Ament.hx:14 —— Ament（羚羊化 + 人性化）
            RegisterType("ament", _ => new AmentEffect());

            // 原版 GnollSpot.hx:15 —— Gnoll Spot（鬣狗化）
            RegisterType("gnoll_spot", _ => new GnollSpotEffect());

            // 原版 TrapOil.hx:14 —— 陷阱油（中性化方向）
            RegisterType("trap_oil", _ => new TrapOilEffect());

            // 原版 PigTruffle.hx:13 —— 小猪块菌 / 野猪块菌（同一效果，boar 开关）
            RegisterType("pig_truffle", definition => new PigTruffleEffect(definition.GetBool("boar")));

            // 原版简单消耗品批次（Akbal 唾液/伊莎贝拉牛奶/薄荷白/纯净桃/羊奶/纯净珍珠/媚唇膏/龙蛋/蛛眼/冰锥）
            RegisterType("akbal_saliva", _ => new AkbalSalivaEffect());

            RegisterType("isabella_milk", _ => new IsabellaMilkEffect());

            RegisterType("peppermint_white", _ => new PeppermintWhiteEffect());

            RegisterType("purity_peach", _ => new PurityPeachEffect());

            RegisterType("sheep_milk", _ => new SheepMilkEffect());

            RegisterType("pure_pearl", _ => new PurePearlEffect());

            RegisterType("lust_stick", _ => new LustStickEffect());

            RegisterType("ember_egg", _ => new EmberEggEffect());

            RegisterType("oculum_arachnae", _ => new OculumArachnaeEffect());

            RegisterType("ice_shard", _ => new IceShardEffect());

            // 原版冬布丁 / 生发精华 / 断剑碎片 / 翼刃
            RegisterType("winter_pudding", _ => new WinterPuddingEffect());

            RegisterType("hair_serum", _ => new HairExtensionSerumEffect());

            RegisterType("sword_shard", _ => new BeautifulSwordShardEffect());

            RegisterType("wingstick", _ => new WingstickEffect());

            // 原版 Ectoplasm / TatteredScroll / SnakeOil（大型转化物品）
            RegisterType("ectoplasm", _ => new EctoplasmEffect());

            RegisterType("tattered_scroll", _ => new TatteredScrollEffect());

            RegisterType("snake_oil", _ => new SnakeOilEffect());

            // 原版 KitsuneGift / ShriveledTentacle / SuperHummus
            RegisterType("kitsune_gift", _ => new KitsuneGiftEffect());

            RegisterType("dried_tentacle", _ => new DryTentacleEffect());

            RegisterType("super_hummus", _ => new SuperHummusEffect());

            // 原版 BimboChampagne / BlackCatBeer
            RegisterType("bimbo_champagne", _ => new BimboChampagneEffect());

            RegisterType("black_cat_beer", _ => new BlackCatBeerEffect());

            RegisterType("succubis_dream", _ => new SuccubisDreamEffect());

            RegisterType("satyr_wine", _ => new SatyrWineEffect());

            RegisterType("ringtail_fig", _ => new RingtailFigEffect());

            RegisterType("taurinum", _ => new TaurinumEffect());

            RegisterType("bimbo_liqueur", _ => new BimboLiqueurEffect());

            RegisterCanUse("bimbo_liqueur", player => !player.HasPerk(Datas.Vanilla.PerkIds.FutaForm));

            RegisterType("red_river_root", _ => new RedRiverRootEffect());

            RegisterType("unlabeled_milk", _ => new UnlabeledBrownMilkBottleEffect());

            RegisterType("whisker_fruit", _ => new WhiskerFruitEffect());

            RegisterType("slimy_cloth", _ => new SlimyClothEffect());

            RegisterType("mouse_cocoa", _ => new MouseCocoaEffect());

            RegisterType("echidna_cake", _ => new EchidnaCakeEffect());

            RegisterType("wolf_pepper", _ => new WolfPepperEffect());

            RegisterType("salamander_firewater", _ => new SalamanderFirewaterEffect());

            RegisterType("rhino_steak", _ => new RhinoSteakEffect());

            RegisterType("minotaur_blood", _ => new MinotaurBloodEffect());

            RegisterType("neon_pink_egg", _ => new NeonPinkEggEffect());

            RegisterType("ton_o_trice", _ => new TonOTriceEffect());

            RegisterType("reptilum", _ => new ReptilumEffect());
        }

        public static Registry<ConsumableEffectDefinition> Definitions => ManagerInstance<ConsumableEffectManager>.Instance.Registry;

        public static void RegisterType(string type, Func<ConsumableEffectDefinition, IConsumableEffect> factory) => Types[type] = factory;

        internal static int RegisterTypes(Type[] types)
        {
            var count = 0;

            foreach (var type in types)
            {
                if (type.IsAbstract || !typeof(IConsumableEffect).IsAssignableFrom(type))
                    continue;

                var attributes = type.GetCustomAttributes(typeof(RegisterAttribute), false);

                if (attributes.Length == 0)
                    continue;

                var ctor = type.GetConstructor(Type.EmptyTypes);

                if (ctor == null)
                    continue;

                foreach (RegisterAttribute attribute in attributes)
                {
                    Types[attribute.Id] = _ => (IConsumableEffect)ctor.Invoke(null);

                    count++;
                }
            }

            return count;
        }

        public static IConsumableEffect Create(ConsumableEffectDefinition definition)
        {
            if (!Types.TryGetValue(definition.Type, out var factory))
                throw new InvalidOperationException($"Unknown consumable effect type '{definition.Type}' for '{definition.Id}'");

            return factory(definition);
        }

        public static void RegisterCanUse(string type, Func<Creatures.Player, bool> check) => CanUseChecks[type] = check;

        public static bool CanUse(Identifier id)
        {
            if (!Definitions.TryGet(id, out var definition))
                return true;

            return !CanUseChecks.TryGetValue(definition.Type, out var check) || check(CoC.Instance.Player);
        }

        public static void RegisterItem(Identifier id, Func<IConsumableEffect> factory) => Register(id, factory);

        public static bool HasItem(Identifier id) => Definitions.Has(id);

        public static IConsumableEffect? ResolveItem(Identifier id) => Resolve(id);

        public static void Register(Identifier id, Func<IConsumableEffect> factory) =>

            Definitions.Register(id, () => new ConsumableEffectDefinition { Id = id, Effect = factory() });

        public static IConsumableEffect? Resolve(Identifier id) => Definitions.TryGet(id, out var definition) ? definition.Effect : null;
    }
}