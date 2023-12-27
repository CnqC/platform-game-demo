using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnqC.PlatformGame;

public class SoundBtn : MonoBehaviour
{
    private Button m_button;

    private void Awake()
    {
        m_button = GetComponent<Button>();
    }
    private void Start()
    {
        if(m_button != null)
        {
            m_button.onClick.RemoveAllListeners();
            m_button.onClick.AddListener(() => PlaySound());
        }
    }
    private void PlaySound()
    {
        AudioController.ins.PlaySound(AudioController.ins.btnClick);

    }
}
