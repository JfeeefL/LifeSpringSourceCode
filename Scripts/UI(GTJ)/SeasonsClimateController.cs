using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gameplay;

public class SeasonsClimateController : MonoBehaviour
{
    [Header("�ɵ�������")]
    /// <summary>
    /// һ�����ڳ�������
    /// </summary>
    public int SeasonDayCount = 5;
    [Range(0,1)]

    /// <summary>
    /// �����������
    /// </summary>
    public float RainProbability_Spring = 0.5f;
    [Range(0, 1)]

    /// <summary>
    /// �ļ��������
    /// </summary>
    public float RainProbability_Summer = 0.5f;
    [Range(0, 1)]
    /// <summary>
    /// �ļ��ɺ�����
    /// </summary>
    public float DroughtProbability_Summer = 0.5f;

    [Range(0, 1)]
    /// <summary>
    /// �＾�������
    /// </summary>
    public float RainProbability_Autumn = 0.5f;

    [Range(0, 1)]
    /// <summary>
    /// �����������
    /// </summary>
    public float RainProbability_Winter = 0.5f;
    [Range(0, 1)]
    /// <summary>
    /// ������ѩ����
    /// </summary>
    public float SnowProbability_Winter = 0.5f;



    /// <summary>
    /// ��ǰ�������
    /// </summary>
    public WeatherType CurrentWeather = WeatherType.Sunshine;

    [Header("��Ҫ��ӵ�Ԫ��")]
    public GameObject YearsText;

    //�������ڲ�ͳ������ʾ������
    private int CurrentYear = 1;
    private int CurrentDay = 0;
    private SeasonsType CurrentSeason = SeasonsType.Winter;

    private void OnEnable()
    {
        //�������ı�ĺ���ע�ᵽ�����ı��ί����
        EventHandler.OnChangeDays += ChangeDay;
    }

    private void OnDisable()
    {
        //�������ı�ĺ����������ı��ί����ע��
        EventHandler.OnChangeDays -= ChangeDay;
    }

    private void Start()
    {
        EventHandler.CallOnChangeDays(1);
    }
    


    /// <summary>
    /// �����ƽ������ĺ���
    /// </summary>
    /// <param name="DayCount">�ƽ�������</param>
    public void ChangeDay(int DayCount)
    {
        CurrentDay += DayCount;
        SeasonsType TempSeason=SeasonsType.Spring;
        if (CurrentDay > 0 && CurrentDay <= SeasonDayCount)
        {
            TempSeason = SeasonsType.Spring;
        }
        else if(CurrentDay > SeasonDayCount && CurrentDay <= 2 * SeasonDayCount)
        {
            TempSeason = SeasonsType.Summer;
        }
        else if (CurrentDay > 2 * SeasonDayCount && CurrentDay <= 3 * SeasonDayCount)
        {
            TempSeason = SeasonsType.Autumn;
        }
        else if (CurrentDay >  3* SeasonDayCount && CurrentDay <= 4 * SeasonDayCount)
        {
            TempSeason = SeasonsType.Winter;
        }
        else
        {
            CurrentYear += 1;
            CurrentDay = 1;
        }
        if (TempSeason != CurrentSeason)
        {
            CurrentSeason = TempSeason;
            EventHandler.CallOnChangeSeason(CurrentSeason);
        }
        WeatherRandomGenerator();
        ChangeUI(DayCount);
        EventHandler.CallOnChangeWater(CellManager.Instance.totalWaterGained,CellManager.Instance.totalWaterAbsorbed+ CellManager.Instance.totalWaterConsume);
    }
    /// <summary>
    /// �����ı�CurrentWeather�ĺ���
    /// </summary>
    private void WeatherRandomGenerator()
    {
        if (RainProbability_Summer + SnowProbability_Winter > 1 || RainProbability_Winter + SnowProbability_Winter > 1)
            {
                Debug.LogError("�����趨����������ʺʹ���1");
                return;
            }
        float RandValue = Random.Range(0,1001);
        switch (CurrentSeason)
            {
                case SeasonsType.Spring:
                    if (RandValue / 1000 <= RainProbability_Spring)
                    {
                        CurrentWeather = WeatherType.Rain;
                    }
                    else
                    {
                        CurrentWeather = WeatherType.Sunshine;
                    }
                    break;
                case SeasonsType.Summer:
                    if (RandValue / 1000 <= RainProbability_Summer)
                    {
                        CurrentWeather = WeatherType.Rain;
                    }
                    else if(RandValue / 1000 > RainProbability_Summer&& RandValue / 1000 <= RainProbability_Summer+DroughtProbability_Summer)
                    {
                        CurrentWeather = WeatherType.Drought;
                    }
                    else
                    {
                        CurrentWeather = WeatherType.Sunshine;
                    }
                    break;
                case SeasonsType.Autumn:
                    if (RandValue / 1000 <= RainProbability_Autumn)
                    {
                        CurrentWeather = WeatherType.Rain;
                    }
                    else
                    {
                        CurrentWeather = WeatherType.Sunshine;
                    }
                    break;
                case SeasonsType.Winter:
                    if (RandValue / 1000 <= RainProbability_Winter)
                    {
                        CurrentWeather = WeatherType.Rain;
                    }
                    else if (RandValue / 1000 > RainProbability_Winter && RandValue / 1000 <= RainProbability_Winter + SnowProbability_Winter)
                    {
                        CurrentWeather = WeatherType.Snow;
                    }
                    else
                    {
                        CurrentWeather = WeatherType.Sunshine;
                    }
                    break;
            }
        EventHandler.CallOnChangeWeather(CurrentWeather);
    }
    /// <summary>
    /// �����ı����¼��ƽ���ɵ�UI�ı�ĺ���
    /// </summary>
    private void ChangeUI(int DayGoing)
    {
        YearsText.GetComponent<Text>().text = CurrentYear.ToString();
        GameObject SeasonPointer = this.transform.parent.Find("Pointer").gameObject;
        SeasonPointer.transform.Rotate(new Vector3(0,0,(float)DayGoing*360f/(4f*(float)SeasonDayCount)));
    }

}
