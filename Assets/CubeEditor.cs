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
public class CubeEditor : MonoBehaviour
{
    [Header("Grid Parameters")]
    [Range(1f,20f)][SerializeField] float gridSize = 10f;

    TextMesh textMesh;

    void Update()
    {
        Vector3 snapPos; //This vector represents the snapped position of the object
        Vector3 gridSizeFactor;  //This variable is personal choice -> I made it to contain the actual normalized grid value according to the transform of the object
        //and the gridSize we select

        //The following three lines are, respectively for the x, y, and z components of snapPos, the integer-rounded values of the position of the object
        //Each line takes your coordinate-position float-value, divides it by the grid size value, produces a factor which indicates how many grid size values are
        // contained in the transformed position value, uses Mathf.RoundtoInt to round this factor to the nearest whole number (corresponding to the nearest "whole" grid value),      
        // and converts the rounded factor back to this "whole grid-value by multiplying the said rounded factor by the grid size

        gridSizeFactor.x = Mathf.RoundToInt(transform.position.x / gridSize);
        gridSizeFactor.y = Mathf.RoundToInt(transform.position.y / gridSize);
        gridSizeFactor.z = Mathf.RoundToInt(transform.position.z / gridSize);
        string labelText = gridSizeFactor.x + "," + gridSizeFactor.z;

        snapPos.x = gridSizeFactor.x * gridSize; 
        snapPos.y = gridSizeFactor.y * gridSize;
        snapPos.z = gridSizeFactor.z * gridSize;
        transform.position = new Vector3(snapPos.x, snapPos.y, snapPos.z);

        textMesh = GetComponentInChildren<TextMesh>();
        textMesh.text = labelText;
        
        gameObject.name = labelText;
    }
}

