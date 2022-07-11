using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera targetCamera;
    public Vector3 worldUp = Vector3.up;

    private void LateUpdate()
    {
        transform.LookAt(targetCamera.transform, worldUp);
    }
}
