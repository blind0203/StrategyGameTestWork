using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePositionsWindowManager : MonoBehaviour {
    public int CharactersPlaced { get; private set; }
    public bool IsAllCharacterPlaced => CharactersPlaced == GameParameters.TeamSize * 2;

    private int _bluePlaced = 0, _redPlaced = 0;

    [SerializeField] private Button _approveButton, _startGameButton;

    [SerializeField] private TMP_Text _player1Text, _player2Text;
    [SerializeField] private TMP_Text _blueCountText, _redCountText;


    private ChoosePositionTile[] _choosePositionTiles;
    private ChoosePositionTile _lastClickedTile = null;

    private void OnEnable() {
        _approveButton.interactable = false;
        _startGameButton.interactable = false;

        CharactersPlaced = 0;

        _bluePlaced = 0;

        _redPlaced = 0;

        _lastClickedTile = null;

        _choosePositionTiles = GetComponentsInChildren<ChoosePositionTile>();

        foreach (var item in _choosePositionTiles) {
            item.Init(this);

            item.OnClick += (x, y) => OnClick(x, y);
        }

        GameParameters.ClearTeamIndexes();

        UpdateCountTexts();

        SetNames();
    }

    private void OnDisable() {
        foreach (var item in _choosePositionTiles) {
            item.OnClick -= OnClick;
        }
    }

    private void SetNames() {
        _player1Text.text = GameParameters.AIPlayerIndex == 0 ? "AI" : "Player";
        _player2Text.text = GameParameters.AIPlayerIndex == 1 ? "AI" : "Player";
    }

    public void ClickRandomEmptyTile() {
        int r = Random.Range(0, 25);

        while (_choosePositionTiles[r].IsApproved) {
            r = (r + Random.Range(1, 5)) % _choosePositionTiles.Length;
        }

        OnClick(_choosePositionTiles[r], true);
    }

    public void OnClick(ChoosePositionTile tile, bool isAIChoosing) {
        if (IsAllCharacterPlaced) return;

        bool isNeedCleaning = _lastClickedTile != null;

        if (isNeedCleaning) {
            _lastClickedTile.SetState(true);
        }

        tile.SetState(false);

        _lastClickedTile = tile;

        if (isAIChoosing) return;
        
        EnableApproveButton();
    }

    private void EnableApproveButton() {
        _approveButton.interactable = true;
    }

    public void Approve() {
        _approveButton.interactable = false;

        _lastClickedTile.Approve();

        _lastClickedTile = null;

        if (CharactersPlaced % 2 == 0) {
            _bluePlaced++;
        } else {
            _redPlaced++;
        }

        UpdateCountTexts();

        CharactersPlaced++;

        if (IsAllCharacterPlaced) {
            EnableStartGameButton();
        }
    }

    private void EnableStartGameButton() {
        _startGameButton.interactable = true;
    }

    private void UpdateCountTexts() {
        _blueCountText.text = _bluePlaced + "/" + GameParameters.TeamSize;
        _redCountText.text = _redPlaced + "/" + GameParameters.TeamSize;
    }
}
