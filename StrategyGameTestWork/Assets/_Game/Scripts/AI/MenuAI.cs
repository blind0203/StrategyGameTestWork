using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAI : MonoBehaviour
{
    [SerializeField] private ChoosePositionsWindowManager _cpwm;

    private void OnEnable() {
        if (GameParameters.AIPlayerIndex < 0) return;

        StartCoroutine(AICycle());
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    private IEnumerator AICycle() {
        int aiPlayerIndex = GameParameters.AIPlayerIndex;

        Debug.Log(aiPlayerIndex);

        while (true) {

            Debug.Log("wait");

            yield return new WaitUntil(() => _cpwm.CharactersPlaced % 2 == aiPlayerIndex);

            if (_cpwm.IsAllCharacterPlaced) yield break;

            yield return new WaitForSeconds(1f);

            Debug.Log("click");

            _cpwm.ClickRandomEmptyTile();

            yield return null;

            Debug.Log("approve");
            _cpwm.Approve();

            yield return null;
        }
    }
}
