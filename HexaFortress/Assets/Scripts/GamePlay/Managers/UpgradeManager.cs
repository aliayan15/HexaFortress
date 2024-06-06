using Managers;
using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class UpgradeManager : SingletonMono<UpgradeManager>
{
    [SerializeField] private SOUpgradeData[] upgrades;
    [SerializeField] private int[] upgradeDays;

    public List<ITowerUpgradeable> BacisTowers = new List<ITowerUpgradeable>();
    public List<ITowerUpgradeable> CannonTowers = new List<ITowerUpgradeable>();
    public List<ITowerUpgradeable> MortarTowers = new List<ITowerUpgradeable>();

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
        //Debug.Log(data.TileType + ": " + _upgrades[data]);
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
        // remove locked towers upgrades
        for (int i = 0; i < dataList.Count; i++)
        {
            bool isUnlocked = TileManager.Instance.GetTileCount(dataList[i].TileType) > 0;

            if (!isUnlocked && dataList.Count > 3)
            {
                dataList.RemoveAt(i);
                i--;
            }
        }
        //Debug.Log("Towers count:" + TileSelector.Instance.Towers.Count);
        dataList.Shuffle();
        datas[0] = dataList[Random.Range(0, dataList.Count)];
        dataList.Remove(datas[0]);
        datas[1] = dataList[Random.Range(0, dataList.Count)];
        dataList.Remove(datas[1]);
        datas[2] = dataList[Random.Range(0, dataList.Count)];
        return datas;
    }

    public int GetUpgradeLevel(SOUpgradeData data)
    {
        if (!_upgrades.ContainsKey(data))
        {
            return 1;
        }
        return _upgrades[data] + 1;
    }

    #region State Change
    private void OnTurnStateChange(TurnStates state)
    {
        if (state == TurnStates.TurnBegin)
        {
            // days to select upgrade
            if (upgradeDays.Any(d => d == GameManager.Instance.DayCount))
            {
                this.Timer(1f, () =>
                {
                    // TODO send event
                    //UIManager.Instance.gameCanvasManager.ShowUpgrades(true);
                });
            }
        }
    }


    private void OnEnable()
    {
        GameManager.OnTurnStateChange += OnTurnStateChange;
    }
    private void OnDisable()
    {
        GameManager.OnTurnStateChange -= OnTurnStateChange;
    }
    #endregion
}

public enum UpgradeType
{
    None,
    HealthDamage,
    ArmorDamage,
    Slow,
    Crit
}

