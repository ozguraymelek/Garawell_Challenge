using System;
using System.Collections;
using System.Collections.Generic;
using Garawell_Case.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class GameStartTimer : Singleton<GameStartTimer>
{
    [Header("Components")] 
    [SerializeField] internal TMP_Text textGameStartTime;
    [SerializeField] internal Image imageGameStartTime;
    
    [Header("Settings")]
    [SerializeField] internal float gameStartTime;
    [SerializeField] internal float max;

    private void Start()
    {
        this.UpdateAsObservable().Where(_ => gameStartTime > 0).Subscribe(unit =>
        {
            gameStartTime -= Time.deltaTime;

            imageGameStartTime.fillAmount = gameStartTime / max;
        });
    }
}
