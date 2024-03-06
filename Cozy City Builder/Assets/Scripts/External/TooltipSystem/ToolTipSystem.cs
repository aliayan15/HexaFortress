using MyUtilities;
using UnityEngine;


public class ToolTipSystem : SingletonMono<ToolTipSystem>
{
    public ToolTip CurrentToolTip;
    public bool CanShowUI { get; set; } = true;

    [SerializeField] private float delay = 0.3f;
    private void Start()
    {
        CurrentToolTip.gameObject.SetActive(false);
    }

    public static void Show(string content)
    {
        if (!Instance.CanShowUI) return;
        Instance.Timer(Instance.delay, () =>
        {
            Instance.CurrentToolTip.SetText(content);
            Instance.CurrentToolTip.gameObject.SetActive(true);
        });
    }
    public static void Show(string content, string header)
    {
        if (!Instance.CanShowUI) return;
        Instance.Timer(0.5f, () =>
        {
            Instance.CurrentToolTip.SetText(content, header);
            Instance.CurrentToolTip.gameObject.SetActive(true);
        });
    }
    public static void Hide()
    {
        Instance.StopAllCoroutines();
        Instance.CurrentToolTip.gameObject.SetActive(false);
    }
}

