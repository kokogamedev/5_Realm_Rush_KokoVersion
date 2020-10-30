using System;
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
    GameObject fxParent;

    //todo: create a dynamic change of hits and scoreperhit as game goes on, also potentially find a way to adjust enemy appearance as its hitpoints increase
    public int currentHits; //initializing number of hits required for enemy death
    public int maxHits;
    [SerializeField] float scoreHitpointsFactor = 500f;
    int scorePerHit = 10; //initializing the points/score you receive for every weapon impact on enemy
    ScoreBoard scoreBoard; //initializing the Scoreboard script located on the scoreboard - allows us to update/extract from said script

    SpawnEnemies enemySpawner;

    public GameObject baseHitfx;
    [SerializeField] float enemyAssaultHitValue = 5f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //At start, extract audiosource component and set it to equal our initialized audioSource variable
        scoreBoard = FindObjectOfType<ScoreBoard>(); //At start, find an object in our scene of type Scoreboard (find the Scoreboard script and its associated scoreboard game object)
        enemySpawner = FindObjectOfType<SpawnEnemies>();
        fxParent = GameObject.Find("TempFX");

        //todo: game not finding TempFX game object
    }

    void OnParticleCollision(GameObject other) //This method processes collisions of a particle system's particles on your game object - this method will handle bullet impacts on your game object
    {
        ProcessHit();
        if (currentHits < 1) //destroys enemy and initiates explosion particle and sound FX if hitpoints is below 1
        {
            KillEnemy();
        }
        else //plays the impactSound SFX (particle effect handled within Unity editor - a subsystem particle explosion effect was added to the main particle system for whenever a collision occurs)
        {
            PlayHitNoise();
        }
    }

    public GameObject GetfxParent()
    {
        return fxParent;
    }

    private void ProcessHit() //this method subtracts  from your gameObject's health/hitpoints, and updates the score on the scoreboard
    {
        currentHits--; 
        scoreBoard.ScoreHit(scorePerHit);

        //todo: consider hit effects --- I used subemitters, but Rick used a nested ParticleSystem called hitParticlePrefab, which he added through [SerializeField] member variable, and then utilized the following line
        //hitParticlePrefab.Play();
    }

    private void KillEnemy() //instantiates a self-destroying explosion particle system at location of game object (and no rotation), and destroys your game object
    {
        PlayDeathFX();

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

        // DESTROYING VFX THROUGH CODE INSTEAD OF UNITY EDITOR ---->
        // IF your particle effect was not self-destroying, you can destroy it here in code, BUT there is an issue -> if you destroy the game object, you destroy the vfx and the whole script along with it
        // SO you must wait for the vfx to play before you can destroy your game object 
        // HOW you do it is the with the following code:
        // float destroyDelay = deathFX.main.duration; //duration of the particle effect
        // Destroy(fx, destroyDelay);
        // Destroy(gameObject);

        // PLAYING AN AUDIOCLIP AT A POINT WITHOUT NEEDING TO WORRY ABOUT IF ITS ASSOCIATED GAME OBJECT IS AROUND TO BE THE AUDIOSOURCE
        // unexpected issue: deathSound too quiet to be heard. 
        // method to debug and fix this:
        // Step 1: Use Debug.Break() to compare enemy death SFX to enemy hit SFX
        // Step 2: Where in our scene are 2D sounds heard --> answer is the camera! 
        // Step 3: solution is to put the audio source right next to the audio listener, our camera
            //FAILED ATTEMPT: AudioSource.PlayClipAtPoint(deathSound, transform.position);
                // Debug.Break() // allow us to see properties of audio source in the inspector of our temporary PlayOneShot audio
                                 // notice that the spatial blend is 3D and set at 1
                                        // spatial blend -> how much the 3D engine has on audio source
                                            // distance from audiosource to camera in combo with 3D spatial blend setting is causing audio to be dropped off before it reaches the camera
            // SUCCESSFUL ATTEMPT: AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);

        Destroy(gameObject);
        enemySpawner.UpdateEnemyCountText();

    }

    private void PlayDeathFX()
    {
        GameObject fx = Instantiate(deathFX, transform.Find("Enemy_A").position, Quaternion.identity);
        fx.transform.parent = fxParent.transform.parent;
    }

    private void PlayHitNoise() //plays one shot of your impactSound SFX - no consideration for overlapping sounds in this case
    {
        audioSource.PlayOneShot(impactSound);//Play the audio you attach to the AudioSource component
    }

    public float GetEnemyAssaultHitValue()
    {
        return enemyAssaultHitValue;
    }
}
