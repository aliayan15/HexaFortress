using UnityEngine;
using UnityEngine.Events;

namespace HexaFortress.GamePlay
{
    public class EnemyTrigger : MonoBehaviour
    {
        public UnityEvent<EnemyController> OnEnemyEnter;
        public UnityEvent<EnemyController> OnEnemyExit;


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyController enemy))
                OnEnemyEnter?.Invoke(enemy);
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out EnemyController enemy))
                OnEnemyExit?.Invoke(enemy);
        }
    }
}

