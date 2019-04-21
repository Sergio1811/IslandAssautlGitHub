using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    Camera mainCamera;
    Transform camera;
    float yaw = 0, pitch = 0;
    public float lateralSpeed, zoomSpeed;
    public float maxZoom, minZoom;
    public float maxYRotationAngle;

    void Awake()
    {
        mainCamera = Camera.main;
        camera = mainCamera.transform.parent;
        yaw = camera.localRotation.eulerAngles.y;
        pitch = camera.localRotation.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("CameraMovementX");
        float vertical = Input.GetAxis("CameraMovementY");
        float zoom = -Input.GetAxis("CameraZoom");

        pitch += vertical * lateralSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -maxYRotationAngle, maxYRotationAngle);

        yaw += horizontal * lateralSpeed * Time.deltaTime;

        camera.localRotation = Quaternion.Euler(pitch, yaw, 0);

        //if (horizontal != 0)
        //    camera.Rotate(new Vector3(0, horizontal * lateralSpeed * Time.deltaTime, 0));

        if ((zoom < 0 && mainCamera.fieldOfView > minZoom) || (zoom > 0 && mainCamera.fieldOfView < maxZoom))
            mainCamera.fieldOfView += zoom * zoomSpeed * Time.deltaTime;

    }
}
