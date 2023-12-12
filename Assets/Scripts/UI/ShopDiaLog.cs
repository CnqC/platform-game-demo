using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnqC.PlatformGame;
using System;

public class ShopDiaLog : Dialog
{
    public Transform gridRoot;
    public ShopItemUI itemUIPb;
    public Text coinCountingTxt;

    public override void Show(bool isShow)
    {
        base.Show(isShow);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinCountingTxt)
        {
            coinCountingTxt.text = GameData.Ins.coin.ToString();

        }

        var shopItems = ShopManager.Ins.items; // mảng items trong shop

        if (shopItems == null || shopItems.Length <= 0) return;
        Helper.ClearChilds(gridRoot);

        for (int i = 0; i < shopItems.Length; i++)
        {
            int shopItemIdx = i;
            var shopItem = shopItems[i];

            if(shopItem != null)
            {
                var itemUIClone = Instantiate(itemUIPb, Vector3.zero, Quaternion.identity);

                itemUIClone.transform.SetParent(gridRoot);

                itemUIClone.transform.localPosition = Vector3.zero;
                itemUIClone.transform.localScale = Vector3.one;

                itemUIClone.UpdateUI(shopItem,shopItemIdx);

                if(itemUIClone.btnComp != null)
                {
                    itemUIClone.btnComp.onClick.RemoveAllListeners();
                    itemUIClone.btnComp.onClick.AddListener(() => ItemEvent(shopItem,shopItemIdx));
                }
            }


        }
    }

    private void ItemEvent(ShopItem shopItem, int shopItemIdx)
    {
        if (shopItem == null) return;

        if(GameData.Ins.coin >= shopItem.price)
        {
            // can buy 

            // giảm giá tiền
            GameData.Ins.coin = GameData.Ins.coin - shopItem.price;

            switch (shopItem.itemtype)
            {
                case CollectableType.Hp:
                    GameData.Ins.hp++;
                    break;
                case CollectableType.Live:
                    GameData.Ins.life++;
                    break;
                case CollectableType.Bullet:
                    GameData.Ins.bullet++;
                    break;
                case CollectableType.Key:
                    GameData.Ins.key++;
                    break;

            }
            GameData.Ins.SaveData();
            UpdateUI(); 
        }
        else
        {
            Debug.Log(" You don't have enough coins");
        }
    }
}
