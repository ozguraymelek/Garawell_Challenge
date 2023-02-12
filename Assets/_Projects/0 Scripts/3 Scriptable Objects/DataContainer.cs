using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "new Data Container",menuName = "Scriptable Objects/Data/Create a new Data Container")]
public class DataContainer : ScriptableObject
{
    [Header("Structs")] 
    [SerializeField] internal PlayerData playerData;
    [SerializeField] internal AIData aIData;
    [SerializeField] internal CarData[] carData;
    [SerializeField] internal SelectionData selectionData;
    [SerializeField] internal MaterialPaletteData materialPaletteData;
}

[Serializable]
public struct PlayerData
{
    [Header("Settings")] 
    [SerializeField] internal string nickname;
    [SerializeField] internal float force;
    [SerializeField] internal int point;
}

[Serializable]
public struct AIData
{
    [Header("Settings")] 
    [SerializeField] internal float force;
    [SerializeField] internal float speed;
}

[Serializable]
public struct SelectionData
{
    [Header("Settings")]
    [SerializeField] internal int activeCarIndex;
    [SerializeField] internal int activeColorIndex;
    [SerializeField] internal int[] activeSkillIndexes;
}

public enum Skills
{
    A,
    B, 
    C
}
[Serializable]
public struct CarData
{
    [Header("Settings")] 
    [SerializeField] internal string model;
    [SerializeField] internal float speed;
    [SerializeField] internal Skills activeSkill;
    [SerializeField] internal float drag;
    [SerializeField] internal float rotateSpeed;
}

[Serializable]
public struct MaterialPaletteData
{
    [Header("Settings")] 
    [SerializeField] internal Material[] palette;
}