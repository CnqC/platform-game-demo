using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnqC.PlatformGame;

public class LevelFailDiaLog : Dialog
{
    public Text timeCountingTxt;
    public Text coinCountingTxt;


    public override void Show(bool isShow)
    {
        base.Show(isShow); 

        if(timeCountingTxt != null)
        {
            timeCountingTxt.text = $"{Helper.TimeConvert(GameManager.Ins.GameplayTime)}";
        }
        if(coinCountingTxt != null)
        {
            coinCountingTxt.text = $"{GameManager.Ins.CurCoin}";
        }
    }


    #region Event Button
    public void Replay()
    {
        GameManager.Ins.Replay();
    }

    public void BachToMenu()
    {
        SceneController.Ins.LoadScene(GameScene.MainMenu.ToString());
    }
    #endregion
}
