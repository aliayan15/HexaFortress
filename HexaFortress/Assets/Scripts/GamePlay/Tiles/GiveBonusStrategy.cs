using UnityEngine;
using UnityEngine.Serialization;

namespace HexaFortress.GamePlay
{
    [CreateAssetMenu(menuName = "ScriptableObject/GiveBonus Strategy")]
    public class GiveBonusStrategy : TileStrategy
    {
        [SerializeField] TileType bonusTileType = TileType.Sheep;

        public override void Init(HexGridNode myNode, TileBase tile)
        {
            var surroundingTiles = GridManager.Instance.GetSurroundingGrids(myNode);
            foreach (var surroundingTile in surroundingTiles)
            {
                if (!surroundingTile.MyTile)
                    continue;

                if (surroundingTile.MyTile.MyType == bonusTileType)
                {
                    if (surroundingTile.MyTile.TryGetComponent(out TileBase tileBonusEffect))
                        tileBonusEffect.DoBonusEffect();
                }
            }
        }

        public override void DoBonusEffect()
        {
        }
    }
}