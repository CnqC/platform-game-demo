using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;
using MonsterLove.StateMachine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public GamePlaySetting setting;

    public Player player;

    public FreeParallax map;
    private StateMachine<GameState> m_fsm;

    private int m_curLive;
    private int m_curCoin;
    private int m_curKey;
    private int m_curBullet;
    private float m_gamePlayTime;
    private int m_goalStar; // how many star player get ?

    public int CurLive { get => m_curLive; set => m_curLive = value; }
    public int CurCoin { get => m_curCoin; set => m_curCoin = value; }
    public int CurKey { get => m_curKey; set => m_curKey = value; }
    public int CurBullet { get => m_curBullet; set => m_curBullet = value; }
    public float GameplayTime { get => m_gamePlayTime; }
    public int GoalStar { get => m_goalStar;  }
    public StateMachine<GameState> Fsm { get => m_fsm; }

    private void Start()
    {
        
        LoadData();

        StartCoroutine(CamFollowDeLay());

        if (setting.isOnMoblie)
        {
            // on moblie
            GUIManager.Ins.showMobileGamePad(true);
        }
        else
        {
            // on pc
            GUIManager.Ins.showMobileGamePad(false);
        }

        AudioController.ins.PlayBackgroundMusic();
    }

     private void LoadData()
    {
        m_curLive = setting.startingLive;
        m_curBullet = setting.startingBullet;

        if(GameData.Ins.life != 0)
        {
            // đã có life r
            m_curLive = GameData.Ins.life;
        }
        if(GameData.Ins.bullet != 0)
        {
            m_curBullet = GameData.Ins.bullet;
        }

        if(GameData.Ins.key != 0)
        {
            m_curKey = GameData.Ins.key;
        }

        if(GameData.Ins.hp != 0)
        {
            player.CurHp = GameData.Ins.hp;
        }

        // check point
        Vector3 checkPoint = GameData.Ins.GetCheckPoint(LevelManager.Ins.CurlevelId);
        
        if(checkPoint != Vector3.zero)
        {
            player.transform.position = checkPoint;
        }

        float gamePlayTime = GameData.Ins.GetPlayTime(LevelManager.Ins.CurlevelId);
        if(gamePlayTime >0)
        {
            m_gamePlayTime = gamePlayTime;
        }

        GUIManager.Ins.UpdateLife(m_curLive);
        GUIManager.Ins.UpdateHp(player.CurHp);
        GUIManager.Ins.UpdateKey(m_curKey);
        GUIManager.Ins.UpdateCoin(m_curCoin);
        GUIManager.Ins.UpdateBullet(m_curBullet);
        GUIManager.Ins.UpdatePlaytime(Helper.TimeConvert(m_gamePlayTime));
        
    }

    public void BackToCheckPoint()
    {
        player.transform.position = GameData.Ins.GetCheckPoint(LevelManager.Ins.CurlevelId);
    }

    public void Revice() 
    { // Player hết máu
        m_curLive--;
        player.CurHp = player.stat.hp;
        GameData.Ins.hp = player.CurHp;
        GameData.Ins.life = m_curLive;
        GameData.Ins.SaveData();
        BackToCheckPoint();
        GUIManager.Ins.UpdateLife(GameData.Ins.life); 
    } 

    public void AddCoins(int coins)
    {
        m_curCoin += coins;
        GameData.Ins.coin += coins;
        GameData.Ins.SaveData();

        GUIManager.Ins.UpdateCoin(m_curCoin);
    }

    public void ReduceBullet()
    {
        m_curBullet--;
        GameManager.Ins.CurBullet = m_curBullet;
        GameData.Ins.SaveData();
        GUIManager.Ins.UpdateBullet(m_curBullet);
    }

    public void Replay()
    {
        SceneController.Ins.LoadLevelScene(LevelManager.Ins.CurlevelId);
    }

    public void NextLevel()
    {
        LevelManager.Ins.CurlevelId++;

        //check trong thằng CountingBuildSetting có bao nhiêu sence trong mảng sceneCountInBuildSettings
        if (LevelManager.Ins.CurlevelId >= SceneManager.sceneCountInBuildSettings -1)
        {
            // nếu mà Id hiện tại vượt qua số lượng sence trong list thi --> quay laị màn hình Menu
            SceneController.Ins.LoadScene(GameScene.MainMenu.ToString());
        }
        else
        { // còn level -> load level tiếp 
            SceneController.Ins.LoadLevelScene(LevelManager.Ins.CurlevelId);
        }
    }

    public void SaveCheckPoint()
    {
        // cập nhập thời gian mà player đã chơi
        GameData.Ins.UpdatePlayTime(LevelManager.Ins.CurlevelId,m_gamePlayTime);
        GameData.Ins.UpdateCheckPoint(LevelManager.Ins.CurlevelId, new Vector3(
            player.transform.position.x - 0.5f,
            player.transform.position.y + 1.5f,
            player.transform.position.z
            )); ;

        GameData.Ins.SaveData();
    }

    public void LevelFail()
    {
        m_fsm.ChangeState(GameState.GameOver);
        // reload score
        GameData.Ins.UpdateLevelScore(LevelManager.Ins.CurlevelId,
            Mathf.RoundToInt(m_gamePlayTime) //Mathf.RoundToInt làm tròn
            ); ;

        GameData.Ins.SaveData();
        StartCoroutine(showDiaLogLevelFailDelay());
    }

    private IEnumerator showDiaLogLevelFailDelay()
    {
        //show cái diaLog trễ hơn 1s sau khi chết
        yield return new WaitForSeconds(1);


        // hiện Dialog và phát âm thanh 
        if (GUIManager.Ins.lvFailDiaLog)
        {
            GUIManager.Ins.lvClearDiaLog.Show(true);
        }

        AudioController.ins.PlaySound(AudioController.ins.fail);
    }


    public void LevelClear()
    {
        if (map)
        {
            m_fsm.ChangeState(GameState.Win);
            GameData.Ins.UpdateLevelScore(LevelManager.Ins.CurlevelId, Mathf.RoundToInt(m_gamePlayTime));
            m_goalStar = LevelManager.Ins.CurLevel.goal.GetStar(Mathf.RoundToInt(m_gamePlayTime));
            GameData.Ins.SaveData();
        }

        StartCoroutine(showDiaLogLevelClearDelay());
    }
    private IEnumerator showDiaLogLevelClearDelay()
    {
        //show cái diaLog trễ hơn 1s sau khi thắng
        yield return new WaitForSeconds(1);


        // hiện Dialog và phát âm thanh 
        if (GUIManager.Ins.lvClearDiaLog)
        {
            GUIManager.Ins.lvClearDiaLog.Show(true);
        }

        AudioController.ins.PlaySound(AudioController.ins.missionComplete);
    }
    private IEnumerator CamFollowDeLay()
    {
        yield return new WaitForSeconds(0.3f);
        CameraFollow.ins.target = player.transform;
    }
    public override void Awake()
    {
        // base.Awake();  hủy khi load scene 

        MakeSingleTon(false); // hủy gameObject khi load sang scene khác

        // create FSM
        m_fsm = StateMachine<GameState>.Initialize(this);
        m_fsm.ChangeState(GameState.Playing);
    }

    public void SetMapSpeed(float speed)
    {
        if (map)
        {
            map.Speed = speed;
        }
    }

    #region FSM
    protected void Starting_Enter() { }
    protected void Starting_Update() { }
    protected void Starting_Exit() { }
    protected void Playing_Enter() { }
    protected void Playing_Update() 
    {
        if (GameData.Ins.IsLevelPassed(LevelManager.Ins.CurlevelId)) return; 

        m_gamePlayTime += Time.deltaTime; // tăng gian ng chơi lên

        GUIManager.Ins.UpdatePlaytime(Helper.TimeConvert(m_gamePlayTime)); // timeconvert chuyển từ số sang thời gian phút, giây
    }
    protected void Playing_Exit() { }
    protected void Win_Enter() { }
    protected void Win_Update() { }
    protected void Win_Exit() { }
    protected void GameOver_Enter() { }
    protected void GameOver_Update() { }
    protected void GameOver_Exit() { }

    #endregion
}
