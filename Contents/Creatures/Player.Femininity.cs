using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Creatures
{
    public partial class Player
    {
        // 原版 Character.hx:58 faceDesc()：按女性化程度返回面部长描述
        public virtual string FaceDesc()
        {
            if (Femininity < 10)
                return "PlayerEvents.FaceDesc.SquareChin".Localized(HasBeard() ? ", chiseled jawline, and beard" : " and chiseled jawline");

            if (Femininity < 20)
                return "PlayerEvents.FaceDesc.Rugged".Localized(FaceDescript(), HasBeard() ? "and beard" : "");

            if (Femininity < 28)
                return "PlayerEvents.FaceDesc.MasculineProfile".Localized();

            if (Femininity < 35)
                return "PlayerEvents.FaceDesc.AngularJaw".Localized();

            if (Femininity < 45)
                return "PlayerEvents.FaceDesc.BarelyMasculine".Localized();

            if (Femininity <= 55)
                return "PlayerEvents.FaceDesc.Androgynous".Localized();

            if (Femininity <= 65)
                return "PlayerEvents.FaceDesc.GentleCurves".Localized();

            if (Femininity <= 72)
                return "PlayerEvents.FaceDesc.Cheekbones".Localized();

            if (Femininity <= 80)
                return "PlayerEvents.FaceDesc.FeminineShapeliness".Localized();

            if (Femininity <= 90)
                return "PlayerEvents.FaceDesc.GorgeousProfile".Localized();

            return "PlayerEvents.FaceDesc.JawDropping".Localized();
        }

        // 原版 Character.hx:211 femininityHigh()
        public virtual int FemininityHigh() => Gender switch
        {
            Gender.Genderless => 85,

            Gender.Hermaphrodite => 85,

            Gender.Female => 100,

            Gender.Male => 71,

            _ => 100,
        };

        // 原版 Character.hx:221 femininityLow()
        public virtual int FemininityLow() => Gender switch
        {
            Gender.Genderless => 20,

            Gender.Hermaphrodite => 20,

            Gender.Female => 30,

            Gender.Male => 0,

            _ => 0,
        };

        // 原版 Character.hx:232 fixFemininity()
        public virtual string FixFemininity()
        {
            if (Femininity > FemininityHigh())
            {
                Femininity = FemininityHigh();

                return "PlayerEvents.FixFemininity.TooFeminine".Localized();
            }

            if (Femininity < FemininityLow())
            {
                Femininity = FemininityLow();

                return "PlayerEvents.FixFemininity.TooMasculine".Localized();
            }

            return "";
        }

        // 原版 Character.hx:109 modFem(goal, strength)：向目标值逐步调整并返回面部变化文案
        public virtual string ModFemGoal(float goal, float strength = 1f)
        {
            var oldFace = FaceDesc();

            var oldFem = Femininity;

            var changed = false;

            if (goal == Femininity)
                return "";

            if (goal < Femininity && goal <= 50)
            {
                Femininity -= strength;

                if (Femininity < goal)
                    Femininity = goal;

                changed = true;
            }

            if (goal > Femininity && goal >= 50)
            {
                Femininity += strength;

                if (Femininity > goal)
                    Femininity = goal;

                changed = true;
            }

            if (!HasPerk(PerkIds.Androgyny))
                FixFemininity();

            if (!changed)
                return "";

            if (oldFace != FaceDesc())
                return goal > oldFem

                    ? "PlayerEvents.ModFem.Soften".Localized(strength)

                    : "PlayerEvents.ModFem.Harden".Localized(strength);

            return goal > oldFem

                ? "PlayerEvents.ModFem.TingleFeminine".Localized(FaceDescript(), strength)

                : "PlayerEvents.ModFem.TingleMasculine".Localized(FaceDescript(), strength);
        }
    }
}