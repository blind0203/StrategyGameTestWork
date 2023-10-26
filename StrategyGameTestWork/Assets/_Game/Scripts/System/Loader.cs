using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Loader : MonoBehaviour
{
    #region SceneNames

    public enum Scenes {
        Loader,
        Menu,
        Game
    }

    #endregion

    #region Fade

    [SerializeField] private CanvasGroup _fadeCanvasGroup;

    #endregion

    #region Loading;

    [SerializeField] private Image _progress;

    #endregion

    private void OnEnable() {
        _fadeCanvasGroup.alpha = 0;

        Application.targetFrameRate = -1;
    }

    private void Start() {
        SceneManager.sceneLoaded += (Scene, LoadSceneMode) => OnLevelLoaded(Scene);

        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame() {
        yield return null;

        LoadWithFade("Menu");
    }

    public void LoadWithFade(string sceneName) {
        _progress.fillAmount = 0;
        _fadeCanvasGroup.blocksRaycasts = true;
        _fadeCanvasGroup.DOFade(1, .25f).SetUpdate(true).SetEase(Ease.InCubic).OnComplete(() => {
            StartCoroutine(LoadingProgress(sceneName));
        });
    }

    private IEnumerator LoadingProgress(string sceneName) {
        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!loadSceneOperation.isDone) {
            _progress.fillAmount = loadSceneOperation.progress;
            yield return null;
        }

        FadeOut();
    }

    private void OnLevelLoaded(Scene scene) {
        
    }

    public void FadeOut() {
        _fadeCanvasGroup.DOFade(0, .5f).SetDelay(.5f).SetUpdate(true).SetEase(Ease.InCubic).OnComplete(() => {
            _fadeCanvasGroup.blocksRaycasts = false;
        });
    }
}
