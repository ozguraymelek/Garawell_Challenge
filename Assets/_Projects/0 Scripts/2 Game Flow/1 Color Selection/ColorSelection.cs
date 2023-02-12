using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelection : Singleton<ColorSelection>
{
    [Header("Scriptable Object Reference")] [SerializeField]
    private DataContainer container;

    [Header("Components")] 
    [SerializeField] internal Button[] colorButtons;
    [SerializeField] internal Material[] mats;
    
    internal void GetActiveColorData()
    {
        CarSelection.Instance.activeCar.gun.renderer.sharedMaterial.color =
            CarSelection.Instance.activeCar.renderer.sharedMaterials[0].color;
    }
    
    public void SwitchCarMaterial(int index)
    {
        CarSelection.Instance.activeCar.renderer.sharedMaterials[0].color =
            container.materialPaletteData.palette[index].color;

        CarSelection.Instance.activeCar.gun.renderer.sharedMaterial.color =
            container.materialPaletteData.palette[index].color;

        container.selectionData.activeColorIndex = index;

        print("Color Changed!");
    }
}
