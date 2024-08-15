using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WeatherEffect : MonoBehaviour
{
    [Header("可设置参数")]
    [Header("雨受力相关的参数")]
    public Vector2 MinRainForce = new Vector2(0, 0);
    public Vector2 MaxRainForce = new Vector2(3, 0);
    [Header("雪受力相关的参数")]
    public Vector2 MinSnowForce = new Vector2(0, 0);
    public Vector2 MaxSnowForce = new Vector2(3, 0);
    [Header("雨强度的参数")]
    public float MinRainStrength = 75;
    public float MaxRainStrength = 150;
    [Header("雪强度的参数")]
    public float MinSnowStrength = 100;
    public float MaxSnowStrength = 150;
    [Header("天气变化时间")]
    public float ChangeTime = 2;
    public float CloudsChangeTime = 0.5f;
    public float SnowStage1Time = 2;
    public float SnowStage2Time = 2;
    public float SnowStage3Time = 2;
    [Header("不同天气下的背景颜色")]
    public Color32 SunsineBackGround = new Color32(240, 240, 240, 255);
    public Color32 RainBackGround = new Color32(230, 230,230,250);
    public Color32 DroughtBackGround = new Color32(255, 255, 255, 255);
    public Color32 SnowBackGround = new Color32(200, 200, 200, 200);

    [Header("配置需要塞的东西")]
    public GameObject BackGround;
    public GameObject Plant;
    public GameObject Snow1,
                                    Snow2,
                                    Snow3;
    public GameObject Clouds;

    private ParticleSystem RainParticleSystem;
    private ParticleSystem SnowParticleSystem;
    private ParticleSystem.ForceOverLifetimeModule RainForceSetting;
    private ParticleSystem.ForceOverLifetimeModule SnowForceSetting;
    private ParticleSystem.EmissionModule RainEmissionSetting;
    private ParticleSystem.EmissionModule SnowEmissionSetting;
    private Color32 CurrentBackgroundColor = new Color32(255,255,255,255);
    private bool SnowBeginGenerateFlag = false;
    private bool SnowBeginFadeFlag = false;
    private float SnowBeginGenerateTime;
    private float SnowBeginFadeTime;
    private int SnowStage = 0;

    private void OnEnable()
    {
        EventHandler.OnChangeWeather += ChangeWeatherTo;
    }

    private void OnDisable()
    {
        EventHandler.OnChangeWeather -= ChangeWeatherTo;
    }

    private void Awake()
    {
        RainParticleSystem = this.transform.Find("Rain").GetComponent<ParticleSystem>();
        SnowParticleSystem = this.transform.Find("Snow").GetComponent<ParticleSystem>();
        RainForceSetting = RainParticleSystem.forceOverLifetime;
        SnowForceSetting = SnowParticleSystem.forceOverLifetime;
        RainEmissionSetting = RainParticleSystem.emission;
        SnowEmissionSetting = SnowParticleSystem.emission;
        CurrentBackgroundColor = BackGround.GetComponent<Image>().color;
        ChangeSnow(0);
        RandomForceGenerator();
        CloseAllWeatherEffect();
    }

    private void FixedUpdate()
    {
        BackGround.GetComponent<Image>().color = CurrentBackgroundColor;
        ChangeSnow(SnowStage);
        if (SnowBeginGenerateFlag)
        {
            if(Time.time-SnowBeginGenerateTime>SnowStage1Time|| Time.time - SnowBeginGenerateTime > SnowStage2Time|| Time.time - SnowBeginGenerateTime > SnowStage3Time)
            {
                SnowStage++;
                SnowBeginGenerateTime = Time.time;
                if (SnowStage == 4)
                {
                    SnowStage = 3;
                    SnowBeginGenerateFlag = false;
                }
            }
        }
        if (SnowBeginFadeFlag)
        {
            if (Time.time - SnowBeginFadeTime > SnowStage3Time || Time.time - SnowBeginFadeTime > SnowStage2Time || Time.time - SnowBeginFadeTime > SnowStage1Time)
            {
                SnowStage--;
                SnowBeginFadeTime = Time.time;
                if (SnowStage <0)
                {
                    SnowStage = 0;
                    SnowBeginFadeFlag = false;
                }
            }
        }
    }

    public void ChangeWeatherTo(WeatherType NewWeather)
    {
        CloseAllWeatherEffect();
        RandomForceGenerator();
        RandomEmissionGenerator();
        switch (NewWeather)
        {
            case WeatherType.Sunshine:
                EventHandler.CallOnSunsine(1);
                CloudsFade();
                BackGroundColoeChange(SunsineBackGround);
                Plant.transform.GetComponent<Animator>().SetBool("Snow", false);
                PlantController.Instance.IsSnow = false;
                SnowBeginFadeFlag = true;
                SnowBeginGenerateFlag = false;
                SnowBeginFadeTime = Time.time;
                break;
            case WeatherType.Rain:
                EventHandler.CallOnRain(2);
                CloudsGenerate();
                RainParticleSystem.Play();
                BackGroundColoeChange(RainBackGround);
                Plant.transform.GetComponent<Animator>().SetBool("Snow", false);
                PlantController.Instance.IsSnow = false;
                SnowBeginFadeFlag = true;
                SnowBeginGenerateFlag = false;
                SnowBeginFadeTime = Time.time;
                break;
            case WeatherType.Drought:
                EventHandler.CallOnDrought(3);
                CloudsFade();
                BackGroundColoeChange(DroughtBackGround);
                Plant.transform.GetComponent<Animator>().SetBool("Snow", false);
                PlantController.Instance.IsSnow = false;
                SnowBeginFadeFlag = true;
                SnowBeginGenerateFlag = false;
                SnowBeginFadeTime = Time.time;
                break;
            case WeatherType.Snow:
                EventHandler.CallOnSnow(4);
                CloudsGenerate();
                SnowParticleSystem.Play();
                BackGroundColoeChange(SnowBackGround);
                Plant.transform.GetComponent<Animator>().SetBool("Snow", true);
                PlantController.Instance.IsSnow = true;
                SnowBeginGenerateFlag = true;
                SnowBeginFadeFlag = false;
                SnowBeginGenerateTime = Time.time;
                break;
        }
    }

    private void CloseAllWeatherEffect()
    {
        RainParticleSystem.Stop();
        SnowParticleSystem.Stop();
    }

    private void ChangeSnow(int ID)
    {
        switch (ID)
        {
            case 1:
                Snow1.SetActive(true);
                Snow2.SetActive(false);
                Snow3.SetActive(false);
                break;
            case 2:
                Snow1.SetActive(false);
                Snow2.SetActive(true);
                Snow3.SetActive(false);
                break;
            case 3:
                Snow1.SetActive(false);
                Snow2.SetActive(false);
                Snow3.SetActive(true);
                break;
            default:
                Snow1.SetActive(false);
                Snow2.SetActive(false);
                Snow3.SetActive(false);
                break;
        }
    }

    private void RandomForceGenerator()
    {
        RainForceSetting.x = Random.Range(MinRainForce.x, MaxRainForce.x);
        RainForceSetting.y = Random.Range(MinRainForce.y, MaxRainForce.y);

        SnowForceSetting.x = Random.Range(MinSnowForce.x, MaxSnowForce.x);
        SnowForceSetting.y = Random.Range(MinSnowForce.y, MaxSnowForce.y);
    }
    private void RandomEmissionGenerator()
    {
        RainEmissionSetting.rateOverTime = Random.Range(MinRainStrength, MaxRainStrength);
        SnowEmissionSetting.rateOverTime = Random.Range(MinSnowStrength, MaxSnowStrength);
    }

    private void BackGroundColoeChange(Color32 NewColor)
    {
        DOTween.To(() => CurrentBackgroundColor, x => CurrentBackgroundColor = x, NewColor, ChangeTime);
        BackGround.GetComponent<Image>().color = CurrentBackgroundColor;
    }

    private void CloudsGenerate()
    {
        for(int i = 0; i < Clouds.transform.childCount; i++)
        {
            Clouds.transform.GetChild(i).GetComponent<SpriteRenderer>().DOColor(new Color32(255, 255, 255, 255), CloudsChangeTime);
        }
    }
    private void CloudsFade()
    {
        for (int i = 0; i < Clouds.transform.childCount; i++)
        {
            Clouds.transform.GetChild(i).GetComponent<SpriteRenderer>().DOColor(new Color32(255, 255, 255, 0), CloudsChangeTime);
        }
    }
}
