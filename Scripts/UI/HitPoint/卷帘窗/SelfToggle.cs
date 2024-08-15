using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelfToggle : MonoBehaviour,IPointerDownHandler
{
    public bool isUse;

    
    public void Refrsh()
    {
        if (isUse)
        {
            GetComponent<Image>().sprite=Resources.LoadAll<Sprite>("Texture/UI")[19];
        }
        else
        {
            GetComponent<Image>().sprite=Resources.LoadAll<Sprite>("Texture/UI")[14];
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        isUse = !isUse;
        ScrollViewList.Instance.contentList[transform.parent.GetSiblingIndex()].isActive = isUse;
        Debug.Log(transform.parent.GetSiblingIndex());
        if (isUse)
        {
            GetComponent<Image>().sprite=Resources.LoadAll<Sprite>("Texture/UI")[19];
        }
        else
        {
            GetComponent<Image>().sprite=Resources.LoadAll<Sprite>("Texture/UI")[14];
        }
    }

}
