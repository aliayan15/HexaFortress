using System;
using UnityEngine;

public class SimpleTimer : MonoBehaviour
{
    private static GameObject _parent;
    private static GameObject _parentAlwaysOn;
    private float _targetTimeStart = 0f;
    private float _targetTime = 0f;
    private Action _endAction;
    private bool _stopped = false;
    private bool _doNotDestroyOnEnd;
    private bool _autoRestart;

    void Update()
    {
        if (_stopped)
        {
            return;
        }

        if (_targetTime <= 0)
        {
            return;
        }

        _targetTime -= Time.deltaTime;

        if (_targetTime <= 0.0f)
        {
            try
            {
                _endAction();
            }
            catch (Exception)
            {
                // ignored
            }

            if (_autoRestart)
            {
                if (!_stopped)
                {
                    Restart();
                }

                return;
            }

            if (_doNotDestroyOnEnd)
            {
                return;
            }

            Destroy(this);
        }
    }

    public void StartTimer(float targetTime, Action endAction, bool doNotDestroyOnEnd = false, bool autoRestart = false)
    {
        _stopped = false;
        _targetTime = targetTime;
        _targetTimeStart = targetTime;
        _endAction = endAction;
        _doNotDestroyOnEnd = doNotDestroyOnEnd;
        _autoRestart = autoRestart;
    }
    /// <summary>
    /// Sayacý yeniden baþlat.
    /// </summary>
    public void Restart()
    {
        _stopped = false;
        _targetTime = _targetTimeStart;
    }
    /// <summary>
    /// sayacý durdur ve sýfýrla.
    /// </summary>
    public void Stop()
    {
        _stopped = true;
        _targetTime = 0;
        Destroy(this);
    }
    /// <summary>
    /// Sayýmý durdur.
    /// </summary>
    public void Pause()
    {
        _stopped = true;
    }
    /// <summary>
    /// Saymaya devam et.
    /// </summary>
    public void Continue()
    {
        _stopped = false;
    }

    public void SetTimer(float targetTime)
    {
        _targetTime = targetTime;
    }

    /// <summary>
    /// Gecikme Oluþturmak için kullanýlýr.Ýkinci parametrede kodlar yazýlýr.
    /// </summary>
    /// <param name="targetTime"></param>
    /// <param name="endAction"></param>
    /// <param name="doNotDestroyOnEnd"></param>
    /// <param name="autoRestart"></param>
    /// <param name="doNotDestroyOnLoadNewScene"></param>
    /// <returns></returns>    
    public static SimpleTimer Create(float targetTime, Action endAction, bool doNotDestroyOnEnd = false, bool autoRestart = false, bool doNotDestroyOnLoadNewScene = false)
    {
        GameObject parentObj;
        if (doNotDestroyOnLoadNewScene)
        {
            if (!_parentAlwaysOn)
            {
                _parentAlwaysOn = new GameObject("Simple Timer (DoNotDestroy)");
                DontDestroyOnLoad(_parentAlwaysOn);
            }

            parentObj = _parentAlwaysOn;
        }
        else
        {
            if (!_parent)
            {
                _parent = new GameObject("Simple Timer");
            }

            parentObj = _parent;
        }

        var timerComponent = parentObj.AddComponent<SimpleTimer>();
        timerComponent.StartTimer(targetTime, endAction, doNotDestroyOnEnd, autoRestart);
        return timerComponent;
    }
}
