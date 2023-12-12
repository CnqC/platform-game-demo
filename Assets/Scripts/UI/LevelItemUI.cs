using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;
using UnityEngine.UI;

public class LevelItemUI : MonoBehaviour
{
    public Image preview;
    public GameObject lockedArea;
    public GameObject checkMark;
    public Text PriceTxt;
    public Button btnComp;

    public void UpdateItemUI(LevelItem levelitem, int levelIndx)
    {
        if (levelitem == null) return;

        var isunlocked = GameData.Ins.IsLevelUnlocked(levelIndx); // check có unlock chưa

        if (preview)
        {
            
            preview.sprite = levelitem.preview; // lấy ra cái sprite preview 
        }

        if(PriceTxt)
        {
            PriceTxt.text = levelitem.price.ToString();// cập nhập lại price
        }

        if(lockedArea)
        {
            lockedArea.SetActive(!isunlocked); // nếu mà lv mở khóa thì lock ẩn
        }
        if (isunlocked)
        {
            if (checkMark)
            {
                // check mark --> là đã mở khóa
                // nếu mà curlevelId ( lv id của ng chơi) = levelindx ( lvId hiện tai đang kra) truyền vào --> bật
                checkMark.SetActive(GameData.Ins.curlevelId == levelIndx);
            }
        }
        else if (checkMark == null)
        {
            // chưa mở 
            checkMark.SetActive(false);
        }
        
    }

}
