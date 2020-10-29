using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField][Range(0.1f,120f)] float secondsBetweenSpawns = 5f;
    [SerializeField] int enemyNumberLimit = 5;
    [SerializeField] EnemyMovement enemyInstance;
    
    public List<EnemyMovement> enemies = new List<EnemyMovement>();
    int currentEnemyCount; //number of enemies currently in the scene at any given time
    public int numEnemiesSpawned = 0; //todo: potentially for later use - lets you know how many enemies have been spawned in this wave
    [SerializeField] Text enemyCountText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while(enemies.Count <= enemyNumberLimit)
        {
            EnemyMovement newEnemy = Instantiate(enemyInstance, transform.position, Quaternion.identity);
            newEnemy.transform.parent = gameObject.transform;
            UpdateEnemyCountText();
            numEnemiesSpawned++;
            yield return new WaitForSeconds(secondsBetweenSpawns);
        }
    }

    public void UpdateEnemyCountText()
    {
        currentEnemyCount = GetEnemies().Length;
        enemyCountText.text = currentEnemyCount.ToString();
        print(currentEnemyCount + " enemies present");
    }

    public EnemyDamage[] GetEnemies()
    {
        return FindObjectsOfType<EnemyDamage>();
    }
}
