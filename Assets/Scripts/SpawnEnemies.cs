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

    [SerializeField] int baseHitPoints = 5;
    public int oldBaseHitPoints;
    public int recentMaxHits;
    [SerializeField] float scoreHitpointsFactor = 500f;

    [SerializeField] AudioClip spawnSound;
    AudioSource audioSource;
    ScoreBoard scoreBoard;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        scoreBoard = FindObjectOfType<ScoreBoard>();
        StartCoroutine(Spawn());
        oldBaseHitPoints = baseHitPoints;
    }

    private IEnumerator Spawn()
    {
        while(enemies.Count <= enemyNumberLimit)
        {
            audioSource.PlayOneShot(spawnSound);
            EnemyMovement newEnemy = Instantiate(enemyInstance, transform.position, Quaternion.identity);
            newEnemy.transform.parent = gameObject.transform;
            ConfigureEnemyHitSettings(newEnemy);
            LevelUpTowers();
            UpdateEnemyCountText();
            numEnemiesSpawned++;
            yield return new WaitForSeconds(secondsBetweenSpawns);
        }
    }

    private void ConfigureEnemyHitSettings(EnemyMovement newEnemy)
    {
        newEnemy.GetComponent<EnemyDamage>().currentHits = SetEnemyHitPoints();
        newEnemy.GetComponent<EnemyDamage>().maxHits = SetEnemyHitPoints();
        recentMaxHits = SetEnemyHitPoints();
    }

    private int SetEnemyHitPoints()
    {
        return Mathf.RoundToInt(baseHitPoints + scoreBoard.score / scoreHitpointsFactor);
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

    private void LevelUpTowers()
    {
        if (recentMaxHits - oldBaseHitPoints == oldBaseHitPoints)
        {
            FindObjectOfType<TowerFactory>().UpgradeTowerFiringRate();
            oldBaseHitPoints = recentMaxHits;
        }
    }
}
