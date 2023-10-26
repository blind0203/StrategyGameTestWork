using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameScreenUI : InGameUIBase {

    [SerializeField] private CanvasGroup _canvasGroup;
    
    public override void OnWin(Character.Team winner) {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }
}
