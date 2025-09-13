using UnityEngine;
using Zenject;

public class DefeatScreenInstaller : MonoInstaller
{
    [Header("Config")]
    [SerializeField] private DefeatScreenConfig _defeatScreenConfig;

    [Header("Scene View")]
    [SerializeField] private DefeatScreenView _defeatScreenView;

    public override void InstallBindings()
    {
        Container.Bind<DefeatScreenConfig>().FromInstance(_defeatScreenConfig).AsSingle();

        Container.Bind<DefeatAttemptModel>().AsSingle();
        Container.Bind<DefeatScreenPresenter>().AsSingle();
        Container.Bind<DefeatFlowCoordinator>().AsSingle();

        if (_defeatScreenView != null)
            Container.QueueForInject(_defeatScreenView);
    }
}