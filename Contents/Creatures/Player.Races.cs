using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using System.Linq;

namespace MonoCoC.Contents.Creatures
{
    public partial class Player
    {
        public string PartId<T>() where T : BodyPart => Body.Get<T>().Id.Path;

        public bool IsPart<T>(params string[] ids) where T : BodyPart => ids.Contains(PartId<T>());

        public string HairType() => PartId<HairPart>();

        public string SkinTone => Body.Get<SkinPart>().Tone;

        public string SkinAdj => Body.Get<SkinPart>().Adj;

        public bool HasWool() => Body.Get<SkinPart>().Type.GetBool("wool");

        public bool HasScales() => IsPart<SkinPart>("lizard_scales", "dragon_scales", "fish_scales");

        public bool HasPlainSkin() => IsPart<SkinPart>("plain");

        public bool HasGooSkin() => IsPart<SkinPart>("goo");

        public bool HasCatFace() => IsPart<FacePart>("cat", "catgirl");

        public bool HasDragonHorns(bool fourHorns = false) =>

            (!fourHorns && IsPart<HornsPart>("draconic_x2") && Body.Get<HornsPart>().Value > 0) ||

            IsPart<HornsPart>("draconic_x4_12_inch_long");

        public bool HasBatLikeWings() => IsPart<WingsPart>("bat_like_tiny", "bat_like_large");

        public bool HasDragonNeck() => IsPart<NeckPart>("draconic") && Body.Get<NeckPart>().Len >= 30;

        public bool HasDragonRearBody() => IsPart<RearBodyPart>("draconic_mane", "draconic_spikes");

        public bool HasFurryUnderBody() => IsPart<UnderBodyPart>("furry");

        // 原版 Creature.hx:3382 hasFeathers()
        public bool HasFeathers() => Body.Get<SkinPart>().Type.GetBool("feathers");

        // 原版 PlayerHelper.hx:28 hasUnderBody(noSnakes = false)
        public bool HasUnderBody(bool noSnakes = false)
        {
            var id = PartId<UnderBodyPart>();

            if (id == "none")
                return false;

            if (noSnakes && (id == "naga" || id == "mermaid"))
                return false;

            return true;
        }

        // 原版 PlayerHelper.hx hasFeatheredUnderBody()
        public bool HasFeatheredUnderBody() => HasUnderBody() && Body.Get<UnderBodyPart>().Skin.HasFeathers;

        // 原版 PlayerHelper.hx hasCockatriceSkin()
        public bool HasCockatriceSkin() => IsPart<SkinPart>("lizard_scales") && IsPart<UnderBodyPart>("cockatrice");
        // 原版 PlayerHelper.hx:159 hasReptileTail()
        public bool HasReptileTail() => IsPart<TailPart>("lizard", "draconic", "salamander");

        // 原版 PlayerHelper.hx hasReptileArms()
        public bool HasReptileArms() => IsPart<ArmsPart>("salamander", "dragon", "lizard");

        // 原版 PlayerHelper.hx hasReptileLegs()
        public bool HasReptileLegs() => IsPart<LowerBodyPart>("lizard", "dragon", "salamander");

        // 原版 PlayerHelper.hx hasDraconicBackSide()：龙翼（大）+ 龙鳞 + 爬虫尾/臂/腿
        public bool HasDraconicBackSide() =>

            HasDragonWings(true) && HasDragonScales() && HasReptileTail() && HasReptileArms() && HasReptileLegs();
    }
}