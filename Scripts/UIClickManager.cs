using System.Collections;
using System.Collections.Generic;
using Core.Core;
using UnityEngine;

public class UIClickManager : MonoBehaviour
{
    [SerializeField]
    private List<PanelControl> list;

    public int endClickCount; 
    
    public int clickCount;
    private void Start()
    {
        clickCount = 1;
        foreach (var e in list)
        {
            if (e.clickCount == clickCount)
            {
                e.panel.SetActive(e.action);
            }
        }
    }

    public void ChangeCount(int val)
    {
        clickCount += val;
        AudioManager.Instance.PlayAudioOnce("按钮音效");
        if (clickCount < 1) clickCount = 1;
            foreach (var e in list)
            {
                if (e.clickCount == clickCount)
                {
                    e.panel.SetActive(e.action);
                }
            }

            if (clickCount == endClickCount)
            {
                GetComponent<StartController>().StartGame();
            }
    }
}

[System.Serializable]
public struct PanelControl
{
    public bool action;
    public GameObject panel;
    public int clickCount;
}