using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class PlayerDectect : MonoBehaviour
{
    public bool disable; // vài con enemy k có chạy theo chỉ đứng yên ngán chân
    public DetectMethod detectMethod;
    public LayerMask targetLayer; // layer mục tiêu
    public float detectDist; // khoảng cách phát hiện được player

    private Player m_target;
    private Vector2 m_dirToTarget;
    private bool m_isDetected; //xác định được Player hay k

    public Player target
    { get => m_target;  }
    public Vector2 DirToTarget { get => m_dirToTarget; }
    public bool IsDetected { get => m_isDetected;  }


    private void Start()
    {
        m_target = GameManager.Ins.player; // lấy ra biến player trong GameManager

    }

    private void FixedUpdate()
    
    {
        if (!m_target || disable == true) return; 

        if(detectMethod == DetectMethod.RayCast) // nếu xác định player = raycast
        {
            // khoảng cách xác định mục tiêu = vị trí của mục tiêu (player) - vị trí của enemy( script này đính vào enemy)
            m_dirToTarget = m_target.transform.position - transform.position;
            m_dirToTarget.Normalize(); // vector2(1,0)

            
            // Vẽ raycast: bắt đầu là tại vị trí của Script( sẽ đính vào enenmy), tới 1 vector( vị trí xđ mục tieu,0), độ dài là detecDis, layer targetlayer 
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(m_dirToTarget.x, 0f),detectDist,targetLayer);

            m_isDetected = hit.collider != null; // raycast mà bắn trúng collider player -> hit != null -> xđ dc player
            
        }
        else if(detectMethod == DetectMethod.CircleOverlap) // nếu xác định player = CircleOverLap
        {
            // vẽ ra 1 hình tròn có: tâm là vị trí của script( enemy) bán kính là detectdist, layer check là targetlayer
            Collider2D col = Physics2D.OverlapCircle(transform.position, detectDist,targetLayer);

            m_isDetected = col != null; // overlapCircle xđ được Player -> col != null 
        }

        if (m_isDetected)
        {
            Debug.Log("Player is being detected");
        }
        else
        {
            Debug.Log("Player is not be detected");
        }
    }

    private void OnDrawGizmos()
    {
        if(detectMethod == DetectMethod.RayCast)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, new Vector3(
                transform.position.x + detectDist,
                transform.position.y,
                transform.position.z));
            
        }
        else if(detectMethod == DetectMethod.CircleOverlap)
        {
                                    // mờ đi
            Gizmos.color = Helper.ChangAlpha(Color.green,0.2f);

            // vẽ ra 1 hình tròn( tâm là vị trí của script(enemy), bk: detectDist)
            Gizmos.DrawSphere(transform.position,detectDist); 
        }
    }
}
