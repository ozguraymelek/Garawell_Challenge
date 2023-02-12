using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class Nickname : MonoBehaviour
{
    [Header("Scriptable Objects")] 
    [SerializeField] internal DataContainer container;
    
    [Header("Components")] 
    [SerializeField] internal TMP_InputField inputNickname;

    private void Start()
    {
        inputNickname.onValueChanged.AddListener(unit =>
        {
            SetNickname();
        });
    }

    public void SetNickname()
    {
        container.playerData.nickname = inputNickname.text;
        CarSelection.Instance.activeCar.textNickname.text = container.playerData.nickname;
    }
}