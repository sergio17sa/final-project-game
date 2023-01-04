using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : Singleton<PathFinding>
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private GridSystem<PathNode> _gridSystem;

    [SerializeField] Transform _debugTile;

    protected override void Awake()
    {
        base.Awake();

        
    }

    public void Setup(int width, int height, float cellSize)
    {
        _gridSystem = new GridSystem<PathNode>(width, height, cellSize,
            (GridSystem<PathNode> g, TilePosition tilePosition) => new PathNode(tilePosition));
    }

    public List<TilePosition> FindPath(TilePosition startTilePosition, TilePosition endTilePosition)
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
                pathNode.CalculteFCost();

                pathNode.ResetLastNode();
            }
        }

        startNode.GCost = 0;
        startNode.HCost = CalculateDistance(startTilePosition, endTilePosition);
        startNode.CalculteFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetlowstFCostNode(openList);

            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbours(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                int tentativeGCost = 
                    currentNode.GCost + CalculateDistance(currentNode.NodeTilePosition, neighbourNode.NodeTilePosition);

                if(tentativeGCost < neighbourNode.GCost)
                {
                    neighbourNode.LastNode = currentNode;
                    neighbourNode.GCost = tentativeGCost;
                    neighbourNode.HCost = CalculateDistance(neighbourNode.NodeTilePosition, endTilePosition);
                    neighbourNode.CalculteFCost();

                    if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);
                }

            }
        }

        //No path found
        return null;
    }

    /*public int CalculateDistance(TilePosition tilePositionA, TilePosition tilePositionB)
    {
        TilePosition tilePositionDistance = tilePositionA - tilePositionB;
        int distance = Mathf.Abs(tilePositionDistance.x) + Mathf.Abs(tilePositionDistance.z);
        return distance * MOVE_STRAIGHT_COST;
    }*/

    public int CalculateDistance(TilePosition tilePositionA, TilePosition tilePositionB)
    {
        TilePosition tilePositionDistance = tilePositionA - tilePositionB;

        int xDistance = Mathf.Abs(tilePositionDistance.x);
        int zDistance = Mathf.Abs(tilePositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
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

            if (tilePosition.z - 1 >= 0)
            {
                //left down
                neighbours.Add(GetNode(tilePosition.x - 1, tilePosition.z - 1));
            }

            if (tilePosition.z + 1 < _gridSystem.GetHeight())
            {
                //left up
                neighbours.Add(GetNode(tilePosition.x - 1, tilePosition.z + 1));
            }
        }

        if (tilePosition.x + 1 < _gridSystem.GetWidth())
        {
            //right node
            neighbours.Add(GetNode(tilePosition.x + 1, tilePosition.z));

            if (tilePosition.z - 1 >= 0)
            {
                //right down
                neighbours.Add(GetNode(tilePosition.x + 1, tilePosition.z - 1));
            }

            if (tilePosition.z + 1 < _gridSystem.GetHeight())
            {
                //right up
                neighbours.Add(GetNode(tilePosition.x + 1, tilePosition.z + 1));
            }
        }

        if(tilePosition.z -1 >= 0)
        {
            //down node
            neighbours.Add(GetNode(tilePosition.x, tilePosition.z - 1));
        }
        
        if(tilePosition.z + 1 < _gridSystem.GetHeight())
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

        while(currentNode.LastNode != null)
        {
            pathNodes.Add(currentNode.LastNode);
            currentNode = currentNode.LastNode;
        }

        pathNodes.Reverse();

        List<TilePosition> nodesTilePosition = new List<TilePosition>();

        foreach(PathNode pathNode in pathNodes)
        {
            nodesTilePosition.Add(pathNode.NodeTilePosition);
        }

        return nodesTilePosition;

    }

    private PathNode GetNode(int x, int z) => _gridSystem.GetTile(new TilePosition(x, z));

}
