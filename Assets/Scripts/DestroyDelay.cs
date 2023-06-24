using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{
    public float timeToDestroy;
    public bool isOverride;

    private void Awake()
    {
        if(!isOverride)
            DestroyObj();
    }

    public void DestroyObj()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
