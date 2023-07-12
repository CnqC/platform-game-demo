using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class Hammer : MonoBehaviour
{
    public LayerMask enemyLayer;

    public float atkRadius;
    public Vector3 offset;

    [SerializeField]
    private Player m_player;


    private void Update()
    {
        if (m_player == null) return;

        if (m_player.transform.localScale.x > 0) // player hướng phải thì phải cộng thêm 1 khoảng + để có thể giữ vị trí bên phải
        {
            if (offset.x < 0)
            {
                offset = new Vector3(offset.x * -1, offset.y, offset.z);
            }
        }
        else if (m_player.transform.localScale.x < 0)// player hướng trái thì phải cộng thêm 1 khoảng - để có thể giữ vị trí bên trái
        {
            {
                if (offset.x > 0)
                {
                    offset = new Vector3(offset.x * -1, offset.y, offset.z);
                }
            }
        }
    }
    public void Attack()
    {
        if (m_player == null) return;

        Collider2D col = Physics2D.OverlapCircle(transform.position + offset, atkRadius, enemyLayer);

        if (col) // khi mà chồng lấp với layer enemy
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();

            if (enemy) 
            {
                enemy.TakeDamege(m_player.stat.damage, m_player);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Helper.ChangAlpha(Color.gray, 0.4f);
        Gizmos.DrawSphere(transform.position + offset, atkRadius);
    }

}
