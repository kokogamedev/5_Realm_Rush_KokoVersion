using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    //This class will contain a circular/ring buffer for placing towers and limiting the number of towers present at any given time

    [SerializeField] Tower towerPrefab; //Tower Prefab
    [SerializeField] Transform towerParent; 
    [SerializeField] int towerLimit = 5;

    int towerCount = 0;

    public void AddTower(Waypoint baseWaypoint)
    {
        if (towerCount < towerLimit)
        {
            InstantiateNewTower(baseWaypoint);
            towerCount++;
        }
        else
        {
            MoveExistingTower();
        }

    }

    private static void MoveExistingTower()
    {
        Debug.Log("Tower limit reached");
        //todo: actually move towers!
    }

    private void InstantiateNewTower(Waypoint baseWaypoint)
    {
        Debug.Log("Place tower at " + baseWaypoint.gameObject.name);
        GameObject towerInstance = Instantiate(towerPrefab.gameObject, baseWaypoint.transform.position, Quaternion.identity);
        towerInstance.transform.parent = towerParent;
        baseWaypoint.isPlaceable = false;
    }
}
