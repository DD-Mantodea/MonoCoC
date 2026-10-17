using MonoCoC.Contents.Flags;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<NPCAwareContentScene>("coc:npcaware_content")]
    public class NPCAwareContentScene : Scene
    {

        // 原版 Mutations.hx:34 changes / :35 changeLimit —— 现由 Contents/Mutations/Mutations.cs 承载
        public int changes => Mutations.Mutations.Instance.Changes;

        public int changeLimit => Mutations.Mutations.Instance.ChangeLimit;

        public FollowerInteractionsScene finter => SceneInstance<FollowerInteractionsScene>.Get();

        public AkkyScene akky => SceneInstance<AkkyScene>.Get();

        public AmilyScene amilyScene => SceneInstance<AmilyScene>.Get();

        public AnemoneScene anemoneScene => SceneInstance<AnemoneScene>.Get();

        public ArianScene arianScene => SceneInstance<ArianScene>.Get();

        public CeraphScene ceraphScene => SceneInstance<CeraphScene>.Get();

        public CeraphFollowerScene ceraphFollowerScene => SceneInstance<CeraphFollowerScene>.Get();

        public EmberScene emberScene => SceneInstance<EmberScene>.Get();

        public ExgartuanScene exgartuan => SceneInstance<ExgartuanScene>.Get();

        public HelScene helScene => SceneInstance<HelScene>.Get();

        public HelFollowerScene helFollower => SceneInstance<HelFollowerScene>.Get();

        public HelSpawnScene helSpawnScene => SceneInstance<HelSpawnScene>.Get();

        public HolliScene holliScene => SceneInstance<HolliScene>.Get();

        public IsabellaScene isabellaScene => SceneInstance<IsabellaScene>.Get();

        public IsabellaFollowerScene isabellaFollowerScene => SceneInstance<IsabellaFollowerScene>.Get();

        public IzmaScene izmaScene => SceneInstance<IzmaScene>.Get();

        public JojoScene jojoScene => SceneInstance<JojoScene>.Get();

        public KihaFollowerScene kihaFollowerScene => SceneInstance<KihaFollowerScene>.Get();

        public KihaScene kihaScene => SceneInstance<KihaScene>.Get();

        public LatexGirlScene latexGirl => SceneInstance<LatexGirlScene>.Get();

        public MarbleScene marbleScene => SceneInstance<MarbleScene>.Get();

        public MarblePurification marblePurification => SceneInstance<MarblePurification>.Get();

        public MilkWaifuScene milkWaifu => SceneInstance<MilkWaifuScene>.Get();

        public NephilaCovenScene nephilaCovenScene => SceneInstance<NephilaCovenScene>.Get();

        public NephilaCovenFollowerScene nephilaCovenFollowerScene => SceneInstance<NephilaCovenFollowerScene>.Get();

        public RaphaelScene raphael => SceneInstance<RaphaelScene>.Get();

        public RathazulScene rathazul => SceneInstance<RathazulScene>.Get();

        public SheilaScene sheilaScene => SceneInstance<SheilaScene>.Get();

        public ShouldraFollowerScene shouldraFollower => SceneInstance<ShouldraFollowerScene>.Get();

        public ShouldraScene shouldraScene => SceneInstance<ShouldraScene>.Get();

        public SophieBimboScene sophieBimbo => SceneInstance<SophieBimboScene>.Get();

        public SophieScene sophieScene => SceneInstance<SophieScene>.Get();

        public SophieFollowerScene sophieFollowerScene => SceneInstance<SophieFollowerScene>.Get();

        public Areas.Bog.SylviaScene sylviaScene => SceneInstance<Areas.Bog.SylviaScene>.Get();

        public UrtaScene urta => SceneInstance<UrtaScene>.Get();

        public UrtaPregsScene urtaPregs => SceneInstance<UrtaPregsScene>.Get();

        public UrtaHeatRutScene urtaHeatRut => SceneInstance<UrtaHeatRutScene>.Get();

        public ValeriaScene valeria => SceneInstance<ValeriaScene>.Get();

        public VapulaScene vapula => SceneInstance<VapulaScene>.Get();

        public Seasonal.NieveScene nieve => SceneInstance<Seasonal.NieveScene>.Get();

        public override void Entry()
        {
            AkkyOwned();
        }

        public bool AkkyOwned()
        {

            return akky.IsOwned();
        }

        public bool AmilyFollower()
        {

            return amilyScene.AmilyFollower();
        }

        public bool AmilyCorrupt()
        {

            return amilyScene.AmilyCorrupt();
        }

        public bool AnemoneFollower()
        {

            return anemoneScene.AnemoneFollower();
        }

        public bool ArianFollower()
        {

            return arianScene.ArianFollower();
        }

        public bool CeraphIsFollower()
        {

            return ceraphFollowerScene.CeraphIsFollower();
        }

        public bool FollowerEmber()
        {

            return SceneInstance<EmberScene>.Get().FollowerEmber();
        }

        public string EmberMF(string man, string woman)
        {

            return SceneInstance<EmberScene>.Get().EmberMF(man, woman);
        }

        public bool FollowerHel()
        {

            return helScene.FollowerHel();
        }

        public bool HelPregnant()
        {

            return helSpawnScene.HelPregnant();
        }

        public bool HelspawnFollower()
        {

            return helSpawnScene.HelspawnFollower();
        }

        public bool IsabellaFollower()
        {

            return isabellaFollowerScene.IsabellaFollower();
        }

        public bool IsabellaAccent() =>

            CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0;

        public bool IzmaFollower()
        {

            return izmaScene.IzmaFollower();
        }

        public bool JojoFollower()
        {

            return jojoScene.JojoFollower();
        }

        public bool CampCorruptJojo()
        {

            return jojoScene.CampCorruptJojo();
        }

        public bool FollowerKiha()
        {

            return kihaFollowerScene.FollowerKiha();
        }

        public bool LatexGooFollower()
        {

            return latexGirl.LatexGooFollower();
        }

        public bool MarbleFollower()
        {

            return marbleScene.MarbleFollower();
        }

        public bool MilkSlave()
        {

            return milkWaifu.MilkSlave();
        }

        public bool NephilaCovenIsFollower()
        {

            return nephilaCovenFollowerScene.NephilaCovenIsFollower();
        }

        public bool RaphaelLikes()
        {

            return raphael.RaphaelLikes();
        }

        public bool FollowerRathazul()
        {

            return rathazul.FollowerRathazul();
        }

        public bool FollowerShouldra()
        {

            return shouldraFollower.FollowerShouldra();
        }

        public bool BimboSophie()
        {

            return sophieBimbo.BimboSophie();
        }

        public bool SophieFollower()
        {

            return SceneInstance<SophieFollowerScene>.Get().SophieFollower();
        }

        public bool UrtaLove(float love = 0)
        {

            return SceneInstance<UrtaScene>.Get().UrtaLove(love);
        }

        public bool VapulaSlave()
        {

            return vapula.VapulaSlave();
        }

        public bool NieveFollower()
        {

            return nieve.NieveFollower();
        }

    }
}