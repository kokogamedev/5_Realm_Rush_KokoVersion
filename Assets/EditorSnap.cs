﻿// The PrintAwake script is placed on a GameObject.  The Awake function is
// called when the GameObject is started at runtime.  The script is also
// called by the Editor.  An example is when the Scene is changed to a
// different Scene in the Project window.
// The Update() function is called, for example, when the GameObject transform
// position is changed in the Editor.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorSnap : MonoBehaviour
{
    [Header("Grid Parameters")]
    [Range(1f,20f)][SerializeField] float gridSize = 10f;

    void Awake()
    {
        Debug.Log("Editor causes this Awake");
    }

    void Update()
    {
        Vector3 snapPos; //This vector represents the snapped position of the object
        //The following three lines are, respectively for the x, y, and z components of snapPos, the integer-rounded values of the position of the object
        //Each line takes your coordinate-position float-value, divides it by the grid size value, produces a factor which indicates how many grid size values are
        // contained in the transformed position value, uses Mathf.RoundtoInt to round this factor to the nearest whole number (corresponding to the nearest "whole" grid value),      
        // and converts the rounded factor back to this "whole" grid-value by multiplying the said rounded factor by the grid size
        snapPos.x = Mathf.RoundToInt(transform.position.x / gridSize) * gridSize; 
        snapPos.y = Mathf.RoundToInt(transform.position.y / gridSize) * gridSize;
        snapPos.z = Mathf.RoundToInt(transform.position.z / gridSize) * gridSize;

        transform.position = new Vector3(snapPos.x, snapPos.y, snapPos.z);
    }
}

