using HexaFortress.Game;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public abstract class TileStrategy : ScriptableObject
    {
        public abstract void Init(HexGridNode myNode,TileBase tile);
        public abstract void DoBonusEffect();

        public virtual void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            
        }
    }
}