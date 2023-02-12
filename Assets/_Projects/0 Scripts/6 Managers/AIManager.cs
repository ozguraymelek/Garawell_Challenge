using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : Singleton<AIManager>
{
    [Header("Settings")] 
    [SerializeField] internal List<AI> spawnedAiPlayers;
}
