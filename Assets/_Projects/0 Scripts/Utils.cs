using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using JetBrains.Annotations;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static void TransitionBetweenUIElements(Transform transform, Vector3? targetPosition = null,
        float? duration = null, [CanBeNull] params TweenCallback[] callbacks)
    {
        targetPosition ??= new Vector3(0f, 0f, 0f);
        duration ??= 1f;

        transform.DOLocalMove(targetPosition.Value, duration.Value).OnComplete(() =>
        {
            foreach (var tweenCallback in callbacks)
            {
                tweenCallback.Invoke();
            }
        });
    }
    
    public static void TransitionBetweenCameras(CinemachineVirtualCamera virtualCamera, CinemachineVirtualCamera[] virtualCameras, [CanBeNull] params TweenCallback[] callbacks)
    {
        virtualCamera.Priority = 10;
        GameManager.Instance.activeVirtualCamera = virtualCamera;

        foreach (var cinemachineVirtualCamera in virtualCameras)
        {
            if (cinemachineVirtualCamera != virtualCamera && cinemachineVirtualCamera.Priority != 0)
                cinemachineVirtualCamera.Priority = 0;
        }
    }
}
