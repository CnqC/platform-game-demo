using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnqC.PlatformGame;

public class SettingDiaLog : Dialog
{
    // sử lý hộp thoại setting

    public Slider musicSlider;
    public Slider soundSlider;

    public override void Show(bool isShow)
    {
        base.Show(isShow);
        if (musicSlider)
        {   
            // lấy dưới máy ng chơi
            musicSlider.value = GameData.Ins.musicVol;
            // update trên ra Slider
            AudioController.ins.SetMusicVolume(musicSlider.value);
        }

        if (soundSlider)
        {
            // lấy dưới máy ng chơi
            soundSlider.value = GameData.Ins.soundVol;
            // update trên ra Slider
            AudioController.ins.setSoundVolume(soundSlider.value);
        }
    }

    public void OnMusicChange(float value)
    {
        AudioController.ins.SetMusicVolume(value);
    }
    public void OnSoundChange(float value)
    {
        AudioController.ins.setSoundVolume(value);
    }

    public void Save()
    {

        // lấy trong audiocontroller ngoài scence chỉnh --> lưu xuong1 data ng dùng
        GameData.Ins.musicVol = AudioController.ins.musicVolume;
        GameData.Ins.soundVol = AudioController.ins.sfxVolume;
        GameData.Ins.SaveData();
        Close(); // đóng hộp thoại
    }

    public override void Close()
    {
        base.Close();
        Time.timeScale = 1f;
    }
}
