using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "ScriptableObjects/Tile Data")]
public class SOTileData : ScriptableObject
{
    
    public GameObject Prefab;
    public TileType TileType;
    public Sprite Icon;
    [Header("Price")]
    public bool IsTherePrice;
    public int BasePrice;
    public int PriceIncrease;
    [Header("Description")]
    public string Header;
    public string Description;
    
}

