using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class InGameTimer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] internal Image imageInGameTime;
    
    [Header("Settings")]
    [SerializeField] internal float nextWaveTransTime;
    [SerializeField] internal float max;

    private void Start()
    {
        this.UpdateAsObservable().Where(_ => nextWaveTransTime > 0 && GameManager.Instance.canStart && Area.Instance.isFalling == false && Area.Instance.currentWave < 5).Subscribe(unit =>
        {
            nextWaveTransTime -= Time.deltaTime;

            imageInGameTime.fillAmount = nextWaveTransTime / max;
        });
        
        this.UpdateAsObservable().Where(_ => Area.Instance.currentWave == 5).Subscribe(unit =>
        {
            Utils.TransitionBetweenUIElements(imageInGameTime.transform, new Vector3(0f, 130f, 0f), .5f,
                () => { imageInGameTime.gameObject.SetActive(false); });
        });

        this.ObserveEveryValueChanged(_ => nextWaveTransTime).Where(_ => nextWaveTransTime < 0).Subscribe(unit =>
        {
            nextWaveTransTime = max;
            imageInGameTime.fillAmount = 1;
            
            Area.Instance.Fall();

            Area.Instance.currentWave++;

            Area.Instance.isFalling = true;
        });
    }
}
