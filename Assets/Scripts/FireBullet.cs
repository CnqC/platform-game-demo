using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class FireBullet : MonoBehaviour
{
    public Player player;
    public Transform firePoint;
    public Bullet bulletPb;

    private float m_curSpeed;

    public void Fire()
    {
        if (!bulletPb || !player || !firePoint || GameManager.Ins.CurBullet <= 0) return;

        // check xem player có quay mặt sang trái k, nếu có sang trái thì phải - speed để có thể cùng hướng trái
        m_curSpeed = player.IsFacingLeft == true ? -(bulletPb.speed) : bulletPb.speed;

        // tạo ra player trên scene
        var bulletClone = Instantiate(bulletPb, firePoint.position,Quaternion.identity);
        bulletClone.speed = m_curSpeed;
        bulletClone.owner = player;

        // giảm số lượng đạn xuống 1 đơn vị
        GameManager.Ins.ReduceBullet();
    
    }
}
