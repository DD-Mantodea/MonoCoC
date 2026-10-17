using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Utils;
using System;

namespace MonoCoC.Contents.Creatures
{
    public partial class Player
    {
        public static void Space()
        {
            if (Scene.View != null)
                Scene.OutputText(" ");
        }

        public bool CuntChange(float cArea, bool display, bool spacingsF = false, bool spacingsB = true)
        {
            if (Vaginas.Count == 0)
                return false;

            var wasVirgin = Vaginas[0].Virgin;

            var stretched = CuntChangeNoDisplay(cArea);

            var devirgined = wasVirgin && !Vaginas[0].Virgin;

            if (devirgined)
            {
                LostVirginity = true;

                if (spacingsF)
                    Space();

                Write("Sex.CuntChange.VirginityLost".Localized());

                if (spacingsB)
                    Space();
            }

            if (display && stretched)
            {
                if (devirgined)
                {
                    if (!spacingsB)
                        Space();
                }
                else if (spacingsF)
                    Space();

                var looseness = Vaginas[0].Looseness;

                Write(looseness switch
                {
                    5 => "Sex.CuntChange.ClownCar",

                    4 => "Sex.CuntChange.GapingWide",

                    3 => "Sex.CuntChange.Gaping",

                    2 => "Sex.CuntChange.Loose",

                    1 => "Sex.CuntChange.Normal",

                    _ => "Sex.CuntChange.Tight"
                }, VaginaDescript(0));

                if (spacingsB)
                    Space();
            }

            return stretched;
        }

        public bool CuntChangeNoDisplay(float cArea)
        {
            if (Vaginas.Count == 0)
                return false;

            var stretched = Vaginas[0].Stretch(cArea, HasPerk(PerkIds.FerasBoonMilkingTwat), VaginalCapacityBonus());

            if (cArea >= 0.5f * VaginalCapacity())
                Vaginas[0].ResetRecoveryProgress();

            return stretched;
        }

        public bool ButtChange(float cArea, bool display, bool spacingsF = true, bool spacingsB = true)
        {
            var stretched = ButtChangeNoDisplay(cArea);

            if (stretched && display)
            {
                if (spacingsF)
                    Space();

                ButtChangeDisplay();

                if (spacingsB)
                    Space();
            }

            return stretched;
        }

        public bool ButtChangeNoDisplay(float cArea)
        {
            var stretched = false;

            if (cArea >= AnalCapacity() && Random.Shared.Next(2) == 0)
            {
                Anal.Looseness += 1;

                stretched = true;

                var butt = StatusEffectByType(StatusEffectIds.ButtStretched);

                butt?.Values[0] = 0;
            }

            if (cArea < AnalCapacity() && cArea >= 0.9f * AnalCapacity() && Random.Shared.Next(4) == 0)
            {
                Anal.Looseness += 1;

                stretched = true;
            }

            if (cArea < 0.9f * AnalCapacity() && cArea >= 0.75f * AnalCapacity() && Random.Shared.Next(10) == 0)
            {
                Anal.Looseness += 1;

                stretched = true;
            }

            if (Anal.Looseness == 0)
            {
                Anal.Looseness += 1;

                stretched = true;
            }

            if (Anal.Looseness > 5)
                Anal.Looseness = 5;

            if (cArea >= 0.5f * AnalCapacity())
                if (!HasStatusEffect(StatusEffectIds.ButtStretched))
                    CreateStatusEffect(StatusEffectIds.ButtStretched);
                else
                    StatusEffectByType(StatusEffectIds.ButtStretched).Values[0] = 0;

            return stretched;
        }

        public void ButtChangeDisplay()
        {
            var looseness = Anal.Looseness;

            if (looseness == 5)
                Write("Sex.ButtChange.ClownCar".Localized(AssholeDescript()));

            if (looseness == 4)
                Write("Sex.ButtChange.GapingWide".Localized(AssholeDescript()));

            if (looseness == 3)
                Write("Sex.ButtChange.Loose".Localized(AssholeDescript()));

            if (looseness == 2)
                Write("Sex.ButtChange.Normal".Localized(AssholeDescript()));

            if (looseness == 1)
                Write("Sex.ButtChange.VirginityLost".Localized());
        }

        public void KillCocks(float deadCock)
        {
            var removed = 0;

            var storedCock = 0;

            if (deadCock < 0)
                deadCock = Cocks.Count;

            while (deadCock > 0)
            {
                var temp = Cocks.Count;

                while (temp > 0)
                {
                    temp--;

                    if (storedCock > Cocks.Count - 1)
                        storedCock = 0;

                    if (Cocks[temp].Length <= Cocks[storedCock].Length)
                        storedCock = temp;
                }

                RemoveCock(storedCock, 1);

                removed += 1;

                deadCock--;

                if (Cocks.Count == 0)
                    deadCock = 0;
            }

            if (removed == 1)
            {
                if (Cocks.Count == 0)
                {
                    Write("Sex.KillCocks.OneGone".Localized());

                    if (HasStatusEffect(StatusEffectIds.Infested))
                        Write("Sex.KillCocks.Infested".Localized());
                }

                if (Cocks.Count == 1)
                    Write("Sex.KillCocks.OneLeft".Localized(CockDescript(0)));

                if (Cocks.Count > 1)
                    Write("Sex.KillCocks.OneMany".Localized(MultiCockDescriptLight()));
            }

            if (removed > 1)
            {
                if (Cocks.Count == 0)
                {
                    Write("Sex.KillCocks.ManyGone".Localized());

                    if (HasStatusEffect(StatusEffectIds.Infested))
                        Write("Sex.KillCocks.Infested".Localized());
                }

                if (Cocks.Count == 1)
                    Write("Sex.KillCocks.ManyLeft".Localized(NumberUtils.Num2Text(removed), CockDescript(0)));

                if (Cocks.Count > 1)
                    Write("Sex.KillCocks.ManyMany".Localized(NumberUtils.Num2Text(removed), MultiCockDescriptLight()));
            }

            if (Cocks.Count == 0)
                RemoveStatusEffect(StatusEffectIds.Infested);

            if (Cocks.Count == 0 && Balls > 0)
            {
                Write("Sex.KillCocks.BallsGone".Localized(SackDescript(), BallsDescript()));

                Balls = 0;

                BallSize = 1;
            }
        }

        public void LengthChange(float temp2, float ncocks)
        {
            if (temp2 < 0 && GameSettings.Hyper)
                return;

            var single = Cocks.Count == 1;

            var multi = MultiCockDescriptLight();

            if (temp2 <= 1 && temp2 > 0)
                if (single)
                    Write("Sex.LengthChange.GrowSlightSingle".Localized(CockDescript(0)));
                else if (ncocks == 1)
                    Write("Sex.LengthChange.GrowSlightOne".Localized(multi));
                else if (ncocks > 1 && ncocks < Cocks.Count)
                    Write("Sex.LengthChange.GrowSlightSome".Localized(multi));
                else if (ncocks == Cocks.Count)
                    Write("Sex.LengthChange.GrowSlightAll".Localized(multi));

            if (temp2 > 1 && temp2 < 3)
                if (single)
                    Write("Sex.LengthChange.GrowMediumSingle".Localized(CockDescript(0)));
                else if (ncocks == Cocks.Count)
                    Write("Sex.LengthChange.GrowMediumAll".Localized(multi));
                else if (ncocks == 1)
                    Write("Sex.LengthChange.GrowMediumOne".Localized(multi));
                else if (ncocks > 1 && ncocks < Cocks.Count)
                    Write("Sex.LengthChange.GrowMediumSome".Localized(NumberUtils.Num2Text((int)ncocks), multi));

            if (temp2 >= 3)
                if (single)
                    Write("Sex.LengthChange.GrowLargeSingle".Localized(CockDescript(0)));
                else if (ncocks == 1)
                    Write("Sex.LengthChange.GrowLargeOne".Localized(multi));
                else if (ncocks > 1 && ncocks < Cocks.Count)
                    Write("Sex.LengthChange.GrowLargeSome".Localized(multi, NumberUtils.Num2Text((int)ncocks)));
                else if (ncocks == Cocks.Count)
                    Write("Sex.LengthChange.GrowLargeAll".Localized(multi));

            if (temp2 > 0)
            {
                var length = Cocks[0].Length;

                if (length >= 8 && length - temp2 < 8)
                    Write((single ? "Sex.LengthChange.Threshold8Single" : "Sex.LengthChange.Threshold8Multi").Localized(multi));

                if (length >= 12 && length - temp2 < 12)
                    Write((single ? "Sex.LengthChange.Threshold12Single" : "Sex.LengthChange.Threshold12Multi").Localized(CockDescript(0), multi));

                if (length >= 16 && length - temp2 < 16)
                {
                    Write((single ? "Sex.LengthChange.Threshold16Single" : "Sex.LengthChange.Threshold16Multi").Localized(CockDescript(0), multi));

                    if (BiggestBreastSize() >= 3)
                    {
                        if (single)
                            Write("Sex.LengthChange.Threshold16BreastSingle".Localized(CockDescript(0)));
                        else
                            Write("Sex.LengthChange.Threshold16BreastMulti".Localized());
                    }
                    else
                        if (single)
                            Write("Sex.LengthChange.Threshold16ChestSingle".Localized(CockDescript(0)));
                        else
                            Write("Sex.LengthChange.Threshold16ChestMulti".Localized(multi));
                }

                if (length >= 20 && length - temp2 < 20)
                {
                    Write((single ? "Sex.LengthChange.Threshold20Single" : "Sex.LengthChange.Threshold20Multi").Localized(CockDescript(0), multi));

                    if (Corruption > 40 && Corruption <= 60)
                        if (single)
                            Write("Sex.LengthChange.Threshold20Cor40Single".Localized());
                        else
                            Write("Sex.LengthChange.Threshold20Cor40Multi".Localized(multi));

                    if (Corruption > 60 && Corruption <= 80)
                        if (single)
                            Write("Sex.LengthChange.Threshold20Cor60Single".Localized(CockDescript(0)));
                        else
                            Write("Sex.LengthChange.Threshold20Cor60Multi".Localized(multi));

                    if (Corruption > 80 && !single)
                        Write("Sex.LengthChange.Threshold20Cor80Multi".Localized(multi));
                }
            }

            if (temp2 < 0 && temp2 >= -1)
                if (single)
                    Write("Sex.LengthChange.ShrinkSlightSingle".Localized(multi));
                else if (ncocks == Cocks.Count)
                    Write("Sex.LengthChange.ShrinkSlightAll".Localized(multi));
                else if (ncocks > 1 && ncocks < Cocks.Count)
                    Write("Sex.LengthChange.ShrinkSlightSome".Localized(NumberUtils.Num2Text((int)ncocks), multi));
                else if (ncocks == 1)
                    Write("Sex.LengthChange.ShrinkSlightOne".Localized(NumberUtils.Num2Text((int)ncocks), multi));

            if (temp2 < -1 && temp2 > -3)
                if (single)
                    Write("Sex.LengthChange.ShrinkMediumSingle".Localized(multi));
                else if (ncocks == Cocks.Count)
                    Write("Sex.LengthChange.ShrinkMediumAll".Localized(multi));
                else if (ncocks == 1)
                    Write("Sex.LengthChange.ShrinkMediumOne".Localized(NumberUtils.Num2Text((int)ncocks), multi));
                else if (ncocks > 1 && ncocks < Cocks.Count)
                    Write("Sex.LengthChange.ShrinkMediumSome".Localized(NumberUtils.Num2Text((int)ncocks), multi));

            if (temp2 <= -3)
                if (single)
                    Write("Sex.LengthChange.ShrinkLargeSingle".Localized(multi));
                else if (ncocks == Cocks.Count)
                    Write("Sex.LengthChange.ShrinkLargeAll".Localized(multi));
                else if (ncocks == 1)
                    Write("Sex.LengthChange.ShrinkLargeOne".Localized(multi));
                else if (ncocks > 1 && Cocks.Count > ncocks)
                    Write("Sex.LengthChange.ShrinkLargeSome".Localized(multi, NumberUtils.Num2Text((int)ncocks)));
        }

        public void SlimeFeed()
        {
            if (HasStatusEffect(StatusEffectIds.SlimeCraving))
            {
                var craving = StatusEffectByType(StatusEffectIds.SlimeCraving);

                craving.Values[0] = 0;

                if (!HasStatusEffect(StatusEffectIds.SlimeCravingFeed))
                    CreateStatusEffect(StatusEffectIds.SlimeCravingFeed);
            }

            if (HasPerk(PerkIds.Diapause))
            {
                var flags = CoC.Instance.Flags;

                flags.Set(
                    GameFlagIds.DiapauseFluidAmount,

                    flags.Get(GameFlagIds.DiapauseFluidAmount) + 3 + Random.Shared.Next(3));

                flags.Set(GameFlagIds.DiapauseNeedsDisplaying, 1);
            }
        }
    }
}