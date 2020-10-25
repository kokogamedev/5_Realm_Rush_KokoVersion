using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] AudioClip impactSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] GameObject deathFX;

    [SerializeField] int hits = 5;
    int scorePerHit = 10;
    ScoreBoard scoreBoard;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
