using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiParameterSetter : MonoBehaviour
{
    [SerializeField] private bool _isAIInUse;

    public void SetAIUseParameter() {
        GameParameters.AIPlayerIndex = _isAIInUse ? Random.Range(0, 2) : -1;
    }
}
