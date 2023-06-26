using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class ActorStat : ScriptableObject
{
    [Header("Common: ")]

    public int hp;
    public int moveSpeed;
    public int damage;

    [Header("Invincible")] // thiết đặt khoảng thời gian khi mà player hay enemy bị đánh trúng sẽ tàng hình
    public float knockBackTime; 
    public float knockBackForce;// đẩy lùi về phía sau
    public float invincibleTime;

}
