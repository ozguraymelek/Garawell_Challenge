using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SkillCharger : Singleton<SkillCharger>
{
    [Header("Scriptable Objects")] 
    [SerializeField] internal DataContainer container;
    
    [Header("Settings")]
    [SerializeField] internal float playerPoint;
    [SerializeField] internal Transform imageTouch;
    [SerializeField] internal bool canUseSkill = false;
    [SerializeField] internal TweenerCore<Vector3, Vector3, VectorOptions> tween;

    [Header("Timer Components")] 
    [SerializeField] internal Image imageSkillTimer;
    
    [Header("Timer Settings")] 
    [SerializeField] internal float skillATime;
    [SerializeField] internal float skillBTime;
    [SerializeField] internal float skillCTime;
    

    private void Start()
    {
        this.ObserveEveryValueChanged(x => playerPoint).Where(x => playerPoint == 100).Subscribe(unit =>
        {
            playerPoint = 100;
            imageTouch.gameObject.SetActive(true);
            tween = imageTouch.transform.DOLocalMoveY(650f, 2f).SetLoops(-1, LoopType.Restart);
            print("KAC DEFA");
            
        });

        #region Update Observables for Timer

        this.UpdateAsObservable().Where(_ => skillATime > 0 && container.carData[container.selectionData.activeCarIndex].activeSkill == Skills.A && canUseSkill).Subscribe(unit =>
        {
            skillATime -= Time.deltaTime;

            imageSkillTimer.fillAmount = skillATime / 3.5f;
        });
        
        this.UpdateAsObservable().Where(_ => skillBTime > 0 && container.carData[container.selectionData.activeCarIndex].activeSkill == Skills.B && canUseSkill).Subscribe(unit =>
        {
            skillBTime -= Time.deltaTime;

            imageSkillTimer.fillAmount = skillBTime / 4f;
        });
        
        this.UpdateAsObservable().Where(_ => skillCTime > 0 && container.carData[container.selectionData.activeCarIndex].activeSkill == Skills.C && canUseSkill).Subscribe(unit =>
        {
            skillCTime -= Time.deltaTime;

            imageSkillTimer.fillAmount = skillCTime / 3f;
        });

        #endregion

        #region Observe Value Changed

        this.ObserveEveryValueChanged(x => skillATime).Where(x => skillATime < 0 && 
                                                                  container.carData[container.selectionData.activeCarIndex].activeSkill == Skills.A).
            Subscribe(unit =>
            {
                skillATime = 0;
                ResetSkillABenefits();
            });
        
        this.ObserveEveryValueChanged(x => skillBTime).Where(x => skillBTime < 0 && 
                                                                  container.carData[container.selectionData.activeCarIndex].activeSkill == Skills.B).
            Subscribe(unit =>
            {
                skillBTime = 0;
                ResetSkillBBenefits();
            });
        
        this.ObserveEveryValueChanged(x => skillCTime).Where(x => skillCTime < 0 && 
                                                                  container.carData[container.selectionData.activeCarIndex].activeSkill == Skills.C).
            Subscribe(unit =>
            {
                skillCTime = 0;
                ResetSkillCBenefits();
            });

        #endregion
    }

    public void Clicked_SkillButton()
    {
        if (playerPoint == 100)
        {
            playerPoint = 0;
            
            imageSkillTimer.gameObject.SetActive(true);
            
            CarSelection.Instance.activeCar.Skill();
            canUseSkill = true;
        }
            
    }

    public void SkillA() //increase scale, but decrease speed - 3.5s
    {
        CarSelection.Instance.activeCar.gun.transform.DOScale(new Vector3(1f, 1f, 1f), 1f);

        container.carData[container.selectionData.activeCarIndex].speed -=
            container.carData[container.selectionData.activeCarIndex].speed / 2;
    }
    
    public void SkillB() //increase force, but grow collider for x axis - 4s
    {
        container.playerData.force += (container.playerData.force / 2) + container.playerData.force;
        CarSelection.Instance.activeCar.bCollider.size = new Vector3(2f, .7f, 2.3f);
    }
    
    public void SkillC() //untouchable, but low scale - 3s
    {
        CarSelection.Instance.activeCar.gun.canTouchable = false;
        // CarSelection.Instance.activeCar.gun.transform.localScale -= new Vector3(.2f, .2f, .2f);
        CarSelection.Instance.activeCar.gun.transform.DOScale(new Vector3(.25f, .25f, .25f), 1f);
    }

    private void ResetSkillABenefits()
    {
        canUseSkill = false;
        
        imageSkillTimer.gameObject.SetActive(false);
        imageTouch.gameObject.SetActive(false);
        imageSkillTimer.fillAmount = 1;
        
        skillATime = 3.5f;
        
        tween.Kill();
        imageTouch.transform.DOLocalMoveY(250, .5f);
        
        GameManager.Instance.imagePlayerPoint.fillAmount = 0;
        
        // CarSelection.Instance.activeCar.gun.transform.localScale = new Vector3(.5f, .5f, .5f);
        CarSelection.Instance.activeCar.gun.transform.DOScale(new Vector3(.5f, .5f, .5f), 1f);
        container.carData[container.selectionData.activeCarIndex].speed = 300f;
    }
    
    private void ResetSkillBBenefits()
    {
        canUseSkill = false;
        
        imageSkillTimer.gameObject.SetActive(false);
        imageTouch.gameObject.SetActive(false);
        imageSkillTimer.fillAmount = 1;

        skillBTime = 4f;
        
        tween.Kill();
        imageTouch.transform.DOLocalMoveY(250, .5f);
        
        GameManager.Instance.imagePlayerPoint.fillAmount = 0;
        
        container.playerData.force = 250f;
        CarSelection.Instance.activeCar.bCollider.size = new Vector3(.86f, .7f, 2.3f);
    }
    
    private void ResetSkillCBenefits()
    {
        canUseSkill = false;
        
        imageSkillTimer.gameObject.SetActive(false);
        imageTouch.gameObject.SetActive(false);
        imageSkillTimer.fillAmount = 1;
        
        skillCTime = 3f;

        tween.Kill();
        imageTouch.transform.DOLocalMoveY(250, .5f);
        
        GameManager.Instance.imagePlayerPoint.fillAmount = 0;
        
        CarSelection.Instance.activeCar.gun.canTouchable = true;
        // CarSelection.Instance.activeCar.gun.transform.localScale -= new Vector3(.5f, .5f, .5f);
        CarSelection.Instance.activeCar.gun.transform.DOScale(new Vector3(.5f, .5f, .5f), 1f);
    }
}