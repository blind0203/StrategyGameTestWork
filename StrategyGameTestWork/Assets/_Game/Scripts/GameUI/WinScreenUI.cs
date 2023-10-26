using DG.Tweening;
using TMPro;
using UnityEngine;

public class WinScreenUI : InGameUIBase
{
    [SerializeField] private TMP_Text _winnerNameText;

    [SerializeField] private Color _blueColor, _redColor;

    [SerializeField] private CanvasGroup _canvasGroup;

    private void OnEnable() {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }

    public override void OnWin(Character.Team winner) {
        _winnerNameText.text = winner == Character.Team.Blue ? "blue" : "red";
        _winnerNameText.color = winner == Character.Team.Blue ? _blueColor : _redColor;

        _canvasGroup.DOFade(1f, 1f).SetUpdate(true).SetEase(Ease.Linear).OnComplete(() => {
            _canvasGroup.blocksRaycasts = true;
        });
    }
}
