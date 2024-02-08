using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtilities
{

    public static class HelperExtansions
    {
        /// <summary>
        /// Get an axis rotation value that editor show.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static float GetEditorRotation(this Transform target, SnapAxis axis = SnapAxis.X)
        {
            float Rotation = 0f;
            switch (axis)
            {
                case SnapAxis.None:
                    break;
                case SnapAxis.X:
                    if (target.eulerAngles.x <= 180f)
                    {
                        Rotation = target.eulerAngles.x;
                    }
                    else
                    {
                        Rotation = target.eulerAngles.x - 360f;
                    }
                    break;
                case SnapAxis.Y:
                    if (target.eulerAngles.y <= 180f)
                    {
                        Rotation = target.eulerAngles.y;
                    }
                    else
                    {
                        Rotation = target.eulerAngles.y - 360f;
                    }
                    break;
                case SnapAxis.Z:
                    if (target.eulerAngles.z <= 180f)
                    {
                        Rotation = target.eulerAngles.z;
                    }
                    else
                    {
                        Rotation = target.eulerAngles.z - 360f;
                    }
                    break;
                case SnapAxis.All:
                    break;
                default:
                    break;
            }

            return Rotation;
        }

        /// <summary>
        /// Get Vector3 rotation value that editor show.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector3 GetEditorRotation(this Transform target)
        {
            Vector3 Rotation = Vector3.zero;

            if (target.eulerAngles.x <= 180f)
            {
                Rotation.x = target.eulerAngles.x;
            }
            else
            {
                Rotation.x = target.eulerAngles.x - 360f;
            }

            if (target.eulerAngles.y <= 180f)
            {
                Rotation.y = target.eulerAngles.y;
            }
            else
            {
                Rotation.y = target.eulerAngles.y - 360f;
            }

            if (target.eulerAngles.z <= 180f)
            {
                Rotation.z = target.eulerAngles.z;
            }
            else
            {
                Rotation.z = target.eulerAngles.z - 360f;
            }

            return Rotation;
        }

        /// <summary>
        /// Reset all triggers.
        /// </summary>
        /// <param name="animator"></param>
        public static void ResetAllAnimatorTriggers(this Animator animator)
        {
            foreach (var trigger in animator.parameters)
            {
                if (trigger.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(trigger.name);
                }
            }
        }
    }
}
