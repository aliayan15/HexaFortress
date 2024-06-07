using HexaFortress.Game;
using HexaFortress.GamePlay;
using KBCore.Refs;
using Managers;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace HexaFortress.UI
{
    public class SelectTileButton : MonoBehaviour
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
        private bool _isPressed = false;
        private bool _isCurrentTileFree = false;


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
            if (tile.IsTherePrice && GameManager.Instance.DayCount != 1) // first day free
            {
                _isCurrentTileFree = false;
                price.SetActive(true);
                priceText.text = TileManager.Instance.GetPriceOfTile(tile.TileType, tile.BasePrice, tile.PriceIncrease).ToString();
            }
            else
            {
                _isCurrentTileFree = true;
                price.SetActive(false);
            }

            layoutElement.ignoreLayout = false;
            canvasItem.Toogle(true);
            if (_isPressed)
                UnsubscribeAction();
        }

        public void DeActivate()
        {
            canvasItem.Toogle(false);
            layoutElement.ignoreLayout = true;
            _myTile = null;
            _isPressed = false;
        }

        public void OnButtonPressed()
        {
            if (!_isCurrentTileFree)
            {
                TilePrice = TileManager.Instance.GetPriceOfTile(_myTile.TileType, _myTile.BasePrice, _myTile.PriceIncrease);
                if (Player.Instance.MyGold < TilePrice)
                {
                    Debug.Log("Not enough Gold!");
                    return;
                }
            }
            // player build
            if (_isPressed) return;
            Player.Instance.EnterBuildMode(_myTile);
            Player.Instance.OnTilePlaced += OnTilePlaced;
            Player.Instance.OnTileCanceled += OnTileCanceled;
            _isPressed = true;
            AudioManager.Instance.PlayBtnSound();
        }
        public void UpdatePrice()
        {
            if (!_myTile)
                return;
            priceText.text = TileManager.Instance.GetPriceOfTile(_myTile.TileType, _myTile.BasePrice, _myTile.PriceIncrease).ToString();
        }

        private void OnTileCanceled()
        {
            UnsubscribeAction();
        }

        private void OnTilePlaced()
        {
            if (!_myTile) return;
            if (!_isCurrentTileFree)
            {
                Player.Instance.AddGold(-TilePrice);
                TileManager.Instance.AddNewTile(_myTile.TileType);
            }

            UIManager.Instance.gameCanvasManager.UpdateTilePrice();
            UnsubscribeAction();
            DeActivate();
        }

        private void UnsubscribeAction()
        {
            if (!_isPressed) return;
            Player.Instance.OnTilePlaced -= OnTilePlaced;
            Player.Instance.OnTileCanceled -= OnTileCanceled;
            _isPressed = false;
        }

   
    }
}

