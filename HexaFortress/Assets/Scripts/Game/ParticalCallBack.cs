using System;
using UnityEngine;

namespace HexaFortress.Game
{
    public class ParticalCallBack : MonoBehaviour
    {
        public Action OnStop;
        
        private void OnParticleSystemStopped()
        {
            OnStop?.Invoke();
        }
    }
}