using MonoCoC.Registry;

namespace MonoCoC.Contents.Items
{

    public static class Undergarments
    {
        public static Item SEA_SHIRT { get; } = ItemRegistry.Create(new Identifier("coc", "sea_shirt"));

        public static Item DSTHONG { get; } = ItemRegistry.Create(new Identifier("coc", "dragon_scale_panties"));

        public static Item EBNVEST { get; } = ItemRegistry.Create(new Identifier("coc", "ebon_vest"));

        public static Item EBNCLTH { get; } = ItemRegistry.Create(new Identifier("coc", "ebon_loincloth"));

        public static Item EBNRJCK { get; } = ItemRegistry.Create(new Identifier("coc", "ebon_jacket"));

        public static Item EBNRTNG { get; } = ItemRegistry.Create(new Identifier("coc", "runed_thong"));

        public static Item EBNRLNC { get; } = ItemRegistry.Create(new Identifier("coc", "runed_loincloth"));

        public static Item EBNCRST { get; } = ItemRegistry.Create(new Identifier("coc", "ebon_corset"));

        public static Item EBNJOCK { get; } = ItemRegistry.Create(new Identifier("coc", "ebon_jock"));

        public static Item EBNTHNG { get; } = ItemRegistry.Create(new Identifier("coc", "ebon_thong"));
    }
}