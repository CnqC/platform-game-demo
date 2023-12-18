using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class LifeCollectable : Collectable
{
    public override void TriggerHandle()
    {
        GameManager.Ins.CurLive += m_bonus;
        GameData.Ins.life = GameManager.Ins.CurLive;
        GameData.Ins.SaveData();

        // Update Game GUI
    }
}
