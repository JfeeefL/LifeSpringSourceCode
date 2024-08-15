using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SeasonMask : MonoBehaviour
{
    [Header("可调整参数")]
    public Color32 SpringMaskColor=new Color32(255,255,255,50);
    public Color32 SummerMaskColor = new Color32(255, 255, 255, 50);
    public Color32 AutumnMaskColor = new Color32(255, 255, 255, 50);
    public Color32 WinterMaskColor = new Color32(255, 255, 255, 50);
    public float ChangeTime = 1;
    private void OnEnable()
    {
        EventHandler.OnChangeSeason += ChangeSeasonMask;
    }
    private void OnDisable()
    {
        EventHandler.OnChangeSeason -= ChangeSeasonMask;
    }
    public void ChangeSeasonMask(SeasonsType CurrentSeason)
    {
        switch (CurrentSeason)
        {
            case SeasonsType.Spring:
                this.GetComponent<Image>().DOColor(SpringMaskColor, ChangeTime);
                break;
            case SeasonsType.Summer:
                this.GetComponent<Image>().DOColor(SummerMaskColor, ChangeTime);
                break;
            case SeasonsType.Autumn:
                this.GetComponent<Image>().DOColor(AutumnMaskColor, ChangeTime);
                break;
            case SeasonsType.Winter:
                this.GetComponent<Image>().DOColor(WinterMaskColor, ChangeTime);
                break;
                
        }
    }
}
