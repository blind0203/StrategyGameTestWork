using UnityEngine;
using Zenject;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private Loader.Scenes _sceneName;

    [Inject] private Loader _loader;

    public void LoadScene() {
        _loader.LoadWithFade(_sceneName.ToString());
    }
}
