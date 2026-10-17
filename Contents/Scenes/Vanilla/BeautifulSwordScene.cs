using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<BeautifulSwordScene>("coc:beautiful_sword_scene")]
    public class BeautifulSwordScene : Scene
    {

        public override void Entry()
        {
        }

        public void DefeatedBySword()
        {
            ClearOutput();

            OutputText("Scenes.Beautifulswordscene.DefeatedBySword.StumbleWounded".Localized());

            OutputText("Scenes.Beautifulswordscene.DefeatedBySword.SwordLevelsTip".Localized());

            OutputText("Scenes.Beautifulswordscene.DefeatedBySword.ImpaledLifted".Localized());

            OutputText("Scenes.Beautifulswordscene.DefeatedBySword.PinnedToTree".Localized());

            OutputText("Scenes.Beautifulswordscene.DefeatedBySword.BlueFlameDeath".Localized());

            OutputText("Scenes.Beautifulswordscene.DefeatedBySword.DisintegratedSeconds".Localized());

            GameOver();
        }

        public void RebellingBeautifulSword(bool wieldAttempt = false)
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.DestroyedBeautifulSword) != 1)
            {
                if (Player.Corruption < (55 + Player.CorruptionTolerance()))
                {
                    if (!wieldAttempt)
                        OutputText("Scenes.Beautifulswordscene.RebellingBeautifulSword.SwordBurnsWielder".Localized());
                    else
                        OutputText("Scenes.Beautifulswordscene.RebellingBeautifulSword.WieldAttemptBurn".Localized());

                    if (!wieldAttempt)
                    {
                        var dmg = 20;

                        dmg -= (int)(Player.ArmorDefense);

                        if (dmg < 1)
                            dmg = 1;

                        Player.HealthChange(-dmg);

                        Player.SetUnarmed();

                        InventoryScene.TakeItem(WeaponIds.BeautifulSword, PlayerMenu);
                    }
                    else
                        DoNext(PlayerMenu);
                }
                else
                {
                    if (Player.Weapon.Id == WeaponIds.BeautifulSword)
                        Player.SetUnarmed();

                    OutputText("Scenes.Beautifulswordscene.RebellingBeautifulSword.SwordBurnsAfterUnarm".Localized());

                    OutputText("Scenes.Beautifulswordscene.RebellingBeautifulSword.SwordTurnsFoe".Localized());

                    Player.DestroyItems(WeaponIds.BeautifulSword, 1);

                    CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "beautiful_sword_fight")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
                }
            }
            else
            {
                OutputText("Scenes.Beautifulswordscene.RebellingBeautifulSword.ImpossibleSwordState".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void DestroyBeautifulSword()
        {

            CoC.Instance.Flags.Set(GameFlagIds.DestroyedBeautifulSword, 1);

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.SwordShardsTaken) < 5)
                AddButton(0, "Common.Button.TakeShard".Localized(), TakeShard);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void TakeShard()
        {

            CoC.Instance.Flags.Set(GameFlagIds.SwordShardsTaken, CoC.Instance.Flags.Get(GameFlagIds.SwordShardsTaken) + 1);

            InventoryScene.TakeItem(ConsumableIds.SwordShard, DestroyBeautifulSword);
        }

    }
}