using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform objectToPan;
    [SerializeField] Transform targetToOrientTowards;
    EnemyMovement enemyMovement;

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = FindObjectOfType<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        PanTowardEnemy();
    }

    private void PanTowardEnemy()
    {
        objectToPan = transform.Find("Pivot");
        targetToOrientTowards = enemyMovement.GetEnemyTransform();
        objectToPan.LookAt(targetToOrientTowards);
    }
}
