using UnityEngine;
using Zenject;

public class CoreSceneInstaller : MonoInstaller
{
    [SerializeField] private SceneNamesData _sceneNamesData;

    public override void InstallBindings()
    {
        Container.Bind<SceneNamesData>().FromInstance(_sceneNamesData).AsSingle();
        Container.Bind<SceneNavigator>().AsSingle();
        Container.Bind<GameResultModel>().AsSingle();
        Container.Bind<IRewardedAdService>().To<DummyRewardedAdService>().AsSingle();
        Container.Bind<ICurrencyWallet>().To<PlayerPrefsCurrencyWallet>().AsSingle();
    }
}