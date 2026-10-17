using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<GooGirlEnemy>("coc:goo_girl")]
    public class GooGirlEnemy : Enemy
    {

        public bool _playerHasBigBoobs;

        public new string Tone => Body.Get<SkinPart>().Tone;

        public string GooColor() => Tone;

        public string GooColor2() => Tone == "blue" ? "azure" : Tone == "purple" ? "plum" : "crystalline";

        public string GooColor3() => Tone == "blue" ? "cerulean" : Tone == "purple" ? "violet" : "clear";

        public string GooColor4() => Tone == "blue" ? "teal" : Tone == "purple" ? "lavender" : "glassy";

        public string GooColor5() => Tone == "blue" ? "sapphire" : Tone == "purple" ? "amethyst" : "diamond";

        public string GooColor6() => GooColor5();

        public string GooColor7() => Tone == "blue" ? "blueberry" : Tone == "purple" ? "grape" : "crystal";

        public string GooColor8() => Tone == "blue" ? "aquamarine" : Tone == "purple" ? "plum" : "transparent";

        public string GooColor9() => Tone == "blue" ? "an aquamarine" : Tone == "purple" ? "a plum" : "a translucent";

        public string GooColor10() => Tone == "blue" ? "blueberries" : Tone == "purple" ? "grapes" : "strawberries";

        public string GooColor11() => Tone == "blue" ? "cerulean tint" : Tone == "purple" ? "violet tint" : "clear body";

        public override string Description =>

            "Npcs.GooGirl.Desc".Localized()

            + (_playerHasBigBoobs

                ? "Npcs.GooGirl.Desc.BigBoobs".Localized(CoC.Instance.Player.ChestDesc())

                : "");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            var player = CoC.Instance.Player;

            _playerHasBigBoobs = player.BiggestBreastSize() >= 3;

            CreateVagina(false, 5, 1);

            if (!HasStatusEffect(StatusEffectIds.BonusVCapacity))
                CreateStatusEffect(StatusEffectIds.BonusVCapacity, 9001);

            CreateBreastRow(_playerHasBigBoobs ? player.BiggestBreastSize() : 3);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 5;

            if (!HasStatusEffect(StatusEffectIds.BonusACapacity))
                CreateStatusEffect(StatusEffectIds.BonusACapacity, 9001);

            Tallness = Random.Shared.Next(8) + 70;

            Body.SetHipsRating("ample");

            Body.SetButtRating("large");

            Body.Get<LowerBodyPart>().Id = BodyPartTypes.Goo;

            var tone = new[] { "blue", "purple", "crystal" }[Random.Shared.Next(3)];

            var skin = Body.Get<SkinPart>();

            skin.Tone = tone;

            skin.Id = BodyPartTypes.Goo;

            skin.Adj = "goopey";

            var hair = Body.Get<HairPart>();

            hair.Color = tone;

            hair.Length = 12 + Random.Shared.Next(10);

            WeaponName = "hands";

            WeaponVerb = "slap";

            ArmorName = "gelatinous skin";

            LustVulnerability = 0.75f;

            FireResistance = 1.5f;

            Gems = Random.Shared.Next(5) + 1;

            // 原版 Monster.hx 中该字段只出现在一条关于擒抱的注释里，没有任何代码路径会读取它，

        }

        public override void Struggle(Combat.CombatContext context) =>

            Combat.Handlers.GooGirlCombatHandlers.Struggle(context);

        public override bool React(Combat.CombatContext context, Combat.CombatReactionContext reaction)
        {
            if (reaction == Combat.CombatReactionContext.PlayerWaited)
                return Combat.Handlers.GooGirlCombatHandlers.OnPlayerWaited(context);

            if (reaction == Combat.CombatReactionContext.Burned)
                Combat.Handlers.GooGirlCombatHandlers.OnBurned(context);

            return true;
        }
    }
}