using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Dictionary<Vector2, Waypoint> grid = new Dictionary<Vector2, Waypoint>();
    [SerializeField] Waypoint startWaypoint, endWaypoint;
    Waypoint waypoint;
    Vector2[] directions = { Vector2.up,
                            Vector2.right,
                            Vector2.down,
                            Vector2.left };

    Queue<Waypoint> queue = new Queue<Waypoint>();
    bool isRunning = true;
    Waypoint searchCenter; //current search center

    // Start is called before the first frame update
    void Start()
    {
        LoadBlocks();
        ColorStartAndEnd();
        Pathfind();
        //ExploreNeighbors();
    }

    private void Pathfind()
    {
        // en-queue the start waypoint
        queue.Enqueue(startWaypoint);

        while (queue.Count > 0 && isRunning) // while the queue has items
        {
            searchCenter = queue.Dequeue(); //de-queue the frontier waypoint
            searchCenter.isExplored = true; // mark frontier as explored
            print("Searching from" + searchCenter.name);
            HaltIfEndisSearchCenter(); //if search center is the end waypoint, stop algorithm
            ExploreNeighbors();//for each direction from frontier, queue new unexplored waypoint 
        }
        //todo: work out path
        print("Finished Pathfinding?");
    }

    private void HaltIfEndisSearchCenter()
    {
        if (searchCenter == endWaypoint)
        {
            //print("Searching from end node, therefore stopping");
            isRunning = false;
        }
    }

    private void ExploreNeighbors()
    {
        if (!isRunning) { return; }
        foreach (Vector2 direction in directions)
        {
            Vector2 neighborCoordinates = searchCenter.GetGridPos() + direction;
            //print("Exploring " + explorationCoordinates);
            try
            {
                QueueNewNeighbors(neighborCoordinates);
            }
            catch
            {
                //do nothing
            }
        }
    }

    private void QueueNewNeighbors(Vector2 neighborCoordinates)
    {
        Waypoint neighbor = grid[neighborCoordinates];

        if (neighbor.isExplored == false && queue.Contains(neighbor) == false)
        {
            queue.Enqueue(neighbor);
            //print("Queueing" + neighbor);
            neighbor.exploredFrom = searchCenter;
        }
    }

    private void LoadBlocks()
    {
        var waypoints = FindObjectsOfType<Waypoint>();

        foreach(Waypoint waypoint in waypoints)
        {
            var gridPos = waypoint.GetGridPos();

            if (grid.ContainsKey(gridPos))
            {
                Debug.LogWarning("Skipping overlapping block" + waypoint);
            }
            else
            {
                grid.Add(gridPos, waypoint);
            }
            //print("Loaded " + grid.Count + " blocks");
        }
    }

    private void ColorStartAndEnd()
    {
        startWaypoint.SetTopColor(Color.green);
        endWaypoint.SetTopColor(Color.red);

        startWaypoint.isStartorEnd = true;
        endWaypoint.isStartorEnd = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
