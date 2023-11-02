using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameAI : MonoBehaviour
{
    [Inject] private GameLoopManager _gameLoopManager;
    [Inject] private TurnSequencer _turnSequencer;

    private Character.Team _aiTeam;

    private List<List<FieldTileComponent>> _availableTiles;

    private int _movedTurnIndex = -1;

    private void OnEnable() {
        if (GameParameters.AIPlayerIndex < 0) return;

        _aiTeam = GameParameters.AIPlayerIndex == 0 ? Character.Team.Blue : Character.Team.Red;

        StartCoroutine(AICycle());
    }

    private void OnDisable() {
        StopAI();
    }

    public void StopAI() {
        StopAllCoroutines();
    }

    private IEnumerator AICycle() {
        while (true) {

            Debug.Log("wait");

            yield return new WaitUntil(() => _gameLoopManager.IsAITurn);

            _movedTurnIndex = _gameLoopManager.TurnIndex;

            yield return new WaitForSeconds(1f);

            List<FieldTileComponent> startTiles = GameFieldData.FieldTiles.FindAll(x => x.CharacterOnTile != null && x.CharacterOnTile.CharacterTeam == _aiTeam);
            
            _availableTiles = new List<List<FieldTileComponent>>();

            for (int i = 0; i < startTiles.Count; i++) {
                _availableTiles.Add(new List<FieldTileComponent>());
            }

            for (int i = 0; i < _availableTiles.Count; i++) {
                CheckNeighbours(startTiles[i], _availableTiles[i], 0);
            }

            yield return null;

            int r = Random.Range(0, _availableTiles.Count);

            while (_availableTiles[r].Count == 0) {
                r.LoopedIncrease(_availableTiles.Count);
            }

            List<FieldTileComponent> attackTiles = _availableTiles[r].FindAll(x => x.CharacterOnTile != null);
            List<FieldTileComponent> moveTiles = _availableTiles[r].FindAll(x => x.CharacterOnTile == null);

            bool isAttack = attackTiles.Count > 0 && Random.Range(0f, 1f) > .33f;

            FieldTileComponent finalTile = isAttack ? attackTiles[Random.Range(0, attackTiles.Count)] : moveTiles[Random.Range(0, moveTiles.Count)];

            _turnSequencer.StartTurnSequence(startTiles[r], finalTile);

            yield return new WaitWhile(() => _movedTurnIndex == _gameLoopManager.TurnIndex);
        }
    }

    private void CheckNeighbours(FieldTileComponent originTile, List<FieldTileComponent> availables, int iteration) {

        if (iteration == 2) return;

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if ((x == 0 && y == 0) || (Mathf.Abs(x) - Mathf.Abs(y) == 0)) continue;

                FieldTileComponent neighbourTile = GameFieldData.FieldTiles.Find(tile => tile.Coordinates - new Vector2Int(x, y) == originTile.Coordinates);

                if (neighbourTile == null) continue;

                if (neighbourTile.CharacterOnTile != null && neighbourTile.CharacterOnTile.CharacterTeam == _aiTeam) continue;

                availables.Add(neighbourTile);

                CheckNeighbours(neighbourTile, availables, iteration + 1);
            }
        }
    }
}
