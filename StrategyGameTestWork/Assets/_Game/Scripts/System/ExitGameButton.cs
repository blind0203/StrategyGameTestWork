using UnityEngine;

public class ExitGameButton : MonoBehaviour
{
    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
