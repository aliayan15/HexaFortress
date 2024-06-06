using MyUtilities;
using UnityEngine;


public class ToolTipSystem : SingletonMono<ToolTipSystem>
{
    public ToolTip CurrentToolTip;
    public bool CanShowUI {
        get { return _canShowUI; }
        set { _canShowUI = value; CanShow3dWorldUI = value; }
    }
    public bool CanShow3dWorldUI { get; set; } = true;
    public bool CanShowWithOnMouse { get; set; } = true;

    private bool _canShowUI = true;
    [SerializeField] private float delay = 0.3f;
    private void Start()
    {
        CurrentToolTip.gameObject.SetActive(false);
    }

    public static void Show(string content, bool is3DWorldObj = false)
    {
        if (!Instance.CanShowUI) return;
        if (is3DWorldObj && !Instance.CanShow3dWorldUI) return;

        Instance.Timer(Instance.delay, () =>
        {
            Instance.CurrentToolTip.SetText(content);
            Instance.CurrentToolTip.gameObject.SetActive(true);
        });
    }
    public static void Show(string content, string header, bool is3DWorldObj = false)
    {
        if (!Instance.CanShowUI) return;
        if (is3DWorldObj && !Instance.CanShow3dWorldUI) return;

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

