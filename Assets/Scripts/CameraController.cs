using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject gimbalObject;
    public GameObject cameraObject;

    private Camera cameraComponent;

    private void Awake()
    {
        cameraComponent = cameraObject.GetComponent<Camera>();
    }

    public void GimbalRotateY(float y)
    {
        gimbalObject.transform.Rotate(new Vector3(0, y, 0));
    }

    public void UpdateOrthographicSize(float delta)
    {
        cameraComponent.orthographicSize = Mathf.Clamp(cameraComponent.orthographicSize + delta, 2f, 20f);
    }
}