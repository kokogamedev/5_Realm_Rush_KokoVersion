using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    //This class will contain a circular/ring buffer for placing towers and limiting the number of towers present at any given time

    [SerializeField] Tower towerPrefab; //Tower Prefab
    [SerializeField] Transform towerParent;
    [SerializeField] int towerLimit = 5;

    //Create empty queue of towers
    Queue<Tower> towerQueue = new Queue<Tower>();

    int towerCount = 0;

    SpawnEnemies enemySpawner;


    public void AddTower(Waypoint baseWaypoint)
    {
        //update tower count
        towerCount = towerQueue.Count;

        if (towerCount < towerLimit)
        {
            InstantiateNewTower(baseWaypoint);
        }
        else
        {
            MoveExistingTower(baseWaypoint);
        }

    }

    private void InstantiateNewTower(Waypoint baseWaypoint)
    {
        // This method is responsible for placing a new tower when the tower limit has not yet been reached
        // In particular, this method does the following: (1) Instantiates a tower towerInstance at waypoint baseWaypoint; (2) sets the gameObject towerInstance to have a new parent towerParent;
        // (3) Extracts the tower script component from towerInstance under variable currentTower; (4) sets the currentTower baseWaypoint to be the input waypoint variable baseWaypoint of this method;
        // (5) sets the waypoint on which the currentTower is instantiated as no longer a placeable block (isPlaceable = false); (6) adds currentTower to the top of the queue

        Debug.Log("Place tower at " + baseWaypoint.gameObject.name);
        GameObject towerInstance = Instantiate(towerPrefab.gameObject, baseWaypoint.transform.position, Quaternion.identity);
        towerInstance.transform.parent = towerParent;

        Tower currentTower = towerInstance.GetComponent<Tower>();
        currentTower.baseWaypoint = baseWaypoint; //set baseWaypoints
        baseWaypoint.isPlaceable = false; //set placeable flags

        towerQueue.Enqueue(currentTower); //enqueue tower
    }

    private void MoveExistingTower(Waypoint newBaseWaypoint)
    {
        // This method is responsible for moving the oldest tower (tower at the bottom of the queue) to the specified baseWaypoint when the tower limit has been reached
        // In particular, this method does the following: (1) Extracts into tower variable oldestTower and dequeues the tower at the bottom of the queue; (2) sets oldestTower's 
        // baseWaypoint to once again be a placeable block as the tower will soon be moved from that location (isPlaceable = true) (3) sets the newBaseWaypoint (the input newBaseWaypoint)
        // to which the oldestTower will be moved to no longer be a placeable block (isPlaceable = false); (4) changes the oldestTower's baseWaypoint to be the 
        // input waypoint variable newBaseWaypoint of this method; (5) Moves the oldestTower to the location of the input newBaseWaypoint by setting its transform.position to be that 
        // of the input newBaseWaypoint; (6) adds oldestTower to the top of the queue

        Tower oldestTower = towerQueue.Dequeue(); //dequeue oldest tower (from bottom of queue)

        //Set placeable flags
        oldestTower.baseWaypoint.isPlaceable = true; //set oldest tower's current block as placeable again (since we will soon move the tower from this location)
        newBaseWaypoint.isPlaceable = false; //set the input baseWaypoint to which the tower will soon be moved as no longer placeable (in preparation for it to be occupied)

        //Update the tower's baseWaypoint to that which is supplied as the input to this method (to where you will be moving the tower)
        oldestTower.baseWaypoint = newBaseWaypoint;

        oldestTower.transform.position = newBaseWaypoint.transform.position; //move oldest tower to new baseWaypoint position

        towerQueue.Enqueue(oldestTower); //enqueue this oldest tower (to top of queue)

        Debug.Log("Tower limit reached");
        //todo: actually move towers!
    }

    public void UpgradeTowerFiringRate()
    {
        foreach(Tower towerInstance in towerQueue)
        {
            towerInstance.SetFiringRate(towerInstance.GetFiringRate() + 1);//particleFireSpeed++
        }
    }
}
