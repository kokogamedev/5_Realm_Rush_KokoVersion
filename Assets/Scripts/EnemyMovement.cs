using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] float dwellTime = 4f;
    Transform enemyTransform;
    [SerializeField] GameObject deathFX;
    [SerializeField] int hits = 5;
    int scorePerHit = 10;
    ScoreBoard scoreBoard;

    AudioSource audioSource;

    [SerializeField] AudioClip impactSound;
    [SerializeField] AudioClip deathSound;


    // Start is called before the first frame update
    void Start()
    {
        Pathfinder pathfinder = FindObjectOfType<Pathfinder>();
        audioSource = GetComponent<AudioSource>();
        List<Waypoint> path = pathfinder.GetPath();
        StartCoroutine(FollowPath(path));
        print("Hey I'm back at Start");
        scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    IEnumerator FollowPath(List<Waypoint> path)
    {
        print("Starting patrol");
        
        foreach (Waypoint waypoint in path)
        {
            //print("Visiting block: " + waypoint.name);
            transform.position = waypoint.transform.position;
            enemyTransform = waypoint.transform;
            yield return new WaitForSeconds(dwellTime);
        }
        print("Ending patrol");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Transform GetEnemyTransform()
    {
        return enemyTransform;
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        if (hits < 1)
        {
            KillEnemy();
        }
        else
        {
            PlayHitNoise();
        }
    }

    private void ProcessHit()
    {
        hits--;
        scoreBoard.ScoreHit(scorePerHit);
        
        //todo: consider hit effects
    }

    private void KillEnemy()
    {
        GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        //fx.transform.parent = parent;
        Destroy(gameObject);
    }

    private void PlayHitNoise()
    {
            audioSource.PlayOneShot(impactSound);//Play the audio you attach to the AudioSource component
    }
}
