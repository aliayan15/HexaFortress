using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private LayerMask gridPosLayer;
        [SerializeField] private GameObject pfUnwalkable;
        [SerializeField] private SOGameProperties gameData;

        private void Start()
        {
            GameManager.Instance.SetState(GameStates.GAME);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (RaycastTile(out HexGridNode grid))
                {
                    if (!grid.CanBuildHere) return;
                    var path = Instantiate(gameData.PathTile, grid.Position, Quaternion.identity);
                    path.Init(grid);
                }

            }
            if (Input.GetMouseButtonDown(1))
            {
                if (RaycastTile(out HexGridNode grid))
                {
                    if (grid.MyTile is not PathTile) return;
                    List<Vector3> pathList = GridManager.Instance.PathFinding.FindPath(grid.Position,
                        GridManager.Instance.PlayerCastle.MyHexNode.Position); ;
                    if (pathList == null) return;

                    for (int i = 0; i < pathList.Count - 1; i++)
                    {
                        pathList[i].Set(pathList[i].x, pathList[i].y + 0.1f, pathList[i].z);
                        Debug.DrawLine(pathList[i], pathList[i + 1], Color.green, 3f);
                    }
                }
            }
        }

        private bool RaycastTile(out HexGridNode grid)
        {
            grid = null;
            var ray = CameraManager.Instance.MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, gridPosLayer, QueryTriggerInteraction.Collide))
            {
                grid = GridManager.Instance.GetGridNode(hit.point);
                if (grid != null)
                {
                    return true;
                }
            }
            return false;
        }

        private void RotateTile(bool left = false)
        {
            var ray = CameraManager.Instance.MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, gridPosLayer, QueryTriggerInteraction.Collide))
            {
                var grid = GridManager.Instance.GetGridNode(hit.point);
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
