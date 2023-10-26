using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class InGameUIBase : MonoBehaviour
{
    private GameLoopManager _gameLoopManager;

    [Inject]
    public void Init(GameLoopManager gameLoopManager) {
        _gameLoopManager = gameLoopManager;
        _gameLoopManager.OnGameEnd += (x) => OnWin(x);
    }

    public abstract void OnWin(Character.Team winner);
}
