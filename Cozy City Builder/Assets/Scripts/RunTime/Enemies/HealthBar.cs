using Shapes;
using UnityEngine;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Line barH;
    [SerializeField] private Line barA;
    private float _endX;
    private Transform _mTransform;
    private float _total;

   
    public void Init(float total)
    {
        _mTransform = GetComponent<Transform>();
        _endX = barH.End.x;
        _total = total;
    }

    /// <summary>
    /// Ratio 0-1
    /// </summary>
    /// <param name="healthRatio"></param>
    /// <param name="armorRatio"></param>
    public void UpdateBar(float healthRatio, float armorRatio)
    {
        armorRatio = (healthRatio + armorRatio) / _total;
        healthRatio = healthRatio / _total;

        Vector3 end = Vector3.zero;
        end.x = healthRatio * _endX;
        barH.End = end;
        end.x = armorRatio * _endX;
        barA.End = end;
    }

    public void LookCamera()
    {
        _mTransform.LookAt(_mTransform.position + CameraManager.Instance.CamPosition.rotation * Vector3.forward,
        CameraManager.Instance.CamPosition.rotation * Vector3.up);
    }
}

