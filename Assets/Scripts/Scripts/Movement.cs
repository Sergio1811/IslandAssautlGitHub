using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private CharacterController characterController;
    private Transform cameraAnchor;

    public enum playerType { pick, ace, sword }
    public playerType actualType;

    float xMovement, zMovement;
    private float currentSpeed;
    public float characterSpeed;
    bool canMove = true;

    private float pressedTimer = 0f;
    public float neededPressedTime;
    private bool receiveInputAction = true; //si el jugador ha mantenido pulsado "neededPressedTime", se vuelve false para no seguir recibiendo el input
    public float hitDistance; //distancia a la que se considera que el jugador da al objeto al que apunta
    public Image actionSphere;
    GameObject actionObject;
    bool actionOn = false;
    public float inmortalDurationTime = 2f;
    float inmortalTimer;
    bool inmortal;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraAnchor = Camera.main.transform.parent.transform;
        actionSphere.transform.rotation = Camera.main.transform.localRotation;
        inmortal = false;
    }

    void Update()
    {
        if (canMove)
            WalkTime();

        if (receiveInputAction)
        {
            StartAction();

            if (actionOn)
                UpdateAction();
        }
        else
            EndAction();

        if (inmortal)
            CheckInmortal();
    }


    void WalkTime()
    {
        Vector3 movement = Vector3.zero;
        xMovement = Input.GetAxisRaw("Horizontal");
        zMovement = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(xMovement) > 0 || Mathf.Abs(zMovement) > 0)
        {
            Vector3 forward = cameraAnchor.forward;
            forward.y = 0;
            forward.Normalize();
            Vector3 right = cameraAnchor.right;
            right.y = 0;
            right.Normalize();

            movement = forward * zMovement + right * xMovement;
            transform.localRotation = Quaternion.LookRotation(movement);
        }

        if (!characterController.isGrounded)
            movement.y += Physics.gravity.y;

        characterController.Move(movement.normalized * characterSpeed * Time.deltaTime);
    }


    void StartAction()
    {
        if (Input.GetAxisRaw("Action")>0.2f)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            Ray ray2 = new Ray(transform.position + transform.right * 1.5f, transform.forward);
            Ray ray3 = new Ray(transform.position - transform.right * 1.5f, transform.forward);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, hitDistance) || Physics.Raycast(ray2, out hit, hitDistance) || Physics.Raycast(ray3, out hit, hitDistance))
            {
                actionObject = hit.collider.gameObject;
                switch (actionObject.tag)
                {
                    case "Rock":
                        if (actualType == playerType.pick)
                            actionOn = true;
                        break;
                    case "Tree":
                        if (actualType == playerType.ace)
                            actionOn = true;
                        break;
                    case "Enemy":
                        if (actualType == playerType.sword)
                            Destroy(actionObject.transform.parent.gameObject);
                        break;
                    case "Chest":
                        if (actualType == playerType.sword)
                            actionOn = true;
                        break;
                    case "Exit":
                        GameManager.Instance.LevelComplete();
                        break;
                }
            }
        }
    }

    void UpdateAction()
    {
        if (Input.GetAxis("Action")>0.2f)
        {
            actionSphere.transform.eulerAngles = new Vector3(actionSphere.transform.eulerAngles.x, cameraAnchor.transform.eulerAngles.y, 0); //Rotación esfera de acción
            canMove = false;
            pressedTimer += Time.deltaTime;
            actionSphere.fillAmount = pressedTimer / neededPressedTime;

            if (pressedTimer >= neededPressedTime)
                receiveInputAction = false;
        }
        else
        {
            actionOn = false;
            actionSphere.fillAmount = 0;
            pressedTimer = 0.0f;
            canMove = true;
        }
    }

    void EndAction()
    {
        switch (actionObject.tag)
        {
            case "Rock":
                if (actualType == playerType.pick)
                    BreakRock(actionObject.transform.parent.gameObject);
                break;
            case "Tree":
                if (actualType == playerType.ace)
                    CutTree(actionObject.transform.parent.gameObject);
                break;
            case "Chest":
                if (actualType == playerType.sword)
                    PickFabrics(actionObject);
                break;
        }

        actionOn = false;
        actionSphere.fillAmount = 0;
        pressedTimer = 0f;
        receiveInputAction = true;
        canMove = true;
    }


    public void BreakRock(GameObject rock)
    {
        GameManager.Instance.PickRock();
        Destroy(rock);
    }

    public void CutTree(GameObject tree)
    {
        GameManager.Instance.PickWood();
        Destroy(tree);
    }

    public void PickFabrics(GameObject chest)
    {
        GameManager.Instance.PickFabrics();
        Destroy(chest);
    }


    void CheckInmortal()
    {
        inmortalTimer += Time.deltaTime;
        if (inmortalTimer >= inmortalDurationTime)
            inmortal = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Dead")
            GameManager.Instance.EndProtoLevel();
    }

    public void Damage (Vector3 direction)
    {
        if (!inmortal)
        {
            inmortal = true;
            inmortalTimer = 0;
            characterController.Move(direction * 10f);
            GameManager.Instance.Damage();
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * hitDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + transform.right * 1.5f, transform.forward * hitDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position - transform.right * 1.5f, transform.forward * hitDistance);

    }
}
