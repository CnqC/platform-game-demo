using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;
using System;

[RequireComponent(typeof(Rigidbody2D))] // yêu cầu 1 thành phần bắt buộc Ridbody2D --> add script thì sẽ tự động add Rid2D, và k thể xóa bỏ cái Rid2D
public class Actor : MonoBehaviour
{
   [Header("Common: ")]
   public ActorStat stat;
   [Header("Layer: ")]
   [LayerList] // hiển thị danh sách layer

   public int normalLayer;
   [LayerList]
   public int invincibleLayer;
   [LayerList]
   public int deadLayer;

    [Header("Reference: ")]
    [SerializeField] // dùng để hiển thị private hay protected ra ngoài inspector
    protected Animator m_anim;
    protected Rigidbody2D m_rb;

    [Header("Vfx: ")]
    public GameObject deadVfxPb;
    public FlashVfx flashVfx;
    protected Actor m_WhoHit; // bị đối tượng nào đánh trúng

    protected int m_curHp; // máu hiện tại
    protected bool m_isKnockBack; // có đang bị trạng thái đẩy lùi không
    protected bool m_isInvincible; // có đang trong trạng thái phản công hay k, khi ta/ enemy bị đánh trúng --> thời gian này k nhận damage
    protected float m_startingGravity; // lực hút trái đất đầu tiên
    protected bool m_isFacingLeft; // có quay mặt sang hướng bên trái hay không
    protected float m_curSpeed;
    protected int m_hozDir, m_vertDir; // di chuyển hướng trái/ phải và trên/ dươi

    public int CurHp { get => m_curHp; set => m_curHp = value; }
    public float CurSpeed { get => m_curSpeed;  }
   
    public bool IsFacingLeft
    {
        get => m_isFacingLeft;
    }

    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        if(m_rb)
            m_startingGravity = m_rb.gravityScale; // lấy ra cái gravity ban đầu

        if (stat == null) return;
        m_curHp = stat.hp;
        m_curSpeed = stat.moveSpeed;
    }

    public virtual void Start()
    {
        Init(); // khởi tạo
    }

    protected virtual void Init()
    {
       
    }
    public virtual void TakeDamege(int dmg, Actor whoHit = null)
    {
        if (m_isInvincible || m_isKnockBack) return; // nếu mà đang trạng thái phản công hay bị giật lùi thì sẽ k nhận sát thương

        if (m_curHp > 0)
        {
            m_WhoHit = whoHit; // truyền giá tị biến whoHit trong takeDamge cho biến m_Whohit
            m_curHp -= dmg; // giảm lượng máu = số lượng damage nhận vào

            if (m_curHp <= 0)
            {
                m_curHp = 0; // nếu máu mà <0 -> trả về 0
                Dead(); // gọi phương thức dead
            }
            KnockBack();
        }
    }

    protected virtual void KnockBack() // đẩy lùi khi nhận sát thương
    {                                              // game Object bị ẩn / không tồn tại trên scene
        if (m_isInvincible || m_isKnockBack || !gameObject.activeInHierarchy) return;

        m_isKnockBack = true;

        // bị đẩy lùi -> sẽ có 1 khoảng thời gian -> thời gian lùi và hết trạng thái -> dùng courotine
        StartCoroutine(StopKnockBack());

        if (flashVfx)
        {
            flashVfx.Flash(stat.invincibleTime); // trong trạng thái bất bại thì sẽ chạy hiệu ứng flash.
        }
    }
    protected IEnumerator StopKnockBack()
    {
        yield return new WaitForSeconds(stat.knockBackTime); // đợi 1 thời gian nhỏ 

        m_isKnockBack = false;
        m_isInvincible = true; // sau bị đẩy lùi sẽ tấn công nên phải tắt cái trạng thái KnockBack
        gameObject.layer = invincibleLayer;

        StartCoroutine(StopInvincible(stat.invincibleTime)); // 1 thời gian thì tắt trạng thái phản công
    }

    protected IEnumerator StopInvincible(float time)
    {
        yield return new WaitForSeconds(time);
        m_isInvincible = false;
        gameObject.layer = normalLayer;

        
    }

    protected void KnockBackMover(float yRate) // xử lý đẩy lùi về sau khi nhận sát thuong
    {
        if(m_WhoHit == null) // nếu mà player k va chạm vào layer enemy, va chạm vào chướng ngại vật
        {
            m_vertDir = m_vertDir == 0 ? 1 : m_vertDir; // nếu di chuyển theo hướng bằng trên dưới  == 0 -> =1 / còn k thì trả lại giá trị cũ
            m_rb.velocity = new Vector2(
                m_hozDir * (-stat.knockBackForce), // khi tấn công thì sẽ bị bật lại -> nên phải âm để ngược hướng hiện tại
                (m_vertDir * 0.55f) * stat.knockBackForce); 
        }
        else // nhận sát thương từ nhân vật
        {
           
            // hướng = vị trí của thằng dây sát thương - vị trí thằng nhân sát thương
            Vector2 dir = m_WhoHit.transform.position - transform.position;
            dir.Normalize();

            if(dir.x > 0)
            { // hướng x >0 -> phải -> đẩy lùi sẽ sang trái (-x,y)
                m_rb.velocity = new Vector2(-stat.knockBackForce, yRate * stat.knockBackForce);
            }
            else if(dir.x < 0)
            {// hướng x < 0 -> trái -> đẩy lùi sẽ sang phải (x,y)
                m_rb.velocity = new Vector2(stat.knockBackForce, yRate*stat.knockBackForce );

            }
        }
    }

    protected void Flip(Direction movedir)
    {

        
        switch (movedir)
        {
            case Direction.left:
                
                if (transform.localScale.x > 0) // nếu di chuyển sang trái mà scale x hiện tại > 0 ( sang phải) thì đổi scale lại thành âm
                {
                    transform.localScale = new Vector3(
                        transform.localScale.x * (-1),
                        transform.localScale.y,
                        transform.localScale.z);
                    m_isFacingLeft = true; // hướng mặt về trái
                }
                break;

            case Direction.Right: // nếu di chuyển sang phải mà scale đang <0 -> chỉnh lại >0.
                
                if (transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(
                        transform.localScale.x * (-1),
                        transform.localScale.y,
                        transform.localScale.z);

                    m_isFacingLeft = false;
                }
                break;
            case Direction.Up:
                if(transform.localScale.y < 0)
                {
                    transform.localScale = new Vector3(
                        transform.localScale.x,
                        transform.localScale.y * (-1),
                        transform.localScale.z);
                }
                break;
            case Direction.Down:
                if (transform.localScale.y > 0)
                {
                    transform.localScale = new Vector3(
                        transform.localScale.x,
                        transform.localScale.y * (-1),
                        transform.localScale.z);
                }
                break;
            
        }
    }

   protected virtual void Dead()
    {
        gameObject.layer = deadLayer;// chuyển về layer dead;

        if (m_rb)
            m_rb.velocity = Vector2.zero; // chết thì stop
    }
                                                      // tgian hien tại  // 1 khoang tgian thực hiện hđ
    protected void ReduceActionRate(ref bool isActed, ref float curTime, float startingTime) // khi làm 1 hành động nào đó trong scene thì giảm dần thời gian của hành động đó xuống 0
    { 
        if(isActed) // nếu hđ đó đã thực hiện
        {
            curTime -= Time.deltaTime;

            if(curTime <= 0)
            {
                isActed = false;
                curTime = startingTime;
            }
        }
    }
    
}
