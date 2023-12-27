using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class AnimEvent : MonoBehaviour
{
    public void HammerAttack()
    {
        CamShake.ins.ShakeTrigger(0.3f,0.1f,1);

        // make sound when doing hammer attack
        AudioController.ins.PlaySound(AudioController.ins.attack);
    }


    public void PlayFootSteepSound()
    {
        AudioController.ins.PlaySound(AudioController.ins.footStep);
    }

   
}
