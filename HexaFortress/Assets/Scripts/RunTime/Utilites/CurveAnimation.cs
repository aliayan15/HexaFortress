using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurveAnimation : MonoBehaviour
{
    public AnimationCurve curve;
    private float _scale;
    private float _time;

    private void Start()
    {
        _scale = transform.localScale.y;
    }

    private void Update()
    {
        Vector3 scale = transform.localScale;
        scale.y = curve.Evaluate(_time) * _scale;
        _time += Time.deltaTime;
        transform.localScale = scale;
        if (_time > 1)
        {
            scale.y = _scale;
            transform.localScale = scale;
            Destroy(this);
        }

    }
}

