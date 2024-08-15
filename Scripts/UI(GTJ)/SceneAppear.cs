using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneAppear : MonoBehaviour
{
    public float AppearTime = 1;
    void Start()
    {
        this.GetComponent<Image>().DOColor(new Color32(255, 255, 255, 255), AppearTime);
        for(int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).transform.GetComponent<Image>())
            {
                this.transform.GetChild(i).transform.GetComponent<Image>().DOColor(new Color32(255, 255, 255, 255), AppearTime);
            }
        }
    }
}
