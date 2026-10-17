using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Bog
{
    [Register<ParasiteScene>("coc:parasite")]
    public class ParasiteScene : Scene
    {

        public override void Entry()
        {
            FindParasite();
        }

        public bool ParasiteUpdate()
        {

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteSlugMatureDay))
                Player.ChangeStatusValue(StatusEffectIds.ParasiteSlugMatureDay, 0, -1);

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteSlug, 0) > 0)
                if (Player.HasCock())
                {
                    Player.ChangeStatusValue(StatusEffectIds.ParasiteSlug, 0, -1);

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteSlug, 0) <= 0)
                    {
                        Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlug);

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.ShouldntHappenBanner".Localized());

                        return true;
                    }

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteSlug, 0) == 60)
                    {
                        OutputText("Scenes.Parasitescene.ParasiteUpdate.OddTinglingBanner".Localized());

                        return true;
                    }

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteSlug, 0) == 48)
                    {
                        OutputText("Scenes.Parasitescene.ParasiteUpdate.BotheredBanner".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.CocksOozePrecum".Localized());
                        else
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.DickOozesPrecum".Localized(
                                 Player.CockDescriptShort()
                            ));

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.PrecumFaintSmell".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.Hadslugparasite) == 0)
                        {
                            if (Player.Corruption <= 33)
                                OutputText("Scenes.Parasitescene.ParasiteUpdate.WorriedSomething".Localized());

                            if (Player.Corruption > 33 && Player.Corruption <= 66)
                                OutputText("Scenes.Parasitescene.ParasiteUpdate.WorriedBody".Localized());

                            if (Player.Corruption > 66)
                                OutputText("Scenes.Parasitescene.ParasiteUpdate.ThinksGoodIdea".Localized());
                        }
                        else
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.SureManaged".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 2 + Random.Shared.Next(5)));

                        return true;
                    }

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteSlug, 0) == 36)
                    {
                        OutputText("Scenes.Parasitescene.ParasiteUpdate.ConstantlyAssaulted".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.CocksDribble".Localized());
                        else
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.DickDribbles".Localized(
                                 Player.CockDescriptShort()
                            ));

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.SmellStronger".Localized(
                             Player.ClothedOrNakedLower(" strip out of your [armor]", "reach down")
                        ));

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.CantReachClimax".Localized());

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.SpreadsSpit".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.Hadslugparasite) == 0)
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.OrgasmFinally".Localized(
                                 Player.CockDescript()
                            ));
                        else
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.OrgasmFinallyAgain".Localized(
                                 Player.CockDescript()
                            ));

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.MinutesLaterClimax".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.Hadslugparasite) == 0)
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.ThrilledNotices".Localized());
                        else
                        {
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.WrigglingBegins".Localized());

                            if (Player.Corruption < 60)
                                OutputText("Scenes.Parasitescene.ParasiteUpdate.BodyBetrays".Localized());
                            else
                                OutputText("Scenes.Parasitescene.ParasiteUpdate.ReconsidersDecision".Localized());
                        }

                        if (CoC.Instance.Flags.Get(GameFlagIds.Hadslugparasite) == 0)
                        {
                            if (Player.Corruption <= 33)
                                OutputText("Scenes.Parasitescene.ParasiteUpdate.TerrifiedWhatTheHell".Localized());
                            else
                                if (Player.Corruption <= 66)
                                    OutputText("Scenes.Parasitescene.ParasiteUpdate.TerrifiedCant".Localized());
                                else
                                    OutputText("Scenes.Parasitescene.ParasiteUpdate.SquirmsDelight".Localized());
                        }
                        else
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.ParasiteGrowing".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 2 + Random.Shared.Next(15)));

                        return true;
                    }

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteSlug, 0) == 24)
                    {
                        OutputText("Scenes.Parasitescene.ParasiteUpdate.CannotWithstand".Localized());

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.YouPrefix".Localized());

                        if (Player.Corruption <= 33)
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.HesitantResigned".Localized(
                                 Player.ClothedOrNakedLower("strip out of your [armor]", "lower your hands")
                            ));
                        else
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.EagerBarelyAble".Localized(
                                 Player.ClothedOrNakedLower("strip out of your [armor]", "lower your hands")
                            ));

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.NoAttempt".Localized());

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.InitiallyGently".Localized(
                             Player.CockDescript()
                        ));

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.RegainConsciousness".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.Hadslugparasite) == 0)
                        {
                            if (Player.Corruption <= 33)
                                OutputText("Scenes.Parasitescene.ParasiteUpdate.PressureMovement".Localized());
                            else
                                if (Player.Corruption <= 66)
                                    OutputText("Scenes.Parasitescene.ParasiteUpdate.PressureMovementAgain".Localized());
                                else
                                    OutputText("Scenes.Parasitescene.ParasiteUpdate.NotSureWhen".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.ShouldntBeLong".Localized());

                            if (Player.Corruption <= 33)
                                OutputText("Scenes.Parasitescene.ParasiteUpdate.BetweenOrgasms".Localized());
                            else
                                if (Player.Corruption > 33 && Player.Corruption < 66)
                                    OutputText("Scenes.Parasitescene.ParasiteUpdate.ShouldBeMoving".Localized());
                                else
                                    OutputText("Scenes.Parasitescene.ParasiteUpdate.MayHaveBeenInfested".Localized());
                        }

                        Player.ApplyStats(new DynStat(GameStats.Lust, 2 + Random.Shared.Next(30)));

                        return true;
                    }

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteSlug, 0) == 12)
                    {
                        OutputText("Scenes.Parasitescene.ParasiteUpdate.PressurePleasure".Localized(
                             Player.ClothedOrNakedLower(" rip your [armor] off", "lean forward")
                        ));

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.CocksSuffix".Localized());
                        else
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.DickSuffix".Localized(
                                 Player.CockDescriptShort()
                            ));

                        Player.Orgasm("Anal");

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.QuiveringRelease".Localized());

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.FallsOnBack".Localized());

                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.PuddleOfSemenAgain".Localized());

                        if (Player.CumQ() >= 250 && Player.CumQ() < 1000)
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.AbsoluteMess".Localized());

                        if (Player.CumQ() >= 1000)
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.VeritableLake".Localized());

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.CreatedNotices".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.Hadslugparasite) == 0)
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.MaybeThatsWhere".Localized());
                        else
                            OutputText("Scenes.Parasitescene.ParasiteUpdate.DefinitelyWhere".Localized());

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.MinutesAfterIncident".Localized());

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.SmellLingers".Localized());

                        Player.CreatePerk(PerkIds.ParasiteMusk, 1, 0, 0, 0);

                        Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlug);

                        OutputText("Scenes.Parasitescene.ParasiteUpdate.NewPerkMusk".Localized());

                        Player.CreateStatusEffect(StatusEffectIds.ParasiteSlugMatureDay, 96, 0, 0, 0);

                        CoC.Instance.Flags.Set(GameFlagIds.Hadslugparasite, 1);

                        Player.ApplyStats(new DynStat(GameStats.Lust, 2 + Random.Shared.Next(30)));

                        return true;
                    }
                }
                else
                {
                    OutputText("Scenes.Parasitescene.ParasiteUpdate.NoProstate".Localized());

                    Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlug);

                    return true;
                }

            if (Player.PerkValue(PerkIds.ParasiteMusk) > 0 && View.Time.Days % 5 == 0 && Player.StatusEffectValue(StatusEffectIds.ParasiteSlugMatureDay, 0) <= 0 && !Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
            {
                Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlugMatureDay);

                OutputText("Scenes.Parasitescene.ParasiteUpdate.MovementOfParasites".Localized());

                OutputText("Scenes.Parasitescene.ParasiteUpdate.SomewhatUsed".Localized());

                OutputText("Scenes.Parasitescene.ParasiteUpdate.ConditioningInitial".Localized());

                OutputText("Scenes.Parasitescene.ParasiteUpdate.MindFilledImages".Localized(
                     Player.CockDescript()
                ));

                if (Player.Corruption < 50)
                    OutputText("Scenes.Parasitescene.ParasiteUpdate.MindWarped".Localized());
                else
                    if (Player.Corruption < 80)
                        OutputText("Scenes.Parasitescene.ParasiteUpdate.LandTakenToll".Localized());
                    else
                        OutputText("Scenes.Parasitescene.ParasiteUpdate.CantWaitStart".Localized());

                Player.CreateStatusEffect(StatusEffectIds.ParasiteSlugReproduction, 2, 0, 0, 0);
            }

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteSlugReproduction, 0) == 1 && View.Time.Hours % 5 == 0 && View.Time.Days % 5 == 1)
            {
                OutputText("Scenes.Parasitescene.ParasiteUpdate.RecentExploits".Localized());

                OutputText("Scenes.Parasitescene.ParasiteUpdate.ThisTimeNoNeed".Localized(
                     Player.CockDescript()
                ));

                OutputText("Scenes.Parasitescene.ParasiteUpdate.FeelsNewlyBirthed".Localized());

                OutputText("Scenes.Parasitescene.ParasiteUpdate.WakeUpFeel".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Parasitescene.ParasiteUpdate.PuddleOfSemenAgain".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Parasitescene.ParasiteUpdate.MessOfSemen".Localized());
                    else
                        OutputText("Scenes.Parasitescene.ParasiteUpdate.LakeOfSemen".Localized());

                OutputText("Scenes.Parasitescene.ParasiteUpdate.ForBirthedParasite".Localized());

                if (Player.Corruption < 60)
                    OutputText("Scenes.Parasitescene.ParasiteUpdate.ReconsidersThoughts".Localized());

                Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlugReproduction);
            }

            if (!Player.HasCock() && Player.PerkValue(PerkIds.ParasiteMusk) > 0)
            {
                OutputText("Scenes.Parasitescene.ParasiteUpdate.NoProstateAgain".Localized());

                Player.RemovePerk(PerkIds.ParasiteMusk);

                Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlugReproduction);

                Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlug);

                return true;
            }

            return false;
        }

        public void FindParasite()
        {
            ClearOutput();

            OutputText("Scenes.Parasitescene.FindParasite.WanderingMarsh".Localized());

            OutputText("Scenes.Parasitescene.FindParasite.NothingThreatens".Localized());

            OutputText("Scenes.Parasitescene.FindParasite.DespiteInsects".Localized());

            OutputText("Scenes.Parasitescene.FindParasite.PreparingLeave".Localized());

            if (Player.HasCock())
            {
                if (Player.HasPerk(PerkIds.ParasiteMusk))
                {
                    OutputText("Scenes.Parasitescene.FindParasite.StripsArmor".Localized());

                    OutputText("Scenes.Parasitescene.FindParasite.FindsWorm".Localized());

                    OutputText("Scenes.Parasitescene.FindParasite.AttemptsFlick".Localized());
                }
                else
                    if (!Player.HasStatusEffect(StatusEffectIds.Infested))
                    {
                        OutputText("Scenes.Parasitescene.FindParasite.StripsArmorAgain".Localized(
                             Player.CockDescript()
                        ));

                        OutputText("Scenes.Parasitescene.FindParasite.GrabsParasite".Localized());

                        if (Player.Corruption < 60)
                            OutputText("Scenes.Parasitescene.FindParasite.HorrifiedGlad".Localized());
                        else
                            OutputText("Scenes.Parasitescene.FindParasite.BitAroused".Localized());

                        OutputText("Scenes.Parasitescene.FindParasite.ThrowsIt".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Parasitescene.FindParasite.WonderWormsInside".Localized(
                             Player.CockDescript()
                        ));

                        OutputText("Scenes.Parasitescene.FindParasite.WormsProtected".Localized());
                    }
            }
            else
            {
                OutputText("Scenes.Parasitescene.FindParasite.StripsArmorThird".Localized());

                OutputText("Scenes.Parasitescene.FindParasite.CouldBeWorse".Localized());
            }

            Player.CreateStatusEffect(StatusEffectIds.ParasiteSlugMet, 1, 0, 0, 0);

            DoNext(() => ReturnToCamp(1));
        }

    }
}