using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSizeParameterSetter : MonoBehaviour
{
    [SerializeField] private int _teamSize;

    public void SetTeamSizeParameter() {
        GameParameters.TeamSize = _teamSize;
    }
}
