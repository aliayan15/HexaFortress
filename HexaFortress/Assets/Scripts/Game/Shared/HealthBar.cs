using UnityEngine;
using UnityEngine.UI;

namespace HexaFortress.Game
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private Image armorBar;
        
        private Transform _mTransform;
        private float _total;

        /// <summary>
        /// Init health bar component.
        /// </summary>
        /// <param name="hasArmor"></param>
        public void Init(bool hasArmor)
        {
            _mTransform = GetComponent<Transform>();
            _total = hasArmor ? 2 : 1;
            healthBar.fillAmount = 1;
            armorBar.fillAmount = 1;
        }

        /// <summary>
        /// Ratio 0-1
        /// </summary>
        /// <param name="healthRatio"></param>
        /// <param name="armorRatio"></param>
        public void UpdateBar(float healthRatio, float armorRatio)
        {
            armorRatio = (healthRatio + armorRatio) / _total;
            healthRatio = healthRatio / _total;

            healthBar.fillAmount = healthRatio;
            armorBar.fillAmount = armorRatio;
        }

        public void LookCamera()
        {
            _mTransform.LookAt(_mTransform.position + CameraManager.Instance.CamPosition.rotation * Vector3.forward,
                CameraManager.Instance.CamPosition.rotation * Vector3.up);
        }
    }
}

