using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Creation
{
    public sealed class CharCreationDraft
    {
        public string Name = "";

        public Age Age = Age.Adult;

        public Gender Gender = Gender.Male;

        public Build Build = Build.Medium;

        public int Orientation = 50;

        public string SkinTone = "light";

        public string HairColor = "brown";

        public float Height = 170;

        public float CockLength = 14;

        public int BreastSize;

        public float HairLength = 5;

        public int BeardStyle;

        public int BeardLength;

        public bool IsVirgin = true;

        public int VirginityState;

        public bool Hardcore;

        public int Difficulty;

        public Gender BuildGender = Gender.Male;

        public Identifier History;

        public Identifier Endowment;

        public Identifier PendingHistory;

        public Identifier PendingEndowment;

        public void ApplyTo(Player player)
        {
            player.ShortName = string.IsNullOrWhiteSpace(Name) ? "Champion" : Name.Trim();

            player.Age = Age;

            player.StartingAge = Age;

            player.Tallness = Height;

            player.ClearGender();

            switch (Gender)
            {
                case Gender.Male:
                    player.CreateCock(CockLength, 1f, BodyPartTypes.Human);
                    break;

                case Gender.Female:
                    player.CreateVagina(IsVirgin);
                    break;

                case Gender.Hermaphrodite:
                    player.CreateCock(CockLength, 1f, BodyPartTypes.Human);
                    player.CreateVagina(IsVirgin);
                    break;
            }

            if (player.BreastRows.Count == 0)
                player.CreateBreastRow(BreastSize);
            else
                player.BreastRows[0].BreastRating = BreastSize;

            player.Body.Get<HairPart>().Color = HairColor;

            player.Body.Get<HairPart>().Length = HairLength;

            player.Body.Get<SkinPart>().Tone = SkinTone;

            var beard = player.Body.Get<BeardPart>();

            beard.Style = BeardStyle;

            beard.Length = BeardLength;

            if (History != default)
                player.CreatePerkIfNotHasPerk(History);

            if (Endowment != default)
                player.CreatePerkIfNotHasPerk(Endowment);

            player.CharCreation = false;
        }
    }
}
