using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace UI.CanvasManagers
{
    public class GameCanvasManager : MonoBehaviour
    {
       

        private void OnGameStateChange(GameStates state)
        {
            
        }

        private void OnEnable()
        {
            GameManager.OnGameStateChange+= OnGameStateChange;
        }
        private void OnDisable()
        {
            GameManager.OnGameStateChange -= OnGameStateChange;
        }

    }
}
