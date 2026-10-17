using System.Collections.Generic;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Pregnancy
{
    public class PregnancyStage
    {
        public int Min;

        public int Max;

        public string TextKey = "";
    }

    public class PregnancyType : IIdentifiable
    {
        public Identifier Id { get; set; } = new("coc", "none");

        public string Kind = "vaginal";

        public string Special = "";

        public string FullTermKey = "";

        public int LegacyId;

        public int Incubation;

        public List<int> Events = [];

        public Dictionary<string, Identifier> Deliveries = [];

        public List<PregnancyStage> Stages = [];

        public Identifier? DeliveryFor(string kind) =>

            Deliveries.TryGetValue(kind, out var id) ? id : null;
    }
}