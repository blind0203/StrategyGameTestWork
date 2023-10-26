using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LoaderInstaller : MonoInstaller
{
    [SerializeField] private Loader _loader;

    public override void InstallBindings() {
        var loaderInstance = Container.InstantiatePrefabForComponent<Loader>(_loader);

        Container.Bind<Loader>().FromInstance(loaderInstance).AsSingle().NonLazy();
    }
}
