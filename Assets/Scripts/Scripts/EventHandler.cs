using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public void Step()
    {
        SoundManager.PlayOneShot(SoundManager.StepSound, GameManager.Instance.cameraAnchor.transform.position);
    }
}
