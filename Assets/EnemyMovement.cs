using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] List<Waypoint> path;
    // Start is called before the first frame update
    void Start()
    {
        PrintAllWaypoints();
    }

    private void PrintAllWaypoints()
    {
        foreach (Waypoint waypoint in path)
        {
            print(waypoint.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
