using System.Collections.Generic;
using Core.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : Singleton<UIManager>
{
    [HideInInspector]
    public GameObject hitPanel;//提示框
    [HideInInspector]
    public TextMeshProUGUI hitText;//提示文字

    [HideInInspector] 
    public ScrollRect scrollView;//卷轴窗

    //[HideInInspector] 
    public List<GameObject> slots;
    public List<GameObject> splitArray;

    [HideInInspector]
    public TMP_InputField geneName;

    private void Start()
    {
        hitPanel = GameObject.Find("提示文本块");
        hitText = hitPanel.GetComponentInChildren<TextMeshProUGUI>();
        scrollView = GameObject.Find("滚轴").GetComponent<ScrollRect>();
        geneName = GameObject.Find("InputField (TMP)").GetComponent<TMP_InputField>();
        Debug.Log(geneName.text);
        for (int i = 1; i <= 9; i++)
        {
            slots.Add( GameObject.Find($"Slot{i}"));
        }
        for (int i = 1; i < 9; i++)
        {
            var array = GameObject.Find($"SplitArray{i}");
            splitArray.Add(array);
            array.SetActive(false);
        }
        
        hitPanel.SetActive(false);
    }
}
