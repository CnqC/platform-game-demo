using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class LineMoving : MonoBehaviour
{
    public Direction moveDir;
    public float movingDist;// độ dài quãng đường tối đa
    public float speed; // tốc độ di chuyển
    public bool isOnlyUp; // chuyển lên trên
    public bool isAuto; // tự động nó chạy 

    private Vector2 m_destination; // đích đến
    private Vector3 m_backDir; // hướng di chuyển ngược lại
    private Vector3 m_startingPos;// vị trí đầu tiên của đối tượng
    private Rigidbody2D m_rb;
    

    public Vector2 Destination { get => m_destination;  }
    public Vector3 BackDir { get => m_backDir; }


    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_startingPos = transform.position; // vị trí hiện tại của Đối tượng dc add script này
    }

    private void Start()
    {
        GetMovingDestination(); // lấy ra đích đến
    }
    private void Update()
    {
        m_backDir = m_startingPos - transform.position; // hướng ngược lại = vị trí ban đầu - vị trí hiện tại
        m_backDir.Normalize();
    }
    private void FixedUpdate()
    {
        if (!isAuto) return;    

        Move();
        SwitchDirChecking(); // đổi hướng khi tới đích
        
    }
    private void GetMovingDestination()
    {
        switch (moveDir)
        {
            case Direction.left:
                m_destination = new Vector2(m_startingPos.x - movingDist, transform.position.y);
                break;
            case Direction.Right:
                m_destination = new Vector2(m_startingPos.x + movingDist, transform.position.y);
                break;
            case Direction.Up:
                m_destination = new Vector2(transform.position.x, m_startingPos.y + movingDist);
                break;
            case Direction.Down:
                m_destination = new Vector2(transform.position.x, m_startingPos.y - movingDist);
                break;
        } 
    }

    public bool IsReached()// di chuyển tới đích
    {
        float dist1 = Vector2.Distance(m_startingPos, transform.position); // khoảng cách ban đầu vs khoảng cách hiện tại
        float dist2 = Vector2.Distance(m_startingPos, m_destination);// khoảng cách điểm bắt đầu tới điểm đích

        // nếu mà khoảng cách ( từ điểm bắt đầu tới vị trí hiện tại) > ( từ điểm bất đầu tới đích)
        // --> đã về đích == true;
        return dist1 > dist2; 

    }

    public void SwitchDir(Vector2 dir)
    {
        if(moveDir == Direction.left || moveDir == Direction.Right)
        {
            moveDir = dir.x < 0 ? Direction.left : Direction.Right; 
        }
        else if (moveDir == Direction.Up || moveDir == Direction.Down)  
        { 
            moveDir = dir.y < 0 ? Direction.Down : Direction.Up;
        }
    }

    public void SwitchDirChecking()
    {
        if (IsReached())
        {
            // đã tới đích // đổi hướng
            SwitchDir(m_backDir);

            

            // lấy ra tọa độ điểm đích mới
            GetMovingDestination();
        }
    }

    public void Move()
    {
        switch (moveDir)
        {
            case Direction.left:
                m_rb.velocity = new Vector2(-speed, m_rb.velocity.y);
                transform.position = new Vector2(transform.position.x, m_startingPos.y);// cố định vị trí ban đầu y
                

                break;
            case Direction.Right:
                m_rb.velocity = new Vector2(speed, m_rb.velocity.y);
                transform.position = new Vector2(transform.position.x, m_startingPos.y);// cố định vị trí ban đầu y
                break;
            case Direction.Up:
                m_rb.velocity = new Vector2(m_rb.velocity.x, speed);
                // vì ta di chuyển dọc nên sẽ cố định x
                transform.position = new Vector2(m_startingPos.x, transform.position.y);
                break;
            case Direction.Down:
                if (isOnlyUp) return;

                m_rb.velocity = new Vector2(m_rb.velocity.x, -speed);
                // vì ta di chuyển dọc nên sẽ cố định x
                transform.position = new Vector2(m_startingPos.x, transform.position.y);
                break;
        }
    }

    private void OnDrawGizmos()
    { 
        Gizmos.color = Color.cyan;
        //m_startingPos = transform.position; // vị trí ban đầu = vị trí hiện tại
        GetMovingDestination(); // lấy tọa độ điểm đich
        Gizmos.DrawLine(transform.position, m_destination);
    }
}
