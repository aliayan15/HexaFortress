using System;
using UnityEngine;

namespace HexaFortress.Game
{
    public class ParticleCallBack : MonoBehaviour
    {
        public Action OnStop;
        
        private void OnParticleSystemStopped()
        {
            OnStop?.Invoke();
        }
    }
}