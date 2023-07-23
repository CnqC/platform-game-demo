using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public enum Direction // các hướng di chuyển trong Game
{
    left,
    Right,
    Up,
    Down,
    None
}

public enum GameState // lưu lại các trạng thái của game 
{
    Starting,
    Playing,
    Win,
    GameOver
}

public enum GamePref // lưu lại các key khi ta sử dụng lưu xuống phần Pref
{
    GameData,
    IsFirstTime,

}

public enum GameTag // lưu lại các tag trong game
{
    Player,
    Enemy,
    MovingPlatform,
    Thorn,
    CollectTable,
    CheckPoint,
    Door,
    DeadZone
}

public enum GameScene // lại lại scene
{
    MainMenu,
    GamePlay,
    Level_
}

public enum SpriteOrder
{
    Normal = 5,
    Inwater = 2
}

public enum PlayerAnimState // các trạng thái (state) ở trong phần animator
{
    SayHello,
    Walk,
    Jump,
    OnAir,
    Land,
    Swim,
    FireBullet,
    Fly,
    FlyOnAir,
    SwimOnDeep,
    OnLadder,
    Dead,
    Idle,
    LadderIdle,
    HammerAttack,
    GotHit
}

public enum EnemyAnimState // các trạng thái của enemy
{
    Moving,
    Chasing,
    GotHit,
    Dead
}

public enum DetectMethod
{
    RayCast,
    CircleOverlap
}

public enum PlayerCollider // collider cho từng loại của player
{
    Default,
    Flying,
    InWater,
    None
}
public enum CollectableType // các dạng collectable
{
    Hp,
    Live,
    Bullet,
    Key,
    None
}