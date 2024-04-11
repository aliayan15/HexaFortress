using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI dec;
    [SerializeField] private TextMeshProUGUI level;

    private SOUpgradeData _myData;


    public void Init(SOUpgradeData data)
    {
        _myData = data;
    }
}

