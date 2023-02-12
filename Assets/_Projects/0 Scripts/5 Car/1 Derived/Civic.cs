using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Civic : Car, IDamageable, IPlayer
{
    private void OnEnable()
    {
        SkillSelection.Instance.GetActiveSkillData();
        ColorSelection.Instance.GetActiveColorData();
        
        CarSelection.Instance.activeCar.textNickname.text = container.playerData.nickname;
    }
    
    private void Start()
    {
        this.FixedUpdateAsObservable().Where(_ => GameManager.Instance.canStart).Subscribe(unit =>
        {
            Control();
        });
    }

    public void Damage(Rigidbody rb)
    {
        if (gun.canTouchable == false)
            container.aIData.force = 0;
        
        rb.AddRelativeForce(new Vector3(container.aIData.force, container.aIData.force, 
            container.aIData.force));
        
        print("AI hit Player!");
        
        if (SkillCharger.Instance.playerPoint >= 100) return;
                
        var instanceGM = GameManager.Instance;
        var instanceSC = SkillCharger.Instance;

        instanceSC.playerPoint -= instanceGM.playerPointDecreaseValue;
        instanceGM.imagePlayerPoint.fillAmount = (instanceSC.playerPoint / 100);
    }

    public void Die()
    {
        DOVirtual.DelayedCall(1.5f, () =>
        {
            GameManager.Instance.win = false;
            
            gameObject.SetActive(false);
                
            container.carData[container.selectionData.activeCarIndex].speed = 0f;
                
            GameManager.Instance.panelLose.gameObject.SetActive(true);
            Utils.TransitionBetweenUIElements(GameManager.Instance.panelLose, new Vector3(-125f, 0f, 0f), 1f);
        });
    }

    public override void Skill()
    {
        if (container.carData[container.selectionData.activeCarIndex].activeSkill == Skills.A)
            SkillCharger.Instance.SkillA();
        
        if (container.carData[container.selectionData.activeCarIndex].activeSkill == Skills.B)
            SkillCharger.Instance.SkillB();
        
        if (container.carData[container.selectionData.activeCarIndex].activeSkill == Skills.C)
            SkillCharger.Instance.SkillC();
    }
}