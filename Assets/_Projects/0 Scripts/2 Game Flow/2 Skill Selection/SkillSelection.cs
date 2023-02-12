using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Editor;
using UnityEngine;
using UnityEngine.UI;

public sealed class SkillSelection : Singleton<SkillSelection>
{
    [Header("Scriptable Object Reference")] 
    [SerializeField] private DataContainer container;

    [Header("Settings")] 
    [SerializeField] internal Button[] skillButtons;
    
    public void SetActiveSkill(string state)
    {
        var tempActiveSkill = (Skills)Enum.Parse(typeof(Skills), state, true); 
        
        container.carData[container.selectionData.activeCarIndex].activeSkill = tempActiveSkill;
        
        container.selectionData.activeSkillIndexes[container.selectionData.activeCarIndex] = (int)tempActiveSkill;
        
        ResetColors();
        
        skillButtons[container.selectionData.activeSkillIndexes[container.selectionData.activeCarIndex]].image.color = Color.red;
    }

    internal void GetActiveSkillData()
    {
        ResetColors();
        
        skillButtons[container.selectionData.activeSkillIndexes[container.selectionData.activeCarIndex]].image.color = Color.red;
    }

    private void ResetColors()
    {
        foreach (var skillButton in skillButtons)
        {
            skillButton.image.color = Color.white;
        }
    }
}