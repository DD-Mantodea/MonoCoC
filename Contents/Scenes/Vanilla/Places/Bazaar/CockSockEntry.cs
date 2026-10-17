namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{

    public record CockSockEntry(string buttonText, string id, int price, string description)
    {
        public string Id => id;
    }
}