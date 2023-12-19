using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class PauseDiaLog : Dialog
{
    public override void Show(bool isShow)
    {
        Time.timeScale = 0f; // dừng tất cả mọi thứ lại
        base.Show(isShow);

        

    }

    public void Replay()
    {
        // restart lại map đó
        Close();
        SceneController.Ins.LoadLevelScene(LevelManager.Ins.CurlevelId);
    }

    public void OpenSetting()
    {
        if(GUIManager.Ins.settingDiaLog != null)
        {
            GUIManager.Ins.settingDiaLog.Show(true); // open SettingDiaLog
        }
    }

    public void Exit()
    {
        Close(); // đóng hộp thoại
        SceneController.Ins.LoadScene("MainMenu");
    }
    public override void Close()
    {
        // stop pause
        Time.timeScale = 1f;
        base.Close();

    }
}
