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
    List<Waypoint> path = new List<Waypoint>();

    public List<Waypoint> GetPath()
    {
        
        if (path.Count < 1) 
        {
            CalculatePath();
        }

        return path;
    }

    private void CalculatePath()
    {
        LoadBlocks();
        BreadthFirstSearch();
        CreatePath();
    }

    private void CreatePath()
    {
        //Start at end waypoint and add it to the path list (also set isPlaceable for this waypoint to false)
        SetAsPath(endWaypoint);

        //initialize previous variable that contains the Waypoint from which the current Waypoint was explored (i.e. the Waypoint exploredFrom variable).  In this case, the current waypoint is the endWaypoint
        Waypoint previous = endWaypoint.exploredFrom;

        //Make a loop that continues as long as the previous waypoint is not equal to the start waypoint 
        while (previous != startWaypoint)
        {
            //Add the previous waypoint to the path list within the while loop (also set isPlaceable for this waypoint to false)
            //NOTE: this line must be before the following line, else you will double count the startWaypoint and miss the second-to-last waypoint
            //the prior potential error is known as a OFF-BY-ONE ERROR
            SetAsPath(previous);

            //Reinitialize the previous waypoint to the exploredFrom variable of the latest previous waypoint (that has already been added to the list)
            previous = previous.exploredFrom;
        }

        //Outside the loop, add the startWaypoint to the end of the list (also set isPlaceable for this waypoint to false)
        SetAsPath(startWaypoint);

        //Reverse the list so it is in the order that the Enemy will follow
        path.Reverse();
    }

    private void SetAsPath(Waypoint waypoint)
    {
        //Set isPlaceable = false for any waypoint on the path
        waypoint.isPlaceable = false;
        path.Add(waypoint); //add waypoint to the path
    }

    private void BreadthFirstSearch()
    {
        // en-queue the start waypoint
        queue.Enqueue(startWaypoint);

        while (queue.Count > 0 && isRunning) // while the queue has items
        {
            searchCenter = queue.Dequeue(); //de-queue the frontier waypoint
            searchCenter.isExplored = true; // mark frontier as explored
            //print("Searching from" + searchCenter.name);
            HaltIfEndisSearchCenter(); //if search center is the end waypoint, stop algorithm
            ExploreNeighbors();//for each direction from frontier, queue new unexplored waypoint 
        }
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
            if (grid.ContainsKey(neighborCoordinates))
            {
                QueueNewNeighbors(neighborCoordinates);
            }
        }
    }

    private void QueueNewNeighbors(Vector2 neighborCoordinates)
    {
        Waypoint neighbor = grid[neighborCoordinates];

        if (neighbor.isExplored == false && queue.Contains(neighbor) == false && neighbor.IsItNeutral() == false)
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
}
