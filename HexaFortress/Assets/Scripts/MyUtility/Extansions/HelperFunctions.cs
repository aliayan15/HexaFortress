using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyUtilities
{

    public static class HelperFunctions
    {

        #region Extension to easily create WaitForSeconds coroutines / Delay
        /// <summary>
        /// Create WaitForSeconds coroutine and Call action with delay.
        /// </summary>
        /// <param name="mono">GameObject That coroutine have</param>
        /// <param name="delay"></param>
        /// <param name="action"></param>
        /// <param name="isRealTime">WaitForSecondsRealtime or WaitForSeconds</param>
        /// <returns></returns>
        public static Coroutine Timer(this MonoBehaviour mono, float delay, UnityAction action, bool isRealTime = false)
        {
            if (isRealTime)
                return mono.StartCoroutine(ExecuteActionInRealtime(delay, action));
            else
                return mono.StartCoroutine(ExecuteActionInScaledTime(delay, action));
        }

        private static IEnumerator ExecuteActionInRealtime(float delay, UnityAction action)
        {
            yield return new WaitForSecondsRealtime(delay);
            action?.Invoke();
            yield break;
        }
        private static IEnumerator ExecuteActionInScaledTime(float delay, UnityAction action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
            yield break;
        }
        #endregion

        /// <summary>
        /// Get anchored Position from world position.
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="canvasRect"></param>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public static Vector2 GetAnchoredPosition(Camera cam, RectTransform canvasRect, Vector3 worldPos)
        {
            Vector2 ViewportPosition = cam.WorldToViewportPoint(worldPos);
            Vector2 anchoredPosition = new Vector2(
            ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
            return anchoredPosition;
        }

        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                (ts[i], ts[r]) = (ts[r], ts[i]);
            }
        }
    }
}
