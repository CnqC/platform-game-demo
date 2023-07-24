using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class GameManager : SingleTon<GameManager>
{
    public GamePlaySetting setting;

    public Player player;

    public FreeParallax map;


    public override void Awake()
    {
        // base.Awake();  hủy khi load scene 

        MakeSingleTon(false); // hủy gameObject khi load sang scene khác
    }

    public void SetMapSpeed(float speed)
    {
        if (map)
        {
            map.Speed = speed;
        }
    }
}
