using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class FieldTileComponent : MonoBehaviour, IPointerClickHandler {
    public enum TileState{
        Disabled, Selectable, Walkable, Attackable
    }

    public TileState State { get; private set; }

    public Vector2Int Coordinates { get; private set; }

    public Action<TileState> OnTileStateSet;

    [SerializeField] private FieldTileVisualComponent _visualComponent;

    [Inject] private InputsSO _inputs;

    [Inject] private FieldPathFinder _fieldPathFinder;

    public void Init(Vector2Int coords) {
        Coordinates = coords;
    }

    public Character CharacterOnTile {
        get; private set;
    }

    public void SetCharacterOnTile(Character character) {
        CharacterOnTile = character;
    }

    public void SetState(TileState tileState) {
        State = tileState;

        OnTileStateSet?.Invoke(State);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button != _inputs.ActionButton) return;

        if (State == TileState.Disabled) return;

        _fieldPathFinder.OnClick(this);

        _visualComponent.OnSelect();
    }
}
