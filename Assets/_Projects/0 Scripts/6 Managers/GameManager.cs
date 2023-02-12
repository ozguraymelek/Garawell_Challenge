using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("Scriptable Objects")] 
    [SerializeField] internal DataContainer container;
    
    [Header("Game Settings")]
    [SerializeField] internal bool canStart;
    [SerializeField] internal bool win = false;
    
    [Header("Camera Components")]
    [SerializeField] internal CinemachineVirtualCamera activeVirtualCamera;

    [Header("Player Components & Settings")] 
    [SerializeField] internal Image imagePlayerPoint;
    [SerializeField] internal float playerPointIncreaseValue;
    [SerializeField] internal float playerPointDecreaseValue;


    [Header("Win Check Components")] 
    [SerializeField] internal Transform panelWin;
    [SerializeField] internal Transform panelLose;

    private void Start()
    {
        this.ObserveEveryValueChanged(_ => AIManager.Instance.spawnedAiPlayers.Count).Where(_ => AIManager.Instance.spawnedAiPlayers.Count == 0).Subscribe(
            unit =>
            {
                WinCheck();
            });
    }

    private void WinCheck()
    {
        DOVirtual.DelayedCall(1.5f, () =>
        {
            win = true;
            
            canStart = false;
            
            container.carData[container.selectionData.activeCarIndex].speed = 0f;
                
            panelWin.gameObject.SetActive(true);
            Utils.TransitionBetweenUIElements(panelWin, new Vector3(-125f, 0f, 0f), 1f);
        });
    }
}