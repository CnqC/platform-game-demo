using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class CoinCollectable : Collectable
{
    public override void TriggerHandle()
    {
        GameManager.Ins.AddCoins(m_bonus);
    }
}
