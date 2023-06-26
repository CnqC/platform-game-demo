using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

[CreateAssetMenu(fileName = "GamePlaySetting", menuName ="CnqC_FPG/GamePlaySetting")]
public class GamePlaySetting : ScriptableObject
{
    public bool isOnMoblie;
    public int startingLive; // số mạng ng chơi bắt đầu chơi
    public int startingBullet; // số đạn mà ng chơi bắt đầu chơi
}
