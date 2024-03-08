using Shapes;
using UnityEngine;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Line bar;
    private float _endX;
    private Transform _mTransform;

    private void Start()
    {
        _mTransform = GetComponent<Transform>();
        _endX = bar.End.x;
    }

    public void UpdateBar(float healthRatio)
    {
        Vector3 end = Vector3.zero;
        end.x = healthRatio * _endX;
        bar.End = end;
    }

    public void LookCamera()
    {
        _mTransform.LookAt(_mTransform.position + CameraManager.Instance.CamPosition.rotation * Vector3.forward,
        CameraManager.Instance.CamPosition.rotation * Vector3.up);
    }
}

