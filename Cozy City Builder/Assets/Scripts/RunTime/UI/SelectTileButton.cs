using KBCore.Refs;
using Managers;
using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SelectTileButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, Self] private CanvasGrupItem canvasItem;
    [SerializeField, Self] private LayoutElement layoutElement;
    [Header("Refs")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject price;
    public SOTileData MyTile => _myTile;
    public int TilePrice { get; private set; }

    private SOTileData _myTile;

    private void OnValidate()
    {
        this.ValidateRefs();
    }

    private void Start()
    {
        DeActivate();
    }

    public void SetTile(SOTileData tile)
    {
        _myTile = tile;
        iconImage.sprite = tile.Icon;
        if (tile.IsTherePrice)
        {
            price.SetActive(true);
            priceText.text = TileManager.Instance.GetPriceOfTile(tile.TileType, tile.BasePrice, tile.PriceIncrease) + " g";
        }
        else
            price.SetActive(false);

        layoutElement.ignoreLayout = false;
        canvasItem.Toogle(true);
    }

    public void DeActivate()
    {
        canvasItem.Toogle(false);
        layoutElement.ignoreLayout = true;
        _myTile = null;
    }

    public void OnButtonPressed()
    {
        if (_myTile.IsTherePrice)
        {
            TilePrice = TileManager.Instance.GetPriceOfTile(_myTile.TileType, _myTile.BasePrice, _myTile.PriceIncrease);
            if (GameManager.Instance.player.MyGold < TilePrice)
            {
                Debug.Log("Not enough Gold!");
                return;
            }
        }
        // player build
        GameManager.Instance.player.EnterBuildMode(_myTile);
        GameManager.Instance.player.OnTilePlaced += OnTilePlaced;
    }

    private void OnTilePlaced()
    {
        if (_myTile.IsTherePrice)
            GameManager.Instance.player.AddGold(-TilePrice);
        DeActivate();
        GameManager.Instance.player.OnTilePlaced -= OnTilePlaced;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}

