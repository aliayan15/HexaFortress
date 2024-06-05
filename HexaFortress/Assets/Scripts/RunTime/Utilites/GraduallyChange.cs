using System;
using System.Threading.Tasks;

// Git link:https://github.com/fisekoo/GraduallyChange   ***Dosya ucretli paketler arasinda
//How to use: GraduallyChange.To( () => startValue, x => startValue = x, targetValue, duration, isSmooth, OnChangeComplete );

public static class GraduallyChange
{
    // TODO: STOP ALL TASKS
    public static Task To(System.Func<float> from, System.Action<float> callback, float to, float duration, bool isSmooth = false)
    {
        return ToTask(() => from(), callback, to, duration, isSmooth);
    }
    public static Task To(System.Action<float> callback, UnityEngine.AnimationCurve curve, int loopCount = 1)
    {
        return ToTask(callback, curve, loopCount);
    }
    /// <summary>Gradually changes a given value to target value.</summary>
    /// <param name="from">Start value.</param>
    /// <param name="callback">Lambda for assigning gradually changing value to a variable.</param>
    /// <param name="to">Target value.</param>
    /// <param name="duration">How long it will take start value to reach target value? In seconds.</param>
    /// <param name="isSmooth">Should interpolation be smooth?</param>
    private static async Task ToTask(System.Func<float> from, System.Action<float> callback, float to, float duration, bool isSmooth = false)
    {
        CheckDurationIsLessThanZero(duration);

        var t = 0f;
        var current = from();
        var fromValue = from();
        while (current != to)
        {
            t += UnityEngine.Time.fixedUnscaledDeltaTime;
            current = isSmooth ? LerpSmooth(fromValue, to, t / duration) : Lerp(fromValue, to, (t / duration));
            if (current.IsSimiliarTo(to))
            {
                callback(to);
                break;
            }
            callback(current);
            await WaitForFixedUpdate();
        }
    }
    /// <summary>Gradually changes a given value to last animation curve keyframe value.</summary>
    /// <param name="callback">Lambda for assigning gradually changing value to a variable.</param>
    /// <param name="curve">Animation curve</param>
    /// <param name="loopCount">How many times the change will loop.</param>
    public static async Task ToTask(System.Action<float> callback, UnityEngine.AnimationCurve curve, int loopCount = 1)
    {
        var lastKeyTime = curve[curve.length - 1].time;
        CheckDurationIsLessThanZero(lastKeyTime);

        var t = 0f;
        var current = 0f;
        var reverse = false;
        var targetTime = lastKeyTime;

        // when the loop count is even, the loop would end at the start position.
        // to avoid that, we are checking if it is even or odd.
        // if even, we are doing one extra repeat for ending at end position.
        for (int i = loopCount % 2 == 0 ? -1 : 0; i < loopCount; i++)
        {
            while (true)
            {
                t = reverse ? t - UnityEngine.Time.fixedUnscaledDeltaTime : t + UnityEngine.Time.fixedUnscaledDeltaTime;
                current = LerpSmooth(curve.Evaluate(0), curve.Evaluate(t), t / lastKeyTime);
                if (t.IsSimiliarTo(targetTime))
                {
                    callback(curve.Evaluate(targetTime));
                    break;
                }
                callback(current);
                await WaitForFixedUpdate();
            }
            reverse = !reverse;
            targetTime = Max(lastKeyTime - t, 0);
        }
    }
    public static async Task WaitForFixedUpdate()
    {
        await Task.Delay((int)(UnityEngine.Time.fixedUnscaledDeltaTime * 1000));
    }
    public static async void OnComplete(this Task task, System.Action action)
    {
        await task;
        action.Invoke();
    }
    /// <summary>Checks if a and b similiar to eachother by delta.</summary>
    /// <param name="delta">Minimum difference between numbers.</param>
    /// <returns>True if numbers are similiar</returns>
    public static bool IsSimiliarTo(this float a, float b, float delta = 0.01f) => System.Math.Abs(b - a) <= delta;

    /// <summary>Lerps between a and b</summary>
    /// <param name="a">The start value, when t is 0</param>
    /// <param name="b">The start value, when t is 1</param>
    /// <param name="t">The t-value from 0 to 1 representing position along the lerp, clamped between 0 and 1</param>
    public static float Lerp(float a, float b, float t) => (1 - t) * a + t * b;

    /// <summary>Lerps between a and b, applying trigonometeric smoothing to the t-value</summary>
    /// <param name="a">The start value, when t is 0</param>
    /// <param name="b">The start value, when t is 1</param>
    /// <param name="t">The t-value from 0 to 1 representing position along the lerp, clamped between 0 and 1</param>
    public static float LerpSmooth(float a, float b, float t) => Lerp(a, b, SmoothCos01(t));

    /// <summary>Applies trigonometric smoothing to the 0-1 interval.</summary>
    public static float SmoothCos01(float x) => (float)System.Math.Cos(x * System.Math.PI) * -0.5f + 0.5f;

    /// <summary>Clamps value between min and max.</summary>
    /// <param name="value">The value to clamp</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value</param>
    public static float Clamp(float value, float min, float max) => value < min ? min : value > max ? max : value;

    /// <summary>Returns the absoulete value of val.</summary>
    public static float Abs(float val) => val < 0 ? val * -1 : val;

    /// <summary>Returns the biggest one of the parameters.</summary>
    public static float Max(float val1, float val2) => val1 < val2 ? val2 : val1;
    private static void CheckDurationIsLessThanZero(float duration)
    {
        if (duration <= 0) throw new DurationIsLessThanZeroException("Duration must be greater than zero");
    }
}

[System.Serializable]
public class DurationIsLessThanZeroException : System.Exception
{
    public DurationIsLessThanZeroException(string message) : base(message) { }
}