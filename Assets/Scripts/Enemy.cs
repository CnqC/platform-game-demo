using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;
using MonsterLove.StateMachine;
using System;

public class Enemy : Actor
{
    [Header("Moving: ")]
    public float movingDist; // khoảng cách tối đa của Enemy

    protected PlayerDectect m_playerDetect;
    protected EnemyStat m_curstat;
    protected Vector2 m_movingDir;
    protected Vector2 m_movingDirBackup;
    protected Vector2 m_startingPos; // vị trí bắt đầu
    protected Vector2 m_targetDir; // hướng tối mục tiêu ( player)
    protected StateMachine<EnemyAnimState> m_fsm;

    public bool IsDead
    {
        get => m_fsm.State == EnemyAnimState.Dead;
    }

    protected override void Awake()
    {
        base.Awake();
        m_playerDetect = GetComponent<PlayerDectect>();
        m_startingPos = transform.position; // vị trí mà Game cbi bắt đầu gán cho biến StartingPos

    }

   

    protected void FSMInit(MonoBehaviour beHav) {
        m_fsm = StateMachine<EnemyAnimState>.Initialize(beHav);
        // bật bị trí của enemy thành dạng Moving khi bắt đầu game
        m_fsm.ChangeState(EnemyAnimState.Moving);
    }

    protected override void Init()
    {
        if (stat != null) // scriptable Ojb !=null
        {
           m_curstat = (EnemyStat)stat;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (m_isKnockBack || IsDead) return;

        Move();
    }

   

    protected virtual void Update() // lập hàm ảo để lớp sau có thể ghi đè
    {
        if (IsDead)
        {
            m_fsm.ChangeState(EnemyAnimState.Dead);
        }

        if (m_isKnockBack || IsDead) return;

        if (m_playerDetect.IsDetected) // player đạ bị phát hiện
        {
            m_fsm.ChangeState(EnemyAnimState.Chasing); // duổi theo
        }
        else
        {
            m_fsm.ChangeState(EnemyAnimState.Moving);// k tìm thấy player thì cứ di chuyển bth
        }

        if (m_rb.velocity.y <= -50) // Nếu như Enemy rơi từ vị trí cao quá -> chết
        {
            Dead();
        }
    }
    protected override void Dead()
    {
        base.Dead();
        m_fsm.ChangeState(EnemyAnimState.Dead);
    }

    protected void GetTargetDir() // lấy ra hướng đến mục tiêu(player)
    {
        // vị trí xác định mục tiêu = vị trí của Player qua biến(m_playerDetect) hướng tới PlayerDetect
        // Gọi phuong thức Target trong PlayerDetect hướng tới Player Script -> lấy vị trí của nó
        m_targetDir = m_playerDetect.target.transform.position - transform.position;
        m_targetDir.Normalize(); // đưa về dạng (1,0)
    }

    public virtual void Move()
    {
       
    }

    public override void TakeDamege(int dmg, Actor whoHit = null)
    {
        if (IsDead) return;
        base.TakeDamege(dmg, whoHit);
        if(m_curHp > 0 && !m_isInvincible) // nếu như Enemy bị tấn công nhưng máu hiện tại vẫn còn
        {
            m_fsm.ChangeState(EnemyAnimState.GotHit);
        }
    }

    #region FSM
    protected virtual  void Moving_Enter() { }
    protected virtual void Moving_Update() {
        m_curSpeed = m_curstat.moveSpeed; // gán giá trị tốc độ hiện tại
        Helper.PlayAnim(m_anim, EnemyAnimState.Moving.ToString());
    }
    protected virtual void Moving_Exit() { }
    protected virtual void Chasing_Enter() {
        m_curSpeed = m_curstat.moveSpeed; // gán giá trị tốc độ hiện tại
        Helper.PlayAnim(m_anim, EnemyAnimState.Chasing.ToString());
    }
    protected virtual void Chasing_Update() { }
    protected virtual void Chasing_Exit() { }
    protected virtual void GotHit_Enter() { }
    protected virtual void GotHit_Update() { }
    protected virtual void GotHit_Exit() { }
    protected virtual  void Dead_Enter() {
        if (deadVfxPb && IsDead) // check hiệu ứng dead có:
        {   // tạo hiệu ứng dead ngay vị trí của enemy lúc chuyển sang, không xoay
            Instantiate(deadVfxPb, transform.position, Quaternion.identity ); 
        }

        gameObject.SetActive(false);// ẩn enemy đi khi chết
    }
    protected void Dead_Update() { }
    protected void Dead_Exit() { }

    #endregion
}
