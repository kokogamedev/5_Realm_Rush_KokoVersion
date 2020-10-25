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
        if (enemyMovement != null)
        {
            objectToPan.LookAt(targetToOrientTowards.position);
        }
    }

    private void FireAtEnemy()
    {
        if (enemyMovement != null)
        {
            isWeaponActive = true;
            PlayFiringSFXWhenEmitting();

        }
        else
        {
            isWeaponActive = false;
        }
        weapon.SetActive(isWeaponActive);
    }

    private void PlayFiringSFXWhenEmitting()
    {
        if (weaponParticleEffect.isEmitting)
        {
            PlayFiringSFX();
        }
    }

    private void PlayFiringSFX()
    {
        if (!weaponAudioSource.isPlaying)
        {
            weaponAudioSource.PlayOneShot(firingSFX);
        }
    }
}
