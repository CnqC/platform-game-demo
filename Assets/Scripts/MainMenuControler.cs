using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;
public class MainMenuControler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!Pref.isFirstTime) // not first time
        {
            GameData.Ins.LoadData();
        }
        else
        {
            // first time
            GameData.Ins.SaveData();
            LevelManager.Ins.Init(); 
        }

        // sau khi loạt thao tác cho first 
        Pref.isFirstTime = false;

        // start menu music
        AudioController.ins.PlayMusic(AudioController.ins.menus);
    }
}
