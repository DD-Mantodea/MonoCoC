using Hjson;

namespace MonoCoC.Contents.Creatures
{
    /// <summary>
    /// 原版 bodyParts/Udder.hx：玩家的泌乳器（是否有、当前储奶量、每小时回充速度）。
    /// </summary>
    public partial class Player
    {
        public UdderState Udder { get; } = new();
    }

    public class UdderState
    {
        public bool HasUdder;

        // 原版注释写 0-100，但牛奶瓶的判定用 1000，这里按原版实际逻辑保留
        public float Fullness;

        public float Refill = 5;

        public void Restore()
        {
            HasUdder = false;

            Fullness = 0;

            Refill = 5;
        }

        public void Save(JsonObject json)
        {
            json["hasUdder"] = HasUdder;

            json["fullness"] = Fullness;

            json["refill"] = Refill;
        }

        public void Load(JsonObject json)
        {
            HasUdder = json.TryGetValue("hasUdder", out var hasUdder) && (bool)hasUdder;

            Fullness = json.TryGetValue("fullness", out var fullness) ? (float)(double)fullness : 0;

            Refill = json.TryGetValue("refill", out var refill) ? (float)(double)refill : 5;
        }
    }
}