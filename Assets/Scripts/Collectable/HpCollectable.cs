using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class HpCollectable : Collectable
{
    public override void TriggerHandle()
    {
        m_player.CurHp += m_bonus;
        GameData.Ins.hp = m_player.CurHp;
        GameData.Ins.SaveData();


        // Update Game GUI
        GUIManager.Ins.UpdateHp(GameData.Ins.hp);

    }
}
