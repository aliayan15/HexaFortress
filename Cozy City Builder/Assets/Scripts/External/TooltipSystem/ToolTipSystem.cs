using MyUtilities;
using UnityEngine;

    
public class ToolTipSystem : SingletonMono<ToolTipSystem>
{
    public ToolTip CurrentToolTip;

    public static void Show(string content)
    {
        Instance.Timer(0.5f, () =>
        {
            Instance.CurrentToolTip.SetText(content);
            Instance.CurrentToolTip.gameObject.SetActive(true);
        });
    }
    public static void Hide()
    {
        Instance.StopAllCoroutines();
        Instance.CurrentToolTip.gameObject.SetActive(false);
    }
}

