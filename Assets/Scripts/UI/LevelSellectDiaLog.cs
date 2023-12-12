using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnqC.PlatformGame;
using System;

public class LevelSellectDiaLog : Dialog
{
    public Transform gridRoot;
    public LevelItemUI lvItemUiPb;
    public Text coinCountingText;

    public override void Show(bool isShow)
    {
        base.Show(isShow);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinCountingText)
        {
            coinCountingText.text = GameData.Ins.coin.ToString(); // cập nhập lại text coin ở dưới data ng dùng
        }

        var levels = LevelManager.Ins.levels;
        if (levels == null || !gridRoot || !lvItemUiPb) return;

        Helper.ClearChilds(gridRoot); // clear Childs xóa đi các con trong 1 thằng cha nào đó

        for (int i = 0; i < levels.Length; i++)
        {
            int levelIdx = i;
            var level = levels[levelIdx];
            if (level == null) continue;

            var itemUiClone = Instantiate(lvItemUiPb, Vector3.zero, Quaternion.identity);

            itemUiClone.transform.SetParent(gridRoot); // cho cái itemUi tạo trên scence sẽ thành con Gridroot
            itemUiClone.transform.localScale = Vector3.one;
            itemUiClone.transform.localPosition = Vector3.zero;
            itemUiClone.UpdateItemUI(level, levelIdx);

            if (itemUiClone.btnComp)
            {
                itemUiClone.btnComp.onClick.RemoveAllListeners(); // remove all sự kiện
                itemUiClone.btnComp.onClick.AddListener(() => ItemEvent(level,levelIdx));// add sự kiện mới
            }
        }
    }

    private void ItemEvent(LevelItem levelItem, int levelIdx)
    {
        if (levelItem == null) return;
        bool isUnlocked = GameData.Ins.IsLevelUnlocked(levelIdx);

        if (isUnlocked)
        {
            GameData.Ins.curlevelId = levelIdx; // reload curId
            GameData.Ins.SaveData();

            LevelManager.Ins.CurlevelId = levelIdx;
          

            UpdateUI();
            SceneController.Ins.LoadLevelScene(levelIdx);
        }
        else
        { 
           // chưa unlock
           if(GameData.Ins.coin >= levelItem.price)
            {
                GameData.Ins.coin = GameData.Ins.coin - levelItem.price; // buy --> trừ
                GameData.Ins.curlevelId = levelIdx; // update Lvindx
                GameData.Ins.UpdateLevelUnlocked(levelIdx, true);
                GameData.Ins.SaveData();

                LevelManager.Ins.CurlevelId = levelIdx;

                UpdateUI();// update giao diện dialog
                SceneController.Ins.LoadLevelScene(levelIdx);
            }
            else
            {
                Debug.Log("You Don't Have Enough Coins");
            }
        }

    }
}
 