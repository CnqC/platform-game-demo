using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnqC.PlatformGame;

public class LevelClearDiaLog : Dialog
{
    public Image[] stars;
    public Sprite activeStar;// active là sao sáng
    public Sprite deactiveStar; //deactive là sao tối

    public Text liveCountingtxt;
    public Text hpCountingTxt;
    public Text timecountingTxt;
    public Text coinCountingTxt;

    public override void Show(bool isShow)
    {
        base.Show(isShow);
        if(stars != null && stars.Length >0)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                var star = stars[i];
                if (star)
                {
                    star.sprite = deactiveStar;
                }
            }

            for (int i = 0; i < GameManager.Ins.GoalStar; i++)
            {
                // player có bao nhiêu star duyệt bấy nhiêu lần
                var star = stars[i];

                if (star)
                {
                    star.sprite = activeStar; 
                }
            }

            if (liveCountingtxt)
            {
                liveCountingtxt.text = $"x {GameManager.Ins.CurLive}";
            }
            if (hpCountingTxt)
            {
                hpCountingTxt.text = $"x {GameManager.Ins.player.CurHp}";
            }
            if (coinCountingTxt)
            {
                coinCountingTxt.text = $"x {GameManager.Ins.CurCoin}";
            }
            if (timecountingTxt)
            {
                timecountingTxt.text = $"x {Helper.TimeConvert(GameManager.Ins.GameplayTime)}";
            }
            

        }
    }

    #region event for Button
    public void Replay()
    {
        Close();
        GameManager.Ins.Replay();
    }

    public void nextLevel()
    {
        Close();
        GameManager.Ins.NextLevel();
    }


    
    #endregion
}
