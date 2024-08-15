using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OrganismUIController : MonoBehaviour
{
    [Header("���趨����")]
    public float ChangeTime = 0.75f;

    public float LatestOrganismRatio=1;

    private void OnEnable()
    {
        EventHandler.OnChangeOrganism += ChangeOrganismUI;
    }

    private void FixedUpdate()
    {
        this.transform.GetChild(2).GetComponent<Image>().fillAmount = LatestOrganismRatio;
    }

    public void ChangeOrganismUI(float CurrentOrganism, float MaxOrganism)
    {
        if (CurrentOrganism < 0 || MaxOrganism < 0)
        {
            Debug.Log("�л������");
            CurrentOrganism = 0;
            MaxOrganism = 1;
        }
        if (CurrentOrganism > MaxOrganism)
        {
            Debug.Log("�ɳ�����һ�׶�");
            CurrentOrganism = 0;
            MaxOrganism = 1;
        }
        if (LatestOrganismRatio == CurrentOrganism / MaxOrganism) return;
        DOTween.To(() => LatestOrganismRatio, x => LatestOrganismRatio = x, CurrentOrganism / MaxOrganism, ChangeTime);
    }
    private void OnDisable()
    {
        EventHandler.OnChangeOrganism -= ChangeOrganismUI;
    }
}
