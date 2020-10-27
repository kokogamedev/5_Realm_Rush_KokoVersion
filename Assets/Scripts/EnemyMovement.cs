using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    Transform enemyTransform;

    [SerializeField] float dwellTime = 1.25f;
    [SerializeField] List<Waypoint> path;

    // Start is called before the first frame update
    void Start()
    {
        Pathfinder pathfinder = FindObjectOfType<Pathfinder>();
        List<Waypoint>path = pathfinder.GetPath();
        StartCoroutine(FollowPath(path));
        //print("Hey I'm back at Start");
    }

    IEnumerator FollowPath(List<Waypoint> path)
    {
        //print("Starting patrol");
        
        foreach (Waypoint waypoint in path)
        {
            //print("Visiting block: " + waypoint.name);
            if (waypoint != path[0])
            {
                gameObject.transform.LookAt(waypoint.transform);
            }
            transform.position = waypoint.transform.position;
            enemyTransform = waypoint.transform;
            yield return new WaitForSeconds(dwellTime);
        }
        //print("Ending patrol");
    }

    public Transform GetEnemyTransform()
    {
        return enemyTransform;
    }
}
