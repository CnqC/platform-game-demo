using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;
using MonsterLove.StateMachine;// tạo ra hệ thống thay đổi trạng thái


public class Player : Actor
{

    private StateMachine<PlayerAnimState> m_fsm; 
    // khởi tạo đối tượng từ StateMachine, với kiểu dữ liệu từ PlayerAnimState, có tên là m_fsm
    
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

    protected override void Awake()
    {
        base.Awake();

        // sử dụng Initialize( kiểu dự liệu  tên) là cách tốt nhất để khởi tạo đối tượng bắt nguồn từ monobehavior 
        m_fsm = StateMachine<PlayerAnimState>.Initialize(this);
        m_fsm.ChangeState(PlayerAnimState.Idle);
      
    }   

    private void Update()
    {

        if (sp)
        {
            if (obstacleChker.IsOnWater)
            {
                sp.sortingOrder = (int)SpriteOrder.Inwater;
            }
            else
            {
                sp.sortingOrder = (int)SpriteOrder.Normal;
            }
        }

        if (IsDead)
        {
            // nếu như Player Dead

            GameManager.Ins.SetMapSpeed(0f);
        }


        ActionHandle();
        if(m_isInvincible == true)
        {
            Debug.Log("true");

        }
        else
        {
            Debug.Log("Nothing");
        }
       

    }
    private void FixedUpdate()
    {
        SmoothJump();
    }

    protected override void Init()
    {
        base.Init();
        if(stat != null) // nếu mà scriptable Ojb của player != null
        {
            m_curStat = (PlayerStat)stat;
        }
    }

    #region ActionPlayer
    private void ActionHandle()
    {
        // xử lý action của player

        if (IsAttacking || m_isKnockBack) return; //khi hero đang đánh hay bị đánh thì k thể leo thang hay làm hành động khác được

        if (GamePadController.Ins.IsStatic)
        {
            GameManager.Ins.SetMapSpeed(0f);
            // khi nhả hết nút ra 
            m_rb.velocity = new Vector2(0f, m_rb.velocity.y); //  nhân vật di chuyển bị trượt 1 đoạn khi k bấm nút gì hết -> stop vận tốc trục X lại
        }
        else
        {
           
        }
        if (obstacleChker.IsOnLadder && m_fsm.State != PlayerAnimState.LadderIdle
          && m_fsm.State != PlayerAnimState.OnLadder) // khi player chạm vào thang thì chuyển sang dạng LadderIdle
        {
            ChangeState(PlayerAnimState.LadderIdle);
        }

        if (!obstacleChker.IsOnWater )
        {
            AttackChecking();
        } 

        ReduceActionRate(ref m_isAttacked, ref m_attackTime, m_curStat.attackRate); // delay lại tấn công của palyer
    }
    #endregion
    protected override void Dead()
    {
     
        if (IsDead) return; // chết = return
        base.Dead(); // ghi đè lại lớp cha
        ChangeState(PlayerAnimState.Dead); 
        
    }

    #region Move for Player
    private void Move (Direction dir)
    {
        if (m_isKnockBack) return;

        m_rb.isKinematic = false; // trả về dạng dymamic

        if(dir == Direction.left || dir == Direction.Right)
        {
            Flip(dir); // lật scale của player dứng hướng di chuyển

            m_hozDir = dir == Direction.left ? -1 : 1; // nếu di chuyển sang trái thì biến m_hordir = -1 còn không thì = 1

            m_rb.velocity = new Vector2(m_hozDir * CurSpeed, m_rb.velocity.y);

            if (CameraFollow.ins.IsHozStuck) // nếu như mà Player bị ngăn k thể di chuyển
            {
                //GameManager.Ins.SetMapSpeed(0f); 
            }
            else
            {   // khi mà Player không bị ngăn bởi bất cứ vật nào, thì BG sau của Player sẽ di chuyển ngược hướng với lại Player
                GameManager.Ins.SetMapSpeed(-m_hozDir * m_curSpeed); 
            }
        } 
        else if (dir == Direction.Up || dir == Direction.Down)
        {
            m_vertDir = dir == Direction.Down ? -1 : 1;

            m_rb.velocity = new Vector2(m_rb.velocity.x, m_vertDir * CurSpeed);
        }
    }
    #endregion

    #region Make Jump animation for player
    private void Jump()
    {
        GamePadController.Ins.CanJump = false; // tránh bấm nút jump nhiều lần, chỉ 1 lần

      
        m_rb.velocity = new Vector2(m_rb.velocity.x,0f); // reset
        m_rb.isKinematic = false;// trả lại dạng dymamic
        m_rb.gravityScale = m_startingGravity;// lực hút trái đất ban đầu
        m_rb.velocity = new Vector2(m_rb.velocity.x, m_curStat.jumpForce);// lay ra gia tri trong ScriptableOjbect
    }

    private void SmoothJump() // làm cho cú nhảy của player khi ấn 1 cái thì nhảy bình thương, nếu giữ thì sẽ nhảy max
    {
        if (obstacleChker.IsOnGround || (obstacleChker.IsOnWater && IsJumping)) return;

        if(m_rb.velocity.y < 0)
        {
            // gravity.y =-9.81 , nên vận tốc sẽ giảm dần và khi vận tốc = 0 thì sẽ rơi
            // nhanh hay chậm là phụ thuộc vào tham số jumpingFallingMultipiler
            m_rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpingFallingMultipiler - 1) * Time.deltaTime;
        }
        else if( m_rb.velocity.y >0 && !GamePadController.Ins.IsJumpHolding)
        {
            m_rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpingMultipiler - 1) * Time.deltaTime;
        }
    }

    #endregion
    private void JumpChecking()
    {
        if (GamePadController.Ins.CanJump)
        {
            Jump();
            ChangeState(PlayerAnimState.Jump);
        }
    }

    private void HozMoveChecking()
    {
        if (GamePadController.Ins.CanMoveLeft)
        {
            Move(Direction.left);
        }

        if (GamePadController.Ins.CanMoveRight)
        {
            Move(Direction.Right);
        }
    }

    private void VertMoveCheck()
    {
        if (IsJumping) return; // nếu đang nhảy thì k tính, di chuyển lên trên chỉ áp dụng cho leo thang
        if (GamePadController.Ins.CanMoveUp)
        {
            Move(Direction.Up);
        }
        else if(GamePadController.Ins.CanMoveDown)
        {
            Move(Direction.Down);
        }
        GamePadController.Ins.CanFly = false; // vô hiue hóa nút bay
    }

    private void WaterCheck()
    {
        if (obstacleChker.IsOnLadder) return; // trên thang thì k check nữa

        if(obstacleChker.IsOnDeepWater) // dưới sâu
        {
            m_rb.gravityScale = 0f; // lục hút = 0
            m_rb.velocity = new Vector2(m_rb.velocity.x, 0f); // đứng yên k di chuyển lên xuống ở dưới nc sâu
            ChangeState(PlayerAnimState.SwimOnDeep);


        } else if (obstacleChker.IsOnWater && !IsJumping) // trên mặt nước
        {
            m_waterFallingTime -= Time.deltaTime; // thời gian này có nghĩa là: Player xuống nước -> chìm trong 1 thời gian ngắn -> dứng yên ( k chìm xuống đáy)

            if(m_waterFallingTime <= 0)
            {
                m_rb.gravityScale = 0f;
                    m_rb.velocity = Vector2.zero;  // đứng yên
            }

            GamePadController.Ins.CanMoveUp = false; // không cho ng chơi bấm lên trên lúc ở sát mặt nước
                                                     // vì sẽ hiện tượng nhảy nhảy trên mặt nước ( chuyển giữa các trạng thái)
            ChangeState(PlayerAnimState.Swim);
        }
    }

    private void AttackChecking()
    {
        if(GamePadController.Ins.CanAttack == true)
        {
            if (m_isAttacked) return; // khi nào tấn công r thì return -> tránh spam
            ChangeState(PlayerAnimState.HammerAttack);
        } else if(GamePadController.Ins.CanFire == true)
        {

            // ktra xem còn đủ đạn hay k thì mới chuyển trạng thái

            ChangeState(PlayerAnimState.FireBullet);
        }
    }
    public override void TakeDamege(int dmg, Actor whoHit = null)
    {
        if (IsDead) return;
        base.TakeDamege(dmg, whoHit);

        //save hp's player 
        GameData.Ins.hp = m_curHp;
        GameData.Ins.SaveData();
        if(m_curHp > 0 && !m_isInvincible)
        {
            ChangeState(PlayerAnimState.GotHit);
        }

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

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(GameTag.Enemy.ToString()))
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>(); // lấy ra đối tượng trong script enemy gán cho game oject tên "col"

            if (enemy)
            {
                TakeDamege(enemy.stat.damage,enemy);
            }
        }

        if (col.gameObject.CompareTag(GameTag.MovingPlatform.ToString()))
        {
            m_rb.isKinematic = true;
            transform.SetParent(col.gameObject.transform); // cho palyer làm con của thằng movingplatform luôn
        }
    }

    private void OnCollisionStay2D(Collision2D col) // trong lúc đang va chạm
    {
        if (col.gameObject.CompareTag(GameTag.MovingPlatform.ToString()))
        {
            if(obstacleChker.IsOnGround && m_fsm.State == PlayerAnimState.Idle) // player vừa ở dưới đất, và đang trong trạng thái idle
            {
                m_rb.isKinematic = true;
                transform.SetParent(col.gameObject.transform); // cho palyer làm con của thằng movingplatform luôn
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(GameTag.MovingPlatform.ToString()))
        {
            if (!obstacleChker.IsOnGround ) // không ở dưới đất
            {
                m_rb.isKinematic = false;
                transform.SetParent(null); // khi ra khỏi cái platform thì sẽ bỏ k còn là lớp con 
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(GameTag.Thorn.ToString()))
        {
            TakeDamege(1);
        }

        if (col.CompareTag(GameTag.CheckPoint.ToString()))
        {
            // lưu lại dữ liệu cho player khi đi qua checkpoint
        }
        if (col.CompareTag(GameTag.CollectTable.ToString()))
        {
            // xử lý việc thu thập các icon Collectable
            Collectable collectable = GetComponent<Collectable>();

            if (collectable)
            {
                collectable.Trigger(); // trong trigger có lớp ảo triggerHandle , còn triggerHandle thì sẽ tùy collectable sẽ overide nó
            }
        }

        if (col.CompareTag(GameTag.Door.ToString()))
        {
            // xử lý việc mở cửa trong game
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(GameTag.DeadZone.ToString())) // va chạm với vùng chết -> chết 
        {
            Dead();
        }
    }

    #region FSM
    private void SayHello_Enter() { }
    private void SayHello_Update()
    {
        Helper.PlayAnim(m_anim, PlayerAnimState.SayHello.ToString());
    }
    private void SayHello_Exit() { }
    private void Walk_Enter() {
        ActiveCol(PlayerCollider.Default);
        m_curSpeed = m_curStat.moveSpeed;
    }
    private void Walk_Update()  
    {
        JumpChecking();
        if(!GamePadController.Ins.CanMoveLeft && !GamePadController.Ins.CanMoveRight) // cả 2 nút trái phải đều k bấm thì sẽ stay còn cả 2 nút bấm thay phiên sẽ walk
        {
            ChangeState(PlayerAnimState.Idle);
        }
        if (!obstacleChker.IsOnGround)
        {
            ChangeState(PlayerAnimState.OnAir);
        }


        HozMoveChecking();
        Helper.PlayAnim(m_anim, PlayerAnimState.Walk.ToString());
    }
    private void Walk_Exit() { }
    private void Jump_Enter() 
    {
        ActiveCol(PlayerCollider.Default); // bật collider default
    }
    private void Jump_Update()
    {
        m_rb.isKinematic = false; // trả lại bình thường

       
        if ((m_rb.velocity.y < 0  && !obstacleChker.IsOnGround) || m_rb.velocity.y < 0 && CameraFollow.ins.IsHozStuck) // khi mà cái vận tốc của y <0( tức là đang rơi, và k chạm đất) hoặc là Player bị chặn  
        {
            ChangeState(PlayerAnimState.OnAir);
        }

        HozMoveChecking(); // đang jump thì vẫn đi sang trái sang phải vận được

        Helper.PlayAnim(m_anim, PlayerAnimState.Jump.ToString());
    }
    private void Jump_Exit() { }
    private void OnAir_Enter() 
    {
        ActiveCol(PlayerCollider.Default);
    }
    private void OnAir_Update()
    {
        m_rb.gravityScale = m_startingGravity; // trả lại cái gravity ban đầu
        HozMoveChecking();

        if (obstacleChker.IsOnGround ) // nếu chạm đất
        {
            // chuyển sang land
            ChangeState(PlayerAnimState.Land);
        }

        if (GamePadController.Ins.CanFly) // ng chơi bấm nút fly
        {
            ChangeState(PlayerAnimState.Fly);
        }

        if (obstacleChker.IsOnWater)
        {
            m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
            WaterCheck();
        }
        Helper.PlayAnim(m_anim, PlayerAnimState.OnAir.ToString());
    }
    private void OnAir_Exit() { }
    private void Land_Enter() 
    {

        ChangeStateDelay(PlayerAnimState.Idle); // khi mà chạm đất thì sẽ đợi 1 thời gian nhỏ thì chuyển sang trạng thái idle
        ActiveCol(PlayerCollider.Default);
    }
    private void Land_Update()
    {
        m_rb.velocity = Vector2.zero;
        Helper.PlayAnim(m_anim, PlayerAnimState.Land.ToString());
    }
    private void Land_Exit() { }
    private void Swim_Enter() {
        ActiveCol(PlayerCollider.InWater);
        m_curSpeed = m_curStat.swimSpeed; // gán giá trị swimspeed cho tốc độ của nhân vật hiện tại
    }
    private void Swim_Update() // trên mặt nước
    {
        JumpChecking();
        WaterCheck();

        HozMoveChecking();
        VertMoveCheck();

        Helper.PlayAnim(m_anim, PlayerAnimState.Swim.ToString());
    }
    private void Swim_Exit() {
        m_waterFallingTime = 1f; 
    }
    private void FireBullet_Enter() 
    {
        ChangeStateDelay(PlayerAnimState.Idle);
    }
    private void FireBullet_Update()
    {
        Helper.PlayAnim(m_anim, PlayerAnimState.FireBullet.ToString());
    }
    private void FireBullet_Exit() { }
    private void Fly_Enter() {
        ActiveCol(PlayerCollider.Flying);
        ChangeStateDelay(PlayerAnimState.FlyOnAir);// sau 1 thời gian kết thúc chuyển thành flyonAir
    }
    private void Fly_Update()
    {
        HozMoveChecking();
        m_rb.velocity = new Vector2(m_rb.velocity.x, -m_curStat.flyingSpeed); // cho 1 tốc độ âm để di chuyển xuống

        if (obstacleChker.IsOnWater)
        {
            m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
            WaterCheck();
        }

        Helper.PlayAnim(m_anim, PlayerAnimState.Fly.ToString());
    }
    private void Fly_Exit() { }
    private void FlyOnAir_Enter() {
        ActiveCol(PlayerCollider.Flying);
    }
    private void FlyOnAir_Update()
    {
        HozMoveChecking();
        m_rb.velocity = new Vector2(m_rb.velocity.x, -m_curStat.flyingSpeed); // y như Fly
        if (obstacleChker.IsOnGround)
        {
            ChangeState(PlayerAnimState.Land);
        }

        if (!GamePadController.Ins.CanFly) // nếu mà đang bay mà ng chơi k ấn giữ nữa --> chuyển sang Onair
        {
            ChangeState(PlayerAnimState.OnAir);
        }

        if (obstacleChker.IsOnWater)
        {
            m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
            WaterCheck();
        }

        Helper.PlayAnim(m_anim, PlayerAnimState.FlyOnAir.ToString());
    }
    private void FlyOnAir_Exit() {
    }
    private void SwimOnDeep_Enter() {
        ActiveCol(PlayerCollider.InWater);
        m_curSpeed = m_curStat.swimSpeed;
        m_rb.velocity = Vector2.zero;
    }
    private void SwimOnDeep_Update()
    {
        
        WaterCheck();
        HozMoveChecking();
        VertMoveCheck();
        Helper.PlayAnim(m_anim, PlayerAnimState.SwimOnDeep.ToString());
    }
    private void SwimOnDeep_Exit() { // khi mà thoát khỏi chế độ bơi thì
        m_rb.velocity = Vector2.zero;
        GamePadController.Ins.CanMoveUp = false; // k cho ng chơi ấn nút lên
    }
    private void OnLadder_Enter() {

        ActiveCol(PlayerCollider.Default);
        m_rb.velocity = Vector2.zero;
    }
    private void OnLadder_Update() {

        VertMoveCheck();  
        HozMoveChecking();
        if(!GamePadController.Ins.CanMoveUp && !GamePadController.Ins.CanMoveDown) // nếu mà ng chơi k bấm cả 2 nút lên và xuống
        {
            m_rb.velocity = new Vector2(m_rb.velocity.x, 0f); //giảm vận tốc của phương y về 0
            ChangeState(PlayerAnimState.LadderIdle);
        }
        if (!obstacleChker.IsOnLadder)
        {
            // nếu k còn trên thang
            ChangeState(PlayerAnimState.OnAir);
        }
    
        m_rb.gravityScale = 0f; // xét lại gravity trên thang

        Helper.PlayAnim(m_anim, PlayerAnimState.OnLadder.ToString());
    }
    private void OnLadder_Exit() { }
    private void Dead_Enter() {
        CamShake.ins.ShakeTrigger(0.7f, 0.1f, 1);
    }
    private void Dead_Update() {

        gameObject.layer = deadLayer;// chuyển về layer dead;
        Helper.PlayAnim(m_anim, PlayerAnimState.Dead.ToString());
    }
    private void Dead_Exit() { }
    private void Idle_Enter() 
    {
        ActiveCol(PlayerCollider.Default);
    }
    private void Idle_Update() {

        JumpChecking();

        if (GamePadController.Ins.CanMoveLeft || GamePadController.Ins.CanMoveRight)
        {
            ChangeState(PlayerAnimState.Walk);
        }
        Helper.PlayAnim(m_anim, PlayerAnimState.Idle.ToString());
        
    }
    private void Idle_Exit() { }
    private void LadderIdle_Enter() {
        ActiveCol(PlayerCollider.Default);
        m_rb.velocity = Vector2.zero; // vận tốc = 0
        m_curSpeed = m_curStat.ladderSpeed;
    }
    private void LadderIdle_Update() {
        
        if (GamePadController.Ins.CanMoveUp || GamePadController.Ins.CanMoveDown)
        {
            ChangeState(PlayerAnimState.OnLadder); // leo thang 
        }

        if (!obstacleChker.IsOnLadder)
        {
            // ng chơi k còn trên ladder nữa chuyển sang OnAir
            ChangeState(PlayerAnimState.OnAir);
        }
        GamePadController.Ins.CanFly = false; // trên thang nên là tắt bay
        m_rb.gravityScale = 0; // trọng lực  =0 , 0 hút nữa
        HozMoveChecking();

        Helper.PlayAnim(m_anim, PlayerAnimState.LadderIdle.ToString());
    }
    private void LadderIdle_Exit() { }
    private void HammerAttack_Enter() 
    {
        ChangeStateDelay(PlayerAnimState.Idle);
        m_isAttacked = true; // check là đã tấn công
    }
    private void HammerAttack_Update() {

        m_rb.velocity = Vector2.zero; // stop
        Helper.PlayAnim(m_anim, PlayerAnimState.HammerAttack.ToString());
    }
    private void HammerAttack_Exit() { }

    private void GotHit_Enter()
    {

    }

    private void GotHit_Update()
    {
        if (m_isKnockBack)
        {
            KnockBackMover(0.25f);
            Helper.PlayAnim(m_anim, PlayerAnimState.GotHit.ToString());
        }
        else if (obstacleChker.IsOnWater) // dưới nước và k bị đánh thì chuyển vè trạng thái trc đó
        {
            ChangeState(m_prevState);
        }
        else // k knockback hay k dưới nước thì chuyển thành idle
        {
            ChangeState(PlayerAnimState.Idle);
        }
    }

    private void GotHit_Exit()
    {

    }
    #endregion


  
}
