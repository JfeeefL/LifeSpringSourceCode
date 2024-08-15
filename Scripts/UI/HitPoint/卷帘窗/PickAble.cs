
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class PickAble : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler,IPointerDownHandler
{
    private int originID;
    private void Start()
    {
        ScrollViewList.Instance.UpdateOtherData();
        
    }


    /// <summary>
    /// 点击函数
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        ScrollViewList.Instance.SetNowChoose(transform.GetSiblingIndex());
        ScrollViewList.Instance.UpdateOtherData();
    }
    /// <summary>
    /// 开始拖动
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        originID = transform.GetSiblingIndex();
        Debug.Log(originID);
        //ScrollViewList.Instance.isRefreshing = true;
        ScrollViewList.Instance.RemoveList(originID);
        transform.SetParent(transform.parent.parent.parent.parent);
        GetComponent<Image>().raycastTarget = false;
    }
    /// <summary>
    /// 持续拖动
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        UIManager.Instance.scrollView.horizontalNormalizedPosition = 0;
        if (Input.mousePosition.y >= UIManager.Instance.scrollView.gameObject.GetComponent<RectTransform>().position.y)
        {
            if (UIManager.Instance.scrollView.verticalNormalizedPosition <=1)
                UIManager.Instance.scrollView.verticalNormalizedPosition += 0.01f;
            else
                UIManager.Instance.scrollView.verticalNormalizedPosition = 1;
        }

        if (Input.mousePosition.y < UIManager.Instance.scrollView.gameObject.GetComponent<RectTransform>().position.y)
        {
            if (UIManager.Instance.scrollView.verticalNormalizedPosition >=0)
                UIManager.Instance.scrollView.verticalNormalizedPosition -= 0.01f;
            else
                UIManager.Instance.scrollView.verticalNormalizedPosition = 0;
        }
    }


/// <summary>
    /// 结束拖动
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        if (go == null)
        {
            ScrollViewList.Instance.AddAtList(originID);
            Destroy(this.gameObject);
            return;
        }
        if (go.CompareTag("ScrollerContent"))
        {
            ScrollViewList.Instance.AddAtList(go.transform.GetSiblingIndex());
            Destroy(this.gameObject);
        }
        else
        {
            //把拖拽的物体放回到原来的位置
            ScrollViewList.Instance.AddAtList(originID);
            Destroy(this.gameObject);
        }

    }


}
