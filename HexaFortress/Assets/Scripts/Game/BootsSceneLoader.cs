using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexaFortress.Game
{
    public class BootsSceneLoader : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}