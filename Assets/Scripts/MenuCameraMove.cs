using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMove : MonoBehaviour {

    public float Velocidad;
    public Transform pivoteCamara;

    private void Update()
    {
        pivoteCamara.Rotate(0, Velocidad * Time.deltaTime, 0);
    }
}