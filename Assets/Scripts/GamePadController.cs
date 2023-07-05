using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class GamePadController : SingleTon<GamePadController>
{
    public float jumpHoldingTime; // thời gian mà ng chơi ấn nút nhảy

    private bool m_canMoveLeft;
    private bool m_canMoveRight;
    private bool m_canMoveUp;
    private bool m_canMoveDown;
    private bool m_canJump;
    private bool m_isJumpHolding; // ng chơi có đăng giữ nút nhảy
    private bool m_canFly;
    private bool m_canSwim;
    private bool m_canFire;
    private bool m_canAttack;

    private bool m_canCheckJumpHolding; // có thể check là ng chơi giữ nút nhảy k
    private float m_curHoldingTime; // thời gian ng chơi giữ nút nhảy

    public bool CanMoveLeft { get => m_canMoveLeft; set => m_canMoveLeft = value; }
    public bool CanMoveRight { get => m_canMoveRight; set => m_canMoveRight = value; }
    public bool CanMoveUp { get => m_canMoveUp; set => m_canMoveUp = value; }
    public bool CanMoveDown { get => m_canMoveDown; set => m_canMoveDown = value; }
    public bool CanJump { get => m_canJump; set => m_canJump = value; }
    public bool IsJumpHolding { get => m_isJumpHolding; set => m_isJumpHolding = value; }
    public bool CanFly { get => m_canFly; set => m_canFly = value; }
    public bool CanFire { get => m_canFire; set => m_canFire = value; }
    public bool CanAttack { get => m_canAttack; set => m_canAttack = value; }
    public bool CanSwim { get => m_canSwim; set => m_canSwim = value; }

    public bool IsStatic
    {
        // không bấm gì

        get => !m_canMoveLeft && !m_canMoveRight && !m_canMoveUp && !m_canMoveDown
            && !m_canJump && !m_canFly && !m_canSwim && !m_isJumpHolding; // không giữ nút nhảy
    }
    

    public override void Awake()
    {
        MakeSingleTon(false); // destroy đối tượng khi load qua scene mới
    }

    private void Update()
    {
        if(!GameManager.Ins.setting.isOnMoblie) // k ở trên mobie
        {
            float hozCheck = Input.GetAxisRaw("Horizontal");
            float vertCheck = Input.GetAxisRaw("Vertical");
            m_canMoveLeft = hozCheck < 0 ? true : false;
            m_canMoveRight = hozCheck > 0 ? true : false;

            m_canMoveUp = vertCheck > 0 ? true : false;
            m_canMoveDown = vertCheck < 0 ? true : false; // ấn nút xuống

           
            m_canJump = Input.GetKeyDown(KeyCode.Space);
            m_canFly = Input.GetKeyDown(KeyCode.F);
            m_canFire = Input.GetKeyDown(KeyCode.C);
            m_canAttack = Input.GetKeyDown(KeyCode.V);

            if (m_canJump)
            {
                //khi mà ng chơi ấn nhảy reset trạng thải ktra ng chơi có đang nhảy hay không
                m_isJumpHolding = false;
                m_canCheckJumpHolding = true; //ng chơi bấm nút nhảy -> ktra xem là ng chơi có giữ nút nhảy hay k ? hay chỉ ấn r thả
                m_curHoldingTime = 0;
            }
            if (m_canCheckJumpHolding)
            {
                m_curHoldingTime += Time.deltaTime;
                if(m_curHoldingTime > jumpHoldingTime) // thời gian mà ng chơi giữ nút nhảy > thời gian giới hạn cho phép giữ nút nhảy
                {
                    m_isJumpHolding = Input.GetKey(KeyCode.Space); //getkey : giữ, getkeydown: 1 lần duy nhất

                    // khi jump thì biến curHoldingTime =0 -> sẽ cộng += Time.delta chơi tới khi nào curholdingTime > jumpholdingtime ( public) --> thì mới dc giữ tiếp nút nhảy 
                }
            }
        }
    }
}
