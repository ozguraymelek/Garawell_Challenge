using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Area : Singleton<Area>
{
    [Header("Components")] 
    [SerializeField] private List<Transform> part0SplitAreas;
    [SerializeField] private List<Transform> part1SplitAreas;
    [SerializeField] private List<Transform> part2SplitAreas;
    [SerializeField] private List<Transform> part3SplitAreas;
    [SerializeField] internal List<Transform> part4SplitAreas;
    
    [Header("Components")] 
    [SerializeField] private List<Renderer> part0SplitAreaMaterials;
    [SerializeField] private List<Renderer> part1SplitAreaMaterials;
    [SerializeField] private List<Renderer> part2SplitAreaMaterials;
    [SerializeField] private List<Renderer> part3SplitAreaMaterials;
    [SerializeField] private List<Renderer> part4SplitAreaMaterials;

    [Header("Settings")] 
    [SerializeField] internal bool isFalling = false;

    [SerializeField] internal int currentWave = 0;

    public void Fall()
    {
        FallActs(currentWave);
    }

    private void FallActs(int i)
    {
        var sequence = DOTween.Sequence();
        switch (i)
        {
            case 0:
                for (int t = 0; t < part0SplitAreas.Count; t++)
                {
                    part0SplitAreaMaterials[t].sharedMaterials[0].color = Color.yellow;
                    sequence.Append(part0SplitAreas[t].transform.DOLocalMoveY(-5f, 1.5f));
                    sequence.AppendInterval(.25f);
                }
                break;
            case 1:
                for (int t = 0; t < part1SplitAreas.Count; t++)
                {
                    part1SplitAreaMaterials[t].sharedMaterials[0].color = Color.yellow;
                    sequence.Append(part1SplitAreas[t].transform.DOLocalMoveY(-5f, 1.5f));
                    sequence.AppendInterval(.25f);
                }
                break;
            case 2:
                for (int t = 0; t < part2SplitAreas.Count; t++)
                {
                    part2SplitAreaMaterials[t].sharedMaterials[0].color = Color.yellow;
                    sequence.Append(part2SplitAreas[t].transform.DOLocalMoveY(-5f, 1.5f));
                    sequence.AppendInterval(.25f);
                }
                break;
            case 3:
                for (int t = 0; t < part3SplitAreas.Count; t++)
                {
                    part3SplitAreaMaterials[t].sharedMaterials[0].color = Color.yellow;
                    sequence.Append(part3SplitAreas[t].transform.DOLocalMoveY(-5f, 1.5f));
                    sequence.AppendInterval(.25f);
                }
                break;
            case 4:
                for (int t = 0; t < part4SplitAreas.Count; t++)
                {
                    part4SplitAreaMaterials[t].sharedMaterials[0].color = Color.yellow;
                    sequence.Append(part4SplitAreas[t].transform.DOLocalMoveY(-5f, 1.5f));
                    sequence.AppendInterval(.25f);
                }
                break;
        }
        
        
        sequence.AppendCallback(() => isFalling = false);
        
    }
}
