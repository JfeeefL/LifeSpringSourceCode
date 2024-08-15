using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class MaskPlay : MonoBehaviour
{
    [SerializeField]
    private List<MaskOrder> list;
    
    public int clickCount;
    
    private void Start()
    {
        clickCount = 1;
        foreach (var e in list)
        {
            if (e.clickCount == clickCount)
            {
                var tr = transform;
                tr.position = e.position;
                tr.localScale = e.scale;
            }
        }
    }

    public void ChangeCount(int val)
    {
        clickCount += val;
        if (clickCount < 1) clickCount = 1;
            foreach (var e in list)
            {
                if (e.clickCount == clickCount)
                {
                    transform.DOMove(e.position, 1);
                    transform.DOScale(e.scale, 1);
                }
            }
    }
}

[System.Serializable]
public struct MaskOrder
{
    public Vector3 position;
    public Vector3 scale;
    public int clickCount;
}
