using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FieldCreator : MonoBehaviour
{
    [SerializeField] private Character _blueCharacter, _redCharacter;
    [SerializeField] private FieldTileComponent _fieldTilePrefab;
    [SerializeField] private Transform _tilesContainer;

    [Inject] private DiContainer _diContainer;

    private void Awake() {
        FillField();
    }

    private void FillField() {
        GameFieldData.FieldTiles = new List<FieldTileComponent>();

        for (int i = 0; i < 25; i++) {
            FieldTileComponent tile = _diContainer.InstantiatePrefabForComponent<FieldTileComponent>(_fieldTilePrefab, _tilesContainer);

            tile.Init(new Vector2Int((int)(i % 5), (int)(i / 5)));

            GameFieldData.FieldTiles.Add(tile);
        }

        CharacterSpawn();
    }

    private void CharacterSpawn() {
        GameFieldData.Characters = new List<Character>();

        List<int> teamPlacement = GameParameters.TeamIndexes;

        for (int i = 0; i < teamPlacement.Count; i++) {
            if (teamPlacement[i] == -1) continue;

            Vector3 position = new Vector3(GameFieldData.FieldTiles[i].Coordinates.x - 2, 0, GameFieldData.FieldTiles[i].Coordinates.y - 2);

            Character spawnedCharacter = Instantiate(teamPlacement[i] == 0 ? _blueCharacter : _redCharacter, position, Quaternion.LookRotation(-position.normalized), null);

            GameFieldData.Characters.Add(spawnedCharacter);

            GameFieldData.FieldTiles[i].SetCharacterOnTile(spawnedCharacter);
        }  
    }
}
