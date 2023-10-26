using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.PointerEventData;

[CreateAssetMenu()]
public class InputsSO : ScriptableObject
{
    public InputButton ActionButton;
    public KeyCode CamRotationButton;

    public float CamRotationSensetivity;
}
