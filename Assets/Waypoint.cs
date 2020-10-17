using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    const int gridSize = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void SetTopColor(Color color)
    {
        MeshRenderer topMeshRenderer = transform.Find("Quad_top").GetComponent<MeshRenderer>();
        topMeshRenderer.material.color = color;
    }

}


