using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] float dwellTime = 1f;
    Transform enemyTransform;
    // Start is called before the first frame update
    void Start()
    {
        Pathfinder pathfinder = FindObjectOfType<Pathfinder>();
        List<Waypoint> path = pathfinder.GetPath();
        StartCoroutine(FollowPath(path));
        print("Hey I'm back at Start");
    }

    IEnumerator FollowPath(List<Waypoint> path)
    {
        print("Starting patrol");
        
        foreach (Waypoint waypoint in path)
        {
            //print("Visiting block: " + waypoint.name);
            transform.position = waypoint.transform.position;
            yield return new WaitForSeconds(dwellTime);
        }
        print("Ending patrol");
    }

    // Update is called once per frame
    void Update()
    {
        enemyTransform = gameObject.transform;
    }

    public Transform GetEnemyTransform()
    {
        return enemyTransform;
    }
}
