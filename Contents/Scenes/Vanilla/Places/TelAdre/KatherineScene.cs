using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.World;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<KatherineScene>("coc:katherine")]
    public class KatherineScene : Scene, ITimeAware
    {

        public const int KBIT_CLOTHES_TATTERED = 0;

        public const int KBIT_CLOTHES_UNIFORM = 1;

        public const int KBIT_CLOTHES_C_CLOTH = 2;

        public const int KBIT_CLOTHES_BODYSUIT = 4;

        public const int KBIT_CLOTHES_B_DRESS = 8;

        public const int KBIT_CLOTHES_SS_ROBE = 16;

        public const int KBIT_CLOTHES_TUBETOP = 32;

        public const int KBIT_CLOTHES_NURSECL = 64;

        public const int KBIT_TRIED_RECEIVE_ANAL = 1;

        public const int KBIT_TRIED_RECEIVE_SUCK_N_FUCK = 2;

        public const int KBIT_TRIED_RECEIVE_ORAL = 4;

        public const int KBIT_TRIED_GIVE_VAGINAL = 8;

        public const int KBIT_TRIED_GIVE_ANAL = 16;

        public const int KBIT_TRIED_GIVE_DOUBLE_PEN = 32;

        public const int KBIT_TRIED_GIVE_SUCK_N_FUCK = 64;

        public const int KBIT_TRIED_DOUBLE_HELIX = 128;

        public const int KBIT_TRIED_BATH = 256;

        public const int KBIT_TRIED_BEDROOM_BONDAGE = 512;

        public const int KBIT_TRIED_DRUNK_BAR_FUCK = 1024;

        public const int KBIT_MILK_TOLD_PLAYER_SHES_DRY = 1;

        public const int KBIT_MILK_SHARE_WITH_FRIENDS = 2;

        public const int KBIT_MILK_SHARE_WITH_OLD_GANG = 4;

        public const int KBIT_MILK_SHARE_WITH_HELENA = 8;

        public const int KBIT_MILK_SHARE_WITH_URTA = 16;

        public const int KBIT_MILK_SHARE_WITH_VALA = 32;

        public const int KBIT_SUB_HIGH_CUM = 1;

        public const int KBIT_SUB_BED_BOND = 2;

        public const int KBIT_SUB_ORGASM_DENIAL = 4;

        public const int KBIT_SUB_CALL_MASTER = 8;

        public const int KBIT_SUB_GROW_KNOT = 16;

        public const int KBIT_SUB_CAT_DICK = 32;

        public const int KBIT_SUB_GROW_BIG_BOOBS = 64;

        public const int KBIT_SUB_GROW_BIG_DICK = 128;

        public const int KBIT_SUB_CAT_GIRL = 256;

        public const int KBIT_SUB_HIGH_LACTATION = 512;

        public const int KBIT_SUB_REM_BALLS = 1024;

        public const int KBIT_SUB_GROW_BIG_BALLS = 2048;

        public const int KBIT_SUB_PUBLIC_EXHIBITION = 4096;

        public const int KBIT_SUB_REM_COCK = 8192;

        public const int KBIT_SUB_START_LACTATION = 16384;

        public const int KLOC_STREETS = 0;

        public const int KLOC_KATHS_APT = 1;

        public const int KLOC_URTAS_APT = 2;

        public const int KLOC_URTAS_HOME = 3;

        public const int KLOC_BAR = 4;

        public const int KLOC_BAR_DRUNK = 5;

        public const int KLOC_BAR_URTA_REFUSED = 6;

        public const int KLOC_LAKE = 7;

        public const int KLOC_DESERT = 8;

        public const int KDATE_LITTLE = 0;

        public const int KDATE_WHENEVER = 1;

        public const int KDATE_LOTS = 2;

        public const int KBIT_LOVER_AMILY = 1;

        public const int KBIT_LOVER_COTTON = 2;

        public const int KBIT_LOVER_EDRYN = 4;

        public const int KBIT_LOVER_HELIA = 8;

        public const int KBIT_LOVER_URTA = 16;

        public const int KBIT_LOVER_VALA = 32;

        // 原版 Katherine.hx:103：_breasts = new BreastStore(KFLAGS.KATHERINE_BREAST_SIZE)
        private BreastStore? _breasts;

        public BreastStore breasts => _breasts ??= new BreastStore(GameFlagIds.KatherineBreastSize);

        public int ballSize { get => CoC.Instance.Flags.Get(GameFlagIds.KatherineBallSize); set => CoC.Instance.Flags.Set(GameFlagIds.KatherineBallSize, value); }

        public int cockLength { get => CoC.Instance.Flags.Get(GameFlagIds.KatherineDickLength); set => CoC.Instance.Flags.Set(GameFlagIds.KatherineDickLength, value); }

        public int cockNumber { get => CoC.Instance.Flags.Get(GameFlagIds.KatherineDickCount); set => CoC.Instance.Flags.Set(GameFlagIds.KatherineDickCount, value); }

        public int knotSize { get => CoC.Instance.Flags.Get(GameFlagIds.KatherineKnotThickness); set => CoC.Instance.Flags.Set(GameFlagIds.KatherineKnotThickness, value); }

        public bool fertile => false;

        public bool furry { get => CoC.Instance.Flags.Get(GameFlagIds.KatherineIsCatGirl) == 0; set => CoC.Instance.Flags.Set(GameFlagIds.KatherineIsCatGirl, value ? 0 : 1); }

        public string hairColor { get => CoC.Instance.Flags.Get(GameFlagIds.KatherineHairColor); set => CoC.Instance.Flags.Set(GameFlagIds.KatherineHairColor, value); }

        public override void Entry()
        {
            BarFirstEncounter();
        }

        // 原版 Katherine.hx:114 timeChange()：挤奶日程 / 每日着装 / Urta 与 Vala 的约会排程
        public virtual bool TimeChange()
        {
            var flags = CoC.Instance.Flags;

            var hours = View.Time.Hours;

            var days = View.Time.Days;

            if (breasts.LactationLevel <= breasts.PreventLactationIncrease)
                switch (hours)
                {
                    case 8:
                        if (MilkOption(KBIT_MILK_SHARE_WITH_FRIENDS)
                            || (MilkOption(KBIT_MILK_SHARE_WITH_OLD_GANG) && days % 2 == 0))

                            breasts.Milked();

                        break;

                    case 11:
                        if (MilkOption(KBIT_MILK_SHARE_WITH_FRIENDS | KBIT_MILK_SHARE_WITH_URTA) && days % 5 == 0)
                            breasts.Milked();

                        break;

                    case 14:
                        if (MilkOption(KBIT_MILK_SHARE_WITH_FRIENDS) && days % 4 == 0)
                            breasts.Milked();

                        break;

                    case 16:
                        if (MilkOption(KBIT_MILK_SHARE_WITH_FRIENDS | KBIT_MILK_SHARE_WITH_OLD_GANG | KBIT_MILK_SHARE_WITH_HELENA) && days % 3 == 0)
                            breasts.Milked();

                        break;

                    case 17:
                        if (MilkOption(KBIT_MILK_SHARE_WITH_FRIENDS | KBIT_MILK_SHARE_WITH_VALA) && days % 5 == 2)
                            breasts.Milked();

                        break;
                }

            // 原版 Katherine.hx:145：Katherine 自己的乳房每小时回充
            breasts.AdvanceTime();

            flags.Set(GameFlagIds.KatherineHoursSinceCum, flags.Get(GameFlagIds.KatherineHoursSinceCum) + 1);

            // 原版 Katherine.hx:144：尚未被雇用（<4）时始终回到 Oswald 后巷
            if (flags.Get(GameFlagIds.KatherineUnlocked) < 4)
            {
                flags.Set(GameFlagIds.KatherineLocation, KLOC_STREETS);

                return false;
            }

            if (hours == 6)
            {
                flags.Set(GameFlagIds.KatherineLocation, KLOC_STREETS);

                flags.Set(GameFlagIds.KatherineClothesWorn, KBIT_CLOTHES_UNIFORM);

                if (flags.Get(GameFlagIds.KatherineUrtaDate) == KDATE_WHENEVER)
                {
                    if (days % 4 == 0)
                        KatherineAndUrtaHadSex(false);
                }
                else if (flags.Get(GameFlagIds.KatherineUrtaDate) == KDATE_LOTS)
                    KatherineAndUrtaHadSex(false);

                if (flags.Get(GameFlagIds.KatherineValaDate) == KDATE_WHENEVER)
                {
                    if (days % 4 == 0)
                        KatherineAndValaHadSex();
                }
                else if (flags.Get(GameFlagIds.KatherineValaDate) == KDATE_LOTS)
                    KatherineAndValaHadSex();
            }
            else if (hours == 10)
            {
                // 原版 Katherine.hx:169：下班后按玩家偏好或随机挑选今天的便装
                var clothesPref = flags.Get(GameFlagIds.KatherineClothesPref);

                if (clothesPref < 0)
                    flags.Set(GameFlagIds.KatherineClothesWorn, -clothesPref);
                else if (clothesPref > 0 && Random.Shared.Next(3) != 0)
                    flags.Set(GameFlagIds.KatherineClothesWorn, clothesPref);
                else
                {
                    // 原版 Katherine.hx:176-195：从「已拥有且非当前偏好」的衣服里随机挑一件
                    var clothesPref2 = flags.Get(GameFlagIds.KatherineClothesWorn);

                    var ownedClothes = flags.Get(GameFlagIds.KatherineClothes);

                    var pool = new List<int>();

                    if (clothesPref2 != KBIT_CLOTHES_C_CLOTH)
                        pool.Add(KBIT_CLOTHES_C_CLOTH); // 始终拥有破布衣

                    if (clothesPref2 != KBIT_CLOTHES_BODYSUIT && (ownedClothes & KBIT_CLOTHES_BODYSUIT) > 0)
                        pool.Add(KBIT_CLOTHES_BODYSUIT);

                    if (clothesPref2 != KBIT_CLOTHES_B_DRESS && (ownedClothes & KBIT_CLOTHES_B_DRESS) > 0)
                        pool.Add(KBIT_CLOTHES_B_DRESS);

                    if (clothesPref2 != KBIT_CLOTHES_SS_ROBE && (ownedClothes & KBIT_CLOTHES_SS_ROBE) > 0)
                        pool.Add(KBIT_CLOTHES_SS_ROBE);

                    if (clothesPref2 != KBIT_CLOTHES_TUBETOP && (ownedClothes & KBIT_CLOTHES_TUBETOP) > 0)
                        pool.Add(KBIT_CLOTHES_TUBETOP);

                    if (clothesPref2 != KBIT_CLOTHES_NURSECL && (ownedClothes & KBIT_CLOTHES_NURSECL) > 0)
                        pool.Add(KBIT_CLOTHES_NURSECL);

                    flags.Set(GameFlagIds.KatherineClothesWorn, pool[Random.Shared.Next(pool.Count)]);
                }
            }

            return false;
        }

        // 原版 Katherine.hx:timeChangeLarge()
        public virtual bool TimeChangeLarge() => false;

        public bool IsValaAtBar()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.FreedVala) != 0 && View.Time.Hours >= 12 && View.Time.Hours <= 21;
        }

        public void InitFlags()
        {

            // 原版 Katherine.hx：初始化 Katherine 自己的 BreastStore 为 A 杯、未泌乳
            breasts.BreastRating = 1;

            breasts.LactationLevel = 0;

            CoC.Instance.Flags.Set(GameFlagIds.KatherineDickCount, 1);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineDickLength, 8);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineKnotThickness, 6);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineBallSize, 1);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineHairColor, "neon pink");

            CoC.Instance.Flags.Set(GameFlagIds.KatherineHoursSinceCum, 200);

            Player.RemoveKeyItem("Silver Kitty-Bell");
        }

        public float CockWidth()
        {

            return HasDogCock() ? 1.5f : 1f;
        }

        public float CockArea()
        {

            return CockWidth() * cockLength;
        }

        public bool HasBalls()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.KatherineBallSize) > 0;
        }

        public bool HasCock()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.KatherineDickCount) > 0;
        }

        public bool IsAt(int location)
        {

            return CoC.Instance.Flags.Get(GameFlagIds.KatherineLocation) == location;
        }

        public bool HasClothing(int clothing)
        {

            return (CoC.Instance.Flags.Get(GameFlagIds.KatherineClothes) & clothing) != 0;
        }

        public void GiveClothing(int clothing)
        {

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothes, CoC.Instance.Flags.Get(GameFlagIds.KatherineClothes) | clothing);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesWorn, clothing);
        }

        public string CockMultiple(string single, string double_)
        {

            return CoC.Instance.Flags.Get(GameFlagIds.KatherineDickCount) <= 1 ? single : double_;
        }

        public bool HasDogCock()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.KatherineDickForm) == 0;
        }

        public string CockType(string dogText = "canine", string catText = "feline")
        {

            return CoC.Instance.Flags.Get(GameFlagIds.KatherineDickForm) == 0 ? dogText : catText;
        }

        public string CatGirl(string isCatGirl, string isCatMorph)
        {

            return (CoC.Instance.Flags.Get(GameFlagIds.KatherineIsCatGirl) == 0 && !noFur) ? isCatMorph : isCatGirl;
        }

        public bool HasAlready(int sex)
        {

            return (CoC.Instance.Flags.Get(GameFlagIds.KatherineSexualExperience) & sex) != 0;
        }

        public void Experience(int sex)
        {

            CoC.Instance.Flags.Set(GameFlagIds.KatherineSexualExperience, CoC.Instance.Flags.Get(GameFlagIds.KatherineSexualExperience) | sex);
        }

        public bool MilkOption(int value)
        {

            return (CoC.Instance.Flags.Get(GameFlagIds.KatherineMilkOptions) & value) != 0;
        }

        public void MilkOptionClear(int value)
        {

            CoC.Instance.Flags.Set(GameFlagIds.KatherineMilkOptions, CoC.Instance.Flags.Get(GameFlagIds.KatherineMilkOptions) & ~value);
        }

        public void MilkOptionSet(int value)
        {

            CoC.Instance.Flags.Set(GameFlagIds.KatherineMilkOptions, CoC.Instance.Flags.Get(GameFlagIds.KatherineMilkOptions) | value);
        }

        public float CumQ()
        {

            var quantity = 40 * Math.Min(168, CoC.Instance.Flags.Get(GameFlagIds.KatherineHoursSinceCum) + 10) / 24f;

            if (ballSize <= 0)
                quantity *= 0.75f + 0.25f * (-ballSize);
            else
                quantity *= ballSize;

            return quantity + 300;
        }

        public void Orgasm()
        {

            CoC.Instance.Flags.Set(GameFlagIds.KatherineHoursSinceCum, 0);
        }

        public string FertileText(string FertileText, string infertileText)
        {

            return fertile ? FertileText : infertileText;
        }

        public int PregSize()
        {

            return 0;
        }

        public string BallAdj()
        {

            var __sw0_0 = CoC.Instance.Flags.Get(GameFlagIds.KatherineBallSize);

            if (__sw0_0 == 0 || __sw0_0 == 1)
                return "small";

            if (__sw0_0 == 2 || __sw0_0 == 3)
                return "large";

            if (__sw0_0 == 4 || __sw0_0 == 5)
                return "huge";

            if (__sw0_0 == 6 || __sw0_0 == 7)
                return "massive";

            if (true )
                return "enormous";
        }

        public string CockAdj()
        {

            var __sw0_0 = CoC.Instance.Flags.Get(GameFlagIds.KatherineDickLength);

            if (__sw0_0 == 0 || __sw0_0 == 1 || __sw0_0 == 2 || __sw0_0 == 3 || __sw0_0 == 4 || __sw0_0 == 5)
                return "little ";

            if (__sw0_0 == 6 || __sw0_0 == 7)
                return " ";

            if (__sw0_0 == 8 || __sw0_0 == 9 || __sw0_0 == 10)
                return "sizable ";

            if (__sw0_0 == 11 || __sw0_0 == 12 || __sw0_0 == 13 || __sw0_0 == 14)
                return "large ";

            if (__sw0_0 == 15 || __sw0_0 == 16 || __sw0_0 == 17 || __sw0_0 == 18)
                return "huge ";

            if (true )
                return "enormous ";
        }

        public string KnotAdj()
        {

            var __sw0_0 = CoC.Instance.Flags.Get(GameFlagIds.KatherineKnotThickness);

            if (__sw0_0 == 0 || __sw0_0 == 1 || __sw0_0 == 2 || __sw0_0 == 3)
                return "small";

            if (__sw0_0 == 4 || __sw0_0 == 5)
                return "sizable";

            if (true )
                return "huge";
        }

        public bool IsMilky()
        {

            return breasts.MilkFullness >= 100 && breasts.Lactation >= 3;
        }

        public void TightTopDescption()
        {

            var __sw0_0 = breasts.BreastRating;

            if (__sw0_0 == BreastCups.Flat || __sw0_0 == 0 || __sw0_0 == 1)
                OutputText("Scenes.Katherine.TightTopDescption.ShowcaseSmallBreasts".Localized());

            if (__sw0_0 == 2 || __sw0_0 == 3 || __sw0_0 == 4)
                OutputText("Scenes.Katherine.TightTopDescption.HoldBreastsInCheck".Localized());

            if (true )
                OutputText("Scenes.Katherine.TightTopDescption.OverflowingTitflesh".Localized());
        }

        public bool CanWearMiniSkirt()
        {

            return cockLength <= 8 && ballSize < 3;
        }

        public string SkirtDescription()
        {

            if (CanWearMiniSkirt())
                return " It's more than long enough to successfully conceal her masculine additions.";
            else
                if (cockLength < 18)
                    return " It doesn't quite draw your eye away from the rather unlady-like bulge at her crotch.";
                else
                    return " It barely conceals the enormous bulge that tents the front.";
        }

        public void Clothes()
        {

            var milky = IsMilky();

            switch (CoC.Instance.Flags.Get(GameFlagIds.KatherineClothesWorn))
            {
                case KBIT_CLOTHES_UNIFORM:
                {
                OutputText("Scenes.Katherine.Clothes.LeatherArmorSuit".Localized());
                }

                break;

                case KBIT_CLOTHES_C_CLOTH:
                {
                OutputText("Scenes.Katherine.Clothes.CleanColoredBlouse".Localized());

                if (breasts.BreastRating < 4)
                    OutputText("Scenes.Katherine.Clothes.TightAtChest".Localized());
                else
                    OutputText("Scenes.Katherine.Clothes.StrainsHoldBreasts".Localized());

                if (milky)
                    OutputText("Scenes.Katherine.Clothes.BlouseLittleDamp".Localized());

                OutputText("Scenes.Katherine.Clothes.MatchingSkirt".Localized(
                    HasCock() ? SkirtDescription() : ""
                ));
                }

                break;

                case KBIT_CLOTHES_BODYSUIT:
                {
                OutputText("Scenes.Katherine.Clothes.BodysuitCurveHugging".Localized());

                if (milky)
                    OutputText("Scenes.Katherine.Clothes.FabricDampTransparent".Localized());
                }

                break;

                case KBIT_CLOTHES_B_DRESS:
                {
                OutputText("Scenes.Katherine.Clothes.BallroomDressSequined".Localized());

                if (milky)
                    OutputText("Scenes.Katherine.Clothes.AbsorbentPaddingDamp".Localized());
                }

                break;

                case KBIT_CLOTHES_SS_ROBE:
                {
                OutputText("Scenes.Katherine.Clothes.SpiderSilkRobe".Localized());

                if (milky)
                    OutputText("Scenes.Katherine.Clothes.SilkCleavesWet".Localized());
                }

                break;

                case KBIT_CLOTHES_TUBETOP:
                {
                OutputText("Scenes.Katherine.Clothes.SnugTubeTop".Localized());

                TightTopDescption();

                OutputText("Scenes.Katherine.Clothes.PlaceholderPeriodSpace".Localized());

                if (CanWearMiniSkirt())
                    OutputText("Scenes.Katherine.Clothes.SkintightShorts".Localized());
                else
                    OutputText("Scenes.Katherine.Clothes.MatchingSkirtAgain".Localized(
                        HasCock() ? SkirtDescription() : ""
                    ));
                }

                break;

                case KBIT_CLOTHES_NURSECL:
                {
                OutputText("Scenes.Katherine.Clothes.WhiteNurseOutfit".Localized());

                TightTopDescption();

                OutputText("Scenes.Katherine.Clothes.PlaceholderPeriodSpaceAgain".Localized());

                if (CanWearMiniSkirt())
                    OutputText("Scenes.Katherine.Clothes.WhiteMiniskirt".Localized());
                else
                    OutputText("Scenes.Katherine.Clothes.WhiteSkirtAgain".Localized(
                        HasCock() ? SkirtDescription() : ""
                    ));

                OutputText("Scenes.Katherine.Clothes.WhiteHatTopper".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Katherine.Clothes.RaggedSecondhandClothes".Localized());
        }

        public string ClothesChoice(string comfortableClothesText, string bodysuitText, string dressText, string robeText, string tubeTopText, string nurseClothesText)
        {

            switch (CoC.Instance.Flags.Get(GameFlagIds.KatherineClothesWorn))
            {
                case KBIT_CLOTHES_BODYSUIT:
                {
                return bodysuitText;
                }

                case KBIT_CLOTHES_B_DRESS:
                {
                return dressText;
                }

                case KBIT_CLOTHES_SS_ROBE:
                {
                return robeText;
                }

                case KBIT_CLOTHES_TUBETOP:
                {
                return tubeTopText;
                }

                case KBIT_CLOTHES_NURSECL:
                {
                return nurseClothesText;
                }
            }

            return comfortableClothesText;
        }

        public string ClothesLowerChoice(string pantsText, string skirtText, string bodysuitText, string dressText, string robeText)
        {

            switch (CoC.Instance.Flags.Get(GameFlagIds.KatherineClothesWorn))
            {
                case KBIT_CLOTHES_C_CLOTH:
                {
                return skirtText;
                }

                case KBIT_CLOTHES_BODYSUIT:
                {
                return bodysuitText;
                }

                case KBIT_CLOTHES_B_DRESS:
                {
                return dressText;
                }

                case KBIT_CLOTHES_SS_ROBE:
                {
                return robeText;
                }

                case KBIT_CLOTHES_TUBETOP:
                {
                if (CanWearMiniSkirt())
                    return skirtText;
                }

                break;

                case KBIT_CLOTHES_NURSECL:
                {
                return skirtText;
                }
            }

            return pantsText;
        }

        public int Submissiveness()
        {

            int subFlags = CoC.Instance.Flags.Get(GameFlagIds.KatherineSubFlags);

            var subCount = 0;

            if ((subFlags & KBIT_SUB_HIGH_CUM) > 0)
                subCount += 1;

            if ((subFlags & KBIT_SUB_BED_BOND) > 0)
                subCount += 1;

            if ((subFlags & KBIT_SUB_ORGASM_DENIAL) > 0)
                subCount += 1;

            if ((subFlags & KBIT_SUB_GROW_KNOT) > 0)
                subCount += 1;

            if ((subFlags & KBIT_SUB_CAT_DICK) > 0)
                subCount += 1;

            if ((subFlags & KBIT_SUB_GROW_BIG_BOOBS) > 0)
                subCount += 1;

            if ((subFlags & KBIT_SUB_GROW_BIG_DICK) > 0)
                subCount += 1;

            if ((subFlags & KBIT_SUB_HIGH_LACTATION) > 0)
                subCount += 1;

            if ((subFlags & KBIT_SUB_REM_BALLS) > 0)
                subCount += 1;

            if ((subFlags & KBIT_SUB_GROW_BIG_BALLS) > 0)
                subCount += 1;

            if ((subFlags & KBIT_SUB_PUBLIC_EXHIBITION) > 0)
                subCount += 2;

            if ((subFlags & KBIT_SUB_REM_COCK) > 0)
                subCount += 1;

            if ((subFlags & KBIT_SUB_CAT_GIRL) > 0 || (noFur && subCount > 2))
                subCount += 1;

            if (subCount < 4 && (subFlags & KBIT_SUB_START_LACTATION) > 0)
                subCount += 1;

            return subCount;
        }

        public void AddSubmissive(int behavior)
        {

            CoC.Instance.Flags.Set(GameFlagIds.KatherineSubFlags, CoC.Instance.Flags.Get(GameFlagIds.KatherineSubFlags) | behavior);
        }

        public bool DoneSubmissive(int behavior)
        {

            return (CoC.Instance.Flags.Get(GameFlagIds.KatherineSubFlags) & behavior) > 0;
        }

        public bool PlayerMaster()
        {

            return (CoC.Instance.Flags.Get(GameFlagIds.KatherineSubFlags) & KBIT_SUB_CALL_MASTER) != 0;
        }

        public string PlayerText()
        {

            if ((CoC.Instance.Flags.Get(GameFlagIds.KatherineSubFlags) & KBIT_SUB_CALL_MASTER) > 0)
                return "[master]";

            return Player.ShortName;
        }

        public void KatherineAndUrtaHadSex(bool drunkSex)
        {

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaTimesSex, CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaTimesSex) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) >= 31)
                return ;

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) == 30 && (drunkSex || CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) < 4 || CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaDate) == KDATE_LITTLE))
                return ;

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaAffection, CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) + 1);
        }

        public void KatherineAndValaHadSex()
        {

            CoC.Instance.Flags.Set(GameFlagIds.KatherineValaTimesSex, CoC.Instance.Flags.Get(GameFlagIds.KatherineValaTimesSex) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineValaAffection) > 11)
                return ;

            CoC.Instance.Flags.Set(GameFlagIds.KatherineValaAffection, CoC.Instance.Flags.Get(GameFlagIds.KatherineValaAffection) + 1);
        }

        public bool NeedIntroductionFromScylla()
        {

            return View.Time.Hours > 8 && View.Time.Hours < 18 && Player.HasKeyItem("Silver Kitty-Bell");
        }

        public void KatherineSprite(bool naked = false)
        {

            if (naked)
                return ;
        }

        public void CatMorphIntr()
        {

            OutputText("Scenes.Katherine.CatMorphIntr.ScyllaResting".Localized());
        }

        public void KatherineGreeting()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.KatherineGreeting.ScyllaSmilesGreets".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.BellReminder".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.ScyllaNodsPleased".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.DifferentRoute".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.NunCallsOut".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.CaughtOffGuard".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.VoiceOffSide".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.SpotLonelyCat".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.IndicatesAlleyway".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.AssureInterested".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.PawnshopSuggestion".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.SoundsMuchBetter".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.KatherineNamesHerself".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.CatsSprawledMilk".Localized());

            OutputText("Scenes.Katherine.KatherineGreeting.AlleyUnlockedNotice".Localized());

            InitFlags();

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUnlocked, 1);

            ReturnToCamp(1);
        }

        public void VisitKatherine()
        {
            ClearOutput();

            KatherineSprite();

            ShowImage("katherine-visit-alley");

            if ((CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) == 1) && (SceneInstance<KatherineEmploymentScene>.Get().InitiateTraining()))
                    return ;

            OutputText("Scenes.Katherine.VisitKatherine.BackAlleyDesc".Localized());

            KatherineVisitNormal();
        }

        public void KatherineVisitNormal(bool checkTraining = true)
        {

            OutputText("Scenes.Katherine.KatherineVisitNormal.KatherineCurrently".Localized());

            switch (Random.Shared.Next(5))
            {
                case 0:
                {
                OutputText("Scenes.Katherine.KatherineVisitNormal.SittingInCorner".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Katherine.KatherineVisitNormal.PacingBackForth".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Katherine.KatherineVisitNormal.SippingMilkBottle".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Katherine.KatherineVisitNormal.YawningStretching".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Katherine.KatherineVisitNormal.WakingFromNap".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) == 1)
            {
                OutputText("Scenes.Katherine.KatherineVisitNormal.SmilesSeesYou".Localized());

                if (checkTraining && CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) > 2)
                {
                    SceneInstance<KatherineEmploymentScene>.Get().TalkToKath();

                    return ;
                }

                KatherineMenu();
            }
            else
            {
                OutputText("Scenes.Katherine.KatherineVisitNormal.SentencePeriodParagraphBreak".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) >= 100)
                {
                    if (GameSettings.UrtaDisabled)
                        SceneInstance<KatherineEmploymentScene>.Get().KatherineTrainingCompleteThereIsNoUrta();
                    else
                        SceneInstance<KatherineEmploymentScene>.Get().KatherineTrainingComplete();
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) >= 66)
                        SceneInstance<KatherineEmploymentScene>.Get().KatherineTrainingStage3();
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) >= 33)
                            SceneInstance<KatherineEmploymentScene>.Get().KatherineTrainingStage2();
                        else
                            SceneInstance<KatherineEmploymentScene>.Get().KatherineTrainingStage1(false);
            }
        }

        public void VisitAtHome()
        {
            ClearOutput();

            if (IsAt(KLOC_KATHS_APT))
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) == 31)
                {
                    KatherineSprite();

                    OutputText("Scenes.Katherine.VisitAtHome.KathUrtaOnBed".Localized());

                    OutputText("Scenes.Katherine.VisitAtHome.UrtaBigPlanWorked".Localized());

                    OutputText("Scenes.Katherine.VisitAtHome.KathReallyClose".Localized());

                    OutputText("Scenes.Katherine.VisitAtHome.UrtaBothStillLove".Localized());

                    OutputText("Scenes.Katherine.VisitAtHome.KathHowCelebrate".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaAffection, 32);

                    ClearButtons();

                    if (Player.HasCock())
                        AddNextButton("369", SceneInstance<KatherineThreesomeScene>.Get().ThreeSixtyNine);

                    if (HasCock())
                        AddNextButton("Common.Button.RoastYou".Localized(), SceneInstance<KatherineThreesomeScene>.Get().RoastYou);
                    else
                        AddNextButton("Common.Button.SceneinstanceKatherinethreesomesceneGetWatch".Localized(), () => SceneInstance<KatherineThreesomeScene>.Get().Watch(false));

                    if (Player.HasCock() || Player.HasVagina())
                        AddNextButton("Common.Button.SpitrKath".Localized(), SceneInstance<KatherineThreesomeScene>.Get().SpitroastKath);
                }
                else
                {
                    KatherineSprite();

                    OutputText("Scenes.Katherine.VisitAtHome.KathWaitingBedroom".Localized());

                    Clothes();

                    OutputText("Scenes.Katherine.VisitAtHome.StandsNextToBed".Localized());

                    KatherineMenu();
                }
            }
            else
                if (View.Time.Hours < 10)
                {
                    OutputText("Scenes.Katherine.VisitAtHome.ApartmentEmptyMorning".Localized());

                    KatherineApartmentEmptyLeave();
                }
                else
                    if (IsAt(KLOC_URTAS_APT) || IsAt(KLOC_URTAS_HOME))
                    {
                        OutputText("Scenes.Katherine.VisitAtHome.EmptyMaybeAtUrtas".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.GoToUrtas".Localized(), KatherineAtUrtas);

                        AddButton(1, "Common.Button.Leave".Localized(), KatherineApartmentEmptyLeave);
                    }
                    else
                    {
                        OutputText("Scenes.Katherine.VisitAtHome.KathOutWetBitch".Localized());

                        Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
                    }
        }

        public void KatherineApartmentLeave()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.KatherineApartmentLeave.JustStoppedBy".Localized());

            if (PlayerMaster())
                OutputText("Scenes.Katherine.KatherineApartmentLeave.ComeBackSoon".Localized());
            else
                OutputText("Scenes.Katherine.KatherineApartmentLeave.NextTimeSomeFun".Localized());

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void KatherineApartmentEmptyLeave()
        {

            OutputText("Scenes.Katherine.KatherineApartmentEmptyLeave.LockUpWalkBack".Localized());

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void KatherineAtUrtas()
        {

            OutputText("Scenes.Katherine.KatherineAtUrtas.WalkToUrtas".Localized());

            if (IsAt(KLOC_URTAS_APT))
            {
                OutputText("Scenes.Katherine.KatherineAtUrtas.KnocksAnswerDoor".Localized());

                OutputText("Scenes.Katherine.KatherineAtUrtas.SpeakOfDevil".Localized());

                OutputText("Scenes.Katherine.KatherineAtUrtas.KathHomeForNight".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.ToKaths".Localized(), DateGotoKaths);

                AddButton(1, "Common.Button.ToTheLake".Localized(), DateKathBath);

                AddButton(2, "Common.Button.ToTheBar".Localized(), DateGotoBar);

                AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
            }
            else
            {
                switch (Random.Shared.Next(4))
                {
                    case 0:
                    {
                    OutputText("Scenes.Katherine.KatherineAtUrtas.SneakUpstairsPeek".Localized());

                    OutputText("Scenes.Katherine.KatherineAtUrtas.SmellSexDoorway".Localized());

                    OutputText("Scenes.Katherine.KatherineAtUrtas.SayHelloOrLeave".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.SayHello".Localized(), UrtaPlusKathCuddle);

                    AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
                    }

                    break;

                    case 1:
                    {
                    OutputText("Scenes.Katherine.KatherineAtUrtas.SneakUpstairsPeekAgain".Localized());

                    OutputText("Scenes.Katherine.KatherineAtUrtas.UrtaDryingKathHair".Localized(
                        ClothesChoice("finishes adjusting her blouse", "finishes straightening out her sheer bodysuit", "does up the last few buttons on her long dress", "fiddles with her spider silk robe, seemingly trying to find a way to show off more of her skin", "adjusts her tube top, making sure the outline of her bra shows through the tight fabric", "adjusts the sexy nurse's outfit you gave her and plays with the little white hat that's sitting on her lap")
                    ));

                    OutputText("Scenes.Katherine.KatherineAtUrtas.KathSpotsMirror".Localized());

                    OutputText("Scenes.Katherine.KatherineAtUrtas.UrtaHoldsComb".Localized(
                        hairColor
                    ));

                    OutputText("Scenes.Katherine.KatherineAtUrtas.UrtaKissShakeHead".Localized());

                    OutputText("Scenes.Katherine.KatherineAtUrtas.UrtaHugsOtherSide".Localized());

                    OutputText("Scenes.Katherine.KatherineAtUrtas.KathSnickersSwat".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.ToKaths".Localized(), TakeKathHome);

                    AddButton(1, "Common.Button.ToTheLake".Localized(), DateKathBath);

                    AddButton(2, "Common.Button.ToTheBar".Localized(), TakeKathToTheBar);
                    }

                    break;
                }

                if (true )
                {
                    OutputText("Scenes.Katherine.KatherineAtUrtas.StepInsideFloor".Localized());

            var kids = SceneInstance<UrtaPregsScene>.Get().UrtaKids();

                    if (kids == 1)
                        OutputText("Scenes.Katherine.KatherineAtUrtas.ChildInKathLap".Localized());
                    else
                        OutputText("Scenes.Katherine.KatherineAtUrtas.KidsLyingOnKath".Localized());

                    OutputText("Scenes.Katherine.KatherineAtUrtas.KathBigSmile".Localized());

                    OutputText("Scenes.Katherine.KatherineAtUrtas.KathHugsWhispers".Localized());
                    // 原版 Katherine.hx:961-975：独生子女时按 URTA_FIRSTBORN_GENDER 选性别代词
                    var firstbornMale = kids == 1 && CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) == 1;

                    if (kids == 1)
                        OutputText((firstbornMale

                            ? "Scenes.Katherine.KatherineAtUrtas.HeS"

                            : "Scenes.Katherine.KatherineAtUrtas.SheS").Localized());
                    else
                        OutputText("Scenes.Katherine.KatherineAtUrtas.TheyreFragment".Localized());

                    OutputText("Scenes.Katherine.KatherineAtUrtas.SoCuteInEar".Localized());

                    OutputText("Scenes.Katherine.KatherineAtUrtas.UrtaWalksDownstairs".Localized());

                    if (kids == 1)
                        OutputText((firstbornMale

                            ? "Scenes.Katherine.KatherineAtUrtas.Him"

                            : "Scenes.Katherine.KatherineAtUrtas.Her").Localized());
                    else
                        OutputText("Scenes.Katherine.KatherineAtUrtas.ThemFragment".Localized());

                    OutputText("Scenes.Katherine.KatherineAtUrtas.GrumblingChores".Localized());

                    if (kids == 1)
                        OutputText((firstbornMale

                            ? "Scenes.Katherine.KatherineAtUrtas.HeRacesOffTo"

                            : "Scenes.Katherine.KatherineAtUrtas.SheRacesOffTo").Localized());
                    else
                        OutputText("Scenes.Katherine.KatherineAtUrtas.RaceOffDirections".Localized());

                    OutputText("Scenes.Katherine.KatherineAtUrtas.WithYouthfulEnergy".Localized());

                    OutputText("Scenes.Katherine.KatherineAtUrtas.UrtaQuickHugErrand".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.ToKaths".Localized(), TakeKathHome);

                    AddButton(1, "Common.Button.ToTheLake".Localized(), DateKathBath);

                    AddButton(2, "Common.Button.ToTheBar".Localized(), TakeKathToTheBar);
                }
            }
        }

        public void UrtaPlusKathCuddle()
        {

            if (Player.Speed > 75)
            {
                OutputText("Scenes.Katherine.UrtaPlusKathCuddle.CreepOverBed".Localized());

                if (HasCock())
                    OutputText("Scenes.Katherine.UrtaPlusKathCuddle.YankKnotOut".Localized());
                else
                    OutputText("Scenes.Katherine.UrtaPlusKathCuddle.DigHerIn".Localized(
                        CatGirl("nails", "claws")
                    ));

                OutputText("Scenes.Katherine.UrtaPlusKathCuddle.KathSheepishSorry".Localized());

                if (SceneInstance<UrtaPregsScene>.Get().UrtaKids() == 1)
                    OutputText("Scenes.Katherine.UrtaPlusKathCuddle.YourSon".Localized());
                else
                    OutputText("Scenes.Katherine.UrtaPlusKathCuddle.OneOfYourKids".Localized());

                OutputText("Scenes.Katherine.UrtaPlusKathCuddle.StrokingUrtaEars".Localized());

                OutputText("Scenes.Katherine.UrtaPlusKathCuddle.UrtaSighsPurr".Localized());

                OutputText("Scenes.Katherine.UrtaPlusKathCuddle.KathMumbles".Localized());
            }
            else
            {
                OutputText("Scenes.Katherine.UrtaPlusKathCuddle.CreepOverBedAgain".Localized());

                OutputText("Scenes.Katherine.UrtaPlusKathCuddle.KathRollsOnBack".Localized());

                OutputText("Scenes.Katherine.UrtaPlusKathCuddle.AsIfCouldntTell".Localized());

                OutputText("Scenes.Katherine.UrtaPlusKathCuddle.KathTwistsTurns".Localized());
            }

            OutputText("Scenes.Katherine.UrtaPlusKathCuddle.FeelingGoodRightNow".Localized(
                PlayerText()
            ));

            OutputText("Scenes.Katherine.UrtaPlusKathCuddle.UrtaLaughsScoops".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.BackToKaths".Localized(), DateGotoKaths);

            AddButton(1, "Common.Button.ToTheLake".Localized(), DateKathBath);

            AddButton(2, "Common.Button.BackToTheBar".Localized(), DateGotoBar);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void KatherineOnDuty()
        {
            ClearOutput();

            KatherineSprite();

            OutputText("Scenes.Katherine.KatherineOnDuty.WatchOfficersScene".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) == 4)
            {
                OutputText("Scenes.Katherine.KatherineOnDuty.CallsRunsIntoArms".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineUnlocked, 5);
            }
            else
                OutputText("Scenes.Katherine.KatherineOnDuty.SkidsToStop".Localized());

            OutputText("Scenes.Katherine.KatherineOnDuty.GoodTimingBreak".Localized());

            KatherinesAppearance(false);

            OutputText("Scenes.Katherine.KatherineOnDuty.NakedTailWaving".Localized(
                CatGirl(" girl", "-morph")
            ));

            ClearButtons();

            AddButton(0, "Common.Button.GiveOral".Localized(), HasCock() ? GiveKatOralPenisWingWang : GiveKatOralPussyLicking);

            if (!Player.IsGenderless())
                AddButton(1, "Common.Button.KathLaps".Localized(), KatherineGivesPCOralAllDayLongDotJPG);
            else
                AddButton(1, "Common.Button.KathLaps".Localized(), null);

            if (Player.HasCockThatFits(70))
            {
                AddButton(2, "Common.Button.FuckHer".Localized(), PenetrateKatsVag);

                AddButton(3, "Common.Button.Anal".Localized(), PcPenetratesKatAnally);
            }
            else
            {
                AddButton(2, "Common.Button.FuckHer".Localized(), null);

                AddButton(3, "Common.Button.Anal".Localized(), null);
            }

            AddButton(14, "Common.Button.Leave".Localized(), KatherineOnDutyLeave);
        }

        public void KatherineOnDutyLeave()
        {

            OutputText("Scenes.Katherine.KatherineOnDutyLeave.BestNotGetTrouble".Localized());

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void BarDescription()
        {

            OutputText("Scenes.Katherine.BarDescription.SittingAloneBooth".Localized(
                ClothesChoice("a matching blouse and skirt", "her sheer bodysuit", "the elegant long dress", "her spider silk robe", "that revealing tube top", "the sexy nurse's outfit")
            ));
        }

        public void BarApproach()
        {
            ClearOutput();

            KatherineSprite();

            OutputText("Scenes.Katherine.BarApproach.KathSitsUpPreen".Localized(
                ClothesChoice("adjusting her blouse", "stretching to show off the bodysuit", "adjusting her dress", "adjusting the spider silk robe", "smoothing out the tube top", "adjusting her cute nurse's hat")
            ));

            OutputText("Scenes.Katherine.BarApproach.SitDownSheAsks".Localized());

            KatherineMenu();
        }

        public void BarFirstEncounter()
        {

            KatherineSprite();

            OutputText("Scenes.Katherine.BarFirstEncounter.FamiliarFelineTable".Localized());

            OutputText("Scenes.Katherine.BarFirstEncounter.RegainsComposure".Localized());

            OutputText("Scenes.Katherine.BarFirstEncounter.HandsKeyPaper".Localized(
                PlayerText()
            ));

            OutputText("Scenes.Katherine.BarFirstEncounter.ScratchBehindEar".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUnlocked, 5);

            KatherineMenu();
        }

        public void BarKathUrtaLoveAnnounce()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.BarKathUrtaLoveAnnounce.WetBitchKathUrta".Localized());

            OutputText("Scenes.Katherine.BarKathUrtaLoveAnnounce.UrtaOffersSeat".Localized());

            OutputText("Scenes.Katherine.BarKathUrtaLoveAnnounce.KathReallyClose".Localized());

            OutputText("Scenes.Katherine.BarKathUrtaLoveAnnounce.UrtaBothStillLove".Localized());

            OutputText("Scenes.Katherine.BarKathUrtaLoveAnnounce.KathHowCelebrate".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaAffection, 32);

            ClearButtons();

            if (Player.HasCock())
                AddNextButton("369", SceneInstance<KatherineThreesomeScene>.Get().ThreeSixtyNine);

            if (HasCock())
                AddNextButton("Common.Button.RoastYou".Localized(), SceneInstance<KatherineThreesomeScene>.Get().RoastYou);
            else
                AddNextButton("Common.Button.SceneinstanceKatherinethreesomesceneGetWatch".Localized(), () => SceneInstance<KatherineThreesomeScene>.Get().Watch(false));

            if (Player.HasCock() || Player.HasVagina())
                AddNextButton("Common.Button.SpitrKath".Localized(), SceneInstance<KatherineThreesomeScene>.Get().SpitroastKath);
        }

        public void KatherineMenu()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) < 4)
            {
                ClearButtons();

                AddButton(0, "Common.Button.Sex".Localized(), KatherineSex);

                AddButton(1, "Common.Button.Talk".Localized(), () => TalkToKatherine());

                AddButton(2, "Common.Button.Appearance".Localized(), () => KatherinesAppearance());

                AddButton(3, "Common.Button.GiveItem".Localized(), () => GiveKatherineAnItem());

                AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
            }
            else
                if (IsAt(KLOC_KATHS_APT))
                {
                    ClearButtons();

                    AddButton(0, "Common.Button.Appearance".Localized(), () => KatherinesAppearance());

                    AddButton(1, "Common.Button.Sex".Localized(), KatherineSex);

                    AddButton(2, "Common.Button.GiveItem".Localized(), () => GiveKatherineAnItem());

                    AddButton(5, "Common.Button.Date".Localized(), KatherineDate);

                    AddButton(6, "Common.Button.Talk".Localized(), () => TalkToKatherine());

                    AddButton(14, "Common.Button.Leave".Localized(), KatherineApartmentLeave);
                }
                else
                {
                    ClearButtons();

                    if (PregSize() == 0)
                        AddButton(0, "Scenes.Lumberjack.Button.Drink".Localized(), KatherineDrinkUp);
                    else
                        AddButton(0, "Scenes.Lumberjack.Button.Drink".Localized(), null);

                    AddButton(1, "Common.Button.Sex".Localized(), KatherineSex);

                    AddButton(5, "Common.Button.Date".Localized(), KatherineDate);

                    AddButton(6, "Common.Button.Talk".Localized(), () => TalkToKatherine());

                    AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
                }
        }

        public void TalkToKatherine(Action from = null)
        {
            ClearOutput();

            if (from == null)
                OutputText("Scenes.Katherine.TalkToKatherine.LikeToTalk".Localized());

            if (IsAt(KLOC_STREETS))
            {
                if (from == null)
                    OutputText("Scenes.Katherine.TalkToKatherine.ShyButExcited".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Racialtension".Localized(), KatherineRacialTension);

                AddButton(1, "Common.Button.HerHistory".Localized(), KatherinesHistory);

                AddButton(2, "Common.Button.Gang".Localized(), AskKatherineAboutGang);

                AddButton(3, "Common.Button.DogCock".Localized(), AskKatherineAboutDogCock);

                AddButton(4, "Common.Button.Vagrancy".Localized(), AskKatherineAboutVagrancy);

                AddButton(5, "Common.Button.Loveandlust".Localized(), AskKatherineAboutLoveAndLust);

                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) == 1 && CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) == 1)
                    AddButton(6, "Common.Button.Employment".Localized(), SceneInstance<KatherineEmploymentScene>.Get().EmploymentTalk);
                else
                    AddButton(6, "Common.Button.Employment".Localized(), null);

                AddButton(14, "Common.Button.Back".Localized(), KatherineMenu);

                return ;
            }

            if (from == null)
                OutputText("Scenes.Katherine.TalkToKatherine.LoveToTalk".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Gangs".Localized(), TalkGangs);

            AddButton(1, "Common.Button.TheSceneinstanceKatherinethreesomesceneGetWatch".Localized(), TalkWatch);

            AddButton(2, "Common.Button.HerHome".Localized(), TalkHome);

            AddButton(3, "Common.Button.TheBar".Localized(), TalkWetBitch);

            if (DoneSubmissive(KBIT_SUB_CAT_GIRL))
                AddButton(4, "Common.Button.CatGirl".Localized(), TalkCatGirl);
            else
                AddButton(4, "Common.Button.CatGirl".Localized(), null);

            if (IsAt(KLOC_KATHS_APT) && CoC.Instance.Flags.Get(GameFlagIds.KatherineClothes) >= 4)
                AddButton(5, "Common.Button.Clothes".Localized(), TalkClothes);
            else
                AddButton(5, "Common.Button.Clothes".Localized(), null);

            if (PlayerLovers() > 0)
                AddButton(6, "Common.Button.Lovers".Localized(), TalkLovers);
            else
                AddButton(6, "Common.Button.Lovers".Localized(), null);

            if (Submissiveness() >= 4)
                AddButton(7, "Common.Button.Master".Localized(), TalkMaster);
            else
                AddButton(7, "Common.Button.Master".Localized(), null);

            if (breasts.Lactating())
                AddButton(8, "Common.Button.HerMilk".Localized(), TalkMilk);
            else
                AddButton(8, "Common.Button.HerMilk".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), KatherineMenu);
        }

        public void KatherineRacialTension()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.KatherineRacialTension.NoticeFelineRacism".Localized());

            OutputText("Scenes.Katherine.KatherineRacialTension.KathShrugsGovernment".Localized());

            OutputText("Scenes.Katherine.KatherineRacialTension.DemonsShouldUnite".Localized());

            OutputText("Scenes.Katherine.KatherineRacialTension.MoreUnitedNow".Localized());

            OutputText("Scenes.Katherine.KatherineRacialTension.ClickTongue".Localized());

            TalkToKatherine(KatherineRacialTension);
        }

        public void KatherinesHistory()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.KatherinesHistory.CuriousAboutPast".Localized());

            OutputText("Scenes.Katherine.KatherinesHistory.NothingDramatic".Localized());

            OutputText("Scenes.Katherine.KatherinesHistory.EntireFamilyVagrant".Localized());

            OutputText("Scenes.Katherine.KatherinesHistory.NothingOfSort".Localized());

            OutputText("Scenes.Katherine.KatherinesHistory.VagrantEverSince".Localized());

            OutputText("Scenes.Katherine.KatherinesHistory.AnticipatesReaction".Localized());

            OutputText("Scenes.Katherine.KatherinesHistory.SkepticalNoFix".Localized());

            TalkToKatherine(KatherinesHistory);
        }

        public void AskKatherineAboutGang()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.AskKatherineAboutGang.AskAboutCatGang".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutGang.LooksAroundPromise".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutGang.WhatTheyDo".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) == 1)
            {
                OutputText("Scenes.Katherine.AskKatherineAboutGang.MetWhenAmbushed".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineMetScylla) != 0)
                    OutputText("Scenes.Katherine.AskKatherineAboutGang.AndScylla".Localized());

                OutputText("Scenes.Katherine.AskKatherineAboutGang.ToGetMilk".Localized());

                OutputText("Scenes.Katherine.AskKatherineAboutGang.FlushesYesWell".Localized());
            }
            else
            {
                OutputText("Scenes.Katherine.AskKatherineAboutGang.SentencePeriod".Localized());

                OutputText("Scenes.Katherine.AskKatherineAboutGang.FlushesWell".Localized());
            }

            OutputText("Scenes.Katherine.AskKatherineAboutGang.WeAllLikeMilk".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutGang.PointOutBodilyFluid".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutGang.AgreesOtherMale".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutGang.GrinsWickedlyMouse".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutGang.PictureAndChuckle".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutGang.OthersGaveHell".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutGang.CertainlyWasntExpected".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutGang.SeenThemHappy".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutGang.MullingRememberQuestion".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutGang.PanhandlingPickpocketing".Localized());

            TalkToKatherine(AskKatherineAboutGang);
        }

        public void AskKatherineAboutDogCock()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.AskKatherineAboutDogCock.CuriousDogCock".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutDogCock.SighsAnswerObvious".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutDogCock.EyesToWaist".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutDogCock.WhyNotWhiskerFruit".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutDogCock.LastTraderGone".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutDogCock.WouldSheChangeIt".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutDogCock.AccustomedToDogDick".Localized());

            TalkToKatherine(AskKatherineAboutDogCock);
        }

        public void AskKatherineAboutVagrancy()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.AskKatherineAboutVagrancy.ExplainVagrancy".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutVagrancy.ScowlsGovernment".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutVagrancy.WonderingTruthPrejudice".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, 1);

            TalkToKatherine(AskKatherineAboutVagrancy);
        }

        public void AskKatherineAboutLoveAndLust()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.AskKatherineAboutLoveAndLust.PhraseQuestionPolitely".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutLoveAndLust.BlushesScrapesFoot".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutLoveAndLust.EyesDrawnToCrotch".Localized());

            OutputText("Scenes.Katherine.AskKatherineAboutLoveAndLust.NodsSadlyBeatific".Localized());

            TalkToKatherine(AskKatherineAboutLoveAndLust);
        }

        public void TalkGangs()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.TalkGangs.AskAboutGangs".Localized());

            OutputText("Scenes.Katherine.TalkGangs.WeirdSometimes".Localized());

            OutputText("Scenes.Katherine.TalkGangs.PacesProblemsSame".Localized());

            OutputText("Scenes.Katherine.TalkGangs.CuddlesBothSides".Localized());

            OutputText("Scenes.Katherine.TalkGangs.SighsDifficultThing".Localized());

            OutputText("Scenes.Katherine.TalkGangs.NoEasyAnswers".Localized());

            KatSexMenu();
        }

        public void TalkWatch()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.TalkWatch.HowAreThings".Localized());

            OutputText("Scenes.Katherine.TalkWatch.GiveItThought".Localized());

            OutputText("Scenes.Katherine.TalkWatch.StretchesGreatView".Localized());

            OutputText("Scenes.Katherine.TalkWatch.CentaurChamberpot".Localized());

            OutputText("Scenes.Katherine.TalkWatch.BeatSomeoneUp".Localized());

            OutputText("Scenes.Katherine.TalkWatch.KathPressesOn".Localized());

            OutputText("Scenes.Katherine.TalkWatch.StartingToSettle".Localized());

            if (IsAt(KLOC_KATHS_APT))
                OutputText("Scenes.Katherine.TalkWatch.ShePresses".Localized());
            else
                if (Player.IsTaur())
                    OutputText("Scenes.Katherine.TalkWatch.WrapsArmsWaist".Localized());
                else
                    OutputText("Scenes.Katherine.TalkWatch.SitsOnLapArms".Localized());

            OutputText("Scenes.Katherine.TalkWatch.BodyAgainstYours".Localized());

            KatSexMenu();
        }

        public void TalkHome()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.TalkHome.AskAboutHome".Localized());

            OutputText("Scenes.Katherine.TalkHome.WonderfulEverything".Localized());

            OutputText("Scenes.Katherine.TalkHome.JustTwoRooms".Localized());

            if (IsAt(KLOC_KATHS_APT))
                OutputText("Scenes.Katherine.TalkHome.WalksWhitewashedWalls".Localized());
            else
                OutputText("Scenes.Katherine.TalkHome.FootBrushesYou".Localized());

            KatSexMenu();
        }

        public void TalkClothes()
        {
            ClearOutput();

            ClearButtons();

            AddButton(0, "Common.Button.NoPreference".Localized(), TalkClothesNoPreference);

            AddButton(1, "Common.Button.Blouse".Localized(), TalkClothesBlouse);

            if (HasClothing(KBIT_CLOTHES_BODYSUIT))
                AddButton(2, "Items.Bodysuit.Short".Localized(), TalkClothesBodysuit);
            else
            {
                OutputText("Scenes.Katherine.TalkClothes.WonderLitheBody".Localized(
                    CatGirl("cat girl", "feline")
                ));

                AddButton(2, "Items.Bodysuit.Short".Localized(), null);
            }

            if (HasClothing(KBIT_CLOTHES_B_DRESS))
                AddButton(2, "Common.Button.LongDress".Localized(), TalkClothesDress);
            else
            {
                OutputText("Scenes.Katherine.TalkClothes.BallroomDressIdea".Localized());

                AddButton(2, "Common.Button.LongDress".Localized(), null);
            }

            if (HasClothing(KBIT_CLOTHES_SS_ROBE))
                AddButton(3, "Common.Button.SilkRobe".Localized(), TalkClothesSSRobe);
            else
            {
                OutputText("Scenes.Katherine.TalkClothes.SilkRobeIdea".Localized(
                    CatGirl("sliding over her skin would drive her wild", "would compliment her fur nicely")
                ));

                AddButton(3, "Common.Button.SilkRobe".Localized(), null);
            }

            if (HasClothing(KBIT_CLOTHES_TUBETOP))
                AddButton(4, "Items.TubeTop.Short".Localized(), TalkClothesTubeTop);
            else
            {
                OutputText("Scenes.Katherine.TalkClothes.TubeTopIdea".Localized());

                AddButton(4, "Items.TubeTop.Short".Localized(), null);
            }

            if (HasClothing(KBIT_CLOTHES_NURSECL))
                AddButton(5, "Items.NurseOutfit.Short".Localized(), TalkClothesNurseOutfit);
            else
            {
                OutputText("Scenes.Katherine.TalkClothes.ShiverObsceneClothing".Localized(
                    CatGirl("cat girl", "feline")
                ));

                AddButton(5, "Items.NurseOutfit.Short".Localized(), null);
            }

            OutputText("Scenes.Katherine.TalkClothes.GiveClothingInput".Localized());

            AddButton(14, "Common.Button.Back".Localized(), KatherineMenu);
        }

        public void TalkClothesNoPreference()
        {

            OutputText("Scenes.Katherine.TalkClothesNoPreference.PlayWithTail".Localized());

            OutputText("Scenes.Katherine.TalkClothesNoPreference.PrefersNaked".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, 0);

            KatSexMenu();
        }

        public void TalkClothesBlouse()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineClothesWorn) == KBIT_CLOTHES_C_CLOTH)
                OutputText("Scenes.Katherine.TalkClothesBlouse.ComplimentBlouse".Localized());
            else
            {
                OutputText("Scenes.Katherine.TalkClothesBlouse.PullsBlouseShow".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesWorn, KBIT_CLOTHES_C_CLOTH);
            }

            OutputText("Scenes.Katherine.TalkClothesBlouse.NotAsNice".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Compliment".Localized(), TalkClothesBlouseCompliment);

            AddButton(1, "Common.Button.Suggest".Localized(), TalkClothesBlouseSuggest);

            if (Submissiveness() > 1)
                AddButton(2, "Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), TalkClothesBlouseInsist);
            else
                AddButton(2, "Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), null);
        }

        public void TalkClothesBlouseCompliment()
        {

            OutputText("Scenes.Katherine.TalkClothesBlouseCompliment.CloseDistanceHug".Localized());

            OutputText("Scenes.Katherine.TalkClothesBlouseCompliment.GoodCamouflage".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.TalkClothesBlouseCompliment.BulgePressingSuggests".Localized());
            else
                OutputText("Scenes.Katherine.TalkClothesBlouseCompliment.GrindsPelvis".Localized());

            KatSexMenu();
        }

        public void TalkClothesBlouseSuggest()
        {

            OutputText("Scenes.Katherine.TalkClothesBlouseSuggest.CloseDistanceHug".Localized());

            OutputText("Scenes.Katherine.TalkClothesBlouseSuggest.GoodCamouflage".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.TalkClothesBlouseSuggest.BulgePressingSuggests".Localized());
            else
                OutputText("Scenes.Katherine.TalkClothesBlouseSuggest.GrindsPelvis".Localized());

            OutputText("Scenes.Katherine.TalkClothesBlouseSuggest.MassageEarsLikeIt".Localized());

            OutputText("Scenes.Katherine.TalkClothesBlouseSuggest.NuzzlesNeckPurr".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, KBIT_CLOTHES_C_CLOTH);

            KatSexMenu();
        }

        public void TalkClothesBlouseInsist()
        {

            OutputText("Scenes.Katherine.TalkClothesBlouseInsist.CloseDistanceHug".Localized());

            OutputText("Scenes.Katherine.TalkClothesBlouseInsist.GoodCamouflage".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.TalkClothesBlouseInsist.BulgePressingSuggests".Localized());
            else
                OutputText("Scenes.Katherine.TalkClothesBlouseInsist.GrindsPelvis".Localized());

            OutputText("Scenes.Katherine.TalkClothesBlouseInsist.MassageEarsWearAlways".Localized());

            OutputText("Scenes.Katherine.TalkClothesBlouseInsist.NuzzlesNeckYes".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, -KBIT_CLOTHES_C_CLOTH);

            KatSexMenu();
        }

        public void TalkClothesBodysuit()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineClothesWorn) == KBIT_CLOTHES_BODYSUIT)
                OutputText("Scenes.Katherine.TalkClothesBodysuit.ComplimentBodysuit".Localized());
            else
            {
                OutputText("Scenes.Katherine.TalkClothesBodysuit.AskBodysuit".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesWorn, KBIT_CLOTHES_BODYSUIT);
            }

            ClearButtons();

            AddButton(0, "Common.Button.Compliment".Localized(), TalkClothesBodysuitCompliment);

            AddButton(1, "Common.Button.Suggest".Localized(), TalkClothesBodysuitSuggest);

            if (Submissiveness() > 1)
                AddButton(2, "Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), TalkClothesBodysuitInsist);
            else
                AddButton(2, "Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), null);
        }

        public void TalkClothesBodysuitCompliment()
        {

            OutputText("Scenes.Katherine.TalkClothesBodysuitCompliment.WrapArmsAmazing".Localized());

            OutputText("Scenes.Katherine.TalkClothesBodysuitCompliment.SnugglesPurr".Localized());

            KatSexMenu();
        }

        public void TalkClothesBodysuitSuggest()
        {

            OutputText("Scenes.Katherine.TalkClothesBodysuitSuggest.WrapArmsAmazing".Localized());

            OutputText("Scenes.Katherine.TalkClothesBodysuitSuggest.StrokeEarWearMore".Localized());

            OutputText("Scenes.Katherine.TalkClothesBodysuitSuggest.HugsClosePurrs".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, KBIT_CLOTHES_BODYSUIT);

            KatSexMenu();
        }

        public void TalkClothesBodysuitInsist()
        {

            OutputText("Scenes.Katherine.TalkClothesBodysuitInsist.WrapArmsAmazing".Localized());

            OutputText("Scenes.Katherine.TalkClothesBodysuitInsist.HandThroughHairExpect".Localized());

            OutputText("Scenes.Katherine.TalkClothesBodysuitInsist.UnsureThenTurnedOn".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, -KBIT_CLOTHES_BODYSUIT);

            KatSexMenu();
        }

        public void TalkClothesDress()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineClothesWorn) == KBIT_CLOTHES_B_DRESS)
                OutputText("Scenes.Katherine.TalkClothesDress.ComplimentDress".Localized());
            else
            {
                OutputText("Scenes.Katherine.TalkClothesDress.AskBallroomDress".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesWorn, KBIT_CLOTHES_B_DRESS);
            }

            ClearButtons();

            AddButton(0, "Common.Button.Compliment".Localized(), TalkClothesDressCompliment);

            AddButton(1, "Common.Button.Suggest".Localized(), TalkClothesDressSuggest);

            if (Submissiveness() > 1)
                AddButton(2, "Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), TalkClothesDressInsist);
            else
                AddButton(2, "Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), null);
        }

        public void TalkClothesDressCompliment()
        {

            OutputText("Scenes.Katherine.TalkClothesDressCompliment.AskForDance".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.TalkClothesDressCompliment.RigidProtrusionDance".Localized(
                    CockMultiple(" under her dress tells", "s under her dress tell")
                ));
            else
                OutputText("Scenes.Katherine.TalkClothesDressCompliment.NipplesRubTips".Localized());

            KatSexMenu();
        }

        public void TalkClothesDressSuggest()
        {

            OutputText("Scenes.Katherine.TalkClothesDressSuggest.WrapArmsDressAmazing".Localized(
                CatGirl("girl", "morph")
            ));

            OutputText("Scenes.Katherine.TalkClothesDressSuggest.WearLongDress".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, KBIT_CLOTHES_B_DRESS);

            KatherineMenu();
        }

        public void TalkClothesDressInsist()
        {

            OutputText("Scenes.Katherine.TalkClothesDressInsist.WrapArmsKathSighs".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, -KBIT_CLOTHES_B_DRESS);

            KatherineMenu();
        }

        public void TalkClothesSSRobe()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineClothesWorn) == KBIT_CLOTHES_SS_ROBE)
                OutputText("Scenes.Katherine.TalkClothesSSRobe.RobeSuitsHer".Localized());
            else
            {
                OutputText("Scenes.Katherine.TalkClothesSSRobe.AskSpiderRobe".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesWorn, KBIT_CLOTHES_SS_ROBE);
            }

            ClearButtons();

            AddButton(0, "Common.Button.Compliment".Localized(), TalkClothesSSRobeCompliment);

            AddButton(1, "Common.Button.Suggest".Localized(), TalkClothesSSRobeSuggest);

            if (Submissiveness() > 1)
                AddButton(2, "Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), TalkClothesSSRobeInsist);
            else
                AddButton(2, "Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), null);
        }

        public void TalkClothesSSRobeCompliment()
        {

            OutputText("Scenes.Katherine.TalkClothesSSRobeCompliment.HandsOverSilk".Localized());

            OutputText("Scenes.Katherine.TalkClothesSSRobeCompliment.KissBadInfluence".Localized(
                PlayerText()
            ));

            KatSexMenu();
        }

        public void TalkClothesSSRobeSuggest()
        {

            OutputText("Scenes.Katherine.TalkClothesSSRobeSuggest.HandsOverSilk".Localized());

            OutputText("Scenes.Katherine.TalkClothesSSRobeSuggest.HugAlwaysWilling".Localized(
                PlayerText()
            ));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, KBIT_CLOTHES_SS_ROBE);

            KatherineMenu();
        }

        public void TalkClothesSSRobeInsist()
        {

            OutputText("Scenes.Katherine.TalkClothesSSRobeInsist.FingersUnderChin".Localized());

            OutputText("Scenes.Katherine.TalkClothesSSRobeInsist.TugBeltUndo".Localized());

            OutputText("Scenes.Katherine.TalkClothesSSRobeInsist.ShiversHeatPanties".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, -KBIT_CLOTHES_SS_ROBE);

            KatSexMenu();
        }

        public void TalkClothesTubeTop()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineClothesWorn) == KBIT_CLOTHES_TUBETOP)
                OutputText("Scenes.Katherine.TalkClothesTubeTop.TubeTopFigure".Localized());
            else
            {
                CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesWorn, KBIT_CLOTHES_TUBETOP);

                OutputText("Scenes.Katherine.TalkClothesTubeTop.AskTubeTop".Localized(
                    ClothesLowerChoice("pulls on the tight shorts that came with it.", "rests her hand on the large bulge in her panties. Her hand reaches over the folded short shorts and she pulls on the skirt that matches her blouse.", "", "", "")
                ));
            }

            ClearButtons();

            AddButton(0, "Common.Button.Compliment".Localized(), TalkClothesTubeTopCompliment);

            AddButton(1, "Common.Button.Suggest".Localized(), TalkClothesTubeTopSuggest);

            if (Submissiveness() > 1)
                AddButton(2, "Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), TalkClothesTubeTopInsist);
            else
                AddButton(2, "Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), null);
        }

        public void TalkClothesTubeTopCompliment()
        {

            OutputText("Scenes.Katherine.TalkClothesTubeTopCompliment.PositionBehindHold".Localized());

            KatSexMenu();
        }

        public void TalkClothesTubeTopSuggest()
        {

            OutputText("Scenes.Katherine.TalkClothesTubeTopSuggest.PositionBehindWear".Localized(
                CatGirl("skin", "fur")
            ));

            OutputText("Scenes.Katherine.TalkClothesTubeTopSuggest.PurrsLoveTo".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, KBIT_CLOTHES_TUBETOP);

            KatherineMenu();
        }

        public void TalkClothesTubeTopInsist()
        {

            OutputText("Scenes.Katherine.TalkClothesTubeTopInsist.PositionBehindExpect".Localized());

            OutputText("Scenes.Katherine.TalkClothesTubeTopInsist.GrindsAssGoodReason".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, -KBIT_CLOTHES_TUBETOP);

            KatSexMenu();
        }

        public void TalkClothesNurseOutfit()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineClothesWorn) == KBIT_CLOTHES_NURSECL)
                OutputText("Scenes.Katherine.TalkClothesNurseOutfit.NurseOutfitFigure".Localized());
            else
            {
                OutputText("Scenes.Katherine.TalkClothesNurseOutfit.AskNurseOutfit".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesWorn, KBIT_CLOTHES_NURSECL);
            }

            OutputText("Scenes.Katherine.TalkClothesNurseOutfit.HowManyEyes".Localized());

            OutputText("Scenes.Katherine.TalkClothesNurseOutfit.AnAwfulLot".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Compliment".Localized(), TalkClothesNurseOutfitCompliment);

            AddButton(1, "Common.Button.Suggest".Localized(), TalkClothesNurseOutfitSuggest);

            if (Submissiveness() > 1)
                AddButton(2, "Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), TalkClothesNurseOutfitInsist);
            else
                AddButton(2, "Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), null);
        }

        public void TalkClothesNurseOutfitCompliment()
        {

            OutputText("Scenes.Katherine.TalkClothesNurseOutfitCompliment.TickleBellyBest".Localized());

            OutputText("Scenes.Katherine.TalkClothesNurseOutfitCompliment.GigglesHugsTight".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Katherine.TalkClothesNurseOutfitCompliment.FingerDownReply".Localized());

            KatSexMenu();
        }

        public void TalkClothesNurseOutfitSuggest()
        {

            OutputText("Scenes.Katherine.TalkClothesNurseOutfitSuggest.PositionBehindWhisper".Localized());

            OutputText("Scenes.Katherine.TalkClothesNurseOutfitSuggest.PurrsSultryShowOff".Localized());

            OutputText("Scenes.Katherine.TalkClothesNurseOutfitSuggest.SqueezeBreastsLoveKnowing".Localized());

            OutputText("Scenes.Katherine.TalkClothesNurseOutfitSuggest.AlrightThenPeople".Localized(
                PlayerText()
            ));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, KBIT_CLOTHES_NURSECL);

            KatherineMenu();
        }

        public void TalkClothesNurseOutfitInsist()
        {

            OutputText("Scenes.Katherine.TalkClothesNurseOutfitInsist.PositionBehindWhisperInsist".Localized());

            OutputText("Scenes.Katherine.TalkClothesNurseOutfitInsist.PurrsSultryInsist".Localized());

            OutputText("Scenes.Katherine.TalkClothesNurseOutfitInsist.SqueezeBreastsExpect".Localized());

            OutputText("Scenes.Katherine.TalkClothesNurseOutfitInsist.SharpIntakeBreath".Localized());

            OutputText("Scenes.Katherine.TalkClothesNurseOutfitInsist.YesUnderstandDisplay".Localized(
                PlayerText()
            ));

            OutputText("Scenes.Katherine.TalkClothesNurseOutfitInsist.FingerUpDownSlit".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, -KBIT_CLOTHES_NURSECL);

            KatSexMenu();
        }

        public void TalkLovers()
        {
            ClearOutput();

            var loverSet = PlayerLovers();

            ClearButtons();

            if ((loverSet & KBIT_LOVER_AMILY) != 0)
                AddNextButton("Common.Button.Amily".Localized(), TalkLoversAmily);

            if ((loverSet & KBIT_LOVER_COTTON) != 0)
                AddNextButton("Npcs.Cotton.Name".Localized(), TalkLoversCotton);

            if ((loverSet & KBIT_LOVER_EDRYN) != 0)
                AddNextButton("Common.Button.Edryn".Localized(), TalkLoversEdryn);

            if ((loverSet & KBIT_LOVER_HELIA) != 0)
                AddNextButton("Common.Button.Helia".Localized(), TalkLoversHelia);

            if ((loverSet & KBIT_LOVER_URTA) != 0)
                AddNextButton("Common.Button.Urta".Localized(), TalkLoversUrta);

            if ((loverSet & KBIT_LOVER_VALA) != 0)
                AddNextButton("Npcs.Vala.Name".Localized(), TalkLoversVala);

            OutputText("Scenes.Katherine.TalkLovers.ShouldTalkToKath".Localized());

            if (Buttons.Length > 1)
                OutputText("Scenes.Katherine.TalkLovers.OneOtherLover".Localized());
            else
                if ((loverSet & KBIT_LOVER_AMILY) != 0)
                    OutputText("Scenes.Katherine.TalkLovers.AmilyName".Localized());
                else
                    if ((loverSet & KBIT_LOVER_COTTON) != 0)
                        OutputText("Scenes.Katherine.TalkLovers.CottonName".Localized());
                    else
                        if ((loverSet & KBIT_LOVER_EDRYN) != 0)
                            OutputText("Scenes.Katherine.TalkLovers.EdrynName".Localized());
                        else
                            if ((loverSet & KBIT_LOVER_HELIA) != 0)
                                OutputText("Scenes.Katherine.TalkLovers.HeliaName".Localized());
                            else
                                if ((loverSet & KBIT_LOVER_URTA) != 0)
                                    OutputText("Scenes.Katherine.TalkLovers.UrtaName".Localized());
                                else
                                    if ((loverSet & KBIT_LOVER_VALA) != 0)
                                        OutputText("Scenes.Katherine.TalkLovers.ValaName".Localized());

            AddButton(14, "Common.Button.Back".Localized(), () => TalkToKatherine());
        }

        public int PlayerLovers()
        {

            var loverSet = 0;

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
                loverSet |= KBIT_LOVER_AMILY;

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonMetFucked) >= 2)
                loverSet |= KBIT_LOVER_COTTON;

            if ((CoC.Instance.Flags.Get(GameFlagIds.EdrynNeverSeeAgain) == 0) && (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) > 3))
                loverSet |= KBIT_LOVER_EDRYN;

            if ((CoC.Instance.Flags.Get(GameFlagIds.HeliaFollowerDisabled) != 1) && SceneInstance<HelScene>.Get().FollowerHel())
                loverSet |= KBIT_LOVER_HELIA;

            if (SceneInstance<UrtaScene>.Get().UrtaFuckbuddy())
                loverSet |= KBIT_LOVER_URTA;

            if (CoC.Instance.Flags.Get(GameFlagIds.ValaTimesConsensualSex) > 0)
                loverSet |= KBIT_LOVER_VALA;

            return loverSet;
        }

        public int KnownLovers()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.KatherineAmilyAffection) > 0 ? 1 : 0;
        }

        public void TalkLoversAmily()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineAmilyAffection) == 0)
            {
                var numLovers = KnownLovers();

                OutputText("Scenes.Katherine.TalkLoversAmily.TellAboutAmily".Localized());

                if (numLovers > 5)
                    OutputText("Scenes.Katherine.TalkLoversAmily.ShakesHeadEnergy".Localized());
                else
                    if (numLovers > 2)
                        OutputText("Scenes.Katherine.TalkLoversAmily.LaughsBusyChampion".Localized());
                    else
                        OutputText("Scenes.Katherine.TalkLoversAmily.WorriedLookMeaning".Localized());

                OutputText("Scenes.Katherine.TalkLoversAmily.AnswersQuestions".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineAmilyAffection, 1);
            }
            else
                OutputText("Scenes.Katherine.TalkLoversAmily.CampNewsUpdate".Localized());

            TalkToKatherine(TalkLoversAmily);
        }

        public void TalkLoversCotton()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineCottonAffection) == 0)
            {
                var numLovers = KnownLovers();

                OutputText("Scenes.Katherine.TalkLoversCotton.TellAboutCotton".Localized());

                if (numLovers > 5)
                    OutputText("Scenes.Katherine.TalkLoversCotton.ShakesHeadEnergy".Localized());
                else
                    if (numLovers > 2)
                        OutputText("Scenes.Katherine.TalkLoversCotton.LaughsBusyChampion".Localized());
                    else
                        OutputText("Scenes.Katherine.TalkLoversCotton.WorriedLookMeaning".Localized());

                OutputText("Scenes.Katherine.TalkLoversCotton.AnswersQuestions".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) > 0)
                {
                    OutputText("Scenes.Katherine.TalkLoversCotton.MentionsCottonChild".Localized());

                    OutputText("Scenes.Katherine.TalkLoversCotton.CottonUsedHerbs".Localized());
                }

                CoC.Instance.Flags.Set(GameFlagIds.KatherineCottonAffection, 1);
            }
            else
                OutputText("Scenes.Katherine.TalkLoversCotton.RecentGymVisits".Localized());

            TalkToKatherine(TalkLoversCotton);
        }

        public void TalkLoversEdryn()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineEdrynAffection) == 0)
            {
                var numLovers = KnownLovers();

                int edrynKids = CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids);

                OutputText("Scenes.Katherine.TalkLoversEdryn.TellAboutEdryn".Localized());

                if (numLovers > 5)
                    OutputText("Scenes.Katherine.TalkLoversEdryn.ShakesHeadEnergy".Localized());
                else
                    if (numLovers > 2)
                        OutputText("Scenes.Katherine.TalkLoversEdryn.LaughsBusyChampion".Localized());
                    else
                        OutputText("Scenes.Katherine.TalkLoversEdryn.WorriedLookMeaning".Localized());

                OutputText("Scenes.Katherine.TalkLoversEdryn.AnswersQuestions".Localized());

                if (edrynKids > 0)
                    OutputText("Scenes.Katherine.TalkLoversEdryn.FatheredWithEdryn".Localized(
                        edrynKids > 1 ? edrynKids + " children" : "a child",

                        edrynKids > 1 ? edrynKids + " kids" : "a kid"
                    ));

                OutputText("Scenes.Katherine.TalkLoversEdryn.PleasedWithFrankness".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineEdrynAffection, 1);
            }
            else
                OutputText("Scenes.Katherine.TalkLoversEdryn.TalkAboutHerWork".Localized());

            TalkToKatherine(TalkLoversEdryn);
        }

        public void TalkLoversHelia()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineHeliaAffection) == 0)
            {
                var numLovers = KnownLovers();

                OutputText("Scenes.Katherine.TalkLoversHelia.TellAboutHelia".Localized());

                if (numLovers > 5)
                    OutputText("Scenes.Katherine.TalkLoversHelia.ShakesHeadEnergy".Localized());
                else
                    if (numLovers > 2)
                        OutputText("Scenes.Katherine.TalkLoversHelia.LaughsBusyChampion".Localized());
                    else
                        OutputText("Scenes.Katherine.TalkLoversHelia.WorriedLookMeaning".Localized());

                OutputText("Scenes.Katherine.TalkLoversHelia.ExplainsSalamanders".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnAge) > 0)
                    if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 0)
                        OutputText("Scenes.Katherine.TalkLoversHelia.SurprisedSalamanderBaby".Localized());
                    else
                        OutputText("Scenes.Katherine.TalkLoversHelia.ApprovesRaisingGirl".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineHeliaAffection, 1);
            }
            else
                OutputText("Scenes.Katherine.TalkLoversHelia.CampNewsUpdate".Localized());

            TalkToKatherine(TalkLoversHelia);
        }

        public void TalkLoversUrta()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) == 0)
            {
                var urtaKids = SceneInstance<UrtaPregsScene>.Get().UrtaKids();

                OutputText("Scenes.Katherine.TalkLoversUrta.TellAboutUrta".Localized());

                OutputText("Scenes.Katherine.TalkLoversUrta.StoryOfMeetingUrta".Localized());

                if (urtaKids > 0)
                    OutputText("Scenes.Katherine.TalkLoversUrta.AlmostChokesHearing".Localized(
                        urtaKids == 1 ? "Common.Text.HaveAKid".Localized() : "Common.Text.HaveHad".Localized() + urtaKids + " children together",

                        Player.GenderText("father.", "mother.", "father? Or mother? Other parent, anyway.", "fathe... um... how did that work exactly?")
                    ));

                CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaAffection, 1);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) == 1)
                    OutputText("Scenes.Katherine.TalkLoversUrta.UrtaAndKathTalking".Localized(
                        Player.GenderText("guy", "girl", "girl", "lover")
                    ));
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) <= 10)
                        OutputText("Scenes.Katherine.TalkLoversUrta.StillUncertainSleeping".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) <= 31)
                        {
                            OutputText("Scenes.Katherine.TalkLoversUrta.SighsBigGrin".Localized());

                            OutputText("Scenes.Katherine.TalkLoversUrta.StretchesShowsRack".Localized());

                            OutputText("Scenes.Katherine.TalkLoversUrta.CouldRestrictSex".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.DontFuck".Localized(), TalkLoversUrtaDont);

                            AddButton(1, "Common.Button.Whenever".Localized(), TalkLoversUrtaWhenever);

                            AddButton(2, "Common.Button.Encourage".Localized(), TalkLoversUrtaEncourage);

                            return ;
                        }
                        else
                            OutputText("Scenes.Katherine.TalkLoversUrta.KathHugsYou".Localized(
                                PlayerText()
                            ));

            TalkToKatherine(TalkLoversUrta);
        }

        public void TalkLoversUrtaDont()
        {

            OutputText("Scenes.Katherine.TalkLoversUrtaDont.ScratchBehindEars".Localized());

            OutputText("Scenes.Katherine.TalkLoversUrtaDont.PromisesToBeGood".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaDate, KDATE_LITTLE);

            TalkToKatherine(TalkLoversUrtaDont);
        }

        public void TalkLoversUrtaWhenever()
        {

            OutputText("Scenes.Katherine.TalkLoversUrtaWhenever.StrokeTailPermission".Localized());

            OutputText("Scenes.Katherine.TalkLoversUrtaWhenever.GigglesGlad".Localized(
                PlayerText()
            ));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaDate, KDATE_WHENEVER);

            TalkToKatherine(TalkLoversUrtaWhenever);
        }

        public void TalkLoversUrtaEncourage()
        {

            OutputText("Scenes.Katherine.TalkLoversUrtaEncourage.RubsInnerThighs".Localized());

            OutputText("Scenes.Katherine.TalkLoversUrtaEncourage.WantsMoreTime".Localized());

            OutputText("Scenes.Katherine.TalkLoversUrtaEncourage.FingersMoveCloser".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.TalkLoversUrtaEncourage.CockAndFragment".Localized(
                    CockMultiple("", "s")
                ));

            OutputText("Scenes.Katherine.TalkLoversUrtaEncourage.PussyYesAgree".Localized());

            OutputText("Scenes.Katherine.TalkLoversUrtaEncourage.MoansTailBats".Localized());

            OutputText("Scenes.Katherine.TalkLoversUrtaEncourage.ForgetMasturbate".Localized());

            OutputText("Scenes.Katherine.TalkLoversUrtaEncourage.RepliesDreamily".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaDate, KDATE_LOTS);

            TalkToKatherine(TalkLoversUrtaEncourage);
        }

        public void TalkLoversVala()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineValaAffection) == 0)
            {
                OutputText("Scenes.Katherine.TalkLoversVala.StorySavingVala".Localized());

                OutputText("Scenes.Katherine.TalkLoversVala.KathName".Localized());

                if (IsAt(KLOC_KATHS_APT))
                    OutputText("Scenes.Katherine.TalkLoversVala.LiesBackStaring".Localized());
                else
                    OutputText("Scenes.Katherine.TalkLoversVala.SipsLooksForVala".Localized());

                OutputText("Scenes.Katherine.TalkLoversVala.SentencePeriod".Localized());

                OutputText("Scenes.Katherine.TalkLoversVala.AfterSilenceSpeaks".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedValaInDungeon) > 0)
                    OutputText("Scenes.Katherine.TalkLoversVala.BestNotToMention".Localized());

                if (IsAt(KLOC_KATHS_APT))
                    OutputText("Scenes.Katherine.TalkLoversVala.HeadInYourLap".Localized());
                else
                    OutputText("Scenes.Katherine.TalkLoversVala.SipsSqueezesHand".Localized());

                OutputText("Scenes.Katherine.TalkLoversVala.KeepEyeOnHer".Localized(
                    PlayerText()
                ));

                CoC.Instance.Flags.Set(GameFlagIds.KatherineValaAffection, 1);
            }
            else
            {
                OutputText("Scenes.Katherine.TalkLoversVala.WhispersWatchingVala".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineValaAffection) > 10)
                {
                    OutputText("Scenes.Katherine.TalkLoversVala.CouldRestrictSex".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.DontFuck".Localized(), TalkLoversValaDont);

                    AddButton(1, "Common.Button.Whenever".Localized(), TalkLoversValaWhenever);

                    AddButton(2, "Common.Button.Encourage".Localized(), TalkLoversValaEncourage);

                    return ;
                }
            }

            TalkToKatherine(TalkLoversVala);
        }

        public void TalkLoversValaDont()
        {

            OutputText("Scenes.Katherine.TalkLoversValaDont.KissAndTellHappy".Localized());

            OutputText("Scenes.Katherine.TalkLoversValaDont.PromisesToBeGood".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineValaDate, KDATE_LITTLE);

            TalkToKatherine(TalkLoversValaDont);
        }

        public void TalkLoversValaWhenever()
        {

            OutputText("Scenes.Katherine.TalkLoversValaWhenever.StrokeTailBlowOff".Localized());

            OutputText("Scenes.Katherine.TalkLoversValaWhenever.GigglesGladFeeling".Localized(
                PlayerText()
            ));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineValaDate, KDATE_WHENEVER);

            TalkToKatherine(TalkLoversValaWhenever);
        }

        public void TalkLoversValaEncourage()
        {

            OutputText("Scenes.Katherine.TalkLoversValaEncourage.PlaysWithTail".Localized());

            OutputText("Scenes.Katherine.TalkLoversValaEncourage.WantsMoreTime".Localized());

            OutputText("Scenes.Katherine.TalkLoversValaEncourage.BothNeedEachOther".Localized());

            OutputText("Scenes.Katherine.TalkLoversValaEncourage.MoansTailTwitches".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineValaDate, KDATE_LOTS);

            TalkToKatherine(TalkLoversValaEncourage);
        }

        public void TalkWetBitch()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.TalkWetBitch.WhyHangOutHere".Localized());

            OutputText("Scenes.Katherine.TalkWetBitch.SmilesAboutFries".Localized());

            OutputText("Scenes.Katherine.TalkWetBitch.WhySheJoinedWatch".Localized());

            TalkToKatherine(TalkWetBitch);
        }

        public void TalkCatGirl()
        {
            ClearOutput();

            if (furry)
            {
                OutputText("Scenes.Katherine.TalkCatGirl.FeelsCatMorph".Localized());

                OutputText("Scenes.Katherine.TalkCatGirl.OpenedMyEyes".Localized());

                OutputText("Scenes.Katherine.TalkCatGirl.UpForMoreFun".Localized());

                OutputText("Scenes.Katherine.TalkCatGirl.NotLizardMorph".Localized());

                OutputText("Scenes.Katherine.TalkCatGirl.FewerLookingAdvantage".Localized());

                OutputText("Scenes.Katherine.TalkCatGirl.LikesCheckedOut".Localized());
            }
            else
            {
                OutputText("Scenes.Katherine.TalkCatGirl.FindingThingsCat".Localized());

                OutputText("Scenes.Katherine.TalkCatGirl.RubsHerCheeks".Localized());

                OutputText("Scenes.Katherine.TalkCatGirl.TakesMomentBefore".Localized());

                OutputText("Scenes.Katherine.TalkCatGirl.GuiltyLikesAttention".Localized());
            }

            KatherineMenu();
        }

        public void TalkMaster()
        {
            ClearOutput();

            if ((CoC.Instance.Flags.Get(GameFlagIds.KatherineSubFlags) & KBIT_SUB_CALL_MASTER) > 0)
            {
                OutputText("Scenes.Katherine.TalkMaster.StopCallingMaster".Localized());

                OutputText("Scenes.Katherine.TalkMaster.ConfusedWrongMessage".Localized());

                OutputText("Scenes.Katherine.TalkMaster.AgreesNoMaster".Localized());

                OutputText("Scenes.Katherine.TalkMaster.SuckTongueRubEars".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineSubFlags, CoC.Instance.Flags.Get(GameFlagIds.KatherineSubFlags) & ~KBIT_SUB_CALL_MASTER);
            }
            else
            {
                OutputText("Scenes.Katherine.TalkMaster.PullsIntoKiss".Localized());

                OutputText("Scenes.Katherine.TalkMaster.AsksWhatSheWants".Localized());

                OutputText("Scenes.Katherine.TalkMaster.EyesDilateHugs".Localized());

                OutputText("Scenes.Katherine.TalkMaster.ScratchesEarPurrs".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineSubFlags, CoC.Instance.Flags.Get(GameFlagIds.KatherineSubFlags) | KBIT_SUB_CALL_MASTER);
            }

            KatherineMenu();
        }

        public void TalkMilk()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.TalkMilk.EnjoysMakingMilk".Localized());

            if (breasts.Lactation <= 3)
                OutputText("Scenes.Katherine.TalkMilk.EnjoysUnburdening".Localized());
            else
                if (breasts.Lactation <= 5)
                    OutputText("Scenes.Katherine.TalkMilk.FillsUpFast".Localized());
                else
                    OutputText("Scenes.Katherine.TalkMilk.NotAsNatureIntended".Localized());

            TalkMilkMenu();
        }

        public void TalkMilkMenu()
        {

            OutputText("Scenes.Katherine.TalkMilkMenu.BreastsProduce".Localized());

            switch (breasts.Lactation)
            {
                case 2:
                {
                OutputText("Scenes.Katherine.TalkMilkMenu.SmallAmountCream".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Katherine.TalkMilkMenu.LikeBreastfeedingMorph".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Katherine.TalkMilkMenu.LargeAmountCream".Localized());
                }

                break;

                case 5:
                {
                OutputText("Scenes.Katherine.TalkMilkMenu.VastAmountsMagicalAid".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Katherine.TalkMilkMenu.UnnaturalAmountLactaid".Localized());

            if (breasts.MilkFullness >= 100)
                OutputText("Scenes.Katherine.TalkMilkMenu.FullToBrim".Localized());
            else
                if (breasts.MilkIsFull())
                    OutputText("Scenes.Katherine.TalkMilkMenu.QuiteFullMoment".Localized());

            var canShareWithUrta = CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) > 10;

            var canShareWithVala = CoC.Instance.Flags.Get(GameFlagIds.KatherineValaAffection) > 10;

            var sharingWithFriends = MilkOption(KBIT_MILK_SHARE_WITH_FRIENDS);

            if (sharingWithFriends)
                OutputText("Scenes.Katherine.TalkMilkMenu.SharesMilkFreely".Localized());
            else
            {
                var sharingArray = new List<string> { "you" };

                if (MilkOption(KBIT_MILK_SHARE_WITH_OLD_GANG))
                    sharingArray.Add("her old gang");
                else
                    if (MilkOption(KBIT_MILK_SHARE_WITH_HELENA))
                        sharingArray.Add("Helena");

                if (canShareWithUrta && MilkOption(KBIT_MILK_SHARE_WITH_URTA))
                    sharingArray.Add("Urta");

                if (canShareWithVala && MilkOption(KBIT_MILK_SHARE_WITH_VALA))
                    sharingArray.Add("Vala");

                OutputText("Scenes.Katherine.TalkMilkMenu.SharesOnlyWithYou".Localized(
                    string.Join(", ", sharingArray)
                ));
            }

            ClearButtons();

            if (!sharingWithFriends && !MilkOption(KBIT_MILK_SHARE_WITH_OLD_GANG))
                AddButton(0, "Common.Button.Helena".Localized(), TalkMilkShareWithHelena);

            if (!sharingWithFriends && canShareWithUrta)
                AddButton(1, "Common.Button.Urta".Localized(), TalkMilkShareWithUrta);

            if (!sharingWithFriends && canShareWithVala)
                AddButton(2, "Npcs.Vala.Name".Localized(), TalkMilkShareWithVala);

            AddButton(4, "Common.Button.Limit".Localized(), TalkMilkLimitation);

            if (!sharingWithFriends)
                AddButton(5, "Common.Button.OldGang".Localized(), TalkMilkShareWithOldGang);

            if (Submissiveness() >= 2)
                AddButton(6, "Common.Button.Friends".Localized(), TalkMilkShareWithFriends);

            AddButton(14, "Common.Button.Back".Localized(), () => TalkToKatherine());
        }

        public void TalkMilkShareWithHelena()
        {
            ClearOutput();

            if (MilkOption(KBIT_MILK_SHARE_WITH_HELENA))
            {
                OutputText("Scenes.Katherine.TalkMilkShareWithHelena.StopSharingHelena".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithHelena.FeelsNiceHelping".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), () => TalkMilkShareWithHelenaAnswer(true));

                AddButton(1, "Common.Button.DontStop".Localized(), () => TalkMilkShareWithHelenaAnswer(false));
            }
            else
            {
                OutputText("Scenes.Katherine.TalkMilkShareWithHelena.SomeoneNeedsMeal".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithHelena.CanTrackHerDown".Localized());

                MilkOptionSet(KBIT_MILK_SHARE_WITH_HELENA);

                TalkMilkMenu();
            }
        }

        public void TalkMilkShareWithHelenaAnswer(bool reallyStop)
        {

            if (reallyStop)
            {
                OutputText("Scenes.Katherine.TalkMilkShareWithHelenaAnswer.LooksRelieved".Localized(
                    PlayerMaster() ? " you [master]" : "s " + Player.ShortName
                ));

                MilkOptionClear(KBIT_MILK_SHARE_WITH_HELENA);
            }
            else
                OutputText("Scenes.Katherine.TalkMilkShareWithHelenaAnswer.NodsWillStop".Localized());

            TalkMilkMenu();
        }

        public void TalkMilkShareWithUrta()
        {
            ClearOutput();

            if (MilkOption(KBIT_MILK_SHARE_WITH_URTA))
            {
                OutputText("Scenes.Katherine.TalkMilkShareWithUrta.StopSharingUrta".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithUrta.WillNeedHelp".Localized());

                MilkOptionClear(KBIT_MILK_SHARE_WITH_URTA);
            }
            else
            {
                OutputText("Scenes.Katherine.TalkMilkShareWithUrta.MoreDrainingHelp".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithUrta.SoundsLikePlan".Localized());

                MilkOptionSet(KBIT_MILK_SHARE_WITH_URTA);
            }

            TalkMilkMenu();
        }

        public void TalkMilkShareWithVala()
        {
            ClearOutput();

            if (MilkOption(KBIT_MILK_SHARE_WITH_VALA))
            {
                OutputText("Scenes.Katherine.TalkMilkShareWithVala.StopSharingVala".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithVala.WillNeedHelp".Localized());

                MilkOptionClear(KBIT_MILK_SHARE_WITH_VALA);
            }
            else
            {
                OutputText("Scenes.Katherine.TalkMilkShareWithVala.MoreDrainingHelp".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithVala.CreamForFaeries".Localized());

                MilkOptionSet(KBIT_MILK_SHARE_WITH_VALA);
            }

            TalkMilkMenu();
        }

        public void TalkMilkShareWithOldGang()
        {
            ClearOutput();

            if (MilkOption(KBIT_MILK_SHARE_WITH_OLD_GANG))
            {
                OutputText("Scenes.Katherine.TalkMilkShareWithOldGang.StopFeedingGang".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithOldGang.SighGladActually".Localized());

                MilkOptionClear(KBIT_MILK_SHARE_WITH_OLD_GANG);

                TalkMilkMenu();
            }
            else
            {
                OutputText("Scenes.Katherine.TalkMilkShareWithOldGang.SpreadTheWealth".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithOldGang.FeedVagrantCats".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithOldGang.TrainedWatchOfficer".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithOldGang.CrimePrevention".Localized());

                MilkOptionSet(KBIT_MILK_SHARE_WITH_OLD_GANG);

                TalkMilkLimitation();
            }
        }

        public void TalkMilkShareWithFriends()
        {
            ClearOutput();

            if (MilkOption(KBIT_MILK_SHARE_WITH_FRIENDS))
            {
                OutputText("Scenes.Katherine.TalkMilkShareWithFriends.StopSharingFriends".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithFriends.EnjoyedMoreThanThought".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithFriends.HintOfEagerness".Localized());

                MilkOptionClear(KBIT_MILK_SHARE_WITH_FRIENDS);

                TalkMilkMenu();
            }
            else
            {
                OutputText("Scenes.Katherine.TalkMilkShareWithFriends.GiftFromGoddess".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithFriends.HandOnChestAsks".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithFriends.LaughShellEnjoyIt".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithFriends.ConflictedThenYields".Localized(
                    PlayerText()
                ));

                OutputText("Scenes.Katherine.TalkMilkShareWithFriends.GetsHangOfIt".Localized());

                OutputText("Scenes.Katherine.TalkMilkShareWithFriends.StaysSilentThinking".Localized());

                MilkOptionSet(KBIT_MILK_SHARE_WITH_FRIENDS);

                TalkMilkLimitation();
            }
        }

        public void TalkMilkLimitation()
        {

            var doneHighLactation = DoneSubmissive(KBIT_SUB_HIGH_LACTATION);

            OutputText("Scenes.Katherine.TalkMilkLimitation.ToldToLimit".Localized());

            switch (breasts.PreventLactationIncrease)
            {
                case 2:
                {
                OutputText("Scenes.Katherine.TalkMilkLimitation.BreastsLightLimit".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Katherine.TalkMilkLimitation.BreastsStrongLimit".Localized());
                }

                break;

                case 5:
                {
                OutputText("Scenes.Katherine.TalkMilkLimitation.BreastsHeavyLimit".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Katherine.TalkMilkLimitation.BreastsModerateLimit".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Katherine.TalkMilkLimitation.NoLimitOnHer".Localized());

            OutputText("Scenes.Katherine.TalkMilkLimitation.LactationBoldEnd".Localized());

            OutputText("Scenes.Katherine.TalkMilkLimitation.FutureLimitQuestion".Localized());

            ClearButtons();

            if (breasts.PreventLactationIncrease != 2)
                AddButton(0, "Common.Button.Light".Localized(), () => TalkMilkLimitationChange(2));

            if (doneHighLactation && breasts.PreventLactationIncrease != 3)
                AddButton(1, "Common.Button.Moderate".Localized(), () => TalkMilkLimitationChange(3));

            if (doneHighLactation && breasts.PreventLactationIncrease != 4)
                AddButton(2, "Common.Button.Strong".Localized(), () => TalkMilkLimitationChange(4));

            if (doneHighLactation && breasts.PreventLactationIncrease != 5)
                AddButton(3, "Scenes.Akbal.HealthLoss.WordHeavy".Localized(), () => TalkMilkLimitationChange(5));

            if (breasts.PreventLactationIncrease != 0)
                AddButton(4, "Common.Button.Unlimited".Localized(), () => TalkMilkLimitationChange(0));

            AddButton(9, "Common.Button.NoChange".Localized(), TalkMilkMenu);
        }

        public void TalkMilkLimitationChange(int newLimit)
        {
            ClearOutput();

            if (newLimit == 0 && !DoneSubmissive(KBIT_SUB_HIGH_LACTATION))
                breasts.PreventLactationIncrease = 3;
            else
                breasts.PreventLactationIncrease = newLimit;

            switch (newLimit)
            {
                case 2:
                {
                OutputText("Scenes.Katherine.TalkMilkLimitationChange.LimitToLittle".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Katherine.TalkMilkLimitationChange.LimitToModerate".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Katherine.TalkMilkLimitationChange.LimitToLarge".Localized());
                }

                break;

                case 5:
                {
                OutputText("Scenes.Katherine.TalkMilkLimitationChange.LimitToHefty".Localized());
                }

                break;
            }

            if (newLimit == 0)
            {
                OutputText("Scenes.Katherine.TalkMilkLimitationChange.NoLimitAtAll".Localized());

                OutputText("Scenes.Katherine.TalkMilkLimitationChange.ShakesHeadWillLet".Localized());

                TalkMilkMenu();

                return ;
            }

            OutputText("Scenes.Katherine.TalkMilkLimitationChange.SmilesWillDoThat".Localized(
                PlayerMaster() ? "Common.Text.YesMaster".Localized() : "Common.Text.Alright".Localized() + Player.ShortName
            ));

            if (breasts.Lactation > breasts.PreventLactationIncrease)
                OutputText("Scenes.Katherine.TalkMilkLimitationChange.MightTakeWhile".Localized());

            OutputText("Scenes.Katherine.TalkMilkLimitationChange.SayEnd".Localized());

            TalkMilkMenu();
        }

        public void KatherineDate()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.KatherineDate.LeadKatherineSomewhere".Localized());

            ClearButtons();

            if (IsAt(KLOC_KATHS_APT))
                AddButton(0, "Common.Button.TheBar".Localized(), DateGotoBar);
            else
                AddButton(0, "Common.Button.KathsPlace".Localized(), DateGotoKaths);

            AddButton(1, "Common.Button.Bath".Localized(), DateKathBath);

            AddButton(14, "Common.Button.Back".Localized(), KatherineMenu);
        }

        public void DateGotoKaths()
        {

            if (IsAt(KLOC_URTAS_APT))
            {
                OutputText("Scenes.Katherine.DateGotoKaths.OfferWalkHerHome".Localized(
                    CatGirl("girl", "morph")
                ));

                OutputText("Scenes.Katherine.DateGotoKaths.UrtaSaysTakeCare".Localized());
            }
            else
                if (IsAt(KLOC_URTAS_HOME))
                {
                    OutputText("Scenes.Katherine.DateGotoKaths.WaitWhileCleansUp".Localized());

                    OutputText("Scenes.Katherine.DateGotoKaths.TracksDownClothing".Localized());
                }
                else
                {
                    OutputText("Scenes.Katherine.DateGotoKaths.AskBackToPlace".Localized());

                    OutputText("Scenes.Katherine.DateGotoKaths.DownsLastDrink".Localized());
                }

            TakeKathHome();
        }

        public void TakeKathHome()
        {

            OutputText("Scenes.Katherine.TakeKathHome.RaceThroughStreets".Localized());

            OutputText("Scenes.Katherine.TakeKathHome.AttacksLockWithKey".Localized());

            Clothes();

            OutputText("Scenes.Katherine.TakeKathHome.StandsByBed".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KLOC_KATHS_APT);

            KatherineMenu();
        }

        public void DateGotoBar()
        {

            if (IsAt(KLOC_URTAS_APT))
            {
                OutputText("Scenes.Katherine.DateGotoBar.OfferTakeToBar".Localized(
                    CatGirl("girl", "morph")
                ));

                OutputText("Scenes.Katherine.DateGotoBar.UrtaSaysTakeCare".Localized());
            }
            else
                if (IsAt(KLOC_URTAS_HOME))
                {
                    OutputText("Scenes.Katherine.DateGotoBar.WaitWhileCleansUp".Localized());

                    OutputText("Scenes.Katherine.DateGotoBar.TracksDownClothing".Localized());
                }
                else
                {
                    OutputText("Scenes.Katherine.DateGotoBar.AskBackToBar".Localized());

                    OutputText("Scenes.Katherine.DateGotoBar.FunThingsRightHere".Localized());
                }

            TakeKathToTheBar();
        }

        public void TakeKathToTheBar()
        {

            OutputText("Scenes.Katherine.TakeKathToTheBar.WalkThroughStreets".Localized());

            OutputText("Scenes.Katherine.TakeKathToTheBar.EntersWetBitch".Localized());

            OutputText("Scenes.Katherine.TakeKathToTheBar.SeatAtBooth".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KLOC_BAR);

            KatherineMenu();
        }

        public void DateKathBath()
        {

            OutputText("Scenes.Katherine.DateKathBath.TripToLake".Localized());

            OutputText("Scenes.Katherine.DateKathBath.NothingToWear".Localized());

            if (IsAt(KLOC_STREETS))
                OutputText("Scenes.Katherine.DateKathBath.HopsBreastsBounce".Localized());
            else
                if (IsAt(KLOC_URTAS_APT) || IsAt(KLOC_URTAS_HOME))
                    OutputText("Scenes.Katherine.DateKathBath.GoesOutsideWithYou".Localized());
                else
                    OutputText((IsAt(KLOC_KATHS_APT) ? "Common.Text.SheJumpsUpOffTheBed".Localized() : "Common.Text.SheSucksUpTheLastOf".Localized()) + " in a way that causes her breasts to bounce. She");

            OutputText("Scenes.Katherine.DateKathBath.GetsUpClose".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KLOC_LAKE);

            BathTime();
        }

        public void KatherineDrinkUp()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.KatherineDrinkUp.AsksWhatDrinking".Localized());

            OutputText("Scenes.Katherine.KatherineDrinkUp.ShowsTumbler".Localized());

            OutputText("Scenes.Katherine.KatherineDrinkUp.StrongEnoughDrunk".Localized());

            OutputText("Scenes.Katherine.KatherineDrinkUp.DrunkByAccident".Localized());

            OutputText("Scenes.Katherine.KatherineDrinkUp.GrinsSheCanIndulge".Localized());

            OutputText("Scenes.Katherine.KatherineDrinkUp.SmileWavesWaitress".Localized());

            OutputText("Scenes.Katherine.KatherineDrinkUp.GirlfriendWasted".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KLOC_BAR_DRUNK);

            KatSexMenu();
        }

        public void KatherinesAppearance(bool clear = true)
        {
            ClearOutput();

            ShowImage("katherine-examine-her-appearance");

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) < 4)
                OutputText("Scenes.Katherine.KatherinesAppearance.StandsNervouslyWatching".Localized());
            else
                if (View.Time.Hours >= 10)
                    OutputText("Scenes.Katherine.KatherinesAppearance.StandsBeamingPride".Localized());

            OutputText("Scenes.Katherine.KatherinesAppearance.LeanBuiltCat".Localized());

            if (furry && !noFur)
            {
                OutputText("Scenes.Katherine.KatherinesAppearance.MorphHeightLine".Localized());

                if (hairColor == "jet black")
                    OutputText("Scenes.Katherine.KatherinesAppearance.BothFurAnd".Localized());
                else
                    OutputText("Scenes.Katherine.KatherinesAppearance.FurBlackBut".Localized());
            }
            else
                OutputText("Scenes.Katherine.KatherinesAppearance.GirlHeightLine".Localized());

            OutputText("Scenes.Katherine.KatherinesAppearance.ShoulderLengthHair".Localized());

            if ((!furry || noFur) && hairColor == "jet black")
                OutputText("Scenes.Katherine.KatherinesAppearance.BlackAsFur".Localized());
            else
                OutputText("Scenes.Katherine.KatherinesAppearance.PlaceholderPeriodBreak".Localized(
                    hairColor
                ));

            Clothes();

            switch (CoC.Instance.Flags.Get(GameFlagIds.KatherineClothesWorn))
            {
                case KBIT_CLOTHES_UNIFORM:
                {
                OutputText("Scenes.Katherine.KatherinesAppearance.WantsCloserLook".Localized());
                }

                break;

                case KBIT_CLOTHES_C_CLOTH:
                {
                OutputText("Scenes.Katherine.KatherinesAppearance.StripsBeforeAsking".Localized());
                }

                break;

                case KBIT_CLOTHES_BODYSUIT:
                {
                OutputText("Scenes.Katherine.KatherinesAppearance.WigglesOutBodysuit".Localized());
                }

                break;

                case KBIT_CLOTHES_B_DRESS:
                {
                OutputText("Scenes.Katherine.KatherinesAppearance.TeasesUndoingButtons".Localized());
                }

                break;

                case KBIT_CLOTHES_SS_ROBE:
                {
                OutputText("Scenes.Katherine.KatherinesAppearance.StepsOutOfRobes".Localized());
                }

                break;

                case KBIT_CLOTHES_TUBETOP:
                {
                OutputText("Scenes.Katherine.KatherinesAppearance.PeelsBeforeAsking".Localized());
                }

                break;

                case KBIT_CLOTHES_NURSECL:
                {
                OutputText("Scenes.Katherine.KatherinesAppearance.UnbuttonsBeforeAsking".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Katherine.KatherinesAppearance.MeeklyUndresses".Localized());

            OutputText("Scenes.Katherine.KatherinesAppearance.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Katherine.KatherinesAppearance.BreastsOnChest".Localized(
                breasts.Adj(),

                breasts.Cup()
            ));

            if (HasCock())
            {
                OutputText("Scenes.Katherine.KatherinesAppearance.PenisBelowBelly".Localized(
                    CockMultiple("phallus begins ", "phalli begin "),

                    CockMultiple("", "pair of "),

                    CockType(),

                    CockMultiple(" ", "s "),

                    CockWidth(),

                    cockLength,

                    CockMultiple("s itself, with a ", " themselves, with "),

                    knotSize,

                    CockMultiple(" at its base. ", "s at their bases. ")
                ));

                if (!HasBalls())
                    OutputText("Scenes.Katherine.KatherinesAppearance.JustBeneathKath".Localized(
                        CockMultiple("cock is", "twin cocks is")
                    ));
                else
                    OutputText("Scenes.Katherine.KatherinesAppearance.BallsSwayBelow".Localized(
                        ballSize,

                        CockMultiple("", "s")
                    ));
            }
            else
                OutputText("Scenes.Katherine.KatherinesAppearance.BetweenLegsRests".Localized());

            OutputText("Scenes.Katherine.KatherinesAppearance.WetEagerCunt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) < 4)
            {
                OutputText("Scenes.Katherine.KatherinesAppearance.RedressesAfterLooking".Localized());

                KatherineMenu();
            }
            else
                if (View.Time.Hours >= 10)
                {
                    OutputText("Scenes.Katherine.KatherinesAppearance.NakedTailWaving".Localized());

                    OutputText("Scenes.Katherine.KatherinesAppearance.ClothesLookGood".Localized(
                        CatGirl("stroking her milky white skin", "smoothing her shiny fur coat")
                    ));

                    OutputText("Scenes.Katherine.KatherinesAppearance.CollectsHerClothes".Localized());

                    KatherineMenu();
                }
        }

        public void GiveKatherineAnItem(int page = 0)
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatherineAnItem.HasPresentForHer".Localized());

            OutputText("Scenes.Katherine.GiveKatherineAnItem.FaceLightsGuilt".Localized());

            var hasJob = CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) >= 4;

            ClearButtons();

            AddButton(14, "Common.Button.Back".Localized(), () => GiveKatherineAnItem());

            if (page == 0)
            {
                AddNextButton("Common.Button.Tfs".Localized(), () => GiveKatherineAnItem(1));

                if (hasJob)
                    AddNextButton("Common.Button.Clothes".Localized(), () => GiveKatherineAnItem(2));
                else
                    AddNextButtonDisabled("Clothes", "She has no place to store her Clothes.", "");

                if (hasJob)
                    AddNextButton("Common.Button.Dyes".Localized(), () => GiveKatherineAnItem(3));
                else
                    AddNextButtonDisabled("Dyes", "She likes her current hair color.", "");

                AddButton(14, "Common.Button.Back".Localized(), KatherineMenu);
            }

            if (page == 1)
            {
                var blackEgg = Player.HasItem(ItemRegistry.Create(ConsumableIds.EggBlack)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.EggLargeBlack));

                var honey = Player.HasItem(ItemRegistry.Create(ConsumableIds.BeeHoney)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney));

                var pinkEgg = Player.HasItem(ItemRegistry.Create(ConsumableIds.EggPink)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.EggLargePink));

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.CaninePepper)))
                    AddNextButton(ItemRegistry.Create(ConsumableIds.CaninePepper).Name, GiveKatACaninePepper);
                else
                    AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.CaninePepper).Name, "Offer her " + ItemRegistry.Create(ConsumableIds.CaninePepper).LongName + ".", ItemRegistry.Create(ConsumableIds.CaninePepper).TooltipHeader);

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LargeCaninePepper)))
                    AddNextButton(ItemRegistry.Create(ConsumableIds.LargeCaninePepper).Name, GiveKatAOverlyLargePepper);
                else
                    AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.LargeCaninePepper).Name, "Offer her " + ItemRegistry.Create(ConsumableIds.LargeCaninePepper).LongName + ".", ItemRegistry.Create(ConsumableIds.LargeCaninePepper).TooltipHeader);

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.DoubleCaninePepper)))
                    AddNextButton(ItemRegistry.Create(ConsumableIds.DoubleCaninePepper).Name, GiveKatADoublePepper);
                else
                    AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.DoubleCaninePepper).Name, "Offer her " + ItemRegistry.Create(ConsumableIds.DoubleCaninePepper).LongName + ".", ItemRegistry.Create(ConsumableIds.DoubleCaninePepper).TooltipHeader);

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.BulbyCaninePepper)))
                    AddNextButton(ItemRegistry.Create(ConsumableIds.BulbyCaninePepper).Name, GiveKatABulbousPepper);
                else
                    AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.BulbyCaninePepper).Name, "Offer her " + ItemRegistry.Create(ConsumableIds.BulbyCaninePepper).LongName + ".", ItemRegistry.Create(ConsumableIds.BulbyCaninePepper).TooltipHeader);

                if (hasJob && Player.HasItem(ItemRegistry.Create(ConsumableIds.KnottyCaninePepper)))
                    AddNextButton(ItemRegistry.Create(ConsumableIds.KnottyCaninePepper).Name, GiveKatAKnottyPepper);
                else
                    AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.KnottyCaninePepper).Name, "Offer her " + ItemRegistry.Create(ConsumableIds.KnottyCaninePepper).LongName + ". She won't accept such a thing if she is not sure about her future.", ItemRegistry.Create(ConsumableIds.KnottyCaninePepper).TooltipHeader);

                if (hasJob && Player.HasItem(ConsumableIds.W_FRUIT))
                    AddNextButton(ItemRegistry.Create(ConsumableIds.W_FRUIT).Name, GiveKatWhiskerFruit);
                else
                    AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.W_FRUIT).Name, "Offer her " + ItemRegistry.Create(ConsumableIds.W_FRUIT).LongName + ". She won't accept such a thing if she is not sure about her future.", ItemRegistry.Create(ConsumableIds.W_FRUIT).TooltipHeader);

                if (Player.HasItem(ItemIds.Reducto))
                    AddNextButton(ItemRegistry.Create(ItemIds.Reducto).Name, UseReductoOnKat);
                else
                    AddNextButtonDisabled(ItemRegistry.Create(ItemIds.Reducto).Name, "Offer her " + ItemRegistry.Create(ItemIds.Reducto).LongName + ".", ItemRegistry.Create(ItemIds.Reducto).TooltipHeader);

                if (hasJob && Player.HasItem(ItemRegistry.Create(ConsumableIds.Lactaid)))
                    AddNextButton(ItemRegistry.Create(ConsumableIds.Lactaid).Name, GiveKatTheGiftOFMilk);
                else
                    AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.Lactaid).Name, "Offer her " + ItemRegistry.Create(ConsumableIds.Lactaid).LongName + ". She won't accept such a thing if she is not sure about her future.", ItemRegistry.Create(ConsumableIds.Lactaid).TooltipHeader);

                if (hasJob && HasCock() && pinkEgg)
                    AddNextButton("Items.PinkEgg.Short".Localized(), UsePinkEggOnKath);
                else
                    AddNextButtonDisabled("Pink Egg", "Offer her a pink egg to remove her cock. She won't accept such a thing if she is not sure about her future.", "Pink Egg");

                if (hasJob && Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft)))
                    AddNextButton(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft).Name, UseIncubiDraftOnKath);
                else
                    AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft).Name, "Offer her " + ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft).LongName + (HasCock() ? "Common.Text.ToMakeHerCockBigger".Localized() : "Common.Text.ToRegrowHerCock".Localized()) + ". She won't accept such a thing if she is not sure about her future.", ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft).TooltipHeader);

                if (hasJob && Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk)))
                    AddNextButton(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk).Name, GiveKatPureSuccubusMilk);
                else
                    AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk).Name, "Offer her " + ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk).LongName + " to make her breasts bigger. She won't accept such a thing if she is not sure about her future.", ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk).TooltipHeader);

                if (hasJob && HasCock() && Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubusDelight)))
                    AddNextButton(ItemRegistry.Create(ConsumableIds.PurifiedSuccubusDelight).Name, GiveKatPureSuccubusDelight);
                else
                    AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.PurifiedSuccubusDelight).Name, "Offer her " + ItemRegistry.Create(ConsumableIds.PurifiedSuccubusDelight).LongName + " to make her balls bigger. She won't accept such a thing if she is not sure about her future.", ItemRegistry.Create(ConsumableIds.PurifiedSuccubusDelight).TooltipHeader);

                if (hasJob && HasCock() && ballSize == 1 && pinkEgg && Player.HasItem(ItemIds.Reducto))
                    AddNextButton("Common.Button.RemBalls".Localized(), UseReductoAndPinkEgg);
                else
                    AddNextButtonDisabled("Rem Balls", "You've met herms that have no balls. You're pretty sure that together the pink egg and the Reducto salve can make her like that. She won't accept such a thing if she is not sure about her future.", "Remove Balls");

                if (furry)
                {
                    if (!noFur && hasJob && blackEgg && Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk)) && Player.HasItem(ItemRegistry.Create(ConsumableIds.GoldenSeed)))
                        AddNextButton("Common.Button.Defur".Localized(), GiveKatDefurFormula);
                    else
                        AddNextButtonDisabled("Defur", "You can offer her to become a cat-girl. It takes a black egg, an untainted bottle of Succubi milk and a golden seed. She won't accept such a thing if she is not sure about her future." + (noFur ? " Not applicable in NoFur mode." : ""), "");
                }
                else
                    if (!noFur && hasJob && honey && Player.HasItem(ConsumableIds.W_FRUIT) && Player.HasItem(ConsumableIds.HairSerum))
                        AddNextButton("Common.Button.Refuzz".Localized(), GiveKatRefuzzFormula);
                    else
                        AddNextButtonDisabled("Refuzz", "You can offer her to become a cat-morph again. It takes a bottle of special bee honey, a piece of whisker-fruit and a bottle of hair extension serum." + (noFur ? " Not applicable in NoFur mode." : ""), "");
            }

            if (page == 2)
            {
                if (!HasClothing(KBIT_CLOTHES_BODYSUIT) && Player.HasItem(ItemRegistry.Create(ArmorIds.Bodysuit)))
                    AddNextButton("Items.Bodysuit.Short".Localized(), GiveKatClothesBodysuit);
                else
                    if (HasClothing(KBIT_CLOTHES_BODYSUIT))
                        AddNextButtonDisabled("Bodysuit", "She already owns a bodysuit.", "");
                    else
                        AddNextButtonDisabled("Bodysuit", "You think she can use a bodysuit.", "");

                if (!HasClothing(KBIT_CLOTHES_B_DRESS) && Player.HasItem(ItemRegistry.Create(ArmorIds.BallroomDress)))
                    AddNextButton("Common.Button.LongDress".Localized(), GiveKatClothesLongDress);
                else
                    if (HasClothing(KBIT_CLOTHES_B_DRESS))
                        AddNextButtonDisabled("Long Dress", "She already owns a long dress.", "");
                    else
                        AddNextButtonDisabled("Long Dress", "You think she can use a long dress.", "");

                if (!HasClothing(KBIT_CLOTHES_NURSECL) && Player.HasItem(ItemRegistry.Create(ArmorIds.NurseOutfit)))
                    AddNextButton("Common.Button.Nurseclothes".Localized(), GiveKatClothesNurseOutfit);
                else
                    if (HasClothing(KBIT_CLOTHES_NURSECL))
                        AddNextButtonDisabled("NurseClothes", "She already owns a nurse Clothes.", "");
                    else
                        AddNextButtonDisabled("NurseClothes", "You think she can use a nurse Clothes.", "");

                if (!HasClothing(KBIT_CLOTHES_SS_ROBE) && Player.HasItem(ItemRegistry.Create(ArmorIds.SS_ROBE)))
                    AddNextButton("Common.Button.SilkRobe".Localized(), GiveKatClothesSilkRobe);
                else
                    if (HasClothing(KBIT_CLOTHES_SS_ROBE))
                        AddNextButtonDisabled("Silk Robe", "She already owns a silk robe.", "");
                    else
                        AddNextButtonDisabled("Silk Robe", "You think she can use a silk robe.", "");

                if (!HasClothing(KBIT_CLOTHES_TUBETOP) && Player.HasItem(ItemRegistry.Create(ArmorIds.TubeTop)))
                    AddNextButton("Items.TubeTop.Short".Localized(), GiveKatClothesTubeTop);
                else
                    if (HasClothing(KBIT_CLOTHES_TUBETOP))
                        AddNextButtonDisabled("Tube Top", "She already owns a tube top.", "");
                    else
                        AddNextButtonDisabled("Tube Top", "You think she can use a tube top.", "");
            }

            if (page == 3 || page == 4)
            {

            void DyeButton(string color, Item dye, Action act)
            {
                if (hairColor == color)
                    AddNextButtonDisabled(dye.ShortNameKey.Localized(), "Her hair is already " + color + ".");
                else if (!Player.HasItem(dye))
                    AddNextButtonDisabled(dye.ShortNameKey.Localized(), "You think she would look good with " + color + " hair if you could find a dye.");
                else
                    AddNextButton(dye.ShortNameKey.Localized(), act);
            }

                if (page == 3)
                {
                    DyeButton("rich auburn", ItemRegistry.Create(ConsumableIds.HairDyeAuburn), GiveKatDyeAuburn);

                    DyeButton("jet black", ItemRegistry.Create(ConsumableIds.HairDyeBlack), GiveKatDyeBlack);

                    DyeButton("light blonde", ItemRegistry.Create(ConsumableIds.HairDyeBlond), GiveKatDyeBlonde);

                    DyeButton("bright blue", ItemRegistry.Create(ConsumableIds.HairDyeBlue), GiveKatDyeBlue);

                    DyeButton("chestnut brown", ItemRegistry.Create(ConsumableIds.HairDyeBrown), GiveKatDyeBrown);

                    DyeButton("cloud gray", ItemRegistry.Create(ConsumableIds.HairDyeGray), GiveKatDyeGray);

                    DyeButton("lime green", ItemRegistry.Create(ConsumableIds.HairDyeGreen), GiveKatDyeGreen);

                    DyeButton("vibrant orange", ItemRegistry.Create(ConsumableIds.HairDyeOrange), GiveKatDyeOrange);

                    DyeButton("neon pink", ItemRegistry.Create(ConsumableIds.HairDyePink), GiveKatDyePink);

                    DyeButton("deep purple", ItemRegistry.Create(ConsumableIds.HairDyePurple), GiveKatDyePurple);

                    AddButton(13, "Common.Button.Next".Localized(), () => GiveKatherineAnItem(4));
                }
                else
                {
                    DyeButton("rainbow colored", ItemRegistry.Create(ConsumableIds.HairDyeRainbow), GiveKatDyeRainbow);

                    DyeButton("flaming red", ItemRegistry.Create(ConsumableIds.HairDyeRed), GiveKatDyeRed);

                    DyeButton("russet red", ItemRegistry.Create(ConsumableIds.HairDyeRusset), GiveKatDyeRusset);

                    DyeButton("sunflower yellow", ItemRegistry.Create(ConsumableIds.HairDyeYellow), GiveKatDyeYellow);

                    DyeButton("snow white", ItemRegistry.Create(ConsumableIds.HairDyeWhite), GiveKatDyeWhite);

                    AddButton(13, "Common.Button.Previous".Localized(), () => GiveKatherineAnItem(3));
                }
            }
        }

        public void UseReductoOnKat()
        {
            ClearOutput();

            var dickMin = (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) >= 4 ? 6 : 8);

            if (cockLength <= dickMin && ballSize <= 1 && knotSize <= 2 && breasts.BreastRating == 0)
            {
                OutputText("Scenes.Katherine.UseReductoOnKat.ThinksButRefuses".Localized(
                    PlayerText()
                ));

                if (PlayerMaster())
                    OutputText("Scenes.Katherine.UseReductoOnKat.SighsPutsJarAway".Localized());
                else
                    OutputText("Scenes.Katherine.UseReductoOnKat.WontGetSmaller".Localized());

                OutputText("Scenes.Katherine.UseReductoOnKat.AsksSomethingElse".Localized());

                GiveKatherineAnItem();
            }
            else
            {
                OutputText("Scenes.Katherine.UseReductoOnKat.OffersSmallSalve".Localized(
                    ClothesLowerChoice("yanking down her shorts", "raising her skirt and pulling down her panties", "opening the folds of her bodysuit", "raising her dress and pulling down her panties", "untying her robe and pulling down her panties")
                ));

                if (Player.Libido > 50)
                    OutputText("Scenes.Katherine.UseReductoOnKat.SalaciousGrin".Localized());

                OutputText("Scenes.Katherine.UseReductoOnKat.PlunksDownHappily".Localized());

                if (IsAt(KLOC_STREETS))
                    OutputText("Scenes.Katherine.UseReductoOnKat.NearbyCrate".Localized());
                else
                    OutputText("Scenes.Katherine.UseReductoOnKat.BedStripsClothes".Localized());

                OutputText("Scenes.Katherine.UseReductoOnKat.KneelsLookingAt".Localized());

                if (Player.Libido > 50)
                    OutputText("Scenes.Katherine.UseReductoOnKat.PlanningWhatToDo".Localized());
                else
                    OutputText("Scenes.Katherine.UseReductoOnKat.WonderingHowBegin".Localized());

                ClearButtons();

                if (knotSize > 2)
                    AddButton(0, "Common.Button.Knot".Localized(), UseRedoctoOnKatsKnot);
                else
                    AddButton(0, "Common.Button.Knot".Localized(), null);

                if (cockLength > dickMin)
                    AddButton(1, "CharCreation.MenuBeardSettings.Button2".Localized(), UseReductoOnKatsKock);
                else
                    AddButton(1, "CharCreation.MenuBeardSettings.Button2".Localized(), null);

                if (ballSize > 1)
                    AddButton(2, "Scenes.PlayerAppearance.Label.Balls".Localized(), ReductoBallSize);
                else
                    AddButton(2, "Scenes.PlayerAppearance.Label.Balls".Localized(), null);

                if (breasts.BreastRating > 0)
                    AddButton(3, "Common.Button.Breasts".Localized(), UseRreductoOnKatsBreasts);
                else
                    AddButton(3, "Common.Button.Breasts".Localized(), null);

                AddButton(14, "Common.Button.Back".Localized(), () => GiveKatherineAnItem());
            }
        }

        public void UseRedoctoOnKatsKnot()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.UseRedoctoOnKatsKnot.StrokesHerSheath".Localized());

            if (cockNumber > 1)
                OutputText("Scenes.Katherine.UseRedoctoOnKatsKnot.RubsCockTipsPlural".Localized(
                    CockType(),

                    cockLength,

                    knotSize
                ));
            else
                OutputText("Scenes.Katherine.UseRedoctoOnKatsKnot.RubsCockTipSingular".Localized(
                    CockType(),

                    cockLength,

                    knotSize
                ));

            OutputText("Scenes.Katherine.UseRedoctoOnKatsKnot.TooWoundUp".Localized());

            if (cockNumber > 1)
            {
                OutputText("Scenes.Katherine.UseRedoctoOnKatsKnot.GaspsYowlKnotsShrink".Localized());

                if (Player.Libido > 50)
                    OutputText("Scenes.Katherine.UseRedoctoOnKatsKnot.CatchInMouth".Localized());
                else
                    OutputText("Scenes.Katherine.UseRedoctoOnKatsKnot.NarrowlyDodges".Localized());

                OutputText("Scenes.Katherine.UseRedoctoOnKatsKnot.DropsSpillsSalve".Localized());
            }
            else
            {
                OutputText("Scenes.Katherine.UseRedoctoOnKatsKnot.GaspsYowlKnotShrinks".Localized());

                if (Player.Libido > 50)
                    OutputText("Scenes.Katherine.UseRedoctoOnKatsKnot.CatchInMouthAgain".Localized());
                else
                    OutputText("Scenes.Katherine.UseRedoctoOnKatsKnot.NarrowlyDodgesAgain".Localized());

                OutputText("Scenes.Katherine.UseRedoctoOnKatsKnot.DropsSpillsSalveAgain".Localized());
            }

            OutputText("Scenes.Katherine.UseRedoctoOnKatsKnot.GlowingOrgasmicSmile".Localized());

            knotSize -= 2;

            if (knotSize < 2)
                knotSize = 2;

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            Player.ConsumeItem(ItemIds.Reducto);

            KatSexMenu();
        }

        public void UseReductoOnKatsKock()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.UseReductoOnKatsKock.GestureExposeYourself".Localized());

            if (Player.Libido > 50)
                OutputText("Scenes.Katherine.UseReductoOnKatsKock.ContinueUntilClose".Localized());

            OutputText("Scenes.Katherine.UseReductoOnKatsKock.ClearsThroatFocus".Localized(
                CatGirl("a brilliant red", "red underneath her sable fur"),

                CockMultiple("", "s")
            ));

            OutputText("Scenes.Katherine.UseReductoOnKatsKock.KneelThrobbingMember".Localized(
                CockMultiple("", "s"),

                CockMultiple("", "s"),

                CockMultiple("cock", "twin cocks")
            ));

            if (Player.Libido > 50)
                OutputText("Scenes.Katherine.UseReductoOnKatsKock.QuiversNearerClimax".Localized());

            OutputText("Scenes.Katherine.UseReductoOnKatsKock.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Katherine.UseReductoOnKatsKock.TiltsTowardHerFace".Localized(
                CockType("puppy pecker", "kitty cock"),

                CockMultiple("", "s"),

                CockMultiple("", "s")
            ));

            if (Player.Libido > 50)
                OutputText("Scenes.Katherine.UseReductoOnKatsKock.LewdSmirkStaring".Localized());

            OutputText("Scenes.Katherine.UseReductoOnKatsKock.RubsPasteIntoShaft".Localized(
                CockMultiple("", "s")
            ));

            if (Player.Libido > 50)
                OutputText("Scenes.Katherine.UseReductoOnKatsKock.HuskyGroanProtest".Localized());

            OutputText("Scenes.Katherine.UseReductoOnKatsKock.KatherineTrembles".Localized());

            if (Player.Libido > 50)
                OutputText("Scenes.Katherine.UseReductoOnKatsKock.OrgasmContinues".Localized());

            OutputText("Scenes.Katherine.UseReductoOnKatsKock.SmearsPasteInches".Localized(
                CockMultiple("", "s"),

                CockMultiple(" spasms and shrinks, ", "s spasm and shrink, ")
            ));

            if (Player.Libido > 50)
                OutputText("Scenes.Katherine.UseReductoOnKatsKock.AftershocksOnBelly".Localized());

            OutputText("Scenes.Katherine.UseReductoOnKatsKock.TwoInchesShorter".Localized());

            OutputText("Scenes.Katherine.UseReductoOnKatsKock.GroansStillHard".Localized(
                CatGirl("skin", "fur")
            ));

            if (Player.Libido > 50)
                OutputText("Scenes.Katherine.UseReductoOnKatsKock.IfFatiguedFragment".Localized());

            OutputText("Scenes.Katherine.UseReductoOnKatsKock.LookOnYouFragment".Localized());

            cockLength -= 2;

            var dickMin = (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) >= 4 ? 6 : 8);

            if (cockLength < dickMin)
                cockLength = dickMin;

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            Player.ConsumeItem(ItemIds.Reducto);

            KatSexMenu();
        }

        public void ReductoBallSize()
        {
            ClearOutput();

            if (IsAt(KLOC_STREETS))
                OutputText("Scenes.Katherine.ReductoBallSize.ExposesSwollenTestes".Localized(
                    ClothesLowerChoice("wriggles out of her shorts", "strips off her panties and raises her skirt", "strips off her bodysuit", "strips off her panties and raises her skirt", "unties her robe")
                ));

            OutputText("Scenes.Katherine.ReductoBallSize.WondersInterference".Localized(
                CatGirl("hair", "fur"),

                CatGirl("cat girl", "feline")
            ));

            OutputText("Scenes.Katherine.ReductoBallSize.WatchThemShrink".Localized(
                CockMultiple("", "s")
            ));

            ballSize -= 2;

            if (ballSize < 1)
                ballSize = 1;

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            Player.ConsumeItem(ItemIds.Reducto);

            KatSexMenu();
        }

        public void UseRreductoOnKatsBreasts()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.UseRreductoOnKatsBreasts.GetOnBed".Localized(
                breasts.Adj(),

                breasts.Cup()
            ));

            if (breasts.Lactating())
            {
                OutputText("Scenes.Katherine.UseRreductoOnKatsBreasts.TailGoesRigid".Localized());

                if (breasts.MilkFullness >= 100)
                    OutputText("Scenes.Katherine.UseRreductoOnKatsBreasts.CreamLongStreams".Localized());
                else
                    if (breasts.MilkIsFull())
                        OutputText("Scenes.Katherine.UseRreductoOnKatsBreasts.CreamLittleSquirts".Localized());
                    else
                        OutputText("Scenes.Katherine.UseRreductoOnKatsBreasts.CreamDropsTrails".Localized());
            }

            breasts.BreastRating--;

            OutputText("Scenes.Katherine.UseRreductoOnKatsBreasts.SitsUpPlays".Localized());

            if (breasts.BreastRating == 0)
                OutputText("Scenes.Katherine.UseRreductoOnKatsBreasts.SmallThinkBoy".Localized());
            else
                if (breasts.BreastRating == 1)
                    OutputText("Scenes.Katherine.UseRreductoOnKatsBreasts.BackToNaturalSize".Localized(
                        PlayerText()
                    ));
                else
                    if (breasts.BreastRating == BreastCups.DDBig)
                        OutputText("Scenes.Katherine.UseRreductoOnKatsBreasts.FeelLighterBetter".Localized());
                    else
                        if (breasts.BreastRating > BreastCups.DDBig)
                            OutputText("Scenes.Katherine.UseRreductoOnKatsBreasts.ThanksMarae".Localized(
                                PlayerText()
                            ));
                        else
                            OutputText("Scenes.Katherine.UseRreductoOnKatsBreasts.UsedToSmaller".Localized());

            OutputText("Scenes.Katherine.UseRreductoOnKatsBreasts.LiesBackSpreads".Localized(
                HasCock() ? "cock" + CockMultiple("", "s") + " aimed at the ceiling" : "entrance dripping"
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            Player.ConsumeItem(ItemIds.Reducto);

            KatSexMenu();
        }

        public void GiveKatABulbousPepper()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatABulbousPepper.OfferBulbousPepper".Localized());

            if (!HasCock())
            {
                OutputText("Scenes.Katherine.GiveKatABulbousPepper.NoEffectSmiles".Localized(
                    ClothesLowerChoice("pushes a hand down her shorts", "hikes up her skirt", "opens the folds of her bodysuit", "hikes up her dress", "parts her robe")
                ));

                OutputText("Scenes.Katherine.GiveKatABulbousPepper.PleaseDontLeave".Localized(
                    PlayerText()
                ));

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.BulbyCaninePepper));

                KatSexMenu();
            }
            else
                if (!HasBalls())
                {
                    OutputText("Scenes.Katherine.GiveKatABulbousPepper.UsedToNotHaving".Localized(
                        ClothesLowerChoice("shorts", "skirt", "bodysuit", "dress", "robe")
                    ));

                    if (!HasDogCock())
                    {
                        if (cockNumber > 1)
                            OutputText("Scenes.Katherine.GiveKatABulbousPepper.CocksAbsorbBarbs".Localized(
                                CockAdj()
                            ));
                        else
                            OutputText("Scenes.Katherine.GiveKatABulbousPepper.CockAbsorbsBarbs".Localized(
                                CockAdj()
                            ));

                        CoC.Instance.Flags.Set(GameFlagIds.KatherineDickForm, 0);
                    }

                    OutputText("Scenes.Katherine.GiveKatABulbousPepper.RubsNewBalls".Localized(
                        CockAdj(),

                        CockMultiple("cock stands", "twin cocks stand")
                    ));

                    ballSize = 1;

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.BulbyCaninePepper));

                    KatSexMenu();
                }
                else
                    if (ballSize < 5)
                    {
                        OutputText("Scenes.Katherine.GiveKatABulbousPepper.SarcasticBiggerBalls".Localized(
                            ClothesLowerChoice("and an arched back, she produces a New swell in her tight shorts as you look on", "she raises her skirt, allowing you to watch as she produces a New swell in her tight panties", "and an arched back, she produces a New swell in the crotch of her semi-transparent bodysuit", "she raises her skirt, allowing you to watch as she produces a New swell in her tight panties", "she unties her robe, allowing you to watch as she produces a New swell in her tight panties")
                        ));

                        if (!HasDogCock())
                        {
                            if (cockNumber > 1)
                                OutputText("Scenes.Katherine.GiveKatABulbousPepper.CocksAbsorbBarbsAgain".Localized(
                                    CockAdj()
                                ));
                            else
                                OutputText("Scenes.Katherine.GiveKatABulbousPepper.CockAbsorbsBarbsAgain".Localized(
                                    CockAdj()
                                ));

                            CoC.Instance.Flags.Set(GameFlagIds.KatherineDickForm, 0);
                        }

                        OutputText("Scenes.Katherine.GiveKatABulbousPepper.SultryTestRun".Localized());

                        ballSize += 2;

                        if (ballSize > 5)
                            ballSize = 5;

                        Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.BulbyCaninePepper));

                        KatSexMenu();
                    }
                    else
                        if (Submissiveness() >= 4 && ballSize < 9)
                        {
                            OutputText("Scenes.Katherine.GiveKatABulbousPepper.RefusesBulbousPepper".Localized());

                            OutputText("Scenes.Katherine.GiveKatABulbousPepper.WantsHerBigger".Localized(
                                BallAdj(),

                                ClothesLowerChoice("shorts", "skirt", "semi-transparent bodysuit", "dress", "robe")
                            ));

                            OutputText("Scenes.Katherine.GiveKatABulbousPepper.PantsAsSwells".Localized(
                                CockMultiple("cock begins", "cocks begin")
                            ));

                            OutputText("Scenes.Katherine.GiveKatABulbousPepper.MassageNutsMore".Localized());

                            OutputText("Scenes.Katherine.GiveKatABulbousPepper.TwistsTurnsEscape".Localized(
                                CatGirl("cat girl", "kitty"),

                                PlayerText()
                            ));

                            OutputText("Scenes.Katherine.GiveKatABulbousPepper.PushPepperLips".Localized());

                            OutputText("Scenes.Katherine.GiveKatABulbousPepper.ScrotumExpands".Localized());

                            if (!HasDogCock())
                            {
                                if (cockNumber > 1)
                                    OutputText("Scenes.Katherine.GiveKatABulbousPepper.CocksAbsorbBarbsThird".Localized(
                                        CockAdj()
                                    ));
                                else
                                    OutputText("Scenes.Katherine.GiveKatABulbousPepper.CockAbsorbsBarbsThird".Localized(
                                        CockAdj()
                                    ));

                                CoC.Instance.Flags.Set(GameFlagIds.KatherineDickForm, 0);
                            }

                            OutputText("Scenes.Katherine.GiveKatABulbousPepper.WrapsArmsSupport".Localized(
                                BallAdj()
                            ));

                            AddSubmissive(KBIT_SUB_GROW_BIG_BALLS);

                            ballSize += 2;

                            if (ballSize > 9)
                                ballSize = 9;

                            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.BulbyCaninePepper));

                            KatSexMenu();
                        }
                        else
                        {
                            if (Submissiveness() >= 4)
                            {
                                OutputText("Scenes.Katherine.GiveKatABulbousPepper.KneelsAndBegs".Localized(
                                    PlayerText()
                                ));

                                if (Player.Corruption >= 75)
                                {
                                    OutputText("Scenes.Katherine.GiveKatABulbousPepper.FunEnormousBalls".Localized(
                                        CatGirl("cat girl", "feline")
                                    ));

                                    OutputText("Scenes.Katherine.GiveKatABulbousPepper.ScratchEarAssure".Localized(
                                        CockMultiple(" beginning to slide from its sheath.", "s beginning to slide from their sheaths.")
                                    ));
                                }
                            }
                            else
                            {
                                OutputText("Scenes.Katherine.GiveKatABulbousPepper.RefusesBulbousPepperAgain".Localized());

                                OutputText("Scenes.Katherine.GiveKatABulbousPepper.HesitatesSecond".Localized());
                            }

                            KatherineMenu();
                        }
        }

        public void GiveKatACaninePepper()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatACaninePepper.OfferCaninePepper".Localized());

            if (HasDogCock())
            {
                OutputText("Scenes.Katherine.GiveKatACaninePepper.DontPushLuck".Localized());

                OutputText("Scenes.Katherine.GiveKatACaninePepper.NiceThoughtTaste".Localized());
            }
            else
            {
                OutputText("Scenes.Katherine.GiveKatACaninePepper.SureToGiveAgain".Localized(
                    CockMultiple("dog cock", "set of dog cocks"),

                    CockMultiple("", " and then two of 'em"),

                    CockMultiple("hard cock", "pair of hard cocks"),

                    CockMultiple("it", "them")
                ));

                OutputText("Scenes.Katherine.GiveKatACaninePepper.FeedCaninePepper".Localized(
                    ClothesLowerChoice("slips off her pants", "raises her skirt and slips off her panties", "open the folds of her bodysuit", "raises her skirt and slips off her panties", "unties her robe and slips off her panties"),

                    CockMultiple("", "s")
                ));

                OutputText("Scenes.Katherine.GiveKatACaninePepper.PepperStretchesCock".Localized(
                    CockMultiple("", "s"),

                    CockMultiple("its", "their"),

                    CockMultiple("", "pair of"),

                    CockAdj(),

                    CockMultiple("", "s"),

                    CockMultiple("it", "them")
                ));

                CoC.Instance.Flags.Set(GameFlagIds.KatherineDickForm, 0);

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.CaninePepper));
            }

            KatSexMenu();
        }

        public void GiveKatADoublePepper()
        {
            ClearOutput();

            if (!HasCock())
            {
                OutputText("Scenes.Katherine.GiveKatADoublePepper.OfferDoublePepper".Localized());

                OutputText("Scenes.Katherine.GiveKatADoublePepper.NoEffectSmiles".Localized(
                    ClothesLowerChoice("pushes a hand down her shorts", "hikes up her skirt", "opens the folds of her bodysuit", "hikes up her dress", "parts her robe")
                ));

                OutputText("Scenes.Katherine.GiveKatADoublePepper.PleaseDontLeave".Localized(
                    PlayerText()
                ));
            }
            else
                if (cockNumber == 1)
                {
                    OutputText("Scenes.Katherine.GiveKatADoublePepper.OfferDoublePepperAgain".Localized());

                    OutputText("Scenes.Katherine.GiveKatADoublePepper.DoubleYourFun".Localized(
                        ClothesLowerChoice("slip her pants down", "raise her skirt and slip off her panties", "open the folds of her bodysuit", "raise her skirt and slip off her panties", "untie her robe and slip off her panties")
                    ));

                    OutputText("Scenes.Katherine.GiveKatADoublePepper.GazeToCrotch".Localized(
                        CockType("canine", "kitty"),

                        cockLength,

                        knotSize
                    ));

                    if (!HasDogCock())
                    {
                        OutputText("Scenes.Katherine.GiveKatADoublePepper.CockAbsorbsBarbs".Localized(
                            CockAdj()
                        ));

                        CoC.Instance.Flags.Set(GameFlagIds.KatherineDickForm, 0);
                    }

                    OutputText("Scenes.Katherine.GiveKatADoublePepper.SecondTipPops".Localized());

                    cockNumber = 2;
                }
                else
                {
                    OutputText("Scenes.Katherine.GiveKatADoublePepper.PuzzledTwinnedPeppers".Localized());

                    if (!HasDogCock())
                    {
                        OutputText("Scenes.Katherine.GiveKatADoublePepper.BackToOldWay".Localized());

                        OutputText("Scenes.Katherine.GiveKatADoublePepper.CatOrDogCocks".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Katherine.GiveKatADoublePepper.TwoIsLimit".Localized());

                        OutputText("Scenes.Katherine.GiveKatADoublePepper.CouldUseMeal".Localized());
                    }

                    OutputText("Scenes.Katherine.GiveKatADoublePepper.EatsDaintilyPants".Localized(
                        ClothesLowerChoice("poke out of her pants", "lift the edge of her skirt", "stretch the thin fabric of her bodysuit", "lift the edge of her dress", "part the folds of her robe")
                    ));

                    if (!HasDogCock())
                    {
                        OutputText("Scenes.Katherine.GiveKatADoublePepper.TipsChangeExpand".Localized(
                            CockAdj()
                        ));

                        CoC.Instance.Flags.Set(GameFlagIds.KatherineDickForm, 0);
                    }

                    OutputText("Scenes.Katherine.GiveKatADoublePepper.SentencePeriod".Localized());

                    OutputText("Scenes.Katherine.GiveKatADoublePepper.BitTooSpicy".Localized());
                }

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.DoubleCaninePepper));

            KatSexMenu();
        }

        public void GiveKatAOverlyLargePepper()
        {
            ClearOutput();

            var kathSubEnough = Submissiveness() >= 4 || (Submissiveness() >= 2 && CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaTimesSex) > 0);

            if (!HasCock())
            {
                OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.WaveOverlyLarge".Localized());

                OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.NoEffectSays".Localized(
                    ClothesLowerChoice("pushes a hand down her shorts", "hikes up her skirt", "opens the folds of her bodysuit", "hikes up her dress", "parts her robe")
                ));

                OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.PleaseDontLeave".Localized(
                    PlayerText()
                ));

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LargeCaninePepper));

                KatSexMenu();
            }
            else
                if (cockLength < 16)
                {
                    OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.WouldReallyBigger".Localized());

                    OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.ExcitedSnatches".Localized(
                        CockMultiple("", "s"),

                        CockMultiple("s", "")
                    ));

                    if (!HasDogCock())
                    {
                        if (cockNumber > 1)
                            OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.TheyStretchBarbs".Localized(
                                CockAdj()
                            ));
                        else
                            OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.ItStretchesBarbs".Localized(
                                CockAdj()
                            ));

                        CoC.Instance.Flags.Set(GameFlagIds.KatherineDickForm, 0);
                    }

                    OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.MoansTestRun".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                    cockLength += 2;

                    if (cockLength > 16)
                        cockLength = 16;

                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LargeCaninePepper));

                    KatSexMenu();
                }
                else
                    if (cockLength < 20 && kathSubEnough)
                    {
                        OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.EagerReinsIn".Localized());

                        OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.FingerOnLips".Localized(
                            CockMultiple("root of her prick. A bit of gentle rubbing and her shaft begins", "little hollow between the roots of her pricks. A bit of gentle rubbing and her shafts begin")
                        ));

                        if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaTimesSex) > 0)
                        {
                            OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.ThinksYouLoveUrta".Localized());

                            OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.WrigglesYesOfCourse".Localized());

                            OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.PushAgainstWall".Localized());

                            OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.TailFlicksWall".Localized(
                                CockMultiple("dick gets", "dicks get")
                            ));
                        }
                        else
                        {
                            OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.RightSoFar".Localized(
                                CockMultiple("", "s")
                            ));

                            OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.WrigglesGuessCan".Localized());

                            OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.PushAgainstWallAgain".Localized(
                                CockMultiple("massive cock", "pair of massive cocks")
                            ));
                        }

                        OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.StaresILoveYou".Localized());

                        OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.CrunchesPepper".Localized(
                            CockMultiple("bulge", "twin bulges")
                        ));

                        OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.DisrobesNewCock".Localized(
                            CockMultiple(". It is", "s. They are")
                        ));

                        if (!HasDogCock())
                        {
                            OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.ChangedBackCanine".Localized(
                                CockMultiple("it", "them")
                            ));

                            CoC.Instance.Flags.Set(GameFlagIds.KatherineDickForm, 0);
                        }

                        OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.StrokesShaftOneHand".Localized(
                            CockMultiple("her", "each"),

                            CockAdj(),

                            CockMultiple(" while straightening her tail with the other", "")
                        ));

                        OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.WantedThisFeelsGood".Localized(
                            CockMultiple("pointing her cock", "leveling both her cocks")
                        ));

                        Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                        AddSubmissive(KBIT_SUB_GROW_BIG_DICK);

                        cockLength += 2;

                        if (cockLength > 20)
                            cockLength = 20;

                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LargeCaninePepper));

                        KatSexMenu();
                    }
                    else
                        if (kathSubEnough)
                        {
                            OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.LooksAtEnormous".Localized(
                                PlayerText(),

                                CockMultiple("a massive cock", "a pair of massive cocks"),

                                PlayerText()
                            ));

                            OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.CanSatisfyYou".Localized());

                            KatSexMenu();
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) >= 4)
                            {
                                OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.EagerReinsInAgain".Localized());

                                OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.NiceThinkingOfMe".Localized());

                                if (SceneInstance<UrtaScene>.Get().UrtaFuckbuddy())
                                    OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.HesitatesSecond".Localized());

                                KatherineMenu();
                            }
                            else
                            {
                                OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.EagerReinsInThird".Localized());

                                OutputText("Scenes.Katherine.GiveKatAOverlyLargePepper.AnythingElseGiving".Localized());

                                KatherineMenu();
                            }
        }

        public void GiveKatDefurFormula()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatDefurFormula.PullOutIngredients".Localized());

            OutputText("Scenes.Katherine.GiveKatDefurFormula.ExplainAlterPhysique".Localized());

            if (Submissiveness() < 3)
            {
                OutputText("Scenes.Katherine.GiveKatDefurFormula.MouthFallsOpen".Localized(
                    PlayerText()
                ));

                OutputText("Scenes.Katherine.GiveKatDefurFormula.HandsThemBack".Localized(
                    PlayerText()
                ));

                OutputText("Scenes.Katherine.GiveKatDefurFormula.RubsHandsFace".Localized());

                OutputText("Scenes.Katherine.GiveKatDefurFormula.HesitatesSecond".Localized());

                KatSexMenu();

                return ;
            }

            if (DoneSubmissive(KBIT_SUB_CAT_GIRL))
                OutputText("Scenes.Katherine.GiveKatDefurFormula.HugsYouTaste".Localized());
            else
            {
                OutputText("Scenes.Katherine.GiveKatDefurFormula.MouthFallsOpenAgain".Localized());

                OutputText("Scenes.Katherine.GiveKatDefurFormula.LikesMeThisWay".Localized(
                    PlayerText()
                ));

                OutputText("Scenes.Katherine.GiveKatDefurFormula.TearsWellUp".Localized());

                OutputText("Scenes.Katherine.GiveKatDefurFormula.WipesAwayTears".Localized());

                if (Player.Corruption < 25)
                    OutputText("Scenes.Katherine.GiveKatDefurFormula.WipeHappyTears".Localized());
                else
                    if (Player.Corruption < 75)
                        OutputText("Scenes.Katherine.GiveKatDefurFormula.WorriedMentally".Localized());
                    else
                        OutputText("Scenes.Katherine.GiveKatDefurFormula.HideTurnedOn".Localized());
            }

            OutputText("Scenes.Katherine.GiveKatDefurFormula.AskForRasp".Localized());

            if (DoneSubmissive(KBIT_SUB_CAT_GIRL))
                OutputText("Scenes.Katherine.GiveKatDefurFormula.StripsSwaysHips".Localized());
            else
            {
                OutputText("Scenes.Katherine.GiveKatDefurFormula.TakesCupSits".Localized());

                OutputText("Scenes.Katherine.GiveKatDefurFormula.SitWaitChanges".Localized());
            }

            OutputText("Scenes.Katherine.GiveKatDefurFormula.RubsPalms".Localized());

            OutputText("Scenes.Katherine.GiveKatDefurFormula.StaresNewNails".Localized(
                hairColor
            ));

            OutputText("Scenes.Katherine.GiveKatDefurFormula.WhispersFeelsWeird".Localized());

            OutputText("Scenes.Katherine.GiveKatDefurFormula.ConvincedStandsUp".Localized());

            if (DoneSubmissive(KBIT_SUB_CAT_GIRL))
            {
                OutputText("Scenes.Katherine.GiveKatDefurFormula.DoneThisBefore".Localized());

                OutputText("Scenes.Katherine.GiveKatDefurFormula.SkipsIntoArms".Localized(
                    PlayerText()
                ));
            }
            else
            {
                OutputText("Scenes.Katherine.GiveKatDefurFormula.SoDifferentTurns".Localized());

                OutputText("Scenes.Katherine.GiveKatDefurFormula.HandOverBreasts".Localized(
                    PlayerText()
                ));

                OutputText("Scenes.Katherine.GiveKatDefurFormula.ExplainingAtWork".Localized());
            }

            furry = false;

            AddSubmissive(KBIT_SUB_CAT_GIRL);

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.GoldenSeed));

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggBlack)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggBlack));
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargeBlack));

            KatSexMenu();
        }

        public void GiveKatAKnottyPepper()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatAKnottyPepper.ShowKnottedPepper".Localized());

            if (!HasCock())
            {
                OutputText("Scenes.Katherine.GiveKatAKnottyPepper.NoEffectSays".Localized(
                    ClothesLowerChoice("pushes a hand down her shorts", "hikes up her skirt", "opens the folds of her bodysuit", "hikes up her dress", "parts her robe")
                ));

                OutputText("Scenes.Katherine.GiveKatAKnottyPepper.PleaseDontLeave".Localized(
                    PlayerText()
                ));

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.KnottyCaninePepper));
            }
            else
                if (knotSize >= 6)
                {
                    OutputText("Scenes.Katherine.GiveKatAKnottyPepper.CantGetBigger".Localized(
                        CockMultiple("", "s")
                    ));

                    OutputText("Scenes.Katherine.GiveKatAKnottyPepper.ExaminesPopsMouth".Localized(
                        PlayerText()
                    ));

                    if (HasDogCock())
                        OutputText("Scenes.Katherine.GiveKatAKnottyPepper.SitsAndWatches".Localized(
                            CockMultiple("is her rock solid erection.", "are her rock solid twin erections.")
                        ));
                    else
                    {
                        OutputText("Scenes.Katherine.GiveKatAKnottyPepper.EyesWideCrosses".Localized());

                        if (cockNumber > 1)
                            OutputText("Scenes.Katherine.GiveKatAKnottyPepper.CocksStretchOutwards".Localized(
                                CockAdj()
                            ));
                        else
                            OutputText("Scenes.Katherine.GiveKatAKnottyPepper.CockStretchesOutwards".Localized(
                                CockAdj()
                            ));

                        CoC.Instance.Flags.Set(GameFlagIds.KatherineDickForm, 0);
                    }

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.KnottyCaninePepper));
                }
                else
                    if (knotSize >= 4)
                    {
                        OutputText("Scenes.Katherine.GiveKatAKnottyPepper.GoodMemories".Localized());

                        OutputText("Scenes.Katherine.GiveKatAKnottyPepper.TriesToGiveBack".Localized());

                        if (Submissiveness() < 1)
                        {
                            OutputText("Scenes.Katherine.GiveKatAKnottyPepper.NotReadyForThat".Localized());

                            OutputText("Scenes.Katherine.GiveKatAKnottyPepper.TeaseKnotFingers".Localized(
                                CockMultiple("", "s")
                            ));

                            OutputText("Scenes.Katherine.GiveKatAKnottyPepper.SomeOtherTimeLove".Localized());

                            OutputText("Scenes.Katherine.GiveKatAKnottyPepper.HesitatesSecond".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Katherine.GiveKatAKnottyPepper.HugsDoingForYou".Localized(
                                PlayerText()
                            ));

                            if (!HasDogCock())
                            {
                                OutputText("Scenes.Katherine.GiveKatAKnottyPepper.EyesWideCrossesAgain".Localized());

                                if (cockNumber > 1)
                                    OutputText("Scenes.Katherine.GiveKatAKnottyPepper.CocksStretchOutwardsAgain".Localized(
                                        CockAdj()
                                    ));
                                else
                                    OutputText("Scenes.Katherine.GiveKatAKnottyPepper.CockStretchesOutwardsAgain".Localized(
                                        CockAdj()
                                    ));

                                CoC.Instance.Flags.Set(GameFlagIds.KatherineDickForm, 0);
                            }

                            OutputText("Scenes.Katherine.GiveKatAKnottyPepper.KnotAlmostMinute".Localized(
                                CockMultiple(" reacts", "s react"),

                                CockMultiple(" just inflates", "s just inflate")
                            ));

                            OutputText("Scenes.Katherine.GiveKatAKnottyPepper.WatchesNoReaction".Localized(
                                CockMultiple(", which is throbbing slowly, the only indication that she just ate a magic imbued pepper is her rock solid erection.", "s, which are throbbing slowly, the only indications that she just ate a magic imbued pepper are her rock solid twin erections.")
                            ));

                            knotSize += 2;

                            if (knotSize > 6)
                                knotSize = 6;

                            AddSubmissive(KBIT_SUB_GROW_KNOT);

                            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.KnottyCaninePepper));
                        }
                    }
                    else
                    {
                        OutputText("Scenes.Katherine.GiveKatAKnottyPepper.LaughsHowForget".Localized(
                            CockMultiple("her cock", "one cock then the other")
                        ));

                        OutputText("Scenes.Katherine.GiveKatAKnottyPepper.LicksExperimentally".Localized(
                            CockMultiple("", "s"),

                            CockMultiple("this bad boy kicks in my knot is", "these bad boys kick in my knots are each")
                        ));

                        if (!HasDogCock())
                        {
                            OutputText("Scenes.Katherine.GiveKatAKnottyPepper.EyesWideCrossesThird".Localized());

                            if (cockNumber > 1)
                                OutputText("Scenes.Katherine.GiveKatAKnottyPepper.CocksStretchOutwardsThird".Localized(
                                    CockAdj()
                                ));
                            else
                                OutputText("Scenes.Katherine.GiveKatAKnottyPepper.CockStretchesOutwardsThird".Localized(
                                    CockAdj()
                                ));

                            CoC.Instance.Flags.Set(GameFlagIds.KatherineDickForm, 0);
                        }

                        OutputText("Scenes.Katherine.GiveKatAKnottyPepper.KnotAlmostMinuteAgain".Localized(
                            CockMultiple(" reacts", "s react"),

                            CockMultiple(" just inflates", "s just inflate")
                        ));

                        OutputText("Scenes.Katherine.GiveKatAKnottyPepper.WatchesNoReactionAgain".Localized(
                            CockMultiple(", which is slowly shrinking, the only indication that she just ate a magic imbued pepper is her rock solid erection.", "s, which are slowly shrinking, the only indications that she just ate a magic imbued pepper are her rock solid twin erections.")
                        ));

                        knotSize += 2;

                        if (knotSize > 4)
                            knotSize = 4;

                        Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.KnottyCaninePepper));
                    }

            if (HasCock())
                OutputText("Scenes.Katherine.GiveKatAKnottyPepper.TailFlicksLeg".Localized(
                    CockMultiple("", "s"),

                    CockMultiple("", "s")
                ));

            KatSexMenu();
        }

        public void UsePinkEggOnKath()
        {
            ClearOutput();

            if (cockNumber == 1)
            {
                OutputText("Scenes.Katherine.UsePinkEggOnKath.ShowPinkEgg".Localized());

                if (Submissiveness() < 6)
                {
                    OutputText("Scenes.Katherine.UsePinkEggOnKath.DontWantLose".Localized(
                        PlayerText()
                    ));

                    KatherineMenu();

                    return ;
                }
                else
                {
                    if (DoneSubmissive(KBIT_SUB_REM_COCK))
                        OutputText("Scenes.Katherine.UsePinkEggOnKath.StrokesCockTakes".Localized());
                    else
                    {
                        OutputText("Scenes.Katherine.UsePinkEggOnKath.DontWantLoseAgain".Localized(
                            PlayerText()
                        ));

                        OutputText("Scenes.Katherine.UsePinkEggOnKath.NeverBeenWithout".Localized());

                        OutputText("Scenes.Katherine.UsePinkEggOnKath.NoImmediateAnswer".Localized());

                        OutputText("Scenes.Katherine.UsePinkEggOnKath.HandHerEgg".Localized());

                        OutputText("Scenes.Katherine.UsePinkEggOnKath.WorriedSmileTakes".Localized());

                        AddSubmissive(KBIT_SUB_REM_COCK);
                    }

                    OutputText("Scenes.Katherine.UsePinkEggOnKath.CracksAndGulps".Localized());

                    OutputText("Scenes.Katherine.UsePinkEggOnKath.AllWomanNow".Localized());

                    cockLength = 6;

                    cockNumber = 0;

                    ballSize = 0;

                    knotSize = 2;
                }
            }
            else
            {
                OutputText("Scenes.Katherine.UsePinkEggOnKath.ShowPinkEggReduce".Localized());

                OutputText("Scenes.Katherine.UsePinkEggOnKath.TakesEggThinks".Localized());

                OutputText("Scenes.Katherine.UsePinkEggOnKath.CarefullyCracks".Localized(
                    CockType("doggy dick", "kitty cock")
                ));

                OutputText("Scenes.Katherine.UsePinkEggOnKath.ThoughtWouldHurt".Localized(
                    PlayerText()
                ));

                cockNumber = 1;
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggPink)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggPink));
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargePink));

            KatSexMenu();
        }

        public void UseIncubiDraftOnKath()
        {
            ClearOutput();

            if (!HasCock())
            {
                OutputText("Scenes.Katherine.UseIncubiDraftOnKath.ShowIncubiDraft".Localized());

                OutputText("Scenes.Katherine.UseIncubiDraftOnKath.CantWaitHole".Localized(
                    PlayerText()
                ));

                OutputText("Scenes.Katherine.UseIncubiDraftOnKath.StripsDownsBottle".Localized());

                OutputText("Scenes.Katherine.UseIncubiDraftOnKath.HipsLaunchUpward".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineDickForm, 0);

                cockNumber = 1;

                OutputText("Scenes.Katherine.UseIncubiDraftOnKath.DoggieDickAgain".Localized());

                if (!DoneSubmissive(KBIT_SUB_REM_BALLS))
                {
                    AddSubmissive(KBIT_SUB_REM_BALLS);

                    OutputText("Scenes.Katherine.UseIncubiDraftOnKath.StrokesNewErection".Localized());

                    OutputText("Scenes.Katherine.UseIncubiDraftOnKath.LaughRemindTestes".Localized());

                    OutputText("Scenes.Katherine.UseIncubiDraftOnKath.RubsBareSkin".Localized());
                }

                OutputText("Scenes.Katherine.UseIncubiDraftOnKath.AimsShaftAsks".Localized());
            }
            else
            {
                var kathSubEnough = Submissiveness() >= 4 || (Submissiveness() >= 2 && CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaTimesSex) > 0);

                OutputText("Scenes.Katherine.UseIncubiDraftOnKath.ShowIncubiDraftSingle".Localized(
                    CockType("canine member", "kitty cock")
                ));

                if (cockLength < 16)
                {
                    cockLength += 2;

                    if (cockLength > 16)
                        cockLength = 16;

                    OutputText("Scenes.Katherine.UseIncubiDraftOnKath.EagerlyExamines".Localized(
                        CockMultiple("", "s"),

                        cockLength,

                        CockMultiple("", "s")
                    ));

                    OutputText("Scenes.Katherine.UseIncubiDraftOnKath.LicksPrecumPurrs".Localized(
                        PlayerText()
                    ));
                }
                else
                    if (cockLength < 20 && kathSubEnough)
                    {
                        OutputText("Scenes.Katherine.UseIncubiDraftOnKath.RollsBottleReins".Localized(
                            PlayerText()
                        ));

                        OutputText("Scenes.Katherine.UseIncubiDraftOnKath.KissToStopHer".Localized(
                            ClothesLowerChoice(" through her tight shorts ", ", grinding her skirt against her concealed prick", " through her semi-transparent bodysuit", ", grinding her dress against her concealed prick", ", soon sliding your fingers inside her robe, allowing you to feel the swell developing in her tight panties"),

                            CockMultiple(" is", "s are"),

                            CockMultiple(" grows", "s grow")
                        ));

                        OutputText("Scenes.Katherine.UseIncubiDraftOnKath.TooMuchReply".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaTimesSex) > 0)
                        {
                            OutputText("Scenes.Katherine.UseIncubiDraftOnKath.ThinksYouLoveUrta".Localized());

                            OutputText("Scenes.Katherine.UseIncubiDraftOnKath.MoansYesOfCourse".Localized());

                            OutputText("Scenes.Katherine.UseIncubiDraftOnKath.PushAgainstWall".Localized());

                            OutputText("Scenes.Katherine.UseIncubiDraftOnKath.TailFlicksWall".Localized(
                                CockMultiple("dick gets", "dicks get")
                            ));
                        }
                        else
                        {
                            OutputText("Scenes.Katherine.UseIncubiDraftOnKath.RightSoFar".Localized(
                                CockMultiple("", "s")
                            ));

                            OutputText("Scenes.Katherine.UseIncubiDraftOnKath.WrigglesGuessCan".Localized());

                            OutputText("Scenes.Katherine.UseIncubiDraftOnKath.PushAgainstWallAgain".Localized(
                                CockMultiple("massive cock", "pair of massive cocks")
                            ));
                        }

                        cockLength += 2;

                        if (cockLength > 20)
                            cockLength = 20;

                        AddSubmissive(KBIT_SUB_GROW_BIG_DICK);

                        OutputText("Scenes.Katherine.UseIncubiDraftOnKath.StaresWhiteFluid".Localized());

                        OutputText("Scenes.Katherine.UseIncubiDraftOnKath.StripClothesLaugh".Localized(
                            CockMultiple("cock comes", "twin shafts come"),

                            CockMultiple("it", "them"),

                            CockMultiple("It now sticks", "They now stick"),

                            CockMultiple("her shaft", "one, then the other"),

                            cockLength,

                            CockMultiple("", "... twice over")
                        ));

                        OutputText("Scenes.Katherine.UseIncubiDraftOnKath.LooksDeepHorny".Localized());
                    }
                    else
                        if (kathSubEnough)
                        {
                            OutputText("Scenes.Katherine.UseIncubiDraftOnKath.LooksAtEnormous".Localized(
                                PlayerText(),

                                CockMultiple("a massive cock", "a pair of massive cocks"),

                                PlayerText()
                            ));

                            OutputText("Scenes.Katherine.UseIncubiDraftOnKath.CanSatisfyYou".Localized());

                            KatSexMenu();

                            return ;
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) >= 4)
                            {
                                OutputText("Scenes.Katherine.UseIncubiDraftOnKath.ClosesEyesShakes".Localized(
                                    CockMultiple("one", "pair")
                                ));

                                OutputText("Scenes.Katherine.UseIncubiDraftOnKath.NiceThinkingOfMe".Localized());

                                OutputText("Scenes.Katherine.UseIncubiDraftOnKath.HesitatesSecond".Localized());

                                KatherineMenu();

                                return ;
                            }
                            else
                            {
                                OutputText("Scenes.Katherine.UseIncubiDraftOnKath.ClosesEyesShakesAgain".Localized(
                                    CockMultiple("one", "pair")
                                ));

                                OutputText("Scenes.Katherine.UseIncubiDraftOnKath.AnythingElseGiving".Localized());

                                KatherineMenu();

                                return ;
                            }
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft));

            KatSexMenu();
        }

        public void GiveKatPureSuccubusDelight()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.ShowSuccubiDelight".Localized());

            if (!HasBalls())
            {
                OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.WithoutBallsDrink".Localized());

                OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.CumBucketsBalls".Localized());

                OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.TipsBackShrugs".Localized());

                OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.ShiftsAssSide".Localized());

                OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.LightSmackHops".Localized(
                    PlayerText(),

                    CockMultiple("", "s")
                ));

                ballSize--;
            }
            else
                if (ballSize < 5)
                {
                    OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.WantedLongerStreams".Localized());

                    OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.LaughsBiggerBalls".Localized(
                        ClothesLowerChoice("and an arched back, she produces a New swell in her tight shorts as you look on", "she raises her skirt, allowing you to watch as she produces a New swell in her tight panties", "and an arched back, she produces a New swell in the crotch of her semi-transparent bodysuit", "she raises her skirt, allowing you to watch as she produces a New swell in her tight panties", "she unties her robe, allowing you to watch as she produces a New swell in her tight panties")
                    ));

                    OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.SultryTestRun".Localized());

                    ballSize += 2;

                    if (ballSize > 5)
                        ballSize = 5;
                }
                else
                    if (Submissiveness() >= 4 && ballSize < 9)
                    {
                        AddSubmissive(KBIT_SUB_GROW_BIG_BALLS);

                        ballSize += 2;

                        if (ballSize > 9)
                            ballSize = 9;

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.RefusesBottle".Localized());

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.WantsHerBigger".Localized(
                            BallAdj(),

                            ClothesLowerChoice("shorts", "skirt", "semi-transparent bodysuit", "dress", "robe")
                        ));

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.PantsAsHardens".Localized(
                            CockMultiple("cock begins", "cocks begin")
                        ));

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.MassageNutsMore".Localized());

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.TwistsTurnsEscape".Localized(
                            CatGirl("cat girl", "kitty"),

                            PlayerText()
                        ));

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.HandBottleDrinks".Localized());

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.ScrotumExpands".Localized(
                            ballSize
                        ));

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.WrapsArmsSupport".Localized(
                            BallAdj()
                        ));
                    }
                    else
                    {
                        if (Submissiveness() >= 4)
                        {
                            OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.KneelsAndBegs".Localized(
                                PlayerText()
                            ));

                            if (Player.Corruption >= 75)
                            {
                                OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.FunEnormousBalls".Localized(
                                    CatGirl("cat girl", "feline")
                                ));

                                OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.ScratchEarAssure".Localized(
                                    CockMultiple(" beginning to slide from its sheath.", "s beginning to slide from their sheaths.")
                                ));
                            }
                        }
                        else
                        {
                            OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.RefusesBigger".Localized());

                            OutputText("Scenes.Katherine.GiveKatPureSuccubusDelight.HesitatesSecond".Localized());
                        }

                        KatherineMenu();

                        return ;
                    }

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineHoursSinceCum, CoC.Instance.Flags.Get(GameFlagIds.KatherineHoursSinceCum) + 24);

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubusDelight));

            KatSexMenu();
        }

        public void GiveKatPureSuccubusMilk()
        {
            ClearOutput();

            var kathSubEnough = Submissiveness() >= 2;

            if (breasts.BreastRating < BreastCups.DDBig)
            {
                OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.ShowSuccubusMilk".Localized());

                OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.CupsAsksBigger".Localized(
                    breasts.Adj(),

                    breasts.Cup()
                ));

                if (breasts.BreastRating == 0)
                    OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.BackToNaturalSize".Localized());
                else
                    if (breasts.BreastRating == 3)
                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.StillNotBigEnough".Localized());
                    else
                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.CouldTryPurified".Localized());

                OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.StripsBeforeMirror".Localized());

                OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.BreastsBalloon".Localized());

                breasts.BreastRating += 1;

                if (breasts.BreastRating == 1)
                {
                    OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.YesYesYesGood".Localized());

                    OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.TurnsIntoArms".Localized());
                }
                else
                    if (breasts.BreastRating < BreastCups.DDBig)
                    {
                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.MoreSandHourglass".Localized());

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.BouncesWatchesRack".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.PuppiesHeavier".Localized());

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.BouncesSaysYeah".Localized());

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.TurnsArmsQuestion".Localized());

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.NipplesHardenPoint".Localized());

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.ParadingThroughTown".Localized(
                            PlayerText(),

                            CatGirl("girl", "feline lover")
                        ));

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.GrindsBreastsHips".Localized());
                    }

                OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.LovingMoodDisappoint".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));
            }
            else
                if (breasts.BreastRating < BreastCups.EE && kathSubEnough)
                {
                    OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.DisrobeForPresent".Localized());

                    OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.ButImSoBig".Localized(
                        PlayerText()
                    ));

                    OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.CupsCutHerOff".Localized());

                    OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.DeepDownWants".Localized());

                    OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.WhatHaveIDone".Localized());

                    OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.KissWorkFingers".Localized(
                        PlayerText()
                    ));

                    breasts.BreastRating += 1;

                    AddSubmissive(KBIT_SUB_GROW_BIG_BOOBS);

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));
                }
                else
                    if (kathSubEnough)
                    {
                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.DisrobeForPresentAgain".Localized());

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.ImSoBigAgain".Localized(
                            PlayerText()
                        ));

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.TooMuchTitflesh".Localized(
                            PlayerText()
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.ShowSuccubusMilkAgain".Localized());

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.HugeTitsEnough".Localized());

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.RubsWhiskers".Localized());

                        if (HasCock())
                            OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.CockGettingErect".Localized(
                                ClothesLowerChoice("inside her pants", "under her skirt", "inside her bodysuit", "under her dress", "under her robe")
                            ));
                        else
                            OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.ItWord".Localized());

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.AppreciatesGift".Localized());

                        OutputText("Scenes.Katherine.GiveKatPureSuccubusMilk.HesitatesSecond".Localized());
                    }

            KatSexMenu();
        }

        public void GiveKatTheGiftOFMilk()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.ShowLactaidBottle".Localized());

            if (!DoneSubmissive(KBIT_SUB_START_LACTATION))
            {
                OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.ExaminesBottleGingerly".Localized());

                OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.PullCorkPushBottle".Localized());

                OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.SighsWantToTry".Localized());
            }

            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.RemovesBraExposes".Localized(
                ClothesChoice("undoes her blouse", "opens the top half of her bodysuit", "undoes the upper half of her dress", "opens her robe", "lifts up her tube top", "undoes the buttons on her nurse's shirt")
            ));

            if (breasts.Lactation < 3)
            {
                if (DoneSubmissive(KBIT_SUB_START_LACTATION))
                    OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.AsksWantMoreMilk".Localized());
                else
                {
                    OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.TossesBackBottleAdds".Localized());

                    breasts.Lactation = 1;
                }

                OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.WatchEffectsKickIn".Localized());

                if (breasts.BreastRating < 3)
                {
                    breasts.BreastRating += 1;

                    OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.SayShesANow".Localized(
                        breasts.Cup()
                    ));
                }

                OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.CreamDripsNipples".Localized());

                if (!DoneSubmissive(KBIT_SUB_START_LACTATION))
                {
                    OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.TasteByFinger".Localized());

                    OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.HopesYouLike".Localized());
                }

                AddSubmissive(KBIT_SUB_START_LACTATION);

                MilkOptionClear(KBIT_MILK_TOLD_PLAYER_SHES_DRY);

                if (breasts.Lactation == 2 && breasts.PreventLactationIncrease == 2)
                    breasts.PreventLactationIncrease = 0;

                if (!DoneSubmissive(KBIT_SUB_HIGH_LACTATION))
                    breasts.PreventLactationIncrease = 3;

                breasts.Lactation += 1;

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.Lactaid));
            }
            else
                if (Submissiveness() < 3)
                {
                    OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.CupsSqueezesDroplets".Localized(
                        PlayerText()
                    ));

                    OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.PushesBottleToChest".Localized());
                }
                else
                    if (breasts.Lactation == 6)
                    {
                        OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.CupsSqueezesStream".Localized(
                            CatGirl("running over", "soaking into the fur of"),

                            ClothesLowerChoice("her pants", "her skirt", "the lower half of her bodysuit", "the lower half of her dress", "the lower half of her robe")
                        ));

                        OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.WasteOfBottle".Localized());
                    }
                    else
                    {
                        switch (breasts.Lactation)
                        {
                            case 3:
                            {
                            if (breasts.PreventLactationIncrease == 3)
                                breasts.PreventLactationIncrease = 0;
                            }

                            break;

                            case 4:
                            {
                            if (breasts.PreventLactationIncrease == 4)
                                breasts.PreventLactationIncrease = 0;
                            }

                            break;

                            case 5:
                            {
                            if (breasts.PreventLactationIncrease == 5)
                                breasts.PreventLactationIncrease = 0;
                            }

                            break;
                        }

                        breasts.Lactation += 1;

                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.Lactaid));

                        if (!DoneSubmissive(KBIT_SUB_HIGH_LACTATION))
                        {
                            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.CupsSqueezesDropletsAgain".Localized(
                                PlayerText()
                            ));

                            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.TriesHandBottleBack".Localized(
                                ClothesLowerChoice("slide your hand under her belt and into her pants", "slip your hand under her skirt", "push your fingers into the crotch of her bodysuit", "slip your hand under her dress", "slide your hand inside her her robe")
                            ));

                            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.PurrsQuietlyNo".Localized());

                            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.OfCourseItDoes".Localized());

                            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.EyesOpenPartway".Localized());

                            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.CupFreeHandWarmth".Localized());

                            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.SlumpsAsleepAfter".Localized());

                            ReturnToCamp(1);

                            AddSubmissive(KBIT_SUB_HIGH_LACTATION);

                            return ;
                        }
                        else
                        {
                            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.PutsBottleHandsChest".Localized());

                            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.RubsNipplesAttention".Localized());

                            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.WarmthAgain".Localized());

                            if (breasts.BreastRating < BreastCups.DDBig)
                            {
                                breasts.BreastRating += 1;

                                OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.ExpandBetweenFingers".Localized(
                                    breasts.Cup()
                                ));
                            }

                            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.DribblesUncontrollably".Localized());

                            OutputText("Scenes.Katherine.GiveKatTheGiftOFMilk.BegsDrinkItAll".Localized());
                        }
                    }

            KatSexMenu();
        }

        public void GiveKatRefuzzFormula()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatRefuzzFormula.TimeToMakeMorph".Localized());

            OutputText("Scenes.Katherine.GiveKatRefuzzFormula.FetchesMixingCup".Localized(
                PlayerText()
            ));

            ClearButtons();

            AddButton(0, "Common.Button.ChangeHer".Localized(), GiveKatRefuzzFormulaChange);

            AddButton(1, "Common.Button.DontChange".Localized(), GiveKatRefuzzFormulaDontChange);
        }

        public void GiveKatRefuzzFormulaChange()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatRefuzzFormulaChange.DumpHoneySerum".Localized());

            OutputText("Scenes.Katherine.GiveKatRefuzzFormulaChange.MixtureBeginsFoam".Localized());

            OutputText("Scenes.Katherine.GiveKatRefuzzFormulaChange.SlathersClearGel".Localized());

            OutputText("Scenes.Katherine.GiveKatRefuzzFormulaChange.NoseChangesMuzzle".Localized());

            OutputText("Scenes.Katherine.GiveKatRefuzzFormulaChange.FurReturnsLast".Localized(
                hairColor == "jet black" ? "" : "She runs a hand through her " + hairColor + " hair. Well, mostly. "
            ));

            OutputText("Scenes.Katherine.GiveKatRefuzzFormulaChange.SitScratchNewFur".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Katherine.GiveKatRefuzzFormulaChange.CockWord".Localized());
            else
                OutputText("Scenes.Katherine.GiveKatRefuzzFormulaChange.CrotchWord".Localized());

            OutputText("Scenes.Katherine.GiveKatRefuzzFormulaChange.PlaceholderPeriodBreak".Localized());

            OutputText("Scenes.Katherine.GiveKatRefuzzFormulaChange.BegsMakeMeScream".Localized(
                PlayerText()
            ));

            if (HasCock())
                OutputText("Scenes.Katherine.GiveKatRefuzzFormulaChange.EmptyAndMyFragment".Localized());

            OutputText("Scenes.Katherine.GiveKatRefuzzFormulaChange.PussyDrenchedSayEnd".Localized());

            furry = true;

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            Player.ConsumeItem(ConsumableIds.W_FRUIT);

            Player.ConsumeItem(ConsumableIds.HairSerum);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.BeeHoney)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.BeeHoney));
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PureHoney));

            KatSexMenu();
        }

        public void GiveKatRefuzzFormulaDontChange()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatRefuzzFormulaDontChange.AsksIfSure".Localized());

            OutputText("Scenes.Katherine.GiveKatRefuzzFormulaDontChange.PrefersSoftFabric".Localized());

            OutputText("Scenes.Katherine.GiveKatRefuzzFormulaDontChange.KissesLipsNeckEar".Localized(
                PlayerText()
            ));

            KatSexMenu();
        }

        public void UseReductoAndPinkEgg()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.UseReductoAndPinkEgg.ShowPinkEggReducto".Localized());

            if (!DoneSubmissive(KBIT_SUB_REM_BALLS))
                OutputText("Scenes.Katherine.UseReductoAndPinkEgg.FeelsBallsHesitates".Localized());

            if (Submissiveness() < 3)
            {
                OutputText("Scenes.Katherine.UseReductoAndPinkEgg.HesitatedASecond".Localized());

                KatherineMenu();
            }
            else
            {
                OutputText("Scenes.Katherine.UseReductoAndPinkEgg.PutSalveEggDown".Localized());

                OutputText("Scenes.Katherine.UseReductoAndPinkEgg.StripsJumpsBed".Localized(
                    CockMultiple("", "s")
                ));

                OutputText("Scenes.Katherine.UseReductoAndPinkEgg.JarOnBelly".Localized());

                if (!DoneSubmissive(KBIT_SUB_REM_BALLS))
                {
                    OutputText("Scenes.Katherine.UseReductoAndPinkEgg.FingerOnLipsTrust".Localized());

                    if (Player.Corruption < 25)
                        OutputText("Scenes.Katherine.UseReductoAndPinkEgg.WarmthInHeart".Localized());
                    else
                        if (Player.Corruption < 75)
                            OutputText("Scenes.Katherine.UseReductoAndPinkEgg.GladThatWorked".Localized());
                        else
                            OutputText("Scenes.Katherine.UseReductoAndPinkEgg.WillingToTrust".Localized(
                                Player.GenderText("gives you wood", "makes you wet", "gives you wood and makes your pussy wet", "creates lusty feelings you are ill equipped to deal with")
                            ));
                }

                OutputText("Scenes.Katherine.UseReductoAndPinkEgg.SentencePeriod".Localized());

                OutputText("Scenes.Katherine.UseReductoAndPinkEgg.CrackEggPourJar".Localized(
                    CockMultiple("", "s")
                ));

                OutputText("Scenes.Katherine.UseReductoAndPinkEgg.ColdBodySquirms".Localized());

                OutputText("Scenes.Katherine.UseReductoAndPinkEgg.WrigglesTooFull".Localized(
                    CockMultiple(" balloons out to its fullest and her cock sprays semen against the ceiling. You stroke her cock", "s balloon out to their fullest and her cocks spray semen against the ceiling. You stroke her cocks")
                ));

                OutputText("Scenes.Katherine.UseReductoAndPinkEgg.ProbesCrotchDrips".Localized(
                    CockMultiple("", "s")
                ));

                if (!DoneSubmissive(KBIT_SUB_REM_BALLS))
                    OutputText("Scenes.Katherine.UseReductoAndPinkEgg.ReallyWasntSure".Localized());

                OutputText("Scenes.Katherine.UseReductoAndPinkEgg.NextTimeWantSee".Localized());

                OutputText("Scenes.Katherine.UseReductoAndPinkEgg.LetOutQuietlyHopeful".Localized());

                Orgasm();

                ballSize = 0;

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggPink));

                Player.ConsumeItem(ItemIds.Reducto);

                AddSubmissive(KBIT_SUB_REM_BALLS);

                Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
            }
        }

        public void GiveKatWhiskerFruit()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatWhiskerFruit.OfferFuzzyFruit".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.TelltaleSwelling".Localized(
                    ClothesLowerChoice("in her pants", "beneath her skirt", "in the crotch of her bodysuit", "beneath her dress", "beneath her robe")
                ));
            else
                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.NipplesHardDiamonds".Localized());

            if (!HasCock() || !HasDogCock())
            {
                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.BitesSucksJuice".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                Player.ConsumeItem(ConsumableIds.W_FRUIT);

                KatSexMenu();

                return ;
            }

            var doneBefore = DoneSubmissive(KBIT_SUB_CAT_DICK);

            OutputText("Scenes.Katherine.GiveKatWhiskerFruit.WavesFruitAway".Localized());

            if (doneBefore)
                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.IsThatWhatWant".Localized(
                    CockMultiple("", "s"),

                    PlayerText()
                ));
            else
            {
                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.PlaceholderSayEndBreak".Localized(
                    CockMultiple(". It's been with me so long I've really gotten used to having this big smooth tip and a knot to go with it", "s. It... well, they've been with me so long I've really got used to having these big smooth tips and the knots to go with them")
                ));

                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.RubsCrotchAbsently".Localized(
                    CockMultiple("It's a", "They're")
                ));

                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.CockSwollenFull".Localized(
                    CockMultiple(" has", "s have")
                ));
            }

            if (Submissiveness() >= 1)
            {
                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.WalkFruitUnderNose".Localized(
                    CockMultiple("", "s")
                ));

                if (!doneBefore)
                    OutputText("Scenes.Katherine.GiveKatWhiskerFruit.RemindCanineCock".Localized());

                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.HeadSwaysPendulum".Localized());

                if (!doneBefore)
                    OutputText("Scenes.Katherine.GiveKatWhiskerFruit.WantsKittyCock".Localized());

                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.PurringLowerBites".Localized(
                    CockMultiple("member", "twin members")
                ));

                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.CockGrowsPointed".Localized(
                    CockMultiple(" narrows along its entire length. The tip becomes", "s narrow along their entire length. The tips become"),

                    CockMultiple(" is", "s are"),

                    cockLength > 12 ? ", rather narrow for " + CockMultiple("its", "their") + " length" : ""
                ));

                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.ChangesReachKnot".Localized());

                if (cockNumber > 1)
                    OutputText("Scenes.Katherine.GiveKatWhiskerFruit.ShrinkInflateRepeat".Localized());
                else
                    OutputText("Scenes.Katherine.GiveKatWhiskerFruit.ItShrinksInflates".Localized());

                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.SentencePeriodBreak".Localized());

                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.FeelsShaftAfter".Localized(
                    CockMultiple("", "s")
                ));

                if (!doneBefore)
                    OutputText("Scenes.Katherine.GiveKatWhiskerFruit.TracesAlongBulge".Localized(
                        CockMultiple("a knot", "knots"),

                        CatGirl("nails", "claws"),

                        CockMultiple("", "s"),

                        CockMultiple("it", "them")
                    ));

                OutputText("Scenes.Katherine.GiveKatWhiskerFruit.KissesHardWantTry".Localized(
                    PlayerText(),

                    doneBefore ? "Nubs and knot" + CockMultiple("", "s") + ", all in one" : "She's got an all New kind of prick for you to test out"
                ));

                CoC.Instance.Flags.Set(GameFlagIds.KatherineDickForm, 1);

                AddSubmissive(KBIT_SUB_CAT_DICK);

                knotSize -= 2;

                if (knotSize < 2)
                    knotSize = 2;

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                Player.ConsumeItem(ConsumableIds.W_FRUIT);
            }

            KatSexMenu();
        }

        public void GiveKatClothesBodysuit()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesBodysuit.StandBeforeMirror".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesBodysuit.EyesFlyOpenFabric".Localized(
                PlayerText(),

                PlayerText()
            ));

            OutputText("Scenes.Katherine.GiveKatClothesBodysuit.ArmAroundWaist".Localized(
                CatGirl("Kath blushes a deep crimson and then", "You feel the heat of a blush under her fur before Kath")
            ));

            OutputText("Scenes.Katherine.GiveKatClothesBodysuit.GetsStartedUndresses".Localized(
                ClothesLowerChoice("run your finger inside the waist of her pants", "run your finger inside the waistband of her skirt", "", "tug gently on the straps of her dress", "run your finger along the low cut top of her robe")
            ));

            OutputText("Scenes.Katherine.GiveKatClothesBodysuit.KnewHowThin".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.GiveKatClothesBodysuit.BulgeBeginsForm".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesBodysuit.FabricHidesDetail".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.GiveKatClothesBodysuit.CockWord".Localized(
                    CockMultiple("", "s"),

                    ballSize > 3 ? " or " + BallAdj() + " balls" : ""
                ));
            else
                OutputText("Scenes.Katherine.GiveKatClothesBodysuit.PussyLipsWord".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesBodysuit.TautOutlinesDetail".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.GiveKatClothesBodysuit.TrappedBulgeGrows".Localized(
                    PregSize() > 5 ? "Common.Text.TipsClimbingTheCurveOfHer".Localized() : "Common.Text.RisingTip".Localized() + CockMultiple("", "s") + " getting nearer and nearer to her breasts"
                ));

            OutputText("Scenes.Katherine.GiveKatClothesBodysuit.OutfitLooksGreat".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesBodysuit.TailSwishesMassage".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesBodysuit.EveryoneWillSee".Localized());

            GiveClothing(KBIT_CLOTHES_BODYSUIT);

            Player.ConsumeItem(ItemRegistry.Create(ArmorIds.Bodysuit));

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), GiveKatClothesBodysuitYes);

            AddButton(1, "Common.Button.No".Localized(), GiveKatClothesBodysuitNo);

            if (Submissiveness() >= 2)
                AddButton(2, "Common.Button.Always".Localized(), GiveKatClothesBodysuitAlways);
            else
                AddButton(2, "Common.Button.Always".Localized(), null);
        }

        public void GiveKatClothesBodysuitYes()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesBodysuitYes.ExactlyWhatIWant".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesBodysuitYes.SighsExpectToSee".Localized(
                PlayerMaster() ? "Common.Text.YesMaster".Localized() : "Common.Text.Alright".Localized() + Player.ShortName
            ));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, KBIT_CLOTHES_BODYSUIT);

            KatSexMenu();
        }

        public void GiveKatClothesBodysuitNo()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesBodysuitNo.KeepAsSomethingSpecial".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesBodysuitNo.TimeToWork".Localized());

            KatSexMenu();
        }

        public void GiveKatClothesBodysuitAlways()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesBodysuitAlways.NibbleEarWriggle".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesBodysuitAlways.IncredibleBodyShowOff".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesBodysuitAlways.GropeFightingMonsters".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.GiveKatClothesBodysuitAlways.CockWord".Localized(
                    CockMultiple("", "s")
                ));
            else
                OutputText("Scenes.Katherine.GiveKatClothesBodysuitAlways.PussyWord".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesBodysuitAlways.OnDisplayWhenever".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesBodysuitAlways.PuttyInYourArms".Localized(
                PlayerText(),

                CatGirl("my skin", "me")
            ));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, -KBIT_CLOTHES_BODYSUIT);

            KatSexMenu();
        }

        public void GiveKatClothesLongDress()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesLongDress.StandBeforeMirror".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesLongDress.TakesWhileFindVoice".Localized(
                PlayerText()
            ));

            OutputText("Scenes.Katherine.GiveKatClothesLongDress.AlwaysGivingPresents".Localized(
                PlayerText()
            ));

            OutputText("Scenes.Katherine.GiveKatClothesLongDress.NeverBeenToParty".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesLongDress.TwirlsThenDance".Localized());

            GiveClothing(KBIT_CLOTHES_B_DRESS);

            Player.ConsumeItem(ItemRegistry.Create(ArmorIds.BallroomDress));

            ClearButtons();

            AddButton(0, "Common.Button.WearIt".Localized(), GiveKatClothesDressWearIt);

            AddButton(1, "Common.Button.Agree".Localized(), GiveKatClothesDressAgree);

            if (Submissiveness() >= 2)
                AddButton(2, "Common.Button.AlwaysWear".Localized(), GiveKatClothesDressAlways);
            else
                AddButton(2, "Common.Button.AlwaysWear".Localized(), null);
        }

        public void GiveKatClothesDressWearIt()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesDressWearIt.HandsOnShoulders".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesDressWearIt.BeamsAtCompliments".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, KBIT_CLOTHES_B_DRESS);

            KatSexMenu();
        }

        public void GiveKatClothesDressAgree()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesDressAgree.ProbablyRightShame".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesDressAgree.SnugglesSoundsGood".Localized());

            KatSexMenu();
        }

        public void GiveKatClothesDressAlways()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesDressAlways.HandsOnShoulders".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesDressAlways.SmilesDoYouInsist".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, -KBIT_CLOTHES_B_DRESS);

            KatSexMenu();
        }

        public void GiveKatClothesNurseOutfit()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.StandBeforeMirror".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.StudiesThreePieces".Localized(
                breasts.Cup()
            ));

            if (breasts.BreastRating < 2)
                OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.TopMakesBreastsBigger".Localized());
            else
                if (breasts.BreastRating < BreastCups.DDBig)
                    OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.TopShowsOffRack".Localized());
                else
                    OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.ButtonsHoldUp".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.PutsTinyHat".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.PicksUpMiniskirt".Localized());

            if (CanWearMiniSkirt())
                OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.SkirtJustFit".Localized());
            else
                OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.NoWayTooShort".Localized(
                    PlayerText()
                ));

            OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.StripsOffHerFragment".Localized());

            if (CanWearMiniSkirt())
                OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.PutsOnPiecesFragment".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineClothesWorn) == KBIT_CLOTHES_C_CLOTH)
                    OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.PutsOnTopHat".Localized());
                else
                    OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.FetchesSkirtWardrobe".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.VisionOfNaughtiness".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesNurseOutfit.HipsSwayTowardYou".Localized(
                CatGirl("It feels nice walking around with all this exposed skin", "I really like how the whiteness of the outfit contrasts with the blackness of my fur"),

                PlayerText()
            ));

            GiveClothing(KBIT_CLOTHES_NURSECL);

            Player.ConsumeItem(ItemRegistry.Create(ArmorIds.NurseOutfit));

            KatSexMenu();
        }

        public void GiveKatClothesSilkRobe()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesSilkRobe.StandBeforeMirror".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesSilkRobe.GawksAtRobes".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesSilkRobe.SpidersSilkStrong".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesSilkRobe.AsksAboutRunes".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesSilkRobe.ConvincesToWear".Localized(
                CatGirl("skin", "fur")
            ));

            OutputText("Scenes.Katherine.GiveKatClothesSilkRobe.CommandingPoseMirror".Localized());

            GiveClothing(KBIT_CLOTHES_SS_ROBE);

            Player.ConsumeItem(ItemRegistry.Create(ArmorIds.SS_ROBE));

            ClearButtons();

            AddButton(0, "Common.Button.WearIt".Localized(), GiveKatClothesRobeWear);

            AddButton(1, "Common.Button.DontWear".Localized(), GiveKatClothesRobeDontWear);

            if (Submissiveness() >= 2)
                AddButton(2, "Common.Button.AlwaysWear".Localized(), GiveKatClothesRobeAlways);
            else
                AddButton(2, "Common.Button.AlwaysWear".Localized(), null);
        }

        public void GiveKatClothesRobeWear()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesRobeWear.SpecialTooExotic".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesRobeWear.PleasedPurrTailRoot".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.GiveKatClothesRobeWear.CockPressAgainst".Localized(
                    CockMultiple(" begins", "s begin")
                ));
            else
                OutputText("Scenes.Katherine.GiveKatClothesRobeWear.PressCrotchAgainst".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesRobeWear.ArousedWearAroundTown".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesRobeWear.YouWinAsAlways".Localized(
                PlayerText()
            ));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, KBIT_CLOTHES_SS_ROBE);

            KatSexMenu();
        }

        public void GiveKatClothesRobeDontWear()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesRobeDontWear.OnlyWearSparingly".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesRobeDontWear.HopsWrapsLegs".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.GiveKatClothesRobeDontWear.CockPressAgainst".Localized(
                    CockMultiple(" begins", "s begin")
                ));
            else
                OutputText("Scenes.Katherine.GiveKatClothesRobeDontWear.PressCrotchAgainst".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesRobeDontWear.SentencePeriod".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesRobeDontWear.CantRepayYou".Localized());

            KatSexMenu();
        }

        public void GiveKatClothesRobeAlways()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesRobeAlways.SpecialTooExotic".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesRobeAlways.PleasedPurrTailRoot".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.GiveKatClothesRobeAlways.CockPressAgainst".Localized(
                    CockMultiple(" begins", "s begin")
                ));
            else
                OutputText("Scenes.Katherine.GiveKatClothesRobeAlways.PressCrotchAgainst".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesRobeAlways.ArousedWearAroundTown".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesRobeAlways.PurrsLikeChainsaw".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesRobeAlways.YouWinAsAlways".Localized(
                PlayerText()
            ));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineClothesPref, -KBIT_CLOTHES_SS_ROBE);

            KatSexMenu();
        }

        public void GiveKatClothesTubeTop()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatClothesTubeTop.StandBeforeMirror".Localized());

            OutputText("Scenes.Katherine.GiveKatClothesTubeTop.StretchesTopBreasts".Localized(
                breasts.Adj(),

                PlayerText()
            ));

            OutputText("Scenes.Katherine.GiveKatClothesTubeTop.HoldsShortsHips".Localized());

            if (!HasCock())
                OutputText("Scenes.Katherine.GiveKatClothesTubeTop.TightLikeGym".Localized());
            else
                if (CanWearMiniSkirt())
                    OutputText("Scenes.Katherine.GiveKatClothesTubeTop.CanFitCock".Localized(
                        CockMultiple("", "s")
                    ));
                else
                    OutputText("Scenes.Katherine.GiveKatClothesTubeTop.NoWayFitCock".Localized(
                        CockMultiple("", "s"),

                        CockMultiple("", "s")
                    ));

            OutputText("Scenes.Katherine.GiveKatClothesTubeTop.StripsOffHerFragment".Localized());

            if (CanWearMiniSkirt())
                OutputText("Scenes.Katherine.GiveKatClothesTubeTop.StrugglesTubeShorts".Localized(
                    breasts.Adj()
                ));
            else
                OutputText("Scenes.Katherine.GiveKatClothesTubeTop.ThenStrugglesTop".Localized(
                    breasts.Adj()
                ));

            OutputText("Scenes.Katherine.GiveKatClothesTubeTop.BehindRubMidriff".Localized(
                PlayerText()
            ));

            GiveClothing(KBIT_CLOTHES_TUBETOP);

            Player.ConsumeItem(ItemRegistry.Create(ArmorIds.TubeTop));

            KatSexMenu();
        }

        public void GiveKatDyeBegin(string dyeColor)
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatDyeBegin.OfferHairDye".Localized());

            OutputText("Scenes.Katherine.GiveKatDyeBegin.SurprisedTakesBottle".Localized());

            OutputText("Scenes.Katherine.GiveKatDyeBegin.DashesRinsesApplies".Localized(
                hairColor,

                dyeColor
            ));

            hairColor = dyeColor;
        }

        public void GiveKatDyeEnd()
        {

            OutputText("Scenes.Katherine.GiveKatDyeEnd.SpinsHugsThankYou".Localized(
                PlayerText()
            ));

            OutputText("Scenes.Katherine.GiveKatDyeEnd.AssureShowsAppreciation".Localized());

            KatSexMenu();
        }

        public void GiveKatDyeAuburn()
        {

            GiveKatDyeBegin("rich auburn");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeAuburn));

            OutputText("Scenes.Katherine.GiveKatDyeAuburn.MirrorCombingWays".Localized(
                CatGirl("skin", "fur")
            ));

            GiveKatDyeEnd();
        }

        public void GiveKatDyeBlack()
        {

            GiveKatDyeBegin("jet black");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeBlack));

            OutputText("Scenes.Katherine.GiveKatDyeBlack.MatchingFurFragment".Localized());

            if (furry && !noFur)
            {
                OutputText("Scenes.Katherine.GiveKatDyeBlack.CoversRestBody".Localized());

                OutputText("Scenes.Katherine.GiveKatDyeBlack.MirrorHandsOnHips".Localized());

                OutputText("Scenes.Katherine.GiveKatDyeBlack.StepBehindArmsWaist".Localized());

                if (HasCock())
                {
                    OutputText("Scenes.Katherine.GiveKatDyeBlack.RubThighsCockAttention".Localized(
                        CockMultiple(" stands", "s stand")
                    ));

                    OutputText("Scenes.Katherine.GiveKatDyeBlack.GrindsAssPurrs".Localized());
                }
                else
                    OutputText("Scenes.Katherine.GiveKatDyeBlack.SinksBackLovesYou".Localized(
                        PlayerText()
                    ));

                KatSexMenu();
            }
            else
            {
                OutputText("Scenes.Katherine.GiveKatDyeBlack.OnEarsAndTail".Localized());

                OutputText("Scenes.Katherine.GiveKatDyeBlack.MirrorFingersHair".Localized());

                GiveKatDyeEnd();
            }
        }

        public void GiveKatDyeBlonde()
        {

            GiveKatDyeBegin("light blonde");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeBlond));

            OutputText("Scenes.Katherine.GiveKatDyeBlonde.MirrorCombingWays".Localized(
                CatGirl("Now I've got [if (!nofur) {blonde hair and no fur, except on my ears and tail|that, and a lot more}]. They're the ones who ought to be jealous now", "I think it looks good on me, especially since there's such a contrast with my fur")
            ));

            GiveKatDyeEnd();
        }

        public void GiveKatDyeBlue()
        {

            GiveKatDyeBegin("bright blue");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeBlue));

            OutputText("Scenes.Katherine.GiveKatDyeBlue.MirrorRunsFingers".Localized());

            GiveKatDyeEnd();
        }

        public void GiveKatDyeBrown()
        {

            GiveKatDyeBegin("chestnut brown");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeBrown));

            OutputText("Scenes.Katherine.GiveKatDyeBrown.MirrorCombingWays".Localized(
                CatGirl("skin", "fur")
            ));

            GiveKatDyeEnd();
        }

        public void GiveKatDyeGray()
        {

            GiveKatDyeBegin("cloud gray");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeGray));

            OutputText("Scenes.Katherine.GiveKatDyeGray.MirrorCombingWays".Localized(
                CatGirl("skin", "fur")
            ));

            GiveKatDyeEnd();
        }

        public void GiveKatDyeGreen()
        {

            GiveKatDyeBegin("lime green");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeGreen));

            OutputText("Scenes.Katherine.GiveKatDyeGreen.MirrorRunsFingers".Localized(
                CatGirl("-girl", "")
            ));

            GiveKatDyeEnd();
        }

        public void GiveKatDyeOrange()
        {

            GiveKatDyeBegin("vibrant orange");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeOrange));

            OutputText("Scenes.Katherine.GiveKatDyeOrange.MirrorRunsFingers".Localized());

            GiveKatDyeEnd();
        }

        public void GiveKatDyePink()
        {

            GiveKatDyeBegin("neon pink");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyePink));

            OutputText("Scenes.Katherine.GiveKatDyePink.MirrorBackToOld".Localized());

            GiveKatDyeEnd();
        }

        public void GiveKatDyePurple()
        {

            GiveKatDyeBegin("deep purple");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyePurple));

            OutputText("Scenes.Katherine.GiveKatDyePurple.MirrorCombingWays".Localized(
                CatGirl("I guess it's the opposite of the [if (!nofur) {black fur and }]bright pink hair I used to have.[if (!nofur) { Now I've got dark purple hair and light skin}]", "My fur is so dark the purple still stands out. Not many girls could say that")
            ));

            GiveKatDyeEnd();
        }

        public void GiveKatDyeRainbow()
        {

            GiveKatDyeBegin("rainbow colored");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeRainbow));

            OutputText("Scenes.Katherine.GiveKatDyeRainbow.MirrorCombingWays".Localized());

            GiveKatDyeEnd();
        }

        public void GiveKatDyeRed()
        {

            GiveKatDyeBegin("flaming red");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeRed));

            OutputText("Scenes.Katherine.GiveKatDyeRed.MirrorStartsPose".Localized());

            GiveKatDyeEnd();
        }

        public void GiveKatDyeRusset()
        {

            GiveKatDyeBegin("russet red");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeRusset));

            OutputText("Scenes.Katherine.GiveKatDyeRusset.MirrorCombingWays".Localized());

            GiveKatDyeEnd();
        }

        public void GiveKatDyeYellow()
        {

            GiveKatDyeBegin("sunflower yellow");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeYellow));

            OutputText("Scenes.Katherine.GiveKatDyeYellow.MirrorRunsFingers".Localized());

            GiveKatDyeEnd();
        }

        public void GiveKatDyeWhite()
        {

            GiveKatDyeBegin("snow white");

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeWhite));

            OutputText("Scenes.Katherine.GiveKatDyeWhite.MirrorCombingWays".Localized(
                CatGirl("Now I've got snow white hair[if (nofur) {--t| and only a little bit of black fur on my ears and tail. T}]hey're the ones who ought to be jealous. In this desert heat, I win!", "I think it looks good on me, but the contrast is something else. It almost hurts my eyes looking at my shoulders, and it's going to be even worse in sunlight. No one is going to walk into me now.")
            ));

            GiveKatDyeEnd();
        }

        public void KatherineSex()
        {
            ClearOutput();

            if (SceneInstance<UrtaScene>.Get().drainedByKath)
                OutputText("Scenes.Katherine.KatherineSex.FewThingsToDo".Localized());
            else
                if (IsAt(KLOC_BAR))
                {
                    OutputText("Scenes.Katherine.KatherineSex.AskInTheMood".Localized());

                    OutputText("Scenes.Katherine.KatherineSex.WhateverCouldYouMean".Localized(
                        PlayerText()
                    ));

                    OutputText("Scenes.Katherine.KatherineSex.AlleywayBehindBar".Localized());
                }
                else
                {
                    OutputText("Scenes.Katherine.KatherineSex.AskInMoodSex".Localized());

                    OutputText("Scenes.Katherine.KatherineSex.StartledThenRallies".Localized());
                }

            KatSexMenu();
        }

        public void KatSexMenu()
        {

            KatherineSprite(true);

            ClearButtons();

            AddButton(0, "Common.Button.Penetration".Localized(), null);

            AddButton(5, "Common.Button.GetPenetrated".Localized(), null);

            AddButton(6, "Common.Button.Doublehelix".Localized(), null);

            AddButton(7, "Common.Button.Suckle".Localized(), null);

            AddButton(1, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), OralKatherineChoices);

            AddButton(2, "Common.Button.Handjob".Localized(), HandjobbiesFurrDemCatFurries);

            if (Player.Lust >= 33)
            {
                if ((Player.HasCock()) && (Player.HasCockThatFits(70)))
                        AddButton(0, "Common.Button.Penetration".Localized(), KatPenetrate);

                if (HasCock())
                {
                    AddButton(5, "Common.Button.GetPenetrated".Localized(), LetKatKnotYou);

                    if (Player.HasVagina() && Player.HasCockThatFits(70))
                        AddButton(6, "Common.Button.Doublehelix".Localized(), KatDoubleHelixCraziness);
                }
            }

            if (Player.LactationQ() > 0 && Player.BiggestLactation() >= 1 && Player.BiggestBreastSize() >= 1)
            {
                if (DoneSubmissive(KBIT_SUB_START_LACTATION))
                    AddButton(7, "Common.Button.Suckle".Localized(), SuckleMenu);
                else
                    AddButton(7, "Common.Button.Suckle".Localized(), SuckleTacularKats);
            }
            else
                if (DoneSubmissive(KBIT_SUB_START_LACTATION))
                    AddButton(7, "Common.Button.Suckle".Localized(), SuckleFromKath);

            if (IsAt(KLOC_KATHS_APT))
            {
                if (HasAlready(KBIT_TRIED_BEDROOM_BONDAGE) || Player.Gems >= 40)
                    AddButton(2, "Common.Button.BedBondage".Localized(), BedroomBondage);
                else
                    AddButton(2, "Common.Button.BedBondage".Localized(), null);

                AddButton(14, "Common.Button.Back".Localized(), KatherineMenu);
            }
            else
            {
                AddButton(14, "Common.Button.Back".Localized(), KatherineMenu);

                if (IsAt(KLOC_BAR))
                {
                    if (!GameSettings.UrtaDisabled)
                        if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) > 10 && SceneInstance<UrtaScene>.Get().UrtaAtBar() && SceneInstance<UrtaScene>.Get().UrtaAvailableForSex())
                            AddButton(3, "Common.Button.SeeUrta".Localized(), KatherineSeeUrta);
                        else
                            AddButton(3, "Common.Button.SeeUrta".Localized(), null);

                    if (CoC.Instance.Flags.Get(GameFlagIds.KatherineValaAffection) > 10 && IsValaAtBar())
                        AddButton(4, "Common.Button.SeeVala".Localized(), KatherineSeeVala);
                    else
                        AddButton(4, "Common.Button.SeeVala".Localized(), null);
                }
                else
                    if (IsAt(KLOC_BAR_DRUNK))
                    {
                        AddButton(7, "Common.Button.Suckle".Localized(), null);

                        if (!GameSettings.UrtaDisabled)
                            if (SceneInstance<UrtaScene>.Get().UrtaAtBar() && SceneInstance<UrtaScene>.Get().UrtaAvailableForSex() && !SceneInstance<UrtaScene>.Get().drainedByKath)
                                AddButton(3, "Common.Button.SeeUrta".Localized(), KatherineDrunkSeeUrta);
                            else
                                AddButton(3, "Common.Button.SeeUrta".Localized(), null);

                        if (IsValaAtBar() && !SceneInstance<UrtaScene>.Get().drainedByKath)
                            AddButton(4, "Common.Button.SeeVala".Localized(), KatherineSeeVala);
                        else
                            AddButton(4, "Common.Button.SeeVala".Localized(), null);

                        AddButton(14, "Common.Button.Back".Localized(), null);
                    }
                    else
                        if (IsAt(KLOC_BAR_URTA_REFUSED))
                        {
                            AddButton(3, "Common.Button.SeeUrta".Localized(), null);

                            if (IsValaAtBar())
                                AddButton(4, "Common.Button.SeeVala".Localized(), KatherineSeeVala);
                            else
                                AddButton(4, "Common.Button.SeeVala".Localized(), null);

                            AddButton(14, "Common.Button.Back".Localized(), null);
                        }
            }
        }

        public void KatherineDrunkSeeUrta()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.SuggestTalkUrta".Localized());

            OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.CaptainNeedsCompany".Localized());

            OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.HelpToUrtaTable".Localized());

            if (SceneInstance<UrtaScene>.Get().UrtaDrunk())
            {
                OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.UrtaHugsCutie".Localized());

                OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.PurrsBuriesHead".Localized());

                OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.FeelLikeIntervene".Localized());

                ClearButtons();

                if (Submissiveness() >= 3)
                {
                    AddButton(0, "Common.Button.SceneinstanceKatherinethreesomesceneGetOrgy".Localized(), SceneInstance<KatherineThreesomeScene>.Get().Orgy);

                    OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.EveryoneEnjoysOrgy".Localized());

                    OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.YouFragment".Localized());
                }
                else
                {
                    AddButton(0, "Common.Button.SceneinstanceKatherinethreesomesceneGetOrgy".Localized(), null);

                    OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.OtherwiseYouFragment".Localized());
                }

                if (Player.IsGenderless())
                {
                    OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.CouldLetThemFuck".Localized());

                    AddButton(1, "Common.Button.SceneinstanceKatherinethreesomesceneGetWatch".Localized(), () => SceneInstance<KatherineThreesomeScene>.Get().WatchNoIntro(true));
                }
                else
                {
                    OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.LetThemFuckAgain".Localized());

                    AddButton(1, "Common.Button.LetEmFuck".Localized(), SceneInstance<KatherineThreesomeScene>.Get().DoubleStuffKath);

                    if (Player.HasCock())
                        AddButton(2, "Common.Button.DblPenKath".Localized(), SceneInstance<KatherineThreesomeScene>.Get().DoublePenetrateKath);
                    else
                        AddButton(2, "Common.Button.DblPenKath".Localized(), null);
                }
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) > 10)
                {
                    OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.UrtaAsksTrouble".Localized());

                    OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.WinksBestTrouble".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.LickOut".Localized(), SceneInstance<KatherineThreesomeScene>.Get().KathLicksOutUrta);

                    if (HasCock())
                        AddButton(1, "Common.Button.SceneinstanceKatherinethreesomesceneGetSandwich".Localized(), SceneInstance<KatherineThreesomeScene>.Get().Sandwich);
                    else
                        AddButton(1, "Common.Button.SceneinstanceKatherinethreesomesceneGetSandwich".Localized(), null);

                    if (!Player.IsGenderless() && HasCock())
                        AddButton(2, "Common.Button.SceneinstanceKatherinethreesomesceneGetKnothole".Localized(), SceneInstance<KatherineThreesomeScene>.Get().Knothole);
                    else
                        AddButton(2, "Common.Button.SceneinstanceKatherinethreesomesceneGetKnothole".Localized(), null);
                }
                else
                {
                    OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.UrtaPushesBack".Localized());

                    OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.EyesWanderChest".Localized());

                    OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.UrtaWhispersDoSomething".Localized());

                    OutputText("Scenes.Katherine.KatherineDrunkSeeUrta.PropsHeadDreamily".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KLOC_BAR_URTA_REFUSED);

                    KatSexMenu();
                }
        }

        public void KatherineSeeUrta()
        {
            ClearOutput();

            if (SceneInstance<UrtaScene>.Get().UrtaDrunk())
            {
                OutputText("Scenes.Katherine.KatherineSeeUrta.SeesCaptainSloshed".Localized(
                    PlayerText()
                ));

                OutputText("Scenes.Katherine.KatherineSeeUrta.OnlyIfAskNicely".Localized());

                OutputText("Scenes.Katherine.KatherineSeeUrta.StandsOffersHand".Localized());

                OutputText("Scenes.Katherine.KatherineSeeUrta.UrtaLooksUpCock".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.SceneinstanceKatherinethreesomesceneGetWatch".Localized(), () => SceneInstance<KatherineThreesomeScene>.Get().Watch(true));

                if (HasCock())
                    AddButton(1, "Common.Button.PinFuck".Localized(), SceneInstance<KatherineThreesomeScene>.Get().PinAndFuck);
                else
                    AddButton(1, "Common.Button.PinFuck".Localized(), null);
            }
            else
            {
                OutputText("Scenes.Katherine.KatherineSeeUrta.UrtaGesturesBoth".Localized());

                OutputText("Scenes.Katherine.KatherineSeeUrta.OffersHandBetterGet".Localized());

                OutputText("Scenes.Katherine.KatherineSeeUrta.FeelsWarmthProjecting".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.RoastYou".Localized(), null);

                AddButton(1, "Common.Button.RoastKath".Localized(), null);

                AddButton(2, "Common.Button.Circlejeck".Localized(), null);

                AddButton(3, "369", null);

                if (HasCock())
                    AddButton(0, "Common.Button.RoastYou".Localized(), SceneInstance<KatherineThreesomeScene>.Get().RoastYou);

                if (!Player.IsGenderless())
                {
                    AddButton(1, "Common.Button.RoastKath".Localized(), SceneInstance<KatherineThreesomeScene>.Get().SpitroastKath);

                    AddButton(2, "Common.Button.Circlejeck".Localized(), SceneInstance<KatherineThreesomeScene>.Get().Circlejerk);

                    if (Player.HasCock())
                        AddButton(3, "369", SceneInstance<KatherineThreesomeScene>.Get().ThreeSixtyNine);
                }
                else
                    if (!HasCock())
                        AddButton(4, "Common.Button.SceneinstanceKatherinethreesomesceneGetWatch".Localized(), () => SceneInstance<KatherineThreesomeScene>.Get().Watch(false));
            }
        }

        public void KatherineSeeVala()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.KatherineSeeVala.ReadyButCatchesEye".Localized());

            OutputText("Scenes.Katherine.KatherineSeeVala.ValaFliesOverSits".Localized(
                CatGirl("girl", "morph")
            ));

            ClearButtons();

            AddButton(1, "Common.Button.EatOut".Localized(), null);

            AddButton(2, "Common.Button.DblStuff".Localized(), null);

            AddButton(0, "Common.Button.FistThem".Localized(), SceneInstance<KatherineThreesomeScene>.Get().FistKathAndVala);

            if (HasCock())
            {
                AddButton(1, "Common.Button.EatOut".Localized(), SceneInstance<KatherineThreesomeScene>.Get().EatOutVala);

                if (!Player.IsTaur() && Player.HasCock())
                    AddButton(2, "Common.Button.DblStuff".Localized(), SceneInstance<KatherineThreesomeScene>.Get().DoubleStuffVala);
            }
        }

        public void KatPenetrate()
        {
            ClearOutput();

            if (IsAt(KLOC_BAR) || IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
            {
                OutputText("Scenes.Katherine.KatPenetrate.TailTwitchesFinishesDrink".Localized());

                OutputText("Scenes.Katherine.KatPenetrate.LeadToAlleyway".Localized());

                OutputText("Scenes.Katherine.KatPenetrate.WorriedGettingCaught".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KLOC_STREETS);
            }

            OutputText("Scenes.Katherine.KatPenetrate.SuggestPenetrateHole".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Vagina".Localized(), PenetrateKatsVag);

            AddButton(1, "Common.Button.Anus".Localized(), PcPenetratesKatAnally);

            if (Player.CockThatFits2(70) >= 0)
                AddButton(2, "Scenes.Masturbation.CentaurHugeCock.BothWord".Localized(), PcPenetratesKatDoubly);
            else
                AddButton(2, "Scenes.Masturbation.CentaurHugeCock.BothWord".Localized(), null);

            if (!HasCock() || knotSize > 4)
                AddButton(3, "Common.Button.Sucknfuck".Localized(), null);
            else
                AddButton(3, "Common.Button.Sucknfuck".Localized(), SuckNFuck);

            AddButton(14, "Common.Button.Back".Localized(), KatSexMenu);
        }

        public void PenetrateKatsVag()
        {
            ClearOutput();

            int x = Player.CockThatFits(70);

            ShowImage("katherine-fuck-her-vagoo");

            OutputText("Scenes.Katherine.PenetrateKatsVag.EyesLockedOnHer".Localized());

            OutputText("Scenes.Katherine.PenetrateKatsVag.PuddlingOntoGround".Localized(
                HasBalls() ? BallAdj() + " scrotum" : CatGirl("milky white", "black, furry") + " thighs"
            ));

            if (IsAt(KLOC_STREETS))
                OutputText("Scenes.Katherine.PenetrateKatsVag.OverBarrel".Localized());
            else
                if (IsAt(KLOC_KATHS_APT))
                    OutputText("Scenes.Katherine.PenetrateKatsVag.EdgeOfBed".Localized());
                else
                    OutputText("Scenes.Katherine.PenetrateKatsVag.OnAllFours".Localized());

            OutputText("Scenes.Katherine.PenetrateKatsVag.EasierToSupport".Localized());

            OutputText("Scenes.Katherine.PenetrateKatsVag.SlideHomeYowl".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Katherine.PenetrateKatsVag.BallsWord".Localized());
            else
                OutputText("Scenes.Katherine.PenetrateKatsVag.HerBalls".Localized());

            OutputText("Scenes.Katherine.PenetrateKatsVag.SentencePeriodBreak".Localized());

            OutputText("Scenes.Katherine.PenetrateKatsVag.YowlsBabbling".Localized(
                CatGirl("supple skin", "silken fur"),

                HasCock() ? "You can just make out the way her cock" + CockMultiple(" flails", "s flail") + " around, stiff as iron with knot" + CockMultiple("", "s") + " swollen to full size - she" : "She"
            ));

            OutputText("Scenes.Katherine.PenetrateKatsVag.ArchingBack".Localized(
                HasCock() ? "Cum gushes like a river from her cock" + CockMultiple("", "s") + " and her" : "Her"
            ));

            if (Player.HasVagina())
                OutputText("Scenes.Katherine.PenetrateKatsVag.ReleasesGirlCum".Localized());
            else
                OutputText("Scenes.Katherine.PenetrateKatsVag.YLetter".Localized());

            OutputText("Scenes.Katherine.PenetrateKatsVag.CockDischargesDepths".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Katherine.PenetrateKatsVag.BellyPuffsOut".Localized());

            OutputText("Scenes.Katherine.PenetrateKatsVag.GaspingPullOut".Localized());

            OutputText("Scenes.Katherine.PenetrateKatsVag.KatherineName".Localized());

            if (IsAt(KLOC_STREETS))
                OutputText("Scenes.Katherine.PenetrateKatsVag.SprawlsOnBarrel".Localized());
            else
                if (IsAt(KLOC_KATHS_APT))
                    OutputText("Scenes.Katherine.PenetrateKatsVag.SprawlsOnBed".Localized());
                else
                    OutputText("Scenes.Katherine.PenetrateKatsVag.CollapsesOnGrass".Localized());

            OutputText("Scenes.Katherine.PenetrateKatsVag.FlicksTailPurring".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Katherine.PenetrateKatsVag.HonestlyNoChoice".Localized());

            OutputText("Scenes.Katherine.PenetrateKatsVag.LittleBoysGirlsHerms".Localized());

            OutputText("Scenes.Katherine.PenetrateKatsVag.ScratchBehindEars".Localized());

            if (IsAt(KLOC_STREETS))
                OutputText("Scenes.Katherine.PenetrateKatsVag.CleansWithRags".Localized());
            else
                if (IsAt(KLOC_KATHS_APT))
                    OutputText("Scenes.Katherine.PenetrateKatsVag.WashInBasin".Localized());
                else
                {
                    OutputText("Scenes.Katherine.PenetrateKatsVag.LakeWaterTowel".Localized());

                    if (Player.CumQ() >= 1500)
                        OutputText("Scenes.Katherine.PenetrateKatsVag.RubBloatedBelly".Localized());
                }

            Player.Orgasm("Dick");

            Orgasm();

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            ReturnToCamp(1);
        }

        public void PcPenetratesKatAnally()
        {
            ClearOutput();

            int x = Player.CockThatFits(70);

            ShowImage("katherine-fuck-her-bungholio");

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.GazeFallsOnPucker".Localized(
                CatGirl("inviting", "ebony")
            ));

            if (HasAlready(KBIT_TRIED_RECEIVE_ANAL))
                OutputText("Scenes.Katherine.PcPenetratesKatAnally.KathSmirksTailRaises".Localized());
            else
                OutputText("Scenes.Katherine.PcPenetratesKatAnally.EyesWidenSwallows".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.TurnsHeadBack".Localized());

            if (IsAt(KLOC_STREETS))
                OutputText("Scenes.Katherine.PcPenetratesKatAnally.SupportsOnCrate".Localized());
            else
                if (IsAt(KLOC_KATHS_APT))
                    OutputText("Scenes.Katherine.PcPenetratesKatAnally.HipsAgainstBedframe".Localized());
                else
                    OutputText("Scenes.Katherine.PcPenetratesKatAnally.HeadDownAssRaised".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.SaunterUpBehind".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) < 4)
                OutputText("Scenes.Katherine.PcPenetratesKatAnally.SurprisinglyGoodShape".Localized());
            else
                OutputText("Scenes.Katherine.PcPenetratesKatAnally.WithInvitingPaleFlesh".Localized(
                    CatGirl("with inviting pale flesh", "in luxurious-looking fur")
                ));

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.HandsAppreciatively".Localized(
                CatGirl("over that smooth bottom", "through the soft hair"),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.InsistsTryingRelax".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.PressesForward".Localized(
                CatGirl("", " black"),

                HasCock() ? CockAdj() + CockType(" dog-cock", " kitty-cock") + CockMultiple("", "s") + " visibly jolting and her " : ""
            ));

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.GaspsThrustsBack".Localized(
                PlayerText()
            ));

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.GruntAndHiss".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.ExcitedYowl".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.PcPenetratesKatAnally.SqueezingProstate".Localized(
                    CockMultiple("", "s"),

                    CockMultiple("", "s")
                ));
            else
                OutputText("Scenes.Katherine.PcPenetratesKatAnally.SoGoodBackThere".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.GonnaBlowAnySecond".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.BeatHerToPunch".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Katherine.PcPenetratesKatAnally.BellyBeginsBulge".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Katherine.PcPenetratesKatAnally.SwellsLikePregnant".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.HerOrgasmUnnoticed".Localized());

            if (IsAt(KLOC_STREETS))
                OutputText("Scenes.Katherine.PcPenetratesKatAnally.FemcumRainsDown".Localized(
                    HasCock() ? " and her " + CockMultiple("cock sprays", "cocks spray") + " cum all along the crate and ground" : ""
                ));
            else
                if (IsAt(KLOC_KATHS_APT))
                    OutputText("Scenes.Katherine.PcPenetratesKatAnally.FemcumSheetsBed".Localized(
                        HasCock() ? "mixing with the sprays of semen from her cock" + CockMultiple("", "s") + ", soaking her belly," : "soaking her"
                    ));
                else
                    OutputText("Scenes.Katherine.PcPenetratesKatAnally.FemcumRainsStream".Localized(
                        HasCock() ? " and her " + CockMultiple("cock sprays", "cocks spray") + " cum all over the grass" : ""
                    ));

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.SpentPullsOut".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.SlinksBonelessly".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Katherine.PcPenetratesKatAnally.CradlesDistendedBelly".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.SentencePeriod".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Katherine.PcPenetratesKatAnally.LooksAtGut".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatAnally.SmirkAtFlattery".Localized());

            if (IsAt(KLOC_STREETS))
                OutputText("Scenes.Katherine.PcPenetratesKatAnally.HelpsHerDress".Localized());
            else
                if (IsAt(KLOC_KATHS_APT))
                {
                    if (Player.Corruption < 25)
                        OutputText("Scenes.Katherine.PcPenetratesKatAnally.ChangesCumSoakedSheets".Localized());
                    else
                        if (Player.Corruption < 75)
                            OutputText("Scenes.Katherine.PcPenetratesKatAnally.KissHopeSeeSoon".Localized());
                        else
                            OutputText("Scenes.Katherine.PcPenetratesKatAnally.LickUpSprayedCum".Localized(
                                CatGirl("her skin soft and smooth", "fur nice and shiny")
                            ));
                }
                else
                {
                    OutputText("Scenes.Katherine.PcPenetratesKatAnally.HelpsHerToFeet".Localized());

                    if (Player.CumQ() >= 1500)
                        OutputText("Scenes.Katherine.PcPenetratesKatAnally.LongerForKatherine".Localized());

                    OutputText("Scenes.Katherine.PcPenetratesKatAnally.SentencePeriodAgain".Localized());
                }

            Player.Orgasm("Dick");

            Orgasm();

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Experience(KBIT_TRIED_RECEIVE_ANAL);

            ReturnToCamp(1);
        }

        public void PcPenetratesKatDoubly()
        {
            ClearOutput();

            int x = Player.CockThatFits(70);

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.UnfastensArmor".Localized());

            if (IsAt(KLOC_KATHS_APT))
                OutputText("Scenes.Katherine.PcPenetratesKatDoubly.TossClothesLiftHer".Localized());
            else
            {
                if (View.Time.Hours < 12)
                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.MorningBreeze".Localized());
                else
                    if (View.Time.Hours < 18)
                        OutputText("Scenes.Katherine.PcPenetratesKatDoubly.WarmAfternoonAir".Localized());
                    else
                        OutputText("Scenes.Katherine.PcPenetratesKatDoubly.CoolEveningAir".Localized());

                if (IsAt(KLOC_LAKE))
                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.Lakeshore".Localized());
            }

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.TwoDelectableHoles".Localized(
                CatGirl("soft skin", "fur")
            ));

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.EyesWideThighsOpen".Localized(
                HasCock() ? "Her cock" + CockMultiple(" strains", "s strain") + " against her " + ClothesLowerChoice("pants", "skirt", "bodysuit", "dress", "robe") + ". You" : "You tease her by rubbing them gently, then you"
            ));

            if (IsAt(KLOC_STREETS))
                OutputText("Scenes.Katherine.PcPenetratesKatDoubly.SoftDirtAlley".Localized());
            else
                if (IsAt(KLOC_KATHS_APT))
                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.OnTopHerBed".Localized());
                else
                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.SandWatersEdge".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.PresentsPussyAss".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.StrokesKatherineTail".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.KathHands".Localized());

            if (IsAt(KLOC_STREETS))
                OutputText("Scenes.Katherine.PcPenetratesKatDoubly.DigsIntoDirt".Localized());
            else
                if (IsAt(KLOC_KATHS_APT))
                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.TightensBunchesSheets".Localized());
                else
                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.DigsIntoSand".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.LowFeralMeow".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.CocksSeatShePushes".Localized(
                CatGirl("smooth", "fuzzy")
            ));

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.PlentyCreamKitten".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.YouWord".Localized());

            if (Player.Corruption < 25)
            {
                OutputText("Scenes.Katherine.PcPenetratesKatDoubly.FightsBaseUrges".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoubly.TinyStrokesDeeper".Localized());
            }
            else
                if (Player.Corruption < 75)
                {
                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.BothCockheadsInside".Localized());

                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.KathWantsFaster".Localized());
                }
                else
                {
                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.GripSolidLinedUp".Localized());

                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.NoChanceRecover".Localized());
                }

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.AboutToAttention".Localized(
                HasCock() ? "reach down and give Kath's cock" + CockMultiple("", "s") : "give Kath's nipples"
            ));

            if (HasCock())
                OutputText("Scenes.Katherine.PcPenetratesKatDoubly.FeelsHerProstate".Localized());
            else
                OutputText("Scenes.Katherine.PcPenetratesKatDoubly.HolesCrushCocks".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.BodyGoesLimp".Localized());

            if (HasCock())
            {
                if (IsAt(KLOC_STREETS))
                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.StickyMudPuddle".Localized());
                else
                    if (IsAt(KLOC_KATHS_APT))
                        OutputText("Scenes.Katherine.PcPenetratesKatDoubly.BedSheetsSoaked".Localized());
                    else
                        OutputText("Scenes.Katherine.PcPenetratesKatDoubly.SemenSlidesLake".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoubly.NoSenseStopping".Localized(
                    CatGirl("pet cat girl", "feline friend")
                ));
            }

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.PoundsSlackBody".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.ThrustsToEdge".Localized());

            var wombShot = Player.Cocks[x].Length > 14 && PregSize() == 0;

            if (wombShot)
            {
                var cockLength = Math.Round(Player.Cocks[x].Length);

                OutputText("Scenes.Katherine.PcPenetratesKatDoubly.TightnessAtTip".Localized(
                    cockLength <= 16 ? "Common.Text.YourCockheadIs".Localized() : "Common.Text.TheFirst".Localized() + (cockLength - 14) + " inches of your cock are"
                ));
            }

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.FirstOfManySalvos".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.FiresIntoBothHoles".Localized());

            if (Player.Cocks.Count > 2)
                OutputText("Scenes.Katherine.PcPenetratesKatDoubly.OtherCockMess".Localized(
                    HasCock() ? (Player.Cocks.Count > 3 ? "Common.Text.Add".Localized() : "Common.Text.Adds".Localized()) + " to the" : "create a"
                ));

            if (Player.CumQ() < 500)
                OutputText("Scenes.Katherine.PcPenetratesKatDoubly.EruptionNoSigns".Localized());
            else
                if (Player.CumQ() < 1500)
                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.EruptionBellyBulges".Localized());
                else
                {
                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.FightingResistance".Localized());

                    if (IsAt(KLOC_STREETS))
                        OutputText("Scenes.Katherine.PcPenetratesKatDoubly.MudWord".Localized());
                    else
                        if (IsAt(KLOC_KATHS_APT))
                            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.CumSoakedSheets".Localized());
                        else
                            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.BeachSandWord".Localized());

                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.LooksPregnantTwins".Localized());

                    if (Player.CumQ() >= 3000)
                    {
                        OutputText("Scenes.Katherine.PcPenetratesKatDoubly.ErectionsCantHold".Localized(
                            CatGirl("you can see stretch marks developing", "that her belly fur begins to lighten. Looking closely you can see that each hair is being forced to stand on end")
                        ));

                        OutputText("Scenes.Katherine.PcPenetratesKatDoubly.FirstTricklesBack".Localized());
                    }

                    OutputText("Scenes.Katherine.PcPenetratesKatDoubly.TorrentStops".Localized());

                    if (wombShot)
                    {
                        if (DoneSubmissive(KBIT_SUB_HIGH_CUM))
                        {
                            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.RecallsFunWayHelp".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.Squeeze".Localized(), PcPenetratesKatDoublyHighCumCorrupt);

                            AddButton(1, "Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), PcPenetratesKatDoublyNormalEnd);

                            return ;
                        }

                        if (Player.IsCorruptEnough(75) || (Player.HasPerk(PerkIds.Pervert) && Player.Corruption >= 33) || Player.HasPerk(PerkIds.Sadist))
                        {
                            OutputText("Scenes.Katherine.PcPenetratesKatDoubly.PerverseMindHelp".Localized());

                            ClearButtons();

                            AddButton(0, "Scenes.Faerie.GladesScene.ButtonHelp".Localized(), PcPenetratesKatDoublyHighCumCorrupt);

                            AddButton(1, "Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), PcPenetratesKatDoublyNormalEnd);

                            return ;
                        }
                    }

                    if (Player.Corruption >= 25)
                        OutputText("Scenes.Katherine.PcPenetratesKatDoubly.RubsBackUntilBurp".Localized());
                    else
                        OutputText("Scenes.Katherine.PcPenetratesKatDoubly.MassagesSidesBelch".Localized());
                }

            PcPenetratesKatDoublyNormalEnd();
        }

        public void PcPenetratesKatDoublyNormalEnd()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.PcPenetratesKatDoublyNormalEnd.NotDoneYet".Localized(
                HasCock() ? "take hold of " + CockMultiple("her flaccid cock", "both her flaccid cocks") : "firmly grip her clit"
            ));

            OutputText("Scenes.Katherine.PcPenetratesKatDoublyNormalEnd.SaysDreamily".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatDoublyNormalEnd.BeginWorkHer".Localized(
                HasCock() ? "softening shaft" + CockMultiple("", "s") : "nub back and forth"
            ));

            OutputText("Scenes.Katherine.PcPenetratesKatDoublyNormalEnd.KeepsFaster".Localized(
                HasCock() ? "stroking her cock" + CockMultiple(",", "s,") : "playing with her clit, rolling it around ",

                HasCock() ? "Her prick" + CockMultiple(" grows", "s grow") + " hard and hot in your hands" : "You feel the heat developing within her pussy and her ass"
            ));

            OutputText("Scenes.Katherine.PcPenetratesKatDoublyNormalEnd.MinutesFuriousStroking".Localized(
                HasCock() ? "trying to bury her cock" + CockMultiple(" in an imaginary pussy", "s in a pair of imaginary pussies") + " but succeeding only in " : ""
            ));

            OutputText("Scenes.Katherine.PcPenetratesKatDoublyNormalEnd.LandsInPuddle".Localized(
                HasCock() ? " in almost every way possible. Her cock" + CockMultiple(" blasts a stream", "s blast twin streams") + " into the air while" : ","
            ));

            if (IsAt(KLOC_STREETS))
                OutputText("Scenes.Katherine.PcPenetratesKatDoublyNormalEnd.FallsToMud".Localized());
            else
                if (IsAt(KLOC_KATHS_APT))
                    OutputText("Scenes.Katherine.PcPenetratesKatDoublyNormalEnd.CollapsesOntoBed".Localized());
                else
                    OutputText("Scenes.Katherine.PcPenetratesKatDoublyNormalEnd.WashesInStream".Localized());

            Player.Orgasm("Dick");

            Orgasm();

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            ReturnToCamp(1);
        }

        public void PcPenetratesKatDoublyHighCumCorrupt()
        {
            ClearOutput();

            var firstTime = !DoneSubmissive(KBIT_SUB_HIGH_CUM);

            if (firstTime)
            {
                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.GripsBellySqueezes".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.EnjoysDiscomfort".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.PressureLessensCough".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.ShallowBreaths".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.RubsSidesTells".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.DislikesTaste".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.ThisDidPleaseYou".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.PleaseDontBeg".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.LessPissedNow".Localized(
                    CatGirl("over her silky skin", "through her soft fur")
                ));

                AddSubmissive(KBIT_SUB_HIGH_CUM);
            }
            else
            {
                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.SqueezesPressureUp".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.PressureLessensDrool".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.LungfulSqueezedAgain".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.StopsWhenLeaking".Localized(
                    PlayerText()
                ));

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.RubsTellsAgain".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.AlreadySoFull".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.TracesFingersSeductively".Localized(
                    CatGirl("over her silky skin", "through her soft fur")
                ));
            }

            OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.CocksGoSoft".Localized());

            if (Player.Corruption < 25)
                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.MateWord".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.PartnerWord".Localized());
                else
                    OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.CumDumpWord".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.ToRecover".Localized());

            if (IsAt(KLOC_LAKE))
            {
                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.TakesFarLonger".Localized());

                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.WashesOffStream".Localized());
            }
            else
                OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.RubberyLegs".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.ArmsAroundHer".Localized());

            OutputText("Scenes.Katherine.PcPenetratesKatDoublyHighCumCorrupt.BlushesGivesKiss".Localized(
                CatGirl("cat girl.", "kitty.")
            ));

            Player.Orgasm("Dick");

            Orgasm();

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            ReturnToCamp(1);
        }

        public void SuckNFuck()
        {
            ClearOutput();

            int x = Player.CockThatFits(70);

            ShowImage("katherine-fuck-and-suck");

            OutputText("Scenes.Katherine.SuckNFuck.GazeDriftsSheath".Localized(
                CockType()
            ));

            if (HasAlready(KBIT_TRIED_RECEIVE_SUCK_N_FUCK))
                OutputText("Scenes.Katherine.SuckNFuck.SwallowsOwnShaft".Localized(
                    CockMultiple("", "s"),

                    CockMultiple("", "s")
                ));
            else
                OutputText("Scenes.Katherine.SuckNFuck.RecallsShrinkingKnot".Localized(
                    CockMultiple("", "s")
                ));

            OutputText("Scenes.Katherine.SuckNFuck.SheSmiles".Localized());

            if (IsAt(KLOC_STREETS))
                OutputText("Scenes.Katherine.SuckNFuck.SitsOnCrate".Localized());
            else
                OutputText("Scenes.Katherine.SuckNFuck.SitsSpreadsLegs".Localized());

            OutputText("Scenes.Katherine.SuckNFuck.SentencePeriod".Localized());

            OutputText("Scenes.Katherine.SuckNFuck.StrokesSheathPussy".Localized(
                CockType("dog", "cat"),

                CockMultiple("", "s")
            ));

            if (!Player.HasPerk(PerkIds.Flexibility))
                OutputText("Scenes.Katherine.SuckNFuck.SpineWinces".Localized());
            else
                OutputText("Scenes.Katherine.SuckNFuck.WatchesResolvesTest".Localized());

            OutputText("Scenes.Katherine.SuckNFuck.StraightensBack".Localized(
                CockMultiple("", "topmost "),

                CockType("dog", "cat"),

                CockType("rubbery, conical", "narrow, barbed"),

                cockLength,

                CockType("dog", "cat")
            ));

            OutputText("Scenes.Katherine.SuckNFuck.StepsForward".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Katherine.SuckNFuck.AwkwardAtFirst".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Katherine.SuckNFuck.YourBalls".Localized());

            OutputText("Scenes.Katherine.SuckNFuck.InnerThighsRhythm".Localized());

            OutputText("Scenes.Katherine.SuckNFuck.HardToDescribe".Localized(
                CockMultiple("", ", and her lower shaft waving in the air between you as if to conduct the performance")
            ));

            var kathCum = CumQ();

            var playerCum = Player.CumQ();

            if (kathCum <= 500)
                OutputText("Scenes.Katherine.SuckNFuck.GulpsUntilEmpty".Localized());
            else
                if (kathCum <= 1500)
                    OutputText("Scenes.Katherine.SuckNFuck.DrinksToAvoidChoking".Localized());
                else
                    OutputText("Scenes.Katherine.SuckNFuck.FranticSwallows".Localized());

            OutputText("Scenes.Katherine.SuckNFuck.ParagraphBreak".Localized());

            OutputText("Scenes.Katherine.SuckNFuck.YourTurnToCum".Localized());

            if (playerCum <= 250)
                OutputText("Scenes.Katherine.SuckNFuck.SpraysIntoHerCunt".Localized());
            else
                if (playerCum <= 750)
                {
                    if (kathCum <= 500)
                        OutputText("Scenes.Katherine.SuckNFuck.BellyBulgesFlood".Localized());
                    else
                        OutputText("Scenes.Katherine.SuckNFuck.LooksHeavilyPregnant".Localized());
                }
                else
                    if (kathCum <= 500)
                        OutputText("Scenes.Katherine.SuckNFuck.LeavesHerBloated".Localized());
                    else
                    {
                        OutputText("Scenes.Katherine.SuckNFuck.CatHermPanicked".Localized());

                        if (IsAt(KLOC_STREETS))
                            OutputText("Scenes.Katherine.SuckNFuck.AlleyWord".Localized());
                        else
                            OutputText("Scenes.Katherine.SuckNFuck.ApartmentFloor".Localized());
                    }

            OutputText("Scenes.Katherine.SuckNFuck.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Katherine.SuckNFuck.ClimaxFinishedPullsOut".Localized());

            if (playerCum > 250)
                OutputText("Scenes.Katherine.SuckNFuck.CascadeOfJism".Localized());

            OutputText("Scenes.Katherine.SuckNFuck.StepsBackBreath".Localized());

            OutputText("Scenes.Katherine.SuckNFuck.StayUntilKnotShrinks".Localized());

            if (kathCum > 500 || playerCum > 250)
            {
                OutputText("Scenes.Katherine.SuckNFuck.SlappingHer".Localized());

                if (kathCum > 1500 || playerCum > 750)
                    OutputText("Scenes.Katherine.SuckNFuck.HugelyWord".Localized());

                OutputText("Scenes.Katherine.SuckNFuck.SwollenCumGutEmphasis".Localized());
            }

            OutputText("Scenes.Katherine.SuckNFuck.ReadyToTryAgain".Localized());

            OutputText("Scenes.Katherine.SuckNFuck.PromiseRemember".Localized());

            Player.Orgasm("Dick");

            Orgasm();

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Experience(KBIT_TRIED_RECEIVE_SUCK_N_FUCK);

            ReturnToCamp(1);
        }

        public void LetKatKnotYou()
        {
            ClearOutput();

            if (IsAt(KLOC_BAR) || IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
            {
                OutputText("Scenes.Katherine.LetKatKnotYou.WaitsRubbingLeg".Localized());

                OutputText("Scenes.Katherine.LetKatKnotYou.ShiversAtThoughts".Localized(
                    CockType(),

                    CockMultiple("", "s")
                ));
            }
            else
                LetKatKnotYouCommonDialogue(false);

            ClearButtons();

            if (Player.HasVagina())
            {
                AddButton(0, "Common.Button.Vagina".Localized(), LetKatKnotYourCuntPussyFuck);

                if (cockNumber > 1)
                    AddButton(2, "Common.Button.Dblpenetr".Localized(), GetDoublePennedByKat);
                else
                    AddButton(2, "Common.Button.Dblpenetr".Localized(), null);
            }
            else
            {
                AddButton(0, "Common.Button.Vagina".Localized(), null);

                AddButton(2, "Common.Button.Dblpenetr".Localized(), null);
            }

            AddButton(1, "Common.Button.Anus".Localized(), GetPenetrated);

            if (Player.HasCock() && CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) > 0)
                AddButton(3, "Common.Button.Sucknfuckd".Localized(), SuckedNFuckedByKat);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) <= 0)
                    AddButton(3, "Common.Button.Sucknfuckd".Localized(), null);
                else
                    AddButton(3, "Common.Button.Sucknfuckd".Localized(), null);

            if (IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
                AddButton(4, "Common.Button.Backroom".Localized(), DrunkFuck);
            else
                if (IsAt(KLOC_BAR))
                    AddButton(4, "Common.Button.Backroom".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), KatSexMenu);
        }

        public void LetKatKnotYouCommonDialogue(bool inAlleyBehindBar)
        {

            if (inAlleyBehindBar)
            {
                OutputText("Scenes.Katherine.LetKatKnotYouCommonDialogue.GulpsDragsAlley".Localized());

                OutputText("Scenes.Katherine.LetKatKnotYouCommonDialogue.OutsideBigHug".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KLOC_STREETS);
            }

            var hasPenetrated = HasAlready(KBIT_TRIED_GIVE_VAGINAL | KBIT_TRIED_GIVE_ANAL | KBIT_TRIED_GIVE_DOUBLE_PEN | KBIT_TRIED_GIVE_SUCK_N_FUCK);

            OutputText("Scenes.Katherine.LetKatKnotYouCommonDialogue.AsksToPenetrateYou".Localized(
                CockType(),

                CockMultiple(" is", "s are")
            ));

            OutputText("Scenes.Katherine.LetKatKnotYouCommonDialogue.TeasesDoggyStyle".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYouCommonDialogue.HermStepsBehind".Localized(
                CockType("mismatched ", "")
            ));

            if (HasDogCock())
                OutputText("Scenes.Katherine.LetKatKnotYouCommonDialogue.VeGotFragment".Localized(
                    CockMultiple("a dog dick", "dog dicks")
                ));
            else
                OutputText("Scenes.Katherine.LetKatKnotYouCommonDialogue.HadLongTime".Localized(
                    CockMultiple("a dog dick", "dog dicks")
                ));

            OutputText("Scenes.Katherine.LetKatKnotYouCommonDialogue.WayNatureIntended".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Katherine.LetKatKnotYouCommonDialogue.WhichHoleToUse".Localized());
        }

        public void LetKatKnotYourCuntPussyFuck()
        {
            ClearOutput();

            ShowImage("katherine-fucks-you-knottily-in-the-vagoo");

            if (IsAt(KLOC_BAR) || IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
                LetKatKnotYouCommonDialogue(true);

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.IndicatesVagina".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.HandsRubPossessively".Localized(
                Player.AssDescript(),

                CockMultiple("her ", "the topmost "),

                cockLength,

                CockType("rubbery-smooth", "barb-covered")
            ));

            if (cockNumber > 1)
            {
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.SecondCockSlaps".Localized());

                if (Player.HasCock())
                    OutputText(Player.MultiCockDescriptLight());
                else
                    if (Player.Balls > 0)
                        OutputText(Player.SackDescript());
                    else
                        OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.BellyWord".Localized());

                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.SentencePeriod".Localized());
            }

            var cunt = Player.Vaginas[0].Looseness;

            Player.CuntChange(CockArea(), true, true, false);

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.ParagraphBreak".Localized());

            if (cunt < Player.Vaginas[0].Looseness)
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.YelpsInShock".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.DigsHipsThrusts".Localized());

            if (cockNumber > 1)
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.SecondCockSlapsWetly".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.GruntsUnbelievable".Localized());

            if (Player.HarpyScore() >= 4 || Player.SharkScore() >= 4 || Player.CatScore() >= 4 || Player.DogScore() >= 4 || Player.BunnyScore() >= 4)
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.MoanLittleSlut".Localized());
            else
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.SayEnd".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.MoansThrustsBack".Localized());

            if (Player.HasTailInsteadOfLegs() || Player.Body.Get<TailPart>().Id == BodyPartTypes.Lizard || Player.Body.Get<TailPart>().Id == BodyPartTypes.Demonic)
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.TailBetweenBreasts".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.CriesHerName".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.KnotGrowing".Localized());

            cunt = Player.Vaginas[0].Looseness;

            Player.CuntChange(CockArea(), true, true, false);

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.LungesGrabsShoulders".Localized());

            if (cunt < Player.Vaginas[0].Looseness)
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.BorderOfPain".Localized());
            else
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.GlovelikeFit".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.ParagraphBreakThird".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.ShuddersClimax".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.CocksSpurting".Localized());

                if (Player.CumQ() < 25)
                    OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.DropsWord".Localized());
                else
                    if (Player.CumQ() < 100)
                        OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.SplashesWord".Localized());
                    else
                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.PuddlesWord".Localized());
                        else
                            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.LakeOfSpunk".Localized());
            }

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.SentencePeriodBreak".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.ArchesBackYowls".Localized());

            if (cockNumber > 1)
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.SecondCockGlazes".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.PeriodSpace".Localized());

            if (CumQ() <= 500)
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.SurprisinglyLargeLoad".Localized());
            else
                if (CumQ() <= 1500)
                    OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.JetAfterJet".Localized(
                        CockType("dog-dicked cat", "barbed tip")
                    ));
                else
                {
                    OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.CumsAndCums".Localized());

                    if (knotSize >= 6)
                        OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.KnotKeepsPlugged".Localized());
                    else
                        OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.StartsLeakingOut".Localized());
                }

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.CollapsesBonelessly".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.KatherineApologizes".Localized());

            if (knotSize >= 6)
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.MustStayUntilDeflate".Localized());
            else
                if (knotSize >= 4)
                    OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.GiveMeLittleWhile".Localized());
                else
                    OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.PullHardPopOut".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.ParagraphBreakFourth".Localized());

            if (Player.VaginalCapacity() >= 100 && IsAt(KLOC_STREETS))
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.NonplussedWaiting".Localized());
            else
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.StaysHereWithHer".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.ParagraphBreakFifth".Localized());

            if (Player.VaginalCapacity() < 100)
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.HourLaterDeflated".Localized());
            else
                OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.YLetter".Localized());

            OutputText("Scenes.Katherine.LetKatKnotYourCuntPussyFuck.GetDressedHeadBack".Localized());

            Player.Orgasm("Vaginal");

            Orgasm();

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Experience(KBIT_TRIED_GIVE_VAGINAL);

            ReturnToCamp(1);
        }

        public void GetPenetrated()
        {
            ClearOutput();

            ShowImage("katherine-fucks-you-knottily-in-the-bungholio");

            if (IsAt(KLOC_BAR) || IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
                LetKatKnotYouCommonDialogue(true);

            OutputText("Scenes.Katherine.GetPenetrated.IndicatesAsshole".Localized());

            OutputText("Scenes.Katherine.GetPenetrated.NotBigFanHands".Localized(
                CatGirl("", " furry"),

                CockMultiple("", "bottom-most "),

                CockType("rubbery-smooth", "barb-covered"),

                CockMultiple("", ", as her upper cock bounces along your back")
            ));

            OutputText("Scenes.Katherine.GetPenetrated.MumblesDragsCock".Localized(
                CockMultiple("", "s"),

                CockType("puppy pecker", "kitty cock"),

                CockMultiple("", ", as well as onto your cheeks, with the other bouncing around above them")
            ));

            var butts = Player.Anal.Looseness;

            Player.ButtChange(CockArea(), true, true, false);

            OutputText("Scenes.Katherine.GetPenetrated.ParagraphBreak".Localized());

            if (butts > Player.Anal.Looseness)
                OutputText("Scenes.Katherine.GetPenetrated.YelpsInShock".Localized());

            OutputText("Scenes.Katherine.GetPenetrated.DigsHipsThrusts".Localized(
                CockMultiple("", ", her secondary cock slapping wetly against your back")
            ));

            if (Player.HarpyScore() >= 4 || Player.SharkScore() >= 4 || Player.CatScore() >= 4 || Player.DogScore() >= 4 || Player.BunnyScore() >= 4)
                OutputText("Scenes.Katherine.GetPenetrated.MoanLittleSlut".Localized(
                    CatGirl("sharp nails dig in", "claws extend")
                ));
            else
                OutputText("Scenes.Katherine.GetPenetrated.SayEndParagraphBreak".Localized());

            OutputText("Scenes.Katherine.GetPenetrated.MoansThrustsBack".Localized());

            if (Player.HasTailInsteadOfLegs() || Player.Body.Get<TailPart>().Id == BodyPartTypes.Lizard || Player.Body.Get<TailPart>().Id == BodyPartTypes.Demonic)
                OutputText("Scenes.Katherine.GetPenetrated.TailBetweenBreasts".Localized());

            OutputText("Scenes.Katherine.GetPenetrated.CriesHerName".Localized());

            OutputText("Scenes.Katherine.GetPenetrated.KnotFilling".Localized());

            if (knotSize >= 6)
                OutputText("Scenes.Katherine.GetPenetrated.LikeMelonInside".Localized());
            else
                if (knotSize >= 4)
                    OutputText("Scenes.Katherine.GetPenetrated.BorderOfPain".Localized());
                else
                    OutputText("Scenes.Katherine.GetPenetrated.NormalSizedKnot".Localized());

            OutputText("Scenes.Katherine.GetPenetrated.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Katherine.GetPenetrated.ShuddersClimax".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Katherine.GetPenetrated.FemcumSplashing".Localized());

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Katherine.GetPenetrated.SpurtingWord".Localized(
                    Player.SMultiCockDesc()
                ));

                if (Player.CumQ() < 25)
                    OutputText("Scenes.Katherine.GetPenetrated.DropsWord".Localized());
                else
                    if (Player.CumQ() < 100)
                        OutputText("Scenes.Katherine.GetPenetrated.SplashesWord".Localized());
                    else
                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Katherine.GetPenetrated.PuddlesWord".Localized());
                        else
                            OutputText("Scenes.Katherine.GetPenetrated.LakeOfSpunk".Localized());
            }

            OutputText("Scenes.Katherine.GetPenetrated.AssholeWringsInvader".Localized());

            OutputText("Scenes.Katherine.GetPenetrated.ArchesBackYowls".Localized());

            if (cockNumber > 1)
                OutputText("Scenes.Katherine.GetPenetrated.SecondCockGlazesBack".Localized());

            OutputText("Scenes.Katherine.GetPenetrated.PeriodSpace".Localized());

            if (CumQ() <= 500)
                OutputText("Scenes.Katherine.GetPenetrated.SurprisinglyLargeLoad".Localized());
            else
                if (CumQ() <= 1500)
                    OutputText("Scenes.Katherine.GetPenetrated.JetAfterJet".Localized(
                        CockType("dog-dicked cat", "barbed tip")
                    ));
                else
                {
                    OutputText("Scenes.Katherine.GetPenetrated.CumsAndCums".Localized());

                    if (knotSize >= 6)
                        OutputText("Scenes.Katherine.GetPenetrated.KnotKeepsPlugged".Localized());
                    else
                        OutputText("Scenes.Katherine.GetPenetrated.StartsLeakingOut".Localized());

                    OutputText("Scenes.Katherine.GetPenetrated.TastesSaltySpunk".Localized());
                }

            OutputText("Scenes.Katherine.GetPenetrated.CollapsesBonelessly".Localized());

            OutputText("Scenes.Katherine.GetPenetrated.KatherineApologizes".Localized());

            if (knotSize >= 6)
                OutputText("Scenes.Katherine.GetPenetrated.MustStayUntilDeflate".Localized());
            else
                if (knotSize >= 4)
                    OutputText("Scenes.Katherine.GetPenetrated.GiveMeLittleWhile".Localized());
                else
                    OutputText("Scenes.Katherine.GetPenetrated.PullHardPopOut".Localized());

            OutputText("Scenes.Katherine.GetPenetrated.StaysHereWithHer".Localized());

            OutputText("Scenes.Katherine.GetPenetrated.HourLaterDeflated".Localized());

            Player.Orgasm("Anal");

            Orgasm();

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Experience(KBIT_TRIED_GIVE_ANAL);

            ReturnToCamp(1);
        }

        public void GetDoublePennedByKat()
        {
            ClearOutput();

            var doneBefore = HasAlready(KBIT_TRIED_GIVE_DOUBLE_PEN);

            ShowImage("katherine-fucks-you-knottily-in-all-the-holes");

            if (IsAt(KLOC_BAR) || IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
                LetKatKnotYouCommonDialogue(true);

            OutputText("Scenes.Katherine.GetDoublePennedByKat.WantBothHoles".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.HandsRubPossessively".Localized(
                CatGirl("soft", "furry"),

                Player.AssDescript(),

                CockAdj(),

                CockType("rubbery-smooth", "barb-covered")
            ));

            Player.CuntChange(CockArea(), true, true, false);

            OutputText("Scenes.Katherine.GetDoublePennedByKat.ParagraphBreak".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.LookBackAtHer".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.FingersDigHips".Localized());

            Player.ButtChange(CockArea(), true, true, false);

            OutputText("Scenes.Katherine.GetDoublePennedByKat.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.ThrustingAtOnce".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.ThrustAssBack".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.MoansKnotsFilling".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.ArchesBackYowl".Localized());

            if (CumQ() <= 500)
                OutputText("Scenes.Katherine.GetDoublePennedByKat.LargeAmountOfCum".Localized());
            else
                if (CumQ() <= 1500)
                    OutputText("Scenes.Katherine.GetDoublePennedByKat.JetAfterJet".Localized(
                        CockType("dog-dicked cat", "barbed tip")
                    ));
                else
                {
                    OutputText("Scenes.Katherine.GetDoublePennedByKat.CumsAndCumsAnd".Localized());

                    if (knotSize >= 6)
                        OutputText("Scenes.Katherine.GetDoublePennedByKat.KnotsKeepPlugged".Localized());
                    else
                        OutputText("Scenes.Katherine.GetDoublePennedByKat.StartsLeaking".Localized());
                }

            OutputText("Scenes.Katherine.GetDoublePennedByKat.CollapsesBonelessly".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.KatherineApologizes".Localized());

            if (knotSize >= 6)
                OutputText("Scenes.Katherine.GetDoublePennedByKat.MustStayUntilDeflate".Localized());
            else
                if (knotSize >= 4)
                    OutputText("Scenes.Katherine.GetDoublePennedByKat.WaitForDeflate".Localized());
                else
                    OutputText("Scenes.Katherine.GetDoublePennedByKat.PullHardToPop".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.NoMatterStay".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.HideWickedGrin".Localized());

            if (CumQ() > 500)
                OutputText("Scenes.Katherine.GetDoublePennedByKat.CumSloshesInside".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.ParagraphBreakThird".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.TooMuchPleaseStop".Localized());

            if (CumQ() > 1500)
                OutputText("Scenes.Katherine.GetDoublePennedByKat.ToItsLimit".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.SmallerSecondLoad".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Katherine.GetDoublePennedByKat.OwnSemenSpraying".Localized(
                    Player.SMultiCockDesc()
                ));

            OutputText("Scenes.Katherine.GetDoublePennedByKat.GaspsAndTwitches".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.ParagraphBreakFourth".Localized());

            OutputText("Scenes.Katherine.GetDoublePennedByKat.HourLaterRise".Localized());

            Player.SlimeFeed();

            Player.Orgasm("VaginalAnal");

            Orgasm();

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Experience(KBIT_TRIED_GIVE_DOUBLE_PEN);

            ReturnToCamp(1);
        }

        public void SuckedNFuckedByKat()
        {
            ClearOutput();

            var doneBefore = HasAlready(KBIT_TRIED_GIVE_SUCK_N_FUCK);

            var x = Player.BiggestCockIndex();

            ShowImage("katherine-suck-and-fucks-you");

            if (IsAt(KLOC_BAR) || IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
                LetKatKnotYouCommonDialogue(true);

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.CrouchFiguring".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.PurrsSpecialTreat".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.DoAsTold".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.BendsOverToLick".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.ForgottenWhosFucking".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Katherine.SuckedNFuckedByKat.YourVagina".Localized());
            else
                OutputText("Scenes.Katherine.SuckedNFuckedByKat.BetweenAsscheeks".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.SlidesCockForward".Localized(
                CockMultiple("", "s")
            ));

            if (cockNumber > 1 && Player.HasVagina())
                OutputText("Scenes.Katherine.SuckedNFuckedByKat.VaginaAndAsshole".Localized());
            else
                OutputText(Player.AssholeOrPussy());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.SlathersPreCum".Localized(
                CockMultiple("", "s")
            ));

            if (Player.HasVagina())
            {
                if (cockNumber > 1)
                    Player.ButtChange(CockArea(), true, true, false);

                Player.CuntChange(CockArea(), true, true, false);
            }
            else
                Player.ButtChange(CockArea(), true, true, false);

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.ParagraphBreak".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.GiveThisAShot".Localized(
                CockMultiple("", "s"),

                CockMultiple("", "s"),

                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.NoPositionComplain".Localized());

            if (Player.HasVagina() && cockNumber > 1)
                OutputText("Scenes.Katherine.SuckedNFuckedByKat.SLetter".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.SoDeliciouslyFull".Localized());

            if (Player.HasVagina() && cockNumber > 1)
                OutputText("Scenes.Katherine.SuckedNFuckedByKat.SLetterAgain".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.FlaringAnchoring".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Katherine.SuckedNFuckedByKat.InYourBalls".Localized());
            else
                OutputText("Scenes.Katherine.SuckedNFuckedByKat.BaseOfSpine".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.EllipsisParagraph".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.KatherineCumsFirst".Localized());

            if (Player.HasVagina() && cockNumber > 1)
                OutputText("Scenes.Katherine.SuckedNFuckedByKat.SLetterThird".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.SpunkGushingHot".Localized());

            if (CumQ() > 500)
                OutputText("Scenes.Katherine.SuckedNFuckedByKat.DoesntStopGrowing".Localized());

            if (CumQ() > 1500)
                OutputText("Scenes.Katherine.SuckedNFuckedByKat.HeavyWithCum".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.SlumpsForwardGroan".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.YourTurnToCum".Localized());

            if (Player.CumQ() <= 250)
                OutputText("Scenes.Katherine.SuckedNFuckedByKat.DrinksEveryDrop".Localized());
            else
                if (Player.CumQ() <= 600)
                    OutputText("Scenes.Katherine.SuckedNFuckedByKat.GulpsDesperately".Localized());
                else
                    OutputText("Scenes.Katherine.SuckedNFuckedByKat.HintOfPanic".Localized(
                        Player.CockDescript(x)
                    ));

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.AllowsToCollapse".Localized());

            if (CumQ() <= 500)
            {
                if (Player.CumQ() <= 250)
                    OutputText("Scenes.Katherine.SuckedNFuckedByKat.LyingBellyToBelly".Localized());
                else
                    OutputText("Scenes.Katherine.SuckedNFuckedByKat.BellySquishingAudibly".Localized());
            }
            else
                if (Player.CumQ() <= 250)
                    OutputText("Scenes.Katherine.SuckedNFuckedByKat.GutSquelchWetly".Localized());
                else
                    OutputText("Scenes.Katherine.SuckedNFuckedByKat.BelliesChurningRippling".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.GrinsHandInching".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.EntwineFingers".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.OnceHerKnot".Localized());

            if (cockNumber > 1 && Player.HasVagina())
                OutputText("Scenes.Katherine.SuckedNFuckedByKat.SLetterFourth".Localized());

            OutputText("Scenes.Katherine.SuckedNFuckedByKat.DeflateDetachClean".Localized());

            Player.SlimeFeed();

            Player.Orgasm("VaginalAnal");

            Orgasm();

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Experience(KBIT_TRIED_GIVE_SUCK_N_FUCK);

            ReturnToCamp(1);
        }

        public void OralKatherineChoices()
        {
            ClearOutput();

            if (IsAt(KLOC_BAR) || IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
            {
                OutputText("Scenes.Katherine.OralKatherineChoices.TailTwitchesExcitedly".Localized());

                OutputText("Scenes.Katherine.OralKatherineChoices.LeadToAlleyway".Localized());

                OutputText("Scenes.Katherine.OralKatherineChoices.WorriedCaught".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KLOC_STREETS);
            }

            OutputText("Scenes.Katherine.OralKatherineChoices.SuggestTasteTest".Localized());

            ClearButtons();

            if (HasCock())
                AddButton(0, "Common.Button.PcSucks".Localized(), GiveKatOralPenisWingWang);
            else
                AddButton(0, "Common.Button.PcLaps".Localized(), GiveKatOralPussyLicking);

            if (!Player.IsGenderless())
                AddButton(1, "Common.Button.KathLaps".Localized(), KatherineGivesPCOralAllDayLongDotJPG);
            else
                AddButton(1, "Common.Button.KathLaps".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), KatSexMenu);
        }

        public void GiveKatOralPenisWingWang()
        {
            ClearOutput();

            var doneBefore = HasAlready(KBIT_TRIED_RECEIVE_ORAL);

            ShowImage("katherine-give-her-blowjobs");

            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.WouldntMindOrally".Localized());

            if (doneBefore)
                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.OkayWithKnot".Localized(
                    CockMultiple("", "s, and the twin cocks,")
                ));
            else
                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.AreYouSure".Localized(
                    CockMultiple("", "s... I mean, now that I have two cocks")
                ));

            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.ReassureHer".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.AvertingEyesAnswers".Localized(
                CatGirl("blushing bright red", "apparently blushing under the fur")
            ));

            if (IsAt(KLOC_KATHS_APT))
            {
                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.MotionSitDown".Localized());

                if (!Player.IsTaur() && Player.Tallness < 102)
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.EdgeOfBed".Localized());
                else
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.TopOfDresser".Localized());
            }
            else
                if (IsAt(KLOC_DESERT))
                {
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.MotionSitAgain".Localized());

                    if (!Player.IsTaur() && Player.Tallness < 102)
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.LargeSandDune".Localized());
                    else
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.SmallSandDune".Localized());
                }
                else
                {
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.MotionHalfSit".Localized());

                    if (!Player.IsTaur() && Player.Tallness < 102)
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.EmptyCrate".Localized());
                    else
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.BroadStoneWall".Localized());
                }

            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.ContraryEager".Localized(
                CockMultiple("her cock and", "both her cocks as well as")
            ));

            if (cockNumber == 1)
            {
                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.FirstTargetPussy".Localized());

                if (Player.Corruption >= 70)
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.ProveSkill".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.PeriodParagraph".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.TeasingLickCircle".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.EnoughAttention".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.MoansTracingPenis".Localized(
                    CockType("conical, pointy", "narrow, bumpy")
                ));

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.MewlsResistPleasure".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.VaginaTeasedEnough".Localized());

                if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.HumThenDeep".Localized());
                else
                    if (Player.HasLongTongue())
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.WhereKnotPops".Localized());
                    else
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.HandRemainsPussy".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.SlurpingHerCock".Localized());

                if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.MolestingHerClit".Localized());
                else
                    if (Player.HasLongTongue())
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.TonguePenetrating".Localized());
                    else
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.FingeringHer".Localized());

                if (!HasBalls())
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.PullAwayMoment".Localized());
                else
                {
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.ChangeTactics".Localized());

                    if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                    {
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.BallsInTheWay".Localized());

                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.AttendHerMember".Localized());
                    }
                    else
                        if (Player.HasLongTongue())
                        {
                            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.ShiverHelplessDesire".Localized());

                            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.AttendMemberAgain".Localized());
                        }
                        else
                        {
                            if (!HasBalls())
                                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.LickClitToo".Localized());
                            else
                                if (ballSize <= 2)
                                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.ReachPussyTease".Localized());
                                else
                                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.HopedPlayBalls".Localized());

                            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.FingersOverTip".Localized());
                        }
                }

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.AfterMolestingAnnounce".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.GaspsThanksMewl".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.NodConsideringSeal".Localized());

                if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                {
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.GrabBaseLickLips".Localized());

                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.TelltaleOrgasmSigns".Localized());

                    if (knotSize >= 6)
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.KnotHindersPussy".Localized());

                    if (Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.PlaySnakePart".Localized());
                    else
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.NotContentUnattended".Localized());

                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.SqueezedDrenched".Localized());

                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.GreedilySwallowSemen".Localized());

                    if (CumQ() <= 500)
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.LargeAmountWarmDrink".Localized());
                    else
                        if (CumQ() <= 1500)
                            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.ThickRopesJizz".Localized());
                        else
                            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.CumsLotWaveAfter".Localized());

                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.PullCockOutSlowly".Localized());
                }
                else
                    if (Player.HasLongTongue())
                    {
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.SeparateForKnot".Localized());

                        if (knotSize >= 6)
                            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.HardKeepTongue".Localized());

                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.PeriodParagraphAgain".Localized());

                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.LoudYowlCums".Localized());

                        if (CumQ() <= 500)
                            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.ThrashesInOrgasm".Localized());
                        else
                            if (CumQ() <= 1500)
                                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.TongueSlidesEasily".Localized());
                            else
                                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.LoadSeepsInside".Localized());

                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.PussySqueezesRhythmically".Localized());

                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.DoneClimaxingSlide".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.FollowAdviceDive".Localized());

                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.HandMeetsResistance".Localized());

                        if (CumQ() > 1500)
                            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.GiggleIntoPussy".Localized());
                    }
            }
            else
            {
                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.SeeingCockBob".Localized(
                    CockType()
                ));

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.HandsInTheWay".Localized());

                if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake || Player.HasLongTongue())
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.TongueWrapsErections".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.ParagraphBreak".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.MoveDownToBalls".Localized());

                if (HasBalls())
                {
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.TeasingLickBalls".Localized());

                    if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake || Player.HasLongTongue())
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.LongTongueLiftsTestes".Localized());

                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.MinisterToBalls".Localized());

                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.YourFragment".Localized());
                }
                else
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.YourFragmentAgain".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.KissesLowerLips".Localized());

                if (Player.HasLongTongue())
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.SuckClitExtra".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.DrawAwayAdmire".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.PleaseContinue".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.FingerHerGently".Localized());

                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.PantsWhimpersLoss".Localized());

                if (!Player.HasLongTongue())
                {
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.StrokeAboveKnot".Localized());

                    if (Player.HasTailInsteadOfLegs() || Player.Body.Get<TailPart>().Id == BodyPartTypes.Demonic)
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.MaximizeStimulation".Localized());

                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.ParagraphBreakAgain".Localized());

                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.MomentsLaterEjaculates".Localized());

                    if (CumQ() > 1500)
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.QuiteALot".Localized());
                }
                else
                {
                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.TongueSlidesOut".Localized());

                    OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.ErectionsReleaseUnison".Localized());

                    if (CumQ() > 1500)
                        OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.LiberallyCoatedSpunk".Localized());
                }
            }

            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.DeedDoneWink".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.ExclaimsAmazing".Localized());

            if (Player.HasLongTongue())
                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.FingersAlongLips".Localized());
            else
                OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.HopeRepayFavor".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPenisWingWang.KissesAgainHands".Localized());

            Orgasm();

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Experience(KBIT_TRIED_RECEIVE_ORAL);

            Player.SlimeFeed();

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            ReturnToCamp(1);
        }

        public void GiveKatOralPussyLicking()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.OfferOralGrin".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.WontRefuse".Localized());

            if (IsAt(KLOC_KATHS_APT))
            {
                OutputText("Scenes.Katherine.GiveKatOralPussyLicking.MotionSitDown".Localized());

                if (!Player.IsTaur() && Player.Tallness < 102)
                    OutputText("Scenes.Katherine.GiveKatOralPussyLicking.EdgeOfBed".Localized());
                else
                    OutputText("Scenes.Katherine.GiveKatOralPussyLicking.TopOfDresser".Localized());
            }
            else
                if (IsAt(KLOC_DESERT))
                {
                    OutputText("Scenes.Katherine.GiveKatOralPussyLicking.MotionSitAgain".Localized());

                    if (!Player.IsTaur() && Player.Tallness < 102)
                        OutputText("Scenes.Katherine.GiveKatOralPussyLicking.LargeSandDune".Localized());
                    else
                        OutputText("Scenes.Katherine.GiveKatOralPussyLicking.SmallSandDune".Localized());
                }
                else
                {
                    OutputText("Scenes.Katherine.GiveKatOralPussyLicking.MotionHalfSit".Localized());

                    if (!Player.IsTaur() && Player.Tallness < 102)
                        OutputText("Scenes.Katherine.GiveKatOralPussyLicking.EmptyCrate".Localized());
                    else
                        OutputText("Scenes.Katherine.GiveKatOralPussyLicking.BroadStoneWall".Localized());
                }

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.SeemsEager".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.AttackOuterLips".Localized());

            if (Player.Corruption >= 70)
                OutputText("Scenes.Katherine.GiveKatOralPussyLicking.ProveSkill".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.PeriodParagraph".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.TeasingLickCircle".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.PryOpenThighs".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.MewlsResistPleasure".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake || Player.HasLongTongue())
                OutputText("Scenes.Katherine.GiveKatOralPussyLicking.FullUseOfTongue".Localized());
            else
                OutputText("Scenes.Katherine.GiveKatOralPussyLicking.TongueUnderClit".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.AfterMolestingClit".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.GaspsThanksMewl".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.TimeToFinish".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.PantingMewlGrowl".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.DeedDoneWink".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.ExclaimsAmazing".Localized());

            if (Player.HasLongTongue())
                OutputText("Scenes.Katherine.GiveKatOralPussyLicking.FingersAlongLips".Localized());
            else
                OutputText("Scenes.Katherine.GiveKatOralPussyLicking.HopeRepayFavor".Localized());

            OutputText("Scenes.Katherine.GiveKatOralPussyLicking.KissesAgainHands".Localized());

            Orgasm();

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Player.SlimeFeed();

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            ReturnToCamp(1);
        }

        public void KatherineGivesPCOralAllDayLongDotJPG()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.KatherineGivesPCOralAllDayLongDotJPG.SeeTongueSkills".Localized());

            if (Player.HasCock() && (Player.Gender == Gender.Male || Random.Shared.Next(2) == 0))
                KatherineLicksAllTheBoyPenises();
            else
                KatherineGoesDownOnTheGirlsOhYahBabyLesbo();
        }

        public void KatherineGoesDownOnTheGirlsOhYahBabyLesbo()
        {
            ClearOutput();

            ShowImage("katherine-eats-out-your-vagoo");

            OutputText("Scenes.Katherine.KatherineGoesDownOnTheGirlsOhYahBabyLesbo.StrokesLipsLicks".Localized());

            OutputText("Scenes.Katherine.KatherineGoesDownOnTheGirlsOhYahBabyLesbo.LicksAgain".Localized());

            OutputText("Scenes.Katherine.KatherineGoesDownOnTheGirlsOhYahBabyLesbo.BothersHerNoSign".Localized());

            if (Player.Cocks.Count > 0)
                OutputText("Scenes.Katherine.KatherineGoesDownOnTheGirlsOhYahBabyLesbo.SemenRainsDown".Localized());

            OutputText("Scenes.Katherine.KatherineGoesDownOnTheGirlsOhYahBabyLesbo.ReleaseHerGasping".Localized());

            OutputText("Scenes.Katherine.KatherineGoesDownOnTheGirlsOhYahBabyLesbo.GroanAtPun".Localized());

            ReturnToCamp(1);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Player.Orgasm("Vaginal");
        }

        public void KatherineLicksAllTheBoyPenises()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            ShowImage("katherine-gives-you-blowjobs/");

            OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.TakesShaftEagerly".Localized(
                Player.CockHead(x)
            ));

            if (Player.HasVagina())
            {
                OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.HandSlidesDown".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.PastYourBalls".Localized());

                OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.StrokesYourCunny".Localized());
            }

            OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.ParagraphBreak".Localized());

            OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.PreStartsFlowing".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.FingersPlungeCunt".Localized());

            OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.SkinInHerMouth".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.FingersPistonInside".Localized());

            OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.ResistanceMinimal".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.CockTingling".Localized());
            else
                OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.BallsTightening".Localized());

            OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.ReleaseImminentErupt".Localized());

            if (Player.CumQ() <= 250)
                OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.DrinksEveryDrop".Localized());
            else
                if (Player.CumQ() <= 600)
                    OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.GulpsDesperately".Localized());
                else
                    OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.HintOfPanic".Localized(
                        Player.CockDescript(x)
                    ));

            OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.ParagraphBreakThird".Localized());

            OutputText("Scenes.Katherine.KatherineLicksAllTheBoyPenises.CollapsesWeakly".Localized(
                CatGirl("smooth", "furry")
            ));

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            ReturnToCamp(1);
        }

        public void KatDoubleHelixCraziness()
        {
            ClearOutput();

            int x = Player.CockThatFits(70);

            if (x < 0)
                x = 0;

            ShowImage("katherine-double-helix-fuck");

            if (IsAt(KLOC_BAR) || IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
            {
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.TailTwitchesEager".Localized());

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.LeadToAlleyway".Localized());

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.WorriedGettingCaught".Localized());

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.StandInAlley".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KLOC_STREETS);
            }
            else
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.YouWord".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.BothSexesPartner".Localized());

            if (HasAlready(KBIT_TRIED_DOUBLE_HELIX))
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.DoubleHelixFun".Localized());
            else
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.KinkyIdeaComing".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.GrinsAndBendsOver".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.CantResistLaugh".Localized());

            if (HasAlready(KBIT_TRIED_DOUBLE_HELIX))
            {
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.PositionLikeLastTime".Localized(
                    CockMultiple("one of ", ""),

                    CockType(),

                    CockMultiple("", "s"),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.MoansAsTipsEnter".Localized());

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.QuickHug".Localized());
            }
            else
            {
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.RealizeTheProblem".Localized());

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.GetsCueUndresses".Localized());

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.DoAsSheSays".Localized());

                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Katherine.KatDoubleHelixCraziness.CurlingTailBack".Localized());
                else
                    if (Player.IsGoo())
                        OutputText("Scenes.Katherine.KatDoubleHelixCraziness.ShiftingGooeyBlob".Localized());
                    else
                        OutputText("Scenes.Katherine.KatDoubleHelixCraziness.LiftingLegUp".Localized());

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.AccessToBoth".Localized(
                    Player.VaginaDescript(0),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.LiesDownAcross".Localized(
                    CockMultiple("one of ", ""),

                    CockType(),

                    CockMultiple("", "s")
                ));

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.RepeatGesture".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.SheReplies".Localized());

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.Katherine".Localized());
            }

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.FlexesLegsPulls".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.ShiveringDelight".Localized(
                Appearance.CockNoun(BodyPartTypes.Human),

                CockType("rubbery, pointy dog", "narrow, barbed kitty")
            ));

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.WhineAndWhimper".Localized());

            Player.CuntChange(CockArea(), true, true, false);

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.ParagraphBreak".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.GaspFilledFull".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.RepliesHoldingMoan".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.SaysDoingBest".Localized(
                Player.ShortName,

                Player.CockDescript(x)
            ));

            if (Player.CumQ() <= 50)
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.SurgingWord".Localized());
            else
                if (Player.CumQ() <= 250)
                    OutputText("Scenes.Katherine.KatDoubleHelixCraziness.GushingWord".Localized());
                else
                    OutputText("Scenes.Katherine.KatDoubleHelixCraziness.ThunderingWord".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.FromCockAndFemcum".Localized());

            if (Player.Wetness() <= 3)
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.DrizzlingWord".Localized());
            else
                if (Player.Wetness() <= 4)
                    OutputText("Scenes.Katherine.KatDoubleHelixCraziness.SpurtingWord".Localized());
                else
                    OutputText("Scenes.Katherine.KatDoubleHelixCraziness.GushingAgain".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.StrivingToMilk".Localized(
                CatGirl("cat girl", "anthropomorphic")
            ));

            if (CumQ() <= 500)
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.BellyBloatsSlightly".Localized());
            else
                if (CumQ() <= 1500)
                    OutputText("Scenes.Katherine.KatDoubleHelixCraziness.BellyBloatsJism".Localized());
                else
                    OutputText("Scenes.Katherine.KatDoubleHelixCraziness.BellyBloatsRapidly".Localized());

            if (Player.CumQ() <= 500)
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.PayBackInKind".Localized());
            else
                if (Player.CumQ() <= 1500)
                    OutputText("Scenes.Katherine.KatDoubleHelixCraziness.PayBackMassive".Localized());
                else
                    OutputText("Scenes.Katherine.KatDoubleHelixCraziness.PayBackMassiveAgain".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.GaspForBreath".Localized());

            if (CumQ() > 1500)
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.NotSwollenStomach".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.KnotLockedInside".Localized());

            if (HasAlready(KBIT_TRIED_DOUBLE_HELIX))
            {
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.RubNippleAsk".Localized());

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.PatsBellySays".Localized());
            }
            else
            {
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.ExasperationAmusement".Localized());

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.GulpsAudibly".Localized());

                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.SmilesAndSays".Localized());
            }

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.FullPussyGuide".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.DoneWithPussy".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.LickingBallsClean".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.MoanShudderBuck".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.BallsDepleted".Localized());
            else
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.CockNearlyEmpty".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.ConfidentSheCould".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.HardeningAgain".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.HollowGroan".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.ExplodeAgainSoon".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.NodWeakly".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.ThoroughOnePrompt".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.NodAgainUnthinking".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.RollsOntoBack".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.LieBackAndEnjoy".Localized());

            if (Player.HasTailInsteadOfLegs() || Player.Body.Get<TailPart>().Id == BodyPartTypes.Demonic)
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.TailTip".Localized());
            else
                OutputText("Scenes.Katherine.KatDoubleHelixCraziness.YourAssWord".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.FinishesAndDeclares".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.RufflingHerEars".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.LeansPurring".Localized());

            OutputText("Scenes.Katherine.KatDoubleHelixCraziness.SayGoodbyeKitty".Localized());

            Player.SlimeFeed();

            Player.Orgasm("Generic");

            Orgasm();

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Experience(KBIT_TRIED_DOUBLE_HELIX);

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void SuckleMenu()
        {

            ClearButtons();

            AddButton(0, "Common.Button.SheSuckles".Localized(), SuckleTacularKats);

            AddButton(1, "Common.Button.YouSuckle".Localized(), SuckleFromKath);

            AddButton(14, "Common.Button.Back".Localized(), KatSexMenu);
        }

        public void SuckleFromKath()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.SuckleFromKath.SmileAsksAnything".Localized());

            if (IsAt(KLOC_BAR))
                OutputText("Scenes.Katherine.SuckleFromKath.UnderstandsSmiles".Localized());
            else
                if (IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
                    OutputText("Scenes.Katherine.SuckleFromKath.BoozeAddledMind".Localized());
                else
                    if (IsAt(KLOC_KATHS_APT))
                        OutputText("Scenes.Katherine.SuckleFromKath.GrinsMakesShow".Localized());
                    else
                        OutputText("Scenes.Katherine.SuckleFromKath.LooksUpAndDown".Localized());

            if (breasts.Lactation == 1)
            {
                OutputText("Scenes.Katherine.SuckleFromKath.BreastsPlaceholder".Localized(
                    breasts.Adj()
                ));

                if (!MilkOption(KBIT_MILK_TOLD_PLAYER_SHES_DRY))
                {
                    OutputText("Scenes.Katherine.SuckleFromKath.SorryRunOut".Localized(
                        PlayerText()
                    ));

                    OutputText("Scenes.Katherine.SuckleFromKath.LooksWorried".Localized());

                    MilkOptionSet(KBIT_MILK_TOLD_PLAYER_SHES_DRY);
                }

                OutputText("Scenes.Katherine.SuckleFromKath.BreastsGetHint".Localized());
            }
            else
                if (breasts.MilkFullness >= 100)
                {
                    if (breasts.Lactation >= 4)
                    {
                        OutputText("Scenes.Katherine.SuckleFromKath.MilkLadenAgain".Localized(
                            breasts.Adj()
                        ));

                        OutputText("Scenes.Katherine.SuckleFromKath.TelltaleStains".Localized(
                            CatGirl("cat girl", "kitty")
                        ));

                        OutputText("Scenes.Katherine.SuckleFromKath.YesSoFull".Localized(
                            PlayerText()
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Katherine.SuckleFromKath.MilkLadenAgain".Localized(
                            breasts.Adj()
                        ));

                        OutputText("Scenes.Katherine.SuckleFromKath.ThankYouAlwaysFull".Localized());
                    }
                }
                else
                    if (breasts.MilkIsFull())
                    {
                        OutputText("Scenes.Katherine.SuckleFromKath.BreastsAgain".Localized(
                            breasts.Adj()
                        ));

                        OutputText("Scenes.Katherine.SuckleFromKath.HopingStopBy".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Katherine.SuckleFromKath.BreastsThird".Localized(
                            breasts.Adj()
                        ));

                        OutputText("Scenes.Katherine.SuckleFromKath.NeverTurnDown".Localized());
                    }

            if (IsAt(KLOC_BAR) || IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
                OutputText("Scenes.Katherine.SuckleFromKath.MoveToHerSide".Localized(
                    ClothesChoice("slipping off her blouse", "wriggling out of the upper part of her bodysuit", "opening the bodice of her dress", "opening the top of her silk robe", "removing her tube top", "removing her nurse's top")
                ));
            else
                if (IsAt(KLOC_KATHS_APT))
                    OutputText("Scenes.Katherine.SuckleFromKath.PutsOnShow".Localized(
                        ClothesChoice("slips off her blouse", "wriggles out of the upper part of her bodysuit", "opens the bodice of her dress", "opens the top of her silk robe", "removes her tube top", "removes her nurse's top")
                    ));
                else
                    OutputText("Scenes.Katherine.SuckleFromKath.PullsOffBreastplate".Localized());

            if (Player.Corruption < 25)
            {
                OutputText("Scenes.Katherine.SuckleFromKath.GuideHandsAside".Localized());

                OutputText("Scenes.Katherine.SuckleFromKath.MassageMammaries".Localized(
                    breasts.Adj()
                ));

                if (breasts.Lactation == 1)
                    OutputText("Scenes.Katherine.SuckleFromKath.SuckingGently".Localized());
                else
                    if ((breasts.MilkFullness >= 100 && breasts.Lactation >= 4) || (breasts.MilkIsFull() && breasts.Lactation >= 6))
                    {
                        OutputText("Scenes.Katherine.SuckleFromKath.CoaxRelaxRelease".Localized());

                        if (breasts.Lactation >= 5)
                            OutputText("Scenes.Katherine.SuckleFromKath.UnableDrinkAll".Localized());
                    }
                    else
                        OutputText("Scenes.Katherine.SuckleFromKath.CoaxRelaxAgain".Localized());

                if (breasts.Lactation >= 2)
                    Player.RefillHunger(breasts.Lactation * 15);

                OutputText("Scenes.Katherine.SuckleFromKath.KathJustPurrs".Localized());
            }
            else
                if (Player.Corruption < 75)
                {
                    OutputText("Scenes.Katherine.SuckleFromKath.PushHandsAway".Localized());

                    OutputText("Scenes.Katherine.SuckleFromKath.StrokeMammaries".Localized(
                        breasts.Adj()
                    ));

                    if (breasts.Lactation == 1)
                        OutputText("Scenes.Katherine.SuckleFromKath.SuckBringBackCream".Localized());
                    else
                        if ((breasts.MilkFullness >= 100 && breasts.Lactation >= 4) || (breasts.MilkIsFull() && breasts.Lactation >= 6))
                        {
                            OutputText("Scenes.Katherine.SuckleFromKath.SuckDrawOut".Localized());

                            if (breasts.Lactation >= 5)
                                OutputText("Scenes.Katherine.SuckleFromKath.UnableDrinkAllAgain".Localized());
                        }
                        else
                            OutputText("Scenes.Katherine.SuckleFromKath.SuckDrawOutAgain".Localized());

                    if (breasts.Lactation >= 2)
                        Player.RefillHunger(breasts.Lactation * 15);

                    OutputText("Scenes.Katherine.SuckleFromKath.KathJustLets".Localized());
                }
                else
                {
                    OutputText("Scenes.Katherine.SuckleFromKath.GrabHandsPress".Localized());

                    if (breasts.Lactation == 1)
                    {
                        OutputText("Scenes.Katherine.SuckleFromKath.BodyStoppedCream".Localized());

                        OutputText("Scenes.Katherine.SuckleFromKath.KneadForceMilk".Localized(
                            breasts.Adj()
                        ));
                    }
                    else
                        if ((breasts.MilkFullness >= 100 && breasts.Lactation >= 4) || (breasts.MilkIsFull() && breasts.Lactation >= 6))
                        {
                            OutputText("Scenes.Katherine.SuckleFromKath.CupHeavyBreasts".Localized());

                            if (breasts.Lactation >= 5)
                                OutputText("Scenes.Katherine.SuckleFromKath.LargeCascadeCream".Localized());
                            else
                                OutputText("Scenes.Katherine.SuckleFromKath.DecentCreamStream".Localized());

                            OutputText("Scenes.Katherine.SuckleFromKath.OntoNippleFeast".Localized());

                            OutputText("Scenes.Katherine.SuckleFromKath.KneadIntoMouth".Localized(
                                breasts.Adj()
                            ));

                            if (breasts.Lactation >= 5)
                                OutputText("Scenes.Katherine.SuckleFromKath.PleasedByProduction".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Katherine.SuckleFromKath.CupFewDrops".Localized());

                            OutputText("Scenes.Katherine.SuckleFromKath.KneadIntoMouthAgain".Localized(
                                breasts.Adj()
                            ));
                        }

                    if (breasts.Lactation >= 2)
                        Player.RefillHunger(breasts.Lactation * 15);

                    OutputText("Scenes.Katherine.SuckleFromKath.SquirmsAndMewls".Localized());
                }

            var wasntLactating = breasts.Lactation == 1;

            if (breasts.Milked() && wasntLactating)
            {
                OutputText("Scenes.Katherine.SuckleFromKath.WindingDownTaste".Localized());

                OutputText("Scenes.Katherine.SuckleFromKath.DrowsyBlissfulTone".Localized());

                OutputText("Scenes.Katherine.SuckleFromKath.MakeSureNotDry".Localized(
                    breasts.Adj()
                ));
            }

            OutputText("Scenes.Katherine.SuckleFromKath.PullAwaySighs".Localized());

            if (breasts.Lactation == 1)
            {
                OutputText("Scenes.Katherine.SuckleFromKath.HopeStartMakingMilk".Localized());

                if (IsAt(KLOC_KATHS_APT))
                    OutputText("Scenes.Katherine.SuckleFromKath.HeadStartsDroop".Localized());
            }
            else
                if (breasts.MilkFullness >= 100)
                {
                    OutputText("Scenes.Katherine.SuckleFromKath.JustWhatNeeded".Localized());

                    if (IsAt(KLOC_BAR) || IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
                    {
                        if (Player.Corruption < 75)
                            OutputText("Scenes.Katherine.SuckleFromKath.HugCloseToCover".Localized());
                        else
                            OutputText("Scenes.Katherine.SuckleFromKath.LeaveBreastsExposed".Localized(
                                breasts.Adj()
                            ));

                        OutputText("Scenes.Katherine.SuckleFromKath.AfterLittleCatnap".Localized(
                            ClothesChoice("her bra and blouse back on", "on her bra and wriggles back into the bodysuit", "on her bra and does up the bodice of her dress", "her bra back on and closes up her robe", "her bra and tube top back on", "her bra and nurse's top back on")
                        ));
                    }
                    else
                        if (IsAt(KLOC_KATHS_APT))
                            OutputText("Scenes.Katherine.SuckleFromKath.ParagraphBreak".Localized());
                        else
                            OutputText("Scenes.Katherine.SuckleFromKath.NudgeHerAwake".Localized());
                }
                else
                    if (breasts.MilkIsFull())
                    {
                        OutputText("Scenes.Katherine.SuckleFromKath.JustWhatNeededAgain".Localized());

                        if (!IsAt(KLOC_KATHS_APT))
                            OutputText("Scenes.Katherine.SuckleFromKath.RemembersModesty".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Katherine.SuckleFromKath.JustWhatWanted".Localized());

                        if (IsAt(KLOC_KATHS_APT))
                            OutputText("Scenes.Katherine.SuckleFromKath.HeadStartsDroopAgain".Localized());
                    }

            if (IsAt(KLOC_BAR) || IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
            {
                if (View.Time.Hours >= 8 && View.Time.Hours <= 16 && CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) == 0)
                    OutputText("Scenes.Katherine.SuckleFromKath.NoticeRubbingOwn".Localized());

                OutputText("Scenes.Katherine.SuckleFromKath.PlacesToBe".Localized());
            }
            else
                if (IsAt(KLOC_KATHS_APT))
                    OutputText("Scenes.Katherine.SuckleFromKath.LoverUncurlsBed".Localized());
                else
                    OutputText("Scenes.Katherine.SuckleFromKath.SeesPatrolMember".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            ReturnToCamp(1);
        }

        public void SuckleTacularKats()
        {
            ClearOutput();

            ShowImage("katherine-suckles-you");

            OutputText("Scenes.Katherine.SuckleTacularKats.MilkyGoodnessOffer".Localized());

            OutputText("Scenes.Katherine.SuckleTacularKats.EyesWidenShock".Localized());

            if (IsAt(KLOC_BAR) || IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
            {
                OutputText("Scenes.Katherine.SuckleTacularKats.MovesToYourSide".Localized(
                    PlayerText()
                ));

                OutputText("Scenes.Katherine.SuckleTacularKats.PlaysWithNipples".Localized(
                    CatGirl("smooth fingers", "finger pads")
                ));

                OutputText("Scenes.Katherine.SuckleTacularKats.NotWaitingLonger".Localized());
            }
            else
            {
                OutputText("Scenes.Katherine.SuckleTacularKats.StridesOverImpatient".Localized());

                OutputText("Scenes.Katherine.SuckleTacularKats.BarelyAboveWhisper".Localized());

                if (Player.Body.Get<EyesPart>().Id == BodyPartTypes.FourSpiderEyes || Player.Body.Get<EyesPart>().Count > 2)
                    OutputText("Scenes.Katherine.SuckleTacularKats.AllOfWord".Localized());

                OutputText("Scenes.Katherine.SuckleTacularKats.YourEyesAnd".Localized());
            }

            OutputText("Scenes.Katherine.SuckleTacularKats.TakesHerCheeks".Localized());

            OutputText("Scenes.Katherine.SuckleTacularKats.SitListeningPurr".Localized());

            if (!Player.HasPerk(PerkIds.Feeder) && Player.LactationQ() < 1000)
                OutputText("Scenes.Katherine.SuckleTacularKats.EmptiesWord".Localized());
            else
                OutputText("Scenes.Katherine.SuckleTacularKats.BestToEmpty".Localized());

            OutputText("Scenes.Katherine.SuckleTacularKats.FirstBreastThenNext".Localized());

            if (!Player.HasPerk(PerkIds.Feeder) && Player.LactationQ() < 1000)
                OutputText("Scenes.Katherine.SuckleTacularKats.YouDryWord".Localized());
            else
                OutputText("Scenes.Katherine.SuckleTacularKats.InexhaustibleSupply".Localized());

            OutputText("Scenes.Katherine.SuckleTacularKats.LetsNippleGo".Localized());

            if (!(Player.LactationQ() < 500))
                if (Player.LactationQ() < 1000)
                    OutputText("Scenes.Katherine.SuckleTacularKats.MilkBellySporting".Localized());
                else
                    if (Player.LactationQ() < 2000)
                        OutputText("Scenes.Katherine.SuckleTacularKats.AlmostPregnantBelly".Localized());
                    else
                        OutputText("Scenes.Katherine.SuckleTacularKats.LooksLikeBalloon".Localized());

            OutputText("Scenes.Katherine.SuckleTacularKats.SmirkAsksFeels".Localized());

            OutputText("Scenes.Katherine.SuckleTacularKats.CatStirsCuddles".Localized());

            if (IsAt(KLOC_KATHS_APT))
                OutputText("Scenes.Katherine.SuckleTacularKats.BedroomWalls".Localized());
            else
                if (IsAt(KLOC_STREETS))
                    OutputText("Scenes.Katherine.SuckleTacularKats.AlleyWalls".Localized());
                else
                {
                    OutputText("Scenes.Katherine.SuckleTacularKats.BarWalls".Localized());

                    if (View.Time.Hours >= 8 && View.Time.Hours <= 16 && CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) == 0)
                        OutputText("Scenes.Katherine.SuckleTacularKats.NoticeRubbingEnvious".Localized());
                }

            OutputText("Scenes.Katherine.SuckleTacularKats.TellsUnabashed".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineMetScylla) != 0)
                OutputText("Scenes.Katherine.SuckleTacularKats.ExceptMaybeNun".Localized());

            OutputText("Scenes.Katherine.SuckleTacularKats.ExclaimSayEndBreak".Localized());

            OutputText("Scenes.Katherine.SuckleTacularKats.HelpWellFedCat".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Lust, -40f));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Player.Milked();

            ReturnToCamp(1);
        }

        public void AmbushByVagrantKittyKats()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.AmbushByVagrantKittyKats.GlimpseCatMorphs".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.GetHelp".Localized(), GetHelpVsKittyKats);

            AddButton(1, "Common.Button.Intervene".Localized(), InterveneWithKittyKats);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveKittyKatsLikeANeeeeeerrrrd);
        }

        public void GetHelpVsKittyKats()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.GetHelpVsKittyKats.ThreeAtOnceHard".Localized());

            OutputText("Scenes.Katherine.GetHelpVsKittyKats.RaceToIntersection".Localized());

            OutputText("Scenes.Katherine.GetHelpVsKittyKats.ExplainAndLead".Localized());

            OutputText("Scenes.Katherine.GetHelpVsKittyKats.GoodDeedPressOn".Localized());

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void InterveneWithKittyKats()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.InterveneWithKittyKats.ThreeAtOnceHard".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats.PassWreckedCrates".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats.FallToGround".Localized());

            InterveneWithKittyKats2();
        }

        public void InterveneWithKittyKats2()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.InterveneWithKittyKats2.BlinkEvenHurts".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats2.SoftVoiceCalls".Localized(
                Player.Mf("him", "her")
            ));

            OutputText("Scenes.Katherine.InterveneWithKittyKats2.RollsEyesTosses".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats2.AsksMilkAside".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats2.SaysWillSave".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats2.MutteringUnderBreath".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats2.HandsAlsoBound".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats2.RollingOtherSide".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats2.TailTwitchingNervous".Localized());

            InterveneWithKittyKats3();
        }

        public void InterveneWithKittyKats3()
        {
            ClearOutput();

            KatherineSprite();

            OutputText("Scenes.Katherine.InterveneWithKittyKats3.WhyWorriedMilk".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats3.SaysHarmInIt".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats3.NeedToEat".Localized());

            Player.Gems -= 25;

            OutputText("Scenes.Katherine.InterveneWithKittyKats3.WhyGuardingYou".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats3.WhyCatsDontTake".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats3.GangsStrippedBare".Localized());

            InterveneWithKittyKats4();
        }

        public void InterveneWithKittyKats4()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.InterveneWithKittyKats4.LieStillStaring".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats4.WhatDoYouMean".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats4.ShiftLactating".Localized());

            if (Player.BiggestBreastSize() < 3)
                OutputText("Scenes.Katherine.InterveneWithKittyKats4.BarelyHaveBreasts".Localized());
            else
                OutputText("Scenes.Katherine.InterveneWithKittyKats4.PressesChestPupils".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats4.FumbleWithArmor".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats4.NotMuchFighter".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Katherine.InterveneWithKittyKats4.BetweenYourKnees".Localized());
            else
                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Katherine.InterveneWithKittyKats4.CoilsWrappedRound".Localized());
                else
                    OutputText("Scenes.Katherine.InterveneWithKittyKats4.HoldingTightly".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats4.WitsAboutEscape".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats4.FreezesStaringEyes".Localized());

            InterveneWithKittyKats5();
        }

        public void InterveneWithKittyKats5()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.InterveneWithKittyKats5.BlurtsOutName".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats5.CriesTooLate".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats5.WhimpersHideCock".Localized());

            OutputText("Scenes.Katherine.InterveneWithKittyKats5.HelpMightPayOff".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Faerie.GladesScene.ButtonHelp".Localized(), HelpKathAfterInterruptu);

            AddButton(1, "Common.Button.Leave".Localized(), LeaveKathYouDontWantThatNastyHermCock);
        }

        public void LeaveKathYouDontWantThatNastyHermCock()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.LeaveKathYouDontWantThatNastyHermCock.BetterOffNotInvolved".Localized());

            OutputText("Scenes.Katherine.LeaveKathYouDontWantThatNastyHermCock.PuttingOutOfMind".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineRandomRecruitmentDisabled, 1);

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void HelpKathAfterInterruptu()
        {
            ClearOutput();

            if (Player.Corruption < 25)
                OutputText("Scenes.Katherine.HelpKathAfterInterruptu.MustDoSomething".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Katherine.HelpKathAfterInterruptu.NeedsFoodNow".Localized());
                else
                    OutputText("Scenes.Katherine.HelpKathAfterInterruptu.TakeHerHereNow".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUnlocked, -1);

            Player.Gems -= 10;

            OutputText("Scenes.Katherine.HelpKathAfterInterruptu.FreeLegsCollect".Localized());

            OutputText("Scenes.Katherine.HelpKathAfterInterruptu.EyesPopAtGems".Localized());

            OutputText("Scenes.Katherine.HelpKathAfterInterruptu.StrideOutAlley".Localized());

            ReturnToCamp(1);
        }

        public void LeaveKittyKatsLikeANeeeeeerrrrd()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.LeaveKittyKatsLikeANeeeeeerrrrd.BetterOffNotInvolved".Localized());

            OutputText("Scenes.Katherine.LeaveKittyKatsLikeANeeeeeerrrrd.PuttingOutOfMind".Localized());

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void RepeatAmbushKatherineRecruitMent()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.RepeatAmbushKatherineRecruitMent.FamiliarSight".Localized());

            OutputText("Scenes.Katherine.RepeatAmbushKatherineRecruitMent.CratesConcealCats".Localized());

            if (Player.Corruption < 75)
                OutputText("Scenes.Katherine.RepeatAmbushKatherineRecruitMent.HopeShesHere".Localized());
            else
            {
                if (Player.HasCock() && Player.HasVagina())
                    OutputText("Scenes.Katherine.RepeatAmbushKatherineRecruitMent.DickSwellsPussy".Localized());
                else
                    if (Player.HasCock())
                        OutputText("Scenes.Katherine.RepeatAmbushKatherineRecruitMent.DickSwells".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Katherine.RepeatAmbushKatherineRecruitMent.PussyMoistens".Localized());
                        else
                            OutputText("Scenes.Katherine.RepeatAmbushKatherineRecruitMent.SphincterTwitches".Localized());

                OutputText("Scenes.Katherine.RepeatAmbushKatherineRecruitMent.FlexibleBodyTits".Localized());
            }

            OutputText("Scenes.Katherine.RepeatAmbushKatherineRecruitMent.LessKeenWhacked".Localized());

            OutputText("Scenes.Katherine.RepeatAmbushKatherineRecruitMent.SkidShortOfCrates".Localized());

            OutputText("Scenes.Katherine.RepeatAmbushKatherineRecruitMent.NoWeaponComing".Localized());

            OutputText("Scenes.Katherine.RepeatAmbushKatherineRecruitMent.AskWhoInCharge".Localized());

            OutputText("Scenes.Katherine.RepeatAmbushKatherineRecruitMent.BargainOrDonate".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Bargain".Localized(), BargainForKittahKatPussah);

            AddButton(1, "Common.Button.Donate".Localized(), DonateToCatSlutsYouCatSlut);

            AddButton(2, "Common.Button.Threaten".Localized(), ThreatenCatSluts);
        }

        public void BargainForKittahKatPussah()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.BargainForKittahKatPussah.SexIsPrevalent".Localized());

            OutputText("Scenes.Katherine.BargainForKittahKatPussah.EvelynUnmoved".Localized());

            OutputText("Scenes.Katherine.BargainForKittahKatPussah.PointHoldGems".Localized());

            Player.Gems -= 25;

            OutputText("Scenes.Katherine.BargainForKittahKatPussah.EvelynConsidersGems".Localized());

            BargainForKittahKataPussah2();
        }

        public void BargainForKittahKataPussah2()
        {
            ClearOutput();

            KatherineSprite();

            OutputText("Scenes.Katherine.BargainForKittahKataPussah2.ShouldersSag".Localized());

            if (Player.Corruption < 25)
                OutputText("Scenes.Katherine.BargainForKittahKataPussah2.BreaksYourHeart".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Katherine.BargainForKittahKataPussah2.NeedsShoulderNow".Localized());
                else
                    OutputText("Scenes.Katherine.BargainForKittahKataPussah2.LongGamePromise".Localized());

            OutputText("Scenes.Katherine.BargainForKittahKataPussah2.TensesAtYourArm".Localized());

            OutputText("Scenes.Katherine.BargainForKittahKataPussah2.AskedLittleTime".Localized());

            OutputText("Scenes.Katherine.BargainForKittahKataPussah2.JumpsToFeet".Localized());

            OutputText("Scenes.Katherine.BargainForKittahKataPussah2.IllegalToTakeOut".Localized());

            SecondTalkWithKatherine();
        }

        public void DonateToCatSlutsYouCatSlut()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlut.KnowTheyreHungry".Localized());

            Player.Gems -= 25;

            OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlut.StepsTakesGems".Localized());

            OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlut.ThanksStranger".Localized());

            OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlut.AboutToPipeUp".Localized());

            OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlut.EvelynNodsAgrees".Localized());

            DonateToCatSlutsYouCatSlutPt2();
        }

        public void DonateToCatSlutsYouCatSlutPt2()
        {
            ClearOutput();

            KatherineSprite();

            OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlutPt2.LeansAgainstWall".Localized());

            OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlutPt2.HeadSinksNoMoney".Localized());

            OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlutPt2.WalkOverBuying".Localized());

            OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlutPt2.AlmostADate".Localized());

            OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlutPt2.IllegalToTakeOut".Localized());

            OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlutPt2.PouncesWrapping".Localized());

            if (Player.Corruption < 25)
                OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlutPt2.TellHerOfCourse".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlutPt2.SayYesWouldLove".Localized());
                else
                    OutputText("Scenes.Katherine.DonateToCatSlutsYouCatSlutPt2.LongGamePromise".Localized());

            SecondTalkWithKatherine();
        }

        public void ThreatenCatSluts()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.ThreatenCatSluts.CrossArmsTell".Localized());

            OutputText("Scenes.Katherine.ThreatenCatSluts.CatsSizingUp".Localized());

            OutputText("Scenes.Katherine.ThreatenCatSluts.KathWinks".Localized());

            if (Player.Corruption < 25)
                OutputText("Scenes.Katherine.ThreatenCatSluts.DesperateForFriend".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Katherine.ThreatenCatSluts.ShameSheIsLonely".Localized());
                else
                    OutputText("Scenes.Katherine.ThreatenCatSluts.TwistRoundFinger".Localized());

            OutputText("Scenes.Katherine.ThreatenCatSluts.SmileTakeHand".Localized());

            OutputText("Scenes.Katherine.ThreatenCatSluts.TriesToSpeak".Localized());

            OutputText("Scenes.Katherine.ThreatenCatSluts.IllegalToTakeOut".Localized());

            SecondTalkWithKatherine();
        }

        public void SecondTalkWithKatherine()
        {
            ClearOutput();

            KatherineSprite();

            OutputText("Scenes.Katherine.SecondTalkWithKatherine.ShortWalkRestaurant".Localized());

            Player.Gems -= 10;

            OutputText("Scenes.Katherine.SecondTalkWithKatherine.HopedTalkOverFood".Localized());

            OutputText("Scenes.Katherine.SecondTalkWithKatherine.SeeWhatSheMeans".Localized());

            OutputText("Scenes.Katherine.SecondTalkWithKatherine.PayBillQuickly".Localized());

            FirstTimeWithKatherineUTerribleScrub();
        }

        public void FirstTimeWithKatherineUTerribleScrub()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.AsksWhatsWrong".Localized());

            OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.SitBesideCompliment".Localized());

            OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.SaysPrettyWavesOff".Localized());

            if (Player.Corruption < 25)
            {
                OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.NotAProblem".Localized());

                OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.LeansSilentFirst".Localized());

                OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.AskTrustThenKiss".Localized());

                OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.HandToPants".Localized());

                OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.LieStillShow".Localized());

                OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.CockSlipsSheath".Localized());
            }
            else
                if (Player.Corruption < 75)
                {
                    OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.TraveledOutsideTelAdre".Localized());

                    OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.LeansSilentAgain".Localized());

                    OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.CanProveIt".Localized());

                    OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.NervousnessBoilsOver".Localized());

                    OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.ReachesDownBlocked".Localized());
                }
                else
                {
                    OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.WantHerNotFriend".Localized());

                    OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.CockSlidesOut".Localized());

                    OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.NipplesPokeThrough".Localized());

                    OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.BreakKissDemand".Localized());
                }

            OutputText("Scenes.Katherine.FirstTimeWithKatherineUTerribleScrub.QuietYes".Localized());

            InitFlags();

            HandjobbiesFurrDemCatFurries();
        }

        public void HandjobbiesFurrDemCatFurries()
        {
            ClearOutput();

            if (IsAt(KLOC_BAR) || IsAt(KLOC_BAR_DRUNK) || IsAt(KLOC_BAR_URTA_REFUSED))
            {
                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.TailTwitchesDrink".Localized());

                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.LeadToAlleyway".Localized());

                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.WorriedCaught".Localized());
            }

            OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.PullIntoLap".Localized());

            if (Player.Corruption < 25)
                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.CaressTenderBreasts".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.TeaseLovelyBreasts".Localized());
                else
                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.ManhandleKnockers".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) > 0 && breasts.MilkIsFull())
                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.FingersCreamOffered".Localized());

            OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.TailTrappedFlicks".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.CocksRise".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.TeasingSlit".Localized());
                else
                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.SeductiveManner".Localized());

            OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.NipplesRockHard".Localized(
                CatGirl("skin", "fur")
            ));

            if (!HasCock())
            {
                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.SpreadWetLips".Localized());

                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.HeatPussyDribbles".Localized());
            }
            else
                if (cockNumber > 1)
                {
                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.TipsHotShafts".Localized());

                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.HeatKnotsSwell".Localized(
                        KnotAdj()
                    ));
                }
                else
                {
                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.TipHotShaft".Localized());

                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.HeatKnotSwells".Localized(
                        KnotAdj()
                    ));
                }

            if (HasCock())
            {
                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.MoanFromLungs".Localized());

                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.HeadTiltsSeekMouth".Localized());

                if (cockNumber > 1)
                {
                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.WorksBothShafts".Localized(
                        CockAdj()
                    ));

                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.AimDicks".Localized());
                }
                else
                {
                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.WorksShaft".Localized(
                        CockAdj()
                    ));

                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.AimDick".Localized());
                }

                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.PullFingersRubWetness".Localized(
                    HasBalls() ? "her" + (noFur ? "" : " fuzzy") + " balls" : "her growing knot",

                    HasBalls() ? "that " + BallAdj() + " ballsack contract" : "her cock" + CockMultiple("", "s") + " twitch and harden"
                ));

                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.NotDoneYet".Localized());

                if (IsAt(KLOC_DESERT))
                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.SandWord".Localized());
                else
                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.Plaster".Localized());

                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.FingersBackClit".Localized(
                    CockMultiple("", "s")
                ));
            }
            else
                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.HeadTiltsAgain".Localized());

            OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.TurnsToLiquid".Localized());

            if (Player.Corruption < 25)
                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.HoldWhileRecovers".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.PlayBreastsWaiting".Localized());
                else
                {
                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.PlayWithTail".Localized());

                    if (HasCock())
                        OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.CollectWarmSpunk".Localized(
                            IsAt(KLOC_DESERT) ? CockMultiple(" along her cock", "s along her cocks") : " over the wall"
                        ));
                    else
                        OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.CollectFemcum".Localized());

                    OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.OfferTongueLicks".Localized());
                }

            if (IsAt(KLOC_DESERT))
                OutputText("Scenes.Katherine.HandjobbiesFurrDemCatFurries.TrainingOverPackUp".Localized());

            Orgasm();

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Player.ApplyStats(new DynStat(GameStats.Lust, 33f));

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) == -1)
                FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo();
            else
                ReturnToCamp(1);
        }

        public void FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.HelpToFeetHug".Localized());

            if (Player.Corruption < 25)
                OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.AssureSeeAgain".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.AssureSeeAgainComma".Localized());
                else
                    OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.DebateCrushSpirits".Localized());

            OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.BeamsNoAmbush".Localized());

            OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.AskAboutGang".Localized());

            OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.KicksStoneManaged".Localized());

            OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.AgreeMeetLater".Localized());

            OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.KnowYoullComeBack".Localized());

            OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.SmileAndTell".Localized());

            OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.SaysYourName".Localized());

            if (Player.ShortName == "Kath" || Player.ShortName == "Katherine" || Player.ShortName == "Helena" || Player.ShortName == "Evelyn" || Player.ShortName == "Oswald")
                OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.WellCoincidence".Localized());
            else
                OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.RollsForeignWord".Localized());

            OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.AmazingFirstDate".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUnlocked, 1);

            OutputText("Scenes.Katherine.FirstTimeWithKatherinePartTwoUltraChampionshipEditionHyperTurbo.NowBehindOswalds".Localized());

            ReturnToCamp(1);
        }

        public void BathTime()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.BathTime.DontWantDive".Localized(
                Player.HasTailInsteadOfLegs() ? "slither" : Player.Mf("walk", "strut")
            ));

            OutputText("Scenes.Katherine.BathTime.DisrobedEnterWater".Localized());

            OutputText("Scenes.Katherine.BathTime.CatsDislikeWater".Localized());

            OutputText("Scenes.Katherine.BathTime.KnockedIntoDrink".Localized());

            Player.HasGottenWashed();

            OutputText("Scenes.Katherine.BathTime.NipplesHarden".Localized());

            if (HasCock())
            {
                OutputText("Scenes.Katherine.BathTime.HerCock".Localized(
                    CockMultiple(" refuses to go soft - it grinds ", "s refuse to go soft - they grind ")
                ));

                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Katherine.BathTime.ScalyUnderbelly".Localized());
                else
                    OutputText("Scenes.Katherine.BathTime.BetweenLegs".Localized());

                OutputText("Scenes.Katherine.BathTime.RedHotPoker".Localized());
            }

            if (Player.IsTaur())
            {
                ClearButtons();

                if (Player.HasCock())
                    AddButton(0, "Common.Button.Ride".Localized(), BathTimeCentaurRide);
                else
                    AddButton(0, "Common.Button.Ride".Localized(), null);

                AddButton(1, "Common.Button.GetPenetrated".Localized(), BathTimeCentaurPenetrated);
            }
            else
            {
                ClearButtons();

                if (Player.HasCock())
                    AddButton(0, "Common.Button.Penetrate".Localized(), BathTimeFuckKath);
                else
                    AddButton(0, "Common.Button.Penetrate".Localized(), null);

                if (HasCock())
                    AddButton(1, "Common.Button.GetPenetrated".Localized(), BathTimePenetrated);
                else
                    AddButton(1, "Common.Button.GetPenetrated".Localized(), null);

                if (!HasCock() && !Player.HasCock())
                {
                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), BathTimeFrustrated);
                }
            }
        }

        public void BathTimeCentaurRide()
        {

            OutputText("Scenes.Katherine.BathTimeCentaurRide.HoldsYouGrinding".Localized(
                HasCock() ? "the " + CockMultiple("tip of her cock", "tips of her cocks") : "her pussy",

                HasCock() ? "cockhead" + CockMultiple(" keeps", "s keep") : "groin keeps"
            ));

            OutputText("Scenes.Katherine.BathTimeCentaurRide.HandOnCheekKiss".Localized());

            OutputText("Scenes.Katherine.BathTimeCentaurRide.FeelKaths".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.BathTimeCentaurRide.PrickAgainstBack".Localized(
                    CockAdj(),

                    CockMultiple(" ", "s "),

                    HasBalls() ? "and her " + BallAdj() + " balls" : ""
                ));
            else
                OutputText("Scenes.Katherine.BathTimeCentaurRide.PussyWetter".Localized());

            OutputText("Scenes.Katherine.BathTimeCentaurRide.CloserYourCockIgnored".Localized(
                HasCock() ? "cock" + CockMultiple("", "s") : "vulva"
            ));

            OutputText("Scenes.Katherine.BathTimeCentaurRide.FeetStrokeRapid".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.BathTimeCentaurRide.HerCockHotCum".Localized(
                    CockMultiple(" sprays a copious load", "s spray copious loads")
                ));
            else
                OutputText("Scenes.Katherine.BathTimeCentaurRide.ThighsTightenFemcum".Localized());

            OutputText("Scenes.Katherine.BathTimeCentaurRide.LegsPumpFuriously".Localized());

            OutputText("Scenes.Katherine.BathTimeCentaurRide.OnlyJustSucceeds".Localized());

            if (Player.CumQ() <= 500)
                OutputText("Scenes.Katherine.BathTimeCentaurRide.StandYourCockStreams".Localized(
                    CockMultiple(" fires", "s fire")
                ));
            else
                if (Player.CumQ() <= 1500)
                    OutputText("Scenes.Katherine.BathTimeCentaurRide.JetsIntoRiver".Localized());
                else
                    if (Player.CumQ() <= 3000)
                        OutputText("Scenes.Katherine.BathTimeCentaurRide.CumLaunchesForcefully".Localized());
                    else
                        OutputText("Scenes.Katherine.BathTimeCentaurRide.OverproductiveContractions".Localized());

            OutputText("Scenes.Katherine.BathTimeCentaurRide.RecoverWakeKatherine".Localized());

            if (!HasAlready(KBIT_TRIED_BATH))
                OutputText("Scenes.Katherine.BathTimeCentaurRide.EnjoyedTripBathsAgain".Localized());

            Player.Orgasm("Generic");

            Orgasm();

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Experience(KBIT_TRIED_BATH);

            ReturnToCamp(1);
        }

        public void BathTimeCentaurPenetrated()
        {

            OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.HoldsYouGrinding".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.HerCockSlidingThighs".Localized(
                    CockMultiple("", "s"),

                    CockMultiple("", "one of "),

                    CockAdj(),

                    CockMultiple("", "s")
                ));
            else
                OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.WaitsLegsTremble".Localized());

            if (Player.HasVagina())
                Player.CuntChange(HasCock() ? CockArea() : 32, true, true, false);
            else
                Player.ButtChange(HasCock() ? CockArea() : 32, true, true, false);

            OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.NoStoppingHer".Localized());

            OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.LostInLustSilent".Localized());

            if (HasCock())
            {
                OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.BellyExpandsSquirt".Localized(
                    HasBalls() ? BallAdj() + "testicles empty" : "bountiful prostate empties"
                ));

                if (Player.HasVagina() && Player.IsPregnant())
                    OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.SemenSealedCervix".Localized());
                else
                {
                    if (Player.HasVagina())
                        OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.SemenIntoWomb".Localized());
                    else
                        OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.SemenInflatesRectum".Localized());

                    if (CumQ() > 500)
                        OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.BellyPressedStreamBed".Localized());
                }

                if (cockNumber > 1)
                    OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.OtherCockReleases".Localized());
            }

            OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.GirlfriendsSensation".Localized());

            if (HasCock())
                OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.HotCockThrobbing".Localized());
            else
                OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.KnucklesGrindingAgainstThe".Localized());

            OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.TooMuchGasp".Localized());

            if (Player.HasCock())
                if (Player.CumQ() <= 500)
                    OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.OwnCockFewStreams".Localized(
                        CockMultiple(" fires", "s fire")
                    ));
                else
                    if (Player.CumQ() <= 1500)
                        OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.JetsFromOwnCock".Localized(
                            CockMultiple("", "s")
                        ));
                    else
                        if (Player.CumQ() <= 3000)
                            OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.CumLaunchesHard".Localized());
                        else
                            OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.OverproductiveContractions".Localized());

            if (HasCock())
            {
                OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.DepositsLoadExhausted".Localized());

                OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.UnwiseToStay".Localized());

                OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.NoHelpLocked".Localized());

                OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.KnotDeflates".Localized());
            }
            else
            {
                OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.ArmFromVag".Localized());

                OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.HaveToLaugh".Localized());

                OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.CollectClothesBounce".Localized());
            }

            if (!HasAlready(KBIT_TRIED_BATH))
                OutputText("Scenes.Katherine.BathTimeCentaurPenetrated.EnjoyedTripBathsAgain".Localized());

            Player.SlimeFeed();

            Player.Orgasm("Generic");

            Orgasm();

            Player.ChangeFatigue(15);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Experience(KBIT_TRIED_BATH);

            ReturnToCamp(1);
        }

        public void BathTimeFuckKath()
        {

            OutputText("Scenes.Katherine.BathTimeFuckKath.ProdsKathsBelly".Localized());

            OutputText("Scenes.Katherine.BathTimeFuckKath.TurnAroundBetweenLegs".Localized());

            if (Player.Corruption < 75)
                OutputText("Scenes.Katherine.BathTimeFuckKath.YouLeadForNow".Localized());
            else
                OutputText("Scenes.Katherine.BathTimeFuckKath.YouInCharge".Localized());

            OutputText("Scenes.Katherine.BathTimeFuckKath.StopsFightingChest".Localized(
                breasts.Adj()
            ));

            OutputText("Scenes.Katherine.BathTimeFuckKath.ColdAffectsShaft".Localized());

            OutputText("Scenes.Katherine.BathTimeFuckKath.FlexesAssCunt".Localized(
                CatGirl("", "feline ")
            ));

            if (Player.Corruption < 25)
                OutputText("Scenes.Katherine.BathTimeFuckKath.MateAndLover".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Katherine.BathTimeFuckKath.PartnerWord".Localized());
                else
                    OutputText("Scenes.Katherine.BathTimeFuckKath.FucktoyWord".Localized());

            OutputText("Scenes.Katherine.BathTimeFuckKath.CumsFromLoveCanal".Localized());

            OutputText("Scenes.Katherine.BathTimeFuckKath.HandsOnHead".Localized(
                CatGirl("skin", "black fur")
            ));

            OutputText("Scenes.Katherine.BathTimeFuckKath.MewlsDesperate".Localized());

            OutputText("Scenes.Katherine.BathTimeFuckKath.MusclesEntice".Localized());

            if (Player.Corruption < 25)
                OutputText("Scenes.Katherine.BathTimeFuckKath.SmileDrivenEdge".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Katherine.BathTimeFuckKath.LovelySound".Localized());
                else
                    OutputText("Scenes.Katherine.BathTimeFuckKath.SlutRightWhere".Localized());

            OutputText("Scenes.Katherine.BathTimeFuckKath.RutLikeAnimal".Localized());

            OutputText("Scenes.Katherine.BathTimeFuckKath.ThenFeelIt".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Katherine.BathTimeFuckKath.WarpedProstate".Localized());
            else
            {
                OutputText("Scenes.Katherine.BathTimeFuckKath.YourWord".Localized());

                if (Player.BallSize >= 18)
                    OutputText("Scenes.Katherine.BathTimeFuckKath.OversizedWord".Localized());
                else
                    if (Player.BallSize >= 12)
                        OutputText("Scenes.Katherine.BathTimeFuckKath.WatermelonSized".Localized());
                    else
                        if (Player.BallSize >= 5)
                            OutputText("Scenes.Katherine.BathTimeFuckKath.CantaloupeSized".Localized());

                OutputText("Scenes.Katherine.BathTimeFuckKath.BallsRetract".Localized());
            }

            OutputText("Scenes.Katherine.BathTimeFuckKath.FirstStreamIntoPussy".Localized());

            if (Player.CumQ() <= 500)
                OutputText("Scenes.Katherine.BathTimeFuckKath.MoreStreamsIntoPussy".Localized());
            else
                if (Player.CumQ() <= 1500)
                    OutputText("Scenes.Katherine.BathTimeFuckKath.JetsIntoKath".Localized());
                else
                {
                    if (Player.CumQ() > 3000)
                        OutputText("Scenes.Katherine.BathTimeFuckKath.YourOverproductive".Localized());

                    OutputText("Scenes.Katherine.BathTimeFuckKath.CumLaunchesHoldHips".Localized());

                    if (PregSize() > 0)
                        OutputText("Scenes.Katherine.BathTimeFuckKath.NowhereToGoCoats".Localized());
                    else
                        OutputText("Scenes.Katherine.BathTimeFuckKath.ForcesOpenFlesh".Localized());

                    OutputText("Scenes.Katherine.BathTimeFuckKath.MewlsFilledSeed".Localized());
                }

            OutputText("Scenes.Katherine.BathTimeFuckKath.TooColdToStay".Localized());

            if (PregSize() > 0)
                OutputText("Scenes.Katherine.BathTimeFuckKath.PregnantWord".Localized());
            else
                if (Player.CumQ() >= 1500)
                    OutputText("Scenes.Katherine.BathTimeFuckKath.CumStuffed".Localized());
                else
                    OutputText("Scenes.Katherine.BathTimeFuckKath.TautWord".Localized());

            OutputText("Scenes.Katherine.BathTimeFuckKath.BellyDeflatingCock".Localized());

            OutputText("Scenes.Katherine.BathTimeFuckKath.LongWalkAsksAgain".Localized());

            Player.Orgasm("Dick");

            Orgasm();

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Experience(KBIT_TRIED_BATH);

            ReturnToCamp(1);
        }

        public void BathTimeFrustrated()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.BathTimeFrustrated.HugsTightFingers".Localized(
                CatGirl("girl", "morph")
            ));

            OutputText("Scenes.Katherine.BathTimeFrustrated.GrindBreastsRace".Localized());

            OutputText("Scenes.Katherine.BathTimeFrustrated.SecondsBecomeMinutes".Localized());

            OutputText("Scenes.Katherine.BathTimeFrustrated.LustFadedFace".Localized());

            OutputText("Scenes.Katherine.BathTimeFrustrated.DragOutFrigid".Localized());

            OutputText("Scenes.Katherine.BathTimeFrustrated.SunMovedUnsatisfied".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(20 + Player.Libido / 20)));

            ReturnToCamp(1);
        }

        public void BathTimePenetrated()
        {

            if (!Player.HasVagina())
            {
                if (Player.HasCock())
                {
                    OutputText("Scenes.Katherine.BathTimePenetrated.StreaksOfPrecum".Localized(
                        CockMultiple("staff slides", "staves slide")
                    ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.Katherine.BathTimePenetrated.TeasingScrotumErect".Localized(
                            Player.HasTailInsteadOfLegs() ? "Common.Text.HerHandsSlideDownYourSide".Localized() : "Common.Text.SheSlidesHerCock".Localized() + CockMultiple("", "s") + " back and forth, rubbing " + CockMultiple("it", "them") + " against",

                            CatGirl("nails", "claws")
                        ));
                }
                else
                    OutputText("Scenes.Katherine.BathTimePenetrated.MaddenedByKath".Localized(
                        CockMultiple("staff", "staves")
                    ));

                OutputText("Scenes.Katherine.BathTimePenetrated.PurrsKissesTailWhips".Localized(
                    Player.HasTailInsteadOfLegs() ? "Common.Text.HerHandsSlideUpAndDown".Localized() : "Common.Text.SheShiftsHerHipsDrivingHer".Localized() + CockMultiple("", "s") + " between your thighs"
                ));

                OutputText("Scenes.Katherine.BathTimePenetrated.PressesAgainstYou".Localized());

                if (furry && !noFur)
                    OutputText("Scenes.Katherine.BathTimePenetrated.SlickFurRubbing".Localized());
                else
                    OutputText("Scenes.Katherine.BathTimePenetrated.WetSkin".Localized());

                OutputText("Scenes.Katherine.BathTimePenetrated.OverloadsSenses".Localized());

                OutputText("Scenes.Katherine.BathTimePenetrated.WaterDriftingPast".Localized());

                OutputText("Scenes.Katherine.BathTimePenetrated.OpenEyesKathHugs".Localized(
                    CockMultiple("that lovely hot cock of hers disappears", "those lovely hot cocks of hers disappear"),

                    CockMultiple("", "upper ")
                ));

                if (HasDogCock())
                {
                    OutputText("Scenes.Katherine.BathTimePenetrated.MovesHipsCircles".Localized(
                        CockMultiple("", "s"),

                        CockMultiple("her cock", "one of her cocks")
                    ));

                    if (cockNumber > 1)
                        OutputText("Scenes.Katherine.BathTimePenetrated.OtherKeepsGrinding".Localized());
                }
                else
                    OutputText("Scenes.Katherine.BathTimePenetrated.FeltYouRelax".Localized());

                OutputText("Scenes.Katherine.BathTimePenetrated.SentencePeriod".Localized());

                OutputText("Scenes.Katherine.BathTimePenetrated.RelaxedDoesntNotice".Localized());

                Player.ButtChange(CockArea(), true, true, false);

                OutputText("Scenes.Katherine.BathTimePenetrated.BegsNotToKnot".Localized());

                OutputText("Scenes.Katherine.BathTimePenetrated.HandsTeaseNipples".Localized());

                OutputText("Scenes.Katherine.BathTimePenetrated.DrawsBackToHead".Localized(
                    CockType("", "its tiny barbs teasing your rectum")
                ));

                OutputText("Scenes.Katherine.BathTimePenetrated.KnotPressesSphincter".Localized(
                    CatGirl("girl", "morph"),

                    CockMultiple("", "s")
                ));

                OutputText("Scenes.Katherine.BathTimePenetrated.SqueezeAsWithdraws".Localized());

                if (HasBalls())
                    OutputText("Scenes.Katherine.BathTimePenetrated.BallsWord".Localized(
                        BallAdj()
                    ));
                else
                    OutputText("Scenes.Katherine.BathTimePenetrated.OveractiveProstate".Localized());

                OutputText("Scenes.Katherine.BathTimePenetrated.TriesForceKnot".Localized());

                OutputText("Scenes.Katherine.BathTimePenetrated.ThrobbingEachLoad".Localized());

                if (CumQ() > 500)
                {
                    OutputText("Scenes.Katherine.BathTimePenetrated.BellyExpandingStretch".Localized());

                    if (CumQ() > 1500)
                        OutputText("Scenes.Katherine.BathTimePenetrated.PainfulTakesForever".Localized());
                }

                if (Player.HasCock())
                {
                    OutputText("Scenes.Katherine.BathTimePenetrated.ParagraphBreak".Localized());

                    if (Player.CumQ() <= 500)
                        OutputText("Scenes.Katherine.BathTimePenetrated.OwnCockFewStreams".Localized(
                            CockMultiple(" fires", "s fire")
                        ));
                    else
                        if (Player.CumQ() <= 1500)
                            OutputText("Scenes.Katherine.BathTimePenetrated.JetAfterJetErupts".Localized(
                                CockMultiple("", "s")
                            ));
                        else
                            if (Player.CumQ() <= 3000)
                                OutputText("Scenes.Katherine.BathTimePenetrated.CumLaunchesForcefully".Localized());
                            else
                                OutputText("Scenes.Katherine.BathTimePenetrated.OverproductiveNoAss".Localized());
                }

                OutputText("Scenes.Katherine.BathTimePenetrated.KnotCantForceIn".Localized());

                OutputText("Scenes.Katherine.BathTimePenetrated.NoKnotSlidesFree".Localized());

                OutputText("Scenes.Katherine.BathTimePenetrated.DragSexKittenBank".Localized());

                if (HasAlready(KBIT_TRIED_BATH))
                    OutputText("Scenes.Katherine.BathTimePenetrated.AlwaysUpForLake".Localized());
                else
                    OutputText("Scenes.Katherine.BathTimePenetrated.BathsAlwaysPopular".Localized());

                Player.SlimeFeed();

                Player.Orgasm("Dick");

                Orgasm();

                CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

                Experience(KBIT_TRIED_BATH);

                ReturnToCamp(1);
            }
            else
            {
                OutputText("Scenes.Katherine.BathTimePenetrated.RollHipsSinksIn".Localized(
                    CockMultiple("", "uppermost ")
                ));

                OutputText("Scenes.Katherine.BathTimePenetrated.LustyKittenEyes".Localized());

                Player.CuntChange(CockArea(), true, true, false);

                OutputText("Scenes.Katherine.BathTimePenetrated.CooperatingReward".Localized(
                    breasts.Adj()
                ));

                if (Player.HasCock())
                    OutputText("Scenes.Katherine.BathTimePenetrated.ColdStreamAffects".Localized());

                OutputText("Scenes.Katherine.BathTimePenetrated.TensesHipMuscles".Localized(
                    CockMultiple(" rocks and shifts", "s rock and shift"),

                    CockMultiple("her cock", "each of her cocks in turn"),

                    HasBalls() ? "cup her " + BallAdj() + " balls. Katherine sighs as you roll them between your fingers and gently tug them away from her body." : "slide your index and middle fingers inside her ass. You find her cum-stuffed prostate and begin to massage the hot bump. You swear you can almost feel it building up a larger load in response."
                ));

                OutputText("Scenes.Katherine.BathTimePenetrated.HandClaspsAround".Localized(
                    CockMultiple("Kath's heavy knot", "the heavy knot Kath has aimed at your pussy")
                ));

                OutputText("Scenes.Katherine.BathTimePenetrated.HandInTheWay".Localized(
                    CockAdj(),

                    PlayerText(),

                    CockMultiple(" is", "s are"),

                    CockMultiple(".", " and the stream.")
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) >= 4 && (Player.IsCorruptEnough(75) || (Player.HasPerk(PerkIds.Pervert) && Player.IsCorruptEnough(33)) || Player.HasPerk(PerkIds.Sadist) || DoneSubmissive(KBIT_SUB_ORGASM_DENIAL)))
                {
                    OutputText("Scenes.Katherine.BathTimePenetrated.LicksLipsPoorSlut".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.LetHerCum".Localized(), BathTimePenetratedNormalEnd);

                    AddButton(1, "Common.Button.Denial".Localized(), BathTimePenetratedDenial);
                }
                else
                    BathTimePenetratedNormalEnd();
            }
        }

        public void BathTimePenetratedNormalEnd()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.BathTimePenetratedNormalEnd.KathOnEdge".Localized());

            OutputText("Scenes.Katherine.BathTimePenetratedNormalEnd.IntoDilatedPupils".Localized(
                CockMultiple("", "s")
            ));

            OutputText("Scenes.Katherine.BathTimePenetratedNormalEnd.EyesRollBack".Localized());

            OutputText("Scenes.Katherine.BathTimePenetratedNormalEnd.KittenDoesntDisappoint".Localized(
                Player.IsPregnant() ? "Common.Text.ReboundingOffYouSealedCervixAnd".Localized() : "Common.Text.FillingYourWomb".Localized() + FertileText(" and seeking out your eggs", ""),

                CockMultiple(".", ". Her other dick coats your thighs in spunk as it squirts its seed into the stream.")
            ));

            if (CumQ() <= 500)
                OutputText("Scenes.Katherine.BathTimePenetratedNormalEnd.LimpAfterBlasts".Localized());
            else
                if (CumQ() > 500)
                    OutputText("Scenes.Katherine.BathTimePenetratedNormalEnd.LimpHeftyDeposit".Localized());
                else
                    if (CumQ() > 1500)
                        OutputText("Scenes.Katherine.BathTimePenetratedNormalEnd.BucketsIntoPussy".Localized(
                            CockMultiple("", "mixes with the stream from her free cock and ")
                        ));
                    else
                        if (CumQ() > 3000)
                            OutputText("Scenes.Katherine.BathTimePenetratedNormalEnd.HoldOnHighPressure".Localized(
                                Player.IsPregnant() ? "" : "Kath's huge load seems to fill your womb in an instant. Then it expands your belly still further, forcing the air from your lungs. In moments you look nine months pregnant and your belly button has become a hard nub that rubs against the " + CatGirl("skin", "fur") + " of Katherine's belly. ",

                                CockMultiple("", ", mixing with the stream from her free cock")
                            ));

            OutputText("Scenes.Katherine.BathTimePenetratedNormalEnd.CarrySpentToShore".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Katherine.BathTimePenetratedNormalEnd.FlopSandyGround".Localized());

            OutputText("Scenes.Katherine.BathTimePenetratedNormalEnd.PurrsHugsAsksLiked".Localized());

            OutputText("Scenes.Katherine.BathTimePenetratedNormalEnd.AssuresItWasGood".Localized());

            if (!HasAlready(KBIT_TRIED_BATH))
                OutputText("Scenes.Katherine.BathTimePenetratedNormalEnd.EnjoyedHerBath".Localized());

            Player.SlimeFeed();

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            Orgasm();

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Experience(KBIT_TRIED_BATH);

            ReturnToCamp(1);
        }

        public void BathTimePenetratedDenial()
        {
            ClearOutput();

            OutputText("Scenes.Katherine.BathTimePenetratedDenial.FingersSqueezeKnot".Localized());

            OutputText("Scenes.Katherine.BathTimePenetratedDenial.DrawAwaySlidesOut".Localized());

            OutputText("Scenes.Katherine.BathTimePenetratedDenial.LaughBeautifulSight".Localized());

            OutputText("Scenes.Katherine.BathTimePenetratedDenial.AllSheCanSay".Localized(
                CockMultiple(" recedes", "s recede"),

                CockMultiple(" starts", "s start")
            ));

            OutputText("Scenes.Katherine.BathTimePenetratedDenial.TakesHerShoulders".Localized());

            OutputText("Scenes.Katherine.BathTimePenetratedDenial.DragOutOfStream".Localized(
                HasBalls() ? "rolling her " + BallAdj() + " balls around with your hands. You think they're a little heavier than they were before. You keep rubbing" : "slipping your fingers back inside her ass. You think her prostate has become slightly heavier than it was before. You keep massaging it",

                CockMultiple("erection", "pair of erections")
            ));

            OutputText("Scenes.Katherine.BathTimePenetratedDenial.HoldsDownLustControl".Localized(
                ClothesLowerChoice("pull down her pants", "reach under her skirt", "slide your hand inside her bodysuit and free her cock" + CockMultiple("", "s"), "reach under her dress", "slip a hand inside her robe and free her cock" + CockMultiple("", "s")),

                CockMultiple("stroking her cock", "alternately stroking her cocks")
            ));

            OutputText("Scenes.Katherine.BathTimePenetratedDenial.WhinesPantsAttention".Localized(
                CockMultiple(" is", "s are"),

                CockMultiple("", "s")
            ));

            if (CumQ() <= 500)
                OutputText("Scenes.Katherine.BathTimePenetratedDenial.LimpSmallPuddle".Localized());
            else
                if (CumQ() <= 1500)
                    OutputText("Scenes.Katherine.BathTimePenetratedDenial.LimpOverduePayload".Localized(
                        CockMultiple("", "s")
                    ));
                else
                    if (CumQ() <= 3000)
                        OutputText("Scenes.Katherine.BathTimePenetratedDenial.BucketsOntoSand".Localized());
                    else
                        OutputText("Scenes.Katherine.BathTimePenetratedDenial.SpermKeepsFlowing".Localized(
                            CockMultiple(" in one continuous stream", "s in two continuous streams")
                        ));

            OutputText("Scenes.Katherine.BathTimePenetratedDenial.CarryHerToGates".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            Orgasm();

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            Experience(KBIT_TRIED_BATH);

            AddSubmissive(KBIT_SUB_ORGASM_DENIAL);

            ReturnToCamp(1);
        }

        public void BedroomBondage()
        {
            ClearOutput();

            if (DoneSubmissive(KBIT_SUB_BED_BOND))
            {
                OutputText("Scenes.Katherine.BedroomBondage.WardrobeFindGear".Localized());

                OutputText("Scenes.Katherine.BedroomBondage.LockArmPretends".Localized(
                    ClothesChoice("Hey! I know I was making eyes at you back at the bar, but what kind of girl do you think I am?", "Please - I'm just a dancer, I work in the club down the street!", "Oh no, a ruffian! Please don't hurt me, I'm just coming home from the ball.", "I'm warning you - I'm a sorceress from the academy and if you don't let go I'm going to use my magics on you.", "Hey! I know I was making eyes at you back at the bar, but what kind of girl do you think I am?", "No, please! I work down at the hospital, I take care of people!")
                ));
            }
            else
            {
                OutputText("Scenes.Katherine.BedroomBondage.QuickKissWaitHere".Localized());

                OutputText("Scenes.Katherine.BedroomBondage.ShopSilkScarves".Localized());

                Player.Gems -= 40;

                OutputText("Scenes.Katherine.BedroomBondage.EarsSwivelWhatsUp".Localized(
                    PlayerText()
                ));

                OutputText("Scenes.Katherine.BedroomBondage.SpinLockArms".Localized());

                OutputText("Scenes.Katherine.BedroomBondage.WhatsDoingSay".Localized(
                    PlayerText()
                ));

                OutputText("Scenes.Katherine.BedroomBondage.TiesWristsScarf".Localized());
            }

            OutputText("Scenes.Katherine.BedroomBondage.PushOntoBack".Localized(
                ClothesLowerChoice(". It doesn't take much effort to pull her pants and panties off", ", flip up her skirt and remove her panties", ", your fingers quickly locating the concealed slit in the crotch of her bodysuit", ", flip up her skirt and remove her panties", ", open the lower portion of her robe and relieve Kath of her panties"),

                CatGirl("girl", "morph"),

                CatGirl("from her skin", "through her fur")
            ));

            OutputText("Scenes.Katherine.BedroomBondage.AdmiresSpread".Localized(
                HasCock() ? ", her hard cock" + CockMultiple("", "s") + " aimed at the ceiling" : "",

                ClothesChoice("Inside her blouse", "Through her sheer bodysuit", "Beneath the embroidery of her elegant dress", "Inside the folds of her silken robe", "Concealed only by her tight tube top", "Concealed only by the skimpy white nurse's top")
            ));

            if (DoneSubmissive(KBIT_SUB_BED_BOND))
            {
                OutputText("Scenes.Katherine.BedroomBondage.GrinFavoriteGame".Localized());

                OutputText("Scenes.Katherine.BedroomBondage.StaysInCharacter".Localized(
                    ClothesChoice("Just cause I brought you home doesn't mean I put out on the first date. You seem like a nice " + Player.Mf("guy", "girl") + ", let's talk about this, huh?", "I just shake my ass for money [mister]. I'm not really that much of a slut. Please let me go, I'll... I'll give you free tickets to my next show!", "No, no, no. I wanted to meet a " + Player.Mf("gentleman", "lady") + ". A ruffian like you, you just can't be my first!", "Don't do this - I could teach you a spell to get off, I could pay for someone else to sleep with you, just don't - uh - fuck me.", "Just cause I brought you home doesn't mean I put out on the first date. You seem like a nice [guy], let's talk about this, huh?", "I know people say if you can't get a date get a nurse, but they still expect you to ask me out. I'm a really naughty nurse but I do expect a little romance.")
                ));
            }
            else
                OutputText("Scenes.Katherine.BedroomBondage.PlansToGiveFetish".Localized());

            OutputText("Scenes.Katherine.BedroomBondage.BlowOnHerEars".Localized(
                HasCock() ? "cock" + CockMultiple("", "s") : "clit"
            ));

            if (DoneSubmissive(KBIT_SUB_BED_BOND))
                OutputText("Scenes.Katherine.BedroomBondage.SayPlaceholder".Localized(
                    ClothesChoice("Oh " + Player.Mf("mister", "miss") + " - that feels good. I never sleep with someone on the first date but I think I'll make an exception for you.", "OK, OK! " + (Player.HasCock() && PregSize() == 0 ? "Common.Text.JustDontKnockMeUpIve".Localized() : "Common.Text.IGetSoHornySpinningAround".Localized() + (PregSize() == 0 ? " Just don't let any guys fuck me, Kay?" : "")), "Let me go! I was born to sleep with " + Player.Mf("lords and gentlemen", "baronesses and ladies") + ". There is no way a dirty ruffian like you - hah! - can make me feel this good.", "It's not fair, my magic just doesn't work when I'm this horny. I suppose I have no choice, I am at your mercy. So please help me get off. There won't be any reprisals if you make it good for me.", "Oh [mister] - that feels good. I never sleep with someone on the first date but I think I'll make an exception for you.", "Mmmm, you've got me. Okay hot stuff, let's see if you're as good at 'digital exams' as I am.")
                ));
            else
                OutputText("Scenes.Katherine.BedroomBondage.SighsPupilsDilated".Localized());

            OutputText("Scenes.Katherine.BedroomBondage.BarelyTouchTease".Localized(
                HasCock() ? "cock" + CockMultiple("", "s") + " or " : ""
            ));

            OutputText("Scenes.Katherine.BedroomBondage.BegsForMore".Localized(
                HasCock() ? "cock" + CockMultiple("", "s") : "clit"
            ));

            OutputText("Scenes.Katherine.BedroomBondage.EvenHarderFountain".Localized(
                HasCock() ? "cock" + CockMultiple("", "s") + " you see precum oozing from the tip" + CockMultiple("", "s") : "clit you see femcum oozing from her pussy",

                HasCock() ? "You could take her pulse by watching her knot" + CockMultiple("", "s") + " expanding and contracting. " : "",

                HasCock() ? "the base of her knot" + CockMultiple(" it inflates and hardens", "s they inflate and harden") : "her inner labia her legs start to shiver"
            ));

            OutputText("Scenes.Katherine.BedroomBondage.SlowsPaceWhisper".Localized(
                HasCock() ? "cock" + CockMultiple("", "s") + (HasBalls() ? ", her balls" : "") + " or her " : ""
            ));

            OutputText("Scenes.Katherine.BedroomBondage.BackToEars".Localized(
                HasCock() ? "With every heartbeat her cock" + CockMultiple("", "s") + " jump but she doesn't cum" : "Each time she takes a breath you hear her pussy dripping juice onto the sheets"
            ));

            if (breasts.Lactating())
                OutputText("Scenes.Katherine.BedroomBondage.SoakedInCream".Localized(
                    ClothesChoice("her blouse", "the upper part of her bodysuit", "the bodice of her dress", "the upper part of her robe", "her tube top", "her nurse's top")
                ));
            else
                OutputText("Scenes.Katherine.BedroomBondage.ByNowFragment".Localized());

            OutputText("Scenes.Katherine.BedroomBondage.GlazedTopToBottom".Localized(
                HasCock() ? "cock" + CockMultiple(" is", "s are") : "pussy is",

                HasCock() ? " " + CockMultiple("It continues to leak like a lazy volcano", "They continue to leak like lazy volcanoes") : ""
            ));

            OutputText("Scenes.Katherine.BedroomBondage.IfEarsWontDo".Localized(
                HasCock() ? "sound like a bucket of water being poured onto a wood floor. The stream" + CockMultiple(" she fires is so powerful it hits the ceiling and spatters", "s she fires are so powerful they hit the ceiling and spatter") + " all over the place like a cum sprinkler system" : "scream that starts at high pitched before passing out of the range of human hearing"
            ));

            OutputText("Scenes.Katherine.BedroomBondage.PantingLaughingCrazy".Localized(
                HasCock() ? "It takes several more shots to drain Katherine's " + (HasBalls() ? BallAdj() + " balls" : "unnatural prostate") + ". When it's over the whole apartment is soaked in cum" : "Kath goes through several more orgasms before she's drained of energy. When it's over the bedsheets are drenched"
            ));

            if (DoneSubmissive(KBIT_SUB_BED_BOND))
                OutputText("Scenes.Katherine.BedroomBondage.SayPlaceholderAgain".Localized(
                    ClothesChoice("Oh baby! No one's ever made me cum like that. Just give me a few minutes to recover. I want to pay you back, you're a keeper.", "That was great. You want a job? Cause if I add this to my act I'll get a lot more gems every show.", "You win, you rapscallion. You made your point, you've forced me to cum. If you untie me now all will be forgiven. I must admit you have some skill in the art of lovemaking.", "I did promise no reprisals if you made me feel good, didn't I? I guess you got what you wanted. Now let me go, I have to get cleaned up for tomorrow.", "Oh baby! No one's ever made me cum like that. Just give me a few minutes to recover. I want to pay you back, you're a keeper.", "Too bad it's over. Ever thought of going professional? You come to the hospital, we give you a cute little outfit like this. There are a lot of patients, doctors and nurses who could use some relief.")
                ));
            else
                OutputText("Scenes.Katherine.BedroomBondage.AmazingSoSpent".Localized(
                    PlayerText()
                ));

            var x = Player.CockThatFits(70);

            if (Player.HasCock() && x > -1)
            {
                var pcLen = (int)(Player.Cocks[x].Length);

                OutputText("Scenes.Katherine.BedroomBondage.AstrideAsksWhy".Localized());

                OutputText("Scenes.Katherine.BedroomBondage.LinesUpHerSlit".Localized());

                OutputText("Scenes.Katherine.BedroomBondage.HaventCumYet".Localized());

                if (PregSize() == 0)
                {
                    OutputText("Scenes.Katherine.BedroomBondage.FlatTummySeed".Localized());

                    if (DoneSubmissive(KBIT_SUB_BED_BOND))
                        OutputText("Scenes.Katherine.BedroomBondage.SayPlaceholderThird".Localized(
                            ClothesChoice("That's what you wanted all along, isn't it? You want to knock up a dumb slut you found at the bar. Well I can't stop you, so do your best stud!", "Fuck! You'll ruin everything. Everyone at the club is gonna see my big fat belly and I'll wind up serving drinks 'til I pop. But since I can't stop you I'm gonna enjoy this.", "Oh no! You're going to fuck a bastard into me. Oh, my life is going to be over, why does this have to feel so good?", "Mmmm, I guess my class is going to learn firsthand about pregnancy and babies. Come on then, put a little apprentice inside me!", "That's what you wanted all along, isn't it? You want to knock up a dumb slut you found at the bar. Well I can't stop you, so do your best stud!", "Oh yes! All the girls at the hospital will be soooo jealous. I can't wait for all the doctors to start fighting over my pregnant cunt. Unless you want to have me? Every day, every night - I don't care, just give me more of this!")
                        ));
                    else
                        OutputText("Scenes.Katherine.BedroomBondage.WhinesBegsInside".Localized());
                }
                else
                {
                    OutputText("Scenes.Katherine.BedroomBondage.PregnantBellyOften".Localized());

                    OutputText("Scenes.Katherine.BedroomBondage.HipsBuckDrool".Localized());

                    if (DoneSubmissive(KBIT_SUB_BED_BOND))
                        OutputText("Scenes.Katherine.BedroomBondage.NeedsSoMuchCum".Localized(
                            ClothesChoice("You know I do stud. I love getting knocked up and parading my big belly around town. I'm sure not going to bars for the drinks right now. Mommy", "No, no. This was an accident. I have to stay fit for my job at the club. But since I'm huge anyway please pump some more cum inside me. Maybe it'll help with the stretch marks. Mommy", "Ugh! This is a noble baby growing in my womb, but why am I wishing it was yours? Oh, I hope my child learns to fuck like you do and not like his father. Please don't stop, mommy", "Yes, I do. It's my duty to produce more adepts for the tower. You must find me when my womb is empty, I would like to carry your child. Now fill me, mommy", "You know I do stud. I love getting knocked up and parading my big belly around town. I'm sure not going to bars for the drinks right now. Mommy", "Mmmm, the doctors and the male nurses don't let us go very long without a bun in the oven. Hot little teases like me get fertility shots to make whole litters of New nurses! Don't you dare stop before you fill me, mommy")
                        ));
                    else
                        OutputText("Scenes.Katherine.BedroomBondage.DontStopNeedCum".Localized(
                            PlayerText()
                        ));
                }

                OutputText("Scenes.Katherine.BedroomBondage.HammerIntoHer".Localized());

                OutputText("Scenes.Katherine.BedroomBondage.ForceAsDeepAs".Localized());

                if (Player.CumQ() <= 500)
                    OutputText("Scenes.Katherine.BedroomBondage.SmallPoolTip".Localized());
                else
                    if (Player.CumQ() <= 1500)
                        OutputText("Scenes.Katherine.BedroomBondage.SpermPoolsInside".Localized());
                    else
                        if (Player.CumQ() <= 3000)
                        {
                            OutputText("Scenes.Katherine.BedroomBondage.SeedFlowsLikeRiver".Localized());

                            if (PregSize() == 0)
                            {
                                OutputText("Scenes.Katherine.BedroomBondage.CervixForcedOpen".Localized());

                                if (pcLen < 15)
                                    OutputText("Scenes.Katherine.BedroomBondage.PressureLessens".Localized());
                                else
                                    OutputText("Scenes.Katherine.BedroomBondage.SinkDeeperInto".Localized(
                                        pcLen == 15 ? "Common.Text.Tip".Localized() : "Common.Text.First".Localized() + (pcLen - 15) + " inches"
                                    ));

                                OutputText("Scenes.Katherine.BedroomBondage.BellyGrowsLarger".Localized());
                            }
                            else
                                OutputText("Scenes.Katherine.BedroomBondage.WombFilledVagina".Localized(
                                    CockMultiple("", "s")
                                ));

                            OutputText("Scenes.Katherine.BedroomBondage.CumFlowsOntoBed".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Katherine.BedroomBondage.MagicInjectsAmount".Localized(
                                CatGirl("girl", "morph")
                            ));

                            if (PregSize() == 0)
                            {
                                OutputText("Scenes.Katherine.BedroomBondage.InstantCervixOpen".Localized());

                                if (pcLen < 15)
                                    OutputText("Scenes.Katherine.BedroomBondage.SeedUnimpededInto".Localized());
                                else
                                    OutputText("Scenes.Katherine.BedroomBondage.SinksBitFurther".Localized(
                                        pcLen == 15 ? "Common.Text.Tip".Localized() : "Common.Text.First".Localized() + (pcLen - 15) + " inches"
                                    ));

                                OutputText("Scenes.Katherine.BedroomBondage.BellyGrowsAgain".Localized());

                                OutputText("Scenes.Katherine.BedroomBondage.HigherPressureBlasts".Localized());

                                if (Player.Corruption < 25)
                                    OutputText("Scenes.Katherine.BedroomBondage.HopeNoDamage".Localized());
                                else
                                    if (Player.Corruption < 75)
                                        OutputText("Scenes.Katherine.BedroomBondage.HopeSheEnjoys".Localized());
                                    else
                                        OutputText("Scenes.Katherine.BedroomBondage.SecretlyStretchBelly".Localized());

                                OutputText("Scenes.Katherine.BedroomBondage.WhimpersTailWagging".Localized());
                            }
                            else
                                OutputText("Scenes.Katherine.BedroomBondage.BirthCanalBalloons".Localized());
                        }

                OutputText("Scenes.Katherine.BedroomBondage.CollapseTopOfHer".Localized(
                    CatGirl("girl", "morph")
                ));

                OutputText("Scenes.Katherine.BedroomBondage.LieThereEnjoyed".Localized(
                    HasCock() ? "realize there's a " + CockMultiple(" hot, hard bulge", "pair of hot, hard bulges") + " between the two of you. " : "notice that Kath's nipples are rock hard once again. ",

                    HasCock() ? "stroke her cock" + CockMultiple(" and her knot", "s and knots") : "massage her throbbing clit"
                ));

                OutputText("Scenes.Katherine.BedroomBondage.RealizesBegsWeakly".Localized());

                if (Player.Corruption < 25)
                    OutputText("Scenes.Katherine.BedroomBondage.ConsiderKnowsLoves".Localized());
                else
                    if (Player.Corruption < 75)
                        OutputText("Scenes.Katherine.BedroomBondage.OfCourseSheCan".Localized());
                    else
                        OutputText("Scenes.Katherine.BedroomBondage.HowThinksThisWorks".Localized(
                            HasCock() ? "cock" + CockMultiple("", "s") + " in hand" : "clit between your fingers",

                            HasCock() ? "jerk her cock" + CockMultiple("", "s") : "work her clit"
                        ));

                OutputText("Scenes.Katherine.BedroomBondage.MoansTriesEscape".Localized(
                    HasCock() ? "aim her cock" + CockMultiple("", "s") + " at the ceiling and once again she gets coated in a shower of her own cum" : "lean in and give her a long deep kiss"
                ));

                OutputText("Scenes.Katherine.BedroomBondage.MarathonStartsSnore".Localized());
            }
            else
                if (!HasCock())
                    OutputText("Scenes.Katherine.BedroomBondage.StrokeHairReassure".Localized());
                else
                {
                    var dPen = Player.HasVagina() && cockNumber > 1;

                    OutputText("Scenes.Katherine.BedroomBondage.AstrideAsksWhyAgain".Localized());

                    OutputText("Scenes.Katherine.BedroomBondage.LowerOntoHer".Localized());

                    if (DoneSubmissive(KBIT_SUB_BED_BOND))
                        OutputText("Scenes.Katherine.BedroomBondage.SayPlaceholderFourth".Localized(
                            ClothesChoice("Oh " + Player.Mf("mister", "miss") + ", I'm never going to have to pick up at that bar again. It's love at first fuck but please, no more.", "No! I can't do it again! I'll dance for you all the time but don't do this or I won't be able to walk straight for a week.", "Ugh, untie me rogue! I already admitted I enjoyed this - how much more must I suffer? My rod" + CockMultiple("", "s") + " will not remain stiff for you long.", "Twice? No " + Player.Mf("mister", "miss") + ", please - we can meet again, I would like that, but I have a class to teach in the morning.", "Oh [mister], I'm never going to have to pick up at that bar again. It's love at first fuck but please, no more.", "I'm really tired, could we maybe have round two tomorrow? Please, pretty please? I'm a medical professional and I can tell you cocks aren't supposed to cum again so soon.")
                        ));
                    else
                        OutputText("Scenes.Katherine.BedroomBondage.DontNotSoSoon".Localized(
                            PlayerText()
                        ));

                    OutputText("Scenes.Katherine.BedroomBondage.KissEndsBabbling".Localized(
                        dPen ? "holes to swallow her cocks" : (Player.HasVagina() ? "Common.Text.Pussy".Localized() : "Common.Text.Rectum".Localized()) + " to swallow up her cock"
                    ));

                    OutputText("Scenes.Katherine.BedroomBondage.RocksForwardBack".Localized(
                        ClothesChoice("blouse", "bodysuit", "dress", "robe", "tube top", "nurse's uniform"),

                        breasts.Adj()
                    ));

                    OutputText("Scenes.Katherine.BedroomBondage.ForgetsComplaints".Localized());

                    OutputText("Scenes.Katherine.BedroomBondage.FamiliarThrobbing".Localized(
                        HasBalls() ? "slide your hand upwards and take hold of her " + BallAdj() + " balls. They're" : "push your fingers past her sphincter and start to massage her prostate. It's"
                    ));

                    OutputText("Scenes.Katherine.BedroomBondage.FingersMassageAsks".Localized());

                    OutputText("Scenes.Katherine.BedroomBondage.BodyStiffensIntense".Localized());

                    Orgasm();

                    OutputText("Scenes.Katherine.BedroomBondage.BellySwellsRedHot".Localized());

                    OutputText("Scenes.Katherine.BedroomBondage.RubsBellyMumbles".Localized());

                    OutputText("Scenes.Katherine.BedroomBondage.GigglesHugTired".Localized());

                    Player.SlimeFeed();
                }

            Player.Orgasm("Generic");

            Orgasm();

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            AddSubmissive(KBIT_SUB_BED_BOND);

            ReturnToCamp(1);
        }

        public void DrunkFuck()
        {

            OutputText("Scenes.Katherine.DrunkFuck.WaitressLastDrink".Localized(
                ClothesLowerChoice("tent in her pants", "bulge under her skirt", "bulge in the crotch of her bodysuit", "bulge under her dress", "bulge inside her robe")
            ));

            OutputText("Scenes.Katherine.DrunkFuck.PullsYouTowardBar".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.NotGentleHotter".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Katherine.DrunkFuck.AttacksClitJuices".Localized());
            else
                if (Player.HasCock())
                    OutputText("Scenes.Katherine.DrunkFuck.MilksProstateCum".Localized());
                else
                    OutputText("Scenes.Katherine.DrunkFuck.WholeHandInside".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.SentencePeriod".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.TroubleStanding".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Katherine.DrunkFuck.YankFromAss".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.PullAsidePanties".Localized(
                ClothesLowerChoice("undo her pants", "raise her skirt", "pull open the hidden slit in her bodysuit", "raise her dress", "open her robe")
            ));

            var dPen = cockNumber > 1 && Player.HasVagina();

            if (dPen)
                OutputText("Scenes.Katherine.DrunkFuck.CuntAndAssGet".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Katherine.DrunkFuck.CuntGets".Localized());
                else
                    OutputText("Scenes.Katherine.DrunkFuck.AssGets".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.SplitWideOpen".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.NeverSeenAggressive".Localized());

            if (dPen)
                OutputText("Scenes.Katherine.DrunkFuck.PussyAssMaking".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Katherine.DrunkFuck.PussyMakingIt".Localized());
                else
                    OutputText("Scenes.Katherine.DrunkFuck.AssMakingIt".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.BangsAgainstDoor".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Katherine.DrunkFuck.BatteringOvercomes".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Katherine.DrunkFuck.ThrustingOvercomes".Localized());
                else
                    OutputText("Scenes.Katherine.DrunkFuck.RectumNervesFire".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.SentencePeriodAgain".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.LaughsLikeMadwoman".Localized());

            if (dPen)
                OutputText("Scenes.Katherine.DrunkFuck.PussyRectumRipple".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Katherine.DrunkFuck.PussyRipples".Localized());
                else
                    OutputText("Scenes.Katherine.DrunkFuck.AssRipples".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.AlongHerShaft".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.WhisperMyTurn".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Katherine.DrunkFuck.Pussy".Localized());
            else
                OutputText("Scenes.Katherine.DrunkFuck.ColonWord".Localized());
            // 原版 Katherine.hx:7042：仅当「不是双插」且 Katherine 有多根屌时才输出这句
            // dPen = player.hasVagina() && cockNumber > 1（原版 Katherine.hx:6956 的局部变量）
            if (!(Player.HasVagina() && cockNumber > 1) && cockNumber > 1)
                OutputText("Scenes.Katherine.DrunkFuck.HerOtherMemberNestled".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.LongHardBlasts".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.WorriedVoiceBehind".Localized(
                PlayerText()
            ));

            if (Player.Corruption < 25)
                OutputText("Scenes.Katherine.DrunkFuck.KnowsYourFault".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Katherine.DrunkFuck.EasierTimeMad".Localized(
                        CatGirl("girl", "morph")
                    ));
                else
                    OutputText("Scenes.Katherine.DrunkFuck.FuckingGoodDominated".Localized());

            OutputText("Scenes.Katherine.DrunkFuck.KnotFreeGetUp".Localized());

            if (Player.Corruption < 25)
                OutputText("Scenes.Katherine.DrunkFuck.BlowKissCatCalls".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Katherine.DrunkFuck.TakeBowKiss".Localized());
                else
                {
                    OutputText("Scenes.Katherine.DrunkFuck.EvilGrinStrideBack".Localized(
                        PlayerText()
                    ));

                    OutputText("Scenes.Katherine.DrunkFuck.ArmsUnderHers".Localized(
                        ClothesChoice("start to unbutton her blouse, exposing her bra", "begin to play with her breasts right through her sheer bodysuit. Her nipples harden and stick out of the stretchy fabric", "pull down the front of her ballroom dress, exposing her bra", "pull the top of her robe open, exposing her bra", "lift her tube top up, exposing her bra", "undo the few buttons on the sexy nurse's top, exposing her bra")
                    ));

                    OutputText("Scenes.Katherine.DrunkFuck.StiffTransfixedStares".Localized());

                    OutputText("Scenes.Katherine.DrunkFuck.SqueaksNoNoNo".Localized());

                    if (Submissiveness() < 6)
                    {
                        OutputText("Scenes.Katherine.DrunkFuck.MinutesThenLetsGo".Localized());

                        OutputText("Scenes.Katherine.DrunkFuck.QuickKissLeavesBar".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Katherine.DrunkFuck.IgnoresOfCourse".Localized());

                        OutputText("Scenes.Katherine.DrunkFuck.StepBackWontDo".Localized());

                        OutputText("Scenes.Katherine.DrunkFuck.HootsAndHollers".Localized());

                        OutputText("Scenes.Katherine.DrunkFuck.LooksBackWondering".Localized());

                        OutputText("Scenes.Katherine.DrunkFuck.GulpsTurnsToCrowd".Localized(
                            ClothesChoice("starts unbuttoning her blouse slowly", "starts unlacing her bodysuit", "starts unlacing her long dress", "starts untying the knot of her robe", "starts running her fingers under the bottom edge of her tube top", "pulls the front of her nurse's top open again")
                        ));

                        OutputText("Scenes.Katherine.DrunkFuck.SmileWatchingDance".Localized(
                            ClothesChoice("pulls off her blouse and starts to play with her breasts. After a few cheers and cries of [say: More!] she removes the bra as well", "pulls her shoulders out of the unlaced top of her bodysuit. Without missing a beat she gets her arms free and pushes the silky fabric down until the bodysuit covers only her legs, ass and belly", "finishes unlacing the dress and pulls her arms free, letting the top of the dress fall away. She starts to play with her breasts and after a few cheers and cries of [say: More!] she removes the bra as well", "undoes the knot and opens the front of her robe, giving the audience a lovely view of her bra and panties", "pulls the tube top over her head and starts to play with her breasts. After a few cheers and cries of [say: More!] she removes the bra as well", "slips out of the nurse's top and starts to play with her breasts. After a few cheers and cries of [say: More!] she removes the bra as well")
                        ));

                        OutputText("Scenes.Katherine.DrunkFuck.EveryEyeLocked".Localized(
                            ClothesChoice(", loosens her skirt and slowly slides it over her hips. She poses before kicking off her panties, leaving herself totally naked in front of the crowd", "and works the bodysuit down over her hips, leaving herself totally naked in front of the crowd", "and wiggles her body, working herself out of the dress and leaving herself totally naked in front of the crowd", "and allows the spider silk robe to slide off her shoulders and drop to the ground. With a twirl she pulls off her bra and slides off her panties, leaving herself totally naked in front of the crowd", ClothesLowerChoice("and slides the skintight shorts down her legs. Her panties soon follow, ", ", loosens her skirt and slowly slides it over her hips. She poses before kicking off her panties,", "", "", "") + " leaving herself totally naked in front of the crowd", "and unfastens the sexy nurse's skirt, letting it drop to the floor. She poses for the crowd, naked save for the little white nurse's cap resting on her head")
                        ));

                        OutputText("Scenes.Katherine.DrunkFuck.SneaksUpBehind".Localized(
                            CockMultiple("her cock with both hands", "both her cocks")
                        ));

                        OutputText("Scenes.Katherine.DrunkFuck.CrowdCheersSmear".Localized(
                            CockMultiple("", "s")
                        ));

                        if (IsValaAtBar())
                            OutputText("Scenes.Katherine.DrunkFuck.ValaPutsTrayDown".Localized(
                                CatGirl("cat girl.", "feline lover.")
                            ));

                        OutputText("Scenes.Katherine.DrunkFuck.PlaysWithBreasts".Localized());

                        OutputText("Scenes.Katherine.DrunkFuck.PutsOnShow".Localized(
                            CockMultiple("", "s"),

                            CockMultiple("that knot", "those knots"),

                            CockMultiple("her shaft toward the ceiling. When Kath cums a thick stream of semen under high pressure blasts from the tip of her cock, striking the wall above the door and raining down in thick, heavy droplets", "her upper cock to the left and her lower cock to the right. When Kath cums twin streams of semen under high pressure blast from the tips of her cocks - one striking the wall on the left of the door, one striking the wall on the right"),

                            CockMultiple("", "s")
                        ));

                        OutputText("Scenes.Katherine.DrunkFuck.EveryoneAppreciatedShow".Localized());

                        OutputText("Scenes.Katherine.DrunkFuck.StridesOutHome".Localized());

                        AddSubmissive(KBIT_SUB_PUBLIC_EXHIBITION);
                    }
                }

            Player.SlimeFeed();

            Player.Orgasm("Generic");

            Orgasm();

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) + 1);

            ReturnToCamp(1);
        }

    }
}