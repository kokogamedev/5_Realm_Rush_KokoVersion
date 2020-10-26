using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    //Parameters (of each tower)
    [SerializeField] Transform objectToPan; //Initialize the child object your are going to rotate to face your target
    [SerializeField] GameObject weapon; //Initialize the weapon game object that contains the particle system for firing bullets, impacting your target, etc (this is essentially your bullet firing particle system)
    [SerializeField] AudioClip firingSFX; //Initialize the bullet firing SFX 

    //State (of each tower)
    Transform targetEnemy; //Initialize the target towards whom you are going to orient (and then fire upon)
    Transform targetEnemyChild; //Initialize the transform of the target towards whom who are going to orient - this will be the child object of our actual targetEnemy due to symantics 
    ParticleSystem weaponParticleEffect; //Initialize the component particle system of your weapon game object (this is the bullet firing particle system)
    AudioSource weaponAudioSource; //Initialize the audiosource component of your weapon game object

    bool isWeaponActive = false; //Initialize the boolean that indicates whether or not your weapon game object is active (whether your bullets should be firing)
    float firingRange = 50f; //Initialize a variable for tower's firing range - the furthest the target before tower cannot fire

    // Start is called before the first frame update
    void Start()
    {
        // todo: this might need some refinemnent when multiple enemies show up... could cause a problme of finding multiple objects of this type
        weaponParticleEffect = weapon.GetComponent<ParticleSystem>(); //At start, extract weapon game object's particle system component and set it to equal our initialized weaponParticleEffect variable
        weaponAudioSource = weapon.GetComponent<AudioSource>(); //At start, extract weapon game object's audiosource component and set it to equal our initialized weaponAudioSource variable
    }

    // Update is called once per frame
    void Update()
    {
        SetTargetEnemy();
        PanAndFireIfInRange();
    }

    private void SetTargetEnemy()
    {
        var sceneEnemies = FindObjectsOfType<EnemyDamage>(); // Get the collection of enemies
        if (sceneEnemies.Length < 1) { return; } // check if there are any items in the enemy collection (are there any enemies spawned?)

        Transform closestEnemy = sceneEnemies[0].transform;// Assume the first is the “winner”

        foreach (EnemyDamage testEnemy in sceneEnemies) // For each item in collection
        {
            closestEnemy = GetClosestEnemy(closestEnemy, testEnemy.transform);
        }
        targetEnemy = closestEnemy; // Return the winner
    }

    private Transform GetClosestEnemy(Transform currentClosest, Transform newPotentialClosest)
    {
        float distanceToCurrentClosest = Vector3.Distance(transform.position, currentClosest.position);
        float distanceToNewPotentialClosest = Vector3.Distance(transform.position, newPotentialClosest.position);

        if (distanceToNewPotentialClosest - distanceToCurrentClosest < Mathf.Epsilon)
        {
            currentClosest = newPotentialClosest; // Update winner (if first is not winner, change winner; else keep first winner)
        }

        return currentClosest;

    }

    private void PanAndFireIfInRange() //Pans and fires at enemy if target is withing range established by firingRange
    {
        //idea: make tower miss slightly sometimes according to an accuracy chance variable?
        //idea: make tower delay slightly when orienting - movement-time requirements?

        if (targetEnemy == null) { FiringStatusInactive(); return; }

        targetEnemyChild = targetEnemy.Find("Enemy_A"); //necessary since transform of parent Enemy game object is not the same as the transform of the actual enemy in the scene
        //todo: find a way to align the transforms of the parent game object and the target enemy
        float rangeToTarget = Vector3.Distance(targetEnemyChild.position, transform.position); //this variable represents the current range of your tower to the target
        if (rangeToTarget <= firingRange) //tests if your current range to Target within tower's firing range AND  whether an enemy (more specifically the EnemyMovement script attached to it) is present in the scene
        {
            PanTowardEnemy(); //orient your pivoting child game objects (the head of your tower and the bullet-firing particle system) to face the target location
            FiringStatusActive();
        }
        else
        {
            FiringStatusInactive();
            //print("Target out of range");
        }

    }

    private void PanTowardEnemy() //this method is responsible for pivoting child game objects (the head of your tower and the bullet-firing particle system) to face the target location
    {
        objectToPan = transform.Find("Pivot"); //this is the game object parent of all your pivoting game objects
        objectToPan.LookAt(targetEnemyChild.position); //rotates your game object to the Vector3 position of your target
    }

    private void FiringStatusInactive()
    {
        isWeaponActive = false;
        weapon.SetActive(isWeaponActive); // set your bullet-firing particle system to be inactive (fire your bullets) at the target if isWeaponActive is false
    }

    private void FiringStatusActive()
    {
        PlayFiringSFXWhenEmitting(); //// this method sets your bullet-firing particle system to be active (fire your bullets) when a particle is emitting
        isWeaponActive = true; //sets the status of your weapon-active status to true
        weapon.SetActive(isWeaponActive); // set your bullet-firing particle system to be active (fire your bullets) at the target if isWeaponActive is true
    }

    private void PlayFiringSFXWhenEmitting() // this method sets your bullet-firing particle system to be active (fire your bullets) at the target when it is present in the scene 
    {
        if (weaponParticleEffect.isEmitting) //this lines tests whether a particle has been emitted from the particle system weaponParticleEffect of your weapon game object
        {
            PlayFiringSFX(); //plays a single clip of your firingSFX weapon-firing sound effect iff a clip is not already playing
        }
    }

    private void PlayFiringSFX() //plays a single clip of your firingSFX weapon-firing sound effect iff a clip is not already playing
    {
        if (!weaponAudioSource.isPlaying) //tests whether your not your clip is already playing
        {
            weaponAudioSource.PlayOneShot(firingSFX); //plays a single clip of your firingSFX weapon-firing sound effect
        }
    }
}
