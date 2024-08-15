using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterUIController : MonoBehaviour
{

    private void OnEnable()
    {
        EventHandler.OnChangeWater += ChangeWaterUI;
    }

    public void ChangeWaterUI(float TotalWater,float DeltaWater)
    {
        Text Num = this.transform.Find("Num").GetComponent<Text>();
        if (TotalWater < 0)
        {
            EventHandler.CallOnPlantDied(1);
            PlantController.Instance.SelfAnimator.SetBool("Die",true);
        }
        if (DeltaWater >= 0)
        {
            Num.text = (int)TotalWater + " +" + (int)DeltaWater;
        }
        else
        {
            Num.text = (int)TotalWater + " " + (int)DeltaWater;
        }
        
    }

    private void OnDisable()
    {
        EventHandler.OnChangeWater -= ChangeWaterUI;
    }
}
