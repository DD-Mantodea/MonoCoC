using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Factory;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Dungeons
{
    public static class FactoryRooms
    {
        [Register("coc:factory_iron_key")]
        public static bool IronKey(DungeonContext context) => Scene.Player.HasKeyItem("Iron Key");

        [Register("coc:factory_supervisor_key")]
        public static bool SupervisorKey(DungeonContext context) => Scene.Player.HasKeyItem("Supervisor's Key");

        [Register("coc:factory_defeat")]
        public static void Defeat(DungeonContext context)
        {
            context.Write("Scenes.Factory.Defeat.KickedOut".Localized());

            context.LeaveDungeon();
        }

        [Register("coc:factory_repair_closet")]
        public static void RepairCloset(DungeonContext context) => SceneInstance<FactoryScene>.Get().RoomRepairCloset();

        [Register("coc:factory_main_chamber")]
        public static void MainChamber(DungeonContext context) => SceneInstance<FactoryScene>.Get().RoomMainChamber();

        [Register("coc:factory_pump_room")]
        public static void PumpRoom(DungeonContext context) => SceneInstance<FactoryScene>.Get().RoomPumpRoom();

        [Register("coc:factory_furnace_room")]
        public static void FurnaceRoom(DungeonContext context) => SceneInstance<FactoryScene>.Get().RoomFurnaceRoom();

        [Register("coc:factory_bathroom")]
        public static void Bathroom(DungeonContext context) => SceneInstance<FactoryScene>.Get().RoomBathroom();

        [Register("coc:factory_lobby")]
        public static void Lobby(DungeonContext context) => SceneInstance<FactoryScene>.Get().RoomLobby();

        [Register("coc:factory_break_room")]
        public static void BreakRoom(DungeonContext context) => SceneInstance<FactoryScene>.Get().RoomBreakRoom();

        [Register("coc:factory_foreman_office")]
        public static void ForemanOffice(DungeonContext context) => SceneInstance<FactoryScene>.Get().RoomForemanOffice();

        [Register("coc:factory_control_room")]
        public static void ControlRoom(DungeonContext context) => SceneInstance<FactoryScene>.Get().RoomControlRoom();

        [Register("coc:factory_premium_storage")]
        public static void PremiumStorage(DungeonContext context) => SceneInstance<FactoryScene>.Get().RoomPremiumStorage();
    }
}