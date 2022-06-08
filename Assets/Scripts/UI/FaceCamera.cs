using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera targetCamera;

    private void LateUpdate()
    {
        transform.LookAt(targetCamera.transform, Vector3.up);
    }
}
