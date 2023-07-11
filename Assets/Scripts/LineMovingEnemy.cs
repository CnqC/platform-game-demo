using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;


[RequireComponent(typeof(LineMoving))]
public class LineMovingEnemy : Enemy
{
    private LineMoving lineMoving;

    protected override void Awake()
    {
        base.Awake();
        lineMoving = GetComponent<LineMoving>();
        FSMInit(this);
    }

    public override void Start()
    {
        base.Start();
        movingDist = lineMoving.movingDist;// khoảng cách tối đa của enemy di chuyển  = khoảng cách tối đa của LineMoving 

    }

    public override void Move()
    {
        
        if (m_isKnockBack) return;
        lineMoving.Move();
      
        Flip(lineMoving.moveDir);
       

    }


    #region FSM

    protected override void Moving_Update()
    {
        base.Moving_Update();
        m_targetDir = lineMoving.BackDir;// hướng di chuyển của mục tiêu ngược lại hướng di chuyển của ngược lại trong lineMoving
        lineMoving.speed = m_curSpeed;
        lineMoving.SwitchDirChecking(); // đổi hướng và chuyển sang tọa độ đích mới
    }
    protected override void Chasing_Enter()
    {
        base.Chasing_Enter();
        GetTargetDir(); // lấy ra hướng của Player
        lineMoving.SwitchDir(m_targetDir); // đuổi theo hướng Player
    }
    protected override void Chasing_Update()
    {
        base.Chasing_Update();
        GetTargetDir(); // lấy ra hướng tới ng chơi

        lineMoving.speed = m_curSpeed;
        
    }
    protected override void Chasing_Exit()
    {
        base.Chasing_Exit();
        lineMoving.SwitchDirChecking(); // khi đuổi kết thúc thì chuyển hướng
    }

    protected override void GotHit_Update()
    {
        base.GotHit_Update();
        lineMoving.SwitchDirChecking();// bị đánh trúng --> gọi ra ktra hướng di chuyển
        GetTargetDir(); 
        if (m_isKnockBack)
        {

        }
        else // k bị đẩy lùi ( hết knockback -> moving)
        {
            m_fsm.ChangeState(EnemyAnimState.Moving);
        }
    }
    #endregion
}
