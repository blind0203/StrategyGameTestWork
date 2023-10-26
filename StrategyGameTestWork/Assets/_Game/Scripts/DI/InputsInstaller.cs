using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputsInstaller : MonoInstaller
{
    [SerializeField] private InputsSO _inputs;

    public override void InstallBindings() {
        Container.Bind<InputsSO>().FromInstance(_inputs).NonLazy();
    }
}
