using HexaFortress.Game;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public enum TileType
    {
        None,
        Path,
        House,
        Forest,
        Water,
        Tower,
        Castle,
        Windmill,
        Mountain,
        FishHouse,
        Fielts,
        Grass,
        FishTile,
        Market,
        TowerUpgrade,
        CastleRepair,
        CastleHealthUpgrade,
        Sheep,
        Cannon,
        Mortar
    }


    public abstract class TileBase : MonoBehaviour
    {
        [SerializeField] protected TileType tileType;

        public TileType MyType => tileType;
        public int CurrentRos { get; set; } = 0;
        public HexGridNode MyHexNode => _myHexNode;

        protected HexGridNode _myHexNode;


        public void Rotate(bool left = false)
        {
            float rosY = 0;
            if (left)
            {
                rosY = SOGameProperties.GetPreviousRotation(CurrentRos, out int index);
                CurrentRos = index;
            }
            else
            {
                rosY = SOGameProperties.GetNextRotation(CurrentRos, out int index);
                CurrentRos = index;
            }

            transform.rotation = Quaternion.Euler(0, rosY, 0);
        }

        /// <summary>
        /// Invoking after placing the tile.
        /// </summary>
        /// <param name="surroundingGrids"></param>
        public virtual void Init(HexGridNode myNode)
        {
            myNode.PlaceTile(this);
            _myHexNode = myNode;
            CheckSurroundingGrids();
        }

        protected virtual void CheckSurroundingGrids()
        {
            var surroundingGrids = GridManager.Instance.GetSurroundingGrids(_myHexNode);
            foreach (var grid in surroundingGrids)
            {
                grid.ActivetePlaceHolder(true);
                GridManager.Instance.OnTurnChange += grid.TurnChange;
            }
        }

        public virtual bool CanBuildHere(HexGridNode grid)
        {
            return true;
        }

        public virtual void OnPlayerHand(bool onHand)
        {
        }
        public virtual void OnPlayerInsert(bool onHand, HexGridNode myNode)
        {
        }

        protected virtual void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            
        }
        
        protected virtual void OnEnable()
        {
            EventManager.AddListener<TurnStateChangeEvent>(OnTurnStateChange);
        }
        protected virtual void OnDisable()
        {
            EventManager.RemoveListener<TurnStateChangeEvent>(OnTurnStateChange);
        }
    }
}