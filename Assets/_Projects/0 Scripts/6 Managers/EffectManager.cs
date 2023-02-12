using System.Collections;
using System.Collections.Generic;
using EZ_Pooling;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] private Transform hitEffect1, hitEffect2;

    public void HitEffect1(Vector3 _pos, Quaternion _rot)
    {
        Transform _effTr = EZ_PoolManager.Spawn(hitEffect1, _pos, _rot);

        StartCoroutine(DespawnEffect(_effTr, 0.5f));
    }
    
    public void HitEffect2(Vector3 _pos, Quaternion _rot)
    {
        Transform _effTr = EZ_PoolManager.Spawn(hitEffect2, _pos, _rot);

        StartCoroutine(DespawnEffect(_effTr, 0.5f));
    }
    
    private IEnumerator DespawnEffect(Transform _effTr, float _despawnTime)
    {
        yield return new WaitForSeconds(_despawnTime);

        EZ_PoolManager.Despawn(_effTr);
    }
}
