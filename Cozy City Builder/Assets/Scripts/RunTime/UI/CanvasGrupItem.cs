using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGrupItem : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        public void Toogle(bool opened)
        {
            if (!_canvasGroup)
                _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = opened ? 1 : 0;
            _canvasGroup.interactable = opened;
            _canvasGroup.blocksRaycasts = opened;
        }
    }
}
