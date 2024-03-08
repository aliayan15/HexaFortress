using KBCore.Refs;
using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathTrace : SingletonMono<PathTrace>
{
    [SerializeField] private PathLine pathLine;

    private List<PathLine> lines = new List<PathLine>();

    public void DrawPath(bool draw)
    {
        if (draw)
            DrawPath();
        else
            ClearPath();
    }

    private void DrawPath()
    {
        HashSet<Vector3> linePoints = new HashSet<Vector3>();
        foreach (var point in TileManager.Instance.EnemySpawnPoints)
        {
            bool lastPointAdded = false;
            var path = GridManager.Instance.PathFinding.FindPath(point,
                        GridManager.Instance.PlayerCastle.MyHexNode.Position);
            if (path == null) continue;

            PathLine newLine = Instantiate(pathLine, Vector3.zero, pathLine.transform.rotation);
            newLine.Line.positionCount = path.Count;
            int positionCount = 0;
            for (int i = 0; i < path.Count; i++)
            {
                if (linePoints.Contains(path[i]))
                {
                    if (!lastPointAdded) lastPointAdded = true;
                    else continue;
                }
                Vector3 pos = path[i];
                pos.y = 0.3f;
                newLine.Line.SetPosition(i, pos);

                linePoints.Add(path[i]);
                positionCount++;
            }
            newLine.Line.positionCount = positionCount;
            lines.Add(newLine);
        }
    }
    private void ClearPath()
    {
        foreach (var item in lines)
        {
            Destroy(item.gameObject);
        }
        lines.Clear();
    }
}

