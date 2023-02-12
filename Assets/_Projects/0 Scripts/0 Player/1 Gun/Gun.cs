using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] internal Renderer renderer;
    
    [Header("Settings")] 
    [SerializeField] internal bool canTouchable = true;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out IDamageable car) == false) return;
        
        var gun = collision.collider.GetComponentInChildren<Gun>();
        if (gun.canTouchable == false) return;
        
        car.Damage(collision.rigidbody);
        
        var contact = collision.GetContact(0);

        EffectManager.Instance.HitEffect2(contact.point, Quaternion.identity);
    }
}
