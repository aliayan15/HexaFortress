using HexaFortress.GamePlay;
using MyUtilities;
using UnityEngine;

namespace HexaFortress.UI
{
    public class ToolTipSystem : SingletonMono<ToolTipSystem>
    {
        [SerializeField] private UIEvents events;
        public ToolTip CurrentToolTip;
        
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
            if (!Instance._canShowUI) return;
            if (is3DWorldObj && !Instance.CanShow3dWorldUI) return;

            Instance.Timer(Instance.delay, () =>
            {
                Instance.CurrentToolTip.SetText(content);
                Instance.CurrentToolTip.gameObject.SetActive(true);
            });
        }
        public static void Show(string content, string header, bool is3DWorldObj = false)
        {
            if (!Instance._canShowUI) return;
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

        private void SetCanShowUI(bool canShow)
        {
            _canShowUI = canShow;
            CanShow3dWorldUI = canShow;
        }
        private void SetCanShow3DWorldUI(bool canShow)
        {
            CanShow3dWorldUI = canShow;
        }

        private void OnEnable()
        {
            events.ShowToolTipUI += SetCanShowUI;
            events.Show3dWorldUI += SetCanShow3DWorldUI;
        }
        private void OnDisable()
        {
            events.ShowToolTipUI -= SetCanShowUI;
            events.Show3dWorldUI -= SetCanShow3DWorldUI;
        }
    }
}

