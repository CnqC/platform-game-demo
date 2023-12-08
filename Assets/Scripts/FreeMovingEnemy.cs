using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;


public class FreeMovingEnemy : Enemy
{

    public bool canRotate; // có thể xoay được
    private float m_movePosXR; // tọa độ của vị trí có thể di chuyển theo chiều X (Right)
    private float m_movePosXL;// tọa độ của vị trí có thể di chuyển theo chiều X (Left)
    private float m_movePosYT;
    private float m_movePosYD;

    private bool m_isHaveMovingPos;// tọa đổ kiểm tra xem enemy di chuyển tới hay chưa
    private Vector2 m_movingPos; // tọa độ mà enemy cần di chuyển tới

    protected override void Awake()
    {
        base.Awake();
        FSMInit(this); // khởi tạo FSM
    }

    protected override void Update()
    {
        base.Update();
        GetTargetDir(); // lấy ra hướng đến player

    }

    public void FindMaxMovePos()
    {
        // tìm vị trí tối đa của Enemy có thể di chuyển tới

        m_movePosXR = m_startingPos.x + movingDist; // max site bên Phải = vị trí ban đầu trục x + 1 khoảng cách chỉ định

        m_movePosXL = m_startingPos.x - movingDist;

        m_movePosYT = m_startingPos.y + movingDist;

        m_movePosYD = m_startingPos.y - movingDist;
    }

    public override void Move()
    {
        if (m_isKnockBack) return;

        if(!m_isHaveMovingPos) // nếu k lấy ra được vị trí cuối cùng mà enemy di chuyển
        {
            float randomPosX = Random.Range(m_movePosXL, m_movePosXR); // lấy 2 khoảng random giữa 2 giá trị lớn nhất trên trục X
            float randomPosY = Random.Range(m_movePosYD, m_movePosYT);

            m_movingPos = new Vector2(randomPosX, randomPosY); // tọa độ di chuyển của Enemy sẽ ngẫu nhiên.
            m_movingDir = m_movingPos - (Vector2)transform.position; // tìm tọa độ của con Enemy đến với tọa độ X
            m_movingDir.Normalize();
            m_movingDirBackup = m_movingDir; // lưu lại vị trí hiện tại của Enemy;
            m_isHaveMovingPos = true; // đã có tọa độ di chuyển tới
        }

        float angle = 0f; // góc quay = 0f

        if (canRotate)
        {
            angle = Mathf.Atan2(m_movingDir.y, m_movingDir.x) * Mathf.Rad2Deg;
        }   

        if(m_movingDir.x > 0f)
        {
            if (canRotate)
            {
                angle = Mathf.Clamp(angle,-41f, 41f); // giới hạn cho angle ( tên, min,max)
                transform.rotation = Quaternion.Euler(0f, 0f, angle); // làm cho đối tượng xoay bằng giá trị trong tọa độ (rotation)
            }

            Flip(Direction.Right); // khi mà hướng di chuyển >0 -> phải -> flip
        }
        else if(m_movingDir.x < 0f)
        {
            if (canRotate)
            {
                float newAngle = angle + 180f;
                newAngle = Mathf.Clamp(newAngle, 25, 325);
                transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
            }
            Flip(Direction.left);
        }

        DestReachedChecking(); // ktra xem đã di chuyển tới đích chưa
    }

    private void DestReachedChecking()
    {

        // khoảng cách giữa con enemy và vị trí đích mà nó cần đến < (0.5f) -> sắp đến đích
        if (Vector2.Distance(transform.position, m_movingPos) <= 0.5f)
            {
            m_isHaveMovingPos = false; // chạy cái override Move() --> lấy ra vị trí mới    
        
            // enemy sắp tới đích -> tìm đích mới và di chuyển tiếp
        }
        else // chưa đến đích thì xét lại speed cho enemy
        {
            m_rb.velocity = m_movingDir * m_curSpeed;
        }
    }



    #region FSM
    
    protected override void Moving_Enter()
    {
        base.Moving_Enter(); 
        // khi vào trạng thái di chuyển -> tìm đích (findMaxMovePos) -> cho nó di chuyển
        m_isHaveMovingPos = false;
        FindMaxMovePos();
    }
    protected override void Chasing_Update()
    {
        base.Chasing_Update();
        // khi đã phát hiện được player -> hướng di chuyển là hướng tới player -> đuổi theo
        m_movingDir = m_targetDir;

    }
    protected override void GotHit_Update()
    {
        if (m_isKnockBack)
        {
            KnockBackMover(0.55f);
        }
        else
        {
            m_fsm.ChangeState(EnemyAnimState.Moving); // hết knock -> moving
        }

    }
    #endregion

    private void OnDrawGizmos()
    {
        // line bên phải
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(
            transform.position.x + movingDist,
            transform.position.y,
            transform.position.z));

        // line bên trái
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, new Vector3(
            transform.position.x - movingDist,
            transform.position.y,
            transform.position.z));

        // Line bên trên
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(
            transform.position.x ,
            transform.position.y + movingDist,
            transform.position.z));

        // Line bên dưới
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, new Vector3(
            transform.position.x,
            transform.position.y - movingDist,
            transform.position.z));
    }
}
