using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class FishFreeMoving : FreeMovingEnemy
{

    // vì chỉ khi player dưới nước thì nó mới đuổi, chứ player trên bờ thì mấy con dưới nc k đu
    protected override void Update()
    {
        base.Update();
        if (!GameManager.Ins.player.obstacleChker.IsOnWater)  // nếu mà player k dưới nước
        {
            m_fsm.ChangeState(EnemyAnimState.Moving);
            return;
        }


    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!GameManager.Ins.player.obstacleChker.IsOnWater)
        {
            m_fsm.ChangeState(EnemyAnimState.Moving);
            
        }
    }


}
