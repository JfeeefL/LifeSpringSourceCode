using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
public class HitPoint : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public string hitWords;
    public float waitTime;

    private Tween count;

    public Func<string> GetHitWords;

    private void Awake()
    {
        GetHitWords = () => hitWords;
    }

    /// <summary>
    /// 鼠标刚进去时发生什么
    /// </summary>
    private void OnMouseEnter()
    {
        if (IsPointerOverGameObject(Input.mousePosition))
            return;
        float timeCount = waitTime;
        count=DOTween.To(() => timeCount, a => timeCount = a, waitTime, waitTime).OnComplete(new TweenCallback(delegate
            {
                ShowHit(true);
            }));
    }
    /// <summary>
    /// 传入 Input.mousePosition 
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    public bool IsPointerOverGameObject(Vector2 screenPosition)
    {
        //实例化点击事件
        PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        //将点击位置的屏幕坐标赋值给点击事件
        eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        //向点击处发射射线
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 1;

    }
    /// <summary>
    /// 鼠标出来时发生什么
    /// </summary>
    private void OnMouseExit()
    {
        count.Kill();
        ExitHit();
    }
/// <summary>
///显示hit方法
/// </summary>
    public void ShowHit(bool isWorld)
    {
        UIManager.Instance.hitPanel.SetActive(true);
        Vector3 nowPosition;
        if(isWorld)
            nowPosition = Camera.main.WorldToScreenPoint(transform.position);
        else
            nowPosition = transform.position;
        Vector2 deltaPivot = Vector2.one/2 -new Vector2(Screen.width, Screen.height)+ (Vector2)Input.mousePosition;
        deltaPivot = new Vector2(deltaPivot.x / Screen.width, deltaPivot.y / Screen.height);
        
        Vector2 pivot = new Vector2(nowPosition.x / Screen.width, nowPosition.x / Screen.height)+deltaPivot;
        UIManager.Instance.hitPanel.transform.GetComponent<RectTransform>().pivot = pivot; 
        UIManager.Instance.hitPanel.transform.position = nowPosition;


        UIManager.Instance.hitText.text = GetHitWords();
    }
/// <summary>
/// 退出hit方法
/// </summary>
    public void ExitHit()
    {
        UIManager.Instance.hitPanel.SetActive(false);
    }

    /// <summary>
    /// 鼠标刚进去时发生什么
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnPointerEnter(PointerEventData eventData)
    {
        float timeCount = waitTime;
        count=DOTween.To(() => waitTime, a => waitTime = a, waitTime, waitTime).OnComplete(new TweenCallback(delegate
        {
            ShowHit(false);
        }));
        
    }
    /// <summary>
    /// 鼠标出来时发生什么
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnPointerExit(PointerEventData eventData)
    {
        count.Kill();
        ExitHit();
    }


    public void RefreshWord()
    {
        if (gameObject.activeSelf)
        {
            string answer = GetHitWords();
            UIManager.Instance.hitText.text = answer;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShowHit(false);
    }
}
