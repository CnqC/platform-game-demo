using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;
using MonsterLove.StateMachine;// tạo ra hệ thống thay đổi trạng thái


public class Player : Actor
{
    private StateMachine<PlayerAnimState> m_fsm;

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
