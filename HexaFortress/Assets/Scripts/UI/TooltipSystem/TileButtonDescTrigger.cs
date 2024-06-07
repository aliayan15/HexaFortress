using KBCore.Refs;
using UnityEngine;

namespace HexaFortress.UI
{
    [RequireComponent(typeof(SelectTileButton))]
    public class TileButtonDescTrigger : ToolTipTrigger
    {
        [SerializeField, Self] SelectTileButton tileButton;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        protected override void Show()
        {
            if (tileButton.MyTile == null) return;
            ToolTipSystem.Show(tileButton.MyTile.Description, tileButton.MyTile.Header);
        }
   
    }
}

