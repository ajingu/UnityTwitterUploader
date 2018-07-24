using UnityEngine;
using Zenject;

public class ManagerInstaller : MonoInstaller<ManagerInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<TwitterManager>().AsSingle();
    }
}