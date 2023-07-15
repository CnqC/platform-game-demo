using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class Bullet : MonoBehaviour
{
    public float speed;
    public LayerMask targetLayer; // viên đạn va chạm với layer mục tiêu nào

    private Vector3 m_prevPos; // tọa độ trước đó

    [HideInInspector] // giấu đi
    public Actor owner;
    private void Awake()
    {
        m_prevPos = transform.position; // tọa độ ban đầu của viên đạn
    }

    private void Update()
    {                   // di chuyển dần dần , trong thế giới của unity
        transform.Translate(transform.right* speed * Time.deltaTime, Space.World);

        
    }

    private void FixedUpdate()
    {
        // hướng di chuyển viên đạn = vị trí hiện tại - vị trí trước đó
        Vector2 dir = (Vector2)(transform.position - m_prevPos);
        // khoảng cách giữa vị trí hiện tại và vị trí trước đó
        float dist = dir.magnitude;
                                            // vị trí, hướng, khoảng cách, layer
        RaycastHit2D hit = Physics2D.Raycast(m_prevPos, dir, dist, targetLayer);

      
        if(hit && hit.collider) // khi mà raycsat trúng collider 
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>(); // lấy ra va chạm của raycast với script Enemy
            if (enemy)
            {
                enemy.TakeDamege(owner.stat.damage, owner);
            }
            gameObject.SetActive(false); // trúng đạn r thì ẩn viên đạn đi
        }

       
        //m_prevPos = transform.position; // sau mỗi lần bắn thì sẽ set up lại vị trí ban đầu
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Helper.ChangAlpha(Color.black, 0.5f);
        Gizmos.DrawLine(transform.position, m_prevPos);
    }
}
