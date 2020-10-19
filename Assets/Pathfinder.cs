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


    // Start is called before the first frame update
    void Start()
    {
        LoadBlocks();
        ColorStartAndEnd();
        ExploreNeighbors();
    }

    private void ExploreNeighbors()
    {
        foreach (Vector2 direction in directions)
        {
            Vector2 explorationCoordinates = startWaypoint.GetGridPos() + direction;
            //print("Exploring " + explorationCoordinates);
            try
            {
                grid[explorationCoordinates].SetTopColor(Color.white);
            }
            catch
            {
                //do nothing
            }
        }
    }

    private void LoadBlocks()
    {
        var waypoints = FindObjectsOfType<Waypoint>();
        Color color;
        
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
            print("Loaded " + grid.Count + " blocks");
        }
    }

    private void ColorStartAndEnd()
    {
        startWaypoint.SetTopColor(Color.green);
        endWaypoint.SetTopColor(Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
