using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameLoopManager : MonoBehaviour
{
    public Action<Character.Team> OnGameEnd;

    public bool IsAITurn => GameParameters.AIPlayerIndex >= 0 && GameParameters.AIPlayerIndex == TurnIndex % 2;

    public bool IsGameEnded { get; private set; }

    public int TurnIndex { get {
            return _turnIndex;
        } private set {
            _turnIndex = value;
            OnTurnEnd?.Invoke(_turnIndex);
        }
    }
    
    public Action<int> OnTurnEnd;
    
    public int BlueCharactersAlive { get {
            return _blueCharactersAlive;
        } private set {
            _blueCharactersAlive = value;

            if (_blueCharactersAlive == 0) {
                IsGameEnded = true;
                OnGameEnd?.Invoke(Character.Team.Red /*winner team*/);
            }
        }
    }
    
    private int _blueCharactersAlive = 0;
    
    public int RedCharactersAlive { get {
            return _redCharactersAlive;
        } private set {
            _redCharactersAlive = value;

            if (_redCharactersAlive == 0) {
                IsGameEnded = true;
                OnGameEnd?.Invoke(Character.Team.Blue /*winner team*/);
            }
        }
    }

    private int _redCharactersAlive = 0;

    private int _turnIndex = 0;

    [SerializeField] private Image _fieldBorderImage;
    [SerializeField] private Color[] _teamColors;

    [Inject] private TurnSequencer _turnSequencer;

    private void Awake() {
        IsGameEnded = false;

        _turnSequencer.OnSequenceEnd += OnTurnEndAction;

        _fieldBorderImage.color = _teamColors[0];

        StartCoroutine(Subscribe());
    }

    private IEnumerator Subscribe() {
        yield return new WaitWhile(() => GameFieldData.Characters == null);
        yield return new WaitWhile(() => GameFieldData.Characters.Count < GameParameters.TeamSize * 2);

        BlueCharactersAlive = GameParameters.TeamSize;
        RedCharactersAlive = GameParameters.TeamSize;

        foreach (var item in GameFieldData.Characters) {
            if (item.CharacterTeam == Character.Team.Blue) {
                item.OnDie += (x) => BlueCharactersAlive--;
            } else {
                item.OnDie += (x) => RedCharactersAlive--;
            }
        }
    }

    public void OnTurnEndAction() {
        if (BlueCharactersAlive == 0 || RedCharactersAlive == 0) {
            return;
        }

        TurnIndex++;

        _fieldBorderImage.DOColor(_teamColors[TurnIndex % 2], .5f);
    }
}
