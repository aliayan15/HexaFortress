using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class DefaultTile : TileBase
    {
        [SerializeField] private ParticleSystem initPartical;

        public override void Init(HexGridNode myNode)
        {
            base.Init(myNode);
            if (initPartical)
                initPartical.Play();
        }
    }
}