using MonoCoC.Extensions;

namespace MonoCoC.Contents.Pregnancy
{

    public static class PregnancyUtils
    {

        public static void CreateVaginaIfMissing(PregnancyContext context)
        {
            var player = context.Player;

            if (player.Vaginas.Count > 0)
                return;

            context.Write("Pregnancy.Utils.VaginaGrows".Localized());

            player.CreateVagina();
        }
    }
}