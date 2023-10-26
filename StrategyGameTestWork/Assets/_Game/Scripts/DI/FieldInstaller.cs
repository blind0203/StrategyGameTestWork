using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FieldInstaller : MonoInstaller {
    [SerializeField] private FieldCreator _fieldCreator;
    [SerializeField] private FieldPathFinder _pathFinder;
    [SerializeField] private TurnSequencer _turnSequencer;
    [SerializeField] private GameLoopManager _gameLoopManager;

    public override void InstallBindings() {
        InstallField();
    }

    private void InstallField() {
        Container.Bind<TurnSequencer>().FromInstance(_turnSequencer).AsSingle().NonLazy();
        Container.Bind<GameLoopManager>().FromInstance(_gameLoopManager).AsSingle().NonLazy();
        Container.Bind<FieldCreator>().FromInstance(_fieldCreator).AsSingle().NonLazy();
        Container.Bind<FieldPathFinder>().FromInstance(_pathFinder).AsSingle().NonLazy();
    }

}
