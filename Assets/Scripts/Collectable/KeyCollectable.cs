using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class KeyCollectable : Collectable
{
    public override void TriggerHandle()
    {
        GameManager.Ins.CurKey += m_bonus;
        GameData.Ins.key = GameManager.Ins.CurKey;
        GameData.Ins.SaveData();

        //Update Game GUI
        GUIManager.Ins.UpdateKey(GameData.Ins.key);
 
    }
}
