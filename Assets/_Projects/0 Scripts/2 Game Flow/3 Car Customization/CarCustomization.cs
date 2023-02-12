using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CarCustomization : MonoBehaviour
{
    [Header("Scriptable Objects References")] 
    [SerializeField] private DataContainer container;

    [Header("Components")] 
    [SerializeField] private Slider sliderDrag;
    [SerializeField] private Slider sliderSpeed;

    private void Start()
    {
        Subscribe();
        
        sliderDrag.onValueChanged.AddListener(unit =>
        {
            SetDragData();
        });
        
        sliderSpeed.onValueChanged.AddListener(unit =>
        {
            SetSpeedData();
        });
    }

    public void SetDragData()
    {
        container.carData[container.selectionData.activeCarIndex].drag = sliderDrag.value;
    }
    
    public void SetSpeedData()
    {
        container.carData[container.selectionData.activeCarIndex].speed = sliderSpeed.value;
    }

    private void Subscribe()
    {
        var instance = CarSelection.Instance;

        sliderDrag.OnValueChangedAsObservable()
            .SubscribeToText(instance.dragValueText, x => sliderDrag.value.ToString(".##"));

        sliderSpeed.OnValueChangedAsObservable()
            .SubscribeToText(instance.speedValueText);
    }
}
