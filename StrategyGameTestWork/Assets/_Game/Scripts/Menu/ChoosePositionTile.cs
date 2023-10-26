using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoosePositionTile : MonoBehaviour, IPointerClickHandler
{
    public bool IsApproved = false;

    public int TileIndex { get; private set; }

    public Action<ChoosePositionTile, bool> OnClick;

    [SerializeField] private Image _characterIcon;
    [SerializeField] private Color _blueTeamColor, _redTeamColor;

    private ChoosePositionsWindowManager _choosePositionsWindowManager;

    private Tween _scaleTween;

    public void Init(ChoosePositionsWindowManager choosePositionsWindowManager) {
        _choosePositionsWindowManager = choosePositionsWindowManager;

        SetState(true, true);

        TileIndex = transform.GetSiblingIndex();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (IsApproved) return;

        if (GameParameters.AIPlayerIndex == _choosePositionsWindowManager.CharactersPlaced % 2) return;

        OnClick?.Invoke(this, false);
    }

    public void SetState(bool isCleaning, bool isInstant = false) {
        bool isBlue = _choosePositionsWindowManager.CharactersPlaced % 2 == 0;
        
        _characterIcon.color = isBlue ? _blueTeamColor : _redTeamColor;

        _scaleTween?.Kill();

        _scaleTween = _characterIcon.transform.DOScale(isCleaning ? 0 : 1f, isInstant ? 0 : .25f).SetEase(isCleaning ? Ease.InSine : Ease.OutBack).SetLink(gameObject);
    }

    public void Approve() {
        IsApproved = true;

        GameParameters.TeamIndexes[transform.GetSiblingIndex()] = _choosePositionsWindowManager.CharactersPlaced % 2 == 0 ? 0 : 1;
    }
}
