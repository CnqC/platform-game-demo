using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class LevelManager : SingleTon<LevelManager>
{
    public LevelItem[] levels;
    private int m_curlevelId;
  

    public int CurlevelId { get => m_curlevelId; set => m_curlevelId = value; }

    public LevelItem CurLevel
    {
        get => levels[m_curlevelId]; // take Id of present Level
                                    // if levelId = 2 --> lấy ra lvItem thứ 2 trong mảng lv
    }

    // for first time
    public void Init()
    {
        if (levels == null || levels.Length <= 0) return;

        for (int i = 0; i < levels.Length; i++)
        {
            var level = levels[i];

            if(level != null)
            {
                if(i == 0)
                {
                    // first lv --> free
                    GameData.Ins.UpdateLevelUnlocked(i, true);
                    GameData.Ins.curlevelId = i;
                }
                else
                {
                    GameData.Ins.UpdateLevelUnlocked(i, false); // block all expect first

                }
            }
            // khởi tạo for first
            // check là level đã chơi qua hay chưa
            GameData.Ins.UpdateLevelPassed(i, false);
            GameData.Ins.UpdatePlayTime(i, 0f); 
            GameData.Ins.UpdateCheckPoint(i, Vector3.zero);
            GameData.Ins.UpdateLevelSocreNoneCheck(i, 0);
            GameData.Ins.SaveData();
        }
    }
}
