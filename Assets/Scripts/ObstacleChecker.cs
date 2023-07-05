using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class ObstacleChecker : MonoBehaviour
{
    // ktra ng chơi vs nền đất
    public LayerMask groundLayer;
    public LayerMask waterLayer;
    public LayerMask ladderLayer;
    public float deepWaterCheckingDistance; // khoảng cách để raycast ktra là đang trên mặt nước hay dưới sâu
    public float checkingRadius; // bán kính để ktra
    public Vector3 offset; //khoảng cách thêm
    public Vector3 deepWaterOffset; // dùng để thay đổi vị trí khi vẽ raycast
    private bool m_isOnGround;
    private bool m_isOnWater;
    private bool m_isOnLadder;
    private bool m_isOnDeepWater;


    public bool IsOnGround { get => m_isOnGround; }
    public bool IsOnWater { get => m_isOnWater; }
    public bool IsOnLadder { get => m_isOnLadder;  }
    public bool IsOnDeepWater { get => m_isOnDeepWater;  }

    private void FixedUpdate()
    {
        m_isOnGround = OverlapChecking(groundLayer);// trả ve true/false và gán giá trị cho isOnGround

        m_isOnWater = OverlapChecking(waterLayer);

        m_isOnLadder = OverlapChecking(ladderLayer);

        RaycastHit2D waterHit = Physics2D.Raycast(transform.position + deepWaterOffset, Vector2.up, deepWaterCheckingDistance, waterLayer);

        m_isOnDeepWater = waterHit;

        if (IsOnGround)
        {
            Debug.Log("dang o tren mat dat");
            Debug.Log(IsOnGround);
        }
        else { 
            Debug.Log("Dang tren khong");
            Debug.Log(IsOnGround);
        }


        //if (IsOnDeepWater)
        //    Debug.Log("Dang O duoi nuoc");
        //else
        //    Debug.Log("Dang o tren mat nuoc");
        //Debug.Log($"Ground: {m_isOnGround}_ Water: {m_isOnWater} _ Ladder: {m_isOnLadder}");


    }

    private bool OverlapChecking(LayerMask layerToCheck)
    {
        Collider2D col = Physics2D.OverlapCircle(
            transform.position + offset, checkingRadius, layerToCheck);

        return col != null;    // vì overlapcricle nên dù có va chạm hay k thì sẽ trả null
                             // nếu col != null -> chồng lấp vs nền
                            // nếu col = null -> k chồng lấp vs nền
    }

    private void OnDrawGizmos()
    {
        // changAlpha -> làm mờ đi màu sắc cho dễ nhìn hơn
        Gizmos.color = Helper.ChangAlpha(Color.red, 0.4f);

        //vẽ hình cầu
        Gizmos.DrawSphere(transform.position + offset, checkingRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + deepWaterOffset,
            new Vector3(transform.position.x + deepWaterOffset.x, // vị trí hiện tại của X + thêm 1 khoảng cách từ x
            transform.position.y + deepWaterOffset.y + deepWaterCheckingDistance, // vì cái raycast của chúng ta sẽ hướng lên trên -> nên sẽ cộng thêm khoảng cách để ktra ở trên mặt nước hay ở dưới sâu
            transform.position.z ));
    }
}
