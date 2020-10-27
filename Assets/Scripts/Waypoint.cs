using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    //Always arrange in order of "solididy" (in order of how likelihood that something might change, from most likely to least likely - in order of publicness)


    public Waypoint exploredFrom;
    public bool isExplored = false;
    public bool isPlaceable = true;

    const int gridSize = 10;
    public bool isNeutral = false;


    // Start is called before the first frame update
    void Start()
    {
        IsItNeutral();
    }

    private void IsItNeutral()
    {
        if (gameObject.CompareTag("Neutral")) //new code used here --> more efficient than just comparing using ==
        {
            isNeutral = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseOver()
    {
        //conditions on left mouse click: isPlaceable = false iff (1) waypoint on path; (2) waypoint already occupied by tower
        if (Input.GetMouseButtonDown(0)) //detect mouse LEFT click
        {
            if (isPlaceable)
            {
                var towerFactory = FindObjectOfType<TowerFactory>();
                towerFactory.AddTower(this); // this is a way of referencing current script/class in C#
            }
            else
            {
                Debug.Log("Not placeable at this location");
            }
        }
    }

    public int GetGridSize()
    {
        return gridSize;
    }

    public Vector2 GetGridPos()
    {
        Vector3 gridCoord3D;  //This variable is personal choice -> I made it to contain the actual normalized grid value according to the transform of the object
                              //and the gridSize we select

        gridCoord3D.x = Mathf.RoundToInt(transform.position.x / gridSize);
        gridCoord3D.y = Mathf.RoundToInt(transform.position.y / gridSize);
        gridCoord3D.z = Mathf.RoundToInt(transform.position.z / gridSize);

        return new Vector2(gridCoord3D.x, gridCoord3D.z);
    }

    public Vector2 GetGridWorldPos()
    {
        Vector3 gridPos3D; //This vector represents the snapped position of the object
        Vector2 gridCoord = GetGridPos();  //This variable is personal choice -> I made it to contain the actual normalized grid value according to the transform of the object
                            //and the gridSize we select

        gridPos3D.x = gridCoord.x * gridSize;
        gridPos3D.z = gridCoord.y * gridSize;

        return new Vector2(gridPos3D.x, gridPos3D.z);

    }
}


