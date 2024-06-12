﻿using DG.Tweening;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    /// <summary>
    /// Grid item data.
    /// </summary>
    [System.Serializable]
    public class HexGridNode
    {
        public Vector3 Position { get; private set; }
        public TileBase MyTile { get; private set; }
        public int x { get; private set; } = 0;
        public int y { get; private set; } = 0;
        public bool CanBuildHere { get; private set; } = false;

        public int gCost;
        public int hCost;
        public int fCost;
        public HexGridNode cameFromNode;

        public bool IsWalkable { get; private set; } = false;

        private GameObject tilePlaceHolder = null;

        public HexGridNode(int x, int y, Vector3 position, GameObject tilePlaceHolder)
        {
            this.x = x;
            this.y = y;
            Position = position;
            this.tilePlaceHolder = tilePlaceHolder;
        }

        public void TurnChange(bool active)
        {
            ActivetePlaceHolder(active);
        }

        public void PlaceTile(TileBase tile)
        {
            MyTile = tile;
            ActivetePlaceHolder(false);
            CanBuildHere = false;
            GridManager.Instance.OnTurnChange -= TurnChange;
        }

        public void ActivetePlaceHolder(bool active)
        {
            if (MyTile != null && active) return;
            if (active && CanBuildHere) return;
            tilePlaceHolder.SetActive(active);
            if (active)
            {
                tilePlaceHolder.transform.DOKill();
                tilePlaceHolder.transform.localScale = Vector3.zero;
                tilePlaceHolder.transform.DOScale(1.95f, 0.4f);
            }
            CanBuildHere = active;
        }

        public void SetIsWalkable(bool isWalkable) => IsWalkable = isWalkable;

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        public override string ToString()
        {
            return "X:" + x + ", Y:" + y;
        }
    }
}

