using Hjson;
using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.World.Cabin
{
    public class CabinState
    {
        public Identifier CurrentStage;

        public HashSet<Identifier> CompletedStages = [];

        public bool IsCompleted(Identifier stage) => CompletedStages.Contains(stage);

        public void Complete(Identifier stage) => CompletedStages.Add(stage);

        public void Save(JsonObject json)
        {
            json["currentStage"] = CurrentStage.ToString();

            var completed = new JsonArray();

            foreach (var stage in CompletedStages)
                completed.Add(stage.ToString());

            json["completedStages"] = completed;
        }

        public void Load(JsonObject json)
        {
            CompletedStages.Clear();

            CurrentStage = json.TryGetValue("currentStage", out var current) &&

                          Identifier.TryParse((string)current, out var stage)

                ? stage

                : default;

            if (!json.TryGetValue("completedStages", out var value) || value is not JsonArray completed)
                return;

            foreach (var item in completed)
                if (Identifier.TryParse((string)item, out var completedStage))
                    CompletedStages.Add(completedStage);
        }
    }
}