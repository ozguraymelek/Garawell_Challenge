using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

public class AI : MonoBehaviour, IDamageable, IAI
{
    [Header("Scriptable Objects References")] 
    [SerializeField] private DataContainer container;

    [Header("Components")]
    [SerializeField] private Rigidbody rb;

    [Header("Settings")] 
    [SerializeField] internal float distance;
    [SerializeField] internal float randX;
    [SerializeField] internal float randZ;
    [SerializeField] internal float randRotate;
    [SerializeField] internal float nextPosTransTime;
    [SerializeField] internal float maxNextPosTransTime;
    [SerializeField] internal float stopRotateDecisionTime;
    [SerializeField] internal float maxStopRotateDecisionTime;
    [SerializeField] internal bool canControl;
    
    private void Start()
    {
        randX = Random.Range(-1, 1);
        randZ = Random.Range(-1, 1);
        
        this.FixedUpdateAsObservable().Where(_ => GameManager.Instance.canStart).Subscribe(unit =>
        {
            Control();
        });
        
        this.UpdateAsObservable().Where(_ => nextPosTransTime > 0 && GameManager.Instance.canStart).Subscribe(unit =>
        {
            nextPosTransTime -= Time.deltaTime;
        });
        this.ObserveEveryValueChanged(_ => nextPosTransTime).Where(_ => nextPosTransTime < 0).Subscribe(unit =>
        {
            nextPosTransTime = Random.Range(0.5f, 2f);
            nextPosTransTime = maxNextPosTransTime;
            
            randX = Random.Range(-1, 1);
            randZ = Random.Range(-1, 1);
        });
        
        this.UpdateAsObservable().Where(_ => stopRotateDecisionTime > 0 && GameManager.Instance.canStart).Subscribe(unit =>
        {
            stopRotateDecisionTime -= Time.deltaTime;
        });
        this.ObserveEveryValueChanged(_ => stopRotateDecisionTime).Where(_ => stopRotateDecisionTime < 0).Subscribe(unit =>
        {
            maxStopRotateDecisionTime = Random.Range(2f, 5f);
            stopRotateDecisionTime = maxStopRotateDecisionTime;

            // canControl = true;
        });
        
        this.UpdateAsObservable().Where(_ => DistanceBetweenPlayer() < 4f && GameManager.Instance.canStart).Subscribe(unit =>
        {
            randRotate = Random.Range(0, 4);

            if (randRotate is 0 or 1 or 2) return;
            
            // canControl = false;
            transform.RotateAround(transform.position, Vector3.up, 360 * Time.deltaTime);
        });
    }
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

    private void Control()
    {
        if (canControl == false) return;
        
        var x = randX * container.aIData.speed *
                Time.fixedDeltaTime;
        
        var z = randZ * container.aIData.speed *
                Time.fixedDeltaTime;

        rb.velocity = new Vector3(x, 0f, z);

        if (randX == 0 && randZ == 0) return;

        if (DistanceBetweenPlayer() < 4) return;
        
        var lookRotation = Quaternion.LookRotation(rb.velocity);
 
        var step = container.carData[container.selectionData.activeCarIndex].rotateSpeed * Time.fixedDeltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, step);
    }

    private float DistanceBetweenPlayer()
    {
        distance = Vector3.Distance(transform.position, CarSelection.Instance.activeCar.transform.position);
        print(distance);

        return distance;
    }

    public void Damage(Rigidbody rb)
    {
        rb.AddRelativeForce(new Vector3(container.playerData.force, container.playerData.force, 
            container.playerData.force));

        print("Player hit AI!");
        
        if (SkillCharger.Instance.playerPoint >= 100) return;
                
        var instanceGM = GameManager.Instance;
        var instanceSC = SkillCharger.Instance;

        instanceSC.playerPoint += instanceGM.playerPointIncreaseValue;
        instanceGM.imagePlayerPoint.fillAmount = (instanceSC.playerPoint / 100);
    }
    
    public void Die()
    {
        DOVirtual.DelayedCall(1f, () =>
        {
            gameObject.SetActive(false);
            AIManager.Instance.spawnedAiPlayers.Remove(this);
        });
    }
}
