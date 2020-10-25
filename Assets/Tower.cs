using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform objectToPan;
    [SerializeField] Transform targetToOrientTowards;
    [SerializeField] GameObject weapon;
    ParticleSystem weaponParticleEffect;
    AudioSource weaponAudioSource;
    EnemyMovement enemyMovement;

    [SerializeField] AudioClip firingSFX;
    AudioSource audioSource;

    bool isWeaponActive = false;

    int currentNumberOfParticles = 0;



    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = FindObjectOfType<EnemyMovement>();
        audioSource = GetComponent<AudioSource>();
        weaponParticleEffect = weapon.GetComponent<ParticleSystem>();
        weaponAudioSource = weapon.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PanTowardEnemy();
        FireAtEnemy();
    }

    private void PanTowardEnemy()
    {
        objectToPan = transform.Find("Pivot");
        //targetToOrientTowards = enemyMovement.transform;
        if (enemyMovement != null)
        {
            objectToPan.LookAt(targetToOrientTowards.position);
            
            //weaponParticleEffect.Play();
        }
        else
        {
            
            //weaponParticleEffect.Stop();
        }
    }

    private void FireAtEnemy()
    {
        if (enemyMovement != null)
        {
            isWeaponActive = true;
            //PlayWeaponParticleFX();
            //PlayFiringSounds();
            if (weaponParticleEffect.isEmitting)
            {
                //weaponAudioSource.Stop();
                if (weaponAudioSource.isPlaying == false)
                {
                    weaponAudioSource.PlayOneShot(firingSFX);
                }
            }
            
        }
        else
        {
            isWeaponActive = false;
            //StopWeaponParticleFX();
        }
        weapon.SetActive(isWeaponActive);
    }

    //private void PlayFiringSounds()
    //{
    //    var amount = Mathf.Abs(currentNumberOfParticles - weaponParticleEffect.particleCount);

    //    if (weaponParticleEffect.particleCount > currentNumberOfParticles)
    //    {
    //        //StartCoroutine(PlaySound(firingSFX, amount));
    //        audioSource.PlayOneShot(firingSFX);
    //    }

    //    currentNumberOfParticles = weaponParticleEffect.particleCount;
    //}

    //private IEnumerator PlaySound(AudioClip firingSFX, int amount)
    //{
    //    throw new NotImplementedException();
    //}

    //private void StopWeaponParticleFX()
    //{
    //    if (weaponParticleEffect.isPlaying)
    //    {
    //        //weaponParticleEffect.Stop();
    //        weapon.SetActive(false);
    //    }
    //}

    //private void PlayWeaponParticleFX()
    //{
    //    if (!weaponParticleEffect.isPlaying)
    //    {
    //        //weaponParticleEffect.Play();
    //        weapon.SetActive(true);
    //    }
    //}


}
