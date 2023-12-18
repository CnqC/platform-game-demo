using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class Collectable : MonoBehaviour
{
    public CollectableType type;
    public int minBonus;
    public int maxBonus;

    public AudioClip collisionSfx;
    public GameObject destroyVfxPb; // hiệu ứng khi mà ăn được item -> sẽ biến mất

    protected int m_bonus;
    protected Player m_player;

    private void Start()
    {
        m_player = GameManager.Ins.player;

        if (!m_player) return;

        m_bonus = Random.Range(minBonus, maxBonus);

        Init();
    }
    protected void DestroyWhenLevelPassed()
    {
        if (GameData.Ins.IsLevelPassed(LevelManager.Ins.CurlevelId))
        {
            // neu level dc clear 1 lan r
            // xóa đi cái collectable để ng chơi k thể vào đó và lấy nó
            Destroy(gameObject);
        }
    }
    public virtual void Init()
    {
        DestroyWhenLevelPassed();
    }

    public virtual void TriggerHandle()
    {

    }

    public  void Trigger()
    {
        TriggerHandle();
        if (destroyVfxPb)
        {
            Instantiate(destroyVfxPb, transform.position, Quaternion.identity);
        }
        // Player Sound
        Destroy(gameObject);
    }
}
