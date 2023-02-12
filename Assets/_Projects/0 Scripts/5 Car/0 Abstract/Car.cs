using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public abstract class Car : MonoBehaviour
{
    [Header("Scriptable Objects")] 
    [SerializeField] internal DataContainer container;
    
    [Header("Script References")]
    [SerializeField] internal Gun gun;
    
    [Header("Components")]
    [SerializeField] internal Renderer renderer;
    [SerializeField] internal TMP_Text textNickname;
    [SerializeField] internal FloatingJoystick joystick;
    [SerializeField] internal Rigidbody rb;
    [SerializeField] internal BoxCollider bCollider;

    [Header("Settings")] 
    [SerializeField] internal bool canControl = false;
    
    public abstract void Skill();

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
        
        canControl = collision.gameObject.layer == LayerMask.NameToLayer("Ground");
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
        
        canControl = other.gameObject.layer != LayerMask.NameToLayer("Ground");
    }

    protected void Control()
    {
        if (canControl == false) return;
        if (GameManager.Instance.canStart == false) return;
        
        var x = joystick.Horizontal * container.carData[container.selectionData.activeCarIndex].speed *
                Time.fixedDeltaTime;
        var z = joystick.Vertical * container.carData[container.selectionData.activeCarIndex].speed *
                Time.fixedDeltaTime;
        
        rb.velocity = new Vector3(x, 0f, z);

        if (joystick.Horizontal == 0 && joystick.Vertical == 0) return;
        
        var lookRotation = Quaternion.LookRotation(rb.velocity);
 
        var step = container.carData[container.selectionData.activeCarIndex].rotateSpeed * Time.fixedDeltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, step);
    }
}
