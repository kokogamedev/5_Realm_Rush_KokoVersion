// The PrintAwake script is placed on a GameObject.  The Awake function is
// called when the GameObject is started at runtime.  The script is also
// called by the Editor.  An example is when the Scene is changed to a
// different Scene in the Project window.
// The Update() function is called, for example, when the GameObject transform
// position is changed in the Editor.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class CubeEditor : MonoBehaviour
{

    Waypoint waypoint;

    void Awake()
    {
        waypoint = GetComponent<Waypoint>();
    }

    void Update()
    {
        SnaptoGrid();
        UpdateLabel();
    }

    private void SnaptoGrid()
    {
        //The following  lines are, respectively for the x, y, and z components of snapPos, the integer-rounded values of the position of the object
        //Each line takes your coordinate-position float-value, divides it by the grid size value, produces a factor which indicates how many grid size values are
        // contained in the transformed position value, uses Mathf.RoundtoInt to round this factor to the nearest whole number (corresponding to the nearest "whole" grid value),      
        // and converts the rounded factor back to this "whole grid-value by multiplying the said rounded factor by the grid size
        int gridSize = waypoint.GetGridSize();
        Vector2 gridPos = waypoint.GetGridWorldPos();
        transform.position = new Vector3(gridPos.x, 0, gridPos.y);
    }

    private void UpdateLabel()
    {
        Vector2 gridCoord = waypoint.GetGridPos();
        string labelText = gridCoord.x + "," + gridCoord.y;

        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        textMesh.text = labelText;

        gameObject.name = labelText;
    }
}

