using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;
using MonsterLove.StateMachine;// tạo ra hệ thống thay đổi trạng thái


public class Player : Actor
{

    private StateMachine<PlayerAnimState> m_fsm;
    
    [Header("Smooth Jumping Setting: ")]
    [Range(0f, 5f)]
    public float jumpingFallingMultipiler = 2.5f;
    [Range(0f, 5f)]
    public float lowJumpingMultipiler = 2.5f;

    [Header("References: ")]
    public SpriteRenderer sp;
    public ObstacleChecker obstacleChker;
    public CapsuleCollider2D defaultCol;
    public CapsuleCollider2D flyingCol;
    public CapsuleCollider2D inWaterCol;

    private PlayerStat m_curStat;      // tham chieu tới scriptable Object để lưu trữ các thông số của player
    private PlayerAnimState m_prevState; // dùng để lưu lại tráng thái đằng trước đằng thái hiện tại
    private float m_waterFallingTime = 1f;// có 1 khoảng tgian để player ở dưới nước
    private float m_attackTime; // thời gian trễ khi tấm công
    private bool m_isAttacked; // đã tấn công chưa


    private bool IsDead
    {
        get => m_fsm.State == PlayerAnimState.Dead || m_prevState == PlayerAnimState.Dead;  
        // trạng thái hiện của hệ thống thay đổi trạng thái == trạng thái dead || trạng thái trước trạng thái hiện tại == trạng thái dead   
    }

    private bool IsJumping
    {
        get => m_fsm.State == PlayerAnimState.Jump || m_fsm.State == PlayerAnimState.OnAir ||
            m_fsm.State == PlayerAnimState.Land;
    }
    private bool IsFlying
    {
        get => m_fsm.State == PlayerAnimState.OnAir || m_fsm.State == PlayerAnimState.Fly ||
            m_fsm.State == PlayerAnimState.FlyOnAir; // chứng tỏ là player đang bay

    }

    private bool IsAttacking
    {
       get => m_fsm.State == PlayerAnimState.HammerAttack || m_fsm.State == PlayerAnimState.FireBullet;  
    }

    private void ActionHandle()
    {
        // xử lý action của player
    }

    protected override void Dead()
    {
        if (IsDead) return; // chết = return

        ChangeState(PlayerAnimState.Dead); 
    }

    public void ChangeState(PlayerAnimState state)
    {
        // chuyển đổi trạng thái
        m_prevState = m_fsm.State; // state hiện tại = preVstae
        m_fsm.ChangeState(state); // chuyển state mới
    }

    // xử lý chuyển sang trạng thái khi mà animation kết thúc
    private IEnumerator ChangeStateDelayCO(PlayerAnimState newState,float timeExtra=0)
    {
        // lấy ra animation clip đang đính vào
        var animClip = Helper.GetClip(m_anim, m_fsm.State.ToString());

        if(animClip != null)
        {
            yield return new WaitForSeconds(animClip.length + timeExtra);// khoảng thời gian của clip animation;
            ChangeState(newState);
        }
        yield return null;
    }

    private void ChangeStateDelay(PlayerAnimState newState, float timeExtra = 0) // phương thức kích hoạt coroutine chuyển state trong bao nhiêu lâu
    {
        StartCoroutine(ChangeStateDelayCO(newState,timeExtra));
    }

    private void ActiveCol(PlayerCollider collider) // kích hoạt col của player
    {
        if (defaultCol)
            defaultCol.enabled = collider == PlayerCollider.Default;
        // nếu như mà giá trị truyền vào của Phương thức = với biến Default trong enum PlayerCollider thì sẽ bật 

        if (flyingCol)
            flyingCol.enabled = collider == PlayerCollider.Flying;

        if (inWaterCol)
            inWaterCol.enabled = collider == PlayerCollider.InWater;
    }


    protected override void Awake()
    {
        base.Awake();
        m_fsm = StateMachine<PlayerAnimState>.Initialize(this);
        m_fsm.ChangeState(PlayerAnimState.Swim);

        //FSM_MethodGen.Gen<PlayerAnimState>();
    }

    #region FSM
    void SayHello_Enter() { }
    private void SayHello_Update()
    {
        Helper.PlayAnim(m_anim, PlayerAnimState.SayHello.ToString());
    }
    void SayHello_Exit() { }
    void Walk_Enter() { }
    private void Walk_Update() {
        Helper.PlayAnim(m_anim, PlayerAnimState.Walk.ToString());
    }
    void Walk_Exit() { }
    void Jump_Enter() { }
    private void Jump_Update()
    {
        Helper.PlayAnim(m_anim, PlayerAnimState.Jump.ToString());
    }
    void Jump_Exit() { }
    void OnAir_Enter() { }
    private void OnAir_Update()
    {
        Helper.PlayAnim(m_anim, PlayerAnimState.OnAir.ToString());
    }
    void OnAir_Exit() { }
    void Land_Enter() { }
    private void Land_Update()
    {
        Helper.PlayAnim(m_anim, PlayerAnimState.Land.ToString());
    }
    void Land_Exit() { }
    void Swim_Enter() { }
    private void Swim_Update()
    {
        Helper.PlayAnim(m_anim, PlayerAnimState.Swim.ToString());
    }
    void Swim_Exit() { }
    void FireBullet_Enter() { }
    private void FireBullet_Update()
    {
        Helper.PlayAnim(m_anim, PlayerAnimState.FireBullet.ToString());
    }
    void FireBullet_Exit() { }
    void Fly_Enter() { }
    private void Fly_Update()
    {
        Helper.PlayAnim(m_anim, PlayerAnimState.Fly.ToString());
    }
    void Fly_Exit() { }
    void FlyOnAir_Enter() { }
    private void FlyOnAir_Update()
    {
        Helper.PlayAnim(m_anim, PlayerAnimState.FlyOnAir.ToString());
    }
    void FlyOnAir_Exit() { }
    void SwimOnDeep_Enter() { }
    private void SwimOnDeep_Update()
    {
        Helper.PlayAnim(m_anim, PlayerAnimState.SwimOnDeep.ToString());
    }
    void SwimOnDeep_Exit() { }
    void OnLadder_Enter() { }
    private void OnLadder_Update() {
        Helper.PlayAnim(m_anim, PlayerAnimState.OnLadder.ToString());
    }
    void OnLadder_Exit() { }
    void Dead_Enter() { }
    private void Dead_Update() {
        Helper.PlayAnim(m_anim, PlayerAnimState.Dead.ToString());
    }
    void Dead_Exit() { }
    void Idle_Enter() { }
    private void Idle_Update() {
        Helper.PlayAnim(m_anim, PlayerAnimState.Idle.ToString());
    }
    void Idle_Exit() { }
    void LadderIdle_Enter() { }
    private void LadderIdle_Update() {
        Helper.PlayAnim(m_anim, PlayerAnimState.LadderIdle.ToString());
    }
    void LadderIdle_Exit() { }
    void HammerAttack_Enter() { }
    private void HammerAttack_Update() {
        Helper.PlayAnim(m_anim, PlayerAnimState.HammerAttack.ToString());
    }
    void HammerAttack_Exit() { }


    #endregion
}
