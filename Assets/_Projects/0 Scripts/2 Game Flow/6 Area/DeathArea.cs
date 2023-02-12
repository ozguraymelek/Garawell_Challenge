using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IPlayer player) == true)
        {
            player.Die();
        }
        
        if (other.TryGetComponent(out IAI ai) == true)
        {
            ai.Die();
        }
    }
}
