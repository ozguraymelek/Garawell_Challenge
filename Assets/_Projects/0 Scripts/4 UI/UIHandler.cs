using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Garawell_Case.UI
{
    public sealed class UIHandler : MonoBehaviour
    {
        [Header("Scriptable Objects")] 
        [SerializeField] internal DataContainer container;
        
        [Header("Canvas Components"), Space(10)]
        [SerializeField] internal GameObject canvasForMenu;
        [SerializeField] internal GameObject canvasForController;
        [SerializeField] internal GameObject canvasForSkillCharger;
        
        [Header("Button Components"), Space(10)]
        [SerializeField] internal Transform buttonChangeColor;
        [SerializeField] internal Transform buttonSelectCar;
        [SerializeField] internal Transform buttonRight;
        [SerializeField] internal Transform buttonLeft;
        [SerializeField] internal Transform buttonPlay;
        
        [Header("Panel Components"), Space(10)]
        [SerializeField] internal Transform panelChangeColor;
        [SerializeField] internal Transform panelSelectCar;
        [SerializeField] internal Transform panelSelectSkill;
        [SerializeField] internal Transform panelGameStartTimer;
        [SerializeField] internal Transform panelSkillCharger;
        [SerializeField] internal Transform panelInGameTimer;

        [Header("Input Components"), Space(10)]
        [SerializeField] internal Transform inputNickname;

        [Header("Camera Settings"), Space(10)] 
        [SerializeField] internal CinemachineVirtualCamera[] virtualCameras;
        
        [Header("Settings"), Space(10)] 
        [SerializeField] internal Transform activePanel;

        private void Start()
        {
            
        }


        public void Clicked_AnyMainButton(string buttonName)
        {
            switch (buttonName)
            {
                case "Change Color":
                    ChangeColor_SelectCarActions("Change Color");
                    break;
                
                case "Select Car":
                    ChangeColor_SelectCarActions("Select Car");
                    break;
                
                case "Back":
                    Utils.TransitionBetweenUIElements(activePanel, new Vector3(0f, -1800f, 0f), 1f,
                        () => activePanel = null, () => Callbacks("Back"));
                    CarSelection.Instance.activeCar.transform.DOLocalMoveZ(0f, 1f);
                    Utils.TransitionBetweenCameras(virtualCameras[0],virtualCameras);
                    break;
                
                case "Right":
                    CarSelection.Instance.IncreaseActiveCarIndex();
                    break;
                
                case "Left":
                    CarSelection.Instance.DecreaseActiveCarIndex();
                    break;
                
                case "Skill A":
                    SkillSelection.Instance.SetActiveSkill("A");
                    break;
                
                case "Skill B":
                    SkillSelection.Instance.SetActiveSkill("B");
                    break;
                
                case "Skill C":
                    SkillSelection.Instance.SetActiveSkill("C");
                    break;
                
                case "Play":
                    PlayActions();
                    break;
                
                case "Try Again":
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    break;
                
                case "Quit":
                    Application.Quit();
                    break;
                
                case "Pause":
                    if (Time.timeScale == 0)
                        Time.timeScale = 1;
                    else
                        Time.timeScale = 0;
                    break;
                    
            }
        }

        private void Callbacks(string buttonName)
        {
            switch (buttonName)
            {
                case "Change Color":
                    ChangeColorCallbackActions();
                    break;
                
                case "Select Car":
                    SelectCarCallbackActions();
                    break;
                
                case "Back":
                    BackCallbackActions();
                    break;
                
                case "Play":
                    PlayCallbackActions();
                    break;
            }
        }

        #region Callbacks

        private void ChangeColorCallbackActions()
        {
            Utils.TransitionBetweenUIElements(panelChangeColor, new Vector3(0f, -630f, 0f));
        }
        
        private void SelectCarCallbackActions()
        {
            Utils.TransitionBetweenUIElements(panelSelectCar, new Vector3(0f, -630f, 0f));
            Utils.TransitionBetweenUIElements(buttonLeft, new Vector3(-400f, 0f, 0f));
            Utils.TransitionBetweenUIElements(buttonRight, new Vector3(400f, 0f, 0f));
        }

        private void BackCallbackActions()
        {
            Utils.TransitionBetweenUIElements(panelSelectSkill, new Vector3(0f, 620f, 0f), .75f);
            Utils.TransitionBetweenUIElements(buttonChangeColor, new Vector3(-300f, -668f, 0f), .5f);
            Utils.TransitionBetweenUIElements(buttonSelectCar, new Vector3(300f, -668f, 0f), .5f);
            Utils.TransitionBetweenUIElements(inputNickname, new Vector3(0f, -680f, 0f), .75f);
            Utils.TransitionBetweenUIElements(buttonLeft, new Vector3(-630f, 0f, 0f));
            Utils.TransitionBetweenUIElements(buttonRight, new Vector3(630f, 0f, 0f));
            Utils.TransitionBetweenUIElements(buttonPlay, new Vector3(0f, -860f, 0f));
        }

        private void PlayCallbackActions()
        {
            panelGameStartTimer.gameObject.SetActive(true);
            
            this.UpdateAsObservable().Where(x => GameStartTimer.Instance.gameStartTime < 0).Subscribe(
                unit =>
                {
                    Utils.TransitionBetweenUIElements(panelGameStartTimer, new Vector3(0f, 740, 0f), .3f,
                        () =>
                        {
                            canvasForMenu.SetActive(false);
                            
                            GameManager.Instance.canStart = true;
                            
                            canvasForController.SetActive(true);
                            
                            canvasForSkillCharger.SetActive(true);
                            
                            panelSkillCharger.DOLocalMove(new Vector3(-170f, 0f, 0f), 1f);
                            panelInGameTimer.DOLocalMove(new Vector3(0f, 0f, 0f), .45f);

                            Utils.TransitionBetweenCameras(virtualCameras[2], virtualCameras);

                            GameManager.Instance.activeVirtualCamera.Follow = CarSelection.Instance.activeCar.transform;
                        });
                    
                    GameStartTimer.Instance.gameStartTime = 0;
                });
        }

        #endregion
        
        private void ChangeColor_SelectCarActions(string buttonName)
        {
            if (buttonName == "Change Color")
            {
                Utils.TransitionBetweenUIElements(buttonChangeColor, new Vector3(-700f, -668f, 0f), .5f, () => Callbacks("Change Color"));
                CarSelection.Instance.activeCar.transform.DOLocalMoveZ(1.15f, 1f);
                Utils.TransitionBetweenUIElements(buttonSelectCar, new Vector3(700f, -668f, 0f), .5f);
                
                activePanel = panelChangeColor;
            }
                
            else if (buttonName == "Select Car")
            {
                Utils.TransitionBetweenUIElements(buttonSelectCar, new Vector3(700f, -668f, 0f), .5f,
                    () => Callbacks("Select Car"));
                CarSelection.Instance.activeCar.transform.DOLocalMoveZ(1.15f, 1f);
                Utils.TransitionBetweenUIElements(buttonChangeColor, new Vector3(-700f, -668f, 0f), .5f);
                
                activePanel = panelSelectCar;
            }
            
            Utils.TransitionBetweenUIElements(buttonPlay, new Vector3(0f, -1020f, 0f), .75f);
            Utils.TransitionBetweenUIElements(panelSelectSkill, new Vector3(0f, 1800f, 0f), .75f);
            Utils.TransitionBetweenUIElements(inputNickname, new Vector3(0f, -1050f, 0f), .75f);
            Utils.TransitionBetweenCameras(virtualCameras[1], virtualCameras);
            
        }

        private void PlayActions()
        {
            Utils.TransitionBetweenUIElements(buttonPlay, new Vector3(0f, -1020f, 0f), .75f, () => Callbacks("Play"));
            Utils.TransitionBetweenUIElements(panelSelectSkill, new Vector3(0f, 1200f, 0f), .75f);
            Utils.TransitionBetweenUIElements(inputNickname, new Vector3(0f, -1050f, 0f), .75f);
            Utils.TransitionBetweenUIElements(buttonSelectCar, new Vector3(700f, -668f, 0f), .5f);
            Utils.TransitionBetweenUIElements(buttonChangeColor, new Vector3(-700f, -668f, 0f), .5f);
        }
    }
}