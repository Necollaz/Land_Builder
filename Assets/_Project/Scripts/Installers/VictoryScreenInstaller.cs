using UnityEngine;
using Zenject;

public class VictoryScreenInstaller : MonoInstaller
{
    [Header("Config")]
    [SerializeField] private VictoryScreenConfig _victoryScreenConfig;

    [Header("Scene View")]
    [SerializeField] private VictoryScreenView _victoryScreenView;

    public override void InstallBindings()
    {
        Container.Bind<VictoryScreenConfig>().FromInstance(_victoryScreenConfig).AsSingle();
        Container.QueueForInject(_victoryScreenView);

        Container.Bind<VictoryRewardModel>().AsSingle();
        Container.Unbind<ILevelRewardSource>();
        Container.Bind<ILevelRewardSource>().To<DummyLevelRewardSource>().AsSingle().WithArguments(_victoryScreenConfig);

        Container.Bind<VictoryScreenPresenter>().AsSingle();
        Container.Bind<VictoryFlowCoordinator>().AsSingle();
    }
}