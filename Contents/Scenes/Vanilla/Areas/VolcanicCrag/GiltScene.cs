using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.VolcanicCrag
{
    [Register<GiltScene>("coc:gilt")]
    public class GiltScene : Scene
    {

        public bool encountered = false;

        public static SaveContent Save => SaveContent(SceneIds.VolcanicCrag.Gilt);

        public string debugName => "Gilt";

        public string debugHint => "";

        public override void Entry()
        {
            EncounterGilt();
        }

        public void Reset()
        {

            Save.SetBool("encountered", false);
        }

        public void EncounterGilt()
        {
            ClearOutput();

            if (!Save.GetBool("encountered"))
            {
                OutputText("Scenes.Giltscene.EncounterGilt.DryHeat".Localized());

                OutputText("Scenes.Giltscene.EncounterGilt.BlindedApproach".Localized());

                OutputText("Scenes.Giltscene.EncounterGilt.InFullView".Localized());

                OutputText("Scenes.Giltscene.EncounterGilt.SpeedyChirps".Localized());

                Save.SetBool("encountered", true);
            }
            else
                OutputText("Scenes.Giltscene.EncounterGilt.MelodicEcho".Localized());

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.Gilt), () => Go<CampScene>());
        }

        public void GiltLose(bool hpVictory)
        {
            ClearOutput();

            RegisterMacro("lustvictory", !hpVictory);

            OutputText("Scenes.Giltscene.GiltLose.PressesWings".Localized());

            OutputText("Scenes.Giltscene.GiltLose.PlumageShakes".Localized());

            if (!Player.HasCock())
            {
                OutputText("Scenes.Giltscene.GiltLose.EagerlyPokes".Localized());

                OutputText("Scenes.Giltscene.GiltLose.RealitySinks".Localized());

                OutputText("Scenes.Giltscene.GiltLose.ThighsClamp".Localized());

                OutputText("Scenes.Giltscene.GiltLose.RocksVigor".Localized());

                OutputText("Scenes.Giltscene.GiltLose.ClawsScrape".Localized());

                OutputText("Scenes.Giltscene.GiltLose.LegsLocked".Localized());
            }
            else
            {
                OutputText("Scenes.Giltscene.GiltLose.SitsCunt".Localized());

                OutputText("Scenes.Giltscene.GiltLose.ShoutsWingsShudder".Localized());

                OutputText("Scenes.Giltscene.GiltLose.RhythmFaster".Localized());

                OutputText("Scenes.Giltscene.GiltLose.BodySeizes".Localized());

                OutputText("Scenes.Giltscene.GiltLose.ReleasesYou".Localized());

                Player.Orgasm("Dick");
            }

            CombatScene.CleanupAfterCombat();
        }

        public void GiltWinChoices(bool hpVictory)
        {
            ClearOutput();

            RegisterMacro("lustvictory", !hpVictory);

            OutputText("Scenes.Giltscene.GiltWinChoices.PoundsGround".Localized());

            ClearButtons();

            AddNextButton("Scenes.Faerie.SexMenu.ButtonOral".Localized(), GiltOral);

            AddNextButton("Common.Button.Fuck".Localized(), GiltFuck);

            AddNextButton("Common.Button.Grind".Localized(), GiltGrind);

            AddNextButton("Common.Button.Earfuck".Localized(), GiltEarFuck);

            AddNextButton("Common.Button.Kill".Localized(), GiltKill);

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void GiltOral()
        {
            ClearOutput();

            OutputText("Scenes.Giltscene.GiltOral.ApproachesOpenly".Localized());

            OutputText("Scenes.Giltscene.GiltOral.ThumbClawsDrag".Localized());

            if (Player.IsGenderless())
            {
                OutputText("Scenes.Giltscene.GiltOral.KissesGroin".Localized());

                OutputText("Scenes.Giltscene.GiltOral.WrithingAppendage".Localized());

                OutputText("Scenes.Giltscene.GiltOral.MoreThanNeed".Localized());
            }
            else
            {
                OutputText("Scenes.Giltscene.GiltOral.BeginsToStand".Localized());

                OutputText("Scenes.Giltscene.GiltOral.RestsFeathers".Localized());

                OutputText("Scenes.Giltscene.GiltOral.GroansPleasure".Localized());

                Player.Orgasm("VaginalAndDick");
            }

            OutputText("Scenes.Giltscene.GiltOral.StumblesBack".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void GiltFuck()
        {
            ClearOutput();

            OutputText("Scenes.Giltscene.GiltFuck.WorkedUp".Localized());

            OutputText("Scenes.Giltscene.GiltFuck.TalonsDig".Localized());

            OutputText("Scenes.Giltscene.GiltFuck.LoserDoesWork".Localized());

            OutputText("Scenes.Giltscene.GiltFuck.SlowsDown".Localized());

            OutputText("Scenes.Giltscene.GiltFuck.CouldBeShe".Localized());

            OutputText("Scenes.Giltscene.GiltFuck.GroansEarsTwitch".Localized());

            OutputText("Scenes.Giltscene.GiltFuck.BodyBreaks".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void GiltEarFuck()
        {
            ClearOutput();

            RegisterMacro("seenHellmouth", CoC.Instance.Flags.Get(GameFlagIds.MetHellmouth) == 1);

            OutputText("Scenes.Giltscene.GiltEarFuck.StrugglesProp".Localized());

            OutputText("Scenes.Giltscene.GiltEarFuck.TeasesLeaning".Localized());

            OutputText("Scenes.Giltscene.GiltEarFuck.MeltsUnderEar".Localized());

            OutputText("Scenes.Giltscene.GiltEarFuck.TipPushes".Localized());

            OutputText("Scenes.Giltscene.GiltEarFuck.GrufflyBegs".Localized());

            OutputText("Scenes.Giltscene.GiltEarFuck.SlipsFingers".Localized());

            OutputText("Scenes.Giltscene.GiltEarFuck.IncoherentSounds".Localized());

            OutputText("Scenes.Giltscene.GiltEarFuck.ShaftTwitches".Localized());

            OutputText("Scenes.Giltscene.GiltEarFuck.NoGoodCum".Localized());

            OutputText("Scenes.Giltscene.GiltEarFuck.ThrustsSlams".Localized());

            OutputText("Scenes.Giltscene.GiltEarFuck.ConstrictsEar".Localized());

            OutputText("Scenes.Giltscene.GiltEarFuck.LastSpurts".Localized());

            OutputText("Scenes.Giltscene.GiltEarFuck.BrainDamage".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void GiltGrind()
        {
            ClearOutput();

            OutputText("Scenes.Giltscene.GiltGrind.BattleHostility".Localized());

            OutputText("Scenes.Giltscene.GiltGrind.ToHerDismay".Localized());

            OutputText("Scenes.Giltscene.GiltGrind.AdmitsTight".Localized());

            OutputText("Scenes.Giltscene.GiltGrind.CurtainWings".Localized());

            OutputText("Scenes.Giltscene.GiltGrind.ClosesExplore".Localized());

            OutputText("Scenes.Giltscene.GiltGrind.CollapsesFlat".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void GiltKill()
        {
            ClearOutput();

            RegisterMacro("isTerrestialMagic", CoC.Instance.Flags.Get(GameFlagIds.MagicSwitch) == 1);

            OutputText("Scenes.Giltscene.GiltKill.SeizesMoment".Localized());

            if (Player.Weapon.IsHolySword())
                OutputText("Scenes.Giltscene.GiltKill.GlimmeringBlade".Localized());
            else
                if (Player.Weapon.Id == WeaponIds.DullScimitar)
                    OutputText("Scenes.Giltscene.GiltKill.LiftsScythe".Localized());
                else
                    if (Player.Weapon.IsAxe())
                        OutputText("Scenes.Giltscene.GiltKill.AnkleStrike".Localized());
                    else
                        if (Player.Weapon.IsFirearm())
                            OutputText("Scenes.Giltscene.GiltKill.ThroughHaze".Localized());
                        else
                            if (Player.Weapon.IsStabby())
                                OutputText("Scenes.Giltscene.GiltKill.UnprotectedFlesh".Localized());
                            else
                                if (Player.Weapon.IsStaff() && Player.Weapon.IsChanneling(Player))
                                    OutputText("Scenes.Giltscene.GiltKill.FocusMight".Localized());
                                else
                                    if (Player.Weapon.IsBlunt())
                                        OutputText("Scenes.Giltscene.GiltKill.HastensSwing".Localized());
                                    else
                                        OutputText("Scenes.Giltscene.GiltKill.RushesStrike".Localized());

            Player.UpgradeDeusVult();

            CombatScene.CleanupAfterCombat();
        }

    }
}