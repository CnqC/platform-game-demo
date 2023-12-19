using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class BulletCollectable : Collectable
{
  

    public override void TriggerHandle()
    {
        GameManager.Ins.CurBullet += m_bonus;
        GameData.Ins.bullet = GameManager.Ins.CurBullet; // save bullet inside 
        GameData.Ins.SaveData();

        // Update Game UI
        GUIManager.Ins.UpdateBullet(GameData.Ins.bullet);
    }
}
