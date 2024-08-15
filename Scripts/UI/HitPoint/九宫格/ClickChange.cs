using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickChange : MonoBehaviour,IPointerDownHandler
{
    public bool isEightSlots;
    public BlockType blockType;
    public TaskType taskType;
    public bool hasType;
    private void Start()
    {
        transform.GetComponentInChildren<Image>().sprite = null;
    }

    public void RefeshImage(bool isCenter)
    {
        if (isCenter)
        {
            transform.GetComponentInChildren<Image>().sprite = taskType switch
            {
                TaskType.Idle => Resources.LoadAll<Sprite>("NineSlotIcon/Command+")[1],
                TaskType.ToCap => Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[6],
                TaskType.ToStore => Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[4],
                TaskType.Reproduce => Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[5]
            };
            var hitPoint = GetComponent<HitPoint>();
            hitPoint.hitWords = taskType switch
            {
                TaskType.Idle => "<color=#6d4c41><b>执行：</b></color>什么都不做",
                TaskType.ToCap => "<color=#6d4c41><b>执行：</b></color>转变为保卫细胞",
                TaskType.ToStore => "<color=#6d4c41><b>执行：</b></color>转变为储水细胞",
                TaskType.Reproduce => "<color=#6d4c41><b>执行：</b></color>向箭头方向分裂",
            };
            
            
            if(taskType==TaskType.Reproduce)
                foreach (var array in UIManager.Instance.splitArray)
                {
                    array.SetActive(true);
                }
            else
                foreach (var array in UIManager.Instance.splitArray)
                {
                    array.SetActive(false);
                }
        }
        else
        {
            transform.GetComponentInChildren<Image>().sprite = blockType switch
            {
                BlockType.Any => Resources.LoadAll<Sprite>("NineSlotIcon/Command+")[0],
                (BlockType)1 => //Stem
                    Resources.LoadAll<Sprite>("NineSlotIcon/Command+")[2],
                (BlockType)2 => //Store,
                    Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[0],
                (BlockType)3 => //Cap,
                    Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[1],
                (BlockType)4 => //Earth,
                    Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[2],
                (BlockType)5 => //Rock
                    Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[3]
            };
            var hitPoint = GetComponent<HitPoint>();
            hitPoint.hitWords = blockType switch
            {
                BlockType.Any => "<color=#6d4c41><b>条件：</b></color>\n任意细胞或地形",
                BlockType.Stem => "<color=#6d4c41><b>匹配条件：</b></color>\n这里是一个干细胞\n高耗水但可分裂的细胞",
                BlockType.Store =>  "<color=#6d4c41><b>匹配条件：</b></color>\n这里是一个储水细胞\n低耗水且可储水的细胞",
                BlockType.Cap => "<color=#6d4c41><b>匹配条件：</b></color>\n这里是一个保卫细胞\n保护细胞免受脱水",
                BlockType.Earth => "<color=#6d4c41><b>匹配条件：</b></color>\n这里是一个土壤\n含水的土壤",
                BlockType.Rock => "<color=#6d4c41><b>匹配条件：</b></color>\n这里是一个石头\n不含水的石头，无法推动",
            };
            Debug.Log("我改词了");
           // hitPoint.RefreshWord();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isEightSlots)
            EightChange();
        else
            CoreChange();
        ScrollViewList.Instance.UpdateContentGeneInfo();
        AudioManager.Instance.PlayAudioOnce("图标切换");
    }
    void CoreChange()
    {
        taskType++;
        Debug.Log(taskType);
        if (taskType > TaskType.Idle)
            taskType = 0;
        Debug.Log(taskType);
        var hitPoint = GetComponent<HitPoint>();
        switch (taskType)
        {
            case TaskType.Reproduce:
                transform.GetComponentInChildren<Image>().sprite = Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[5];
                hitPoint.hitWords ="向箭头方向分裂";
                foreach (var array in UIManager.Instance.splitArray)
                {
                    array.SetActive(true);
                }
                break;
            case TaskType.ToCap:
                transform.GetComponentInChildren<Image>().sprite = Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[6];
                hitPoint.hitWords ="转变为保卫细胞";
                foreach (var array in UIManager.Instance.splitArray)
                {
                    array.SetActive(false);
                }
                break;
            case TaskType.ToStore:
                transform.GetComponentInChildren<Image>().sprite = Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[4];
                hitPoint.hitWords ="转变为储水细胞";
                foreach (var array in UIManager.Instance.splitArray)
                {
                    array.SetActive(false);
                }
                break;
            case TaskType.Idle:
                transform.GetComponentInChildren<Image>().sprite = Resources.LoadAll<Sprite>("NineSlotIcon/Command+")[1];
                hitPoint.hitWords ="什么都不做";
                foreach (var array in UIManager.Instance.splitArray)
                {
                    array.SetActive(false);   
                }
                break;
        }
        hitPoint.RefreshWord();
    }

    void EightChange()
    {
        blockType++;
        if (blockType > BlockType.Rock)
            blockType = 0;
        var hitPoint = GetComponent<HitPoint>();
        switch (blockType)
        {
            case BlockType.Any:
                transform.GetComponentInChildren<Image>().sprite= Resources.LoadAll<Sprite>("NineSlotIcon/Command+")[0];
                hitPoint.hitWords ="<color=#6d4c41><b>条件：</b></color>\n任意细胞或地形";
                Debug.Log("我改词了");
                break;
            case (BlockType)1 ://Stem
                transform.GetComponentInChildren<Image>().sprite= Resources.LoadAll<Sprite>("NineSlotIcon/Command+")[2];
                hitPoint.hitWords = "<color=#6d4c41><b>匹配条件：</b></color>\n这里是一个干细胞\n高耗水但可分裂的细胞";
                break;
            case (BlockType)2 ://Store,
                transform.GetComponentInChildren<Image>().sprite= Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[0];
                hitPoint.hitWords =  "<color=#6d4c41><b>匹配条件：</b></color>\n这里是一个储水细胞\n低耗水且可储水的细胞";
                break;
            case (BlockType)3 ://Cap,
                transform.GetComponentInChildren<Image>().sprite= Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[1];
                hitPoint.hitWords = "<color=#6d4c41><b>匹配条件：</b></color>\n这里是一个保卫细胞\n保护细胞免受脱水";
                break;
            case (BlockType)4 ://Earth,
                transform.GetComponentInChildren<Image>().sprite= Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[2];
                hitPoint.hitWords = "<color=#6d4c41><b>匹配条件：</b></color>\n这里是一个土壤\n含水的土壤";
                break;
            case (BlockType)5 ://Rock
                transform.GetComponentInChildren<Image>().sprite= Resources.LoadAll<Sprite>("NineSlotIcon/CommandFix")[3];
                hitPoint.hitWords = "<color=#6d4c41><b>匹配条件：</b></color>\n这里是一个石头\n不含水的石头，无法推动";
                break;
        }
        hitPoint.RefreshWord();
    }
}
