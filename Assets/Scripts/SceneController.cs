using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;
using UnityEngine.SceneManagement;

public class SceneController : SingleTon<SceneController>
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevelScene(int level)
    {
        string sceneName = GameScene.Level_.ToString() + level; // trở thành level_1,level_2,... tùy vào tham số level ta đưa vào
        LoadScene(sceneName);
    }
}
