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
    [SerializeField] Text enemyHitPointsText;

    [SerializeField] int baseHitPoints = 5;
    public int scorePerHit = 10;
    public int oldBaseScore;
    public int recentMaxHits;
    [SerializeField] float scoreHitpointsFactor = 500f;
    [SerializeField] int fireRateUpgradeFactor = 2;
    [SerializeField] int towerLimitUpgradeFactor = 4;

    int playerLevel = 1;
    [SerializeField] Text playerLevelText;

    [SerializeField] AudioClip spawnSound;
    AudioSource audioSource;
    ScoreBoard scoreBoard;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        scoreBoard = FindObjectOfType<ScoreBoard>();
        
        StartCoroutine(Spawn());
        oldBaseScore = scorePerHit * baseHitPoints;
    }

    private IEnumerator Spawn()
    {
        while(enemies.Count <= enemyNumberLimit)
        {
            audioSource.PlayOneShot(spawnSound);
            EnemyMovement newEnemy = Instantiate(enemyInstance, transform.position, Quaternion.identity);
            newEnemy.transform.parent = gameObject.transform;
            ConfigureEnemyHitSettings(newEnemy);
            UpdateEnemyHitPointsText();
            LevelUpPlayer();
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
    }

    public void UpdateEnemyHitPointsText()
    {
        enemyHitPointsText.text = recentMaxHits.ToString();
    }

    public EnemyDamage[] GetEnemies()
    {
        return FindObjectsOfType<EnemyDamage>();
    }

    public void LevelUpTowers()
    {
        if (scoreBoard.score == fireRateUpgradeFactor * oldBaseScore)
        {
            FindObjectOfType<TowerFactory>().UpgradeTowerFiringRate();
            oldBaseScore = scoreBoard.score;
        }
        if (scoreBoard.score == towerLimitUpgradeFactor*oldBaseScore)
        {
            FindObjectOfType<TowerFactory>().UpgradeTowerLimit();
        }
    }

    private void LevelUpPlayer()
    {
        if ( (recentMaxHits - baseHitPoints)/2 % 1 == 0) // check if your recentMaxHits is divisible by baseHitPoints with no remainder
        {
            playerLevel = ((recentMaxHits - baseHitPoints)/2) + 1;
            playerLevelText.text = playerLevel.ToString();
        }
    }
}
