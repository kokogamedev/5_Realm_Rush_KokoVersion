using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonFiringSoundSystem : MonoBehaviour
{
    [SerializeField] ParticleSystem parentParticleSystem;
    int currentNumberOfParticles = 0;
    
    [SerializeField] AudioClip[] firingSound;
    [SerializeField] AudioClip[] impactSound;

    // Start is called before the first frame update
    void Start()
    {
        parentParticleSystem = gameObject.GetComponent<ParticleSystem>();
        if (parentParticleSystem = null)
        {
            Debug.LogError("Missing parent particle system!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        var amount = Mathf.Abs(currentNumberOfParticles - parentParticleSystem.particleCount);

        if(parentParticleSystem.particleCount > currentNumberOfParticles)
        {
            //StartCoroutine(PlaySound(firingSound[Random.Range(0, firingSound.Length)], amount));
        }
        //if(parentParticleSystem.collision.)

    }

    //IEnumerator PlaySound(AudioClip clip, int amount)
    //{

    //}
}
