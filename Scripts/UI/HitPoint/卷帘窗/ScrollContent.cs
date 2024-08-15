using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class ScrollContent : MonoBehaviour
{
    public ContentInfos thisContentInfos;
    /// <summary>
    /// 加载文字
    /// </summary>
    public void LoadText()
    {
        GetComponentInChildren<Text>().text=thisContentInfos.data;
    }

    public void LoadGene()
    {
        
    }
}
