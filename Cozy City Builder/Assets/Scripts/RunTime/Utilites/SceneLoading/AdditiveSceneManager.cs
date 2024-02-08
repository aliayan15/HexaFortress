using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AdditiveSceneManager : MonoBehaviour
{
    [SerializeField][Tooltip("Scene Name to load")] private string sceneName;
    [SerializeField]
    [Tooltip("Unload time delay after trigger exit")]
    private float unLoadTimeDelay = 2f;

    public UnityEvent AsyncLoadDone;


    private bool _isLoaded = false;
    private bool _playerIsIn = false;
    private Coroutine _unLoadTimeLimit;
    private Coroutine _asyncLoadCoroutine;
    private AsyncOperation _asyncLoad = null;


    #region Trigger EVENTS
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerIsIn = true;
        ChangeSceneOrder(true);
        DebugLog("OnTriggerEnter");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerIsIn = false;
        if (_unLoadTimeLimit != null)
            StopCoroutine(_unLoadTimeLimit);
        _unLoadTimeLimit = StartCoroutine(UnLoadTimeDelay());
        DebugLog("OnTriggerExit");
    }

    // wait unLoadTimeDelay secont to unload
    private IEnumerator UnLoadTimeDelay()
    {
        yield return new WaitForSeconds(unLoadTimeDelay);
        if (!_playerIsIn)
            ChangeSceneOrder(false);
        _unLoadTimeLimit = null;
    }
    #endregion

    #region Set Load/Unload Order
    private void ChangeSceneOrder(bool load)
    {
        // order has change, cansel old one
        if (_asyncLoadCoroutine != null)
            StopCoroutine(_asyncLoadCoroutine);
        _asyncLoadCoroutine = StartCoroutine(SceneOrder(load));
    }

    // if scene already loading/unloading wait until it finish and execute new order
    private IEnumerator SceneOrder(bool load)
    {
        while (_asyncLoad != null)
        {
            yield return null;
        }

        if (load)
            StartCoroutine(LoadScene());
        else
            StartCoroutine(UnLoadScene());
        _asyncLoadCoroutine = null;
    }
    #endregion

    #region Load or UnLoad
    IEnumerator LoadScene()
    {
        if (_isLoaded) yield break;
        _isLoaded = true;

        _asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!_asyncLoad.isDone)
        {
            yield return null;
        }

        AsyncLoadDone?.Invoke();
        _asyncLoad = null;
#if UNITY_EDITOR
        DebugLog("Scene Loaded: " + sceneName);
#endif
    }

    IEnumerator UnLoadScene()
    {
        if (!_isLoaded) yield break;
        _isLoaded = false;

        _asyncLoad = SceneManager.UnloadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully unload
        while (!_asyncLoad.isDone)
        {
            yield return null;
        }

        Resources.UnloadUnusedAssets();
        _asyncLoad = null;
#if UNITY_EDITOR
        DebugLog("Scene Unloaded: " + sceneName);
#endif
    }
    #endregion

    private void DebugLog(string msg)
    {
#if UNITY_EDITOR
        Debug.Log(msg);
#endif
    }
}
