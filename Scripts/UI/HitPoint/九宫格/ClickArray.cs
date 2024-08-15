using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickArray : MonoBehaviour,IPointerDownHandler
{
    public Vector2Int direction;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        HightLightSelf();
        ScrollViewList.Instance.UpdateArray(direction);
    }
    
    /// <summary>
    /// 高亮自己
    /// </summary>
    public void HightLightSelf()
    {
        foreach (var array in UIManager.Instance.splitArray)
        {
            array.GetComponent<Image>().color = new Color(1, 1, 1,0.34f);
        }
        transform.GetComponent<Image>().color = new Color(1, 1, 1,1);
    }
}
