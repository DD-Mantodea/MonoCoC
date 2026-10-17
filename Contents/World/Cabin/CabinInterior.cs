using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.World
{
    public static class CabinInterior
    {
        private static GameFlags Flags => CoC.Instance.Flags;

        private static Player Player => CoC.Instance.Player;

        public static bool HasBed => Flags.Get(GameFlagIds.CampCabinFurnitureBed) > 0;

        public static bool HasNightstand => Flags.Get(GameFlagIds.CampCabinFurnitureNightstand) > 0;

        public static bool HasDresser => Flags.Get(GameFlagIds.CampCabinFurnitureDresser) > 0;

        public static bool HasTable => Flags.Get(GameFlagIds.CampCabinFurnitureTable) > 0;

        public static bool HasTableChair1 => Flags.Get(GameFlagIds.CampCabinFurnitureChair1) > 0;

        public static bool HasTableChair2 => Flags.Get(GameFlagIds.CampCabinFurnitureChair2) > 0;

        public static bool HasBookshelf => Flags.Get(GameFlagIds.CampCabinFurnitureBookshelf) > 0;

        public static bool HasDesk => Flags.Get(GameFlagIds.CampCabinFurnitureDesk) > 0;

        public static bool HasDeskChair => Flags.Get(GameFlagIds.CampCabinFurnitureDeskchair) > 0;

        public static bool HasBedding() =>

            Player.HasKeyItem("Moth Bedding") || Player.HasKeyItem("Spider-Silk Bedding");

        public static void ResetBeddingPerks()
        {
            Player.RemovePerk(PerkIds.MothBedding);

            Player.RemovePerk(PerkIds.SpiderBedding);
        }
    }
}