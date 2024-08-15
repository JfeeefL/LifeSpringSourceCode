using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Core;
using Gameplay;

public class PlantController : Singleton<PlantController>
{
    [Header("可以设置的参数")]
    public int Stage1CellCount = 10;
    public int Stage2CellCount = 15;
    public int Stage3CellCount = 20;
    public int Stage4CellCount = 25;
    public int Stage5CellCount = 30;
    public int CurrentPlantStage = 0;

    
    private int CurrentCellCount;
    [Header("对外公开但是不要手动改")]
    public bool IsSnow = false;
    public Animator SelfAnimator;

    private void Start()
    {
        SelfAnimator = this.GetComponent<Animator>();
        CurrentCellCount = CellManager.Instance.CellCount;
    }

    private void FixedUpdate()
    {
        CurrentCellCount = CellManager.Instance.CellCount;
        if (CurrentCellCount >=Stage1CellCount&&CurrentCellCount<Stage2CellCount && SelfAnimator.GetInteger("Stage") != 1&& !IsSnow)
        {
            SelfAnimator.SetInteger("Stage", 1);
            CurrentPlantStage = 1;
        }
        else if(CurrentCellCount >= Stage2CellCount && CurrentCellCount < Stage3CellCount && SelfAnimator.GetInteger("Stage") != 2 && !IsSnow)
        {
            SelfAnimator.SetInteger("Stage", 2);
            CurrentPlantStage = 2;
        }
        else if (CurrentCellCount >= Stage3CellCount && CurrentCellCount < Stage4CellCount && SelfAnimator.GetInteger("Stage") != 3 && !IsSnow)
        {
            SelfAnimator.SetInteger("Stage", 3);
            CurrentPlantStage = 3;
        }
        else if (CurrentCellCount >= Stage4CellCount && CurrentCellCount < Stage5CellCount && SelfAnimator.GetInteger("Stage") != 4 && !IsSnow)
        {
            SelfAnimator.SetInteger("Stage", 4);
            CurrentPlantStage = 4;
        }
        else if (CurrentCellCount >= Stage5CellCount && SelfAnimator.GetInteger("Stage")!=5 && !IsSnow)
        {
            SelfAnimator.SetInteger("Stage", 5);
            CurrentPlantStage = 5;
            EventHandler.CallOnPlantRipe(1);
        }

        switch (PlantController.Instance.CurrentPlantStage)
        {
            case 0:
                EventHandler.CallOnChangeOrganism(CellManager.Instance.CellCount, PlantController.Instance.Stage1CellCount);
                break;
            case 1:
                EventHandler.CallOnChangeOrganism(CellManager.Instance.CellCount, PlantController.Instance.Stage2CellCount);
                break;
            case 2:
                EventHandler.CallOnChangeOrganism(CellManager.Instance.CellCount, PlantController.Instance.Stage3CellCount);
                break;
            case 3:
                EventHandler.CallOnChangeOrganism(CellManager.Instance.CellCount, PlantController.Instance.Stage4CellCount);
                break;
            case 4:
                EventHandler.CallOnChangeOrganism(CellManager.Instance.CellCount, PlantController.Instance.Stage5CellCount);
                break;
            case 5:
                EventHandler.CallOnChangeOrganism(1, 1);
                break;
        }
    }
}
