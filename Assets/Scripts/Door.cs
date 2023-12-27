using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;
using System;

public class Door : MonoBehaviour
{
    public int keyRequired;
    public Sprite OpenSp;
    public Sprite CloseSp;

    private SpriteRenderer m_sp;
    private bool m_isOpen;

    public bool IsOpen { get => m_isOpen;  }    

    private void Awake()
    {
        m_sp = GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        DoorChecking();
    }

    private void DoorChecking()
    {
        m_isOpen = GameData.Ins.IsLevelUnlocked(LevelManager.Ins.CurlevelId + 1);
        m_sp.sprite = m_isOpen ? OpenSp : CloseSp; 
    }

    public void OpenDoor()
    {
        if (m_isOpen)
        {
            GameManager.Ins.CurKey = 0;
            GameManager.Ins.LevelClear();

            //update Key trên GUi
            GUIManager.Ins.UpdateKey(GameManager.Ins.CurKey);
            return;
        }


        if(GameManager.Ins.CurKey >= keyRequired)
        {
            //rs key
            GameManager.Ins.CurKey = 0;
            GameData.Ins.key = 0;

            // level đã mở khóa: mở level mới vs levelpass == true
            GameData.Ins.UpdateLevelUnlocked(LevelManager.Ins.CurlevelId +1,true);
            GameData.Ins.UpdateLevelPassed(LevelManager.Ins.CurlevelId, true);
            GameData.Ins.SaveData();

            GameManager.Ins.LevelClear();
            
            // thay đổi sprite
            DoorChecking();

            //update Key trên GUI
            GUIManager.Ins.UpdateKey(GameManager.Ins.CurKey);
        }
    }
}
