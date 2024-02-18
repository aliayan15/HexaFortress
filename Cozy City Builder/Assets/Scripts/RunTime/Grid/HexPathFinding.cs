using System.Collections.Generic;
using UnityEngine;


public class HexPathFinding
{

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static HexPathFinding Instance { get; private set; }

    private List<HexGridNode> _openList;
    private List<HexGridNode> _closedList;
    private Vector2 _gridSize;
    private GridManager _gridManager;

    public HexPathFinding(GridManager gridManager, Vector2 gridSize)
    {
        Instance = this;
        _gridManager = gridManager;
        _gridSize = gridSize;
    }

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

        _openList = new List<HexGridNode> { startNode };
        _closedList = new List<HexGridNode>();

        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                HexGridNode pathNode = _gridManager.GetGridNode(x, y);
                pathNode.gCost = 99999999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (_openList.Count > 0)
        {
            HexGridNode currentNode = GetLowestFCostNode(_openList);
            if (currentNode == endNode)
            {
                // Reached final node
                return CalculatePath(endNode);
            }

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            foreach (HexGridNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (_closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.IsWalkable)
                {
                    _closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!_openList.Contains(neighbourNode))
                    {
                        _openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of nodes on the openList
        return null;
    }

    private List<HexGridNode> GetNeighbourList(HexGridNode currentNode)
    {
        List<HexGridNode> neighbourList = new List<HexGridNode>();

        bool oddRow = currentNode.y % 2 == 1;

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
        }
        if (currentNode.x + 1 < _gridSize.x)
        {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
        }
        if (currentNode.y - 1 >= 0)
        {
            // Down
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        }
        if (currentNode.y + 1 < _gridSize.y)
        {
            // Up
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        }

        if (oddRow)
        {
            if (currentNode.y + 1 < _gridSize.y && currentNode.x + 1 < _gridSize.x)
            {
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
            }
            if (currentNode.y - 1 >= 0 && currentNode.x + 1 < _gridSize.x)
            {
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            }
        }
        else
        {
            if (currentNode.y + 1 < _gridSize.y && currentNode.x - 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
            }
            if (currentNode.y - 1 >= 0 && currentNode.x - 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            }
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

    private int CalculateDistanceCost(HexGridNode a, HexGridNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_STRAIGHT_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private HexGridNode GetLowestFCostNode(List<HexGridNode> pathNodeList)
    {
        HexGridNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
}

