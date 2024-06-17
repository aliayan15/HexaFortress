using System.Collections.Generic;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class HexPathFinding
    {
        public static HexPathFinding Instance { get; private set; }

        private Queue<HexGridNode> _openQueue;
        private HashSet<HexGridNode> _closedList;
        private GridManager _gridManager;

        public HexPathFinding(GridManager gridManager)
        {
            Instance = this;
            _gridManager = gridManager;
        }

        /// <summary>
        /// Return path vector from start to target or null.
        /// </summary>
        /// <param name="startWorldPosition"></param>
        /// <param name="endWorldPosition"></param>
        /// <returns></returns>
        public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            _gridManager.GetGridCoordinate(startWorldPosition, out int startX, out int startY);
            _gridManager.GetGridCoordinate(endWorldPosition, out int endX, out int endY);

            List<HexGridNode> path = FindPath(startX, startY, endX, endY);
            if (path == null)
            {
                return null;
            }
            else
            {
                List<Vector3> vectorPath = new List<Vector3>();
                foreach (HexGridNode pathNode in path)
                {
                    vectorPath.Add(pathNode.Position);
                }
                return vectorPath;
            }
        }

        public List<HexGridNode> FindPath(int startX, int startY, int endX, int endY)
        {
            HexGridNode startNode = _gridManager.GetGridNode(startX, startY);
            HexGridNode endNode = _gridManager.GetGridNode(endX, endY);

            if (startNode == null || endNode == null)
            {
                // Invalid Path
                return null;
            }

            _openQueue = new Queue<HexGridNode>();
            _closedList = new HashSet<HexGridNode>();

            _openQueue.Enqueue(startNode);
            startNode.cameFromNode = null;

            while (_openQueue.Count > 0)
            {
                HexGridNode currentNode = _openQueue.Dequeue();
                if (currentNode == endNode)
                {
                    // Reached final node
                    return CalculatePath(endNode);
                }

                _closedList.Add(currentNode);

                foreach (HexGridNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if (_closedList.Contains(neighbourNode) || !neighbourNode.IsWalkable)
                    {
                        continue;
                    }

                    if (!_openQueue.Contains(neighbourNode))
                    {
                        neighbourNode.cameFromNode = currentNode;
                        _openQueue.Enqueue(neighbourNode);
                    }
                }
            }

            // Out of nodes on the openQueue
            return null;
        }

        private List<HexGridNode> GetNeighbourList(HexGridNode currentNode)
        {
            List<HexGridNode> neighbourList = new List<HexGridNode>();
            // get path tile
            PathTile pathTile = currentNode.MyTile as PathTile;
            if (pathTile == null) return neighbourList;

            foreach (var point in pathTile.ConnectionPoints)
            {
                neighbourList.Add(_gridManager.GetGridNode(point.position));
            }

            return neighbourList;
        }

        public HexGridNode GetNode(int x, int y)
        {
            return _gridManager.GetGridNode(x, y);
        }

        private List<HexGridNode> CalculatePath(HexGridNode endNode)
        {
            List<HexGridNode> path = new List<HexGridNode>();
            path.Add(endNode);
            HexGridNode currentNode = endNode;
            while (currentNode.cameFromNode != null)
            {
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }
    }
}
