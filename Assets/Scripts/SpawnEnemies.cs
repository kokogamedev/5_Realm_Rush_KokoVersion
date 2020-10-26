using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] float secondsBetweenSpawns = 5f;
    [SerializeField] int enemyNumberLimit = 5;
    [SerializeField] EnemyMovement EnemyInstance;
    public List<EnemyMovement> enemies = new List<EnemyMovement>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while(enemies.Count <= enemyNumberLimit)
        {
            EnemyMovement newEnemy = Instantiate(EnemyInstance, transform.parent);
            enemies.Add(newEnemy);
            newEnemy.transform.parent = gameObject.transform;
            yield return new WaitForSeconds(secondsBetweenSpawns);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
