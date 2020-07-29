using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject gimbalObject;
    public GameObject cameraObject;

    public void GimbalRotateY(float y)
    {
        gimbalObject.transform.Rotate(new Vector3(0, y, 0));
    }
}