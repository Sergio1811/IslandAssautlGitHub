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
    public bool cameraRotating;

    void Awake()
    {
        mainCamera = Camera.main;
        camera = mainCamera.transform.parent;
        yaw = camera.localRotation.eulerAngles.y;
        pitch = camera.localRotation.eulerAngles.x;
    }

    void Update()
    {
        if (InputManager.Instance.GetInput("CameraBack") && !cameraRotating) cameraRotating = true; ;

        if (cameraRotating)
        {
            CameraBehindPlayer();
        }

        else
        {
            float horizontal = InputManager.Instance.GetAxis("CameraMovementX");
            float vertical = InputManager.Instance.GetAxis("CameraMovementY");
            float zoom = -InputManager.Instance.GetAxis("CameraZoom");

            pitch += vertical * lateralSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, -maxYRotationAngle, maxYRotationAngle);

            yaw += horizontal * lateralSpeed * Time.deltaTime;

            camera.localRotation = Quaternion.Euler(pitch, yaw, 0);

            if ((zoom < 0 && mainCamera.fieldOfView > minZoom) || (zoom > 0 && mainCamera.fieldOfView < maxZoom))
                mainCamera.fieldOfView += zoom * zoomSpeed * Time.deltaTime;
        }

    }

    public void CameraBehindPlayer()
    {
        Vector3 newRotation = new Vector3(transform.eulerAngles.x, GameManager.Instance.player.transform.eulerAngles.y, transform.eulerAngles.z);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(newRotation), lateralSpeed * Time.deltaTime);
        yaw = camera.localRotation.eulerAngles.y;
        pitch = camera.localRotation.eulerAngles.x;

        if (transform.rotation == Quaternion.Euler(newRotation)) cameraRotating = false;
    }
}
