using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    Transform enemyTransform;

    [SerializeField] float dwellTimeFactor = 1.25f;
    [SerializeField] float minDwellTime = 0.5f;
    [SerializeField] [Range(5f,50f)] float hitFactor = 5f;
    SpawnEnemies enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        Pathfinder pathfinder = FindObjectOfType<Pathfinder>();
        List<Waypoint>path = pathfinder.GetPath();
        StartCoroutine(FollowPath(path));
        //print("Hey I'm back at Start");
        enemySpawner = FindObjectOfType<SpawnEnemies>();
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
            yield return new WaitForSeconds(DwellIncreaseWithHit());
        }

        //Base Hit Explosion happens here
        EnemyHitsBase();
    }

    private void EnemyHitsBase()
    {
        BaseHitFX();
        Destroy(gameObject);
        enemySpawner.UpdateEnemyCountText();
    }

    private void BaseHitFX()
    {
        GameObject baseHitfx = Instantiate(this.GetComponent<EnemyDamage>().baseHitfx, transform.Find("Enemy_A").position, Quaternion.identity);
        baseHitfx.transform.parent = this.GetComponent<EnemyDamage>().GetfxParent().transform.parent;
    }

    public Transform GetEnemyTransform()
    {
        return enemyTransform;
    }

    public float DwellIncreaseWithHit()
    {
        EnemyDamage enemyDamage = GetComponent<EnemyDamage>();
        float currentDwellTime = (enemyDamage.currentHits/enemyDamage.maxHits)*dwellTimeFactor + minDwellTime;
        return currentDwellTime;
    }
}
