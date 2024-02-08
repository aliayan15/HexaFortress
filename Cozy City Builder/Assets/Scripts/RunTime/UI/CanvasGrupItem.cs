using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGrupItem : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Toogle(bool opened)
        {
            _canvasGroup.alpha = opened ? 1 : 0;
            _canvasGroup.interactable = opened;
            _canvasGroup.blocksRaycasts = opened;
        }
    }
}
