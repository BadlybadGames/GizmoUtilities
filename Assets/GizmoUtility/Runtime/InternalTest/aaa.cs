using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class aaa : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        GizmoUtility.Label(Vector3.zero, "Hello world");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GizmoUtility.Arrow(Vector3.zero, Vector3.one, 4, duration:4, animatedColor:true, endcolor:Color.blue);
        }
    }
}