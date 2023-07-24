using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class AnimEvent : MonoBehaviour
{
    public void HammerAttack()
    {
        CamShake.ins.ShakeTrigger(0.3f,0.1f,1);
    }
}
