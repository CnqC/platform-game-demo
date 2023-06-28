using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class GameManager : SingleTon<GameManager>
{
    public GamePlaySetting setting;
    public override void Awake()
    {
        // base.Awake();  hủy khi load scene 

        MakeSingleTon(false); // hủy gameObject khi load sang scene khác
    }
}
