using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : Singleton<PathFinding>
{
    private GridSystem<PathNode> _gridSystem;
    [SerializeField] private LayerMask obstaclesLayerMask;

    [SerializeField] Transform _debugTile;

    public void Setup(int width, int height, float cellSize)
    {
        _gridSystem = new GridSystem<PathNode>(width, height, cellSize,
            (GridSystem<PathNode> g, TilePosition tilePosition) => new PathNode(tilePosition));

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                TilePosition tilePosition = new TilePosition(x, z);
                Vector3 worldPosition = GridManager.Instance.GetWorldPosition(tilePosition);
                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(
                    worldPosition + Vector3.down * raycastOffsetDistance,
                    Vector3.up,
                    raycastOffsetDistance * 2,
                    obstaclesLayerMask))
                {
                    GetNode(x, z).IsWalkable = false;
                }
            }
        }

        //_gridSystem.CreateDebugTiles(_debugTile);
    }

    public List<TilePosition> FindPath(TilePosition startTilePosition, TilePosition endTilePosition, out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = _gridSystem.GetTile(startTilePosition);
        PathNode endNode = _gridSystem.GetTile(endTilePosition);

        openList.Add(startNode);

        for (int x = 0; x < _gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < _gridSystem.GetHeight(); z++)
            {
                TilePosition tilePosition = new TilePosition(x, z);
                PathNode pathNode = _gridSystem.GetTile(tilePosition);

                pathNode.GCost = int.MaxValue;
                pathNode.HCost = 0;
                pathNode.CalculateFCost();

                pathNode.ResetLastNode();
            }
        }

        startNode.GCost = 0;
        startNode.HCost = CalculateDistance(startTilePosition, endTilePosition);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetlowstFCostNode(openList);

            if (currentNode == endNode)
            {
                pathLength = endNode.FCost;
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbours(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                if (!neighbourNode.IsWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost =
                    currentNode.GCost + CalculateDistance(currentNode.NodeTilePosition, neighbourNode.NodeTilePosition);

                if (tentativeGCost < neighbourNode.GCost)
                {
                    neighbourNode.LastNode = currentNode;
                    neighbourNode.GCost = tentativeGCost;
                    neighbourNode.HCost = CalculateDistance(neighbourNode.NodeTilePosition, endTilePosition);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);
                }

            }
        }

        //No path found
        pathLength = 0;
        return null;
    }

    public int CalculateDistance(TilePosition tilePositionA, TilePosition tilePositionB)
    {
        int xDistance = Mathf.Abs(tilePositionA.x - tilePositionB.x);
        int zDistance = Mathf.Abs(tilePositionA.z - tilePositionB.z);
        int distance = (int)Mathf.Sqrt(xDistance * xDistance + zDistance * zDistance);
        return distance;
    }

    private PathNode GetlowstFCostNode(List<PathNode> pathNodes)
    {
        PathNode lowestFCostNode = pathNodes[0];

        for (int i = 0; i < pathNodes.Count; i++)
        {
            if (pathNodes[i].FCost < lowestFCostNode.FCost)
            {
                lowestFCostNode = pathNodes[i];
            }
        }

        return lowestFCostNode;
    }

    private List<PathNode> GetNeighbours(PathNode currentNode)
    {
        List<PathNode> neighbours = new List<PathNode>();

        TilePosition tilePosition = currentNode.NodeTilePosition;

        if (tilePosition.x - 1 >= 0)
        {
            //left node
            neighbours.Add(GetNode(tilePosition.x - 1, tilePosition.z));
        }

        if (tilePosition.x + 1 < _gridSystem.GetWidth())
        {
            //right node
            neighbours.Add(GetNode(tilePosition.x + 1, tilePosition.z));
        }

        if (tilePosition.z - 1 >= 0)
        {
            //down node
            neighbours.Add(GetNode(tilePosition.x, tilePosition.z - 1));
        }

        if (tilePosition.z + 1 < _gridSystem.GetHeight())
        {
            //up node
            neighbours.Add(GetNode(tilePosition.x, tilePosition.z + 1));
        }

        return neighbours;
    }

    private List<TilePosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodes = new List<PathNode>();

        pathNodes.Add(endNode);

        PathNode currentNode = endNode;

        while (currentNode.LastNode != null)
        {
            pathNodes.Add(currentNode.LastNode);
            currentNode = currentNode.LastNode;
        }

        pathNodes.Reverse();

        List<TilePosition> nodesTilePosition = new List<TilePosition>();

        foreach (PathNode pathNode in pathNodes)
        {
            nodesTilePosition.Add(pathNode.NodeTilePosition);
        }

        return nodesTilePosition;

    }

    private PathNode GetNode(int x, int z) => _gridSystem.GetTile(new TilePosition(x, z));

    public bool HasPath(TilePosition startTilePosition, TilePosition endTilePosition)
    {
        return FindPath(startTilePosition, endTilePosition, out int pathLength) != null;
    }

    public int GetPathLength(TilePosition startTilePosition, TilePosition endTilePosition)
    {
        FindPath(startTilePosition, endTilePosition, out int pathLength);
        return pathLength;
    }

    public bool IsWalkableTile(TilePosition tilePosition) => _gridSystem.GetTile(tilePosition).IsWalkable;

    public void SetIsWalkableTile(TilePosition tilePosition, bool isWalkable)
    {
        _gridSystem.GetTile(tilePosition).IsWalkable = isWalkable;
    }
}
