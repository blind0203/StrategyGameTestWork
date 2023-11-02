using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FieldPathFinder : MonoBehaviour {
    public FieldTileComponent _firstClickedTile { get; private set; }
    public FieldTileComponent _secondClickedTile { get; private set; }

    private TurnSequencer _turnSequencer;

    private GameLoopManager _gameLoopManager;

    private List<FieldTileComponent> _checkedTiles;

    [Inject]
    public void Init(TurnSequencer turnSequencer, GameLoopManager gameLoopManager) {
        _turnSequencer = turnSequencer;
        _gameLoopManager = gameLoopManager;

        turnSequencer.OnSequenceStart += DeselectAll;

        _gameLoopManager.OnTurnEnd += (x) => SetSelectables(x);

        StartCoroutine(AfterInit());
    }

    public IEnumerator AfterInit() {
        yield return new WaitWhile(() => GameFieldData.FieldTiles == null);
        yield return new WaitWhile(() => GameFieldData.FieldTiles.Count < 25);

        SetSelectables(0);
    }

    public void OnClick(FieldTileComponent tile) {
        if (_firstClickedTile == tile) return;

        if (_firstClickedTile == null || tile.State == FieldTileComponent.TileState.Selectable) {
            Debug.Log("select");

            _firstClickedTile = tile;
            _checkedTiles = new List<FieldTileComponent>();

            DeselectAll();
            SetSelectables(_gameLoopManager.TurnIndex);

            CheckNeighbours(tile, 0);
        } else if (tile.State == FieldTileComponent.TileState.Walkable || tile.State == FieldTileComponent.TileState.Attackable) {
            _turnSequencer.StartTurnSequence(_firstClickedTile, tile);

            _firstClickedTile = null;
        }
    }

    public void DeselectAll() {
        foreach (var item in GameFieldData.FieldTiles) {
            item.SetState(FieldTileComponent.TileState.Disabled);
        }
    }

    public void SetSelectables(int currentTurnIndex) {
        if (_gameLoopManager.IsAITurn) return;

        foreach (var item in GameFieldData.FieldTiles) {
            Character.Team team = currentTurnIndex % 2 == 0 ? Character.Team.Blue : Character.Team.Red;

            if (item.CharacterOnTile != null && item.CharacterOnTile.CharacterTeam == team) {
                item.SetState(FieldTileComponent.TileState.Selectable);
            } else {
                item.SetState(FieldTileComponent.TileState.Disabled);
            }
        }
    }

    private void CheckNeighbours(FieldTileComponent originTile, int iteration) {
        if (iteration == 2) return;

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if ((x == 0 && y == 0) || (Mathf.Abs(x) - Mathf.Abs(y) == 0)) continue;

                FieldTileComponent neighbourTile = GameFieldData.FieldTiles.Find(tile => tile.Coordinates - new Vector2Int(x, y) == originTile.Coordinates);

                if (neighbourTile == null) continue;

                if (neighbourTile.CharacterOnTile != null) {
                    Character.Team team = _gameLoopManager.TurnIndex % 2 == 0 ? Character.Team.Blue : Character.Team.Red;

                    if (neighbourTile.CharacterOnTile.CharacterTeam == team) {
                        _checkedTiles.Add(neighbourTile);
                        neighbourTile.SetState(FieldTileComponent.TileState.Selectable);
                        continue;
                    }

                    if (neighbourTile.CharacterOnTile.CharacterTeam != team) {
                        _checkedTiles.Add(neighbourTile);
                        neighbourTile.SetState(FieldTileComponent.TileState.Attackable);
                        continue;
                    }
                } else {
                    neighbourTile.SetState(FieldTileComponent.TileState.Walkable);
                }

                CheckNeighbours(neighbourTile, iteration + 1);
            }
        }
    }
}