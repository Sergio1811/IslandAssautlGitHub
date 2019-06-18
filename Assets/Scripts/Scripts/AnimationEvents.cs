using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void Step()
    {
        if(this.tag!="Enemy")
            SoundManager.PlayOneShot(SoundManager.StepSound, GameManager.Instance.cameraAnchor.transform.position);
    }
}
