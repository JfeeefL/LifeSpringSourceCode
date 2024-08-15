using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Core;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using TMPro;

public class ScrollViewList : Singleton<ScrollViewList>
{
    public GameObject contentPrefab;
    [SerializeField]
    public List<ContentInfos> contentList;

    private ContentInfos nowPickContentInfos;
    private int pickID;
    public bool isRefreshing;
    
    
    /// <summary>
    /// 存储文件地址
    /// </summary>
    private string jsonFolder;
    /// <summary>
    /// 文件数量
    /// </summary>
    private int fileCount=1;
    protected override void Awake()
    {
        base.Awake();
        jsonFolder = Application.persistentDataPath + "/SAVE DATA/";
    }
    
 
    /// <summary>
    /// 当前基因保存
    /// </summary>
    [Button]
    public void Save()
    {
        var resultPath = jsonFolder + "data" + ".json";
        var jsonData = JsonConvert.SerializeObject(contentList, Formatting.Indented);

        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        File.WriteAllText(resultPath, jsonData);
    }
    /// <summary>
    /// 读取储存数据
    /// </summary>
    [Button]
    private void Load()
    {
        if (Directory.Exists(jsonFolder))
        {
            var resultPath = jsonFolder + "data" + ".json";
                if (File.Exists(resultPath))
                {
                    var stringData = File.ReadAllText(resultPath);
                    var jsonData = JsonConvert.DeserializeObject<List<ContentInfos>>(stringData);
                    contentList = jsonData;
                    if (contentList != null)
                    {
                        RefeshContentViewList();
                    }
                    else
                    {
                        AddNewItem();
                    }
                }
        }
        
    }
/// <summary>
/// 退出时保存
/// </summary>
    private void OnApplicationQuit()
    {
        Save();
    }

    private void Start()
    {
        contentPrefab = Resources.Load<GameObject>("Prefab/名字预制体");
        Load();
        RefeshContentViewList();
        UpdateOtherData();

    }
/// <summary>
/// 赋值当前的指针值
/// </summary>
/// <param name="direction"></param>
    public void UpdateArray(Vector2Int direction)
    {
        nowPickContentInfos.thisGene.Direction = direction;
    }
/// <summary>
/// 点击content后更新其他UI信息
/// </summary>
    public void UpdateOtherData()
    {
       if(nowPickContentInfos==null)
           return;
    
       UIManager.Instance.geneName.text=nowPickContentInfos.data;
        
       UIManager.Instance.slots[0].GetComponentInChildren<ClickChange>().blockType=
           nowPickContentInfos.thisGene.TypeCondition[0, 2];
        
       UIManager.Instance.slots[1].GetComponentInChildren<ClickChange>().blockType=
           nowPickContentInfos.thisGene.TypeCondition[1, 2];
        
       UIManager.Instance.slots[2].GetComponentInChildren<ClickChange>().blockType=
           nowPickContentInfos.thisGene.TypeCondition[2, 2];
        
       UIManager.Instance.slots[3].GetComponentInChildren<ClickChange>().blockType=
           nowPickContentInfos.thisGene.TypeCondition[0, 1];

       UIManager.Instance.slots[4].GetComponentInChildren<ClickChange>().taskType =
           nowPickContentInfos.thisGene.Task;

       UIManager.Instance.slots[5].GetComponentInChildren<ClickChange>().blockType =
           nowPickContentInfos.thisGene.TypeCondition[2, 1];
        
       UIManager.Instance.slots[6].GetComponentInChildren<ClickChange>().blockType=
           nowPickContentInfos.thisGene.TypeCondition[0, 0]   ;
        
       UIManager.Instance.slots[7].GetComponentInChildren<ClickChange>().blockType=
           nowPickContentInfos.thisGene.TypeCondition[1, 0]   ;
        
       UIManager.Instance.slots[8].GetComponentInChildren<ClickChange>().blockType=
           nowPickContentInfos.thisGene.TypeCondition[2, 0]   ;
        
       for (int i = 0; i < UIManager.Instance.slots.Count; i++)
       {
           UIManager.Instance.slots[i].GetComponentInChildren<ClickChange>().RefeshImage(i==4);
       }

       foreach (var array in UIManager.Instance.splitArray)
       {
           array.GetComponent<Image>().color = new Color(1, 1, 1, 0.34f);
       }

       var nowdir = nowPickContentInfos.thisGene.Direction;
       switch (nowdir.x,nowdir.y)
       {
           case (-1,1):
               UIManager.Instance.splitArray[0].GetComponent<Image>().color = new Color(1, 1, 1, 1);
               break;
           case (0,1):
               UIManager.Instance.splitArray[1].GetComponent<Image>().color = new Color(1, 1, 1, 1);
               break;
           case (1,1):
               UIManager.Instance.splitArray[2].GetComponent<Image>().color = new Color(1, 1, 1, 1);
               break;
           case (-1,0):
               UIManager.Instance.splitArray[3].GetComponent<Image>().color = new Color(1, 1, 1, 1);
               break;
           case (1,0):
               UIManager.Instance.splitArray[4].GetComponent<Image>().color = new Color(1, 1, 1, 1);
               break;
           case (-1,-1):
               UIManager.Instance.splitArray[5].GetComponent<Image>().color = new Color(1, 1, 1, 1);
               break;
           case (0,-1):
               UIManager.Instance.splitArray[6].GetComponent<Image>().color = new Color(1, 1, 1, 1);
               break;
           case (1,-1):
               UIManager.Instance.splitArray[7].GetComponent<Image>().color = new Color(1, 1, 1, 1);
               break;
           default:
               break;
       }
        
    }


/// <summary>
/// 更新当前的基因信息
/// </summary>
    public void UpdateContentGeneInfo()
    {
        nowPickContentInfos.thisGene.TypeCondition[0, 2] =
            UIManager.Instance.slots[0].GetComponentInChildren<ClickChange>().blockType;
        
        nowPickContentInfos.thisGene.TypeCondition[1, 2] =
            UIManager.Instance.slots[1].GetComponentInChildren<ClickChange>().blockType;
        
        nowPickContentInfos.thisGene.TypeCondition[2, 2] =
            UIManager.Instance.slots[2].GetComponentInChildren<ClickChange>().blockType;
        
        nowPickContentInfos.thisGene.TypeCondition[0, 1] =
            UIManager.Instance.slots[3].GetComponentInChildren<ClickChange>().blockType;
        
        nowPickContentInfos.thisGene.Task =
            UIManager.Instance.slots[4].GetComponentInChildren<ClickChange>().taskType;
        
        nowPickContentInfos.thisGene.TypeCondition[2,1] =
            UIManager.Instance.slots[5].GetComponentInChildren<ClickChange>().blockType;
        nowPickContentInfos.thisGene.TypeCondition[0, 0] =
            UIManager.Instance.slots[6].GetComponentInChildren<ClickChange>().blockType;
        nowPickContentInfos.thisGene.TypeCondition[1, 0] =
            UIManager.Instance.slots[7].GetComponentInChildren<ClickChange>().blockType;
        nowPickContentInfos.thisGene.TypeCondition[2, 0] =
            UIManager.Instance.slots[8].GetComponentInChildren<ClickChange>().blockType;
        
       
    }
/// <summary>
/// 更新名字
/// </summary>
    public void UpdateNameInfo()
    {
        nowPickContentInfos.data = UIManager.Instance.geneName.text;
        RefeshContentViewList();
    }
/// <summary>
/// 添加到列表
/// </summary>
/// <param name="i"></param>
    public void AddAtList(int i)
    {
        contentList.Insert(i,nowPickContentInfos);
        RefeshContentViewList();
    }
//添加新的
public void AddNewItem()
{
    var newInfo = new ContentInfos();
    newInfo.data = "新基因";
    contentList.Add(newInfo);
   
    RefeshContentViewList();
}
//删除选中
public void DeletNowChooseItem()
{
    if (contentList.Contains(nowPickContentInfos))
    {
        contentList.Remove(nowPickContentInfos);
        if (contentList.Count != 0)
        {
            nowPickContentInfos = contentList[contentList.Count - 1];
            Debug.Log("我删除了"+nowPickContentInfos.data);
        }
        else
            nowPickContentInfos = null;
    }

        UpdateOtherData();
        RefeshContentViewList();
}

/// <summary>
/// 同步cellmanager的基因信息
/// </summary>
    public void SyncGene()
    {
        if (contentList == null)
            return;
        RefeshContentViewList();
        CellManager.Instance.GlobalGenes.Clear();
        foreach (var gene in contentList)
        {
            if(gene.isActive)
                CellManager.Instance.GlobalGenes.Add(gene.thisGene);
        }
        Save();
        
    }

    /// <summary>
    /// 暂时去除某个元素
    /// </summary>
    /// <param name="i"></param>
    public void RemoveList(int i)
    {
        nowPickContentInfos = contentList[i];
        pickID = i;
        contentList.RemoveAt(i);
        //RefeshContentViewList();
    }

    public void SetNowChoose(int i)
    {
        nowPickContentInfos = contentList[i];
        for (int j = 0; j < transform.childCount; j++)
        {
            transform.GetChild(j).GetChild(3).gameObject.SetActive(false);
        }
        transform.GetChild(i).GetChild(3).gameObject.SetActive(true);
        pickID = i;
    }
    
    /// <summary>
    /// 刷新列表中物体
    /// </summary>
    public void RefeshContentViewList()
    {
        foreach (var aChild in GetComponentsInChildren<ScrollContent>())
        {
            Destroy(aChild.gameObject);
        }

        if (contentList.Count == 0)
        {
            return;
        }
        if (nowPickContentInfos == null)
        {
            nowPickContentInfos = contentList[0];
            pickID = 0;
        }
        if(nowPickContentInfos.thisGene.Direction==Vector2Int.zero)
            nowPickContentInfos.thisGene.Direction=Vector2Int.down;
        
        isRefreshing = true;
     
      
        int i = 0;
        foreach (var t in contentList)
        {
            i++;
            var content = Instantiate(contentPrefab, transform);
            content.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            content.GetComponentInChildren<SelfToggle>().isUse = t.isActive;
            content.GetComponentInChildren<SelfToggle>().Refrsh();
           
            if (nowPickContentInfos == t)
                content.transform.GetChild(3).gameObject.SetActive(true);
            

            var slot = content.GetComponent<ScrollContent>();
               slot.thisContentInfos =t;
               slot.LoadText();
        }
        
        
        isRefreshing = false;
        Save();
    }
    
}
