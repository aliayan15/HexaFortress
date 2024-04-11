using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class UpgradeManager : SingletonMono<UpgradeManager>
{
    [SerializeField] private SOUpgradeData[] upgrades;

    public List<ITowerUpgradeable> BacisTowers;
    public List<ITowerUpgradeable> CannonTowers;
    public List<ITowerUpgradeable> MortarTowers;

    private Dictionary<SOUpgradeData, int> _upgrades = new Dictionary<SOUpgradeData, int>();

    public void Upgrade(SOUpgradeData data)
    {
        switch (data.TileType)
        {
            case TileType.Cannon:
                UpgradeCannonTower(data.Upgrade);
                break;
            case TileType.Mortar:
                UpgradeMortarTower(data.Upgrade);
                break;
            case TileType.Tower:
                UpgradeBasicTower(data.Upgrade);
                break;
        }
        if (!_upgrades.ContainsKey(data))
        {
            _upgrades.Add(data, 1);
        }
        else
        {
            int current = _upgrades[data];
            _upgrades[data] = current + 1;
        }
        Debug.Log(data.TileType + ": " + _upgrades[data]);
    }

    private void UpgradeBasicTower(UpgradeType type)
    {
        foreach (var item in BacisTowers)
        {
            switch (type)
            {
                case UpgradeType.HealthDamage:
                    item.UpgradeHealthDamage();
                    break;
                case UpgradeType.ArmorDamage:
                    item.UpgradeArmorDamage();
                    break;
                case UpgradeType.Slow:
                    item.UpgradeSlowEffect();
                    break;
                case UpgradeType.Crit:
                    item.UpgradeCritEffect();
                    break;
                default:
                    break;
            }
        }
    }
    private void UpgradeMortarTower(UpgradeType type)
    {
        foreach (var item in MortarTowers)
        {
            switch (type)
            {
                case UpgradeType.HealthDamage:
                    item.UpgradeHealthDamage();
                    break;
                case UpgradeType.ArmorDamage:
                    item.UpgradeArmorDamage();
                    break;
                case UpgradeType.Slow:
                    item.UpgradeSlowEffect();
                    break;
                case UpgradeType.Crit:
                    item.UpgradeCritEffect();
                    break;
                default:
                    break;
            }
        }
    }
    private void UpgradeCannonTower(UpgradeType type)
    {
        foreach (var item in CannonTowers)
        {
            switch (type)
            {
                case UpgradeType.HealthDamage:
                    item.UpgradeHealthDamage();
                    break;
                case UpgradeType.ArmorDamage:
                    item.UpgradeArmorDamage();
                    break;
                case UpgradeType.Slow:
                    item.UpgradeSlowEffect();
                    break;
                case UpgradeType.Crit:
                    item.UpgradeCritEffect();
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Return 3 different upgrade
    /// </summary>
    /// <returns></returns>
    public SOUpgradeData[] GetRandomUpgrade()
    {
        SOUpgradeData[] datas = new SOUpgradeData[3];
        var dataList = upgrades.ToList();
        datas[0] = dataList[Random.Range(0, dataList.Count)];
        dataList.Remove(datas[0]);
        datas[1] = dataList[Random.Range(0, dataList.Count)];
        dataList.Remove(datas[1]);
        datas[2] = dataList[Random.Range(0, dataList.Count)];
        dataList.Remove(datas[2]);
        return datas;
    }

}

public enum UpgradeType
{
    None,
    HealthDamage,
    ArmorDamage,
    Slow,
    Crit
}

