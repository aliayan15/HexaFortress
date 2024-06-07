using UnityEngine;
using UnityEngine.EventSystems;

namespace HexaFortress.UI
{
    public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string header;
        public string content;
        [Space(5)]
        [SerializeField] protected bool showHeader = false;
        [SerializeField] protected bool is3dWorld = false;

        private bool _canShowWithOnMouse = true;


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!is3dWorld)
                ToolTipSystem.Instance.CanShowWithOnMouse = false;
            Show();
        }

        protected virtual void Show()
        {
            if (!showHeader)
                ToolTipSystem.Show(content, is3dWorld);
            else
                ToolTipSystem.Show(content, header, is3dWorld);
        }
        protected virtual void Hide()
        {
            ToolTipSystem.Hide();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!is3dWorld)
                ToolTipSystem.Instance.CanShowWithOnMouse = true;
            Hide();
        }

        private void OnMouseEnter()
        {
            if (!ToolTipSystem.Instance.CanShowWithOnMouse) return;
            Show();
        }
        private void OnMouseExit()
        {
            if (!ToolTipSystem.Instance.CanShowWithOnMouse) return;
            Hide();
        }
    }
}

