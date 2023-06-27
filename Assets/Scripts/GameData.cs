using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class GameData : SingleTon<GameData>
{
    public int coin;
    public int curlevelId;
    public float musicVol;
    public float soundVol;

    public int hp;
    public int life;
    public int bullet;
    public int ket;
      
    public List<Vector3> checkPoints; // checkpoint của từng map
    public List<bool> levelUnlocked; // ktra level đã mở khóa hay chưa
    public List<bool> levelPasseds;
    public List<float> gamePlayTime; // lưu lại thời gian chơi
    public List<float> completeTimes; // thời gian hoàn thành mỗi level

    public void SaveData()
    {
        Pref.GameData = JsonUtility.ToJson(this); // chuyển tất cả các thông tin được lưu xuống thành dạng json 
        
    
    }

    public void LoadData()
    {
        string data = Pref.GameData; // lấy dữ liệu json từ GameData
        if (string.IsNullOrEmpty(data)) return;

        JsonUtility.FromJsonOverwrite(data, this); // đè lại dữ liệu mới xuống máy ng dùng
    }

    private T GetValue <T> (List<T> datalist, int idx)
    {                                                                   // phần tử ttong datalist
        if (datalist == null || datalist.Count <= 0 || idx < 0 || idx > datalist.Count) return default  ; // return lại giá trị mặc định của kiểu dự liệu là Default

        return datalist[idx]; // return phần tử thứ idx của datalist

    }
                                                     // chỉ số phần tử, giá trị chúng ta cần cập nhập
    private void  UpdateValue<T> (ref List<T> datalist, int idx, T value)
    {
        if (datalist == null|| idx < 0) return;

        if(datalist.Count <=0 || (datalist.Count >0 && idx > datalist.Count))
        {
            datalist.Add(value);
        }
        else // cập nhập lại phần tử thứ idx và gán cho biến value
        {
            datalist[idx] = value;
        }
    }

    #region LEVEL
    public bool GetLevelUnlocked(int Id)
    {
        return GetValue<bool>(levelUnlocked, Id);
    }

    public void UpdateLevelUnlocked(int Id, bool isUnlocked) // cập nhập lại các giá trị trong listLevelUnlocked theo cái chỉ số id
    {
        UpdateValue<bool>(ref levelUnlocked, Id, isUnlocked);
    }



    public bool GetLevelPassed(int Id)
    {
        return GetValue<bool>(levelPasseds, Id);
    }

    public void UpdateLevelPassed(int Id, bool isPassed) // cập nhập lại các giá trị trong listLevelPasseds theo cái chỉ số id
    {
        UpdateValue<bool>(ref levelPasseds, Id, isPassed);
    }



    public float GetLevelScore(int levelId)
    {
        return GetValue<float>(completeTimes, levelId);
    }

    public void UpdateLevelScore(int levelId, float completeTime) // cập nhập lại các giá trị của completeTimes theo cái chỉ số levelId với giá trị truyền vào là completetime
    {
        float oldCompleteTime = GetLevelScore(levelId); // socre cũ

        if(completeTime < oldCompleteTime) // thời giam hoàm thành màn chơi nó nhỏ hơn đã từng hoàng thành -> lưu
        UpdateValue<float>(ref completeTimes, levelId, completeTime);
    }


    public void UpdateLevelSocreNoneCheck(int levelId, float completeTime) // cập nhập lại socre mà k cần đk
    {
        UpdateValue<float>(ref completeTimes, levelId, completeTime);
    }


    public bool GetCheckPoint(int levelId) // lấy ra 1 checkpoint ứng với từng level 
    {
        return GetValue<bool>(levelPasseds, levelId);
    }

    public void UpdateCheckPoint(int levelId, Vector3 checkPoint) // cập nhập lại các checkPoint ứng với các level với giá trị..
    {
        UpdateValue<Vector3>(ref checkPoints, levelId, checkPoint);
    }



    public float GetPlayTime(int levelId) // lấy ra 1 thời gian chơi ứng vói levelId
    {
        return GetValue<float> (gamePlayTime , levelId);
    }

    public void UpdatePlayTime(int levelId, float playTime) // cập nhập lại các giá trị thời gian chơi ứng với chỉ số Levelid
    {
        UpdateValue<float> (ref gamePlayTime, levelId  , playTime);
    }

    public bool IsLevelUnlocked(int id) // level đã mở chưa
    {
        if (levelUnlocked == null || levelUnlocked.Count <= 0) return false; // null hay k có ptu --> returns

        return levelUnlocked[id]; // có -> trả về lvUnlocked tại chỉ số id

    }

    public bool IsLevelPassed(int id)
    {
        if (levelPasseds == null || levelPasseds.Count <= 0) return false; // null hay k có ptu --> returns

        return levelPasseds[id]; // có -> trả về lvUnlocked tại chỉ số id

    }
    #endregion
}
