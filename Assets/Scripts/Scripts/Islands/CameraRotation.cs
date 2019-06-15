using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private InputManager inputManager;
    Camera mainCamera;
    Transform cameraParent;

    float yaw, pitch;
    [Range(50f, 200f)]
    public float lateralSpeed, zoomSpeed;
    [Range(0f, 100f)]
    public float maxZoom, minZoom;
    [Range(0f, 100f)]
    public float maxYRotationAngle;
    [HideInInspector] public bool cameraRotating;

    private void Start()
    {
        inputManager = InputManager.Instance;
        mainCamera = Camera.main;
        cameraParent = mainCamera.transform.parent;
        yaw = cameraParent.localRotation.eulerAngles.y;
        pitch = cameraParent.localRotation.eulerAngles.x;
    }

    void Update()
    {
        if (inputManager.GetInputDown("CameraBack") && !cameraRotating) cameraRotating = true;

        if (cameraRotating)
            CameraBehindPlayer();
        else
            UpdateCameraPosition();
        
    }

    
    public void CameraBehindPlayer()
    {
        Vector3 newRotation = new Vector3(transform.eulerAngles.x, GameManager.Instance.player.transform.eulerAngles.y, transform.eulerAngles.z);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(newRotation), lateralSpeed * Time.deltaTime);
        yaw = cameraParent.localRotation.eulerAngles.y;
        pitch = cameraParent.localRotation.eulerAngles.x;

        if (transform.rotation == Quaternion.Euler(newRotation)) cameraRotating = false;
        if (pitch > maxYRotationAngle) pitch -= 360;
    }


    void UpdateCameraPosition()
    {
        float horizontal = inputManager.GetAxis("CameraMovementX");
        float vertical = inputManager.GetAxis("CameraMovementY");
        float zoom = -inputManager.GetAxis("CameraZoom");

        pitch += vertical * lateralSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -maxYRotationAngle, maxYRotationAngle);

        yaw += horizontal * lateralSpeed * Time.deltaTime;

        cameraParent.localRotation = Quaternion.Euler(pitch, yaw, 0);

        if ((zoom < 0 && mainCamera.orthographicSize > minZoom) || (zoom > 0 && mainCamera.orthographicSize < maxZoom))
            mainCamera.orthographicSize += zoom * zoomSpeed * Time.deltaTime;
    }
}
