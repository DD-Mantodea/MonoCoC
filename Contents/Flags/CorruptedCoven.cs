using System;

namespace MonoCoC.Contents.Flags
{

    [Flags]
    public enum CorruptedCovenFlags
    {
        None = 0,

        MetCirce = 1 << 0,

        ToldName = 1 << 1,

        TalkedMaraeBless = 1 << 2,

        TalkedFeraBless = 1 << 3,

        TalkedJeremiahWeapons = 1 << 4,

        TalkedLaurentiusIncident = 1 << 5,

        TalkedManorDullahan = 1 << 6,

        TalkedCorrWitchHex = 1 << 7,

        TalkedTelAdreWizards = 1 << 8,

        TalkedGargoyle = 1 << 9,

        TalkedSandwitchBless = 1 << 10,

        TalkedKitsuneEnlightenment = 1 << 11,

        TalkedDominikaSword = 1 << 12,

        TalkedNamelessHorror = 1 << 13,

        CaughtAlchemy = 1 << 14,

        GotRingOfEtherealTearing = 1 << 15,

        GotMajorRingOfAccuracy = 1 << 16,

        GotSpectreRing = 1 << 17,

        BroughtJeremiahBack = 1 << 18,

        HadSexOnce = 1 << 19,

        BroughtDullahan = 1 << 20,

        ReturnedDullahanPostdream = 1 << 21,

        TalkedBeautifulSword = 1 << 22,

        BroughtManorBooks = 1 << 23
    }

    public static class CorruptedCoven
    {

        public static CorruptedCovenFlags Read(GameFlags flags) =>

            (CorruptedCovenFlags)flags.Get(GameFlagIds.CorrWitchCoven);

        public static bool Has(int raw, CorruptedCovenFlags bits) =>

            ((CorruptedCovenFlags)raw & bits) != 0;

        public static void Add(GameFlags flags, CorruptedCovenFlags bits) =>

            flags.Set(GameFlagIds.CorrWitchCoven,

                (int)(Read(flags) | bits));

        public static void Remove(GameFlags flags, CorruptedCovenFlags bits) =>

            flags.Set(GameFlagIds.CorrWitchCoven,

                (int)(Read(flags) & ~bits));
    }
}