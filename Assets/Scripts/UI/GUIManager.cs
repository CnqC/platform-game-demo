using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnqC.PlatformGame;

public class GUIManager : SingleTon<GUIManager>
{
    public Text liveCountingTxt;
    public Text hpCountingTxt;
    public Text coinCountingTxt;
    public Text timeCountingTxt;
    public Text bulletCountingTxt;
    public Text keyCountingTxt;
    public GameObject mobieGamepad;


    public Dialog settingDiaLog;
    public Dialog pauseDiaLog;
    public Dialog lvClearDiaLog;
    public Dialog lvFailDiaLog;

    public override void Awake()
    {
        MakeSingleTon(false); // không giữ lại khi load sence
    }

    public void UpdateTxt( Text txt, string content)
    {
        if(txt != null)
        {
            txt.text = content; // update text
        }
    }
    public void UpdateLife(int live)
    {
        UpdateTxt(liveCountingTxt, live.ToString());
    }
    public void UpdateHp(int hp)
    {
        UpdateTxt(hpCountingTxt,hp.ToString());
    }

    public void UpdateCoin(int coin)
    {
        UpdateTxt(coinCountingTxt, coin.ToString());
    }
    public void UpdatePlaytime(string time)
    {
        UpdateTxt(timeCountingTxt, time.ToString());
    }
    public void UpdateBullet(int bullet)
    {
        UpdateTxt(bulletCountingTxt, bullet.ToString());
    }
    public void UpdateKey(int key)
    {
        UpdateTxt(keyCountingTxt, key.ToString());
    }

     public void showMobileGamePad(bool isShow)
    {
        if(mobieGamepad)
        {
            mobieGamepad.SetActive(isShow);
        }
    }

}
