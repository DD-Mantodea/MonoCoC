using System.Collections.Generic;

namespace MonoCoC.Saving.Legacy
{
    public interface ILegacyKFlag
    {
        public int Value { get; set; }

        public string Name { get; set; }
    }

    public struct LegacyKFlag<T> : ILegacyKFlag
    {
        public int Value { get; set; }

        public string Name { get; set; }

        public LegacyKFlag(int value, string name)
        {
            Value = value;

            Name = name;

            LegacyKFlags.FLAGS.Add(value, this);
        }
    }

    public static class LegacyKFlags
    {
        public static Dictionary<int, ILegacyKFlag> FLAGS = [];

        public static LegacyKFlag<int> MARBLE_PREGNANCY_INCUBATION = new(1, "MARBLE_PREGNANCY_INCUBATION"); // Marble Pregnancy Incubation

        public static LegacyKFlag<int> MARBLE_PREGNANCY_TYPE = new(2, "MARBLE_PREGNANCY_TYPE"); // Marble Pregnancy Type" Description="1 = cowgirl

        public static LegacyKFlag<float> MARBLE_LUST = new(3, "MARBLE_LUST"); // Marble Lust

        public static LegacyKFlag<int> MARBLE_DICK_TYPE = new(4, "MARBLE_DICK_TYPE"); // Marble Dick Type

        public static LegacyKFlag<int> MARBLE_DICK_LENGTH = new(5, "MARBLE_DICK_LENGTH"); // Marble Dick Length

        public static LegacyKFlag<int> MARBLE_DICK_THICKNESS = new(6, "MARBLE_DICK_THICKNESS"); // Marble Dick Thickness

        public static LegacyKFlag<int> MARBLE_UNUSED_DOOTY = new(7, "MARBLE_UNUSED_DOOTY"); // Marble Dooty. Haha doody.  (DUTY, "MARBLE_UNUSED_DOOTY") Seems unused

        public static LegacyKFlag<int> MARBLE_KIDS = new(8, "MARBLE_KIDS");

        public static LegacyKFlag<int> MARBLE_NURSERY_CONSTRUCTION = new(9, "MARBLE_NURSERY_CONSTRUCTION"); // Marble's Bitch-Bin (0 = not start, less than 100 = in progress.  100 = Finished

        public static LegacyKFlag<int> PLAYER_PREGGO_WITH_WORMS = new(10, "PLAYER_PREGGO_WITH_WORMS"); // (I think, "PLAYER_PREGGO_WITH_WORMS")// HAS WORM CUM DISPLAYED YET?

        public static LegacyKFlag<int> TIMES_FUCKED_URTA = new(11, "TIMES_FUCKED_URTA"); // URTA - times fucked

        public static LegacyKFlag<int> URTA_COMFORTABLE_WITH_OWN_BODY = new(12, "URTA_COMFORTABLE_WITH_OWN_BODY"); // URTA - horsecock comfort level (-1 = hates self, no luvs, "URTA_COMFORTABLE_WITH_OWN_BODY")

        public static LegacyKFlag<int> URTA_TIME_SINCE_LAST_CAME = new(13, "URTA_TIME_SINCE_LAST_CAME"); // URTA - hours until can be horny again

        public static LegacyKFlag<int> PC_SEEN_URTA_SEX_TOYS = new(14, "PC_SEEN_URTA_SEX_TOYS"); // URTA - seen Urta's toyz?

        public static LegacyKFlag<int> PLAYER_RESISTED_AKBAL = new(15, "PLAYER_RESISTED_AKBAL"); // Akbal resisted?  - 1 means the PC has resisted at least once

        public static LegacyKFlag<int> AKBAL_SUBMISSION_COUNTER = new(16, "AKBAL_SUBMISSION_COUNTER"); // Akbal submission counter

        public static LegacyKFlag<int> AKBAL_SUBMISSION_STATE = new(17, "AKBAL_SUBMISSION_STATE"); // Akbal submission state. -1=Lost to him, 1=Beaten him, 2=Akbal is your bitch

        public static LegacyKFlag<float> MINOTAUR_CUM_ADDICTION_TRACKER = new(18, "MINOTAUR_CUM_ADDICTION_TRACKER"); // Minotaur cum Addiction Tracker

        public static LegacyKFlag<int> TIME_SINCE_LAST_CONSUMED_MINOTAUR_CUM = new(19, "TIME_SINCE_LAST_CONSUMED_MINOTAUR_CUM"); // Time Since Last Minocum fix

        public static LegacyKFlag<int> MINOTAUR_CUM_ADDICTION_STATE = new(20, "MINOTAUR_CUM_ADDICTION_STATE"); // Current Addict State (0 = normal, 1= addicted, 2 = needy, 3= withdrawal, "MINOTAUR_CUM_ADDICTION_STATE")

        public static LegacyKFlag<int> MINOTAUR_CUM_REALLY_ADDICTED_STATE = new(21, "MINOTAUR_CUM_REALLY_ADDICTED_STATE"); // lust status affect timer

        public static LegacyKFlag<int> IN_COMBAT_USE_PLAYER_WAITED_FLAG = new(22, "IN_COMBAT_USE_PLAYER_WAITED_FLAG"); // Did PC Wait in combat?

        public static LegacyKFlag<int> PC_FETISH = new(23, "PC_FETISH"); // Fetish Level 1 - exhibitionism, 2 - bondage, 3 - pacifism

        public static LegacyKFlag<int> PC_MET_CERAPH = new(24, "PC_MET_CERAPH"); // Met Ceraph

        public static LegacyKFlag<int> INCREASED_HAIR_GROWTH_TIME_REMAINING = new(25, "INCREASED_HAIR_GROWTH_TIME_REMAINING"); // Increased Hair-growth Days Left

        public static LegacyKFlag<int> INCREASED_HAIR_GROWTH_SERUM_TIMES_APPLIED = new(26, "INCREASED_HAIR_GROWTH_SERUM_TIMES_APPLIED"); // Increased Hair-growth stack

        public static LegacyKFlag<int> CHEAT_ENTERING_COUNTER = new(27, "CHEAT_ENTERING_COUNTER"); // Cheat Input Counter

        public static LegacyKFlag<int> TIMES_CHEATED_COUNTER = new(28, "TIMES_CHEATED_COUNTER"); // Cheat Break Counter

        public static LegacyKFlag<float> URTA_PC_AFFECTION_COUNTER = new(29, "URTA_PC_AFFECTION_COUNTER"); // Urta love counter

        public static LegacyKFlag<int> URTA_PC_LOVE_COUNTER = new(30, "URTA_PC_LOVE_COUNTER"); // Urta Love Level

        public static LegacyKFlag<int> URTA_ANGRY_AT_PC_COUNTDOWN = new(31, "URTA_ANGRY_AT_PC_COUNTDOWN"); // Urta pissed countdown

        public static LegacyKFlag<int> URTA_KNOWS_PC_HAS_MARBLE_FOLLOWER = new(32, "URTA_KNOWS_PC_HAS_MARBLE_FOLLOWER"); // Marble Cockblock happened = new Flag<int>(0, "URTA_KNOWS_PC_HAS_MARBLE_FOLLOWER") - no, 1 = yes

        public static LegacyKFlag<int> PC_SEEN_URTA_BADASS_FIGHT = new(33, "PC_SEEN_URTA_BADASS_FIGHT"); // Seen Urta be a badass?

        public static LegacyKFlag<int> PC_ENCOUNTERED_CHRISTMAS_ELF_BEFORE = new(34, "PC_ENCOUNTERED_CHRISTMAS_ELF_BEFORE"); // Last year xmas elf encountered

        public static LegacyKFlag<int> AMILY_MET = new(35, "AMILY_MET"); //   (0 = not met, 1 = met, "AMILY_MET")Amily Met.     0=Not Met, 1=Met

        public static LegacyKFlag<int> AMILY_VILLAGE_ENCOUNTERS_DISABLED = new(36, "AMILY_VILLAGE_ENCOUNTERS_DISABLED"); //  1=true,44=village buttonAmily Village Encounters Disabled.     1=True, 44=Village Button?

        public static LegacyKFlag<int> AMILY_GROSSED_OUT_BY_WORMS = new(37, "AMILY_GROSSED_OUT_BY_WORMS"); //  1=freaked out-Amily encounters disabled due to worms 0=False, 1= True

        public static LegacyKFlag<int> AMILY_AFFECTION = new(38, "AMILY_AFFECTION"); //   (< 15 = low.  In between = medium. 40+= high affect, "AMILY_AFFECTION")Amily Affection.    15 = Low.  In Between = Medium. 40+= High Affect

        public static LegacyKFlag<int> AMILY_OFFER_ACCEPTED = new(39, "AMILY_OFFER_ACCEPTED"); //   (1 = true, 0 = not yet, "AMILY_OFFER_ACCEPTED")Amily Offer Accepted.    0=False, 1=True

        public static LegacyKFlag<int> AMILY_BIRTH_TOTAL = new(40, "AMILY_BIRTH_TOTAL"); // Amily Birth Total.

        public static LegacyKFlag<int> AMILY_INCUBATION = new(41, "AMILY_INCUBATION"); //    0 = not pregnant, otherwise hours till birth 168Amily Incubation.    0=Not Pregnant, Otherwise Hours Till Birth Starting From 168

        public static LegacyKFlag<int> AMILY_FUCK_COUNTER = new(42, "AMILY_FUCK_COUNTER"); // Amily Fuck Counter.

        public static LegacyKFlag<int> AMILY_FOLLOWER = new(43, "AMILY_FOLLOWER"); // Amily Follower. 0=Not Follower, 1=Follower 2=Corrupted Follower?

        public static LegacyKFlag<int> AMILY_VILLAGE_ACCESSIBLE = new(44, "AMILY_VILLAGE_ACCESSIBLE"); // Amily Village Accessible.    Related To 36]

        public static LegacyKFlag<int> AMILY_WANG_LENGTH = new(45, "AMILY_WANG_LENGTH"); // Amily's Wang Length" Description="Starts 4x1, maxes @ 13x2

        public static LegacyKFlag<float> AMILY_WANG_GIRTH = new(46, "AMILY_WANG_GIRTH"); // Amily Penis Girth.    Default=0

        public static LegacyKFlag<int> AMILY_CUP_SIZE = new(47, "AMILY_CUP_SIZE"); //  5-JanAmily's Cup Size (1 to 5, "AMILY_CUP_SIZE")

        public static LegacyKFlag<float> AMILY_NIPPLE_LENGTH = new(48, "AMILY_NIPPLE_LENGTH"); //  0.3-4Nipple Length from .3 to 4"

        public static LegacyKFlag<int> AMILY_HIP_RATING = new(49, "AMILY_HIP_RATING"); //  girly-womanlyAmily Hip Rating - girly to womanly - Default=12

        public static LegacyKFlag<int> AMILY_ASS_SIZE = new(50, "AMILY_ASS_SIZE"); // Can increase ass from "unremarkable ass" to "delightfully jiggly" - Default=12

        public static LegacyKFlag<int> AMILY_LACTATION_RATE = new(51, "AMILY_LACTATION_RATE"); // Amily Lactation Rate. Default=0

        public static LegacyKFlag<string> AMILY_CLOTHING = new(52, "AMILY_CLOTHING"); // Amily Clothing. Default="sexy rags"

        public static LegacyKFlag<int> LUMI_MET = new(53, "LUMI_MET"); // Lumi Met

        public static LegacyKFlag<int> NUMBER_OF_TIMES_MET_SCYLLA = new(54, "NUMBER_OF_TIMES_MET_SCYLLA"); // Met Scylla:   -1 = decline.  0 = not met. 1 = sucked once, 2 = sucked twice, etc.

        public static LegacyKFlag<int> TIMES_ENCOUNTED_TAMANIS_DAUGHTERS = new(55, "TIMES_ENCOUNTED_TAMANIS_DAUGHTERS"); // Times encountered Tamani's Daughters

        public static LegacyKFlag<int> TAMANI_TIMES_HYPNOTISED = new(56, "TAMANI_TIMES_HYPNOTISED"); // Tamani Hypno Level. Increases by 1 for each hypno event.  1-3 slight lust raises, 4-9 medium lust raises, 10-19 super high lust raises, 20+ high chance of autorape with special scene.

        public static LegacyKFlag<int> TAMANI_DAUGHTER_PREGGO_COUNTDOWN = new(57, "TAMANI_DAUGHTER_PREGGO_COUNTDOWN"); // Daughter Pregnancy Counter. They will not return until this countdown timer is 0. Same length as Tamani's incubation &#x2014; approx. 1 week.

        public static LegacyKFlag<int> TAMANI_DAUGHTER_CHAIR_COUNTER = new(58, "TAMANI_DAUGHTER_CHAIR_COUNTER"); // Times In Goblin Daughter 'CHAIR'

        public static LegacyKFlag<int> TIMES_FUCKED_TAMANIS_DAUGHTERS = new(59, "TIMES_FUCKED_TAMANIS_DAUGHTERS"); // TimeSleptWithDaughters

        public static LegacyKFlag<int> UNABLE_TO_MASTURBATE_BECAUSE_CENTAUR = new(60, "UNABLE_TO_MASTURBATE_BECAUSE_CENTAUR"); // Fail Masturbation As Centaur?

        public static LegacyKFlag<float> CERULEAN_POTION_BAD_END_FUTA_COUNTER = new(61, "CERULEAN_POTION_BAD_END_FUTA_COUNTER"); // Cerulean Potion Bad-End Futa Count

        public static LegacyKFlag<int> CERULEAN_POTION_NEUTER_ATTEMPTED = new(62, "CERULEAN_POTION_NEUTER_ATTEMPTED"); // Cerulean Potion Neuter Attempted

        public static LegacyKFlag<string> TEMP_STORAGE_SAVE_DELETION = new(63, "TEMP_STORAGE_SAVE_DELETION"); // Used as temp storage for deleting save slots

        public static LegacyKFlag<int> MARBLE_GROSSED_OUT_BECAUSE_WORM_INFESTATION = new(64, "MARBLE_GROSSED_OUT_BECAUSE_WORM_INFESTATION"); // Marble not sleeping with you due to worms

        public static LegacyKFlag<int> FORCE_BEE_TO_PRODUCE_HONEY = new(65, "FORCE_BEE_TO_PRODUCE_HONEY"); // Force Bee Honey Drop - Found Bee Honey.    1=Found

        public static LegacyKFlag<int> HAIR_GROWTH_STOPPED_BECAUSE_LIZARD = new(66, "HAIR_GROWTH_STOPPED_BECAUSE_LIZARD"); // Hair no longer grows due to lizard

        public static LegacyKFlag<int> TIMES_AUTOFELLATIO_DUE_TO_CAT_FLEXABILITY = new(67, "TIMES_AUTOFELLATIO_DUE_TO_CAT_FLEXABILITY"); // CAT: Autofellated

        public static LegacyKFlag<int> EDRYN_PREGNANCY_INCUBATION = new(68, "EDRYN_PREGNANCY_INCUBATION");

        public static LegacyKFlag<int> EDRYN_NUMBER_OF_KIDS = new(69, "EDRYN_NUMBER_OF_KIDS"); // EDRYN: Kids

        public static LegacyKFlag<int> EDRYN_NEVER_SEE_AGAIN = new(70, "EDRYN_NEVER_SEE_AGAIN"); // EDRYN: NEVER SEE AGAIN

        public static LegacyKFlag<int> EDRYN_PREGNANT_AND_NOT_TOLD_PC_YET = new(71, "EDRYN_PREGNANT_AND_NOT_TOLD_PC_YET"); // EDRYN: Pregnancy Panic happen?

        public static LegacyKFlag<int> EDRYN_NEEDS_TO_TALK_ABOUT_KID = new(72, "EDRYN_NEEDS_TO_TALK_ABOUT_KID"); // EDRYN: Gotta talk about kid

        public static LegacyKFlag<int> EDRYN_GIFT_COUNTER = new(73, "EDRYN_GIFT_COUNTER"); // EDRYN: GOOD HAND OUTS

        public static LegacyKFlag<int> AMILY_MET_PURE_JOJO = new(74, "AMILY_MET_PURE_JOJO"); // AMILY: Met pure Jojo

        public static LegacyKFlag<int> AMILY_SPAR_WITH_PURE_JOJO = new(75, "AMILY_SPAR_WITH_PURE_JOJO"); // AMILY: Spar with Jojo

        public static LegacyKFlag<int> AMILY_PISSED_PC_CORRUPED_JOJO = new(76, "AMILY_PISSED_PC_CORRUPED_JOJO"); // AMILY: Knew pure Jojo and is pissed that you 'broke' him.

        public static LegacyKFlag<int> AMILY_WAIT_FOR_PC_FIX_JOJO = new(77, "AMILY_WAIT_FOR_PC_FIX_JOJO"); // AMILY: PC agreed to 'fix' Jojo.  No sex until this is reset to 0.

        public static LegacyKFlag<int> AMILY_BLOCK_COUNTDOWN_BECAUSE_CORRUPTED_JOJO = new(78, "AMILY_BLOCK_COUNTDOWN_BECAUSE_CORRUPTED_JOJO"); // AMILY: Jojo fixing countdown - no encounters with Amily till it reaches 0.

        public static LegacyKFlag<int> JOJO_FIXED_STATUS = new(79, "JOJO_FIXED_STATUS"); // JOJO: 'FIXED' = new Flag<int>(1), 'FIXED + NOTE READ = new Flag<int>(2, "JOJO_FIXED_STATUS")

        public static LegacyKFlag<int> JOJO_DEAD_OR_GONE = new(80, "JOJO_DEAD_OR_GONE"); // JOJO DEAD OR GONE = new Flag<int>(1, "JOJO_DEAD_OR_GONE")

        public static LegacyKFlag<int> AMILY_DISCOVERED_TENTATLE_JOJO = new(81, "AMILY_DISCOVERED_TENTATLE_JOJO"); // AMILY: Discovered tentacle Jojo happened

        public static LegacyKFlag<int> AMILY_MET_RATHAZUL = new(82, "AMILY_MET_RATHAZUL"); // AMILY: Met Rathazul

        public static LegacyKFlag<int> RATHAZUL_CORRUPT_JOJO_FREAKOUT = new(83, "RATHAZUL_CORRUPT_JOJO_FREAKOUT"); // RATHAZUL: Corrupt Jojo freak-out!

        public static LegacyKFlag<int> JOJO_RATHAZUL_intERACTION_COUNTER = new(84, "JOJO_RATHAZUL_intERACTION_COUNTER"); // RATHAZUL: NON CORRUPT JOJO STUFF

        public static LegacyKFlag<int> MARBLE_OR_AMILY_FIRST_FOR_FREAKOUT = new(85, "MARBLE_OR_AMILY_FIRST_FOR_FREAKOUT"); // Which came first?  Marble(1) or Amily(2, "MARBLE_OR_AMILY_FIRST_FOR_FREAKOUT") - Freak-out at Camp if both Marble and Amily present.    1=true, 2=No freak-out

        public static LegacyKFlag<int> AMILY_NOT_FREAKED_OUT = new(86, "AMILY_NOT_FREAKED_OUT"); // Amily Freak-out Session

        public static LegacyKFlag<int> RATHAZUL_CAMP_intERACTION_COUNTDOWN = new(87, "RATHAZUL_CAMP_intERACTION_COUNTDOWN"); // Rathazul inter-follower countdown timer

        public static LegacyKFlag<int> DISCOVERED_HIGH_MOUNTAIN = new(88, "DISCOVERED_HIGH_MOUNTAIN"); // DISCOVER HIGH MOUNTAIN (Rangers, what a terrible TV show, "DISCOVERED_HIGH_MOUNTAIN")

        public static LegacyKFlag<float> COMBAT_BONUS_XP_VALUE = new(89, "COMBAT_BONUS_XP_VALUE"); // Sekrit Bonus XP Pass

        public static LegacyKFlag<int> MET_SOPHIE_COUNTER = new(90, "MET_SOPHIE_COUNTER"); // Met Sophie?

        public static LegacyKFlag<int> FUCKED_SOPHIE_COUNTER = new(91, "FUCKED_SOPHIE_COUNTER"); // Times Had Sex With Sophie

        public static LegacyKFlag<int> BREASTFEAD_SOPHIE_COUNTER = new(92, "BREASTFEAD_SOPHIE_COUNTER"); // Times Breastfed Sophie

        public static LegacyKFlag<int> SOPHIE_PREGNANCY_TYPE = new(93, "SOPHIE_PREGNANCY_TYPE"); // Was previously SOPHIE_WILD_EGG_COUNTDOWN_TIMER

        public static LegacyKFlag<int> SOPHIE_EGGS_LAID = new(94, "SOPHIE_EGGS_LAID"); // Sophie eggs laid

        public static LegacyKFlag<int> PC_CURRENTLY_LUSTSTICK_AFFECTED = new(95, "PC_CURRENTLY_LUSTSTICK_AFFECTED"); // N

        public static LegacyKFlag<int> SOPHIE_ANGRY_AT_PC_COUNTER = new(96, "SOPHIE_ANGRY_AT_PC_COUNTER"); // Sophie Pissed Off Counter

        public static LegacyKFlag<int> TIMES_PISSED_OFF_SOPHIE_COUNTER = new(97, "TIMES_PISSED_OFF_SOPHIE_COUNTER"); // Times Pissed Off Sophie

        public static LegacyKFlag<int> TIMES_FUCKED_SOPHIE_LESBIAN = new(98, "TIMES_FUCKED_SOPHIE_LESBIAN"); // Times Lesbo-Sexed
        //public static LegacyKFlag<int> EASY_MODE_ENABLE_FLAG:int = new LegacyKFlag<int>(99); // No longer used
        public static LegacyKFlag<int> CORRUPT_MARAE_FOLLOWUP_ENCOUNTER_STATE = new(100, "CORRUPT_MARAE_FOLLOWUP_ENCOUNTER_STATE"); // Corrupt Marae Follow-ups

        public static LegacyKFlag<int> FOUND_WIZARD_STAFF = new(101, "FOUND_WIZARD_STAFF"); // Found Wizard Staff

        public static LegacyKFlag<int> JOJO_LAST_MEDITATION = new(102, "JOJO_LAST_MEDITATION"); // Last time Jojo meditation used

        public static LegacyKFlag<int> TIMES_AUTOFELLATIOED_EXGARTUAN = new(103, "TIMES_AUTOFELLATIOED_EXGARTUAN"); // Times Autofellated Exgartuan

        public static LegacyKFlag<int> WHITNEY_GEMS_PAID_THIS_WEEK = new(104, "WHITNEY_GEMS_PAID_THIS_WEEK"); // Whitney Gems Paid Out That Week

        public static LegacyKFlag<int> TIMES_MET_SCYLLA_IN_ADDICTION_GROUP = new(105, "TIMES_MET_SCYLLA_IN_ADDICTION_GROUP"); // SCYLLA:  Times met Addiction Group

        public static LegacyKFlag<int> TIMES_SCYLLA_ADDICT_GROUP_EXPLOITED = new(106, "TIMES_SCYLLA_ADDICT_GROUP_EXPLOITED"); // SCYLLA: Times addict group taken advantage of

        public static LegacyKFlag<int> SCYLLA_TIMES_SHARED_IN_ADDICT_GROUP = new(107, "SCYLLA_TIMES_SHARED_IN_ADDICT_GROUP"); // SCYLLA: Times given the option to 'share' something

        public static LegacyKFlag<int> SCYLLA_MILK_THERAPY_TIMES = new(108, "SCYLLA_MILK_THERAPY_TIMES"); // SCYLLA: Milk therapy times

        public static LegacyKFlag<int> SCYLLA_CUM_THERAPY_TIMES = new(109, "SCYLLA_CUM_THERAPY_TIMES"); // SCYLLA: Cum therapy times

        public static LegacyKFlag<int> SCYLLA_SEX_THERAPY_TIMES = new(110, "SCYLLA_SEX_THERAPY_TIMES"); // SCYLLA: Sex therapy times

        public static LegacyKFlag<int> CERULEAN_SUCCUBUS_HERM_COUNTER = new(111, "CERULEAN_SUCCUBUS_HERM_COUNTER"); // Cerulean Succubus: Herm times used

        public static LegacyKFlag<int> USED_MILKER_TODAY = new(112, "USED_MILKER_TODAY"); // Used milkers today?

        public static LegacyKFlag<int> DISCOVERED_DUNGEON_2_ZETAZ = new(113, "DISCOVERED_DUNGEON_2_ZETAZ"); // Discovered Dungeon 2 (Zetaz's Lair, "DISCOVERED_DUNGEON_2_ZETAZ")

        public static LegacyKFlag<int> DEFEATED_ZETAZ = new(114, "DEFEATED_ZETAZ"); // Zetaz Defeated

        public static LegacyKFlag<int> ZETAZ_DOOR_UNLOCKED = new(115, "ZETAZ_DOOR_UNLOCKED"); // Zetaz' Door unlocked

        public static LegacyKFlag<int> ZETAZ_IMP_HORDE_DEFEATED = new(116, "ZETAZ_IMP_HORDE_DEFEATED"); // Imp Horde Defooted

        public static LegacyKFlag<int> ZETAZ_FUNGUS_ROOM_DEFEATED = new(117, "ZETAZ_FUNGUS_ROOM_DEFEATED"); // Fungus Room Defeated (1 = yes, "ZETAZ_FUNGUS_ROOM_DEFEATED")

        public static LegacyKFlag<int> SILLY_MODE_PONIES = new(118, "SILLY_MODE_PONIES"); // PONIES?!?!?!

        public static LegacyKFlag<int> FREED_VALA = new(119, "FREED_VALA"); // Vala Released?

        public static LegacyKFlag<int> WEEKLY_FAIRY_ORGY_COUNTDOWN = new(120, "WEEKLY_FAIRY_ORGY_COUNTDOWN"); // Weekly Faerie Orgy Countdown

        public static LegacyKFlag<int> ENCOUNTERED_VALA_AT_BAR = new(121, "ENCOUNTERED_VALA_AT_BAR"); // Vala Met At Bar?

        public static LegacyKFlag<int> FUCKED_VALA_AT_BAR_TODAY = new(122, "FUCKED_VALA_AT_BAR_TODAY"); // Fucked Vala Today @ Bar?

        public static LegacyKFlag<int> TIMES_FUCKED_VALA_IN_DUNGEON = new(123, "TIMES_FUCKED_VALA_IN_DUNGEON"); // Times Vala Used IN Dungeon

        public static LegacyKFlag<int> INVESTIGATED_VALA_AFTER_ZETAZ_DEFEATED = new(124, "INVESTIGATED_VALA_AFTER_ZETAZ_DEFEATED"); // Investigated Vala Since Zetaz downed?

        public static LegacyKFlag<int> TIME_SINCE_VALA_ATTEMPTED_RAPE_PC = new(125, "TIME_SINCE_VALA_ATTEMPTED_RAPE_PC"); // rape attempt

        public static LegacyKFlag<int> TIMES_PC_DEFEATED_VALA = new(126, "TIMES_PC_DEFEATED_VALA"); // Times beaten Vala into the dirt

        public static LegacyKFlag<int> TIMES_PC_DEFEATED_VALA_AND_RAEPED = new(127, "TIMES_PC_DEFEATED_VALA_AND_RAEPED"); // Times beat AND raped vala

        public static LegacyKFlag<int> ZETAZ_DEFEATED_AND_KILLED = new(128, "ZETAZ_DEFEATED_AND_KILLED"); // Zetaz Killed

        public static LegacyKFlag<int> ZETAZ_LAIR_TOOK_BONDAGE_STRAPS = new(129, "ZETAZ_LAIR_TOOK_BONDAGE_STRAPS"); // Bondage Straps Taken

        public static LegacyKFlag<int> ZETAZ_LAIR_DEMON_VENDOR_PRESENT = new(130, "ZETAZ_LAIR_DEMON_VENDOR_PRESENT"); // Sean Demon Vendor Here

        public static LegacyKFlag<int> TIMES_EXPLORED_PLAINS = new(131, "TIMES_EXPLORED_PLAINS"); // Times Explored Plains

        public static LegacyKFlag<int> MET_BUNBUN = new(132, "MET_BUNBUN"); // Met Bunbun

        public static LegacyKFlag<int> RAPHEAL_COUNTDOWN_TIMER = new(133, "RAPHEAL_COUNTDOWN_TIMER"); // Raphael Countdown (Time till Quiksilver Fawkes.  Set to -1 betrayed, set to -2 if covered for.

        public static LegacyKFlag<bool> RAPHAEL_MET = new(134, "RAPHAEL_MET"); // Raphael Met? (1 = Y, "RAPHAEL_MET")

        public static LegacyKFlag<bool> RAPHAEL_DRESS_GIVEN = new(135, "RAPHAEL_DRESS_GIVEN"); // Raphael gave dress

        public static LegacyKFlag<bool> REJECTED_RAPHAEL = new(136, "REJECTED_RAPHAEL"); // Raphael Reject?

        public static LegacyKFlag<int> RAPHAEL_RAPIER_TRANING = new(137, "RAPHAEL_RAPIER_TRANING"); // Raphael Rapier Training - OR? - Weapon strength post-newgame.

        public static LegacyKFlag<int> RAPHAEL_intELLIGENCE_TRAINING = new(138, "RAPHAEL_intELLIGENCE_TRAINING"); // Raphael intelligence Training

        public static LegacyKFlag<bool> RAPHAEL_DISGUSTED_BY_PC_APPEARANCE = new(139, "RAPHAEL_DISGUSTED_BY_PC_APPEARANCE"); // Raphael Disgusted by PC appearance
        //public static LegacyKFlag<int> RAPHAEL_SECOND_DATE:int = new LegacyKFlag<int>(140); // No longer used
        public static LegacyKFlag<int> HELLHOUND_MASTER_PROGRESS = new(141, "HELLHOUND_MASTER_PROGRESS"); // Hellhound Progression

        public static LegacyKFlag<int> SALON_MINOCUM_UNLOCKED = new(142, "SALON_MINOCUM_UNLOCKED"); // Can buy minotaur cum from goblins?

        public static LegacyKFlag<int> TIMES_CAUGHT_URTA_WITH_SCYLLA = new(143, "TIMES_CAUGHT_URTA_WITH_SCYLLA"); // How Many Times Have You Caught Urta + Scylla?

        public static LegacyKFlag<int> TIMES_ADVANTAGED_SCYLLA_MULTICOCKS = new(144, "TIMES_ADVANTAGED_SCYLLA_MULTICOCKS"); // Multitake Scylla meeting take advantage ever done?

        public static LegacyKFlag<int> URTA_CONFRONTED_SCYLLA = new(145, "URTA_CONFRONTED_SCYLLA"); // Urta confrontation about Scylla ever done?

        public static LegacyKFlag<int> URTA_DRINK_FREQUENCY = new(146, "URTA_DRINK_FREQUENCY"); // Urta drink toggle = new Flag<int>(0, "URTA_DRINK_FREQUENCY") is same, 1 is more, -1 is less.

        public static LegacyKFlag<int> URTA_BANNED_FROM_SCYLLA = new(147, "URTA_BANNED_FROM_SCYLLA"); // Is Urta banned from Scylla?  1 = yes.

        public static LegacyKFlag<bool> URTA_PAID_OUT_RAPHAELS_BOUNTY = new(148, "URTA_PAID_OUT_RAPHAELS_BOUNTY"); // Has Urta paid out Raphael's Bounty? (1 = yes, "URTA_PAID_OUT_RAPHAELS_BOUNTY")

        public static LegacyKFlag<bool> RAPHAEL_BONED = new(149, "RAPHAEL_BONED"); // Did you bone Raphael?

        public static LegacyKFlag<int> DOMINIKA_STAGE = new(150, "DOMINIKA_STAGE"); // Dominika Stage - 0=unknown, 1=met, 2=received, -1=blocked

        public static LegacyKFlag<int> DOMINIKA_SUCKED_OFF_LARGE_COCKS = new(151, "DOMINIKA_SUCKED_OFF_LARGE_COCKS"); // Times Dominika Sucked off Smaller dicks

        public static LegacyKFlag<int> DOMINIKA_SUCKED_OFF_SMALL_COCKS = new(152, "DOMINIKA_SUCKED_OFF_SMALL_COCKS"); // Times Dominika sucked off bigger dicks

        public static LegacyKFlag<int> DOMINIKA_VAGINAL_ORAL_RECEIVED = new(153, "DOMINIKA_VAGINAL_ORAL_RECEIVED"); // Times received vaginal oral from fellatrix

        public static LegacyKFlag<int> DOMINIKA_EMBARRASSED_ABOUT_MAGIC = new(154, "DOMINIKA_EMBARRASSED_ABOUT_MAGIC"); // Has fellatrix been embarrassed about your magic knowledge yet?

        public static LegacyKFlag<int> DOMINIKA_LEARNING_COOLDOWN = new(155, "DOMINIKA_LEARNING_COOLDOWN"); // Dominika learning cooldown

        public static LegacyKFlag<int> DOMINIKA_SUCKED_OFF_DOGCOCKS = new(156, "DOMINIKA_SUCKED_OFF_DOGCOCKS"); // Fellatrix - times sucked dogcock

        public static LegacyKFlag<int> DOMINIKA_SPECIAL_FOLLOWUP = new(157, "DOMINIKA_SPECIAL_FOLLOWUP"); // Fellatrix - Special follow-up happen? (1 = agree, 2 = eh, 3 disagree, add 3 for post-dream, "DOMINIKA_SPECIAL_FOLLOWUP")

        public static LegacyKFlag<int> AMILY_MET_AS = new(158, "AMILY_MET_AS"); // Amily remembers PC's gender at first meeting. Amily_Met_As:int=158;

        public static LegacyKFlag<int> AMILY_CONFESSED_LESBIAN = new(159, "AMILY_CONFESSED_LESBIAN"); //  1=yes,2=and accepted1=Yes,2=And Accepted

        public static LegacyKFlag<int> AMILY_TIMES_FUCKED_FEMPC = new(160, "AMILY_TIMES_FUCKED_FEMPC");

        public static LegacyKFlag<int> AMILY_HERM_TIMES_FUCKED_BY_FEMPC = new(161, "AMILY_HERM_TIMES_FUCKED_BY_FEMPC");

        public static LegacyKFlag<int> PC_TIMES_BIRTHED_AMILYKIDS = new(162, "PC_TIMES_BIRTHED_AMILYKIDS"); // Times PC has birthed Amily's brood - Pc_Times_Birthed_Amilykids:int=162;

        public static LegacyKFlag<int> PC_PENDING_PREGGERS = new(163, "PC_PENDING_PREGGERS"); //  1=yes,2=finished1=Yes,2=Finished

        public static LegacyKFlag<int> AMILY_PC_GENDER = new(164, "AMILY_PC_GENDER");

        public static LegacyKFlag<int> AMILY_HERM_QUEST = new(165, "AMILY_HERM_QUEST"); //  1=Amily flipped out, 2=accepted as Amily's dad1=Amily Flipped Out, 2=Accepted As Amily'S Dad

        public static LegacyKFlag<int> AMILY_ALLOWS_FERTILITY = new(166, "AMILY_ALLOWS_FERTILITY");

        public static LegacyKFlag<int> LIFETIME_GYM_MEMBER = new(167, "LIFETIME_GYM_MEMBER"); // Permanent Gym Membership? (1 = yes, "LIFETIME_GYM_MEMBER")

        public static LegacyKFlag<int> AMILY_CORRUPT_FLIPOUT = new(168, "AMILY_CORRUPT_FLIPOUT"); // Amily flip out about corruption yet?

        public static LegacyKFlag<int> AMILY_DRUG_MADE_COUNTER = new(169, "AMILY_DRUG_MADE_COUNTER"); // Times cooked Amily's Drug

        public static LegacyKFlag<int> AMILY_CORRUPTION = new(170, "AMILY_CORRUPTION"); // Amily's Corruption Value

        public static LegacyKFlag<int> AMILY_HAS_BALLS_AND_SIZE = new(171, "AMILY_HAS_BALLS_AND_SIZE"); // Amily's Balls: 1 = yes, higher numbers = bigger balls.

        public static LegacyKFlag<int> AMILY_VAGINAL_WETNESS = new(172, "AMILY_VAGINAL_WETNESS"); // Amily's Wetness

        public static LegacyKFlag<int> AMILY_CAMP_CORRUPTION_FREAKED = new(173, "AMILY_CAMP_CORRUPTION_FREAKED"); // In camp Amily warns you!  DUN DUN DUN! - Amily Freaked out about your corruption.    0=Not freaked out, 1=Freaked out

        public static LegacyKFlag<int> DOMINIKA_SUCKED_OFF_HORSECOCKS = new(174, "DOMINIKA_SUCKED_OFF_HORSECOCKS"); // Times Dominika Sucked off horsecocks

        public static LegacyKFlag<int> DOMINIKA_DRAMA = new(175, "DOMINIKA_DRAMA"); // Dominidrama - 1 = yes, 2 = no

        public static LegacyKFlag<int> TIMES_HAD_YOGA = new(176, "TIMES_HAD_YOGA"); // Cotton - TIMES HAD YOGA

        public static LegacyKFlag<int> COTTON_MET_FUCKED = new(177, "COTTON_MET_FUCKED"); // FUCKED - 0 = never met.  1 = met but not fucked. 2 = fucked

        public static LegacyKFlag<int> GYM_FEES_PAID_TODAY = new(178, "GYM_FEES_PAID_TODAY"); // Gym paid fees today?

        public static LegacyKFlag<int> JASUN_MET = new(179, "JASUN_MET"); // Met Jasun

        public static LegacyKFlag<int> JASUN_FUCKED_COUNTER = new(180, "JASUN_FUCKED_COUNTER"); // Times fucked Jasun

        public static LegacyKFlag<int> JASUN_NAME_LEARNED = new(181, "JASUN_NAME_LEARNED"); // Jasun name learned

        public static LegacyKFlag<int> GYM_MACHINE_STATUS = new(182, "GYM_MACHINE_STATUS"); // Gym Sex Machine Status (0 - not encountered, 1 = encountered, 2 = used.

        public static LegacyKFlag<int> TIMES_USED_SEX_MACHINE_AS_MALE = new(183, "TIMES_USED_SEX_MACHINE_AS_MALE"); //  Times Used Sex Machine As Male

        public static LegacyKFlag<int> TIMES_USED_SEX_MACHINE_AS_FEMALE = new(184, "TIMES_USED_SEX_MACHINE_AS_FEMALE"); //  Times Used Sex Machine As Female

        public static LegacyKFlag<int> TIMES_USED_SEX_MACHINE_AS_EUNUCH = new(185, "TIMES_USED_SEX_MACHINE_AS_EUNUCH"); //  Times Used Sex Machine As Genderless

        public static LegacyKFlag<int> TIMES_USED_SEX_MACHINE_AS_HERM = new(186, "TIMES_USED_SEX_MACHINE_AS_HERM"); //  Times Used Sex Machine As Herm

        public static LegacyKFlag<int> NEW_GAME_PLUS_BONUS_STORED_XP = new(187, "NEW_GAME_PLUS_BONUS_STORED_XP"); //  New Game Plus - Stored XP

        public static LegacyKFlag<int> NEW_GAME_PLUS_BONUS_STORED_ITEMS = new(188, "NEW_GAME_PLUS_BONUS_STORED_ITEMS"); //  New Game Plus - Stored Gems

        public static LegacyKFlag<int> NEW_GAME_PLUS_BONUS_WEAPON_NAME = new(189, "NEW_GAME_PLUS_BONUS_WEAPON_NAME"); //  NG+ Weapon name

        public static LegacyKFlag<int> NEW_GAME_PLUS_BONUS_WEAPON_VALUE = new(190, "NEW_GAME_PLUS_BONUS_WEAPON_VALUE"); //  NG+ Weapon Value

        public static LegacyKFlag<int> NEW_GAME_PLUS_BONUS_WEAPON_PERK = new(191, "NEW_GAME_PLUS_BONUS_WEAPON_PERK"); //  NG+ Weapon Perk

        public static LegacyKFlag<int> NEW_GAME_PLUS_BONUS_WEAPON_VERB = new(192, "NEW_GAME_PLUS_BONUS_WEAPON_VERB"); //  NG+ Weapon Verb

        public static LegacyKFlag<int> NEW_GAME_PLUS_BONUS_WEAPON_ATTACK = new(193, "NEW_GAME_PLUS_BONUS_WEAPON_ATTACK"); //  NG+ Weapon Attack

        public static LegacyKFlag<int> NEW_GAME_PLUS_BONUS_ARMOR_NAME = new(194, "NEW_GAME_PLUS_BONUS_ARMOR_NAME"); //  NG+ Armor Name

        public static LegacyKFlag<int> NEW_GAME_PLUS_BONUS_ARMOR_PERK = new(195, "NEW_GAME_PLUS_BONUS_ARMOR_PERK"); //  NG+ Armor Perk

        public static LegacyKFlag<int> NEW_GAME_PLUS_BONUS_ARMOR_DEFENSE = new(196, "NEW_GAME_PLUS_BONUS_ARMOR_DEFENSE"); //  NG+ ArmorDef

        public static LegacyKFlag<int> NEW_GAME_PLUS_BONUS_ARMOR_VALUE = new(197, "NEW_GAME_PLUS_BONUS_ARMOR_VALUE"); //  NG + Armor Value

        public static LegacyKFlag<int> TEMP_STORAGE_PASTRY_NAME = new(198, "TEMP_STORAGE_PASTRY_NAME"); //  Holding pattern for the name of the adversary

        public static LegacyKFlag<int> TEMP_STORAGE_PASTRY_PRICE = new(199, "TEMP_STORAGE_PASTRY_PRICE"); //  Holding pattern for unspeakable sums

        public static LegacyKFlag<int> MINOTAUR_CUM_ECLAIR_UNLOCKED = new(200, "MINOTAUR_CUM_ECLAIR_UNLOCKED"); //  Unlocked mino cum eclair

        public static LegacyKFlag<int> TIMES_VISITED_BAKERY = new(201, "TIMES_VISITED_BAKERY"); //  Times visited bakery

        public static LegacyKFlag<int> TIMES_FUCKED_HECKEL_BLOWJOB = new(202, "TIMES_FUCKED_HECKEL_BLOWJOB"); //  Times Heckel BJ

        public static LegacyKFlag<int> TIMES_FUCKED_HECKEL_ANAL = new(203, "TIMES_FUCKED_HECKEL_ANAL"); //  Times Heckel Anal

        public static LegacyKFlag<int> MET_HECKEL = new(204, "MET_HECKEL"); //  Met Heckel

        public static LegacyKFlag<int> MET_IFRIS = new(205, "MET_IFRIS"); //  Met Ifris

        public static LegacyKFlag<int> TIMES_FUCKED_IFRIS_BLOWJOB = new(206, "TIMES_FUCKED_IFRIS_BLOWJOB"); //  Ifris sucked?

        public static LegacyKFlag<int> TIMES_FUCKED_IFRIS_LICKED = new(207, "TIMES_FUCKED_IFRIS_LICKED"); //  Ifris licked?

        public static LegacyKFlag<int> IFRIS_SHOWED_OFF = new(208, "IFRIS_SHOWED_OFF"); //  Shown off for Ifris

        public static LegacyKFlag<int> HAVE_ENCOUNTERED_GNOLL_PLAINS = new(209, "HAVE_ENCOUNTERED_GNOLL_PLAINS"); //  New gnoll met?

        public static LegacyKFlag<int> BAZAAR_SLIPPERY_SQUEEZE_VISITED = new(210, "BAZAAR_SLIPPERY_SQUEEZE_VISITED"); //  Visited the Slippery Squeeze?

        public static LegacyKFlag<int> BAZAAR_ENTERED = new(211, "BAZAAR_ENTERED"); //  Entered the bizarre bazaar?

        public static LegacyKFlag<int> BAZAAR_ENCOUNTERED = new(212, "BAZAAR_ENCOUNTERED"); //  Found the bazaar previously?

        public static LegacyKFlag<int> CINNABAR_HOUSE_VISITED = new(213, "CINNABAR_HOUSE_VISITED"); //  -Been to Cinnabar's House?

        public static LegacyKFlag<int> CINNABAR_NUMBER_ENCOUNTERS = new(214, "CINNABAR_NUMBER_ENCOUNTERS"); //  -CINNABAR introduced?

        public static LegacyKFlag<int> CINNABAR_NUMBER_TIMES_FUCKED = new(215, "CINNABAR_NUMBER_TIMES_FUCKED"); //  -CINNABAR Times Fucked

        public static LegacyKFlag<int> CINNABAR_FUCKED_CORRUPT_PREVIOUSLY = new(216, "CINNABAR_FUCKED_CORRUPT_PREVIOUSLY"); //  -CINNABARWas last fuck corrupt fuck? - Cinnabar Hard Fucked.    0=No, 1=Yes

        public static LegacyKFlag<int> CERAPH_GENITAL_STEAL_CHOICE = new(217, "CERAPH_GENITAL_STEAL_CHOICE"); // Ceraph's genital stealing choice is stored here between functions

        public static LegacyKFlag<int> CERAPH_DICKS_OWNED = new(218, "CERAPH_DICKS_OWNED"); // Number of Dicks Ceraph owns

        public static LegacyKFlag<int> CERAPH_PUSSIES_OWNED = new(219, "CERAPH_PUSSIES_OWNED"); // Number of Pussies Ceraph owns

        public static LegacyKFlag<int> CERAPH_TITS_OWNED = new(220, "CERAPH_TITS_OWNED"); // Number of Tit's Ceraph owns

        public static LegacyKFlag<int> ROXANNE_MET = new(221, "ROXANNE_MET"); // -Met her?

        public static LegacyKFlag<int> ROXANNE_DRINING_CONTEST_WON = new(222, "ROXANNE_DRINING_CONTEST_WON"); // -Times Won Contest

        public static LegacyKFlag<int> ROXANNE_DRINING_CONTEST_LOST = new(223, "ROXANNE_DRINING_CONTEST_LOST"); // -Times Lost Contest

        public static LegacyKFlag<int> ROXANNE_DRINKING_CONTEST_LAST_WINNER = new(224, "ROXANNE_DRINKING_CONTEST_LAST_WINNER"); // -Who won contest last? (1 = pc, 2 = Strahza, "ROXANNE_DRINKING_CONTEST_LAST_WINNER")

        public static LegacyKFlag<int> ROXANNE_TIME_WITHOUT_SEX = new(225, "ROXANNE_TIME_WITHOUT_SEX"); // -How long has Strahza gone without sex? Reducto will probably reduce this.

        public static LegacyKFlag<int> ROXANNE_DRINKING_CONTEST_LOSE_ON_PURPOSE = new(226, "ROXANNE_DRINKING_CONTEST_LOSE_ON_PURPOSE"); // -Is PC losing the Roxanne's drinking contest intentionally?

        public static LegacyKFlag<int> ROXANNE_DRINKING_CONTEST_BONUS_SCORE = new(227, "ROXANNE_DRINKING_CONTEST_BONUS_SCORE"); // -Drinking Contest Bonus Score

        public static LegacyKFlag<int> DIAPAUSE_FLUID_AMOUNT = new(228, "DIAPAUSE_FLUID_AMOUNT"); // Diapause fluid store

        public static LegacyKFlag<int> DIAPAUSE_NEEDS_DISPLAYING = new(229, "DIAPAUSE_NEEDS_DISPLAYING"); // Diapause gain need displayed?

        public static LegacyKFlag<int> IZMA_ENCOUNTER_COUNTER = new(230, "IZMA_ENCOUNTER_COUNTER"); // Sharkgirls met? (Izma counter, "IZMA_ENCOUNTER_COUNTER")

        public static LegacyKFlag<int> IZMA_TIMES_FOUGHT_AND_WON = new(231, "IZMA_TIMES_FOUGHT_AND_WON"); // Izma fight

        public static LegacyKFlag<int> IZMA_TALKED_AT_LAKE = new(232, "IZMA_TALKED_AT_LAKE"); // Times Talked At shark pussy

        public static LegacyKFlag<int> IZMA_WORMS_SCARED = new(233, "IZMA_WORMS_SCARED"); // Izma worm scare (1 = Izma has been scared off by worms, "IZMA_WORMS_SCARED")

        public static LegacyKFlag<string> BONUS_ITEM_AFTER_COMBAT_ID = new(234, "BONUS_ITEM_AFTER_COMBAT_ID"); // Izma loot storage - Izma saved loot status

        public static LegacyKFlag<int> IZMA_GLOVES_TAKEN = new(235, "IZMA_GLOVES_TAKEN"); // Taken Izma gloves

        public static LegacyKFlag<int> IZMA_AMILY_FREAKOUT_STATUS = new(236, "IZMA_AMILY_FREAKOUT_STATUS"); // Izma + Amily Freak-out Status = (-1 = done, 0 = nothing yet, 1 = queued for camp prog, "IZMA_AMILY_FREAKOUT_STATUS")

        public static LegacyKFlag<int> IZMA_MARBLE_FREAKOUT_STATUS = new(237, "IZMA_MARBLE_FREAKOUT_STATUS"); // Izma + Marble Freak-out Status = (Above, "IZMA_MARBLE_FREAKOUT_STATUS")

        public static LegacyKFlag<int> IZMA_FOLLOWER_STATUS = new(238, "IZMA_FOLLOWER_STATUS"); // Izma Follower Toggle (-1 = stay at lake, 0 = neutral, 1 = live in camp, "IZMA_FOLLOWER_STATUS")

        public static LegacyKFlag<int> IZMA_TALK_LEVEL = new(239, "IZMA_TALK_LEVEL"); // Izma Talk level

        public static LegacyKFlag<int> MINO_CHEF_intRO = new(240, "MINO_CHEF_intRO"); // Mino Chef First Meeting Happen?

        public static LegacyKFlag<int> MINO_CHEF_EXPLAINED_INGREDIENTS = new(241, "MINO_CHEF_EXPLAINED_INGREDIENTS"); // mino explained what he needs yet?

        public static LegacyKFlag<int> MADDIE_STATUS = new(242, "MADDIE_STATUS"); // , 3 =stayed, 4 = epilogue'ed

        public static LegacyKFlag<int> MINO_CHEF_BAKERY_PROC_COUNTER = new(243, "MINO_CHEF_BAKERY_PROC_COUNTER"); // Counter for minotaur chef baker procs - Maddie present at bakery.    Mod%8=0, present

        public static LegacyKFlag<int> COTTON_UNUSUAL_YOGA_BOOK_TRACKER = new(244, "COTTON_UNUSUAL_YOGA_BOOK_TRACKER"); // Cotton Unusual Yoga Guidebook tracker

        public static LegacyKFlag<int> COTTON_BREAKFAST_CLUB = new(245, "COTTON_BREAKFAST_CLUB"); // Cotton Breakfast Club

        public static LegacyKFlag<int> IZMA_TIGERSHARK_TOOTH_COUNTDOWN = new(246, "IZMA_TIGERSHARK_TOOTH_COUNTDOWN"); // Tiger Tooth Countdown

        public static LegacyKFlag<int> GENDERLESS_CENTAUR_MADNESS = new(247, "GENDERLESS_CENTAUR_MADNESS"); // Genderless centaur madness

        public static LegacyKFlag<int> IZMA_PREGNANCY_DISCUSSED = new(248, "IZMA_PREGNANCY_DISCUSSED"); // Izma Preggers Discussion

        public static LegacyKFlag<int> IZMA_PREGNANCY_ENABLED = new(249, "IZMA_PREGNANCY_ENABLED"); // Pregnancy Enabled (1 = yes, "IZMA_PREGNANCY_ENABLED")

        public static LegacyKFlag<int> IZMA_INCUBATION = new(250, "IZMA_INCUBATION"); // Izma's Incubation timer

        public static LegacyKFlag<int> IZMA_CHILDREN_SHARKGIRLS = new(251, "IZMA_CHILDREN_SHARKGIRLS"); // Izma sharkgirls

        public static LegacyKFlag<int> IZMA_CHILDREN_TIGERSHARKS = new(252, "IZMA_CHILDREN_TIGERSHARKS"); // Izma tigersharks

        public static LegacyKFlag<int> IZMA_TIME_TILL_NEW_BOOK_AVAILABLE = new(253, "IZMA_TIME_TILL_NEW_BOOK_AVAILABLE"); // Izma Nu Book Countdown
        // public static LegacyKFlag<int> UNKNOWN_FLAG_NUMBER_00254 = new LegacyKFlag<int>(254);
        // public static LegacyKFlag<int> UNKNOWN_FLAG_NUMBER_00255 = new LegacyKFlag<int>(255);
        public static LegacyKFlag<int> ISABELLA_CAMP_APPROACHED = new(256, "ISABELLA_CAMP_APPROACHED"); // PC decided to approach Isabella's camp yet? 1

        public static LegacyKFlag<int> ISABELLA_MET = new(257, "ISABELLA_MET"); // Met Isabella?

        public static LegacyKFlag<int> ISABELLA_OKAY_WITH_TALL_FOLKS = new(258, "ISABELLA_OKAY_WITH_TALL_FOLKS"); // Is Isabella okay with tall folks?

        public static LegacyKFlag<int> ISABELLA_MET_SHORT_PC = new(259, "ISABELLA_MET_SHORT_PC"); // Has Isabella ever met the PC while PC is short?

        public static LegacyKFlag<int> ISABELLA_ANGRY_AT_PC_COUNTER = new(260, "ISABELLA_ANGRY_AT_PC_COUNTER"); // Isabella angry counter

        public static LegacyKFlag<int> ISABELLA_SLEEP_RAPE_COUNTER = new(261, "ISABELLA_SLEEP_RAPE_COUNTER"); // Isabella Sleep rape counter

        public static LegacyKFlag<int> ISABELLA_TIMES_TALKED = new(262, "ISABELLA_TIMES_TALKED"); // TImes talked with Isabella

        public static LegacyKFlag<int> NANCY_MET = new(263, "NANCY_MET"); // Met Nancy

        public static LegacyKFlag<int> NANCY_RELATIONSHIP_LEVEL = new(264, "NANCY_RELATIONSHIP_LEVEL"); // Nancy relationship level

        public static LegacyKFlag<int> NANCY_TIMES_TALKED = new(265, "NANCY_TIMES_TALKED"); // Talk Nancy

        public static LegacyKFlag<int> NANCY_TIMES_BONED = new(266, "NANCY_TIMES_BONED"); // Times boned
        //public static LegacyKFlag<int> LILIUM_TIMES_TALKED = new LegacyKFlag<int>(267); // No longer used
        //public static LegacyKFlag<int> PLAYER_DISARMED_WEAPON_ID:int = new LegacyKFlag<int>(268); // No longer used
        public static LegacyKFlag<int> PLAYER_DISARMED_WEAPON_ATTACK = new(269, "PLAYER_DISARMED_WEAPON_ATTACK"); // weapon damage

        public static LegacyKFlag<int> TIMES_ENCOUNTERED_FEMALE_SPIDERMORPHS = new(270, "TIMES_ENCOUNTERED_FEMALE_SPIDERMORPHS"); // Times encountered spiderbitches

        public static LegacyKFlag<int> FEMALE_SPIDERMORPH_PREGNANCY_INCUBATION = new(271, "FEMALE_SPIDERMORPH_PREGNANCY_INCUBATION"); // Spider Preggo Timer

        public static LegacyKFlag<int> TIMES_EXPLORED_SWAMP = new(272, "TIMES_EXPLORED_SWAMP"); // Times Explored Swamp
        //public static LegacyKFlag<int> SHOW_SPRITES_FLAG:int = new LegacyKFlag<int>(273); // No longer used
        public static LegacyKFlag<int> RATHAZUL_SILK_ARMOR_COUNTDOWN = new(274, "RATHAZUL_SILK_ARMOR_COUNTDOWN"); // Rathazul silk armor queue - 1 = armor done!

        public static LegacyKFlag<int> RATHAZUL_SILK_ARMOR_TYPE = new(275, "RATHAZUL_SILK_ARMOR_TYPE"); // Rathazul armor type selector - 1=armor, 2=robes (3=bra, 4=panties, 5=loincloth, "RATHAZUL_SILK_ARMOR_TYPE")

        public static LegacyKFlag<int> TIMES_ENCOUNTERED_BASILISK = new(276, "TIMES_ENCOUNTERED_BASILISK"); // Times Encountered Basilisk

        public static LegacyKFlag<int> TIMES_ENCOUNTERED_DRIDER = new(277, "TIMES_ENCOUNTERED_DRIDER"); // Times Encountered Drider

        public static LegacyKFlag<int> LOTTIE_DISABLED = new(278, "LOTTIE_DISABLED"); // LOTTIE DISABLED (1+ = YUS, "LOTTIE_DISABLED")

        public static LegacyKFlag<int> LOTTIE_FIGURE = new(279, "LOTTIE_FIGURE"); // LOTTIE FIGURE (HIGHER = SLIMMER, "LOTTIE_FIGURE")

        public static LegacyKFlag<int> LOTTIE_CONFIDENCE = new(280, "LOTTIE_CONFIDENCE"); // LOTTIE CONFIDENCE (HIGHER = AWESOMER, "LOTTIE_CONFIDENCE")

        public static LegacyKFlag<int> LOTTIE_ENCOUNTER_COUNTER = new(281, "LOTTIE_ENCOUNTER_COUNTER"); // TIMES ENCOUNTERED LOTTIE

        public static LegacyKFlag<int> SOPHIE_BIMBO = new(282, "SOPHIE_BIMBO"); // Sophie Bimbo Follower?

        public static LegacyKFlag<int> SOPHIE_DISABLED_FOREVER = new(283, "SOPHIE_DISABLED_FOREVER"); // Sophie Disabled Forever

        public static LegacyKFlag<int> TIMES_SOPHIE_HAS_DRUNK_OVI_ELIXIR = new(284, "TIMES_SOPHIE_HAS_DRUNK_OVI_ELIXIR"); // Has Ovi Event Procced?

        public static LegacyKFlag<int> LUSTSTICK_RESISTANCE_PROGRESS = new(285, "LUSTSTICK_RESISTANCE_PROGRESS"); // Luststick Resistance builder - decrements by 1 every day, incremented by 1 for every 2 hours of luststick

        public static LegacyKFlag<int> CERAPH_BELLYBUTTON_PIERCING = new(286, "CERAPH_BELLYBUTTON_PIERCING"); // Have Ceraph's belly-button piercing

        public static LegacyKFlag<int> CERAPH_TOKEN = new(287, "CERAPH_TOKEN"); // Carrying Careph's token?

        public static LegacyKFlag<int> CERAPH_HIDING_DICK = new(288, "CERAPH_HIDING_DICK"); // Ceraph Cock Hidden (1 = yes, 0 = cock floppin', "CERAPH_HIDING_DICK") - Ceraph's Demon Dong status 0=Visible, 1=Not visible

        public static LegacyKFlag<int> CERAPH_LICKED_YOU_OUT = new(289, "CERAPH_LICKED_YOU_OUT"); // Times Ceraph Licked You Out

        public static LegacyKFlag<int> CERAPH_PUNISHED = new(290, "CERAPH_PUNISHED"); // Punished Ceraph?

        public static LegacyKFlag<int> CERAPH_BEATEN_AND_RAPED_COUNTER = new(291, "CERAPH_BEATEN_AND_RAPED_COUNTER"); // Times Beaten Ceraph And Raped Her Without Losing

        public static LegacyKFlag<int> BAZAAR_DEMONS_LISTENED_IN = new(292, "BAZAAR_DEMONS_LISTENED_IN"); // Times listened in on demons

        public static LegacyKFlag<int> CERAPH_FAUX_CORRUPTION_DISABLED = new(293, "CERAPH_FAUX_CORRUPTION_DISABLED"); // Disabled Ceraph Faux Corruption - Ceraph Corruption Scene

        public static LegacyKFlag<int> CERAPH_SWEETIE_NO_RP_DONE = new(294, "CERAPH_SWEETIE_NO_RP_DONE"); // Times Done 'SWEETIE NO!' Ceraph Marble RP

        public static LegacyKFlag<int> CERAPH_SWEETIE_NO_RP_UTTER_MOO_DISABLED = new(295, "CERAPH_SWEETIE_NO_RP_UTTER_MOO_DISABLED"); // In Ceraph's SWEETIE NO!, is Utter Moo disabled?

        public static LegacyKFlag<int> CERAPH_UDDERS = new(296, "CERAPH_UDDERS"); // Ceraph Udder on or off? - Ceraph Has Udders.    0=No, 1=Yes

        public static LegacyKFlag<int> LOTTIE_LOVES_HER_BODY = new(297, "LOTTIE_LOVES_HER_BODY"); // Lottie love her bodY? (1 = yes, "LOTTIE_LOVES_HER_BODY")

        public static LegacyKFlag<int> LOTTIE_PORKED_COUNT = new(298, "LOTTIE_PORKED_COUNT"); // Lottie porked count? (1+ = yes, "LOTTIE_PORKED_COUNT")

        public static LegacyKFlag<int> LOTTIE_TRAINER = new(299, "LOTTIE_TRAINER"); // Lotties Trainer? (1 = yes, "LOTTIE_TRAINER")

        public static LegacyKFlag<int> LOTTIE_LAST_ENCOURAGEMENT_STATE = new(300, "LOTTIE_LAST_ENCOURAGEMENT_STATE"); // LOTTIE: Last encouragement state

        public static LegacyKFlag<int> LOTTIE_ENCOURAGEMENT_THRESHOLD_31_30_DOWN = new(301, "LOTTIE_ENCOURAGEMENT_THRESHOLD_31_30_DOWN"); // LOTTIE: encouragement go from 31-&gt;30 yet? (1 = yes, "LOTTIE_ENCOURAGEMENT_THRESHOLD_31_30_DOWN")

        public static LegacyKFlag<int> LOTTIE_ENCOURAGEMENT_THRESHOLD_30_31_UP = new(302, "LOTTIE_ENCOURAGEMENT_THRESHOLD_30_31_UP"); // LOTTIE: Encouragement moved from 30 to 31 yet? (1 = yes, "LOTTIE_ENCOURAGEMENT_THRESHOLD_30_31_UP")

        public static LegacyKFlag<int> LOTTIE_ENCOURAGEMENT_THRESHOLD_80_79_DOWN = new(303, "LOTTIE_ENCOURAGEMENT_THRESHOLD_80_79_DOWN"); // LOTTIE: Encouragement moved from 80 to 79 yet? (1 = yes, "LOTTIE_ENCOURAGEMENT_THRESHOLD_80_79_DOWN")

        public static LegacyKFlag<int> LOTTIE_ENCOURAGEMENT_THRESHOLD_79_80_UP = new(304, "LOTTIE_ENCOURAGEMENT_THRESHOLD_79_80_UP"); // LOTTIE: Encouragement moved from 79 to 80 yet? (1=yes, "LOTTIE_ENCOURAGEMENT_THRESHOLD_79_80_UP")
        //public static LegacyKFlag<int> SILLY_MODE_ENABLE_FLAG:int = new LegacyKFlag<int>(305); // No longer used
        public static LegacyKFlag<int> LOTTIE_NEEDS_TO_TALK_ORAL = new(306, "LOTTIE_NEEDS_TO_TALK_ORAL"); // LOTTIE: Need to talk about Oral?

        public static LegacyKFlag<int> LOTTIE_NEEDS_TO_TALK_ANAL = new(307, "LOTTIE_NEEDS_TO_TALK_ANAL"); // LOTTIE: Need to talk about Anal?

        public static LegacyKFlag<int> LOTTIE_WEIGHT_CONCLUSION = new(308, "LOTTIE_WEIGHT_CONCLUSION"); // LOTTIE: WEIGHT AT CONCLUSION OF LAST TALK EVENT

        public static LegacyKFlag<int> LOTTIE_FATASS_LOW_ESTEEM = new(309, "LOTTIE_FATASS_LOW_ESTEEM"); // LOTTIE: FATASS LOW SELF-ESTEEM SPECIAL EVENT TRIGGER YET? (1 = yes, "LOTTIE_FATASS_LOW_ESTEEM")

        public static LegacyKFlag<int> LOTTIE_DOGGYSTYLE_UNLOCKED = new(310, "LOTTIE_DOGGYSTYLE_UNLOCKED"); // LOTTIE: DOGGYSTYLE UNLOCKED? (1 = yes, "LOTTIE_DOGGYSTYLE_UNLOCKED")

        public static LegacyKFlag<int> LOTTIE_WALKIE_UNLOCKED = new(311, "LOTTIE_WALKIE_UNLOCKED"); // LOTTIE: WALKIES UNLOCKED? (1 = yes, "LOTTIE_WALKIE_UNLOCKED")

        public static LegacyKFlag<int> LOTTIE_LOVING_FAT_BITCH_EVENT = new(312, "LOTTIE_LOVING_FAT_BITCH_EVENT"); // LOTTIE: LOVING FAT BITCH 1TIME EVENT TRIGGER? (1 = yes, "LOTTIE_LOVING_FAT_BITCH_EVENT")

        public static LegacyKFlag<int> LOTTIE_COWGIRL_UNLOCKED = new(313, "LOTTIE_COWGIRL_UNLOCKED"); // LOTTIE: COWGIRL UNLOCKED? (1 = yes, "LOTTIE_COWGIRL_UNLOCKED")

        public static LegacyKFlag<int> LOTTIE_QUICKIE_UNLOCKED = new(314, "LOTTIE_QUICKIE_UNLOCKED"); // LOTTIE: QUICKIE UNLOCKED (1 = yes, "LOTTIE_QUICKIE_UNLOCKED")

        public static LegacyKFlag<int> LOTTIE_CHARLOTTES_WEB_EVENT = new(315, "LOTTIE_CHARLOTTES_WEB_EVENT"); // LOTTIE: CHARLOTTES WEB 1TIME HAPPEN (1 = yes, "LOTTIE_CHARLOTTES_WEB_EVENT")

        public static LegacyKFlag<int> LOTTIE_THREESOME_UNLOCKED = new(316, "LOTTIE_THREESOME_UNLOCKED"); // LOTTIE: THREESOME UNLOCKED (1 = yes, "LOTTIE_THREESOME_UNLOCKED")

        public static LegacyKFlag<int> LOTTIE_ELLE_NTR_UNLOCKED = new(317, "LOTTIE_ELLE_NTR_UNLOCKED"); // LOTTIE: ELLE NTR UNLOCKED (1 = yes, "LOTTIE_ELLE_NTR_UNLOCKED")

        public static LegacyKFlag<int> LOTTIE_MAX_GOAL_LOVE_REACHED = new(318, "LOTTIE_MAX_GOAL_LOVE_REACHED"); // LOTTIE: MAX GOALS AND MAX LOVE REACHED (1 = yes, "LOTTIE_MAX_GOAL_LOVE_REACHED")

        public static LegacyKFlag<int> LOTTIE_FEMDOM_UNLOCKED = new(319, "LOTTIE_FEMDOM_UNLOCKED"); // LOTTIE: FEMDOM UNLOCKED (1 = yes, "LOTTIE_FEMDOM_UNLOCKED")

        public static LegacyKFlag<int> LOTTIE_REVERSE_COWGIRL_UNLOCKED = new(320, "LOTTIE_REVERSE_COWGIRL_UNLOCKED"); // LOTTIE: REVERSE COWGIRL UNLOCKED (1 = yes, "LOTTIE_REVERSE_COWGIRL_UNLOCKED")

        public static LegacyKFlag<int> LOTTIE_TIMES_GIVEN_ITEMS = new(321, "LOTTIE_TIMES_GIVEN_ITEMS"); // LOTTIE: TImes given item

        public static LegacyKFlag<int> LOTTIE_LABOVA_COUNTDOWN = new(322, "LOTTIE_LABOVA_COUNTDOWN"); // LOTTIE: LA BOVA COUNTDOWN - counts down to 1 before she'll use it again.  Also used to track if LaBova has ever been used

        public static LegacyKFlag<int> LOTTIE_REDUCTO_USED = new(323, "LOTTIE_REDUCTO_USED"); // LOTTIE: HAS SHE USED REDUCTO?

        public static LegacyKFlag<int> LOTTIE_HAM_SAMMICH = new(324, "LOTTIE_HAM_SAMMICH"); // LOTTIE: HAM SAMMICH

        public static LegacyKFlag<int> TIMES_ENCOUNTERED_MINOTAUR_MOB = new(325, "TIMES_ENCOUNTERED_MINOTAUR_MOB"); // Times met minotaur mob

        public static LegacyKFlag<int> ADULT_MINOTAUR_OFFSPRINGS = new(326, "ADULT_MINOTAUR_OFFSPRINGS"); // Number of grown up minotaur offspring - TRIBE SIZE

        public static LegacyKFlag<int> MINOTAUR_SONS_PENDING = new(327, "MINOTAUR_SONS_PENDING"); // Number of minotaur sons pending growing up

        public static LegacyKFlag<int> MINOTAUR_SONS_GROWUP_COUNTER = new(328, "MINOTAUR_SONS_GROWUP_COUNTER"); // Grow up countdown

        public static LegacyKFlag<int> MINOTAUR_SONS_WASTED_TURN = new(329, "MINOTAUR_SONS_WASTED_TURN"); // Minos wasting a turn? (1 = yes, "MINOTAUR_SONS_WASTED_TURN")

        public static LegacyKFlag<int> MINOTAUR_SONS_CUM_REPEAT_COOLDOWN = new(330, "MINOTAUR_SONS_CUM_REPEAT_COOLDOWN"); // Cooldown on minotaur cum repeat messages

        public static LegacyKFlag<int> JOJO_COCK_MILKING_COUNTER = new(331, "JOJO_COCK_MILKING_COUNTER"); // Times Jojo Milked

        public static LegacyKFlag<int> JOJO_COCK_MILKING_COOLDOWN = new(332, "JOJO_COCK_MILKING_COOLDOWN"); // Jojo milk cooldown 4 hours

        public static LegacyKFlag<int> PC_COCK_MILKED_COUNTER = new(333, "PC_COCK_MILKED_COUNTER"); // Times PC been cock milked

        public static LegacyKFlag<int> FAP_ARENA_SESSIONS = new(334, "FAP_ARENA_SESSIONS"); // Times fap arena'ed

        public static LegacyKFlag<int> FAP_ARENA_VICTORIES = new(335, "FAP_ARENA_VICTORIES"); // Fap Arena victories

        public static LegacyKFlag<int> AMILY_OFFERED_DEFURRY = new(336, "AMILY_OFFERED_DEFURRY"); //  1 = Offered to defurry Amily

        public static LegacyKFlag<int> AMILY_NOT_FURRY = new(337, "AMILY_NOT_FURRY"); //  1 = Amily is no longer a flea-ridden furry who stinks up your carpet.

        public static LegacyKFlag<int> AMILY_IS_BATMAN = new(338, "AMILY_IS_BATMAN"); //  1 = You turned Amily into a human and then pissed all over her happy thoughts.  She now stalks you from rooftops while buying graphite helmets, utility belts, and a sweet, jetpowered car in the theme of a rat.

        public static LegacyKFlag<int> FAP_ARENA_RULES_EXPLAINED = new(339, "FAP_ARENA_RULES_EXPLAINED"); // Fap Arena - rules been explained?

        public static LegacyKFlag<int> MINOTAUR_CUM_intAKE_COUNT = new(340, "MINOTAUR_CUM_intAKE_COUNT"); // came in with minotaur cum

        public static LegacyKFlag<int> KIHA_TOLL = new(341, "KIHA_TOLL");

        public static LegacyKFlag<int> KIHA_TOLL_DURATION = new(342, "KIHA_TOLL_DURATION");

        public static LegacyKFlag<int> TIMES_MET_KIHA = new(343, "TIMES_MET_KIHA");

        public static LegacyKFlag<int> KIHA_TALK_STAGE = new(344, "KIHA_TALK_STAGE");

        public static LegacyKFlag<int> PC_WIN_LAST_KIHA_FIGHT = new(345, "PC_WIN_LAST_KIHA_FIGHT");

        public static LegacyKFlag<int> AMILY_VISITING_URTA = new(346, "AMILY_VISITING_URTA"); // AMILY_VISITING_URTA (1 = Amily is gone, 2 = they fucked), (3 = DICK MODE) (4 = THREESOME MODE, "AMILY_VISITING_URTA")

        public static LegacyKFlag<int> AMILY_NEED_TO_FREAK_ABOUT_URTA = new(347, "AMILY_NEED_TO_FREAK_ABOUT_URTA"); // AMILY_NEED_TO_FREAK_ABOUT_URTA (1= yes, "AMILY_NEED_TO_FREAK_ABOUT_URTA")

        public static LegacyKFlag<int> JOEY_BIG_BALLS_COUNTER = new(348, "JOEY_BIG_BALLS_COUNTER"); // Joey Big balls counter

        public static LegacyKFlag<int> JOEY_TOLD_TO_MASTURBATE_COUNTER = new(349, "JOEY_TOLD_TO_MASTURBATE_COUNTER"); // Times told joey to masturbate his big balls.

        public static LegacyKFlag<int> URTA_AMILY_FOLLOWUP_NEEDED = new(350, "URTA_AMILY_FOLLOWUP_NEEDED"); // Urta x Amily Urta follow-up needed?  (1 = cue Urta angst, 2 = done, "URTA_AMILY_FOLLOWUP_NEEDED")

        public static LegacyKFlag<int> TIMES_MET_SHOULDRA = new(351, "TIMES_MET_SHOULDRA");

        public static LegacyKFlag<int> TIMES_BEATEN_SHOULDRA = new(352, "TIMES_BEATEN_SHOULDRA");

        public static LegacyKFlag<int> TIMES_POSSESSED_BY_SHOULDRA = new(353, "TIMES_POSSESSED_BY_SHOULDRA");

        public static LegacyKFlag<int> TIMED_SHARKGINAS = new(354, "TIMED_SHARKGINAS");

        public static LegacyKFlag<int> TIMES_SHARKPENISED = new(355, "TIMES_SHARKPENISED");

        public static LegacyKFlag<int> SHOULDRA_SLIME_PENOR_TIMES = new(356, "SHOULDRA_SLIME_PENOR_TIMES");

        public static LegacyKFlag<int> SHOULDRA_GENDERLESS_FUCK_COUNT = new(357, "SHOULDRA_GENDERLESS_FUCK_COUNT");

        public static LegacyKFlag<int> SHOULDRA_PENIS_DEFEAT_TIMES = new(358, "SHOULDRA_PENIS_DEFEAT_TIMES");

        public static LegacyKFlag<int> SHOULDRA_WORM_SCENE_COUNTER = new(359, "SHOULDRA_WORM_SCENE_COUNTER");

        public static LegacyKFlag<int> SHOULDRA_EXGARTUAN_SPIRIT_SEX_COUNT = new(360, "SHOULDRA_EXGARTUAN_SPIRIT_SEX_COUNT");

        public static LegacyKFlag<int> SHOULDRA_VAGINAL_POSSESSIONS = new(361, "SHOULDRA_VAGINAL_POSSESSIONS");

        public static LegacyKFlag<int> SHOULDRA_HERMSEX_COUNT = new(362, "SHOULDRA_HERMSEX_COUNT");

        public static LegacyKFlag<int> SHOULDRA_USES_YOUR_GIANT_COCK_COUNT = new(363, "SHOULDRA_USES_YOUR_GIANT_COCK_COUNT");

        public static LegacyKFlag<int> TIMES_MET_OOZE = new(364, "TIMES_MET_OOZE");

        public static LegacyKFlag<int> SHOULDRA_PALADIN_MAIDEN_COUNTDOWN = new(365, "SHOULDRA_PALADIN_MAIDEN_COUNTDOWN"); // COUNTDOWN TILL MAIDEN FOLLOWUP

        public static LegacyKFlag<int> SHOULDRA_PALADIN_MAIDEN_COUNTER = new(366, "SHOULDRA_PALADIN_MAIDEN_COUNTER"); // TIMES MET GHOSTGIRL MAIDEN

        public static LegacyKFlag<int> ISABELLA_TIMES_OFFERED_FOLLOWER = new(367, "ISABELLA_TIMES_OFFERED_FOLLOWER");

        public static LegacyKFlag<int> ISABELLA_AFFECTION = new(368, "ISABELLA_AFFECTION");

        public static LegacyKFlag<int> ISABELLA_FOLLOWER_ACCEPTED = new(369, "ISABELLA_FOLLOWER_ACCEPTED");

        public static LegacyKFlag<int> ISABELLA_PLAINS_DISABLED = new(370, "ISABELLA_PLAINS_DISABLED");

        public static LegacyKFlag<int> ISABELLA_CAMP_DISABLED = new(371, "ISABELLA_CAMP_DISABLED");

        public static LegacyKFlag<int> ISABELLA_ACCENT_TRAINING_PERCENT = new(372, "ISABELLA_ACCENT_TRAINING_PERCENT");

        public static LegacyKFlag<int> ISABELLA_ACCENT_TRAINING_COOLDOWN = new(373, "ISABELLA_ACCENT_TRAINING_COOLDOWN");

        public static LegacyKFlag<int> ISABELLA_ACCENT_FORCED_ON = new(374, "ISABELLA_ACCENT_FORCED_ON");

        public static LegacyKFlag<int> ISABELLA_MILKED_YET = new(375, "ISABELLA_MILKED_YET");

        public static LegacyKFlag<int> ISABELLA_BLOWJOBS_DISABLED = new(376, "ISABELLA_BLOWJOBS_DISABLED");

        public static LegacyKFlag<int> ISABELLA_MORNING_FELLATIO_COUNT = new(377, "ISABELLA_MORNING_FELLATIO_COUNT");

        public static LegacyKFlag<int> ISABELLA_TIME_SINCE_LAST_HOTDOGGING = new(378, "ISABELLA_TIME_SINCE_LAST_HOTDOGGING");

        public static LegacyKFlag<int> ISABELLA_TIMES_HOTDOGGED = new(379, "ISABELLA_TIMES_HOTDOGGED");

        public static LegacyKFlag<int> ISABELLA_MILK_COOLDOWN = new(380, "ISABELLA_MILK_COOLDOWN");

        public static LegacyKFlag<int> ISABELLA_MURBLE_BLEH = new(381, "ISABELLA_MURBLE_BLEH");

        public static LegacyKFlag<int> ISABELLA_SPARRING_intRO = new(382, "ISABELLA_SPARRING_intRO");

        public static LegacyKFlag<int> ISABELLA_PROBOVA_BURP_COUNT = new(383, "ISABELLA_PROBOVA_BURP_COUNT");

        public static LegacyKFlag<int> GOOGIRL_BIRTHS = new(384, "GOOGIRL_BIRTHS");

        public static LegacyKFlag<int> GOOGIRL_CONSECUTIVE_LOSSES = new(385, "GOOGIRL_CONSECUTIVE_LOSSES");

        public static LegacyKFlag<int> DOMINIKA_TIMES_MULTICOCK_SLOBBERED = new(386, "DOMINIKA_TIMES_MULTICOCK_SLOBBERED");

        public static LegacyKFlag<int> DOMINIKA_TIMES_HYPNO_BJ = new(387, "DOMINIKA_TIMES_HYPNO_BJ");

        public static LegacyKFlag<int> DOMINIKA_LAST_HYPNO_SUCK_COUNT = new(388, "DOMINIKA_LAST_HYPNO_SUCK_COUNT");

        public static LegacyKFlag<int> CERAPH_ROLEPLAY_AS_DOMINIKA_COUNT = new(389, "CERAPH_ROLEPLAY_AS_DOMINIKA_COUNT");

        public static LegacyKFlag<int> HEL_TALKED_ABOUT_BERSERKING = new(390, "HEL_TALKED_ABOUT_BERSERKING");

        public static LegacyKFlag<int> HEL_TALKED_ABOUT_HER = new(391, "HEL_TALKED_ABOUT_HER");

        public static LegacyKFlag<int> HEL_TALKED_ABOUT_ATTACKING_YOU = new(392, "HEL_TALKED_ABOUT_ATTACKING_YOU");

        public static LegacyKFlag<int> HEL_FUCKBUDDY = new(393, "HEL_FUCKBUDDY");

        public static LegacyKFlag<int> HEL_FUCK_COUNTER = new(394, "HEL_FUCK_COUNTER");

        public static LegacyKFlag<int> HEL_AFFECTION = new(395, "HEL_AFFECTION");

        public static LegacyKFlag<int> HEL_TIMES_ENCOUNTERED = new(396, "HEL_TIMES_ENCOUNTERED");

        public static LegacyKFlag<int> HEL_ISABELLA_THREESOME_ENABLED = new(397, "HEL_ISABELLA_THREESOME_ENABLED");

        public static LegacyKFlag<int> KATHERINE_UNLOCKED = new(398, "KATHERINE_UNLOCKED");

        public static LegacyKFlag<int> KATHERINE_DICK_COUNT = new(399, "KATHERINE_DICK_COUNT");

        public static LegacyKFlag<int> KATHERINE_DICK_LENGTH = new(400, "KATHERINE_DICK_LENGTH");

        public static LegacyKFlag<int> KATHERINE_KNOT_THICKNESS = new(401, "KATHERINE_KNOT_THICKNESS");

        public static LegacyKFlag<int> KATHERINE_BALL_SIZE = new(402, "KATHERINE_BALL_SIZE");

        public static LegacyKFlag<int> KATHERINE_TIMES_SEXED = new(403, "KATHERINE_TIMES_SEXED");

        public static LegacyKFlag<int> EDRYN_TIMES_HEL_THREESOMED = new(404, "EDRYN_TIMES_HEL_THREESOMED");

        public static LegacyKFlag<int> HEL_EDRYN_OFFER = new(405, "HEL_EDRYN_OFFER");

        public static LegacyKFlag<int> ZETSUKO_MET = new(406, "ZETSUKO_MET");

        public static LegacyKFlag<int> ROGAR_PHASE = new(407, "ROGAR_PHASE");

        public static LegacyKFlag<int> ROGAR_DIRT = new(408, "ROGAR_DIRT");

        public static LegacyKFlag<int> ROGAR_WARNING = new(409, "ROGAR_WARNING");

        public static LegacyKFlag<int> ROGAR_DISABLED = new(410, "ROGAR_DISABLED");

        public static LegacyKFlag<int> ROGAR_FUCKED_TODAY = new(411, "ROGAR_FUCKED_TODAY");

        public static LegacyKFlag<int> IZMA_KIDS_IN_WILD = new(412, "IZMA_KIDS_IN_WILD"); // IZMA_KIDS_IN_THE_WILD

        public static LegacyKFlag<int> EXGARTUAN_TIGHTPANTS_MASTURBATE_COUNT = new(413, "EXGARTUAN_TIGHTPANTS_MASTURBATE_COUNT");

        public static LegacyKFlag<int> BOOBGARTUAN_SURPRISE_COUNT = new(414, "BOOBGARTUAN_SURPRISE_COUNT");

        public static LegacyKFlag<int> GOTTEN_INQUISITOR_ARMOR = new(415, "GOTTEN_INQUISITOR_ARMOR");

        public static LegacyKFlag<int> DOMINIKAS_SWORD_GIVEN = new(416, "DOMINIKAS_SWORD_GIVEN");

        public static LegacyKFlag<int> HEL_FOXY_FOURSOME_WARNED = new(417, "HEL_FOXY_FOURSOME_WARNED");

        public static LegacyKFlag<int> HISTORY_PERK_SELECTED = new(418, "HISTORY_PERK_SELECTED"); // HISTORY PERK SET - tail, fur, or scales.    0=Nothing

        public static LegacyKFlag<int> AMILY_TIMES_BUTTFUCKED_PC = new(419, "AMILY_TIMES_BUTTFUCKED_PC");

        public static LegacyKFlag<int> TIMES_FUCKED_AMILYBUTT = new(420, "TIMES_FUCKED_AMILYBUTT");

        public static LegacyKFlag<int> KIHA_AFFECTION_LEVEL = new(421, "KIHA_AFFECTION_LEVEL");

        public static LegacyKFlag<int> KIHA_AFFECTION = new(422, "KIHA_AFFECTION");

        public static LegacyKFlag<int> KIHA_CORRUPTION_BITCH = new(423, "KIHA_CORRUPTION_BITCH");

        public static LegacyKFlag<int> KIHA_NEED_SPIDER_TEXT = new(424, "KIHA_NEED_SPIDER_TEXT");

        public static LegacyKFlag<int> KIHA_AND_HEL_WHOOPIE = new(425, "KIHA_AND_HEL_WHOOPIE");

        public static LegacyKFlag<int> KIHA_ADMITTED_WARM_FEELINZ = new(426, "KIHA_ADMITTED_WARM_FEELINZ"); // KIHA_CERVIXGINITY_TAKEN

        public static LegacyKFlag<int> KIHA_MOVE_IN_OFFER = new(427, "KIHA_MOVE_IN_OFFER");

        public static LegacyKFlag<int> KIHA_FOLLOWER = new(428, "KIHA_FOLLOWER");

        public static LegacyKFlag<int> KIHA_NEEDS_TO_REACT_TO_HORSECOCKING = new(429, "KIHA_NEEDS_TO_REACT_TO_HORSECOCKING");

        public static LegacyKFlag<int> KIHA_CERVIXGINITY_TAKEN = new(430, "KIHA_CERVIXGINITY_TAKEN");

        public static LegacyKFlag<int> KIHA_HORSECOCK_FUCKED = new(431, "KIHA_HORSECOCK_FUCKED");

        public static LegacyKFlag<int> KIHA_CHOKED_OUT_PC = new(432, "KIHA_CHOKED_OUT_PC");

        public static LegacyKFlag<int> VALA_CUMBATH_TIMES = new(433, "VALA_CUMBATH_TIMES");

        public static LegacyKFlag<int> TIMES_AMILY_AND_JOJO_PLAYED_TIMES = new(434, "TIMES_AMILY_AND_JOJO_PLAYED_TIMES");

        public static LegacyKFlag<int> AMILY_X_JOJO_COOLDOWN = new(435, "AMILY_X_JOJO_COOLDOWN");

        public static LegacyKFlag<int> AMILY_INCEST_COUNTDOWN_TIMER = new(436, "AMILY_INCEST_COUNTDOWN_TIMER"); // 1 = timer started, 30 days = RIPE FOR INCEST

        public static LegacyKFlag<int> YVONNE_FUCK_COUNTER = new(437, "YVONNE_FUCK_COUNTER");

        public static LegacyKFlag<int> TIMES_CERAPH_PORTAL_FUCKED = new(438, "TIMES_CERAPH_PORTAL_FUCKED");

        public static LegacyKFlag<int> IZMA_NO_COCK = new(439, "IZMA_NO_COCK");

        public static LegacyKFlag<int> ASKED_IZMA_ABOUT_WANG_REMOVAL = new(440, "ASKED_IZMA_ABOUT_WANG_REMOVAL");

        public static LegacyKFlag<int> SALON_PAID = new(441, "SALON_PAID");

        public static LegacyKFlag<int> URTA_FAMILY_TALK_ATTEMPTS = new(442, "URTA_FAMILY_TALK_ATTEMPTS");

        public static LegacyKFlag<int> URTA_OPEN_ABOUT_EDRYN = new(443, "URTA_OPEN_ABOUT_EDRYN");

        public static LegacyKFlag<int> PC_DISLIKES_URTA_AND_EDRYN_TOGETHER = new(444, "PC_DISLIKES_URTA_AND_EDRYN_TOGETHER");

        public static LegacyKFlag<int> DISCUSSED_URTA_ALCOHOLISM = new(445, "DISCUSSED_URTA_ALCOHOLISM");

        public static LegacyKFlag<int> MET_NIAMH = new(446, "MET_NIAMH");

        public static LegacyKFlag<int> GOT_NIAMH_BEER = new(447, "GOT_NIAMH_BEER");

        public static LegacyKFlag<int> TALKED_NIAMH = new(448, "TALKED_NIAMH");

        public static LegacyKFlag<int> NIAMH_MOVED_OUT_COUNTER = new(449, "NIAMH_MOVED_OUT_COUNTER");

        public static LegacyKFlag<int> NIAMH_STATUS = new(450, "NIAMH_STATUS");

        public static LegacyKFlag<int> NIAMH_SEAN_BREW_BIMBO_LIQUEUR_COUNTER = new(451, "NIAMH_SEAN_BREW_BIMBO_LIQUEUR_COUNTER"); // NIAMH_SEAN_BREW_BIMBO_LIQUEUR_COUNTER" Description="1 = ready!

        public static LegacyKFlag<int> TIMES_NIAMH_BAZAAR_MET = new(452, "TIMES_NIAMH_BAZAAR_MET");

        public static LegacyKFlag<int> TIMES_MET_ANEMONE = new(453, "TIMES_MET_ANEMONE");

        public static LegacyKFlag<int> ANEMONE_KID = new(454, "ANEMONE_KID");

        public static LegacyKFlag<int> KID_ITEM_FIND_HOURS = new(455, "KID_ITEM_FIND_HOURS");

        public static LegacyKFlag<int> ANEMONE_WATCH = new(456, "ANEMONE_WATCH");

        public static LegacyKFlag<string> ANEMONE_WEAPON_ID = new(457, "ANEMONE_WEAPON_ID");

        public static LegacyKFlag<int> MURBLE_FARM_TALK_LEVELS = new(458, "MURBLE_FARM_TALK_LEVELS");

        public static LegacyKFlag<int> BROKE_UP_WITH_MARBLE = new(459, "BROKE_UP_WITH_MARBLE");

        public static LegacyKFlag<int> MARBLE_PLAYED_WITH_KIDS_TODAY = new(460, "MARBLE_PLAYED_WITH_KIDS_TODAY");

        public static LegacyKFlag<int> MARBLE_CAMPTALK_LEVEL = new(461, "MARBLE_CAMPTALK_LEVEL");

        public static LegacyKFlag<int> MARBLE_TELADRE_STORY = new(462, "MARBLE_TELADRE_STORY");

        public static LegacyKFlag<int> MARBLE_WARNING = new(463, "MARBLE_WARNING");

        public static LegacyKFlag<int> FARM_DISABLED = new(464, "FARM_DISABLED");

        public static LegacyKFlag<int> MARBLE_BOVA_LEVEL = new(465, "MARBLE_BOVA_LEVEL");

        public static LegacyKFlag<int> JOEY_OFFERED_MILKER = new(466, "JOEY_OFFERED_MILKER");

        public static LegacyKFlag<int> ANTS_PC_FAILED_PHYLLA = new(467, "ANTS_PC_FAILED_PHYLLA");

        public static LegacyKFlag<int> ANT_COLONY_KEPT_HIDDEN = new(468, "ANT_COLONY_KEPT_HIDDEN");

        public static LegacyKFlag<int> PC_READY_FOR_ANT_COLONY_CHALLENGE = new(469, "PC_READY_FOR_ANT_COLONY_CHALLENGE");

        public static LegacyKFlag<int> PHYLLA_SAVED = new(470, "PHYLLA_SAVED");

        public static LegacyKFlag<int> MET_ANT_ARENA = new(471, "MET_ANT_ARENA");

        public static LegacyKFlag<int> ANT_ARENA_WINS = new(472, "ANT_ARENA_WINS");

        public static LegacyKFlag<int> ANT_ARENA_LOSSES = new(473, "ANT_ARENA_LOSSES");

        public static LegacyKFlag<int> ANTS_PC_BEAT_GNOLL = new(474, "ANTS_PC_BEAT_GNOLL");

        public static LegacyKFlag<int> ANTS_PC_LOST_TO_GNOLL = new(475, "ANTS_PC_LOST_TO_GNOLL");

        public static LegacyKFlag<int> MET_ANT_ARENA_GNOLL = new(476, "MET_ANT_ARENA_GNOLL");

        public static LegacyKFlag<int> FOX_BAD_END_WARNING = new(477, "FOX_BAD_END_WARNING");

        public static LegacyKFlag<int> HEL_AFFECTION_FOLLOWER = new(478, "HEL_AFFECTION_FOLLOWER");

        public static LegacyKFlag<int> HEL_FOLLOWER_LEVEL = new(479, "HEL_FOLLOWER_LEVEL");

        public static LegacyKFlag<int> TOOK_GOO_ARMOR = new(480, "TOOK_GOO_ARMOR");

        public static LegacyKFlag<int> LOST_GOO_ARMOR_FIGHT = new(481, "LOST_GOO_ARMOR_FIGHT");

        public static LegacyKFlag<int> WON_GOO_ARMOR_FIGHT = new(482, "WON_GOO_ARMOR_FIGHT");

        public static LegacyKFlag<int> HEL_REDUCED_ENCOUNTER_RATE = new(483, "HEL_REDUCED_ENCOUNTER_RATE");

        public static LegacyKFlag<int> MET_VALERIA = new(484, "MET_VALERIA");

        public static LegacyKFlag<int> HEL_HARPIES_DEFEATED = new(485, "HEL_HARPIES_DEFEATED");

        public static LegacyKFlag<int> HEL_DUNGEON_MEAD_LOOTED = new(486, "HEL_DUNGEON_MEAD_LOOTED");

        public static LegacyKFlag<int> HEL_BRIGID_DEFEATED = new(487, "HEL_BRIGID_DEFEATED");

        public static LegacyKFlag<int> HEL_PC_TALKED_WITH_HAKON = new(488, "HEL_PC_TALKED_WITH_HAKON");

        public static LegacyKFlag<int> HEL_DUNGEON_TAKEN_WHIP = new(489, "HEL_DUNGEON_TAKEN_WHIP");

        public static LegacyKFlag<int> HEL_DUNGEON_TAKEN_STRAPS = new(490, "HEL_DUNGEON_TAKEN_STRAPS");

        public static LegacyKFlag<int> HEL_DUNGEON_TAKEN_DAGGER = new(491, "HEL_DUNGEON_TAKEN_DAGGER");

        public static LegacyKFlag<int> HEL_PHOENIXES_DEFEATED = new(492, "HEL_PHOENIXES_DEFEATED");

        public static LegacyKFlag<int> HEL_HARPY_QUEEN_DEFEATED = new(493, "HEL_HARPY_QUEEN_DEFEATED");

        public static LegacyKFlag<int> HARPY_QUEEN_EXECUTED = new(494, "HARPY_QUEEN_EXECUTED"); // HARPY_QUEEN_EXECUTED" Description="-1 = RELEASED, 1 = KILLED

        public static LegacyKFlag<int> HEL_KNOWS_ABOUT_HAKON = new(495, "HEL_KNOWS_ABOUT_HAKON");

        public static LegacyKFlag<int> FOUGHT_WITH_HEL_IN_DUNGEON = new(496, "FOUGHT_WITH_HEL_IN_DUNGEON");

        public static LegacyKFlag<int> TOOK_QUEEN_STAFF = new(497, "TOOK_QUEEN_STAFF");

        public static LegacyKFlag<int> VALARIA_AT_CAMP = new(498, "VALARIA_AT_CAMP");

        public static LegacyKFlag<int> VELARIA_FUTA = new(499, "VELARIA_FUTA");

        public static LegacyKFlag<int> DECLINED_TO_VISIT_REBECCS_VILLAGE = new(500, "DECLINED_TO_VISIT_REBECCS_VILLAGE");

        public static LegacyKFlag<int> TIMES_IN_DEMON_PIT = new(501, "TIMES_IN_DEMON_PIT");

        public static LegacyKFlag<int> TIMES_REFUSED_REBECCS_OFFER = new(502, "TIMES_REFUSED_REBECCS_OFFER");

        public static LegacyKFlag<int> OWCAS_ATTITUDE = new(503, "OWCAS_ATTITUDE");

        public static LegacyKFlag<int> VAPULA_SUBMISSIVENESS = new(504, "VAPULA_SUBMISSIVENESS");

        public static LegacyKFlag<int> DAYS_SINCE_LAST_DEMON_DEALINGS = new(505, "DAYS_SINCE_LAST_DEMON_DEALINGS");

        public static LegacyKFlag<int> OWCA_UNLOCKED = new(506, "OWCA_UNLOCKED");

        public static LegacyKFlag<int> REBECCS_LAST_PLEA = new(507, "REBECCS_LAST_PLEA");

        public static LegacyKFlag<int> OWCA_ANGER_DISABLED = new(508, "OWCA_ANGER_DISABLED");

        public static LegacyKFlag<int> SLIMEGINAED = new(509, "SLIMEGINAED");

        public static LegacyKFlag<int> GHOST_GIRL_SLIME_X_SHOULDRA_COUNTER = new(510, "GHOST_GIRL_SLIME_X_SHOULDRA_COUNTER");

        public static LegacyKFlag<int> GENDERLESS_MASTURBATION_WITH_GHOST_COUNT = new(511, "GENDERLESS_MASTURBATION_WITH_GHOST_COUNT");

        public static LegacyKFlag<int> SHOULDRA_TONGUE_LICKS_TIMES = new(512, "SHOULDRA_TONGUE_LICKS_TIMES");

        public static LegacyKFlag<float> SHOULDRA_FOLLOWER_STATE = new(513, "SHOULDRA_FOLLOWER_STATE");

        public static LegacyKFlag<int> SHOULDRA_PLOT_LEVEL = new(514, "SHOULDRA_PLOT_LEVEL");

        public static LegacyKFlag<int> SHOULDRA_TALK_NEEDED = new(515, "SHOULDRA_TALK_NEEDED");

        public static LegacyKFlag<int> SHOULDRA_PLOT_COUNTDOWN = new(516, "SHOULDRA_PLOT_COUNTDOWN");

        public static LegacyKFlag<int> SHOULDRA_SLEEP_TIMER = new(517, "SHOULDRA_SLEEP_TIMER");

        public static LegacyKFlag<int> SHOULDRA_MAGIC_COOLDOWN = new(518, "SHOULDRA_MAGIC_COOLDOWN");

        public static LegacyKFlag<int> SHOULDRA_TIMES_NIGHT_RAPED_PC = new(519, "SHOULDRA_TIMES_NIGHT_RAPED_PC");

        public static LegacyKFlag<float> SHOULDRA_EXGARTUDRAMA = new(520, "SHOULDRA_EXGARTUDRAMA");

        public static LegacyKFlag<int> SHOULDRA_BAKERY_TIMES = new(521, "SHOULDRA_BAKERY_TIMES");

        public static LegacyKFlag<int> FERA_RELEASED = new(522, "FERA_RELEASED");

        public static LegacyKFlag<int> EMBER_AFFECTION = new(523, "EMBER_AFFECTION"); // : Pretty obvious

        public static LegacyKFlag<int> EMBER_HATCHED = new(524, "EMBER_HATCHED"); // : is ember hatched? 1 = true

        public static LegacyKFlag<int> EMBER_GENDER = new(525, "EMBER_GENDER"); //  1 for male, 2 for female, 3 for herm. This also controls the egg's shell color.

        public static LegacyKFlag<int> EMBER_TYPE = new(526, "EMBER_TYPE"); // numerical value: Ember is supposed to have many forms, this will control which one is born. (This is important for when Ember has hybrid forms., "EMBER_TYPE")

        public static LegacyKFlag<int> EMBER_COR = new(527, "EMBER_COR"); // dragon-girl Ember uses this. (Default starting value = new Flag<int>(50), "EMBER_COR")

        public static LegacyKFlag<int> EMBER_HAIR = new(528, "EMBER_HAIR"); // 0 for no hair, 1 for hair, 2 for mane.

        public static LegacyKFlag<int> EMBER_MILK = new(529, "EMBER_MILK"); // 0 for no lactation, 1 for lactating.

        public static LegacyKFlag<int> EMBER_OVIPOSITION = new(530, "EMBER_OVIPOSITION"); // 0 for no egg laying, 1 for egg laying.

        public static LegacyKFlag<int> EMBER_ROUNDFACE = new(531, "EMBER_ROUNDFACE"); // 0 for anthro Ember, 1 for dragon-girl Ember. (You might want to control this with the Type flag since only default Embers use this variable., "EMBER_ROUNDFACE")

        public static LegacyKFlag<int> EMBER_EGG_FLUID_COUNT = new(532, "EMBER_EGG_FLUID_COUNT"); // This controls when it's time to hatch. Every item use and every time you use the egg as a masturbation aid, this will be incremented. Threshold for birthing is 5, but the birthing process can only be triggered when using as a masturbatory aid. This is done to allow players the chance to modify Ember before actually hatching.

        public static LegacyKFlag<int> EMBER_STAT = new(533, "EMBER_STAT"); // All Embers have a hidden stat, Corrupt has Ego, Pure has Confidence, Tainted has Affection, and hybrids vary. There is a need to track this, but only 1 special stat for every Ember.

        public static LegacyKFlag<int> EMBER_intERNAL_DICK = new(534, "EMBER_intERNAL_DICK"); // Dragon-girl Ember can have either an internal sheath to keep " + emberMF("his","her", "EMBER_intERNAL_DICK") + " dick in or have it be more human-like. 0 = internal, 1 = external.

        public static LegacyKFlag<int> TIMES_EQUIPPED_EMBER_SHIELD = new(535, "TIMES_EQUIPPED_EMBER_SHIELD");

        public static LegacyKFlag<int> TOOK_EMBER_EGG = new(536, "TOOK_EMBER_EGG"); // PC Take ember's egg home?

        public static LegacyKFlag<int> EGG_BROKEN = new(537, "EGG_BROKEN"); // PC Smash!? ember's egg?

        public static LegacyKFlag<int> TIMES_FOUND_EMBERS_EGG = new(538, "TIMES_FOUND_EMBERS_EGG"); // Times stumbled into ze egg.

        public static LegacyKFlag<int> EMBER_JACKED_ON = new(539, "EMBER_JACKED_ON"); // Has the PC masturbated on the egg yet?  Needed to hatcH!

        public static LegacyKFlag<int> EMBER_OVI_BITCHED_YET = new(540, "EMBER_OVI_BITCHED_YET"); // Used to trigger emberBitchesAboutPCBeingFullOfEggs(, "EMBER_OVI_BITCHED_YET")

        public static LegacyKFlag<int> EMBER_LUST_BITCHING_COUNTER = new(541, "EMBER_LUST_BITCHING_COUNTER");

        public static LegacyKFlag<int> EMBER_CURRENTLY_FREAKING_ABOUT_MINOCUM = new(542, "EMBER_CURRENTLY_FREAKING_ABOUT_MINOCUM"); //  Used to trigger minotaurJizzFreakout(, "EMBER_CURRENTLY_FREAKING_ABOUT_MINOCUM")

        public static LegacyKFlag<int> DRANK_EMBER_BLOOD_TODAY = new(543, "DRANK_EMBER_BLOOD_TODAY"); // Cooldown for ember TFs

        public static LegacyKFlag<int> EMBER_PUSSY_FUCK_COUNT = new(544, "EMBER_PUSSY_FUCK_COUNT");

        public static LegacyKFlag<int> TIMES_BUTTFUCKED_EMBER = new(545, "TIMES_BUTTFUCKED_EMBER");

        public static LegacyKFlag<int> VAPULA_HAREM_FUCK = new(546, "VAPULA_HAREM_FUCK");

        public static LegacyKFlag<int> VAPULA_THREESOMES = new(547, "VAPULA_THREESOMES");

        public static LegacyKFlag<int> OWCA_SACRIFICE_DISABLED = new(548, "OWCA_SACRIFICE_DISABLED");

        public static LegacyKFlag<float> VAPULA_FOLLOWER = new(549, "VAPULA_FOLLOWER");

        public static LegacyKFlag<int> JOJO_MOVE_IN_DISABLED = new(550, "JOJO_MOVE_IN_DISABLED");

        public static LegacyKFlag<int> VAPULA_DAYS_SINCE_FED = new(551, "VAPULA_DAYS_SINCE_FED");

        public static LegacyKFlag<int> VAPULA_TEASE_COUNT = new(552, "VAPULA_TEASE_COUNT");

        public static LegacyKFlag<int> EMBER_INCUBATION = new(553, "EMBER_INCUBATION");

        public static LegacyKFlag<int> EMBER_CHILDREN_MALES = new(554, "EMBER_CHILDREN_MALES");

        public static LegacyKFlag<int> EMBER_CHILDREN_FEMALES = new(555, "EMBER_CHILDREN_FEMALES");

        public static LegacyKFlag<int> EMBER_CHILDREN_HERMS = new(556, "EMBER_CHILDREN_HERMS");

        public static LegacyKFlag<int> EMBER_EGGS = new(557, "EMBER_EGGS");

        public static LegacyKFlag<int> EMBER_BITCHES_ABOUT_PREGNANT_PC = new(558, "EMBER_BITCHES_ABOUT_PREGNANT_PC");

        public static LegacyKFlag<int> EMBER_TALKS_TO_PC_ABOUT_PC_MOTHERING_DRAGONS = new(559, "EMBER_TALKS_TO_PC_ABOUT_PC_MOTHERING_DRAGONS");

        public static LegacyKFlag<int> EMBER_PREGNANT_TALK = new(560, "EMBER_PREGNANT_TALK");

        public static LegacyKFlag<int> TIMES_MET_CHAMELEON = new(561, "TIMES_MET_CHAMELEON");

        public static LegacyKFlag<int> TIMES_IN_BENOITS = new(562, "TIMES_IN_BENOITS");

        public static LegacyKFlag<int> BENOIT_AFFECTION = new(563, "BENOIT_AFFECTION");

        public static LegacyKFlag<int> BENOIT_EXPLAINED_SHOP = new(564, "BENOIT_EXPLAINED_SHOP");

        public static LegacyKFlag<int> BENOIT_SUGGEST_UNLOCKED = new(565, "BENOIT_SUGGEST_UNLOCKED");

        public static LegacyKFlag<int> TURKEY_FUCK_YEAR_DONE = new(566, "TURKEY_FUCK_YEAR_DONE");

        public static LegacyKFlag<string> BENOIT_1 = new(567, "BENOIT_1"); // Benoit's inventory items

        public static LegacyKFlag<string> BENOIT_2 = new(568, "BENOIT_2"); // Benoit's inventory items

        public static LegacyKFlag<string> BENOIT_3 = new(569, "BENOIT_3"); // Benoit's inventory items

        public static LegacyKFlag<int> BENOIT_TALKED_TODAY = new(570, "BENOIT_TALKED_TODAY");

        public static LegacyKFlag<int> BENOIT_TALKED_TO_PROPERLY = new(571, "BENOIT_TALKED_TO_PROPERLY");

        public static LegacyKFlag<int> BENOIT_EGGS = new(572, "BENOIT_EGGS");

        public static LegacyKFlag<int> BENOIT_TIMES_SEXED_FEMPCS = new(573, "BENOIT_TIMES_SEXED_FEMPCS");

        public static LegacyKFlag<int> BENOIT_BIRTH_DELAY = new(574, "BENOIT_BIRTH_DELAY");

        public static LegacyKFlag<int> BENOIT_WOMB_TALK_UNLOCKED = new(575, "BENOIT_WOMB_TALK_UNLOCKED");

        public static LegacyKFlag<int> BENOIT_POST_FIRSTFUCK_TALK = new(576, "BENOIT_POST_FIRSTFUCK_TALK");

        public static LegacyKFlag<float> BENOIT_TESTED_BASILISK_WOMB = new(577, "BENOIT_TESTED_BASILISK_WOMB");

        public static LegacyKFlag<int> TIMES_ENCOUNTERED_SAND_TRAPS = new(578, "TIMES_ENCOUNTERED_SAND_TRAPS");

        public static LegacyKFlag<int> SANDTRAP_NAGA_3SOME = new(579, "SANDTRAP_NAGA_3SOME");

        public static LegacyKFlag<int> TAMANI_TIME_OUT = new(580, "TAMANI_TIME_OUT");

        public static LegacyKFlag<int> TIMES_OVIPOSITED_TAMANI = new(581, "TIMES_OVIPOSITED_TAMANI");

        public static LegacyKFlag<int> URTA_EGGS = new(582, "URTA_EGGS");

        public static LegacyKFlag<int> URTA_FERTILE_EGGS = new(583, "URTA_FERTILE_EGGS");

        public static LegacyKFlag<int> URTA_EGG_FORCE_EVENT = new(584, "URTA_EGG_FORCE_EVENT");

        public static LegacyKFlag<int> URTA_TIMES_EGG_PREGGED = new(585, "URTA_TIMES_EGG_PREGGED");

        public static LegacyKFlag<int> URTA_PREGNANCY_TYPE = new(586, "URTA_PREGNANCY_TYPE"); //Previously URTA_EGG_INCUBATION

        public static LegacyKFlag<int> URTA_FLATBELLY_NOTICE = new(587, "URTA_FLATBELLY_NOTICE");

        public static LegacyKFlag<int> EGG_WITCH_COUNTER = new(588, "EGG_WITCH_COUNTER");

        public static LegacyKFlag<int> EGG_WITCH_TYPE = new(589, "EGG_WITCH_TYPE");

        public static LegacyKFlag<int> TIMES_EGGED_JOJO = new(590, "TIMES_EGGED_JOJO");

        public static LegacyKFlag<int> JOJO_EGGCUBATE_COUNT = new(591, "JOJO_EGGCUBATE_COUNT");

        public static LegacyKFlag<int> DICK_EGG_INCUBATION = new(592, "DICK_EGG_INCUBATION");

        public static LegacyKFlag<int> TIMES_EGGED_IN_COCK = new(593, "TIMES_EGGED_IN_COCK");

        public static LegacyKFlag<int> TIMES_FUCKED_FLOWER = new(594, "TIMES_FUCKED_FLOWER");

        public static LegacyKFlag<int> TIMES_RIDDEN_FLOWER = new(595, "TIMES_RIDDEN_FLOWER");

        public static LegacyKFlag<int> FUCK_FLOWER_LEVEL = new(596, "FUCK_FLOWER_LEVEL");

        public static LegacyKFlag<int> FUCK_FLOWER_GROWTH_COUNTER = new(597, "FUCK_FLOWER_GROWTH_COUNTER");

        public static LegacyKFlag<int> FUCK_FLOWER_KILLED = new(598, "FUCK_FLOWER_KILLED");

        public static LegacyKFlag<int> AMILY_TREE_FLIPOUT = new(599, "AMILY_TREE_FLIPOUT");

        public static LegacyKFlag<int> HOLLI_FRUIT = new(600, "HOLLI_FRUIT");

        public static LegacyKFlag<int> HOLLI_FRUIT_EXPLAINED = new(601, "HOLLI_FRUIT_EXPLAINED");

        public static LegacyKFlag<int> HOLLI_DEFENSE_ON = new(602, "HOLLI_DEFENSE_ON");

        public static LegacyKFlag<int> SATYR_KIDS = new(603, "SATYR_KIDS");

        public static LegacyKFlag<int> RUBI_AFFECTION = new(604, "RUBI_AFFECTION");

        public static LegacyKFlag<int> RUBI_DISABLED = new(605, "RUBI_DISABLED");

        public static LegacyKFlag<int> RUBI_ADMITTED_GENDER = new(606, "RUBI_ADMITTED_GENDER");

        public static LegacyKFlag<int> RUBI_intRODUCED = new(607, "RUBI_intRODUCED");

        public static LegacyKFlag<int> RUBI_BLU_BALLS = new(608, "RUBI_BLU_BALLS");

        public static LegacyKFlag<int> RUBI_ORGASM_DENIAL = new(609, "RUBI_ORGASM_DENIAL");

        public static LegacyKFlag<int> RUBI_BIMBO = new(610, "RUBI_BIMBO");

        public static LegacyKFlag<int> RUBI_INCUBUS_PROGRESS = new(611, "RUBI_INCUBUS_PROGRESS");

        public static LegacyKFlag<int> RUBI_SUITCLOTHES = new(612, "RUBI_SUITCLOTHES");

        public static LegacyKFlag<int> RUBI_FETISH_CLOTHES = new(613, "RUBI_FETISH_CLOTHES");

        public static LegacyKFlag<int> RUBI_GREEN_ADVENTURER = new(614, "RUBI_GREEN_ADVENTURER");

        public static LegacyKFlag<int> RUBI_TUBE_TOP = new(615, "RUBI_TUBE_TOP");

        public static LegacyKFlag<int> RUBI_BODYSUIT = new(616, "RUBI_BODYSUIT");

        public static LegacyKFlag<int> RUBI_LONGDRESS = new(617, "RUBI_LONGDRESS");

        public static LegacyKFlag<int> RUBI_TIGHT_PANTS = new(618, "RUBI_TIGHT_PANTS");

        public static LegacyKFlag<int> RUBI_NURSE_CLOTHES = new(619, "RUBI_NURSE_CLOTHES");

        public static LegacyKFlag<int> RUBI_SWIMWEAR = new(620, "RUBI_SWIMWEAR");

        public static LegacyKFlag<int> RUBI_PROSTITUTION = new(621, "RUBI_PROSTITUTION"); // -1 don't see others, -2 = broke up, 1 = NTR okay, 2 = prostitute

        public static LegacyKFlag<int> RUBIS_HOUSE_FIRST_TIME = new(622, "RUBIS_HOUSE_FIRST_TIME");

        public static LegacyKFlag<int> RUBI_GOT_BIMBO_SKIRT = new(623, "RUBI_GOT_BIMBO_SKIRT");

        public static LegacyKFlag<int> RUBI_PROFIT = new(624, "RUBI_PROFIT");

        public static LegacyKFlag<int> RUBI_BIMBO_MINIDRESS = new(625, "RUBI_BIMBO_MINIDRESS");

        public static LegacyKFlag<int> MANSION_VISITED = new(626, "MANSION_VISITED");

        public static LegacyKFlag<int> REDHEAD_IS_FUTA = new(627, "REDHEAD_IS_FUTA");

        public static LegacyKFlag<int> MET_KITSUNES = new(628, "MET_KITSUNES");

        public static LegacyKFlag<int> AMILY_OVIPOSITED_COUNT = new(629, "AMILY_OVIPOSITED_COUNT");

        public static LegacyKFlag<int> AMILY_OVIPOSITED_COUNTDOWN = new(630, "AMILY_OVIPOSITED_COUNTDOWN");

        public static LegacyKFlag<int> AMILY_OVIPOSITION_UNLOCKED = new(631, "AMILY_OVIPOSITION_UNLOCKED");

        public static LegacyKFlag<int> BENOIT_GENERIC_EGGS = new(632, "BENOIT_GENERIC_EGGS");

        public static LegacyKFlag<int> URTA_SCYLLA_BIG_DICK_TIMES_DONE = new(633, "URTA_SCYLLA_BIG_DICK_TIMES_DONE");

        public static LegacyKFlag<int> VAPULA_EARNED_A_SPANK = new(634, "VAPULA_EARNED_A_SPANK");

        public static LegacyKFlag<int> AMILY_TIMES_SWIMFUCKED = new(635, "AMILY_TIMES_SWIMFUCKED");

        public static LegacyKFlag<int> AMILY_OWNS_BIKINI = new(636, "AMILY_OWNS_BIKINI");

        public static LegacyKFlag<int> CANDY_CANE_YEAR_MET = new(637, "CANDY_CANE_YEAR_MET");

        public static LegacyKFlag<int> GATS_ANGEL_GOOD_ENDED = new(638, "GATS_ANGEL_GOOD_ENDED");

        public static LegacyKFlag<int> GATS_ANGEL_DISABLED = new(639, "GATS_ANGEL_DISABLED");

        public static LegacyKFlag<int> GATS_ANGEL_QUEST_BEGAN = new(640, "GATS_ANGEL_QUEST_BEGAN");

        public static LegacyKFlag<int> GATS_ANGEL_TIME_TO_FIND_KEY = new(641, "GATS_ANGEL_TIME_TO_FIND_KEY");

        public static LegacyKFlag<int> XMAS_CHICKEN_YEAR = new(642, "XMAS_CHICKEN_YEAR");

        public static LegacyKFlag<int> KAMI_ENCOUNTER = new(643, "KAMI_ENCOUNTER");

        public static LegacyKFlag<int> POLAR_PETE_YEAR_MET = new(644, "POLAR_PETE_YEAR_MET");

        public static LegacyKFlag<int> JACK_FROST_YEAR = new(645, "JACK_FROST_YEAR");

        public static LegacyKFlag<int> JACK_FROST_PROGRESS = new(646, "JACK_FROST_PROGRESS");

        public static LegacyKFlag<int> NIEVE_STAGE = new(647, "NIEVE_STAGE"); //No longer used

        public static LegacyKFlag<int> NIEVE_GENDER = new(648, "NIEVE_GENDER"); //No longer used

        public static LegacyKFlag<string> NIEVE_MOUTH = new(649, "NIEVE_MOUTH"); //No longer used

        public static LegacyKFlag<int> TIMES_LOST_HECKEL_DOM_CHALLENGE = new(650, "TIMES_LOST_HECKEL_DOM_CHALLENGE");

        public static LegacyKFlag<int> TIMES_DOMMED_HECKEL = new(651, "TIMES_DOMMED_HECKEL");

        public static LegacyKFlag<int> TIMES_MET_CHICKEN_HARPY = new(652, "TIMES_MET_CHICKEN_HARPY");

        public static LegacyKFlag<int> EGGS_BOUGHT = new(653, "EGGS_BOUGHT");

        public static LegacyKFlag<int> GOO_TFED_MEAN = new(654, "GOO_TFED_MEAN");

        public static LegacyKFlag<int> GOO_TFED_NICE = new(655, "GOO_TFED_NICE");

        public static LegacyKFlag<string> GOO_NAME = new(656, "GOO_NAME");

        public static LegacyKFlag<int> GOO_SLAVE_RECRUITED = new(657, "GOO_SLAVE_RECRUITED");

        public static LegacyKFlag<string> GOO_EYES = new(658, "GOO_EYES");

        public static LegacyKFlag<int> GOO_TOSSED_AFTER_NAMING = new(659, "GOO_TOSSED_AFTER_NAMING");

        public static LegacyKFlag<int> TIMES_FUCKED_NORMAL_GOOS = new(660, "TIMES_FUCKED_NORMAL_GOOS");

        public static LegacyKFlag<int> PC_KNOWS_ABOUT_BLACK_EGGS = new(661, "PC_KNOWS_ABOUT_BLACK_EGGS");

        public static LegacyKFlag<float> GOO_HAPPINESS = new(662, "GOO_HAPPINESS");

        public static LegacyKFlag<float> GOO_OBEDIENCE = new(663, "GOO_OBEDIENCE");

        public static LegacyKFlag<float> GOO_FLUID_AMOUNT = new(664, "GOO_FLUID_AMOUNT");

        public static LegacyKFlag<int> GOO_PREFERRED_TIT_SIZE = new(665, "GOO_PREFERRED_TIT_SIZE");

        public static LegacyKFlag<int> GOO_NIPPLE_TYPE = new(666, "GOO_NIPPLE_TYPE");

        public static LegacyKFlag<float> GOO_DICK_LENGTH = new(667, "GOO_DICK_LENGTH");

        public static LegacyKFlag<int> GOO_DICK_TYPE = new(668, "GOO_DICK_TYPE");

        public static LegacyKFlag<int> TIMES_THOUGHT_ABOUT_GOO_RECRUITMENT = new(669, "TIMES_THOUGHT_ABOUT_GOO_RECRUITMENT");

        public static LegacyKFlag<int> GOO_INDIRECT_FED = new(670, "GOO_INDIRECT_FED");

        public static LegacyKFlag<int> TIMES_FED_LATEXY_MINO_CUM = new(671, "TIMES_FED_LATEXY_MINO_CUM");

        public static LegacyKFlag<int> LATEX_GOO_TIMES_FEMDOMMED_BY_PC = new(672, "LATEX_GOO_TIMES_FEMDOMMED_BY_PC");

        public static LegacyKFlag<int> COTTON_PREGNANCY_INCUBATION = new(673, "COTTON_PREGNANCY_INCUBATION");

        public static LegacyKFlag<int> COTTON_PREGNANCY_TYPE = new(674, "COTTON_PREGNANCY_TYPE");

        public static LegacyKFlag<int> COTTON_KID_COUNT = new(675, "COTTON_KID_COUNT");

        public static LegacyKFlag<int> COTTON_OLDEST_KID_AGE = new(676, "COTTON_OLDEST_KID_AGE");

        public static LegacyKFlag<int> PC_IS_A_DEADBEAT_COTTON_DAD = new(677, "PC_IS_A_DEADBEAT_COTTON_DAD");

        public static LegacyKFlag<int> PC_IS_A_GOOD_COTTON_DAD = new(678, "PC_IS_A_GOOD_COTTON_DAD");

        public static LegacyKFlag<int> COTTON_HERBS_OFF = new(679, "COTTON_HERBS_OFF");

        public static LegacyKFlag<int> COTTON_CONTRACEPTION_TALK = new(680, "COTTON_CONTRACEPTION_TALK");

        public static LegacyKFlag<int> COTTON_KNOCKED_UP_PC_AND_TALK_HAPPENED = new(681, "COTTON_KNOCKED_UP_PC_AND_TALK_HAPPENED");

        public static LegacyKFlag<int> HOLLI_SUBMISSIVE = new(682, "HOLLI_SUBMISSIVE");

        public static LegacyKFlag<int> TIMES_ORPHANAGED_WITH_RAPHAEL = new(683, "TIMES_ORPHANAGED_WITH_RAPHAEL");

        public static LegacyKFlag<int> LOPPE_FURRY = new(684, "LOPPE_FURRY");

        public static LegacyKFlag<int> LOPPE_FERTILE = new(685, "LOPPE_FERTILE");

        public static LegacyKFlag<int> LOPPE_KIDS = new(686, "LOPPE_KIDS");

        public static LegacyKFlag<int> LOPPE_TRAINING = new(687, "LOPPE_TRAINING");

        public static LegacyKFlag<int> LOPPE_KIDS_LIMIT = new(688, "LOPPE_KIDS_LIMIT");

        public static LegacyKFlag<int> LOPPE_URTA_CHATS = new(689, "LOPPE_URTA_CHATS");

        public static LegacyKFlag<float> LOPPE_PC_MET_UMA = new(690, "LOPPE_PC_MET_UMA");

        public static LegacyKFlag<int> LOPPE_TIMES_SEXED = new(691, "LOPPE_TIMES_SEXED");

        public static LegacyKFlag<int> LOPPE_DENIAL_COUNTER = new(692, "LOPPE_DENIAL_COUNTER");

        public static LegacyKFlag<int> LOPPE_DISABLED = new(693, "LOPPE_DISABLED");

        public static LegacyKFlag<int> TIMES_ASKED_LOPPE_ABOUT_LOPPE = new(694, "TIMES_ASKED_LOPPE_ABOUT_LOPPE");

        public static LegacyKFlag<int> LOPPE_MET = new(695, "LOPPE_MET");

        public static LegacyKFlag<float> HELIA_FOLLOWER_DISABLED = new(696, "HELIA_FOLLOWER_DISABLED");

        public static LegacyKFlag<int> HEL_intROS_LEVEL = new(697, "HEL_intROS_LEVEL");

        public static LegacyKFlag<int> MINO_SONS_HAVE_SOPHIE = new(698, "MINO_SONS_HAVE_SOPHIE");

        public static LegacyKFlag<int> KEEP_HELIA_AND_SOPHIE = new(699, "KEEP_HELIA_AND_SOPHIE");

        public static LegacyKFlag<int> FOLLOWER_HEL_TALKS = new(700, "FOLLOWER_HEL_TALKS"); //  FOLLOWER_HEL_TALKS

        public static LegacyKFlag<string> SLEEP_WITH = new(701, "SLEEP_WITH"); //  related to who you sleep with in camp

        public static LegacyKFlag<int> COTTON_OLDEST_KID_GENDER = new(702, "COTTON_OLDEST_KID_GENDER");

        public static LegacyKFlag<int> HEL_CAN_SWIM = new(703, "HEL_CAN_SWIM");

        public static LegacyKFlag<int> HEL_GUARDING = new(704, "HEL_GUARDING");

        public static LegacyKFlag<int> PC_PROMISED_HEL_MONOGAMY_FUCKS = new(705, "PC_PROMISED_HEL_MONOGAMY_FUCKS");

        public static LegacyKFlag<int> HEL_RAPED_TODAY = new(706, "HEL_RAPED_TODAY");

        public static LegacyKFlag<int> FOUND_ISABELLA_AT_FARM_TODAY = new(707, "FOUND_ISABELLA_AT_FARM_TODAY");

        public static LegacyKFlag<int> KITSUNE_SHRINE_VISIT = new(708, "KITSUNE_SHRINE_VISIT");

        public static LegacyKFlag<int> TOOK_KITSUNE_STATUE = new(709, "TOOK_KITSUNE_STATUE");

        public static LegacyKFlag<int> URTA_MET_HEL = new(710, "URTA_MET_HEL");

        public static LegacyKFlag<float> URTA_QUEST_STATUS = new(711, "URTA_QUEST_STATUS");

        public static LegacyKFlag<int> URTA_FUCKED_HEL = new(712, "URTA_FUCKED_HEL");

        public static LegacyKFlag<int> SUCCUBI_MILKED_GNOLL_PRINCESS = new(713, "SUCCUBI_MILKED_GNOLL_PRINCESS");

        public static LegacyKFlag<int> URTA_TAKEN_BY_GNOLLS = new(714, "URTA_TAKEN_BY_GNOLLS");

        public static LegacyKFlag<int> URTA_BECOMES_SNAKE_SLAVE = new(715, "URTA_BECOMES_SNAKE_SLAVE");

        public static LegacyKFlag<int> URTA_GOBLIN_SLAVE = new(716, "URTA_GOBLIN_SLAVE");

        public static LegacyKFlag<int> URTA_TENTACLE_RAPED = new(717, "URTA_TENTACLE_RAPED");

        public static LegacyKFlag<int> URTA_TENTACLE_GAPED = new(718, "URTA_TENTACLE_GAPED");

        public static LegacyKFlag<int> URTA_IS_VULQUINE_MENACE = new(719, "URTA_IS_VULQUINE_MENACE");

        public static LegacyKFlag<int> URTA_RAPED_MINO_LORD = new(720, "URTA_RAPED_MINO_LORD");

        public static LegacyKFlag<int> URTA_MINO_AND_SUCCUBI_SLAVE = new(721, "URTA_MINO_AND_SUCCUBI_SLAVE");

        public static LegacyKFlag<int> EDRYN_PREGNANCY_TYPE = new(722, "EDRYN_PREGNANCY_TYPE"); //In previous versions was EDRYN_BIRF_COUNTDOWN - Corrected in Saves.unFuckSave(, "EDRYN_PREGNANCY_TYPE")

        public static LegacyKFlag<int> URTA_FERTILE = new(723, "URTA_FERTILE");

        public static LegacyKFlag<int> URTA_PREG_EVERYBODY = new(724, "URTA_PREG_EVERYBODY");

        public static LegacyKFlag<int> KELT_BREAK_LEVEL = new(725, "KELT_BREAK_LEVEL");

        public static LegacyKFlag<int> KELLY_CUNT_TYPE = new(726, "KELLY_CUNT_TYPE");

        public static LegacyKFlag<int> NEVER_RESIST_KELT = new(727, "NEVER_RESIST_KELT");

        public static LegacyKFlag<int> WHITNEY_FLIPPED_OUT_OVER_KELLY = new(728, "WHITNEY_FLIPPED_OUT_OVER_KELLY");

        public static LegacyKFlag<int> TIMES_PUNISHED_KELLY = new(729, "TIMES_PUNISHED_KELLY");

        public static LegacyKFlag<int> TIMES_RIM_JOBBED_BY_KELLY = new(730, "TIMES_RIM_JOBBED_BY_KELLY");

        public static LegacyKFlag<int> TIMES_RIDDEN_KELLY_FOR_PUNISHMENT = new(731, "TIMES_RIDDEN_KELLY_FOR_PUNISHMENT");

        public static LegacyKFlag<int> KELLY_BONUS_TIT_ROWS = new(732, "KELLY_BONUS_TIT_ROWS");

        public static LegacyKFlag<int> KELLY_LACTATING = new(733, "KELLY_LACTATING");

        public static LegacyKFlag<int> KELLY_DISOBEYING_COUNTER = new(734, "KELLY_DISOBEYING_COUNTER");

        public static LegacyKFlag<int> KELLY_VAGINALLY_FUCKED_COUNT = new(735, "KELLY_VAGINALLY_FUCKED_COUNT");

        public static LegacyKFlag<int> VALENTINES_EVENT_YEAR = new(736, "VALENTINES_EVENT_YEAR");

        public static LegacyKFlag<int> SOPHIE_HEAT_COUNTER = new(737, "SOPHIE_HEAT_COUNTER");

        public static LegacyKFlag<int> SOPHIE_INCUBATION = new(738, "SOPHIE_INCUBATION");

        public static LegacyKFlag<int> SOPHIE_BROACHED_SLEEP_WITH = new(739, "SOPHIE_BROACHED_SLEEP_WITH");

        public static LegacyKFlag<int> SOPHIE_ADULT_KID_COUNT = new(740, "SOPHIE_ADULT_KID_COUNT");

        public static LegacyKFlag<int> SOPHIE_DAUGHTER_MATURITY_COUNTER = new(741, "SOPHIE_DAUGHTER_MATURITY_COUNTER");

        public static LegacyKFlag<int> SOPHIE_CAMP_EGG_COUNTDOWN = new(742, "SOPHIE_CAMP_EGG_COUNTDOWN");

        public static LegacyKFlag<int> SOPHIE_FAMILY_INCEST = new(743, "SOPHIE_FAMILY_INCEST");

        public static LegacyKFlag<int> RATHAZUL_DEBIMBO_OFFERED = new(744, "RATHAZUL_DEBIMBO_OFFERED");

        public static LegacyKFlag<int> SOPHIE_DEBIMBOED = new(745, "SOPHIE_DEBIMBOED");

        public static LegacyKFlag<int> SOPHIES_DAUGHTERS_DEBIMBOED = new(746, "SOPHIES_DAUGHTERS_DEBIMBOED");

        public static LegacyKFlag<int> SOPHIE_RECRUITED_PURE = new(747, "SOPHIE_RECRUITED_PURE");

        public static LegacyKFlag<int> SOPHIE_FOLLOWER_IRRITATION = new(748, "SOPHIE_FOLLOWER_IRRITATION");

        public static LegacyKFlag<int> TIMES_MORNING_SOPHIE_FEMDOMMED = new(749, "TIMES_MORNING_SOPHIE_FEMDOMMED");

        public static LegacyKFlag<int> DAUGHTER_ONE_BIMBO = new(750, "DAUGHTER_ONE_BIMBO");

        public static LegacyKFlag<int> DAUGHTER_TWO_BIMBO = new(751, "DAUGHTER_TWO_BIMBO");

        public static LegacyKFlag<int> DAUGHTER_THREE_BIMBO = new(752, "DAUGHTER_THREE_BIMBO");

        public static LegacyKFlag<int> DAUGHTER_FOUR_BIMBO = new(753, "DAUGHTER_FOUR_BIMBO");

        public static LegacyKFlag<int> NO_PURE_SOPHIE_RECRUITMENT = new(754, "NO_PURE_SOPHIE_RECRUITMENT");

        public static LegacyKFlag<int> SOPHIE_FOLLOWER_PROGRESS = new(755, "SOPHIE_FOLLOWER_PROGRESS");

        public static LegacyKFlag<int> KID_A_XP = new(756, "KID_A_XP");

        public static LegacyKFlag<int> KID_SITTER = new(757, "KID_SITTER"); // 0 = no sitter, 1 = possible, 2 = doing dat shit

        public static LegacyKFlag<int> HAD_KID_A_DREAM = new(758, "HAD_KID_A_DREAM");

        public static LegacyKFlag<int> CUSTOM_PC_ENABLED = new(759, "CUSTOM_PC_ENABLED"); //No longer in use

        public static LegacyKFlag<int> SHEILA_CORRUPTION = new(760, "SHEILA_CORRUPTION");

        public static LegacyKFlag<int> SHEILA_XP = new(761, "SHEILA_XP");

        public static LegacyKFlag<int> SHEILA_PREGNANCY_INCUBATION = new(762, "SHEILA_PREGNANCY_INCUBATION"); //Previously SHEILA_PREG

        public static LegacyKFlag<int> SHEILA_JOEYS = new(763, "SHEILA_JOEYS");

        public static LegacyKFlag<int> SHEILA_DEMON = new(764, "SHEILA_DEMON");

        public static LegacyKFlag<int> SHEILA_CITE = new(765, "SHEILA_CITE");

        public static LegacyKFlag<int> SHOULDRA_MET_VALA = new(766, "SHOULDRA_MET_VALA");

        public static LegacyKFlag<int> TIMES_VALA_CONSENSUAL_BIG = new(767, "TIMES_VALA_CONSENSUAL_BIG");

        public static LegacyKFlag<int> TIMES_VAPULA_AND_GIANT_VALA = new(768, "TIMES_VAPULA_AND_GIANT_VALA");

        public static LegacyKFlag<int> BIKINI_ARMOR_BONUS = new(769, "BIKINI_ARMOR_BONUS");

        public static LegacyKFlag<int> OWN_MAIDEN_BIKINI = new(770, "OWN_MAIDEN_BIKINI");

        public static LegacyKFlag<int> AMILY_X_IZMA_POTION_3SOME = new(771, "AMILY_X_IZMA_POTION_3SOME");

        public static LegacyKFlag<int> MET_ESSY = new(772, "MET_ESSY");

        public static LegacyKFlag<int> TURNED_DOWN_ESSY_FIRST_MEETING = new(773, "TURNED_DOWN_ESSY_FIRST_MEETING");

        public static LegacyKFlag<int> ACCEPTED_ESSY_FIRST_MEETING = new(774, "ACCEPTED_ESSY_FIRST_MEETING");

        public static LegacyKFlag<int> GIVEN_AMILY_NURSE_OUTFIT = new(775, "GIVEN_AMILY_NURSE_OUTFIT");

        public static LegacyKFlag<int> TIMES_EATEN_EDRYN_PUSSY_RUT = new(776, "TIMES_EATEN_EDRYN_PUSSY_RUT");

        public static LegacyKFlag<int> MET_OTTERGIRL = new(777, "MET_OTTERGIRL");

        public static LegacyKFlag<int> TIMES_SOLO_FED_NUN = new(778, "TIMES_SOLO_FED_NUN");

        public static LegacyKFlag<int> FED_SCYLLA_TODAY = new(779, "FED_SCYLLA_TODAY");

        public static LegacyKFlag<int> TIMES_SOPHIE_AND_IZMA_FUCKED = new(780, "TIMES_SOPHIE_AND_IZMA_FUCKED");

        public static LegacyKFlag<int> TOLD_SOPHIE_TO_IZMA = new(781, "TOLD_SOPHIE_TO_IZMA");

        public static LegacyKFlag<int> URTA_CUM_NO_CUM_DAYS = new(782, "URTA_CUM_NO_CUM_DAYS");

        public static LegacyKFlag<int> URTA_X_RAPHAEL_HAPPENED = new(783, "URTA_X_RAPHAEL_HAPPENED");

        public static LegacyKFlag<int> IZMA_X_LATEXY_DISABLED = new(784, "IZMA_X_LATEXY_DISABLED");

        public static LegacyKFlag<int> TIMES_IZMA_DOMMED_LATEXY = new(785, "TIMES_IZMA_DOMMED_LATEXY");

        public static LegacyKFlag<int> TIMES_HELIA_DOUBLE_DONGED = new(786, "TIMES_HELIA_DOUBLE_DONGED");

        public static LegacyKFlag<int> EVER_INFESTED = new(787, "EVER_INFESTED");

        public static LegacyKFlag<int> CAME_WORMS_AFTER_COMBAT = new(788, "CAME_WORMS_AFTER_COMBAT");

        public static LegacyKFlag<int> URTA_INCUBATION = new(789, "URTA_INCUBATION");

        public static LegacyKFlag<int> URTA_TIMES_BIRTHED = new(790, "URTA_TIMES_BIRTHED");

        public static LegacyKFlag<int> URTA_TIMES_PC_BIRTHED = new(791, "URTA_TIMES_PC_BIRTHED");

        public static LegacyKFlag<int> URTA_KIDS_MALES = new(792, "URTA_KIDS_MALES");

        public static LegacyKFlag<int> URTA_KIDS_FEMALES = new(793, "URTA_KIDS_FEMALES");

        public static LegacyKFlag<int> URTA_KIDS_HERMS = new(794, "URTA_KIDS_HERMS");

        public static LegacyKFlag<int> URTA_FIRSTBORN_GENDER = new(795, "URTA_FIRSTBORN_GENDER");

        public static LegacyKFlag<int> URTA_FIRSTBORN_COCKTYPE = new(796, "URTA_FIRSTBORN_COCKTYPE");

        public static LegacyKFlag<int> URTA_LATESTBORN_GENDER = new(797, "URTA_LATESTBORN_GENDER");

        public static LegacyKFlag<int> URTA_LATESTBORN_COCKTYPE = new(798, "URTA_LATESTBORN_COCKTYPE");

        public static LegacyKFlag<int> NEED_URTA_LETTER = new(799, "NEED_URTA_LETTER");

        public static LegacyKFlag<int> EVENT_PARSER_ESCAPE = new(800, "EVENT_PARSER_ESCAPE");

        public static LegacyKFlag<int> URTA_INCUBATION_CELEBRATION = new(801, "URTA_INCUBATION_CELEBRATION");

        public static LegacyKFlag<int> URTA_PREGNANT_DELIVERY_SCENE = new(802, "URTA_PREGNANT_DELIVERY_SCENE");

        public static LegacyKFlag<int> TIMES_MASSAGED_URTA_BELLY = new(803, "TIMES_MASSAGED_URTA_BELLY");

        public static LegacyKFlag<int> TIMES_URTA_BOOB_WORSHIPPED = new(804, "TIMES_URTA_BOOB_WORSHIPPED");

        public static LegacyKFlag<int> TIMES_NURSED_FROM_URTA = new(805, "TIMES_NURSED_FROM_URTA");

        public static LegacyKFlag<int> URTA_LUNCH_PLAY = new(806, "URTA_LUNCH_PLAY");

        public static LegacyKFlag<int> LIANNA_HAVESTED_MALES = new(807, "LIANNA_HAVESTED_MALES");

        public static LegacyKFlag<int> LIANNA_HAVESTED_LADIES = new(808, "LIANNA_HAVESTED_LADIES");

        public static LegacyKFlag<int> FIRST_TIME_AT_URTA_HOUSE = new(809, "FIRST_TIME_AT_URTA_HOUSE");

        public static LegacyKFlag<int> NEW_BABY_ANNOUNCED = new(810, "NEW_BABY_ANNOUNCED");

        public static LegacyKFlag<int> DISCUSSED_LUBE_SPRAY = new(811, "DISCUSSED_LUBE_SPRAY");

        public static LegacyKFlag<int> SANDTRAP_LOSS_REPEATS = new(812, "SANDTRAP_LOSS_REPEATS");

        public static LegacyKFlag<int> MINERVA_LOVE = new(813, "MINERVA_LOVE");

        public static LegacyKFlag<int> MINERVA_BACKSTORY = new(814, "MINERVA_BACKSTORY");

        public static LegacyKFlag<int> MINERVA_BACKSTORY_LEARNED = new(815, "MINERVA_BACKSTORY_LEARNED");

        public static LegacyKFlag<int> TIMES_MINERVA_SEXED = new(816, "TIMES_MINERVA_SEXED");

        public static LegacyKFlag<int> TIMES_BUTTFUCKED_MINERVA = new(817, "TIMES_BUTTFUCKED_MINERVA");

        public static LegacyKFlag<int> TIMES_MINERVA_COWGIRLED = new(818, "TIMES_MINERVA_COWGIRLED");

        public static LegacyKFlag<int> TIMES_MINERVA_LAPSEXED = new(819, "TIMES_MINERVA_LAPSEXED");

        public static LegacyKFlag<int> MINERVA_LEZZES_OUT = new(820, "MINERVA_LEZZES_OUT");

        public static LegacyKFlag<int> MET_MINERVA = new(821, "MET_MINERVA");

        public static LegacyKFlag<int> MET_OLIVIA = new(822, "MET_OLIVIA");

        public static LegacyKFlag<int> LAST_EASTER_YEAR = new(823, "LAST_EASTER_YEAR");

        public static LegacyKFlag<int> TIMES_EMBER_LUSTY_FUCKED = new(824, "TIMES_EMBER_LUSTY_FUCKED");

        public static LegacyKFlag<string> GAR_NAME = new(825, "GAR_NAME");

        public static LegacyKFlag<int> GAR_CONFIDENCE = new(826, "GAR_CONFIDENCE");

        public static LegacyKFlag<int> RITUAL_intRO = new(827, "RITUAL_intRO");

        public static LegacyKFlag<int> KINKY_RITUALS_SPOKEN = new(828, "KINKY_RITUALS_SPOKEN");

        public static LegacyKFlag<int> GAR_HISTORY = new(829, "GAR_HISTORY");

        public static LegacyKFlag<int> GAR_CATHEDRAL = new(830, "GAR_CATHEDRAL");

        public static LegacyKFlag<int> GAR_NAME_TALKS = new(831, "GAR_NAME_TALKS");

        public static LegacyKFlag<int> TIMES_RUBI_MASSAGED = new(832, "TIMES_RUBI_MASSAGED");

        public static LegacyKFlag<int> SANURA_DISABLED = new(833, "SANURA_DISABLED");

        public static LegacyKFlag<int> MET_SANURA = new(834, "MET_SANURA");

        public static LegacyKFlag<int> BEATEN_SANURA_COUNT = new(835, "BEATEN_SANURA_COUNT");

        public static LegacyKFlag<int> SANDWITCH_MOB_DEFEATED = new(836, "SANDWITCH_MOB_DEFEATED");

        public static LegacyKFlag<int> SANDWITCH_THRONE_UNLOCKED = new(837, "SANDWITCH_THRONE_UNLOCKED");

        public static LegacyKFlag<int> SAND_WITCHES_FRIENDLY = new(838, "SAND_WITCHES_FRIENDLY");

        public static LegacyKFlag<int> CUM_WITCH_DEFEATED = new(839, "CUM_WITCH_DEFEATED");

        public static LegacyKFlag<int> ENTERED_SANDWITCH_DUNGEON = new(840, "ENTERED_SANDWITCH_DUNGEON");

        public static LegacyKFlag<int> PAWJOBS = new(841, "PAWJOBS");

        public static LegacyKFlag<int> RIDDLE_ONE = new(842, "RIDDLE_ONE");

        public static LegacyKFlag<int> RIDDLE_TWO = new(843, "RIDDLE_TWO");

        public static LegacyKFlag<int> RIDDLE_THREE = new(844, "RIDDLE_THREE");

        public static LegacyKFlag<int> TIMES_SUBMITTED_TO_SANURA = new(845, "TIMES_SUBMITTED_TO_SANURA");

        public static LegacyKFlag<int> TIMES_WINFUCKED_SANURA = new(846, "TIMES_WINFUCKED_SANURA");

        public static LegacyKFlag<int> SAND_MOTHER_DEFEATED = new(847, "SAND_MOTHER_DEFEATED");

        public static LegacyKFlag<int> TIMES_TENTACLED_SAND_MOTHER = new(848, "TIMES_TENTACLED_SAND_MOTHER");

        public static LegacyKFlag<int> SAND_WITCHES_COWED = new(849, "SAND_WITCHES_COWED");

        public static LegacyKFlag<int> SAND_WITCH_LOOT_TAKEN = new(850, "SAND_WITCH_LOOT_TAKEN");

        public static LegacyKFlag<int> TIMES_FRIENDLY_FUCKED_SAND_MOTHER = new(851, "TIMES_FRIENDLY_FUCKED_SAND_MOTHER");

        public static LegacyKFlag<int> MORE_CUM_WITCHES = new(852, "MORE_CUM_WITCHES");

        public static LegacyKFlag<int> CUM_WITCHES_FIGHTABLE = new(853, "CUM_WITCHES_FIGHTABLE");

        public static LegacyKFlag<int> SAND_WITCH_LEAVE_ME_ALONE = new(854, "SAND_WITCH_LEAVE_ME_ALONE");

        public static LegacyKFlag<int> BEEN_BLESSED_BY_CUM_WITCH = new(855, "BEEN_BLESSED_BY_CUM_WITCH");

        public static LegacyKFlag<int> DISCOVERED_WITCH_DUNGEON = new(856, "DISCOVERED_WITCH_DUNGEON");

        public static LegacyKFlag<int> URTA_PETPLAY_DONE = new(857, "URTA_PETPLAY_DONE");

        public static LegacyKFlag<int> SHEILA_CLOCK = new(858, "SHEILA_CLOCK");

        public static LegacyKFlag<int> SHEILA_IMPS = new(859, "SHEILA_IMPS");

        public static LegacyKFlag<int> TIMES_VISITED_MALI = new(860, "TIMES_VISITED_MALI");

        public static LegacyKFlag<int> TIMES_BEEN_TO_LIBRARY = new(861, "TIMES_BEEN_TO_LIBRARY");

        public static LegacyKFlag<int> MALI_TAKEN_BLADE = new(862, "MALI_TAKEN_BLADE");

        public static LegacyKFlag<int> ESSRAYLE_ESCAPED_DUNGEON = new(863, "ESSRAYLE_ESCAPED_DUNGEON");

        public static LegacyKFlag<int> TOLD_MOTHER_TO_RELEASE_ESSY = new(864, "TOLD_MOTHER_TO_RELEASE_ESSY");

        public static LegacyKFlag<int> ESSY_DUNGEON_FUCKED = new(865, "ESSY_DUNGEON_FUCKED");

        public static LegacyKFlag<int> ESSY_MET_IN_DUNGEON = new(866, "ESSY_MET_IN_DUNGEON");

        public static LegacyKFlag<int> DOUBLE_ATTACK_STYLE = new(867, "DOUBLE_ATTACK_STYLE"); // DOUBLE_ATTACK_STYLE" Description="0 = always, 1 = dynamic, 2 = single

        public static LegacyKFlag<int> SPELLS_CAST = new(868, "SPELLS_CAST");

        public static LegacyKFlag<string> MILK_NAME = new(869, "MILK_NAME");

        public static LegacyKFlag<int> MILK_SIZE = new(870, "MILK_SIZE");

        public static LegacyKFlag<int> MET_MILK_SLAVE = new(871, "MET_MILK_SLAVE");

        public static LegacyKFlag<int> COUNTDOWN_TO_NIGHT_RAPE = new(872, "COUNTDOWN_TO_NIGHT_RAPE");

        public static LegacyKFlag<int> PHYLLA_CAPACITY = new(873, "PHYLLA_CAPACITY");

        public static LegacyKFlag<int> ANT_KIDS = new(874, "ANT_KIDS");

        public static LegacyKFlag<int> ANT_WAIFU = new(875, "ANT_WAIFU");

        public static LegacyKFlag<int> PHYLLA_STAY_HOME = new(876, "PHYLLA_STAY_HOME");

        public static LegacyKFlag<int> PHYLLA_CAMP_VISITS = new(877, "PHYLLA_CAMP_VISITS");

        public static LegacyKFlag<int> DAYS_PHYLLA_IN_CAMP = new(878, "DAYS_PHYLLA_IN_CAMP");

        public static LegacyKFlag<int> PHYLLA_EGG_LAYING = new(879, "PHYLLA_EGG_LAYING"); // PHYLLLA_EGG_LAYING

        public static LegacyKFlag<int> PHYLLA_BLOWJOBS = new(880, "PHYLLA_BLOWJOBS");

        public static LegacyKFlag<int> TALKED_WITH_PHYLLA_ABOUT_HISTORY = new(881, "TALKED_WITH_PHYLLA_ABOUT_HISTORY");

        public static LegacyKFlag<int> TIMES_LINKED_BJ_SUCK = new(882, "TIMES_LINKED_BJ_SUCK");

        public static LegacyKFlag<int> PHYLLA_FUCKS = new(883, "PHYLLA_FUCKS");

        public static LegacyKFlag<int> TIMES_CORRUPT_MALE_ANT_ORGY = new(884, "TIMES_CORRUPT_MALE_ANT_ORGY");

        public static LegacyKFlag<int> TIMES_CORRUPT_FEMALE_ANT_ORGY = new(885, "TIMES_CORRUPT_FEMALE_ANT_ORGY");

        public static LegacyKFlag<int> PHYLLA_TIMES_DRIDER_EGG_LAYED = new(886, "PHYLLA_TIMES_DRIDER_EGG_LAYED");

        public static LegacyKFlag<int> DAYS_PHYLLA_HAS_SPENT_BIRTHING = new(887, "DAYS_PHYLLA_HAS_SPENT_BIRTHING");

        public static LegacyKFlag<int> ANTS_BIRTHED_FROM_LICKING = new(888, "ANTS_BIRTHED_FROM_LICKING");

        public static LegacyKFlag<int> PHYLLA_COOLDOWN = new(889, "PHYLLA_COOLDOWN");

        public static LegacyKFlag<int> TIMES_EGG_IMPREGNATING_PHYLLA = new(890, "TIMES_EGG_IMPREGNATING_PHYLLA");

        public static LegacyKFlag<int> PHYLLA_DRIDER_INCUBATION = new(891, "PHYLLA_DRIDER_INCUBATION");

        public static LegacyKFlag<int> HAS_SEEN_MINO_AND_COWGIRL = new(892, "HAS_SEEN_MINO_AND_COWGIRL");

        public static LegacyKFlag<int> PHYLLA_GEMS_HUNTED_TODAY = new(893, "PHYLLA_GEMS_HUNTED_TODAY");

        public static LegacyKFlag<int> PHYLLA_DRIDER_BABIES_COUNT = new(894, "PHYLLA_DRIDER_BABIES_COUNT");

        public static LegacyKFlag<int> SOCK_COUNTER = new(895, "SOCK_COUNTER");

        public static LegacyKFlag<string> SOCK_HOLDING = new(896, "SOCK_HOLDING");

        public static LegacyKFlag<float> FOUND_SOCKS = new(897, "FOUND_SOCKS");

        public static LegacyKFlag<int> SOCKS_BOUGHT = new(898, "SOCKS_BOUGHT");

        public static LegacyKFlag<int> GILDED_JERKED = new(899, "GILDED_JERKED");

        public static LegacyKFlag<int> PHYLLA_INHERITED_KNOWLEDGE = new(900, "PHYLLA_INHERITED_KNOWLEDGE");

        public static LegacyKFlag<int> PHYLLA_IZMA_TALK = new(901, "PHYLLA_IZMA_TALK");

        public static LegacyKFlag<int> AKBAL_TIMES_BITCHED = new(902, "AKBAL_TIMES_BITCHED");

        public static LegacyKFlag<int> AKBAL_BITCH_Q = new(903, "AKBAL_BITCH_Q");

        public static LegacyKFlag<int> KELLY_BONUS_BOOB_ROWS = new(904, "KELLY_BONUS_BOOB_ROWS");

        public static LegacyKFlag<int> KELLY_INCUBATION = new(905, "KELLY_INCUBATION");

        public static LegacyKFlag<int> KELLY_TIMES_PEPPERED = new(906, "KELLY_TIMES_PEPPERED");

        public static LegacyKFlag<int> KELLY_HEAT_TIME = new(907, "KELLY_HEAT_TIME");

        public static LegacyKFlag<int> KELLY_KIDS = new(908, "KELLY_KIDS");

        public static LegacyKFlag<int> KELLY_FIRST_KID_GENDER = new(909, "KELLY_FIRST_KID_GENDER");

        public static LegacyKFlag<int> KAIJU_MEETINGS = new(910, "KAIJU_MEETINGS");

        public static LegacyKFlag<int> KAIJU_DISABLED = new(911, "KAIJU_DISABLED");

        public static LegacyKFlag<int> KAIJU_TALK_CYCLE = new(912, "KAIJU_TALK_CYCLE");

        public static LegacyKFlag<int> KAIJU_COCK = new(913, "KAIJU_COCK");

        public static LegacyKFlag<int> KAIJU_BAD_END_COUNTER = new(914, "KAIJU_BAD_END_COUNTER");

        public static LegacyKFlag<int> BROOKE_AFFECTION = new(915, "BROOKE_AFFECTION");

        public static LegacyKFlag<int> BROOKE_SHOWERED_WITH = new(916, "BROOKE_SHOWERED_WITH");

        public static LegacyKFlag<int> BROOKE_AND_HECKEL_3SOME = new(917, "BROOKE_AND_HECKEL_3SOME");

        public static LegacyKFlag<int> TIMES_IN_BROOKES_BUTT = new(918, "TIMES_IN_BROOKES_BUTT");

        public static LegacyKFlag<int> BROOKE_MEDIUM_SCENE = new(919, "BROOKE_MEDIUM_SCENE"); // BROOKE_MEDIUM_SCENE" Description="Brooke fucked ya.

        public static LegacyKFlag<int> BROOKE_MET = new(920, "BROOKE_MET");

        public static LegacyKFlag<int> BROOKE_MET_TODAY = new(921, "BROOKE_MET_TODAY");

        public static LegacyKFlag<int> BROOKE_GRUMPS_ABOUT_TA = new(922, "BROOKE_GRUMPS_ABOUT_TA");

        public static LegacyKFlag<int> FOUGHT_HOLLI = new(923, "FOUGHT_HOLLI");

        public static LegacyKFlag<int> THREATENED_HOLLI = new(924, "THREATENED_HOLLI");

        public static LegacyKFlag<int> DIDNT_FUCK_PHYLLA_ON_RECRUITMENT = new(925, "DIDNT_FUCK_PHYLLA_ON_RECRUITMENT");

        public static LegacyKFlag<int> HELIA_ANAL_TRAINING_OFFERED = new(926, "HELIA_ANAL_TRAINING_OFFERED");

        public static LegacyKFlag<int> HELIA_ANAL_TRAINING = new(927, "HELIA_ANAL_TRAINING");

        public static LegacyKFlag<int> HELIA_BIRTHDAY_OFFERED = new(928, "HELIA_BIRTHDAY_OFFERED");

        public static LegacyKFlag<int> HELIA_BDAY_DRINKS = new(929, "HELIA_BDAY_DRINKS");

        public static LegacyKFlag<int> HELIA_BDAY_HAKON_AND_KIRI = new(930, "HELIA_BDAY_HAKON_AND_KIRI");

        public static LegacyKFlag<int> HELIA_BDAY_PHOENIXES = new(931, "HELIA_BDAY_PHOENIXES");

        public static LegacyKFlag<int> HELIA_BDAY_FOX_TWINS = new(932, "HELIA_BDAY_FOX_TWINS");

        public static LegacyKFlag<int> ARIAN_FOLLOWER = new(933, "ARIAN_FOLLOWER");

        public static LegacyKFlag<int> ARIAN_PARK = new(934, "ARIAN_PARK"); // -1 = disabled, 1 = helped.

        public static LegacyKFlag<int> ARIAN_HEALTH = new(935, "ARIAN_HEALTH"); // Higher is better.

        public static LegacyKFlag<int> ARIAN_ANAL_XP = new(936, "ARIAN_ANAL_XP");

        public static LegacyKFlag<int> ARIAN_CAPACITY = new(937, "ARIAN_CAPACITY");

        public static LegacyKFlag<int> ARIAN_COCK_SIZE = new(938, "ARIAN_COCK_SIZE");

        public static LegacyKFlag<int> ARIAN_DOUBLE_COCK = new(939, "ARIAN_DOUBLE_COCK");

        public static LegacyKFlag<int> ARIAN_VAGINA = new(940, "ARIAN_VAGINA");

        public static LegacyKFlag<int> ARIAN_BREASTS = new(941, "ARIAN_BREASTS");

        public static LegacyKFlag<int> ARIAN_VIRGIN = new(942, "ARIAN_VIRGIN");

        public static LegacyKFlag<int> ARIAN_S_DIALOGUE = new(943, "ARIAN_S_DIALOGUE");

        public static LegacyKFlag<int> ARIAN_HERM_CHAT = new(944, "ARIAN_HERM_CHAT");

        public static LegacyKFlag<int> ARIAN_ASS_CHAT = new(945, "ARIAN_ASS_CHAT");

        public static LegacyKFlag<int> ARIAN_LESSONS = new(946, "ARIAN_LESSONS");

        public static LegacyKFlag<int> ARIAN_DOUBLE_PENETRATION_CHAT = new(947, "ARIAN_DOUBLE_PENETRATION_CHAT");

        public static LegacyKFlag<int> ARIAN_FIRST_REPTILUM = new(948, "ARIAN_FIRST_REPTILUM");

        public static LegacyKFlag<int> ARIAN_TREATMENT = new(949, "ARIAN_TREATMENT");

        public static LegacyKFlag<int> ARIAN_HAS_BLOWN = new(950, "ARIAN_HAS_BLOWN");

        public static LegacyKFlag<int> ARIAN_MORNING = new(951, "ARIAN_MORNING");

        public static LegacyKFlag<int> ARIAN_EGG_CHAT = new(952, "ARIAN_EGG_CHAT");

        public static LegacyKFlag<int> ARIAN_EGG_EVENT = new(953, "ARIAN_EGG_EVENT");

        public static LegacyKFlag<string> ARIAN_EGG_COLOR = new(954, "ARIAN_EGG_COLOR");

        public static LegacyKFlag<int> ARIAN_EGG_COUNTER = new(955, "ARIAN_EGG_COUNTER");

        public static LegacyKFlag<string> HELSPAWN_NAME = new(956, "HELSPAWN_NAME");

        public static LegacyKFlag<int> HELSPAWN_PERSONALITY = new(957, "HELSPAWN_PERSONALITY");

        public static LegacyKFlag<int> HELSPAWN_DADDY = new(958, "HELSPAWN_DADDY");

        public static LegacyKFlag<string> HELSPAWN_WEAPON = new(959, "HELSPAWN_WEAPON");

        public static LegacyKFlag<int> HELSPAWN_AGE = new(960, "HELSPAWN_AGE");

        public static LegacyKFlag<int> HELSPAWN_GROWUP_COUNTER = new(961, "HELSPAWN_GROWUP_COUNTER");

        public static LegacyKFlag<int> HEL_LOVE = new(962, "HEL_LOVE");

        public static LegacyKFlag<int> HELIA_KIDS_CHAT = new(963, "HELIA_KIDS_CHAT");

        public static LegacyKFlag<int> HELIA_TALK_SEVEN = new(964, "HELIA_TALK_SEVEN");

        public static LegacyKFlag<int> HEL_NTR_TRACKER = new(965, "HEL_NTR_TRACKER");

        public static LegacyKFlag<int> HEL_BONUS_POintS = new(966, "HEL_BONUS_POintS");

        public static LegacyKFlag<int> HEL_PREGNANCY_INCUBATION = new(967, "HEL_PREGNANCY_INCUBATION");

        public static LegacyKFlag<int> HELIA_PREGNANCY_TYPE = new(968, "HELIA_PREGNANCY_TYPE"); //Previously HEL_PREGNANCY_NOTICES

        public static LegacyKFlag<int> HAD_FIRST_HELSPAWN_TALK = new(969, "HAD_FIRST_HELSPAWN_TALK");

        public static LegacyKFlag<int> HELSPAWN_INCEST = new(970, "HELSPAWN_INCEST");

        public static LegacyKFlag<int> HEL_TALK_EIGHT = new(971, "HEL_TALK_EIGHT");

        public static LegacyKFlag<int> HELSPAWN_DISCOVER_BOOZE = new(972, "HELSPAWN_DISCOVER_BOOZE");

        public static LegacyKFlag<int> HELSPAWN_FUCK_intERRUPTUS = new(973, "HELSPAWN_FUCK_intERRUPTUS");

        public static LegacyKFlag<int> SPIDER_BRO_GIFT = new(974, "SPIDER_BRO_GIFT");

        public static LegacyKFlag<int> HAKON_AND_KIRI_VISIT = new(975, "HAKON_AND_KIRI_VISIT");

        public static LegacyKFlag<string> KELLY_HAIR_COLOR = new(976, "KELLY_HAIR_COLOR");

        public static LegacyKFlag<int> KELLY_TALK_N_HAND_TIMES = new(977, "KELLY_TALK_N_HAND_TIMES");

        public static LegacyKFlag<int> KELLY_TIMES_REWARDED = new(978, "KELLY_TIMES_REWARDED");

        public static LegacyKFlag<int> KELLY_TIMES_DIED_HAIR = new(979, "KELLY_TIMES_DIED_HAIR");

        public static LegacyKFlag<int> KELLY_TIMES_APPLESAUCED = new(980, "KELLY_TIMES_APPLESAUCED");

        public static LegacyKFlag<int> KELLY_REWARD_COOLDOWN = new(981, "KELLY_REWARD_COOLDOWN");

        public static LegacyKFlag<int> KIHA_CAMP_WATCH = new(982, "KIHA_CAMP_WATCH");

        public static LegacyKFlag<int> TIMES_ARIAN_DILDOED = new(983, "TIMES_ARIAN_DILDOED");

        public static LegacyKFlag<int> RUBI_BREAST_SIZE = new(984, "RUBI_BREAST_SIZE");

        public static LegacyKFlag<int> RUBI_COCK_SIZE = new(985, "RUBI_COCK_SIZE");

        public static LegacyKFlag<int> RUBI_COCK_TYPE = new(986, "RUBI_COCK_TYPE");

        public static LegacyKFlag<int> RUBI_LOWERBODY = new(987, "RUBI_LOWERBODY");

        public static LegacyKFlag<int> RUBI_HORNTYPE = new(988, "RUBI_HORNTYPE");

        public static LegacyKFlag<int> RUBI_NIPPLETYPE = new(989, "RUBI_NIPPLETYPE");

        public static LegacyKFlag<int> RUBI_NO_CUNT = new(990, "RUBI_NO_CUNT");

        public static LegacyKFlag<int> RUBI_CUNTTYPE = new(991, "RUBI_CUNTTYPE");

        public static LegacyKFlag<int> RUBI_EYE_DESCRIPT = new(992, "RUBI_EYE_DESCRIPT");

        public static LegacyKFlag<int> RUBI_EYE_TYPE = new(993, "RUBI_EYE_TYPE");

        public static LegacyKFlag<int> RUBI_SKIN = new(994, "RUBI_SKIN");

        public static LegacyKFlag<int> RUBI_HAIR = new(995, "RUBI_HAIR");

        public static LegacyKFlag<int> RUBI_HAIR_LENGTH = new(996, "RUBI_HAIR_LENGTH");

        public static LegacyKFlag<int> RUBI_BALLS_TYPE = new(997, "RUBI_BALLS_TYPE");

        public static LegacyKFlag<int> RUBI_EAR_TYPE = new(998, "RUBI_EAR_TYPE");

        public static LegacyKFlag<int> RUBI_HANDS = new(999, "RUBI_HANDS");

        public static LegacyKFlag<int> RUBI_WHISKERS = new(1000, "RUBI_WHISKERS");

        public static LegacyKFlag<int> RUBI_FEET = new(1001, "RUBI_FEET");

        public static LegacyKFlag<int> RUBI_DEBIMBO = new(1002, "RUBI_DEBIMBO");

        public static LegacyKFlag<int> RUBI_NIPPLE_TYPE = new(1003, "RUBI_NIPPLE_TYPE");

        public static LegacyKFlag<int> RUBI_SHE = new(1004, "RUBI_SHE");

        public static LegacyKFlag<int> TIMES_DISCUSSED_RUBIS_IDENTITY = new(1005, "TIMES_DISCUSSED_RUBIS_IDENTITY");

        public static LegacyKFlag<int> RUBI_ICECREAM_CONFESSION = new(1006, "RUBI_ICECREAM_CONFESSION");

        public static LegacyKFlag<int> TIMES_RUBI_DATED = new(1007, "TIMES_RUBI_DATED");

        public static LegacyKFlag<int> RUBI_FANCY_CONFESSION = new(1008, "RUBI_FANCY_CONFESSION");

        public static LegacyKFlag<int> RUBI_BAR_CONFESSION = new(1009, "RUBI_BAR_CONFESSION");

        public static LegacyKFlag<int> RUBI_TIMES_ANALLY_TRAINED = new(1010, "RUBI_TIMES_ANALLY_TRAINED");

        public static LegacyKFlag<int> RUBI_TIMES_GIVEN_AN_ITEM = new(1011, "RUBI_TIMES_GIVEN_AN_ITEM");

        public static LegacyKFlag<int> RUBI_BONDAGE_STRAPS = new(1012, "RUBI_BONDAGE_STRAPS");

        public static LegacyKFlag<int> RUBI_INQUISITORS_CORSET = new(1013, "RUBI_INQUISITORS_CORSET");

        public static LegacyKFlag<int> RUBI_SETUP = new(1014, "RUBI_SETUP");

        public static LegacyKFlag<int> EXPLORATION_PAGE = new(1015, "EXPLORATION_PAGE");

        public static LegacyKFlag<int> BOG_EXPLORED = new(1016, "BOG_EXPLORED");

        public static LegacyKFlag<int> TIMES_ENCOUNTERED_FROG = new(1017, "TIMES_ENCOUNTERED_FROG");

        public static LegacyKFlag<int> FERA_ENCOUNTER_DATE = new(1018, "FERA_ENCOUNTER_DATE");

        public static LegacyKFlag<int> FERAS_TRAP_SPRUNG = new(1019, "FERAS_TRAP_SPRUNG");

        public static LegacyKFlag<int> TIMES_VALERIA_GOO_THREESOMED = new(1020, "TIMES_VALERIA_GOO_THREESOMED");

        public static LegacyKFlag<int> MORE_TURKEY = new(1021, "MORE_TURKEY");

        public static LegacyKFlag<int> LYNNETTE_PREGNANCY_CYCLE = new(1022, "LYNNETTE_PREGNANCY_CYCLE"); //0-3 = pregnant. 4-6 = not.

        public static LegacyKFlag<int> LYNNETTE_APPROVAL = new(1023, "LYNNETTE_APPROVAL");

        public static LegacyKFlag<int> LYNNETTE_BABY_COUNT = new(1024, "LYNNETTE_BABY_COUNT");

        public static LegacyKFlag<int> LYNNETTE_CARRYING_COUNT = new(1025, "LYNNETTE_CARRYING_COUNT");

        public static LegacyKFlag<int> LYNNETTE_MET_UNPREGNANT = new(1026, "LYNNETTE_MET_UNPREGNANT");

        public static LegacyKFlag<int> LYNNETTE_ANNOUNCED_APPROVAL = new(1027, "LYNNETTE_ANNOUNCED_APPROVAL");

        public static LegacyKFlag<int> LYNNETTE_FUCK_COUNTER = new(1028, "LYNNETTE_FUCK_COUNTER");

        public static LegacyKFlag<int> UMA_TIMES_SEXED = new(1029, "UMA_TIMES_SEXED");

        public static LegacyKFlag<int> UMA_TIMES_MASSAGED = new(1030, "UMA_TIMES_MASSAGED");

        public static LegacyKFlag<int> UMA_TIMES_ACUPUNCTURE_UNDO = new(1031, "UMA_TIMES_ACUPUNCTURE_UNDO");

        public static LegacyKFlag<int> UMA_TIMES_SUCKED_YOU = new(1032, "UMA_TIMES_SUCKED_YOU");

        public static LegacyKFlag<int> TIMES_RUT_FUCKED_URTAS_CUNT = new(1033, "TIMES_RUT_FUCKED_URTAS_CUNT");

        public static LegacyKFlag<int> TIMES_KIHA_ANALED = new(1034, "TIMES_KIHA_ANALED");

        public static LegacyKFlag<int> TIMES_ENCOUNTERED_GOBLIN_ASSASSIN = new(1035, "TIMES_ENCOUNTERED_GOBLIN_ASSASSIN");

        public static LegacyKFlag<int> IZUMI_MET = new(1036, "IZUMI_MET"); // Player has had the introduction/meeting scene with Izumi

        public static LegacyKFlag<int> IZUMI_LAST_ENCOUNTER = new(1037, "IZUMI_LAST_ENCOUNTER"); // Outcome of the last encounter with Izumi, 0 = Unset, 1 = PC Lost in a fight, 2 = PC ran away, 3 = PC Won in a fight

        public static LegacyKFlag<int> IZUMI_TIMES_SUBMITTED = new(1038, "IZUMI_TIMES_SUBMITTED"); // Times the player has voluntarily submitted to Izumi's demands

        public static LegacyKFlag<int> IZUMI_TIMES_GRABBED_THE_HORN = new(1039, "IZUMI_TIMES_GRABBED_THE_HORN"); // Times the player has beaten Izumi in a fight, and touched her horn.

        public static LegacyKFlag<int> IZUMI_TIMES_LOST_FIGHT = new(1040, "IZUMI_TIMES_LOST_FIGHT"); // Times the player has been beaten by Izumi in a fight

        public static LegacyKFlag<int> IZUMI_SEEN_PC_GENDER = new(1041, "IZUMI_SEEN_PC_GENDER"); // Indicates if Izumi knows the PCs true gender/equipment (val in the last gender int that Izumi knows the player was, "IZUMI_SEEN_PC_GENDER")
        //public static LegacyKFlag<int> CUSTOM_FONT_SIZE:int = new LegacyKFlag<int>(1042); // No longer used
        public static LegacyKFlag<int> FERRET_BAD_END_WARNING = new(1043, "FERRET_BAD_END_WARNING");

        public static LegacyKFlag<int> TIMES_TALKED_WITH_JOJO = new(1044, "TIMES_TALKED_WITH_JOJO"); // Number of times the player has talked with Pure Jojo.

        public static LegacyKFlag<int> TIMES_TRAINED_WITH_JOJO = new(1045, "TIMES_TRAINED_WITH_JOJO"); // Number of times the player has trained with Jojo

        public static LegacyKFlag<int> UNLOCKED_JOJO_TRAINING = new(1046, "UNLOCKED_JOJO_TRAINING");

        public static LegacyKFlag<int> MET_FROSTY = new(1047, "MET_FROSTY");

        public static LegacyKFlag<int> SIGNED_FROSTYS_CONTRACT = new(1048, "SIGNED_FROSTYS_CONTRACT");

        public static LegacyKFlag<int> FROSTY_POintS = new(1049, "FROSTY_POintS");

        public static LegacyKFlag<int> SEEN_GENDERLESS_FROSTY_REJECTION = new(1050, "SEEN_GENDERLESS_FROSTY_REJECTION");

        public static LegacyKFlag<int> MARBLE_PURIFIED = new(1051, "MARBLE_PURIFIED");

        public static LegacyKFlag<int> MARBLE_BOYS = new(1052, "MARBLE_BOYS");

        public static LegacyKFlag<int> CLARA_IMPRISONED = new(1053, "CLARA_IMPRISONED");

        public static LegacyKFlag<int> CLARA_PURIFIED = new(1054, "CLARA_PURIFIED");

        public static LegacyKFlag<int> MARBLE_TIME_SINCE_NURSED_IN_HOURS = new(1055, "MARBLE_TIME_SINCE_NURSED_IN_HOURS");

        public static LegacyKFlag<int> MARBLE_BREAST_SIZE = new(1056, "MARBLE_BREAST_SIZE");

        public static LegacyKFlag<int> TIMES_GIVEN_MARBLE_PURE_LABOVA = new(1057, "TIMES_GIVEN_MARBLE_PURE_LABOVA");

        public static LegacyKFlag<int> MARBLE_RATHAZUL_COUNTER_1 = new(1058, "MARBLE_RATHAZUL_COUNTER_1");

        public static LegacyKFlag<int> MARBLE_PURIFICATION_STAGE = new(1059, "MARBLE_PURIFICATION_STAGE");

        public static LegacyKFlag<int> MURBLE_TEA_DRINKER_COUNT = new(1060, "MURBLE_TEA_DRINKER_COUNT");

        public static LegacyKFlag<int> MARBLE_RATHAZUL_COUNTER_2 = new(1061, "MARBLE_RATHAZUL_COUNTER_2");

        public static LegacyKFlag<int> MARBLE_WARNED_ABOUT_CORRUPTION = new(1062, "MARBLE_WARNED_ABOUT_CORRUPTION");

        public static LegacyKFlag<int> MARBLE_LEFT_OVER_CORRUPTION = new(1063, "MARBLE_LEFT_OVER_CORRUPTION");

        public static LegacyKFlag<int> MARBLE_COUNTUP_TO_PURIFYING = new(1064, "MARBLE_COUNTUP_TO_PURIFYING");

        public static LegacyKFlag<int> TIMES_ENCOUNTERED_PRINCESS_GWYNN = new(1065, "TIMES_ENCOUNTERED_PRINCESS_GWYNN"); // Times the player has run into Princess Gwyn

        public static LegacyKFlag<int> WILD_HUNT_ENCOUNTERS = new(1066, "WILD_HUNT_ENCOUNTERS");    // Total wild hunt encounters

        public static LegacyKFlag<int> ERLKING_DISABLED = new(1067, "ERLKING_DISABLED"); // Wild hunt/Erlking has been disabled
        //public static LegacyKFlag<int> ERLKING_ENCOUNTER_COUNTER:int = new LegacyKFlag<int>(1068); // used to check "every 5th encounter" which was replaced with random
        public static LegacyKFlag<int> KATHERINE_MET_SCYLLA = new(1069, "KATHERINE_MET_SCYLLA");

        public static LegacyKFlag<int> WHITNEY_TALK_MURBLE_AND_KELT = new(1070, "WHITNEY_TALK_MURBLE_AND_KELT"); // Player has done the talk with Whitney requiring they have met Murble and Kelt

        public static LegacyKFlag<int> WHITNEY_TALK_TELADRE = new(1071, "WHITNEY_TALK_TELADRE"); // Player talked to Whitney about TelAdre

        public static LegacyKFlag<int> WHITNEY_TALK_HIGH_MOUNTAIN = new(1072, "WHITNEY_TALK_HIGH_MOUNTAIN"); // Player talked to Whitney about the High Mountains

        public static LegacyKFlag<int> WHITNEY_TALK_DUNGEON = new(1073, "WHITNEY_TALK_DUNGEON"); // Player talked to Whitney about shutting D1 down

        public static LegacyKFlag<int> WHITNEY_TALK_DUNGEON_FOLLOWUP = new(1074, "WHITNEY_TALK_DUNGEON_FOLLOWUP"); // Player talked to Whitney about D1 #

        public static LegacyKFlag<int> FARM_CORRUPTION_DISABLED = new(1075, "FARM_CORRUPTION_DISABLED"); // Player selected "Never" when presented with an option to corrupt farm

        public static LegacyKFlag<int> FARM_CORRUPT_PROMPT_DISPLAY = new(1076, "FARM_CORRUPT_PROMPT_DISPLAY"); // Tracking the state for prompt display

        public static LegacyKFlag<int> FARM_CORRUPTION_STARTED = new(1077, "FARM_CORRUPTION_STARTED"); // Player has started the farm corruption process

        public static LegacyKFlag<int> WHITNEY_CORRUPTION = new(1078, "WHITNEY_CORRUPTION"); // Whitney's corruption value

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_AMILY = new(1079, "FOLLOWER_AT_FARM_AMILY"); // Amily as at the farm

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_JOJO = new(1080, "FOLLOWER_AT_FARM_JOJO"); // Jojo is at the farm

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_SOPHIE = new(1081, "FOLLOWER_AT_FARM_SOPHIE"); // Sophie is at the farm (0 = no, 1 = Sophie, 2 = Bimbo Sophie, "FOLLOWER_AT_FARM_SOPHIE")

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_IZMA = new(1082, "FOLLOWER_AT_FARM_IZMA"); // Izma is at the farm (0 = no, 1 = Izma, 2 = Izmael, "FOLLOWER_AT_FARM_IZMA")

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_ISABELLA = new(1083, "FOLLOWER_AT_FARM_ISABELLA"); // Isabell is at the farm

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_VAPULA = new(1084, "FOLLOWER_AT_FARM_VAPULA"); // Vapula is at the farm

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_LATEXY = new(1085, "FOLLOWER_AT_FARM_LATEXY"); // Latexy is at the farm

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_CERAPH = new(1086, "FOLLOWER_AT_FARM_CERAPH"); // Ceraph is at the farm

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_HOLLI = new(1087, "FOLLOWER_AT_FARM_HOLLI"); // Holli has corrupted the farm (Yes I know she can't actually move gtfo) (0 = no, anything else = num days affected, "FOLLOWER_AT_FARM_HOLLI")

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_KELLY = new(1088, "FOLLOWER_AT_FARM_KELLY"); // Kelly is at the farm (Don't think this is strictly required, depends if pc can disable Kelly's corruption effect, "FOLLOWER_AT_FARM_KELLY")

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_BATH_GIRL = new(1089, "FOLLOWER_AT_FARM_BATH_GIRL"); // Bathslut is at the farm

        public static LegacyKFlag<int> FARM_CORRUPTION_FIRST_DATE = new(1090, "FARM_CORRUPTION_FIRST_DATE"); // Day value that the Farm completed the corruption process

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_MARBLE = new(1091, "FOLLOWER_AT_FARM_MARBLE");

        public static LegacyKFlag<int> FARM_CORRUPTION_DAYS_SINCE_LAST_PAYOUT = new(1092, "FARM_CORRUPTION_DAYS_SINCE_LAST_PAYOUT");

        public static LegacyKFlag<int> FARM_CORRUPTION_GEMS_WAITING = new(1093, "FARM_CORRUPTION_GEMS_WAITING");

        public static LegacyKFlag<int> FARM_UPGRADES_REFINERY = new(1094, "FARM_UPGRADES_REFINERY");

        public static LegacyKFlag<int> WHITNEY_CORRUPTION_COMPLETE = new(1095, "WHITNEY_CORRUPTION_COMPLETE");

        public static LegacyKFlag<int> FOLLOWER_PRODUCTION_AMILY = new(1096, "FOLLOWER_PRODUCTION_AMILY");

        public static LegacyKFlag<int> FARM_SUCCUMILK_STORED = new(1097, "FARM_SUCCUMILK_STORED");

        public static LegacyKFlag<int> FOLLOWER_PRODUCTION_JOJO = new(1098, "FOLLOWER_PRODUCTION_JOJO");

        public static LegacyKFlag<int> FARM_INCUDRAFT_STORED = new(1099, "FARM_INCUDRAFT_STORED");

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_AMILY_GIBS_MILK = new(1100, "FOLLOWER_AT_FARM_AMILY_GIBS_MILK");

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_JOJO_GIBS_DRAFT = new(1101, "FOLLOWER_AT_FARM_JOJO_GIBS_DRAFT");

        public static LegacyKFlag<int> FOLLOWER_PRODUCTION_SOPHIE = new(1102, "FOLLOWER_PRODUCTION_SOPHIE");

        public static LegacyKFlag<string> FOLLOWER_PRODUCTION_SOPHIE_COLORCHOICE = new(1103, "FOLLOWER_PRODUCTION_SOPHIE_COLORCHOICE");

        public static LegacyKFlag<int> FARM_EGG_STORED = new(1104, "FARM_EGG_STORED");

        public static LegacyKFlag<int> FARM_EGG_COUNTDOWN = new(1105, "FARM_EGG_COUNTDOWN");

        public static LegacyKFlag<int> FOLLOWER_PRODUCTION_VAPULA = new(1106, "FOLLOWER_PRODUCTION_VAPULA");

        public static LegacyKFlag<int> FOLLOWER_AT_FARM_VAPULA_GIBS_MILK = new(1107, "FOLLOWER_AT_FARM_VAPULA_GIBS_MILK");

        public static LegacyKFlag<int> HOLLI_FUCKED_TODAY = new(1108, "HOLLI_FUCKED_TODAY");

        public static LegacyKFlag<int> FARM_UPGRADES_CONTRACEPTIVE = new(1109, "FARM_UPGRADES_CONTRACEPTIVE");

        public static LegacyKFlag<int> KELT_TALKED_FARM_MANAGEMENT = new(1110, "KELT_TALKED_FARM_MANAGEMENT");

        public static LegacyKFlag<int> FARM_CONTRACEPTIVE_STORED = new(1111, "FARM_CONTRACEPTIVE_STORED");

        public static LegacyKFlag<int> WHITNEY_CORRUPTION_HIGHEST = new(1112, "WHITNEY_CORRUPTION_HIGHEST");

        public static LegacyKFlag<int> WHITNEY_CORRUPTION_0_30_DROP_MESSAGE = new(1113, "WHITNEY_CORRUPTION_0_30_DROP_MESSAGE");

        public static LegacyKFlag<int> FARM_CORRUPTION_APPROACHED_WHITNEY = new(1114, "FARM_CORRUPTION_APPROACHED_WHITNEY");

        public static LegacyKFlag<int> WHITNEY_LEAVE_0_60 = new(1115, "WHITNEY_LEAVE_0_60");

        public static LegacyKFlag<int> WHITNEY_MENU_31_60 = new(1116, "WHITNEY_MENU_31_60");

        public static LegacyKFlag<int> WHITNEY_MENU_61_90 = new(1117, "WHITNEY_MENU_61_90");

        public static LegacyKFlag<int> WHITNEY_LEAVE_61_90 = new(1118, "WHITNEY_LEAVE_61_90");

        public static LegacyKFlag<int> WHITNEY_MENU_91_119 = new(1119, "WHITNEY_MENU_91_119");

        public static LegacyKFlag<int> WHITNEY_DISABLED_FOR_DAY = new(1120, "WHITNEY_DISABLED_FOR_DAY");

        public static LegacyKFlag<int> WHITNEY_DEFURRED = new(1121, "WHITNEY_DEFURRED");

        public static LegacyKFlag<int> WHITNEY_DOM = new(1122, "WHITNEY_DOM");

        public static LegacyKFlag<int> FARM_UPGRADES_MILKTANK = new(1123, "FARM_UPGRADES_MILKTANK");

        public static LegacyKFlag<int> QUEUE_BREASTMILKER_UPGRADE = new(1124, "QUEUE_BREASTMILKER_UPGRADE");

        public static LegacyKFlag<int> QUEUE_COCKMILKER_UPGRADE = new(1125, "QUEUE_COCKMILKER_UPGRADE");

        public static LegacyKFlag<int> QUEUE_REFINERY_UPGRADE = new(1126, "QUEUE_REFINERY_UPGRADE");

        public static LegacyKFlag<int> QUEUE_CONTRACEPTIVE_UPGRADE = new(1127, "QUEUE_CONTRACEPTIVE_UPGRADE");

        public static LegacyKFlag<int> QUEUE_MILKTANK_UPGRADE = new(1128, "QUEUE_MILKTANK_UPGRADE");

        public static LegacyKFlag<string> WHITNEY_TATTOO_COLLARBONE = new(1129, "WHITNEY_TATTOO_COLLARBONE");

        public static LegacyKFlag<string> WHITNEY_TATTOO_SHOULDERS = new(1130, "WHITNEY_TATTOO_SHOULDERS");

        public static LegacyKFlag<string> WHITNEY_TATTOO_LOWERBACK = new(1131, "WHITNEY_TATTOO_LOWERBACK");

        public static LegacyKFlag<string> WHITNEY_TATTOO_BUTT = new(1132, "WHITNEY_TATTOO_BUTT");

        public static LegacyKFlag<int> FARM_CORRUPTION_BRANDING_MENU_UNLOCKED = new(1133, "FARM_CORRUPTION_BRANDING_MENU_UNLOCKED");

        public static LegacyKFlag<int> QUEUE_BRANDING_UPGRADE = new(1134, "QUEUE_BRANDING_UPGRADE");

        public static LegacyKFlag<int> QUEUE_BRANDING_AVAILABLE_TALK = new(1135, "QUEUE_BRANDING_AVAILABLE_TALK");

        public static LegacyKFlag<int> WHITNEY_ORAL_TRAINING = new(1136, "WHITNEY_ORAL_TRAINING");

        public static LegacyKFlag<string> AMILY_TATTOO_COLLARBONE = new(1137, "AMILY_TATTOO_COLLARBONE");

        public static LegacyKFlag<string> AMILY_TATTOO_SHOULDERS = new(1138, "AMILY_TATTOO_SHOULDERS");

        public static LegacyKFlag<string> AMILY_TATTOO_LOWERBACK = new(1139, "AMILY_TATTOO_LOWERBACK");

        public static LegacyKFlag<string> AMILY_TATTOO_BUTT = new(1140, "AMILY_TATTOO_BUTT");

        public static LegacyKFlag<string> JOJO_TATTOO_COLLARBONE = new(1141, "JOJO_TATTOO_COLLARBONE");

        public static LegacyKFlag<string> JOJO_TATTOO_SHOULDERS = new(1142, "JOJO_TATTOO_SHOULDERS");

        public static LegacyKFlag<string> JOJO_TATTOO_LOWERBACK = new(1143, "JOJO_TATTOO_LOWERBACK");

        public static LegacyKFlag<string> JOJO_TATTOO_BUTT = new(1144, "JOJO_TATTOO_BUTT");

        public static LegacyKFlag<string> SOPHIE_TATTOO_COLLARBONE = new(1145, "SOPHIE_TATTOO_COLLARBONE");

        public static LegacyKFlag<string> SOPHIE_TATTOO_SHOULDERS = new(1146, "SOPHIE_TATTOO_SHOULDERS");

        public static LegacyKFlag<string> SOPHIE_TATTOO_LOWERBACK = new(1147, "SOPHIE_TATTOO_LOWERBACK");

        public static LegacyKFlag<string> SOPHIE_TATTOO_BUTT = new(1148, "SOPHIE_TATTOO_BUTT");

        public static LegacyKFlag<string> VAPULA_TATTOO_COLLARBONE = new(1149, "VAPULA_TATTOO_COLLARBONE");

        public static LegacyKFlag<string> VAPULA_TATTOO_SHOULDERS = new(1150, "VAPULA_TATTOO_SHOULDERS");

        public static LegacyKFlag<string> VAPULA_TATTOO_LOWERBACK = new(1151, "VAPULA_TATTOO_LOWERBACK");

        public static LegacyKFlag<string> VAPULA_TATTOO_BUTT = new(1152, "VAPULA_TATTOO_BUTT");

        public static LegacyKFlag<string> KELLY_TATTOO_COLLARBONE = new(1153, "KELLY_TATTOO_COLLARBONE");

        public static LegacyKFlag<string> KELLY_TATTOO_SHOULDERS = new(1154, "KELLY_TATTOO_SHOULDERS");

        public static LegacyKFlag<string> KELLY_TATTOO_LOWERBACK = new(1155, "KELLY_TATTOO_LOWERBACK");

        public static LegacyKFlag<string> KELLY_TATTOO_BUTT = new(1156, "KELLY_TATTOO_BUTT");

        public static LegacyKFlag<string> MILKY_TATTOO_COLLARBONE = new(1157, "MILKY_TATTOO_COLLARBONE");

        public static LegacyKFlag<string> MILKY_TATTOO_SHOULDERS = new(1158, "MILKY_TATTOO_SHOULDERS");

        public static LegacyKFlag<string> MILKY_TATTOO_LOWERBACK = new(1159, "MILKY_TATTOO_LOWERBACK");

        public static LegacyKFlag<string> MILKY_TATTOO_BUTT = new(1160, "MILKY_TATTOO_BUTT");

        public static LegacyKFlag<int> WHITNEY_DOM_FIRST_PLEASURE = new(1161, "WHITNEY_DOM_FIRST_PLEASURE");

        public static LegacyKFlag<int> FARM_UPGRADES_ORGYROOM = new(1162, "FARM_UPGRADES_ORGYROOM");

        public static LegacyKFlag<int> MASSAGE_HAPPY_ENDINGS = new(1163, "MASSAGE_HAPPY_ENDINGS");

        public static LegacyKFlag<int> QUEUE_ORGYROOM_UPGRADE = new(1164, "QUEUE_ORGYROOM_UPGRADE");

        public static LegacyKFlag<int> FOUND_CATHEDRAL = new(1165, "FOUND_CATHEDRAL");

        public static LegacyKFlag<int> PLACES_PAGE = new(1166, "PLACES_PAGE");
        //public static LegacyKFlag<int> FUCK_YOU_GOT_MINE_RAPHAEL:int = new LegacyKFlag<int>(1167); //Unused
        public static LegacyKFlag<int> MARBLE_MILKED_BEFORE = new(1168, "MARBLE_MILKED_BEFORE");

        public static LegacyKFlag<int> TATTOO_SAVEFIX_APPLIED = new(1169, "TATTOO_SAVEFIX_APPLIED");

        public static LegacyKFlag<int> BIRTHS_PHOUKA = new(1170, "BIRTHS_PHOUKA"); // For the status page, possible future content

        public static LegacyKFlag<int> BIRTHS_FAERIE = new(1171, "BIRTHS_FAERIE"); // For the status page, possible future content

        public static LegacyKFlag<int> PHOUKA_ENCOUNTER_STATUS = new(1172, "PHOUKA_ENCOUNTER_STATUS"); // 0 for never encountered, 1 for encountered at least once, other values can be used later if you're doing something for one

        public static LegacyKFlag<int> PHOUKA_LORE = new(1173, "PHOUKA_LORE"); // 0 when you don't know what they're called, 1 once you learn their name, higher values as you hear more drunken stories.

        public static LegacyKFlag<int> PREGNANCY_CORRUPTION = new(1174, "PREGNANCY_CORRUPTION"); // Starts at 2 when you get pregnant. Drops if you drink pure honey, rises if you drink phouka whiskey

        public static LegacyKFlag<int> TREACLE_MINE_YEAR_DONE = new(1175, "TREACLE_MINE_YEAR_DONE"); // Same as for the Fera encounter, this is a once a year Halloween deal

        public static LegacyKFlag<int> PLAYER_INFECTED_HELLHOUNDS = new(1176, "PLAYER_INFECTED_HELLHOUNDS");

        public static LegacyKFlag<int> PLAYER_INFECTED_MINOTAURS = new(1177, "PLAYER_INFECTED_MINOTAURS");

        public static LegacyKFlag<int> PLAYER_INFECTED_IMPS = new(1178, "PLAYER_INFECTED_IMPS");

        public static LegacyKFlag<int> FEMOIT_UNLOCKED = new(1179, "FEMOIT_UNLOCKED"); // Unlocked the option to feminize Benoit

        public static LegacyKFlag<int> BIMBO_FEMOIT_UNLOCKED = new(1180, "BIMBO_FEMOIT_UNLOCKED"); // Unlocked the option to bimbofy Benoit

        public static LegacyKFlag<int> BENOIT_STATUS = new(1181, "BENOIT_STATUS"); // Benoit's "mode" -- 0 = Male, 1 = Fem, 2 = Bimbo, 3 = Herm (only in mod, "BENOIT_STATUS")

        public static LegacyKFlag<int> FEMOIT_NEXTDAY_EVENT = new(1182, "FEMOIT_NEXTDAY_EVENT"); // Proc the event the following day when you visit - stores the minimum day number that the event can proc on.

        public static LegacyKFlag<int> FEMOIT_TALKED_TO = new(1183, "FEMOIT_TALKED_TO");

        public static LegacyKFlag<int> KATHERINE_RANDOM_RECRUITMENT_DISABLED = new(1184, "KATHERINE_RANDOM_RECRUITMENT_DISABLED");

        public static LegacyKFlag<int> TIMES_FUCKED_FEMOIT = new(1185, "TIMES_FUCKED_FEMOIT");

        public static LegacyKFlag<int> FEMOIT_EGGS = new(1186, "FEMOIT_EGGS");

        public static LegacyKFlag<int> FEMOIT_INCUBATION = new(1187, "FEMOIT_INCUBATION");

        public static LegacyKFlag<int> FEMOIT_READY_FOR_EGGS = new(1188, "FEMOIT_READY_FOR_EGGS");

        public static LegacyKFlag<int> FEMOIT_FIRST_CLUTCH_MISSED = new(1189, "FEMOIT_FIRST_CLUTCH_MISSED");

        public static LegacyKFlag<int> FEMOIT_EGGS_LAID = new(1190, "FEMOIT_EGGS_LAID");

        public static LegacyKFlag<int> FEMOIT_HELPED_LAY = new(1191, "FEMOIT_HELPED_LAY");

        public static LegacyKFlag<int> FEMOIT_SPOONED = new(1192, "FEMOIT_SPOONED");

        public static LegacyKFlag<int> FEMOIT_NEXTDAY_EVENT_DONE = new(1193, "FEMOIT_NEXTDAY_EVENT_DONE");

        public static LegacyKFlag<int> VALA_HEALED_HONEY = new(1194, "VALA_HEALED_HONEY");

        public static LegacyKFlag<int> AMILY_PREGNANCY_TYPE = new(1195, "AMILY_PREGNANCY_TYPE");

        public static LegacyKFlag<int> AMILY_BUTT_PREGNANCY_TYPE = new(1196, "AMILY_BUTT_PREGNANCY_TYPE");

        public static LegacyKFlag<int> FEMALE_SPIDERMORPH_PREGNANCY_TYPE = new(1197, "FEMALE_SPIDERMORPH_PREGNANCY_TYPE");

        public static LegacyKFlag<int> PHYLLA_VAGINAL_PREGNANCY_TYPE = new(1198, "PHYLLA_VAGINAL_PREGNANCY_TYPE");

        public static LegacyKFlag<int> TAMANI_PREGNANCY_TYPE = new(1199, "TAMANI_PREGNANCY_TYPE");

        public static LegacyKFlag<int> TAMANI_PREGNANCY_INCUBATION = new(1200, "TAMANI_PREGNANCY_INCUBATION");

        public static LegacyKFlag<int> TAMANI_PREGNANCY_COUNT = new(1201, "TAMANI_PREGNANCY_COUNT"); //Just how many has she got stuffed in there?

        public static LegacyKFlag<int> TAMANI_TIMES_IMPREGNATED = new(1202, "TAMANI_TIMES_IMPREGNATED");

        public static LegacyKFlag<int> TAMANI_MET = new(1203, "TAMANI_MET");

        public static LegacyKFlag<int> TAMANI_NUMBER_OF_DAUGHTERS = new(1204, "TAMANI_NUMBER_OF_DAUGHTERS");

        public static LegacyKFlag<int> TAMANI_DAUGHTERS_PREGNANCY_TYPE = new(1205, "TAMANI_DAUGHTERS_PREGNANCY_TYPE");

        public static LegacyKFlag<int> TAMANI_DAUGHTERS_PREGNANCY_COUNT = new(1206, "TAMANI_DAUGHTERS_PREGNANCY_COUNT");

        public static LegacyKFlag<int> IZMA_PREGNANCY_TYPE = new(1207, "IZMA_PREGNANCY_TYPE");

        public static LegacyKFlag<int> EMBER_PREGNANCY_TYPE = new(1208, "EMBER_PREGNANCY_TYPE");

        public static LegacyKFlag<int> JOJO_BUTT_PREGNANCY_TYPE = new(1209, "JOJO_BUTT_PREGNANCY_TYPE");

        public static LegacyKFlag<int> SHEILA_DISABLED = new(1210, "SHEILA_DISABLED"); //1 = Told to avoid or you refused apology, 2 = Baby Issues, 3 = Worm Infested, 4 = Jojo's 'Wife'

        public static LegacyKFlag<int> SHEILA_PREGNANCY_TYPE = new(1211, "SHEILA_PREGNANCY_TYPE");

        public static LegacyKFlag<int> KELLY_PREGNANCY_TYPE = new(1212, "KELLY_PREGNANCY_TYPE");

        public static LegacyKFlag<int> KELLY_KIDS_MALE = new(1213, "KELLY_KIDS_MALE"); //Tracks number of sons with Kelly. Total number of children is stored in KELLY_KIDS

        public static LegacyKFlag<bool> FUCK_OFF_THEIVING_RAPHAEL = new(1214, "FUCK_OFF_THEIVING_RAPHAEL");

        public static LegacyKFlag<int> VALA_TIMES_CONSENSUAL_SEX = new(1215, "VALA_TIMES_CONSENSUAL_SEX");

        public static LegacyKFlag<int> KATHERINE_DICK_FORM = new(1216, "KATHERINE_DICK_FORM");

        public static LegacyKFlag<string> KATHERINE_BREAST_SIZE = new(1217, "KATHERINE_BREAST_SIZE");

        public static LegacyKFlag<int> KATHERINE_HOURS_SINCE_CUM = new(1218, "KATHERINE_HOURS_SINCE_CUM");

        public static LegacyKFlag<int> KATHERINE_SEXUAL_EXPERIENCE = new(1219, "KATHERINE_SEXUAL_EXPERIENCE");

        public static LegacyKFlag<int> KATHERINE_CLOTHES = new(1220, "KATHERINE_CLOTHES");

        public static LegacyKFlag<int> KATHERINE_CLOTHES_PREF = new(1221, "KATHERINE_CLOTHES_PREF");

        public static LegacyKFlag<int> KATHERINE_CLOTHES_WORN = new(1222, "KATHERINE_CLOTHES_WORN");

        public static LegacyKFlag<string> KATHERINE_HAIR_COLOR = new(1223, "KATHERINE_HAIR_COLOR");

        public static LegacyKFlag<int> KATHERINE_IS_CAT_GIRL = new(1224, "KATHERINE_IS_CAT_GIRL");

        public static LegacyKFlag<int> KATHERINE_LOCATION = new(1225, "KATHERINE_LOCATION");

        public static LegacyKFlag<int> KATHERINE_TRAINING = new(1226, "KATHERINE_TRAINING");

        public static LegacyKFlag<int> KATHERINE_SUB_FLAGS = new(1227, "KATHERINE_SUB_FLAGS");

        public static LegacyKFlag<int> KATHERINE_AMILY_AFFECTION = new(1228, "KATHERINE_AMILY_AFFECTION");

        public static LegacyKFlag<int> KATHERINE_COTTON_AFFECTION = new(1229, "KATHERINE_COTTON_AFFECTION");

        public static LegacyKFlag<int> KATHERINE_EDRYN_AFFECTION = new(1230, "KATHERINE_EDRYN_AFFECTION");

        public static LegacyKFlag<int> KATHERINE_HELIA_AFFECTION = new(1231, "KATHERINE_HELIA_AFFECTION");

        public static LegacyKFlag<int> KATHERINE_URTA_AFFECTION = new(1232, "KATHERINE_URTA_AFFECTION");

        public static LegacyKFlag<int> KATHERINE_URTA_DATE = new(1233, "KATHERINE_URTA_DATE");

        public static LegacyKFlag<int> KATHERINE_URTA_TIMES_SEX = new(1234, "KATHERINE_URTA_TIMES_SEX");

        public static LegacyKFlag<int> KATHERINE_VALA_AFFECTION = new(1235, "KATHERINE_VALA_AFFECTION");

        public static LegacyKFlag<int> KATHERINE_VALA_DATE = new(1236, "KATHERINE_VALA_DATE");

        public static LegacyKFlag<int> KATHERINE_VALA_TIMES_SEX = new(1237, "KATHERINE_VALA_TIMES_SEX");

        public static LegacyKFlag<int> SAVE_FILE_intEGER_FORMAT_VERSION = new(1238, "SAVE_FILE_intEGER_FORMAT_VERSION");

        public static LegacyKFlag<int> D3_DISCOVERED = new(1239, "D3_DISCOVERED");

        public static LegacyKFlag<int> D3_ENTERED_MAGPIEHALL = new(1240, "D3_ENTERED_MAGPIEHALL");

        public static LegacyKFlag<int> D3_BASILISKS_REMOVED_FROM_MAGPIE_HALL = new(1241, "D3_BASILISKS_REMOVED_FROM_MAGPIE_HALL");

        public static LegacyKFlag<int> D3_MIRRORS_SHATTERED = new(1242, "D3_MIRRORS_SHATTERED");

        public static LegacyKFlag<int> D3_JEAN_CLAUDE_DEFEATED = new(1243, "D3_JEAN_CLAUDE_DEFEATED");

        public static LegacyKFlag<int> D3_DOPPELGANGER_DEFEATED = new(1244, "D3_DOPPELGANGER_DEFEATED");

        public static LegacyKFlag<int> D3_MECHANIC_LAST_GREET = new(1245, "D3_MECHANIC_LAST_GREET");

        public static LegacyKFlag<int> D3_MECHANIC_FIGHT_RESULT = new(1246, "D3_MECHANIC_FIGHT_RESULT");

        public static LegacyKFlag<int> D3_MECHANIC_COCK_TYPE_SELECTION = new(1247, "D3_MECHANIC_COCK_TYPE_SELECTION"); // This is the kinda shit that sounds like it might get referenced in future (HAHAHA YEAH RIGHT?, "D3_MECHANIC_COCK_TYPE_SELECTION")

        public static LegacyKFlag<int> D3_STATUE_DEFEATED = new(1248, "D3_STATUE_DEFEATED");

        public static LegacyKFlag<int> D3_GARDENER_DEFEATED = new(1249, "D3_GARDENER_DEFEATED");

        public static LegacyKFlag<int> D1_OMNIBUS_KILLED = new(1250, "D1_OMNIBUS_KILLED");

        public static LegacyKFlag<int> D3_CENTAUR_DEFEATED = new(1251, "D3_CENTAUR_DEFEATED");

        public static LegacyKFlag<int> D3_EGGS_AVAILABLE = new(1252, "D3_EGGS_AVAILABLE");

        public static LegacyKFlag<int> NOT_HELPED_ARIAN_TODAY = new(1253, "NOT_HELPED_ARIAN_TODAY");

        public static LegacyKFlag<int> BEE_GIRL_STATUS = new(1254, "BEE_GIRL_STATUS"); //Player Attitude + Conversation Stage

        public static LegacyKFlag<int> BEE_GIRL_COMBAT_WINS_WITH_RAPE = new(1255, "BEE_GIRL_COMBAT_WINS_WITH_RAPE");

        public static LegacyKFlag<int> BEE_GIRL_COMBAT_WINS_WITHOUT_RAPE = new(1256, "BEE_GIRL_COMBAT_WINS_WITHOUT_RAPE");

        public static LegacyKFlag<int> BEE_GIRL_COMBAT_LOSSES = new(1257, "BEE_GIRL_COMBAT_LOSSES");

        public static LegacyKFlag<int> KATHERINE_MILK_OPTIONS = new(1258, "KATHERINE_MILK_OPTIONS");

        public static LegacyKFlag<int> SCYLLA_FURRY_FOURSOME_COUNT = new(1259, "SCYLLA_FURRY_FOURSOME_COUNT");

        public static LegacyKFlag<int> SCYLLA_SMALLCOCK_intRO = new(1260, "SCYLLA_SMALLCOCK_intRO"); // Player has had the initial lil' dick intro version with Scylla

        public static LegacyKFlag<int> DRIDERINCUBUS_DEFEATED = new(1261, "DRIDERINCUBUS_DEFEATED");

        public static LegacyKFlag<int> DRIDERINCUBUS_KILLED = new(1262, "DRIDERINCUBUS_KILLED");

        public static LegacyKFlag<int> MITZI_RECRUITED = new(1263, "MITZI_RECRUITED");

        public static LegacyKFlag<int> MITZI_FUCKED = new(1264, "MITZI_FUCKED");

        public static LegacyKFlag<int> DRIDERINCUBUS_FUCKED = new(1265, "DRIDERINCUBUS_FUCKED");

        public static LegacyKFlag<int> MINOTAURKING_DEFEATED = new(1266, "MINOTAURKING_DEFEATED");

        public static LegacyKFlag<int> MINOTAURKING_KILLED = new(1267, "MINOTAURKING_KILLED");

        public static LegacyKFlag<int> MINOTAURKING_FUCKED = new(1268, "MINOTAURKING_FUCKED");

        public static LegacyKFlag<int> MINOTAURKINGSSLUT_FUCKED = new(1269, "MINOTAURKINGSSLUT_FUCKED");

        public static LegacyKFlag<int> MINOTAURKINGS_TOUCH = new(1270, "MINOTAURKINGS_TOUCH");

        public static LegacyKFlag<int> MINOTAURKING_ALIVE = new(1271, "MINOTAURKING_ALIVE");

        public static LegacyKFlag<int> LETHICE_DEFEATED = new(1272, "LETHICE_DEFEATED");
        //public static LegacyKFlag<int> LAST_ATTACK_TYPE = new LegacyKFlag<int>(1273); //Obsolete, replaced by currAbilityUsed.abilityType
        public static LegacyKFlag<string> LETHICE_NAME = new(1274, "LETHICE_NAME");

        public static LegacyKFlag<int> LETHICE_FUCKED = new(1275, "LETHICE_FUCKED");

        public static LegacyKFlag<int> LETHICE_REDEEMED = new(1276, "LETHICE_REDEEMED");

        public static LegacyKFlag<int> LETHICE_KILLED = new(1277, "LETHICE_KILLED");

        public static LegacyKFlag<int> LETHICE_LEFT_ALIVE = new(1278, "LETHICE_LEFT_ALIVE");

        public static LegacyKFlag<int> GAME_END = new(1279, "GAME_END");
        // -------------------------
        // Unknown flags 1280 - 1294
        // -------------------------
        public static LegacyKFlag<int> OVIMAX_OVERDOSE = new(1295, "OVIMAX_OVERDOSE");

        public static LegacyKFlag<int> MINO_CHEF_TALKED_RED_RIVER_ROOT = new(1296, "MINO_CHEF_TALKED_RED_RIVER_ROOT");
        // public static LegacyKFlag<int> RATHAZUL_MIXOLOGY_XP = new LegacyKFlag<int>(1297); // No longer used
        //public static LegacyKFlag<int> DISABLE_QUICKLOAD_CONFIRM:int = new LegacyKFlag<int>(1298); // No longer used
        //public static LegacyKFlag<int> DISABLE_QUICKSAVE_CONFIRM:int = new LegacyKFlag<int>(1299); // No longer used
        public static LegacyKFlag<int> BENOIT_EYES_TALK_UNLOCKED = new(1300, "BENOIT_EYES_TALK_UNLOCKED"); // Player has talked to Benoit(e, "BENOIT_EYES_TALK_UNLOCKED") about basilisk eyes

        public static LegacyKFlag<int> BENOIT_BASIL_EYES_GRANTED = new(1301, "BENOIT_BASIL_EYES_GRANTED"); // Counter to keep track, how often you gained them

        public static LegacyKFlag<int> BENOIT_HAIRPIN_TALKED_TODAY = new(1302, "BENOIT_HAIRPIN_TALKED_TODAY"); // Talked to Benoit(e, "BENOIT_HAIRPIN_TALKED_TODAY") about the hairpin today

        public static LegacyKFlag<int> IMPS_PETRIFIED = new(1303, "IMPS_PETRIFIED"); // Total imps petrified

        public static LegacyKFlag<int> CAMP_WALL_STATUES = new(1304, "CAMP_WALL_STATUES"); // Total imp statues at camp wall

        public static LegacyKFlag<int> CODEX_ENTRY_BASILISKS = new(1305, "CODEX_ENTRY_BASILISKS");
        // -------------------------
        // Unknown flags 1306 - 1309
        // -------------------------
        public static LegacyKFlag<int> COCKATRICES_UNLOCKED = new(1310, "COCKATRICES_UNLOCKED"); // Are cockatrice encounters unlocked?

        public static LegacyKFlag<int> TIMES_ENCOUNTERED_COCKATRICES = new(1311, "TIMES_ENCOUNTERED_COCKATRICES"); // Times encountered cockatrices

        public static LegacyKFlag<int> CODEX_ENTRY_COCKATRICES = new(1312, "CODEX_ENTRY_COCKATRICES"); // Cockatrices codex entry unlocked
        // -------------------------
        // Unknown flags 1313 - 2000
        // -------------------------
        public static LegacyKFlag<int> TIMES_ORGASMED = new(2001, "TIMES_ORGASMED"); //The reason flags are moved down there is to provide room when CoC ever gets updated and flags gets added. This is to avoid screwing up saves.

        public static LegacyKFlag<int> CORRUPTED_MARAE_KILLED = new(2002, "CORRUPTED_MARAE_KILLED");

        public static LegacyKFlag<int> CAMP_BUILT_CABIN = new(2003, "CAMP_BUILT_CABIN");

        public static LegacyKFlag<int> CAMP_CABIN_PROGRESS = new(2004, "CAMP_CABIN_PROGRESS");

        public static LegacyKFlag<int> CAMP_CABIN_WOOD_RESOURCES = new(2005, "CAMP_CABIN_WOOD_RESOURCES");

        public static LegacyKFlag<int> CAMP_CABIN_FURNITURE_BED = new(2006, "CAMP_CABIN_FURNITURE_BED");

        public static LegacyKFlag<int> CAMP_CABIN_FURNITURE_NIGHTSTAND = new(2007, "CAMP_CABIN_FURNITURE_NIGHTSTAND");

        public static LegacyKFlag<int> CAMP_CABIN_FURNITURE_DRESSER = new(2008, "CAMP_CABIN_FURNITURE_DRESSER");

        public static LegacyKFlag<int> CAMP_CABIN_FURNITURE_TABLE = new(2009, "CAMP_CABIN_FURNITURE_TABLE");

        public static LegacyKFlag<int> CAMP_CABIN_FURNITURE_CHAIR1 = new(2010, "CAMP_CABIN_FURNITURE_CHAIR1");

        public static LegacyKFlag<int> CAMP_CABIN_FURNITURE_CHAIR2 = new(2011, "CAMP_CABIN_FURNITURE_CHAIR2");

        public static LegacyKFlag<int> CAMP_CABIN_FURNITURE_DESK = new(2012, "CAMP_CABIN_FURNITURE_DESK");

        public static LegacyKFlag<int> CAMP_CABIN_FURNITURE_DESKCHAIR = new(2013, "CAMP_CABIN_FURNITURE_DESKCHAIR");

        public static LegacyKFlag<int> CAMP_CABIN_FURNITURE_BOOKSHELF = new(2014, "CAMP_CABIN_FURNITURE_BOOKSHELF");

        public static LegacyKFlag<int> MET_MARAE = new(2015, "MET_MARAE");

        public static LegacyKFlag<int> MET_MARAE_CORRUPTED = new(2016, "MET_MARAE_CORRUPTED");

        public static LegacyKFlag<int> MARAE_QUEST_START = new(2017, "MARAE_QUEST_START");

        public static LegacyKFlag<int> MARAE_QUEST_COMPLETE = new(2018, "MARAE_QUEST_COMPLETE");

        public static LegacyKFlag<int> MARAE_LETHICITE = new(2019, "MARAE_LETHICITE"); //Not used anymore, may be reclaimed for later use.

        public static LegacyKFlag<int> FACTORY_FOUND = new(2020, "FACTORY_FOUND");

        public static LegacyKFlag<int> FACTORY_SUCCUBUS_DEFEATED = new(2021, "FACTORY_SUCCUBUS_DEFEATED");

        public static LegacyKFlag<int> FACTORY_INCUBUS_DEFEATED = new(2022, "FACTORY_INCUBUS_DEFEATED");

        public static LegacyKFlag<int> FACTORY_OMNIBUS_DEFEATED = new(2023, "FACTORY_OMNIBUS_DEFEATED");

        public static LegacyKFlag<int> FACTORY_SHUTDOWN = new(2024, "FACTORY_SHUTDOWN"); //1 if shutdown, 2 if overloaded.

        public static LegacyKFlag<int> FACTORY_INCUBUS_BRIBED = new(2025, "FACTORY_INCUBUS_BRIBED");

        public static LegacyKFlag<int> FACTORY_TAKEN_LACTAID = new(2026, "FACTORY_TAKEN_LACTAID");

        public static LegacyKFlag<int> FACTORY_TAKEN_GROPLUS = new(2027, "FACTORY_TAKEN_GROPLUS");

        public static LegacyKFlag<int> FACTORY_MILKER_BUILT = new(2028, "FACTORY_MILKER_BUILT");

        public static LegacyKFlag<int> KELT_KILL_PLAN = new(2029, "KELT_KILL_PLAN");

        public static LegacyKFlag<int> KELT_KILLED = new(2030, "KELT_KILLED");

        public static LegacyKFlag<int> WHITNEY_GONE = new(2031, "WHITNEY_GONE"); //If you murdered Whitney. You monster. Not yet implemented.

        public static LegacyKFlag<int> CODEX_CURRENT_ENTRY = new(2032, "CODEX_CURRENT_ENTRY");

        public static LegacyKFlag<int> CODEX_CURRENT_PAGE = new(2033, "CODEX_CURRENT_PAGE");

        public static LegacyKFlag<int> CODEX_UNUSED_FLAG_1 = new(2034, "CODEX_UNUSED_FLAG_1");

        public static LegacyKFlag<int> CODEX_UNUSED_FLAG_2 = new(2035, "CODEX_UNUSED_FLAG_2");

        public static LegacyKFlag<int> CODEX_ENTRY_ANEMONES = new(2036, "CODEX_ENTRY_ANEMONES");

        public static LegacyKFlag<int> CODEX_ENTRY_ARACHNES = new(2037, "CODEX_ENTRY_ARACHNES");

        public static LegacyKFlag<int> CODEX_ENTRY_FETISHFOLLOWERS = new(2038, "CODEX_ENTRY_FETISHFOLLOWERS");

        public static LegacyKFlag<int> CODEX_ENTRY_GIANTBEES = new(2039, "CODEX_ENTRY_GIANTBEES");

        public static LegacyKFlag<int> CODEX_ENTRY_GOBLINS = new(2040, "CODEX_ENTRY_GOBLINS");

        public static LegacyKFlag<int> CODEX_ENTRY_GOOGIRLS = new(2041, "CODEX_ENTRY_GOOGIRLS");

        public static LegacyKFlag<int> CODEX_ENTRY_HARPIES = new(2042, "CODEX_ENTRY_HARPIES");

        public static LegacyKFlag<int> CODEX_ENTRY_HELLHOUNDS = new(2043, "CODEX_ENTRY_HELLHOUNDS");

        public static LegacyKFlag<int> CODEX_ENTRY_IMPS = new(2044, "CODEX_ENTRY_IMPS");

        public static LegacyKFlag<int> CODEX_ENTRY_LABOVINES = new(2045, "CODEX_ENTRY_LABOVINES");

        public static LegacyKFlag<int> CODEX_ENTRY_LIZANS = new(2046, "CODEX_ENTRY_LIZANS");

        public static LegacyKFlag<int> CODEX_ENTRY_MAGIC = new(2047, "CODEX_ENTRY_MAGIC");

        public static LegacyKFlag<int> CODEX_ENTRY_MINOTAURS = new(2048, "CODEX_ENTRY_MINOTAURS");

        public static LegacyKFlag<int> CODEX_ENTRY_NAGAS = new(2049, "CODEX_ENTRY_NAGAS");

        public static LegacyKFlag<int> CODEX_ENTRY_ORCS = new(2050, "CODEX_ENTRY_ORCS");

        public static LegacyKFlag<int> CODEX_ENTRY_SALAMANDERS = new(2051, "CODEX_ENTRY_SALAMANDERS");

        public static LegacyKFlag<int> CODEX_ENTRY_SANDWITCHES = new(2052, "CODEX_ENTRY_SANDWITCHES");

        public static LegacyKFlag<int> CODEX_ENTRY_SATYRS = new(2053, "CODEX_ENTRY_SATYRS");

        public static LegacyKFlag<int> CODEX_ENTRY_SHARKGIRLS = new(2054, "CODEX_ENTRY_SHARKGIRLS");

        public static LegacyKFlag<int> CODEX_ENTRY_SUCCUBUS = new(2055, "CODEX_ENTRY_SUCCUBUS");

        public static LegacyKFlag<int> PIG_SLUT_DISABLED = new(2056, "PIG_SLUT_DISABLED");

        public static LegacyKFlag<int> MARBLE_CORRUPT_MARAE_STORY = new(2057, "MARBLE_CORRUPT_MARAE_STORY");

        public static LegacyKFlag<int> CAMP_PORTAL_PROGRESS = new(2058, "CAMP_PORTAL_PROGRESS"); //Will eventually allow you to travel back to Ingnam.

        public static LegacyKFlag<int> DISCOVERED_GLACIAL_RIFT = new(2059, "DISCOVERED_GLACIAL_RIFT");

        public static LegacyKFlag<int> DISCOVERED_VOLCANO_CRAG = new(2060, "DISCOVERED_VOLCANO_CRAG");

        public static LegacyKFlag<int> MET_YETI_FIRST_TIME = new(2061, "MET_YETI_FIRST_TIME");

        public static LegacyKFlag<int> VALERIA_FOUND_IN_GLACIAL_RIFT = new(2062, "VALERIA_FOUND_IN_GLACIAL_RIFT");

        public static LegacyKFlag<int> SHEILA_LETHICITE_FORGE_DAY = new(2063, "SHEILA_LETHICITE_FORGE_DAY"); //Scarred Blade forge progress.

        public static LegacyKFlag<int> CHEAT_ENTERING_COUNTER_2 = new(2064, "CHEAT_ENTERING_COUNTER_2"); //Debug menu

        public static LegacyKFlag<int> GIACOMO_MET = new(2065, "GIACOMO_MET");

        public static LegacyKFlag<int> MOD_SAVE_VERSION = new(2066, "MOD_SAVE_VERSION"); //Current version of save.

        public static LegacyKFlag<int> GIACOMO_NOTICES_WORMS = new(2067, "GIACOMO_NOTICES_WORMS");

        public static LegacyKFlag<int> KEPT_PURE_JOJO_OVER_VAPULA = new(2068, "KEPT_PURE_JOJO_OVER_VAPULA"); //Requires 90+ intelligence.

        public static LegacyKFlag<int> KEPT_PURE_AMILY_OVER_VAPULA = new(2069, "KEPT_PURE_AMILY_OVER_VAPULA"); //Requires 90+ intelligence.

        public static LegacyKFlag<int> PHOENIX_ENCOUNTERED = new(2070, "PHOENIX_ENCOUNTERED");

        public static LegacyKFlag<float> LUSTSTICK_LIBIDO_INITIAL = new(2071, "LUSTSTICK_LIBIDO_INITIAL");

        public static LegacyKFlag<int> LUSTSTICK_LIBIDO_MOD = new(2072, "LUSTSTICK_LIBIDO_MOD");

        public static LegacyKFlag<int> PHOENIX_WANKED_COUNTER = new(2073, "PHOENIX_WANKED_COUNTER");

        public static LegacyKFlag<int> TIMES_MASTURBATED = new(2074, "TIMES_MASTURBATED");

        public static LegacyKFlag<int> DEMONS_DEFEATED = new(2075, "DEMONS_DEFEATED");

        public static LegacyKFlag<int> AMILY_VILLAGE_EXPLORED = new(2076, "AMILY_VILLAGE_EXPLORED"); //Times explored TownRuins, counts for achievement.

        public static LegacyKFlag<int> TIMES_TRANSFORMED = new(2077, "TIMES_TRANSFORMED"); //Times transformed, counts for achievement.

        public static LegacyKFlag<int> CORRUPTED_GLADES_DESTROYED = new(2078, "CORRUPTED_GLADES_DESTROYED"); //Now reused. Formerly, this flag tracked whether omnibus was killed. Now this tracks progress until extinction of glades.

        public static LegacyKFlag<int> HERMOIT_NEXTDAY_EVENT = new(2079, "HERMOIT_NEXTDAY_EVENT");

        public static LegacyKFlag<int> HERMOIT_NEXTDAY_EVENT_DONE = new(2080, "HERMOIT_NEXTDAY_EVENT_DONE");

        public static LegacyKFlag<int> ACHIEVEMENT_PROGRESS_FASTING = new(2081, "ACHIEVEMENT_PROGRESS_FASTING");

        public static LegacyKFlag<int> ACHIEVEMENT_PROGRESS_GLUTTON = new(2082, "ACHIEVEMENT_PROGRESS_GLUTTON");

        public static LegacyKFlag<int> ACHIEVEMENT_PROGRESS_SCHIZOPHRENIA = new(2083, "ACHIEVEMENT_PROGRESS_SCHIZOPHRENIA");

        public static LegacyKFlag<int> ACHIEVEMENT_PROGRESS_CLEAN_SLATE = new(2084, "ACHIEVEMENT_PROGRESS_CLEAN_SLATE");

        public static LegacyKFlag<int> ACHIEVEMENT_PROGRESS_TOTAL_DAMAGE = new(2085, "ACHIEVEMENT_PROGRESS_TOTAL_DAMAGE");

        public static LegacyKFlag<int> CLEARED_HEL_TOWER = new(2086, "CLEARED_HEL_TOWER");

        public static LegacyKFlag<int> MINERVA_PURIFICATION_PROGRESS = new(2087, "MINERVA_PURIFICATION_PROGRESS"); //Purification progress. 10 denotes completion. -10 indicates corruption.

        public static LegacyKFlag<int> MINERVA_PURIFICATION_MARAE_TALKED = new(2088, "MINERVA_PURIFICATION_MARAE_TALKED"); //0 if not started, 1 if the condition is met, 2 is talked.

        public static LegacyKFlag<int> MINERVA_PURIFICATION_JOJO_TALKED = new(2089, "MINERVA_PURIFICATION_JOJO_TALKED"); //0 if not started, 1 if the condition is met, 2 is talked.

        public static LegacyKFlag<int> MINERVA_PURIFICATION_RATHAZUL_TALKED = new(2090, "MINERVA_PURIFICATION_RATHAZUL_TALKED"); //0 if not started, 1 if the condition is met, 2 is talked.

        public static LegacyKFlag<int> MINERVA_CHILDREN = new(2091, "MINERVA_CHILDREN"); //Children with Minerva!

        public static LegacyKFlag<int> MINERVA_CORRUPTION_PROGRESS = new(2092, "MINERVA_CORRUPTION_PROGRESS"); //Corruption progress. 10 denotes completion.

        public static LegacyKFlag<int> MINERVA_TOWER_TREE = new(2093, "MINERVA_TOWER_TREE"); //Set to 1 if seed is planted and tree grows.

        public static LegacyKFlag<int> MINERVA_TOWER_REPAIRED = new(2094, "MINERVA_TOWER_REPAIRED"); //Alters the part where you enter the tower. 1 for first time.

        public static LegacyKFlag<int> MINERVA_PREGNANCY_TYPE = new(2095, "MINERVA_PREGNANCY_TYPE");

        public static LegacyKFlag<int> MINERVA_PREGNANCY_INCUBATION = new(2096, "MINERVA_PREGNANCY_INCUBATION");

        public static LegacyKFlag<int> MINERVA_FOUND_LOST_SHARKGIRL = new(2097, "MINERVA_FOUND_LOST_SHARKGIRL"); //Possible mini-quest?

        public static LegacyKFlag<int> MINERVA_TELLS_ABOUT_PREGNANCY = new(2098, "MINERVA_TELLS_ABOUT_PREGNANCY"); //Increments to 1 when she gives birth for first time.

        public static LegacyKFlag<int> TIMES_BIRTHED_SHARPIES = new(2099, "TIMES_BIRTHED_SHARPIES");

        public static LegacyKFlag<int> ACHIEVEMENT_PROGRESS_DEFORESTER = new(2100, "ACHIEVEMENT_PROGRESS_DEFORESTER");

        public static LegacyKFlag<int> ACHIEVEMENT_PROGRESS_IM_NO_LUMBERJACK = new(2101, "ACHIEVEMENT_PROGRESS_IM_NO_LUMBERJACK");

        public static LegacyKFlag<int> ACHIEVEMENT_PROGRESS_HAMMER_TIME = new(2102, "ACHIEVEMENT_PROGRESS_HAMMER_TIME");

        public static LegacyKFlag<int> ACHIEVEMENT_PROGRESS_SCAVENGER = new(2103, "ACHIEVEMENT_PROGRESS_SCAVENGER");

        public static LegacyKFlag<int> TIMES_BAD_ENDED = new(2104, "TIMES_BAD_ENDED");

        public static LegacyKFlag<int> CONDOMED = new(2105, "CONDOMED"); //If you use condom

        public static LegacyKFlag<int> HELIA_HERMIFIED = new(2106, "HELIA_HERMIFIED"); //1 if Helia is made a hermaphrodite.

        public static LegacyKFlag<int> IMP_LORD_MALEHERM_PROGRESS = new(2107, "IMP_LORD_MALEHERM_PROGRESS"); //1 if started, 10 if finished.

        public static LegacyKFlag<int> EMBER_SPAR_VICTORIES = new(2108, "EMBER_SPAR_VICTORIES"); //Times won against Ember spars.

        public static LegacyKFlag<int> PHOENIX_HP_LOSS_COUNTER = new(2109, "PHOENIX_HP_LOSS_COUNTER"); //If you lose by HP 4 times in a row, bad end.

        public static LegacyKFlag<int> LIZAN_ROGUE_SEX_COUNTER = new(2110, "LIZAN_ROGUE_SEX_COUNTER");

        public static LegacyKFlag<int> BENOIT_PISTOL_BOUGHT = new(2111, "BENOIT_PISTOL_BOUGHT");
        //public static LegacyKFlag<int> RANGED_AMMO = new LegacyKFlag<int>(2112); //Never needed to be a flag in the first place
        public static LegacyKFlag<int> BENOIT_CLOCK_BOUGHT = new(2113, "BENOIT_CLOCK_BOUGHT");

        public static LegacyKFlag<int> BENOIT_CLOCK_ALARM = new(2114, "BENOIT_CLOCK_ALARM");

        public static LegacyKFlag<int> GOBLIN_ELDER_GRATEFUL_OVER_LETHICE_DEFEAT = new(2115, "GOBLIN_ELDER_GRATEFUL_OVER_LETHICE_DEFEAT");

        public static LegacyKFlag<int> MET_LIZAN_FIRST_TIME = new(2116, "MET_LIZAN_FIRST_TIME");

        public static LegacyKFlag<int> TIMES_ENCOUNTERED_GOBLIN_WARRIOR = new(2117, "TIMES_ENCOUNTERED_GOBLIN_WARRIOR");

        public static LegacyKFlag<int> TIMES_ENCOUNTERED_GOBLIN_SHAMAN = new(2118, "TIMES_ENCOUNTERED_GOBLIN_SHAMAN");

        public static LegacyKFlag<int> TIMES_ENCOUNTERED_GOBLIN_ELDER = new(2119, "TIMES_ENCOUNTERED_GOBLIN_ELDER");

        public static LegacyKFlag<int> TIMES_ENCOUNTERED_GOBLIN_QUEEN = new(2120, "TIMES_ENCOUNTERED_GOBLIN_QUEEN");
        //public static LegacyKFlag<int> TIMES_ENCOUNTERED_IMP_WARLORD:int = new LegacyKFlag<int>(2121); //deprecated
        //public static LegacyKFlag<int> TIMES_ENCOUNTERED_IMP_OVERLORD:int = new LegacyKFlag<int>(2122); //deprecated
        //public static LegacyKFlag<int> IN_INGNAM:int = new LegacyKFlag<int>(2123); //All Ingnam flags are deprecated
        //public static LegacyKFlag<int> INGNAM_PROLOGUE_COMPLETE:int = new LegacyKFlag<int>(2124);
        //public static LegacyKFlag<int> INGNAM_WEAPONSMITH_TALKED:int = new LegacyKFlag<int>(2125);
        //public static LegacyKFlag<int> INGNAM_ARMORSMITH_TALKED:int = new LegacyKFlag<int>(2126);
        //public static LegacyKFlag<int> INGNAM_RUMORS:int = new LegacyKFlag<int>(2127);
        //public static LegacyKFlag<int> INGNAM_FARMER_MET:int = new LegacyKFlag<int>(2128);
        //public static LegacyKFlag<int> INGNAM_BLACKMARKET_TALKED:int = new LegacyKFlag<int>(2129);
        //public static LegacyKFlag<int> INGNAM_ALCHEMIST_TALKED:int = new LegacyKFlag<int>(2130);
        //public static LegacyKFlag<int> UNKNOWN_FLAG_NUMBER_02131:int = new LegacyKFlag<int>(2131);
        //public static LegacyKFlag<int> INGNAM_EARS_FREAKOUT:int = new LegacyKFlag<int>(2132);
        //public static LegacyKFlag<int> INGNAM_EARS_LAST_TYPE:int = new LegacyKFlag<int>(2133);
        //public static LegacyKFlag<int> INGNAM_TAIL_FREAKOUT:int = new LegacyKFlag<int>(2134);
        //public static LegacyKFlag<int> INGNAM_TAIL_LAST_TYPE:int = new LegacyKFlag<int>(2135);
        public static LegacyKFlag<int> DISABLED_JOJO_RAPE = new(2136, "DISABLED_JOJO_RAPE"); //If 1, disables Jojo rape for good, locks out corrupt path permanently.

        public static LegacyKFlag<int> DISABLED_SEX_MACHINE = new(2137, "DISABLED_SEX_MACHINE");

        public static LegacyKFlag<int> ENEMY_CRITICAL = new(2138, "ENEMY_CRITICAL");

        public static LegacyKFlag<int> MARBLE_HAMMER_QUEST_PROGRESS = new(2139, "MARBLE_HAMMER_QUEST_PROGRESS");

        public static LegacyKFlag<int> SHIFT_KEY_DOWN = new(2140, "SHIFT_KEY_DOWN");
        // Prison flags no longer used.
        //	public static LegacyKFlag<int> IN_PRISON:int = new LegacyKFlag<int>(2141); //In prison. Replaces old flag 1001 in prisoner mod.
        //	public static LegacyKFlag<int> PRISON_CAPTURE_CHANCE:int = new LegacyKFlag<int>(2142); //Captor chance. Replaces old flag 1000 in prisoner mod.
        //	public static LegacyKFlag<int> PRISON_TRAINING_LEVEL:int = new LegacyKFlag<int>(2143); //Training level. Replaces old flag 1002 in prisoner mod.
        //	public static LegacyKFlag<int> PRISON_TRAINING_REFUSED:int = new LegacyKFlag<int>(2144); //Training refused. Replaces old flag 1003 in prisoner mod.
        //	public static LegacyKFlag<int> PRISON_DIRT_ENABLED:int = new LegacyKFlag<int>(2145);
        //	public static LegacyKFlag<int> PRISON_PUNISHMENT:int = new LegacyKFlag<int>(2146); //0: None, 1: Stockade, 2: Solitary Confinement, 3: BJ Trainer
        //	public static LegacyKFlag<int> PRISON_DOOR_UNLOCKED:int = new LegacyKFlag<int>(2147);
        //	public static LegacyKFlag<int> PRISON_CAPTURE_COUNTER:int = new LegacyKFlag<int>(2148);
        //	public static LegacyKFlag<int> PRISON_DILDO_RACK:int = new LegacyKFlag<int>(2149);
        //	public static LegacyKFlag<int> PRISON_TRAIN_SELF_CONTROL_UNLOCKED:int = new LegacyKFlag<int>(2150);
        //	public static LegacyKFlag<int> PRISON_TRAIN_ANAL_CAPACITY_UNLOCKED:int = new LegacyKFlag<int>(2151);
        //	public static LegacyKFlag<int> PRISON_TRAIN_PUPPY_TRICKS_UNLOCKED:int = new LegacyKFlag<int>(2152);
        //	public static LegacyKFlag<int> PRISON_STUDY_MANNERS_UNLOCKED:int = new LegacyKFlag<int>(2153);
        //	public static LegacyKFlag<int> PRISON_STUDY_BREATHING_UNLOCKED:int = new LegacyKFlag<int>(2154);
        //	public static LegacyKFlag<int> PRISON_BITCHED_ABOUT_MARBLE_MILK:int = new LegacyKFlag<int>(2155);
        //	public static LegacyKFlag<int> PRISON_EVENT_TIMEOUT:int = new LegacyKFlag<int>(2156);
        //	public static LegacyKFlag<int> PRISON_STORAGE_ARMOR:int = new LegacyKFlag<int>(2157);
        //	public static LegacyKFlag<int> PRISON_STORAGE_WEAPON:int = new LegacyKFlag<int>(2158);
        //	public static LegacyKFlag<int> PRISON_STORAGE_SHIELD:int = new LegacyKFlag<int>(2159);
        // public static LegacyKFlag<int> MET_BEHEMOTH:int = new LegacyKFlag<int>(2160); //Gone now
        // public static LegacyKFlag<int> BEHEMOTH_ANAL_PITCH:int = new LegacyKFlag<int>(2161); //Gone now
        // public static LegacyKFlag<int> BEHEMOTH_ANAL_CATCH:int = new LegacyKFlag<int>(2162); //Gone now
        // public static LegacyKFlag<int> BEHEMOTH_VAGINAL_CATCH:int = new LegacyKFlag<int>(2163); //Gone now
        // public static LegacyKFlag<int> BEHEMOTH_COCK_SUCKED:int = new LegacyKFlag<int>(2164); //Gone now
        // public static LegacyKFlag<int> BEHEMOTH_CUM_BATH:int = new LegacyKFlag<int>(2165); //Gone now
        // public static LegacyKFlag<int> BEHEMOTH_WATERSPORTS:int = new LegacyKFlag<int>(2166); //Gone now
        // public static LegacyKFlag<int> BEHEMOTH_GEMS:int = new LegacyKFlag<int>(2167); //Gone now
        // public static LegacyKFlag<int> BEHEMOTH_TALK_LEVEL:int = new LegacyKFlag<int>(2168); //Gone now
        // public static LegacyKFlag<int> BEHEMOTH_CHILDREN:int = new LegacyKFlag<int>(2169); //Gone now
        // public static LegacyKFlag<int> BEHEMOTH_TENT:int = new LegacyKFlag<int>(2170); //Gone now
        // public static LegacyKFlag<int> BEHEMOTH_TALKED_ABOUT_CUM:int = new LegacyKFlag<int>(2171); //Gone now
        public static LegacyKFlag<int> THIEF_GEMS = new(2172, "THIEF_GEMS");

        public static LegacyKFlag<int> INGNAM_GREETED_AFTER_LONGTIME = new(2173, "INGNAM_GREETED_AFTER_LONGTIME");
        // public static LegacyKFlag<int> CODEX_ENTRY_BEHEMOTH:int = new LegacyKFlag<int>(2174); //Gone now
        public static LegacyKFlag<int> SCARRED_BLADE_STATUS = new(2175, "SCARRED_BLADE_STATUS");

        public static LegacyKFlag<int> PURE_MARAE_ENDGAME = new(2176, "PURE_MARAE_ENDGAME"); //1 if met, 2 if defeated.

        public static LegacyKFlag<int> LETHICITE_ARMOR_TAKEN = new(2177, "LETHICITE_ARMOR_TAKEN"); //1 if lethicite armor is taken.

        public static LegacyKFlag<int> CAMP_WALL_PROGRESS = new(2178, "CAMP_WALL_PROGRESS"); //100 if complete.

        public static LegacyKFlag<int> CAMP_WALL_GATE = new(2179, "CAMP_WALL_GATE"); //1 if built.

        public static LegacyKFlag<int> CAMP_WALL_SKULLS = new(2180, "CAMP_WALL_SKULLS"); //Number of Imp Skulls hung.
        // public static LegacyKFlag<int> BEHEMOTH_CHILD_1_BIRTH_DAY:int = new LegacyKFlag<int>(2181); //Gone now
        // public static LegacyKFlag<int> BEHEMOTH_CHILD_2_BIRTH_DAY:int = new LegacyKFlag<int>(2182); //Gone now
        // public static LegacyKFlag<int> BEHEMOTH_CHILD_3_BIRTH_DAY:int = new LegacyKFlag<int>(2183); //Gone now
        public static LegacyKFlag<int> JOJO_SEX_COUNTER = new(2184, "JOJO_SEX_COUNTER");

        public static LegacyKFlag<int> JOJO_ANAL_XP = new(2185, "JOJO_ANAL_XP"); //Also used for Bimbo Jojo.

        public static LegacyKFlag<int> JOJO_LITTERS = new(2186, "JOJO_LITTERS"); //Litters of mouse-morphs birthed. Will also be used to track litters by Joy.

        public static LegacyKFlag<int> JOJO_BLOWJOB_XP = new(2187, "JOJO_BLOWJOB_XP"); //How many times you've sucked Jojo's cock.

        public static LegacyKFlag<int> JOJO_VAGINAL_CATCH_COUNTER = new(2188, "JOJO_VAGINAL_CATCH_COUNTER"); //How many times Jojo has penetrated you vaginally.

        public static LegacyKFlag<int> JOJO_LITTERS_AMILY_REACTION_COUNTER = new(2189, "JOJO_LITTERS_AMILY_REACTION_COUNTER");

        public static LegacyKFlag<int> JOJO_ANAL_CATCH_COUNTER = new(2190, "JOJO_ANAL_CATCH_COUNTER"); //How many times Jojo has penetrated you anally.

        public static LegacyKFlag<int> KIHA_UNDERGARMENTS = new(2191, "KIHA_UNDERGARMENTS"); //0 if nude, 1 if wearing panties, 2 if wearing loincloth. (Spider-silk only, "KIHA_UNDERGARMENTS")

        public static LegacyKFlag<int> KIHA_PREGNANCY_TYPE = new(2192, "KIHA_PREGNANCY_TYPE");

        public static LegacyKFlag<int> KIHA_INCUBATION = new(2193, "KIHA_INCUBATION");
        // public static LegacyKFlag<int> UNKNOWN_FLAG_NUMBER_02194 = new LegacyKFlag<int>(2194);
        public static LegacyKFlag<int> KIHA_CHILDREN_BOYS = new(2195, "KIHA_CHILDREN_BOYS");

        public static LegacyKFlag<int> KIHA_CHILDREN_GIRLS = new(2196, "KIHA_CHILDREN_GIRLS");

        public static LegacyKFlag<int> KIHA_CHILDREN_HERMS = new(2197, "KIHA_CHILDREN_HERMS");

        public static LegacyKFlag<int> KIHA_CHILD_MATURITY_COUNTER = new(2198, "KIHA_CHILD_MATURITY_COUNTER");

        public static LegacyKFlag<int> KIHA_PREGNANCY_POTENTIAL = new(2199, "KIHA_PREGNANCY_POTENTIAL");

        public static LegacyKFlag<int> VALERIA_FLUIDS = new(2200, "VALERIA_FLUIDS"); //Enabled in New Game+. 100 is full.

        public static LegacyKFlag<int> IZMA_FEEDING_VALERIA = new(2201, "IZMA_FEEDING_VALERIA");

        public static LegacyKFlag<int> HELLHOUNDS_KILLED = new(2202, "HELLHOUNDS_KILLED"); //Flag used to be pigtail truffle bad end warning. Has been replaced.

        public static LegacyKFlag<int> IMPS_KILLED = new(2203, "IMPS_KILLED");

        public static LegacyKFlag<int> GOBLINS_KILLED = new(2204, "GOBLINS_KILLED");

        public static LegacyKFlag<int> MINOTAURS_KILLED = new(2205, "MINOTAURS_KILLED");

        public static LegacyKFlag<int> TIMES_SLEPT_WITH_EMBER = new(2206, "TIMES_SLEPT_WITH_EMBER");

        public static LegacyKFlag<int> EMBER_MORNING = new(2207, "EMBER_MORNING");

        public static LegacyKFlag<int> BASILISK_RESISTANCE_TRACKER = new(2208, "BASILISK_RESISTANCE_TRACKER");

        public static LegacyKFlag<float> MINOTAUR_CUM_RESISTANCE_TRACKER = new(2209, "MINOTAUR_CUM_RESISTANCE_TRACKER");

        public static LegacyKFlag<int> DLC_APRIL_FOOLS = new(2210, "DLC_APRIL_FOOLS");

        public static LegacyKFlag<int> GOBLIN_ELDER_TALK_COUNTER = new(2211, "GOBLIN_ELDER_TALK_COUNTER");

        public static LegacyKFlag<int> ISABELLA_POTENCY_STATE = new(2212, "ISABELLA_POTENCY_STATE");

        public static LegacyKFlag<float> ISABELLA_COWMOTHER = new(2213, "ISABELLA_COWMOTHER"); //0.5 means prompt happened. 1 indicates she can give birth to cowgirls.

        public static LegacyKFlag<int> ISABELLA_PREGNANCY_TYPE = new(2214, "ISABELLA_PREGNANCY_TYPE");

        public static LegacyKFlag<int> ISABELLA_PREGNANCY_INCUBATION = new(2215, "ISABELLA_PREGNANCY_INCUBATION");

        public static LegacyKFlag<int> ISABELLA_PREGNANCY_BOOSTED = new(2216, "ISABELLA_PREGNANCY_BOOSTED"); //1 indicates her pregnancy is boosted by Ovi Elixir. Resets to 0 when she gives birth.

        public static LegacyKFlag<int> ISABELLA_CHILDREN_HUMANS = new(2217, "ISABELLA_CHILDREN_HUMANS");

        public static LegacyKFlag<int> ISABELLA_CHILDREN_COWGIRLS = new(2218, "ISABELLA_CHILDREN_COWGIRLS");

        public static LegacyKFlag<int> ISABELLA_COUNTDOWN_TO_CONTRACEPTIONS = new(2219, "ISABELLA_COUNTDOWN_TO_CONTRACEPTIONS"); //If Isabella joins your camp, set to 240. -1 means done.

        public static LegacyKFlag<int> ISABELLA_VALERIA_SPARRED = new(2220, "ISABELLA_VALERIA_SPARRED");

        public static LegacyKFlag<int> FAERIE_ENCOUNTER_DISABLED = new(2221, "FAERIE_ENCOUNTER_DISABLED");

        public static LegacyKFlag<int> MINERVA_CORRUPTED_CUM_ADDICTION = new(2222, "MINERVA_CORRUPTED_CUM_ADDICTION"); //This will be used for Corrupted Minerva.

        public static LegacyKFlag<int> ERLKING_CANE_OBTAINED = new(2223, "ERLKING_CANE_OBTAINED");
        //public static LegacyKFlag<int> ERLKING_CANE_ATTACK_COUNTER:int = new LegacyKFlag<int>(2224); //No longer used
        public static LegacyKFlag<int> KIHA_DESTROYING_CORRUPTED_GLADES = new(2225, "KIHA_DESTROYING_CORRUPTED_GLADES"); //1 if Kiha is currently destroying corrupted glades.

        public static LegacyKFlag<int> AMILY_DESTROYING_CORRUPTED_GLADES = new(2226, "AMILY_DESTROYING_CORRUPTED_GLADES"); //1 if Amily is currently destroying corrupted glades.
        //public static LegacyKFlag<int> BIMBO_LIQUEUR_STASH_COUNTER_FOR_JOJO:int = new LegacyKFlag<int>(2227); //All Joy flags are deprecated
        //public static LegacyKFlag<int> JOJO_BIMBO_STATE:int = new LegacyKFlag<int>(2228);
        //public static LegacyKFlag<int> JOY_COCK_SIZE:int = new LegacyKFlag<int>(2229);
        //public static LegacyKFlag<int> JOY_BREAST_SIZE:int = new LegacyKFlag<int>(2230);
        //public static LegacyKFlag<int> JOY_VAGINAL_WETNESS:int = new LegacyKFlag<int>(2231);
        //public static LegacyKFlag<int> JOY_LACTAID_MILKED_COUNTER:int = new LegacyKFlag<int>(2232);
        //public static LegacyKFlag<int> JOY_intELLIGENCE:int = new LegacyKFlag<int>(2233);
        //public static LegacyKFlag<int> TIMES_PENETRATED_JOY_VAGINALLY:int = new LegacyKFlag<int>(2234);
        //public static LegacyKFlag<int> TIMES_LICKED_JOYS_PUSSY:int = new LegacyKFlag<int>(2235);
        //public static LegacyKFlag<int> TIMES_FROTTED_WITH_JOY:int = new LegacyKFlag<int>(2236);
        //public static LegacyKFlag<int> TIMES_GET_BLOWN_BY_JOY:int = new LegacyKFlag<int>(2237);
        //public static LegacyKFlag<int> TIMES_GET_LICKED_BY_JOY:int = new LegacyKFlag<int>(2238);
        //public static LegacyKFlag<int> JOY_TALKED_ABOUT_YOURSELF:int = new LegacyKFlag<int>(2239);
        //public static LegacyKFlag<int> JOY_TALKED_ABOUT_HERSELF:int = new LegacyKFlag<int>(2240);
        //public static LegacyKFlag<int> JOY_TALKED_ABOUT_OLD_LIFE:int = new LegacyKFlag<int>(2241);
        //public static LegacyKFlag<int> JOY_TALKED_ABOUT_DEMONS:int = new LegacyKFlag<int>(2242);
        //public static LegacyKFlag<int> JOY_TALKED_ABOUT_BABIES:int = new LegacyKFlag<int>(2243);
        //public static LegacyKFlag<int> JOY_COCK_FONDNESS:int = new LegacyKFlag<int>(2244);
        //public static LegacyKFlag<int> JOY_EATEN_BLUE_EGG:int = new LegacyKFlag<int>(2245);
        //public static LegacyKFlag<int> JOY_PREGNANCY_TYPE:int = new LegacyKFlag<int>(2246);
        //public static LegacyKFlag<int> JOY_PREGNANCY_INCUBATION:int = new LegacyKFlag<int>(2247);
        //public static LegacyKFlag<int> JOY_TWINS_BIRTHED:int = new LegacyKFlag<int>(2248);
        //public static LegacyKFlag<int> JOY_NIGHT_FUCK:int = new LegacyKFlag<int>(2249);
        //public static LegacyKFlag<int> JOY_TAKES_BABIES_AWAY_COUNTER:int = new LegacyKFlag<int>(2250);
        //public static LegacyKFlag<int> BLACK_COCK_MET_ANITA:int = new LegacyKFlag<int>(2251); //All Black Cock flags are deprecated
        //public static LegacyKFlag<int> BLACK_COCK_MET_ANDY:int = new LegacyKFlag<int>(2252);
        //public static LegacyKFlag<int> BLACK_COCK_MET_HARRY:int = new LegacyKFlag<int>(2253);
        //public static LegacyKFlag<int> BLACK_COCK_MET_FRIDA:int = new LegacyKFlag<int>(2254);
        //public static LegacyKFlag<int> BLACK_COCK_ANITA_SONGS_LISTENED:int = new LegacyKFlag<int>(2255);
        //public static LegacyKFlag<int> BLACK_COCK_ANITA_SEX_intRO:int = new LegacyKFlag<int>(2256);
        //public static LegacyKFlag<int> BLACK_COCK_ANITA_SEX_COUNTER:int = new LegacyKFlag<int>(2257);
        //public static LegacyKFlag<int> BLACK_COCK_ANDY_STASH_STATE:int = new LegacyKFlag<int>(2258);
        //public static LegacyKFlag<int> BLACK_COCK_ANDY_ORAL_GIVEN:int = new LegacyKFlag<int>(2259);
        //public static LegacyKFlag<int> BLACK_COCK_ANDY_SEX_COUNTER:int = new LegacyKFlag<int>(2260);
        //public static LegacyKFlag<int> BLACK_COCK_HARRY_SEX_intRO:int = new LegacyKFlag<int>(2261);
        //public static LegacyKFlag<int> BLACK_COCK_HARRY_SEX_COUNTER:int = new LegacyKFlag<int>(2262);
        //public static LegacyKFlag<int> BLACK_COCK_FRIDAS_CAKE_EATEN_COUNTER:int = new LegacyKFlag<int>(2263);
        //public static LegacyKFlag<int> CODEX_ENTRY_RHINOS:int = new LegacyKFlag<int>(2264);
        //public static LegacyKFlag<int> CODEX_ENTRY_ECHIDNAS:int = new LegacyKFlag<int>(2265);
        public static LegacyKFlag<int> URTA_VIXEN_AND_CREAM_COUNTER = new(2266, "URTA_VIXEN_AND_CREAM_COUNTER");

        public static LegacyKFlag<int> CAMP_CABIN_STONE_RESOURCES = new(2267, "CAMP_CABIN_STONE_RESOURCES");

        public static LegacyKFlag<int> ACHIEVEMENT_PROGRESS_YABBA_DABBA_DOO = new(2268, "ACHIEVEMENT_PROGRESS_YABBA_DABBA_DOO");

        public static LegacyKFlag<int> ACHIEVEMENT_PROGRESS_ANTWORKS = new(2269, "ACHIEVEMENT_PROGRESS_ANTWORKS");

        public static LegacyKFlag<int> CODEX_ENTRY_ZEBRAS = new(2270, "CODEX_ENTRY_ZEBRAS");

        public static LegacyKFlag<int> ACHIEVEMENT_PROGRESS_EGG_HUNTER = new(2271, "ACHIEVEMENT_PROGRESS_EGG_HUNTER");

        public static LegacyKFlag<int> CURRENT_WEATHER = new(2272, "CURRENT_WEATHER");

        public static LegacyKFlag<float> WEATHER_CHANGE_COOLDOWN = new(2273, "WEATHER_CHANGE_COOLDOWN"); //4 hours to prevent weather from changing too fast.

        public static LegacyKFlag<int> TAMANI_DEFEAT_COUNTER = new(2274, "TAMANI_DEFEAT_COUNTER"); //If this reaches 4, you can bad-end Tamani.

        public static LegacyKFlag<float> TAMANI_BAD_ENDED = new(2275, "TAMANI_BAD_ENDED"); //Determines if Tamani is bad-ended. (Killed, "TAMANI_BAD_ENDED")

        public static LegacyKFlag<int> PHYLLA_STONES_HUNTED_TODAY = new(2276, "PHYLLA_STONES_HUNTED_TODAY");
        //public static LegacyKFlag<int> ANZU_PALACE_UNLOCKED:int = new LegacyKFlag<int>(2277); //Anzu flags are deprecated
        //public static LegacyKFlag<int> ANZU_RELATIONSHIP_LEVEL:int = new LegacyKFlag<int>(2278);
        //public static LegacyKFlag<int> ANZU_AFFECTION:int = new LegacyKFlag<int>(2279);
        //public static LegacyKFlag<int> ANZU_TIMES_TALKED:int = new LegacyKFlag<int>(2280);
        //public static LegacyKFlag<int> ANZU_TALKED_PAST:int = new LegacyKFlag<int>(2281);
        //public static LegacyKFlag<int> ANZU_TALKED_PALACE:int = new LegacyKFlag<int>(2282);
        //public static LegacyKFlag<int> ANZU_TALKED_RIFT:int = new LegacyKFlag<int>(2283);
        //public static LegacyKFlag<int> ANZU_TALKED_POWERS:int = new LegacyKFlag<int>(2284);
        //public static LegacyKFlag<int> ANZU_TALKED_GODS:int = new LegacyKFlag<int>(2285);
        //public static LegacyKFlag<int> ANZU_TIMES_DINED_BREAKFAST:int = new LegacyKFlag<int>(2286);
        //public static LegacyKFlag<int> ANZU_TIMES_DINED_LUNCH:int = new LegacyKFlag<int>(2287);
        //public static LegacyKFlag<int> ANZU_TIMES_DINED_DINNER:int = new LegacyKFlag<int>(2288);
        //public static LegacyKFlag<int> ANZU_TIMES_DRUNK_SEX:int = new LegacyKFlag<int>(2289);
        //public static LegacyKFlag<int> ANZU_TIMES_RACED:int = new LegacyKFlag<int>(2290);
        //public static LegacyKFlag<int> ANZU_ANAL_CATCH_COUNTER:int = new LegacyKFlag<int>(2291);
        //public static LegacyKFlag<int> ANZU_VAGINAL_CATCH_COUNTER:int = new LegacyKFlag<int>(2292);
        //public static LegacyKFlag<int> ANZU_BLOWN_YOU_COUNTER:int = new LegacyKFlag<int>(2293);
        //public static LegacyKFlag<int> ANZU_SUCKED_OFF_COUNTER:int = new LegacyKFlag<int>(2294);
        //public static LegacyKFlag<int> ANZU_ANAL_PITCH_COUNTER:int = new LegacyKFlag<int>(2295);
        //public static LegacyKFlag<int> ANZU_FED_COUNTER:int = new LegacyKFlag<int>(2296);
        public static LegacyKFlag<int> TIMES_EXPLORED = new(2297, "TIMES_EXPLORED"); // Replaces `player.explored`

        public static LegacyKFlag<int> TIMES_EXPLORED_FOREST = new(2298, "TIMES_EXPLORED_FOREST"); // Replaces `player.exploredForest`

        public static LegacyKFlag<int> TIMES_EXPLORED_DESERT = new(2299, "TIMES_EXPLORED_DESERT"); // Replaces `player.exploredDesert`

        public static LegacyKFlag<int> TIMES_EXPLORED_MOUNTAIN = new(2300, "TIMES_EXPLORED_MOUNTAIN"); // Replaces `player.exploredMountain`

        public static LegacyKFlag<int> TIMES_EXPLORED_LAKE = new(2301, "TIMES_EXPLORED_LAKE"); // Replaces `player.exploredLake`

        public static LegacyKFlag<int> HELIA_SPAR_VICTORIES = new(2302, "HELIA_SPAR_VICTORIES");

        public static LegacyKFlag<int> HELSPAWN_SPAR_VICTORIES = new(2303, "HELSPAWN_SPAR_VICTORIES");

        public static LegacyKFlag<int> KIHA_SPAR_VICTORIES = new(2304, "KIHA_SPAR_VICTORIES");

        public static LegacyKFlag<int> ISABELLA_SPAR_VICTORIES = new(2305, "ISABELLA_SPAR_VICTORIES");

        public static LegacyKFlag<int> MINERVA_SPAR_VICTORIES = new(2306, "MINERVA_SPAR_VICTORIES");

        public static LegacyKFlag<int> VALERIA_SPAR_VICTORIES = new(2307, "VALERIA_SPAR_VICTORIES");

        public static LegacyKFlag<int> AIKO_SPAR_VICTORIES = new(2308, "AIKO_SPAR_VICTORIES");
        // public static LegacyKFlag<int> UNKNOWN_FLAG_NUMBER_02309 = new LegacyKFlag<int>(2309); // Reserved for additional sparrable partner.
        public static LegacyKFlag<int> SANDWITCH_SERVICED = new(2310, "SANDWITCH_SERVICED"); // Number of times, the player has serviced a sand witch.

        public static LegacyKFlag<int> JOJO_STATUS = new(2311, "JOJO_STATUS"); // Replacement for `CoC.monk` variable.
        //public static LegacyKFlag<int> IZMA_BROFIED:int = new LegacyKFlag<int>(2312); //All Izmael flags are deprecated
        //public static LegacyKFlag<int> IZMAEL_ANAL_PENETRATED:int = new LegacyKFlag<int>(2313);
        //public static LegacyKFlag<int> IZMAEL_FRENZIED:int = new LegacyKFlag<int>(2314);
        //public static LegacyKFlag<int> IZMAEL_ENTRAPPED:int = new LegacyKFlag<int>(2315);
        //public static LegacyKFlag<int> IZMAEL_BATHTIME_COUNTER:int = new LegacyKFlag<int>(2316);
        // -------------------------
        // Unknown flags 2316 - 2317
        // -------------------------
        public static LegacyKFlag<int> TOWN_RUINS_SCAVENGE_CONSIDERED = new(2319, "TOWN_RUINS_SCAVENGE_CONSIDERED");

        public static LegacyKFlag<int> TOWN_RUINS_HOUSES_DISMANTLED = new(2320, "TOWN_RUINS_HOUSES_DISMANTLED");

        public static LegacyKFlag<int> TOWN_RUINS_HOUSE_SCAVENGE_PROGRESS = new(2321, "TOWN_RUINS_HOUSE_SCAVENGE_PROGRESS");

        public static LegacyKFlag<int> TOWN_RUINS_TOWNHALL_SCAVENGE_PROGRESS = new(2322, "TOWN_RUINS_TOWNHALL_SCAVENGE_PROGRESS");

        public static LegacyKFlag<int> TOWN_RUINS_SHOP_SCAVENGE_PROGRESS = new(2323, "TOWN_RUINS_SHOP_SCAVENGE_PROGRESS");

        public static LegacyKFlag<int> TOWN_RUINS_HOUSE_FURNITURE_FOUND = new(2324, "TOWN_RUINS_HOUSE_FURNITURE_FOUND");

        public static LegacyKFlag<int> TOWN_RUINS_TOWNHALL_FURNITURE_FOUND = new(2325, "TOWN_RUINS_TOWNHALL_FURNITURE_FOUND");

        public static LegacyKFlag<int> TOWN_RUINS_SHOP_FURNITURE_FOUND = new(2326, "TOWN_RUINS_SHOP_FURNITURE_FOUND");

        public static LegacyKFlag<int> PABLO_MET = new(2327, "PABLO_MET");

        public static LegacyKFlag<int> PABLO_AFFECTION = new(2328, "PABLO_AFFECTION");

        public static LegacyKFlag<int> PABLO_WORKOUT_COUNTER = new(2329, "PABLO_WORKOUT_COUNTER");

        public static LegacyKFlag<int> PABLO_SECRET_LEARNED = new(2330, "PABLO_SECRET_LEARNED");

        public static LegacyKFlag<int> PABLO_SEX_UNLOCKED = new(2331, "PABLO_SEX_UNLOCKED");

        public static LegacyKFlag<int> PABLO_GOT_DICKED_VAGINALLY = new(2332, "PABLO_GOT_DICKED_VAGINALLY");

        public static LegacyKFlag<int> PABLO_GOT_DICKED_ANALLY = new(2333, "PABLO_GOT_DICKED_ANALLY");

        public static LegacyKFlag<int> PABLO_USED_YOUR_PUSSY = new(2334, "PABLO_USED_YOUR_PUSSY");

        public static LegacyKFlag<int> PABLO_USED_YOUR_ASS = new(2335, "PABLO_USED_YOUR_ASS");

        public static LegacyKFlag<int> PABLO_FREAKED_OUT_OVER_WORMS = new(2336, "PABLO_FREAKED_OUT_OVER_WORMS");

        public static LegacyKFlag<int> TIMES_MINO_MUTUAL = new(2337, "TIMES_MINO_MUTUAL"); //How many times the Minotaur's "minoMutual" scene been accessed
        //public static LegacyKFlag<int> BIMBO_MINISKIRT_PROGRESS_DISABLED:int = new LegacyKFlag<int>(2338); //No longer used
        public static LegacyKFlag<int> AMAROK_LOSSES = new(2339, "AMAROK_LOSSES"); //Counts losses to Amarok. 5 = bad end

        public static LegacyKFlag<int> MRAPIER_BOUGHT = new(2340, "MRAPIER_BOUGHT"); //You only get one due to its OPness (no longer used; it's not like having two of the same weapon actually has any benefit, "MRAPIER_BOUGHT")

        public static LegacyKFlag<int> GOTTEN_INQUISITOR_CORSET = new(2341, "GOTTEN_INQUISITOR_CORSET");

        public static LegacyKFlag<int> GOTTEN_INQUISITOR_ROBES = new(2342, "GOTTEN_INQUISITOR_ROBES");

        public static LegacyKFlag<int> GOBLIN_ELDER_FOREPLAY_COUNTER = new(2343, "GOBLIN_ELDER_FOREPLAY_COUNTER");

        public static LegacyKFlag<int> GOBLIN_ELDER_RELATION_COUNTER = new(2344, "GOBLIN_ELDER_RELATION_COUNTER");

        public static LegacyKFlag<int> GOBLIN_ELDER_WRESTLE_COUNTER = new(2345, "GOBLIN_ELDER_WRESTLE_COUNTER");

        public static LegacyKFlag<int> GOBLIN_ELDER_BEAST_COUNTER = new(2346, "GOBLIN_ELDER_BEAST_COUNTER");

        public static LegacyKFlag<int> WEAPON_SHOP_UPGRADE_ITEM = new(2347, "WEAPON_SHOP_UPGRADE_ITEM");

        public static LegacyKFlag<int> WEAPON_SHOP_UPGRADE_TIME = new(2348, "WEAPON_SHOP_UPGRADE_TIME");

        public static LegacyKFlag<int> ARMOR_SHOP_UPGRADE_ITEM = new(2349, "ARMOR_SHOP_UPGRADE_ITEM");

        public static LegacyKFlag<int> ARMOR_SHOP_UPGRADE_TIME = new(2350, "ARMOR_SHOP_UPGRADE_TIME");

        public static LegacyKFlag<int> CHILL_SMITH_UPGRADE_ITEM = new(2351, "CHILL_SMITH_UPGRADE_ITEM");

        public static LegacyKFlag<int> CHILL_SMITH_UPGRADE_TIME = new(2352, "CHILL_SMITH_UPGRADE_TIME");

        public static LegacyKFlag<int> AIKO_TIMES_MET = new(2353, "AIKO_TIMES_MET");	//1=first, 2=second 3=third, 4+=fourth

        public static LegacyKFlag<int> AIKO_CORRUPTION = new(2354, "AIKO_CORRUPTION");	//corruption score /100
        //public static LegacyKFlag<int> AIKO_CORRUPTION_ACTIVE:int = new LegacyKFlag<int>(2355);	//Deprecated (her corruption is no longer toggleable)
        public static LegacyKFlag<int> AIKO_AFFECTION = new(2356, "AIKO_AFFECTION");	//affection score  /100

        public static LegacyKFlag<int> AIKO_SEXED = new(2357, "AIKO_SEXED");	//times sexed Aiko

        public static LegacyKFlag<int> AIKO_BALL_RETURNED = new(2358, "AIKO_BALL_RETURNED");	//1=returned ball 2=didn't return ball (lock out Aiko encounters, "AIKO_BALL_RETURNED")

        public static LegacyKFlag<int> AIKO_FIRST_CHOICE = new(2359, "AIKO_FIRST_CHOICE");	//1=riches, 2=power, 3=sex, 4=nothing, 5=fight, 6=touchFluffTail

        public static LegacyKFlag<int> AIKO_FIRST_SEX_COOLDOWN = new(2360, "AIKO_FIRST_SEX_COOLDOWN");	//cooldown period after first time sex prank

        public static LegacyKFlag<int> AIKO_HAS_FOUGHT = new(2361, "AIKO_HAS_FOUGHT");	//has fought Aiko about ball

        public static LegacyKFlag<int> AIKO_FIGHT_WON = new(2362, "AIKO_FIGHT_WON");	//1=won, 2=lost fight about ball

        public static LegacyKFlag<int> AIKO_HOT_BLOOD = new(2363, "AIKO_HOT_BLOOD");	//once off special encounter

        public static LegacyKFlag<int> AIKO_APOLOGY_SINCERE = new(2364, "AIKO_APOLOGY_SINCERE");	//1=sincere, 2=trick apologize for fighting

        public static LegacyKFlag<int> AIKO_RAPE = new(2365, "AIKO_RAPE");	//1=raped Aiko in scene 2

        public static LegacyKFlag<int> AIKO_TALK_CULTURE = new(2366, "AIKO_TALK_CULTURE");	//1=first time 2+=repeated times

        public static LegacyKFlag<int> AIKO_TALK_AIKO = new(2367, "AIKO_TALK_AIKO");	//1=first time 2+=repeated times

        public static LegacyKFlag<int> AIKO_TALK_ARCHERY = new(2368, "AIKO_TALK_ARCHERY");	//1=first time 2+=repeated times

        public static LegacyKFlag<int> AIKO_BOSS_intRO = new(2369, "AIKO_BOSS_intRO");	//1=Yamata scene introduction at lvl 24+ and Aiko 100 affection

        public static LegacyKFlag<int> AIKO_BOSS_COMPLETE = new(2370, "AIKO_BOSS_COMPLETE");	//1=You killed Yamata, 2=Aiko killed Yamata, 3=Yamata is released

        public static LegacyKFlag<int> AIKO_BOSS_OUTRO = new(2371, "AIKO_BOSS_OUTRO");	//

        public static LegacyKFlag<int> YAMATA_MASOCHIST = new(2372, "YAMATA_MASOCHIST");	//counter increases to 100, if not defeated before reaching 100, gain masochist perk.

        public static LegacyKFlag<int> TOTAL_HP_VICTORIES = new(2373, "TOTAL_HP_VICTORIES"); //How many HP victories. If you defeat Lethice while this counter is 0 and don't kill NPCs, gain Pacifist achievement.

        public static LegacyKFlag<int> TENTACLE_BEASTS_KILLED = new(2374, "TENTACLE_BEASTS_KILLED");

        public static LegacyKFlag<int> WORMS_MASS_KILLED = new(2375, "WORMS_MASS_KILLED");

        public static LegacyKFlag<float> ACHIEVEMENT_PROGRESS_DAMAGE_SPONGE = new(2376, "ACHIEVEMENT_PROGRESS_DAMAGE_SPONGE");
        // -------------------------
        // Unknown flags 2377 - 2636
        // -------------------------
        // Hellspawn flags - thrown here for the time being
        // public static LegacyKFlag<int> PLAYERCAMEBEFORERIDING = new LegacyKFlag<int>(2637);
        public static LegacyKFlag<int> HELSPAWN_HADSEX = new(2638, "HELSPAWN_HADSEX");
        //Ceraph test (delete from here if it fails)
        public static LegacyKFlag<int> CERAPHSUBMISSIVE = new(2639, "CERAPHSUBMISSIVE"); //Counter for Ceraph submissiveness, when it reaches 10 PC becomes a bitch nigga

        public static LegacyKFlag<int> CERAPHCHASITY = new(2640, "CERAPHCHASITY"); //1 means yay, 0 means nay

        public static LegacyKFlag<int> CERAPHTOLDGIRLS = new(2641, "CERAPHTOLDGIRLS"); //1 Means Ceraph told everyone that you're the camp's personal whore
        //End of Ceraph stuff
        // public static LegacyKFlag<int> UNKNOWN_FLAG_NUMBER_02642 = new LegacyKFlag<int>(2642);
        //public static LegacyKFlag<int> ASCENSION_RESET:int = new LegacyKFlag<int>(2643); // No longer used
        public static LegacyKFlag<int> ASCENSIONING = new(2644, "ASCENSIONING"); //Handles scaling in-game. Used to prevent ascension mode switches from actively changing the current game.

        public static LegacyKFlag<int> HADSLUGPARASITE = new(2645, "HADSLUGPARASITE"); //Tracks whether or not you were infected by a slug parasite in the past.

        public static LegacyKFlag<int> METVOLCANICGOLEM = new(2646, "METVOLCANICGOLEM"); //Tracks whether or not you met the Volcanic Golem before.

        public static LegacyKFlag<float> VOLCANICGOLEMHP = new(2647, "VOLCANICGOLEMHP"); //The Volcanic Golem has the same HP bar between encounters.

        public static LegacyKFlag<float> VOLCANICGOLEMSHIELDHP = new(2648, "VOLCANICGOLEMSHIELDHP"); //Tracks the Volcanic Golem's shimmering shield HP.

        public static LegacyKFlag<int> DESTROYEDVOLCANICGOLEM = new(2649, "DESTROYEDVOLCANICGOLEM"); //Tracks if the volcanic golem is destroyed.

        public static LegacyKFlag<int> VOLCWITCHNUMBEROFCHILDREN = new(2650, "VOLCWITCHNUMBEROFCHILDREN"); //Tracks how many times you fathered a corrupted witch. This is abstract; there's no real pregnancy event.

        public static LegacyKFlag<int> VOLCWITCHNUMBEROFBIRTHS = new(2651, "VOLCWITCHNUMBEROFBIRTHS"); //Tracks how many times you've birthed a corrupted witch.

        public static LegacyKFlag<int> POTENTPREGNANCYBOOST = new(2652, "POTENTPREGNANCYBOOST"); //Tracks if the player is due the potent pregnancy stat boost.

        public static LegacyKFlag<int> AMILY_TREE_MADEUPBULLSHIT = new(2653, "AMILY_TREE_MADEUPBULLSHIT"); //You managed to convince your lover of a lie so you can fuck a tree.

        public static LegacyKFlag<int> PARASITE_EEL_DAYDONE = new(2654, "PARASITE_EEL_DAYDONE"); //Makes sure parasites only hunger for semen(and only raise your lust for it, "PARASITE_EEL_DAYDONE") once every three days.

        public static LegacyKFlag<int> AMILY_BULGE_STRIKES = new(2655, "AMILY_BULGE_STRIKES"); //Three strikes means you're out.

        public static LegacyKFlag<int> AMILY_BULGE_PROMISE = new(2656, "AMILY_BULGE_PROMISE"); //Player promised to never again impale Amily on his dick. Oh man.

        public static LegacyKFlag<int> CERAPH_KILLED = new(2657, "CERAPH_KILLED"); //Player killed Ceraph.

        public static LegacyKFlag<int> KIHA_KILLED = new(2658, "KIHA_KILLED"); //Player killed Kiha. What a monster.

        public static LegacyKFlag<int> DULLAHAN_MET = new(2659, "DULLAHAN_MET"); //Met the dullahan

        public static LegacyKFlag<int> DULLAHAN_RUDE = new(2660, "DULLAHAN_RUDE"); //You were rude to the dullahan. No more nice Gan Caenn.

        public static LegacyKFlag<int> DULLAHAN_DEAD = new(2661, "DULLAHAN_DEAD"); //You're aware that the Dullahan is, in fact, dead.

        public static LegacyKFlag<int> BEAUTIFUL_SWORD_LEVEL = new(2662, "BEAUTIFUL_SWORD_LEVEL"); //Tracks the Beautiful Sword power.
        //public static LegacyKFlag<int> OTHERCOCANON_SURVIVALTWEAK:int = new LegacyKFlag<int>(2663); // No longer used
        public static LegacyKFlag<int> SWORD_SHARDS_TAKEN = new(2665, "SWORD_SHARDS_TAKEN"); //How many shards you've taken.

        public static LegacyKFlag<int> ZETAZ_POTION_CURED = new(2666, "ZETAZ_POTION_CURED"); //Tracks whether or not you're being affected by Zetaz's concoction. 0 = completely, 1 = partially. Purity Blessing Perk eliminates it.

        public static LegacyKFlag<int> UNKNOWN_FLAG_NUMBER_02667 = new(2667, "UNKNOWN_FLAG_NUMBER_02667"); //How many times the Minotaur's "minoMutual" scene been accessed

        public static LegacyKFlag<int> TIMES_BEATEN_DULLAHAN_SPAR = new(2668, "TIMES_BEATEN_DULLAHAN_SPAR"); //how many times the player has beaten the dullahan while sparring. Used to award the "Counter" ability, and unlock the Manor dungeon.

        public static LegacyKFlag<int> RECEIVED_MASTURBATION_WARNING = new(2669, "RECEIVED_MASTURBATION_WARNING");

        public static LegacyKFlag<int> UNKNOWN_FLAG_NUMBER_02670 = new(2670, "UNKNOWN_FLAG_NUMBER_02670");

        public static LegacyKFlag<int> MANOR_PROGRESS = new(2671, "MANOR_PROGRESS"); //going to try to use just one flag for the entire manor.

        public static LegacyKFlag<int> FOUND_MANOR = new(2672, "FOUND_MANOR"); //except for this.

        public static LegacyKFlag<int> CODEX_EPHRAIM_JOURNAL = new(2673, "CODEX_EPHRAIM_JOURNAL"); //except for this.

        public static LegacyKFlag<int> ACCEPTED_DULL_REQUEST = new(2674, "ACCEPTED_DULL_REQUEST"); //Dullahan has made her request. 0 = not made. 1 = made, accepted, 2 = made, not accepted. 3 = accepted, quest complete, took scythe.

        public static LegacyKFlag<int> CORR_WITCH_COVEN = new(2675, "CORR_WITCH_COVEN"); //again, one flag for all Coven needs.

        public static LegacyKFlag<int> UNIT_AMOUNT = new(2676, "UNIT_AMOUNT"); //used for "horde" fights.

        public static LegacyKFlag<int> SAVE_FIXED = new(2677, "SAVE_FIXED"); //Preventing players from fixing their save twice.

        public static LegacyKFlag<int> PUMPKIN_SEEDS_EATEN = new(2678, "PUMPKIN_SEEDS_EATEN"); //tracks the alternate way to get Fera's events.

        public static LegacyKFlag<int> PUMPKIN_SEEDS_FAIL = new(2679, "PUMPKIN_SEEDS_FAIL"); //Ignore this

        public static LegacyKFlag<int> IZMA_JOJO_CHILDREN_EVENT = new(2680, "IZMA_JOJO_CHILDREN_EVENT");

        public static LegacyKFlag<int> ANNEMARIE_STATUS = new(2681, "ANNEMARIE_STATUS");

        public static LegacyKFlag<int> FOUND_WIZARD_TOWER = new(2682, "FOUND_WIZARD_TOWER");

        public static LegacyKFlag<int> WIZARD_TOWER_PROGRESS = new(2683, "WIZARD_TOWER_PROGRESS");

        public static LegacyKFlag<int> GRABBED_LIGHT_RAIL_AVENGER = new(2684, "GRABBED_LIGHT_RAIL_AVENGER");

        public static LegacyKFlag<int> VILKUS_DEFEAT_DAY = new(2685, "VILKUS_DEFEAT_DAY");
        //public static LegacyKFlag<int> USING_CHARVIEWER:int = new LegacyKFlag<int>(2686); //No longer used
        public static LegacyKFlag<int> CHILD_LOSSES_SANDWITCH = new(2687, "CHILD_LOSSES_SANDWITCH"); //5 is badend.

        public static LegacyKFlag<int> GIFTED_FLOWER = new(2688, "GIFTED_FLOWER");

        public static LegacyKFlag<int> HELIA_MINO_SITUATION = new(2689, "HELIA_MINO_SITUATION");

        public static LegacyKFlag<int> HELIA_ADDICTION_LEVEL = new(2690, "HELIA_ADDICTION_LEVEL"); //The usual.

        public static LegacyKFlag<int> HELIA_FED_CUM = new(2691, "HELIA_FED_CUM");

        public static LegacyKFlag<int> HELIA_FED_DAY = new(2692, "HELIA_FED_DAY"); //unused

        public static LegacyKFlag<int> HELIA_OUT = new(2693, "HELIA_OUT");

        public static LegacyKFlag<int> HELIA_CURE_KNOWN = new(2694, "HELIA_CURE_KNOWN");

        public static LegacyKFlag<int> HELIA_ADDICTION_COMPLETE = new(2695, "HELIA_ADDICTION_COMPLETE");

        public static LegacyKFlag<int> LOLIPOP_COUNTER = new(2696, "LOLIPOP_COUNTER");

        public static LegacyKFlag<int> ALICE_CHATS = new(2697, "ALICE_CHATS"); // Alice chats ; flag system

        public static LegacyKFlag<int> CODEX_ENTRY_ALICE = new(2698, "CODEX_ENTRY_ALICE"); // Alice Codex

        public static LegacyKFlag<int> EQUIPPED_NAUGHTY_NUN = new(2699, "EQUIPPED_NAUGHTY_NUN");

        public static LegacyKFlag<int> TOOK_NAUGHTY_HABIT = new(2700, "TOOK_NAUGHTY_HABIT");

        public static LegacyKFlag<int> MET_CULTIST_NUN = new(2701, "MET_CULTIST_NUN");

        public static LegacyKFlag<int> PRAYED_WITH_NUN = new(2702, "PRAYED_WITH_NUN");

        public static LegacyKFlag<int> DEFILED_NUN = new(2703, "DEFILED_NUN");

        public static LegacyKFlag<int> UNLOCKED_ASCENSION_AGE = new(2704, "UNLOCKED_ASCENSION_AGE");

        public static LegacyKFlag<int> DESTROYED_BEAUTIFUL_SWORD = new(2705, "DESTROYED_BEAUTIFUL_SWORD");

        public static LegacyKFlag<int> CORRUPTED_COVEN_CIRCE_CHIT_CHAT_COUNT = new(2706, "CORRUPTED_COVEN_CIRCE_CHIT_CHAT_COUNT");

        public static LegacyKFlag<int> CIRCE_TOOK_JEREMIAH = new(2707, "CIRCE_TOOK_JEREMIAH"); //Not such a bad end after all.

        public static LegacyKFlag<int> CORRUPTED_COVEN_CIRCE_CHIT_CHAT_COOLDOWN = new(2708, "CORRUPTED_COVEN_CIRCE_CHIT_CHAT_COOLDOWN");

        public static LegacyKFlag<int> NH_BJ = new(2709, "NH_BJ");//seriously.

        public static LegacyKFlag<int> DULLAHAN_HORSE_NAME = new(2710, "DULLAHAN_HORSE_NAME");//LENOOORE? LENORE

        public static LegacyKFlag<int> DULL_DATES = new(2711, "DULL_DATES");

        public static LegacyKFlag<int> MET_HELLMOUTH = new(2712, "MET_HELLMOUTH");

        public static LegacyKFlag<int> CODEX_ENTRY_HELLMOUTH = new(2713, "CODEX_ENTRY_HELLMOUTH");
        // public static LegacyKFlag<int> UNKNOWN_FLAG_NUMBER_02714 = new LegacyKFlag<int>(2714);
        public static LegacyKFlag<int> AKBAL_QUEST_STATUS = new(2715, "AKBAL_QUEST_STATUS");

        public static LegacyKFlag<int> AKBAL_FUCKERY = new(2716, "AKBAL_FUCKERY");

        public static LegacyKFlag<int> AKBAL_DAY_DONE = new(2717, "AKBAL_DAY_DONE");

        public static LegacyKFlag<string> AKKY_NAME = new(2718, "AKKY_NAME"); //Name given to Akbal's new housecat form

        public static LegacyKFlag<int> MET_ALIANDRA = new(2719, "MET_ALIANDRA"); //Currently unused, just tracked for potential future expansion

        public static LegacyKFlag<int> ALIANDRA_DEAD = new(2720, "ALIANDRA_DEAD"); //Died before she even got any content...

        public static LegacyKFlag<int> MAGIC_SWITCH = new(2721, "MAGIC_SWITCH"); //Sets the type of magic you're currently using, default is black/white/gray

        public static LegacyKFlag<int> CODEX_ENTRY_TERRESTRIAL_FIRE = new(2722, "CODEX_ENTRY_TERRESTRIAL_FIRE");

        public static LegacyKFlag<int> GENDER_SWITCH = new(2723, "GENDER_SWITCH"); //0 = auto, 1 = male, 2 = female

        public static LegacyKFlag<int> HELLMOUTHS_KILLED = new(2724, "HELLMOUTHS_KILLED");

        public static LegacyKFlag<int> ALICES_KILLED = new(2725, "ALICES_KILLED");
        //public static LegacyKFlag<int> QUICKLOAD_ANYWHERE_ENABLED:int = new LegacyKFlag<int>(2726); // No longer used
        public static LegacyKFlag<int> LIDDELLIUM_FLAG = new(2727, "LIDDELLIUM_FLAG"); //0+ is times drank, -1 is identified

        public static LegacyKFlag<int> LIDDELLIUM_SEAN_FLAG = new(2728, "LIDDELLIUM_SEAN_FLAG"); //Sean interactions with liddellium

        public static LegacyKFlag<int> LIDDELLIUM_LUMI_FLAG = new(2729, "LIDDELLIUM_LUMI_FLAG"); //Does Lumi sell Liddellium?

        public static LegacyKFlag<int> LIDDELLIUM_CERAPH_FLAG = new(2730, "LIDDELLIUM_CERAPH_FLAG"); //Ceraph's interactions with Liddellium

        public static LegacyKFlag<int> LIDDELLIUM_DUNGEON_FLAG = new(2731, "LIDDELLIUM_DUNGEON_FLAG"); //The Liddellium Dungeon/event chain

        public static LegacyKFlag<int> FARM_STABLES = new(2732, "FARM_STABLES"); //1 = found the stables

        public static LegacyKFlag<int> CODEX_ENTRY_FAERIE = new(2733, "CODEX_ENTRY_FAERIE");

        public static LegacyKFlag<int> CODEX_ENTRY_PHOUKA = new(2734, "CODEX_ENTRY_PHOUKA");

        public static LegacyKFlag<int> SYLVIA_PREGNANCY_TYPE = new(2735, "SYLVIA_PREGNANCY_TYPE");

        public static LegacyKFlag<int> SYLVIA_PREGNANCY_INCUBATION = new(2736, "SYLVIA_PREGNANCY_INCUBATION");

        public static LegacyKFlag<int> CODEX_ENTRY_ALRAUNE = new(2737, "CODEX_ENTRY_ALRAUNE");

        public static LegacyKFlag<int> FROG_PREGNANCY_TYPE = new(2738, "FROG_PREGNANCY_TYPE");

        public static LegacyKFlag<int> FROG_PREGNANCY_INCUBATION = new(2739, "FROG_PREGNANCY_INCUBATION");

        public static LegacyKFlag<int> CODEX_ENTRY_RAT_MICE = new(2740, "CODEX_ENTRY_RAT_MICE");
        // -------------------------
        // Unknown flags 2741 - 2776
        // -------------------------
        //public static LegacyKFlag<int> NOFUR_MODE_ENABLE_FLAG:int = new LegacyKFlag<int>(2777); // No longer used
        //public static LegacyKFlag<int> APPLES_OF_IDUNN:int = new LegacyKFlag<int>(2778); // No longer used
        public static LegacyKFlag<int> EMBER_AGE = new(2779, "EMBER_AGE");
        // -------------------------
        // Unknown flags 2780 - 2783
        // -------------------------
        //public static LegacyKFlag<int> LOLI_GHOST:int = new LegacyKFlag<int>(2784); // No longer used
        //public static LegacyKFlag<int> LOLI_GARGOYLE:int = new LegacyKFlag<int>(2785); // No longer used
        public static LegacyKFlag<int> GAR_LOVER_TALKS = new(2786, "GAR_LOVER_TALKS");

        public static LegacyKFlag<int> GAR_LOVER_CHOICE = new(2787, "GAR_LOVER_CHOICE"); // 1=Slave, 2=Lover, 3=Family

        public static LegacyKFlag<int> LEWDED_THE_GARGOYLE = new(2788, "LEWDED_THE_GARGOYLE");

        public static LegacyKFlag<int> EMBER_HERM_EGGS = new(2789, "EMBER_HERM_EGGS");
        // -------------------------
        // Unknown flags 2790 - 2791
        // -------------------------
        //public static LegacyKFlag<int> UNDERAGE_ENABLED:int = new LegacyKFlag<int>(2792); // No longer used
        //public static LegacyKFlag<int> GORE_ENABLED:int = new LegacyKFlag<int>(2793); // No longer used
        // -------------------------
        // Unknown flags 2794 - 2795
        // -------------------------
        public static LegacyKFlag<int> BED_BEARS = new(2796, "BED_BEARS"); //Tracks the number of teddy bears sharing your bed

        public static LegacyKFlag<int> ICE_WEAPON_TIMER = new(2797, "ICE_WEAPON_TIMER"); //Tracks how long ice weapons last
        // -------------------------
        // Unknown flags 2798 - 2899
        // -------------------------
        //public static LegacyKFlag<int> TIMES_ORGASM_VAGINAL:int = new LegacyKFlag<int>(2900); //No longer used
        //public static LegacyKFlag<int> TIMES_ORGASM_ANAL:int = new LegacyKFlag<int>(2901); //No longer used
        //public static LegacyKFlag<int> TIMES_ORGASM_DICK:int = new LegacyKFlag<int>(2902); //No longer used
        //public static LegacyKFlag<int> TIMES_ORGASM_TITS:int = new LegacyKFlag<int>(2903); //No longer used
        //public static LegacyKFlag<int> TIMES_ORGASM_NIPPLES:int = new LegacyKFlag<int>(2904); //No longer used
        //public static LegacyKFlag<int> TIMES_ORGASM_LIPS:int = new LegacyKFlag<int>(2905); //No longer used
        //public static LegacyKFlag<int> BIMBOSKIRT_MINIMUM_LUST:int = new LegacyKFlag<int>(2906); //No longer used
        // -------------------------
        // Unknown flags 2907 - 2909
        // -------------------------
        //public static LegacyKFlag<int> GRIMDARK_BACKGROUND_UNLOCKED:int = new LegacyKFlag<int>(2910); //deprecated
        // //public static LegacyKFlag<int> GRIMDARK_FACTORY_DOOR_UNLOCKED:int = new LegacyKFlag<int>(2911); //deprecated
        // -------------------------
        // Unknown flags 2912 - 2917
        // -------------------------
        public static LegacyKFlag<int> PARASITE_NEPHILA_DAYDONE = new(2918, "PARASITE_NEPHILA_DAYDONE"); //Makes sure parasites only hunger for semen(and only raise your lust for it, "PARASITE_NEPHILA_DAYDONE") once every three days.

        public static LegacyKFlag<int> TIMES_MET_NEPHILA = new(2919, "TIMES_MET_NEPHILA"); //Notes how many times original nephila has been met

        public static LegacyKFlag<int> PC_MET_NEPHILA_COVEN = new(2920, "PC_MET_NEPHILA_COVEN"); //Notes how many times nephila coven members have been met

        public static LegacyKFlag<int> NEPHILA_COVEN_QUEEN_CROWNED = new(2921, "NEPHILA_COVEN_QUEEN_CROWNED"); //Indicates whether the player has been crowned by the coven yet or not; controls whether the Nephila Coven follower shows up in camp yet or not.

        public static LegacyKFlag<int> NEPHILA_QUEEN_ARMOR = new(2922, "NEPHILA_QUEEN_ARMOR"); //Indicates that the nephila coven has summoned nephila queen armor in the past and can no longer do so. Prevents selling and re-acquiring armor.

        public static LegacyKFlag<int> NEPHILA_SECOND_ASCENSION = new(2923, "NEPHILA_SECOND_ASCENSION"); //Indicates that a nephila Arch Queen has tested themselves against the nephila throne a second time and received the nephila scepter. Allows a 20+ infection queen to do third ascension and Bad End.

        public static LegacyKFlag<int> CODEX_ENTRY_NEPHILA = new(2924, "CODEX_ENTRY_NEPHILA"); //Triggers the addition of the Nephila codex entry.

        public static LegacyKFlag<int> NEPHILA_MOUSE_MET = new(2925, "NEPHILA_MOUSE_MET"); //Tracks whether Julienne has been met yet. Also tracks whether she's been converted to an industrial state at the end of the mini-quest (2 = no & 3 = yes, "NEPHILA_MOUSE_MET").

        public static LegacyKFlag<int> NEPHILA_MOUSE_OWNED = new(2926, "NEPHILA_MOUSE_OWNED"); //Tracks Julienne being added to camp and Julienne's growth/ability to satisfy champion mouse hunger.

        public static LegacyKFlag<int> CODEX_ENTRY_GNOLL = new(2927, "CODEX_ENTRY_GNOLL");
        // -------------------------
        // Unknown flags 2928 - 2972
        // -------------------------
        //public static LegacyKFlag<int> ADDICTIONS_DISABLED:int = new LegacyKFlag<int>(2973); // No longer used
        //public static LegacyKFlag<int> LEGACY_RACIAL_STATS_ENABLED:int = new LegacyKFlag<int>(2974); // No longer used
        //public static LegacyKFlag<int> ENEMY_STATS_BARS_ENABLED:int = new LegacyKFlag<int>(2975); // No longer used
        //public static LegacyKFlag<int> ANIMATE_STATS_BARS:int = new LegacyKFlag<int>(2976); // No longer used
        public static LegacyKFlag<int> DELETE_ITEMS = new(2977, "DELETE_ITEMS"); // Inventory deletion toggle, 0=no, 1=one at a time, 2=stacks
        //public static LegacyKFlag<int> PRISON_ENABLED:int = new LegacyKFlag<int>(2978); // No longer used
        public static LegacyKFlag<int> NEW_GAME_PLUS_LEVEL = new(2979, "NEW_GAME_PLUS_LEVEL"); // Current intensity of New Game+. Each ascension increments this counter by 1.
        //public static LegacyKFlag<int> NEW_GAME_PLUS_BONUS_UNLOCKED_HERM:int = new LegacyKFlag<int>(2980); // No longer used
        //public static LegacyKFlag<int> USE_OLD_intERFACE:int = new LegacyKFlag<int>(2981); // No longer used
        //public static LegacyKFlag<int> IMAGEPACK_ENABLED:int = new LegacyKFlag<int>(2982); // No longer used
        //public static LegacyKFlag<int> TEXT_BACKGROUND_STYLE:int = new LegacyKFlag<int>(2983); // No longer used
        //public static LegacyKFlag<int> AUTO_LEVEL:int = new LegacyKFlag<int>(2984); // No longer used
        //public static LegacyKFlag<int> SFW_MODE:int = new LegacyKFlag<int>(2985); // No longer used
        //public static LegacyKFlag<int> WATERSPORTS_ENABLED:int = new LegacyKFlag<int>(2986); // No longer used
        //public static LegacyKFlag<int> USE_METRICS:int = new LegacyKFlag<int>(2987); // No longer used
        //public static LegacyKFlag<int> USE_OLD_FONT:int = new LegacyKFlag<int>(2988); // No longer used
        //public static LegacyKFlag<int> BACKGROUND_STYLE:int = new LegacyKFlag<int>(2989); // No longer used.
        //public static LegacyKFlag<int> GAME_DIFFICULTY:int = new LegacyKFlag<int>(2990); // No longer used
        //public static LegacyKFlag<int> HARDCORE_MODE:int = new LegacyKFlag<int>(2991); // No longer used
        //public static LegacyKFlag<int> HARDCORE_SLOT:int = new LegacyKFlag<int>(2992); // No longer used
        //public static LegacyKFlag<int> HUNGER_ENABLED:int = new LegacyKFlag<int>(2993); // No longer used
        //public static LegacyKFlag<int> USE_12_HOURS:int = new LegacyKFlag<int>(2994); // No longer used
        public static LegacyKFlag<int> MEANINGLESS_CORRUPTION = new(2995, "MEANINGLESS_CORRUPTION"); // Raises corruption tolerance by 100.

        public static LegacyKFlag<int> ITS_EVERY_DAY = new(2996, "ITS_EVERY_DAY"); // all special calender events occur every day!
        //public static LegacyKFlag<int> LOW_STANDARDS_FOR_ALL:int = new LegacyKFlag<int>(2997); // No longer used
        //public static LegacyKFlag<int> HYPER_HAPPY:int = new LegacyKFlag<int>(2998); // No longer used
        public static LegacyKFlag<int> GRIMDARK_MODE = new(2999, "GRIMDARK_MODE"); // No longer used

        public static LegacyKFlag<bool> HELSPAWN_JOINED_SLUT = new(3000, "HELSPAWN_JOINED_SLUT");
    }
}