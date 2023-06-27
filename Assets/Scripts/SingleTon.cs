using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnqC.PlatformGame;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    static T m_ins;

    public static T Ins
    {
        get
        {
            return m_ins;

        }
    }

    public virtual void Awake()
    {
        MakeSingleTon(true);
    }

    public void MakeSingleTon( bool destroyOnLoad)
    {
        if(m_ins == null)
        {
            m_ins = this as T;
            if (destroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
