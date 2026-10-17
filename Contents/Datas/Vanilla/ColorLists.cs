using System;
using System.Linq;

namespace MonoCoC.Contents.Datas.Vanilla
{

    public static class ColorLists
    {
        public static readonly string[] HumanSkin = ["light", "fair", "olive", "dark", "ebony", "mahogany", "russet"];

        public static readonly string[] GoblinSkin = ["pale yellow", "grayish-blue", "green", "dark green"];

        public static readonly string[] GooMorph = ["green", "purple", "blue", "cerulean", "emerald"];

        public static readonly string[] ImpSkin = ["red", "orange"];

        public static readonly string[] SalamanderSkin = ["light", "fair", "tan", "dark"];

        public static readonly string[] RedPandaHair = ["white", "auburn", "red", "russet"];

        public static readonly string[] HorseFur = ["brown", "chocolate", "auburn", "sandy brown", "caramel", "peach", "black", "midnight black", "dark gray", "gray", "light gray", "silver", "white", "brown and white", "black and white"];

        // 原版 CAT_FUR：上色/下色成对；单色条目下色为 null（旧实现把双色条目拆成了两个坏字符串）
        public static readonly (string Upper, string? Under)[] CatFur =

        [
            ("brown", null), ("chocolate", null), ("auburn", null), ("caramel", null),

            ("orange", null), ("sandy brown", null), ("golden", null), ("black", null),

            ("midnight black", null), ("dark gray", null), ("gray", null), ("light gray", null),

            ("silver", null), ("white", null),

            ("orange", "white"), ("brown", "white"), ("black", "white"), ("gray", "white"),
        ];

        public static readonly string[] DogFur = ["brown", "chocolate", "auburn", "caramel", "orange", "black", "dark gray", "gray", "light gray", "silver", "white", "orange and white", "brown and white", "black and white"];

        public static readonly string[] FoxFur = ["{upper:\"orange", "under:\"white\"}", "{upper:\"orange", "under:\"white\"}", "{upper:\"orange", "under:\"white\"}", "{upper:\"red", "under:\"white\"}", "{upper:\"black", "under:\"white\"}", "{upper:\"white\"}", "{upper:\"tan\"}", "{upper:\"brown\"}"];

        public static readonly string[] FerretHair = ["white", "brown", "caramel"];

        public static readonly string[] FerretFur = ["[\"brown", "brown\"]", "[\"brown", "black\"]", "[\"light brown", "caramel\"]", "[\"caramel", "caramel\"]", "[\"caramel", "cream\"]", "[\"silver", "silver\"]", "[\"white", "white\"]", "[\"sandy brown", "brown\"]", "[\"sapphire blue", "electric blue\"]"];

        public static readonly string[] BasicKitsuneHair = ["white", "black", "black", "black", "red", "red", "red"];

        public static readonly string[] BasicKitsuneOuterFur = ["orange", "black", "red", "white"];

        public static readonly string[] BasicKitsuneFur = ["{upper:\"orange", "under: \"white\"}", "{upper:\"black\"}", "{upper:\"black", "under: \"white\"}", "{upper:\"red\"}", "{upper:\"red", "under: \"white\"}", "{upper:\"white\"}"];

        public static readonly string[] ElderKitsune = ["metallic golden", "golden blonde", "metallic silver", "silver blonde", "snow white", "iridescent gray"];

        public static readonly string[] KitsuneSkin = ["tan", "olive", "light"];

        public static readonly string[] KitsuneSkinMystic = ["dark", "ebony", "ashen", "sable", "milky white"];

        public static readonly string[] Cockatrice = ["[\"blue", "turquoise", "blue\"]", "[\"orange", "red", "orange\"]", "[\"green", "yellow", "green\"]", "[\"purple", "pink", "purple\"]", "[\"black", "white", "black\"]", "[\"blonde", "brown", "blonde\"]", "[\"white", "gray", "white\"]"];

        public static readonly string[] GnollSkin = ["black", "dusky-brown"];

        public static readonly string[] MermaidScale = ["blue", "teal", "emerald-green", "red", "pink", "white", "silver"];

        public static bool Contains(this string[] list, string value) =>

            list.Any((x) => string.Equals(x, value, StringComparison.OrdinalIgnoreCase));
    }
}