using UnityEngine;
using UnityEngine.Events;

namespace HexaFortress.GamePlay
{
    public class EnemyTrigger : MonoBehaviour
    {
        public UnityEvent<Enemy> OnEnemyEnter;
        public UnityEvent<Enemy> OnEnemyExit;


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
                OnEnemyEnter?.Invoke(enemy);
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
                OnEnemyExit?.Invoke(enemy);
        }
    }
}

