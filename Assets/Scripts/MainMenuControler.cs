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
            // lưu xuống máy ng chơi âm thanh, nhạc lần đầu
            GameData.Ins.musicVol = AudioController.ins.musicVolume;
            GameData.Ins.soundVol = AudioController.ins.sfxVolume;
            GameData.Ins.SaveData();

            // first time
            GameData.Ins.SaveData();
            LevelManager.Ins.Init(); 
        }

        AudioController.ins.setSoundVolume(GameData.Ins.soundVol);
        AudioController.ins.SetMusicVolume(GameData.Ins.musicVol);
        // sau khi loạt thao tác cho first 
        Pref.isFirstTime = false;

        // start menu music
        AudioController.ins.PlayMusic(AudioController.ins.menus);
    }
}
