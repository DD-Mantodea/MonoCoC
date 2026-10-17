using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<BaseKitsuneScene>("coc:base_kitsune")]
    public class BaseKitsuneScene : Scene
    {

        public const string PHYSICAL_SKILL = "physical";

        public const string MAGICAL_SKILL = "magical";

        public const int SEAL_ATTACK = 0;

        public const int SEAL_TEASE = 1;

        public const int SEAL_SPELLS = 2;

        public const int SEAL_ITEMS = 3;

        public const int SEAL_MOVEMENT = 4;

        public const int SEAL_PHYSICAL = 5;

        public const int SEAL_MAGICAL = 6;

        protected static readonly int[] sealTypes =

        [
            SEAL_ATTACK, SEAL_TEASE, SEAL_SPELLS, SEAL_ITEMS, SEAL_MOVEMENT, SEAL_PHYSICAL, SEAL_MAGICAL,
        ];

        public override void Entry()
        {
            CalculateAttackResist();
        }

        public int CalculateAttackResist()
        {

            var resist = 0;

            resist += (int)(Math.Min(Player.Intelligence / 2, 40));

            if (Player.HasPerk(PerkIds.Whispered))
                resist += 20;

            if (Player.IsReligious() && Player.IsPureEnough(20))
                resist += (int)(20 - Player.CorAdjustedDown());

            if (Player.HasPerk(PerkIds.EnlightenedNinetails) || Player.HasPerk(PerkIds.CorruptedNinetails))
                resist += 30;

            return resist;
        }

        public void SealPlayerAttack()
        {

            OutputText("Scenes.Basekitsune.SealPlayerAttack.PlayfullyDartsAroundYou".Localized());

            OutputText("Scenes.Basekitsune.SealPlayerAttack.Text".Localized());

            OutputText("Scenes.Basekitsune.SealPlayerAttack.DespiteYourBestEfforts".Localized());

            Player.CreateStatusEffect(StatusEffectIds.Sealed, 4, 0, 0, 0);
        }

        public void SealPlayerPhysicalSpecialSkills()
        {

            SealPlayerSpecial(PHYSICAL_SKILL);
        }

        public void SealPlayerMagicSpecialSkills()
        {

            SealPlayerSpecial(MAGICAL_SKILL);
        }

        public void SealPlayerSpecial(string skillType)
        {

            OutputText("Scenes.Basekitsune.SealPlayerSpecial.YouJumpWithSurprise".Localized(
                skillType
            ));

            OutputText("Scenes.Basekitsune.SealPlayerSpecial.Text".Localized());

            OutputText("Scenes.Basekitsune.SealPlayerSpecial.TheKitsuneSSpell".Localized(
                skillType
            ));

            if (skillType == PHYSICAL_SKILL)
                Player.CreateStatusEffect(StatusEffectIds.Sealed, 4, 5, 0, 0);
            else
                if (skillType == MAGICAL_SKILL)
                    Player.CreateStatusEffect(StatusEffectIds.Sealed, 4, 6, 0, 0);
        }

        public void SealPlayerTease()
        {

            OutputText("Scenes.Basekitsune.SealPlayerTease.YouAreTakenBy".Localized());

            Player.CreateStatusEffect(StatusEffectIds.Sealed, 4, 1, 0, 0);
        }

        public void SealPlayerItems()
        {

            OutputText("Scenes.Basekitsune.SealPlayerItems.TheKitsuneSaysAs".Localized());

            Player.CreateStatusEffect(StatusEffectIds.Sealed, 4, 3, 0, 0);
        }

        public void SealPlayerSpells()
        {

            OutputText("Scenes.Basekitsune.SealPlayerSpells.TheKitsuneSaysWith".Localized());

            Player.CreateStatusEffect(StatusEffectIds.Sealed, 4, 2, 0, 0);
        }

        public void ResistSeal()
        {

            OutputText("Scenes.Basekitsune.ResistSeal.UponYourTouchThe".Localized());

            Player.RemoveStatusEffect(StatusEffectIds.Sealed);
        }

        public void SealPlayerMovement()
        {

            OutputText("Scenes.Basekitsune.SealPlayerMovement.TheKitsuneSaysPopping".Localized());

            Player.CreateStatusEffect(StatusEffectIds.Sealed, 4, 4, 0, 0);
        }

        public void KitsuneSealAttack(int type = -1)
        {

            var resist = CalculateAttackResist();

            int select = (type >= 0 ? type : sealTypes[Random.Shared.Next(sealTypes.Length)]);

            switch (select)
            {
                case SEAL_ATTACK:
                {
                SealPlayerAttack();
                }

                break;

                case SEAL_TEASE:
                {
                SealPlayerTease();
                }

                break;

                case SEAL_SPELLS:
                {
                SealPlayerSpells();
                }

                break;

                case SEAL_ITEMS:
                {
                SealPlayerItems();
                }

                break;

                case SEAL_MOVEMENT:
                {
                SealPlayerMovement();
                }

                break;

                case SEAL_PHYSICAL:
                {
                SealPlayerPhysicalSpecialSkills();
                }

                break;

                case SEAL_MAGICAL:
                {
                SealPlayerMagicSpecialSkills();
                }

                break;
            }

            if (resist >= Random.Shared.Next(100))
                ResistSeal();
        }

    }
}