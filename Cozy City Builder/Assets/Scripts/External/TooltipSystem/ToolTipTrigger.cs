using UnityEngine;
using UnityEngine.EventSystems;


public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    public string content;
    [Space(5)]
    [SerializeField] private bool showHeader = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Show();
    }

    protected virtual void Show()
    {
        if (!showHeader)
            ToolTipSystem.Show(content);
        else
            ToolTipSystem.Show(content, header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipSystem.Hide();
    }

    
}

