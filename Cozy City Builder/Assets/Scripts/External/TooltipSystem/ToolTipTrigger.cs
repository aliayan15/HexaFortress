using UnityEngine;
using UnityEngine.EventSystems;


public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string content;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Show();
    }

    private void Show()
    {
        ToolTipSystem.Show(content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipSystem.Hide();
    }

    //public void MouseExit()
    //{
    //    ToolTipSystem.Hide();
    //}
}

