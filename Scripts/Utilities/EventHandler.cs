using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    /// <summary>
    /// 日子推进事件
    /// </summary>
    public static event Action<int> OnChangeDays;
    /// <summary>
    /// 日子推进事件的调用
    /// </summary>
    /// <param name="DayCount">日子推进的天数</param>
    public static void CallOnChangeDays(int DayCount)
    {
        OnChangeDays?.Invoke(DayCount);
    }

    /// <summary>
    /// 更改水含量UI事件
    /// </summary>
    public static event Action<float,float> OnChangeWater;
    /// <summary>
    /// 更改水含量UI事件的调用
    /// </summary>
    /// <param name="CurrentWater">当前含水量</param>
    /// <param name="MaxWater">最大含水量</param>
    public static void CallOnChangeWater(float TotalWater,float DeltaWater)
    {
        OnChangeWater?.Invoke(TotalWater,DeltaWater);
    }

    /// <summary>
    /// 更改有机物UI事件
    /// </summary>
    public static event Action<float, float> OnChangeOrganism;
    /// <summary>
    /// 更改有机物UI事件的调用
    /// </summary>
    /// <param name="CurrentOrganism">当前含水量</param>
    /// <param name="MaxOrganism">最大含水量</param>
    public static void CallOnChangeOrganism(float CurrentOrganism, float MaxOrganism)
    {
        OnChangeOrganism?.Invoke(CurrentOrganism, MaxOrganism);
    }

    /// <summary>
    /// 天气更改事件
    /// </summary>
    public static event Action<WeatherType> OnChangeWeather;
    /// <summary>
    /// 天气更改事件的调用
    /// </summary>
    /// <param name="NewWeather"></param>
    public static void CallOnChangeWeather(WeatherType NewWeather)
    {
        OnChangeWeather?.Invoke(NewWeather);
    }

    /// <summary>
    /// 晴天事件
    /// </summary>
    public static event Action<float> OnSunsine;
    /// <summary>
    /// 晴天事件的调用
    /// </summary>
    /// <param name="NewWeather"></param>
    public static void CallOnSunsine(float DltaValue)
    {
        OnSunsine?.Invoke(DltaValue);
    }
    /// <summary>
    /// 雨天事件
    /// </summary>
    public static event Action<float> OnRain;
    /// <summary>
    /// 雨天事件的调用
    /// </summary>
    /// <param name="NewWeather"></param>
    public static void CallOnRain(float DltaValue)
    {
        OnRain?.Invoke(DltaValue);
    }
    /// <summary>
    /// 干旱事件
    /// </summary>
    public static event Action<float> OnDrought;
    /// <summary>
    /// 干旱事件的调用
    /// </summary>
    /// <param name="NewWeather"></param>
    public static void CallOnDrought(float DltaValue)
    {
        OnDrought?.Invoke(DltaValue);
    }
    /// <summary>
    /// 下雪事件
    /// </summary>
    public static event Action<float> OnSnow;
    /// <summary>
    /// 下雪事件的调用
    /// </summary>
    /// <param name="NewWeather"></param>
    public static void CallOnSnow(float DeltaValue)
    {
        OnSnow?.Invoke(DeltaValue);
    }

    /// <summary>
    /// 季节更改事件
    /// </summary>
    public static event Action<SeasonsType> OnChangeSeason;
    /// <summary>
    /// 季节更改事件的调用
    /// </summary>
    /// <param name="NewWeather"></param>
    public static void CallOnChangeSeason(SeasonsType NewSeason)
    {
        OnChangeSeason?.Invoke(NewSeason);
    }
    /// <summary>
    /// 植物死亡事件
    /// </summary>
    public static event Action<float> OnPlantDied;
    /// <summary>
    /// 植物死亡事件的调用
    /// </summary>
    /// <param name="NewWeather"></param>
    public static void CallOnPlantDied(float DeltaValue)
    {
        OnPlantDied?.Invoke(DeltaValue);
    }

    /// <summary>
    /// 植物成熟事件
    /// </summary>
    public static event Action<float> OnPlantRipe;
    /// <summary>
    /// 植物成熟事件的调用
    /// </summary>
    /// <param name="NewWeather"></param>
    public static void CallOnPlantRipe(float DeltaValue)
    {
        OnPlantRipe?.Invoke(DeltaValue);
    }
}
