using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public Text priceTxt;
    public Text amountTxt;
    public Image preview;
    public Button btnComp;

    public void UpdateUI(ShopItem shopitem, int itemIdx)
    {
        if (shopitem == null) return;
        if (preview)
        {
            preview.sprite = shopitem.preview;
        }
        switch (shopitem.itemtype)
        {
            case CollectableType.Hp:
                UpdateAmountTxt(GameData.Ins.hp);
                break;
            case CollectableType.Bullet :
                UpdateAmountTxt(GameData.Ins.bullet);
                break;
            case CollectableType.Live:
                UpdateAmountTxt(GameData.Ins.life);
                break;
            case CollectableType.Key:
                UpdateAmountTxt(GameData.Ins.key);
                break;

        }
        if (priceTxt)
        {
            priceTxt.text = shopitem.price.ToString();
        }
  
    }
     private void UpdateAmountTxt(int amount)
    {
        if (amountTxt)
        {
            amountTxt.text = amount.ToString();
        }
    }
}
