using UnityEngine;
using UnityEngine.Events;

namespace HexaFortress.GamePlay
{
    [CreateAssetMenu(menuName = "UIEvents")]
    public class UIEvents : ScriptableObject
    {
        /// <summary>
        /// On player gold change.(gold)
        /// </summary>
        public UnityAction<int> OnGoldChange;
        /// <summary>
        /// On game day change.(dayCount)
        /// </summary>
        public UnityAction<int> OnDayChange;
        /// <summary>
        /// On tile count change.(remainingTileCount)
        /// </summary>
        public UnityAction<int> OnTileCountChange;
        /// <summary>
        /// OnGoldIncomeChange.(goldPerDay, expensesPerDay)
        /// </summary>
        public UnityAction<int,int> OnGoldIncomeChange;
        /// <summary>
        /// On Castle Repair amount change.
        /// </summary>
        public UnityAction OnCastleToolTipChage;
        /// <summary>
        /// Show night UI.(show-hide)
        /// </summary>
        public UnityAction<bool> ShowNightUI;
        /// <summary>
        /// Update night circle.
        /// </summary>
        public UnityAction<float> UpdateNightCircle;
    }
}