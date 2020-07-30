using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject gimbalObject;
    public GameObject cameraObject;

    private Camera cameraComponent;
    private Transform targetTransform;
    private Transform gimbalTransform;

    private void Awake()
    {
        cameraComponent = cameraObject.GetComponent<Camera>();
        targetTransform = GameManager.Instance.playerObject.transform;
        gimbalTransform = gimbalObject.transform;
    }

    private void LateUpdate()
    {
        gimbalTransform.position = new Vector3(targetTransform.position.x, targetTransform.position.y + 15, targetTransform.position.z);
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