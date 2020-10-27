using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    AudioSource audioSource; //initializing the audiosource from which we will play sound effects

    [SerializeField] AudioClip impactSound; //initializing the audioclip for weapon impacts on enemy
    [SerializeField] AudioClip deathSound; //initializing the audioclip for death/explosion sounds for enemy
    //[SerializeField] ParticleSystem deathFX; //initializing the particle system for enemy death (explosion) --- part of an attempted way to instantiate and play this particle system (see associated todo in KillEnemy()
    [SerializeField] GameObject deathFX;

    //todo: create a dynamic change of hits and scoreperhit as game goes on, also potentially find a way to adjust enemy appearance as its hitpoints increase
    [SerializeField] int hits = 5; //initializing number of hits required for enemy death
    int scorePerHit = 10; //initializing the points/score you receive for every weapon impact on enemy
    ScoreBoard scoreBoard; //initializing the Scoreboard script located on the scoreboard - allows us to update/extract from said script

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //At start, extract audiosource component and set it to equal our initialized audioSource variable
        scoreBoard = FindObjectOfType<ScoreBoard>(); //At start, find an object in our scene of type Scoreboard (find the Scoreboard script and its associated scoreboard game object)
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject other) //This method processes collisions of a particle system's particles on your game object - this method will handle bullet impacts on your game object
    {
        ProcessHit();
        if (hits < 1) //destroys enemy and initiates explosion particle and sound FX if hitpoints is below 1
        {
            KillEnemy();
        }
        else //plays the impactSound SFX (particle effect handled within Unity editor - a subsystem particle explosion effect was added to the main particle system for whenever a collision occurs)
        {
            PlayHitNoise();
        }
    }

    private void ProcessHit() //this method subtracts  from your gameObject's health/hitpoints, and updates the score on the scoreboard
    {
        hits--; 
        scoreBoard.ScoreHit(scorePerHit);

        //todo: consider hit effects --- I used subemitters, but Rick used a nested ParticleSystem called hitParticlePrefab, which he added through [SerializeField] member variable, and then utilized the following line
        //hitParticlePrefab.Play();
    }

    private void KillEnemy() //instantiates a self-destroying explosion particle system at location of game object (and no rotation), and destroys your game object
    {

        GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        //fx.transform.parent = parent;

        // Important to destroy particle effect after instantiation -- set Stop Action to Destroy in Particle system settings in Unity inspector!

        //Following code was attempting another method used by Rick (pre-video-check... this was my attempt), but for some reason this is not working... 
        // The idea is to disable PlayOnAwake in the particle system prefab, [SerializeField] the particle system of deathFX rather than the game object, and then command that it play after instantiating the deathFX.gameObject
        // The fault is in deathFX.Play()... or at least in how I am using it with some Unity editor particle system setting... no solution at this time.
        // todo: find a solution to why this did not work
        //GameObject fx = Instantiate(deathFX.gameObject, transform.position, Quaternion.identity);
        //if (deathFX.isPlaying == false && deathFX.isEmitting == false)
        //{
        //    deathFX.Play();
        //}

        Destroy(gameObject);
    }

    private void PlayHitNoise() //plays one shot of your impactSound SFX - no consideration for overlapping sounds in this case
    {
        audioSource.PlayOneShot(impactSound);//Play the audio you attach to the AudioSource component
    }
}
