using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static FieldTileComponent;

public class FieldTileVisualComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _borderImage;

    [SerializeField] private Image _selectionIndicator;

    [SerializeField] private float _hoveredPixelMul, _nonHoveredPixelMul, _disabledPixelMul;

    [SerializeField] private Color _disabledColor, _selectableColor, _walkableColor, _attackableColor;

    private TileState _state;

    private Tween _hoverBorderTween;
    private Tween _colorBorderTween;

    private Tween _selectionIndicatorScaleTween;

    private FieldTileComponent _tileComponent;

    private void OnEnable() {
        _borderImage.pixelsPerUnitMultiplier = _disabledPixelMul;

        _selectionIndicator.transform.localScale = Vector3.zero;

        _tileComponent = GetComponent<FieldTileComponent>();

        _tileComponent.OnTileStateSet += (x) => SetStateVisuals(x);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (_state == TileState.Disabled) return;

        _hoverBorderTween?.Kill();
        _hoverBorderTween = HoverTween(_hoveredPixelMul);
    }

    public void OnPointerExit(PointerEventData eventData) {
        _hoverBorderTween?.Kill();
        _hoverBorderTween = HoverTween(_state == TileState.Disabled ? _disabledPixelMul : _nonHoveredPixelMul);
    }

    private Tween HoverTween(float pixelMultiplyer) {
        return DOVirtual.Float(_borderImage.pixelsPerUnitMultiplier, pixelMultiplyer, .25f, (x) => _borderImage.pixelsPerUnitMultiplier = x).SetEase(Ease.InCubic | Ease.OutBack);
    }

    public void OnSelect() {
        if (_state != TileState.Selectable) return;

        _selectionIndicatorScaleTween?.Kill();
        _selectionIndicatorScaleTween = _selectionIndicator.transform.DOScale(1f, .25f).SetEase(Ease.OutBack);
    }

    public void SetStateVisuals(TileState tileState) {
        _colorBorderTween?.Kill();

        _state = tileState;

        _hoverBorderTween?.Kill();
        _hoverBorderTween = HoverTween(_nonHoveredPixelMul);

        if (tileState == TileState.Disabled) {
            _colorBorderTween = _borderImage.DOColor(_disabledColor, .25f);

            _hoverBorderTween?.Kill();
            _hoverBorderTween = HoverTween(_disabledPixelMul);

            _selectionIndicatorScaleTween?.Kill();
            _selectionIndicatorScaleTween = _selectionIndicator.transform.DOScale(0, .25f).SetEase(Ease.InCubic);

        } else if (tileState == TileState.Walkable) {
            _colorBorderTween = _borderImage.DOColor(_walkableColor, .25f);
        } else if (tileState == TileState.Selectable) {
            _colorBorderTween = _borderImage.DOColor(_selectableColor, .25f);
        } else if (tileState == TileState.Attackable) {
            _colorBorderTween = _borderImage.DOColor(_attackableColor, .25f);
        }   
    }
}
