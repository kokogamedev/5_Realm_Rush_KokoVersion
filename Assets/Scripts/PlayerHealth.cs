using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    EnemyDamage currentEnemyDamage;
    Transform collidingEnemy;

    [SerializeField] float baseHealth = 100;
    [SerializeField] Text healthText;
    // Start is called before the first frame update
    void Start()
    {
        //var enemyArray = infoFromTowers.GetEnemies();
        healthText.text = baseHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        currentEnemyDamage = IdentifyEnemy(other);
        DamageBase();
    }

    private EnemyDamage IdentifyEnemy(Collider enemyCollider)
    {
        collidingEnemy = enemyCollider.transform.parent; //find the parent of the collider's transform (this is because the collider belongs to the child of the game object to which EnemyDamage is attached)
        return collidingEnemy.GetComponent<EnemyDamage>();
    }

    private void DamageBase()
    {
        if (currentEnemyDamage != null)
        {
            float enemyAssaultHitValue = currentEnemyDamage.GetEnemyAssaultHitValue();
            baseHealth -= enemyAssaultHitValue;
        }
        healthText.text = baseHealth.ToString();
    }

    public float GetBaseHealth()
    {
        return baseHealth;
    }
}
