using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGrupItem : MonoBehaviour
    {
        [HideInInspector]
        public CanvasGroup canvasGroup;

        public void Toogle(bool opened)
        {
            if (!canvasGroup)
                canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = opened ? 1 : 0;
            canvasGroup.interactable = opened;
            canvasGroup.blocksRaycasts = opened;
        }
    }
}
