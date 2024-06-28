#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexaFortress.Editor
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            var activeScene = SceneManager.GetActiveScene();
            bool canInstantiateSystemBoost = activeScene.name != EditorGlobalConsts.BoostSceneName;

            if (canInstantiateSystemBoost)
                Object.Instantiate(Resources.Load(EditorGlobalConsts.SystemBoostPrefabName));
        }
    }
}
#endif