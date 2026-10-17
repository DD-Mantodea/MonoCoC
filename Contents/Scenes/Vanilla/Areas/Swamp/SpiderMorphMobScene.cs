using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Swamp
{
    [Register<SpiderMorphMobScene>("coc:spider_morph_mob")]
    public class SpiderMorphMobScene : Scene
    {
        public static CombatContext CombatContext => CombatController.Instance!.Context;

        public bool missFirstRound = false;

        public override void Entry()
        {
            SpiderStandardAttack();
        }

        public void SpiderStandardAttack()
        {

            if (missFirstRound || CombatCalculator.Avoid(CombatContext, new AvoidDamageParameters { DoDodge = true, DoParry = true, DoBlock = false }).AttackFailed)
            {
                missFirstRound = false;

                OutputText("Scenes.SpiderMorphMob.SpiderStandardAttack.ANumberOfSpiders".Localized());
            }

            if (!(missFirstRound || CombatCalculator.Avoid(CombatContext, new AvoidDamageParameters { DoDodge = true, DoParry = true, DoBlock = false }).AttackFailed))
            {
                OutputText("Scenes.SpiderMorphMob.SpiderStandardAttack.ANumberOfSpiders.Two".Localized());

                var damage = CombatCalculator.ReduceDamage(CombatContext, Player, CurrentMonster.Strength + CurrentMonster.WeaponAttack) + 20;

                if (damage <= 0)
                {
                    damage = 0;

                    if (Random.Shared.Next((int)(Player.ArmorDefense + Player.Toughness)) < Player.ArmorDefense)
                        OutputText("Scenes.SpiderMorphMob.SpiderStandardAttack.YouAbsorbAndDeflect".Localized(
                            CurrentMonster.WeaponVerb
                        ));
                    else
                        OutputText("Scenes.SpiderMorphMob.SpiderStandardAttack.YouDeflectAndBlock".Localized(
                            CurrentMonster.WeaponVerb,

                            CurrentMonster.Article,

                            CombatContext.Enemy.ShortName
                        ));
                }
                else
                    if (damage < 6)
                        OutputText("Scenes.SpiderMorphMob.SpiderStandardAttack.YouAreStruckA".Localized(
                            CurrentMonster.Article,

                            CombatContext.Enemy.ShortName
                        ));
                    else
                        if (damage < 11)
                            OutputText("Scenes.SpiderMorphMob.SpiderStandardAttack.WoundsYou".Localized(
                                CombatContext.Enemy.CapitalA,

                                CombatContext.Enemy.ShortName
                            ));
                        else
                            if (damage < 21)
                                OutputText("Scenes.SpiderMorphMob.SpiderStandardAttack.StaggersYouWithThe".Localized(
                                    CombatContext.Enemy.CapitalA,

                                    CombatContext.Enemy.ShortName,

                                    CurrentMonster.Pronoun3,

                                    CurrentMonster.WeaponVerb
                                ));
                            else
                                if (damage > 20)
                                    OutputText("Scenes.SpiderMorphMob.SpiderStandardAttack.MutilateYouWithPowerful".Localized(
                                        CombatContext.Enemy.CapitalA,

                                        CombatContext.Enemy.ShortName,

                                        CurrentMonster.Pronoun3,

                                        CurrentMonster.WeaponVerb
                                    ));

                if (damage > 0)
                {
                    Player.HealthChange(-damage);

                    if (CurrentMonster.LustVulnerability > 0 && Player.Armor == null)
                    {
                        if (!CurrentMonster.Plural)
                            OutputText("Scenes.SpiderMorphMob.SpiderStandardAttack.BrushesAgainstYourExposed".Localized(
                                CombatContext.Enemy.CapitalA,

                                CombatContext.Enemy.ShortName
                            ));
                        else
                            OutputText("Scenes.SpiderMorphMob.SpiderStandardAttack.BrushAgainstYourExposed".Localized(
                                CombatContext.Enemy.CapitalA,

                                CombatContext.Enemy.ShortName
                            ));

                        CurrentMonster.Lust += 10 * CurrentMonster.LustVulnerability;
                    }
                }

            }

            KihaSPOIDAHAI();
        }

        public void Struggle()
        {
            ClearOutput();

            if (Player.HasStatusEffect(StatusEffectIds.UberWeb))
            {
                OutputText("Scenes.SpiderMorphMob.Struggle.YouClawYourWay".Localized());

                Player.RemoveStatusEffect(StatusEffectIds.UberWeb);
            }
        }

        public void SpoidahHordeWebLaunchahs()
        {

            if (missFirstRound || CombatCalculator.Avoid(CombatContext, new AvoidDamageParameters { DoDodge = true, DoParry = false, DoBlock = false }).AttackFailed)
                OutputText("Scenes.SpiderMorphMob.SpoidahHordeWebLaunchahs.OneOfTheDriders".Localized());

            if (!(missFirstRound || CombatCalculator.Avoid(CombatContext, new AvoidDamageParameters { DoDodge = true, DoParry = false, DoBlock = false }).AttackFailed))
            {
                OutputText("Scenes.SpiderMorphMob.SpoidahHordeWebLaunchahs.SomeOfTheSpiders".Localized());

                Player.CreateStatusEffect(StatusEffectIds.UberWeb, 0, 0, 0, 0);

                CurrentMonster.Health -= 250;
            }
        }

        public void KihaSPOIDAHAI()
        {

            OutputText("Scenes.SpiderMorphMob.KihaSPOIDAHAI.Text".Localized());

            SpriteSelect("kiha");

            OutputText("Scenes.SpiderMorphMob.KihaSPOIDAHAI.WhileTheyReTangled".Localized());

            CurrentMonster.Health -= 50;
        }

        public void PerformCombatAction()
        {

            SpriteSelect("kiha");

            if (Random.Shared.Next(2) == 0 || Player.HasStatusEffect(StatusEffectIds.UberWeb))
                SpiderStandardAttack();
            else
                SpoidahHordeWebLaunchahs();
        }

        public void Defeated(bool hpVictory)
        {

            SceneInstance<KihaFollowerScene>.Get().BeatSpiderMob();
        }

        public void Won(bool hpVictory, bool pcCameWorms = false)
        {

            if (pcCameWorms)
            {
                OutputText("Scenes.SpiderMorphMob.Won.TheSpidersSmileTo".Localized());

                CombatController.Instance!.EndLustLoss();
            }
            else
                SceneInstance<KihaFollowerScene>.Get().LoseToSpiderMob();
        }

    }
}