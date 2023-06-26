using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

[CreateAssetMenu(fileName = "Player stat", menuName = "CnqC_PFG/Player stat")]
public class PlayerStat : ActorStat
{
    public float jumpForce;
    public float flyingSpeed;
    public float ladderSpeed;
    public float swimSpeed;
    public float attackRate; // khoảng nghỉ giữa các đòn tấn công

}
