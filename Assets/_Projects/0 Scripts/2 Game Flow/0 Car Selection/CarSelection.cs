using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public sealed class CarSelection : Singleton<CarSelection>
{
    [Header("Scriptable Object Reference")] [SerializeField]
    private DataContainer container;

    [Header("Text Components")]
    [SerializeField] internal TMP_Text modelValueText;
    [SerializeField] internal TMP_Text speedValueText;
    [SerializeField] internal TMP_Text activeSkillValueText;
    [SerializeField] internal TMP_Text dragValueText;
    
    [Header("Settings"), Space(10)] [SerializeField]
    internal List<Car> cars;

    [Header("Game Flow Settings"), Space(10)] [SerializeField]
    internal Car activeCar;

    private void Start()
    {
        GetCarForStart();
        
        this.ObserveEveryValueChanged(index => container.selectionData.activeCarIndex).Subscribe(unit =>
        {
            SwitchCar();
            SetCarInfo(container.selectionData.activeCarIndex);
        });
    }

    private void GetCarForStart()
    {
        activeCar = cars[container.selectionData.activeCarIndex];
        activeCar.gameObject.SetActive(true);
    }

    internal void DecreaseActiveCarIndex()
    {
        activeCar.gameObject.SetActive(false);
        activeCar.enabled = false;
        
        container.selectionData.activeCarIndex--;

        if (container.selectionData.activeCarIndex < 0)
        {
            container.selectionData.activeCarIndex = cars.Count - 1;
        }
    }
    internal void IncreaseActiveCarIndex()
    {
        activeCar.gameObject.SetActive(false);
        activeCar.enabled = false;
        
        container.selectionData.activeCarIndex++;

        if (container.selectionData.activeCarIndex > cars.Count - 1)
        {
            container.selectionData.activeCarIndex = 0;
        }
    }
    
    private void SwitchCar()
    {
        if (activeCar == null) return;

        activeCar = cars[container.selectionData.activeCarIndex];
            
        activeCar.gameObject.SetActive(true);

        activeCar.enabled = true;
    }

    private void SetCarInfo(int index)
    {
        modelValueText.text =
            container.carData[index].model;
        
        speedValueText.text =
            container.carData[index].speed.ToString();
        
        activeSkillValueText.text =
            container.carData[index].activeSkill.ToString();
        
        dragValueText.text =
            container.carData[index].drag.ToString();
    }
}
