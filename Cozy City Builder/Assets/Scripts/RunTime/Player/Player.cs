using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private LayerMask gridPosLayer;

        private GridManager _gridManager;

        private void Start()
        {
            _gridManager = GameManager.Instance.gridManager;
            GameManager.Instance.SetState(GameStates.GAME);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RotateTile();
            }
            if (Input.GetMouseButtonDown(1))
            {
                RotateTile(true);
            }
        }

        private void RotateTile(bool left = false)
        {
            var ray = CameraManager.Instance.MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, gridPosLayer, QueryTriggerInteraction.Collide))
            {
                var grid = _gridManager.GetGridItem(hit.point);
                if (grid != null)
                {
                    if (grid.MyTile)
                    {
                        grid.MyTile.Rotate(left);
                    }
                }

            }
        }
    }
}
