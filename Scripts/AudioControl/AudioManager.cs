using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Core;
using DG.Tweening;

public class AudioManager : Singleton<AudioManager>
{
    [Header("���Ե����Ĳ���")]
    public float ChangeTime = 1;
    public float FadeTime = 2;
    [Range(0, 1)]
    public float IconStrength = 1;
    [Range(0, 1)]
    public float ButtonStrength = 1;
    [Range(0, 1)]
    public float SliderStrength = 1;
    [Range(0, 1)]
    public float ClickStrength = 1;
    [Range(0, 1)]
    public float BGMStrength_Spring = 0.5f;
    [Range(0, 1)]
    public float BGMStrength_Summer = 0.5f;
    [Range(0, 1)]
    public float BGMStrength_Autumn = 0.5f;
    [Range(0, 1)]
    public float BGMStrength_Winter = 0.5f;
    [Range(0, 1)]
    public float RainStrength = 0.5f;
    [Range(0, 1)]
    public float SnowStrength = 0.5f;

    [Header("��Ҫ���õı�Ҫ��Դ")]
    public List<AudioClip> Audios=new List<AudioClip>();
    [Header("��¶�۲쵫�ǲ�Ҫ��")]
    public List<float> TrackStrength=new List<float>();
    

    private int NumAudioTracks=0;
    private List<AudioSource> AudioTracks = new List<AudioSource>();
    private int SeasonTrackID=-1;
    private int WeatherTrackID=-1;
    private int SeasonBgmTrackID = -1;
    private WeatherType LatestWeather = WeatherType.Snow;

    private void OnEnable()
    {
        EventHandler.OnChangeSeason += SeasonChangeProcess;
        EventHandler.OnChangeWeather += WeatherChangeProcess;
    }
    private void OnDisable()
    {
        EventHandler.OnChangeSeason -= SeasonChangeProcess;
        EventHandler.OnChangeWeather -= WeatherChangeProcess;
    }

    private void FixedUpdate()
    {
        int Count = 0;
        foreach(var i in AudioTracks)
        {
            i.volume = TrackStrength[Count];
            Count++;
        }
    }

    public int IndieTracksPlayAudio(string AudioName,bool IsLoop,float Strength)
    {
        AudioSource NewTrack = this.gameObject.AddComponent<AudioSource>();
        NewTrack.loop = IsLoop;
        AudioClip NewAudio = GetAudio(AudioName);
        if (NewAudio == null)
        {
            Debug.LogError("δ����AudioManager���ҵ���" + AudioName + "����Ƶ�ļ�");
            return -1;
        }
        NewTrack.clip = NewAudio;
        NewTrack.volume = Strength;
        NewTrack.Play();
        AudioTracks.Add(NewTrack);
        TrackStrength.Add(Strength);
        int TrackID = AudioTracks.Count - 1;
        return TrackID;
    }

    public void PlayAudioOnce(string AudioName)
    {
        AudioClip NewAudio = GetAudio(AudioName);
        if (NewAudio == null)
        {
            Debug.Log("δ�ҵ�"+AudioName);
            return;
        }
        GameObject NewAudioPlayer = new GameObject();
        NewAudioPlayer.name = AudioName+"������";
        AudioSource NewTrack = NewAudioPlayer.gameObject.AddComponent<AudioSource>();
        NewTrack.loop = false;
        NewTrack.clip = NewAudio;
        if (NewAudio.name == "�����") NewTrack.volume = ClickStrength;
        else if (NewAudio.name == "��������") NewTrack.volume = SliderStrength;
        else if (NewAudio.name == "��ť��Ч") NewTrack.volume = ButtonStrength;
        else if (NewAudio.name == "ͼ���л�") NewTrack.volume = IconStrength;
        else NewTrack.volume = 1;

        NewTrack.Play();
        StartCoroutine(OnAudioEnding(NewTrack));
    }

    IEnumerator OnAudioEnding(AudioSource ObservedOne)
    {
        yield return new WaitForSeconds(ObservedOne.clip.length);
        Debug.Log(ObservedOne + "���Ž���");
        DestroyImmediate(ObservedOne.gameObject);
    }

    public int GetTrackID(string AudioName)
    {
        int IDCount = 0;
        bool Flag = false;
        foreach (var i in AudioTracks)
        {
            if (i.name == AudioName)
            {
                Flag = true;
                break;
            }
            IDCount++;
        }

        if (Flag) return IDCount;
        Debug.LogError("δ����AudioManager���ҵ���" + AudioName + "����Ƶ�ļ�");
        return -1;
        
    }
    public AudioClip GetAudio(string AudioName)
    {
        AudioClip NewAudio = null;
        foreach (var i in Audios)
        {
            if (i.name == AudioName)
            {
                NewAudio = i;
                break;
            }
        }
        return NewAudio;
    }
    public void ChangeOneTrackClip(int AudioTrackID,string NewAudioName,bool IsLoop,float TargetStrength)
    {
        DOTween.To(() => TrackStrength[AudioTrackID], x => TrackStrength[AudioTrackID] = x, 0, ChangeTime)
        .OnStepComplete(() => 
        {
            ChangeTweenerCallBack(AudioTrackID,NewAudioName,IsLoop,TargetStrength);
        }).SetLoops(1);
    }

    private void ChangeTweenerCallBack(int AudioTrackID, string NewAudioName, bool IsLoop, float TargetStrength)
    {
        Tweener NewTweener = DOTween.To(() => TrackStrength[AudioTrackID], x => TrackStrength[AudioTrackID] = x, TargetStrength, ChangeTime);
        AudioTracks[AudioTrackID].clip = GetAudio(NewAudioName);
        AudioTracks[AudioTrackID].loop = IsLoop;
        AudioTracks[AudioTrackID].Play();
    }

    private void ChangeToSilent(int AudioTrackID)
    {
        Tweener NewTweener = DOTween.To(() => TrackStrength[AudioTrackID], x => TrackStrength[AudioTrackID] = x, 0, FadeTime);
    }
    public void MuteAllTracks()
    {
        foreach (var i in AudioTracks)
        {
            i.Stop();
        }
    }

    public void SeasonChangeProcess(SeasonsType NewSeason)
    {
        if (SeasonTrackID==-1)
        {
            switch (NewSeason)
            {
                case SeasonsType.Spring:
                    SeasonTrackID= IndieTracksPlayAudio("���챳����",true,1);
                    break;
                case SeasonsType.Summer:
                    SeasonTrackID = IndieTracksPlayAudio("���챳����", true,1);
                    break;
                case SeasonsType.Autumn:
                    SeasonTrackID = IndieTracksPlayAudio("���챳����", true,1);
                    break;
                case SeasonsType.Winter:
                    SeasonTrackID = IndieTracksPlayAudio("���챳����", true,1);
                    break;
            }
        }
        else
        {
            switch (NewSeason)
            {
                case SeasonsType.Spring:
                    ChangeOneTrackClip(SeasonTrackID, "���챳����", true,1);
                    break;
                case SeasonsType.Summer:
                    ChangeOneTrackClip(SeasonTrackID, "���챳����", true,1);
                    break;
                case SeasonsType.Autumn:
                    ChangeOneTrackClip(SeasonTrackID, "���챳����", true,1);
                    break;
                case SeasonsType.Winter:
                    ChangeOneTrackClip(SeasonTrackID, "���챳����", true,1);
                    break;
            }
        }

        if (SeasonBgmTrackID == -1)
        {
            switch (NewSeason)
            {
                case SeasonsType.Spring:
                    SeasonBgmTrackID = IndieTracksPlayAudio("��", true,BGMStrength_Spring);
                    break;
                case SeasonsType.Summer:
                    SeasonBgmTrackID = IndieTracksPlayAudio("��", true,BGMStrength_Summer);
                    break;
                case SeasonsType.Autumn:
                    SeasonBgmTrackID = IndieTracksPlayAudio("��", true,BGMStrength_Autumn);
                    break;
                case SeasonsType.Winter:
                    SeasonBgmTrackID = IndieTracksPlayAudio("��", true,BGMStrength_Winter);
                    break;
            }
        }
        else
        {
            switch (NewSeason)
            {
                case SeasonsType.Spring:
                    ChangeOneTrackClip(SeasonBgmTrackID, "��", true,BGMStrength_Spring);
                    break;
                case SeasonsType.Summer:
                    ChangeOneTrackClip(SeasonBgmTrackID, "��", true,BGMStrength_Summer);
                    break;
                case SeasonsType.Autumn:
                    ChangeOneTrackClip(SeasonBgmTrackID, "��", true,BGMStrength_Autumn);
                    break;
                case SeasonsType.Winter:
                    ChangeOneTrackClip(SeasonBgmTrackID, "��", true,BGMStrength_Winter);
                    break;
            }
        }
    }
    public void WeatherChangeProcess(WeatherType NewWeather)
    {
        if(WeatherTrackID==-1)
        {
            LatestWeather = NewWeather;
            switch (NewWeather)
            {
                case WeatherType.Rain:
                    WeatherTrackID = IndieTracksPlayAudio("��", true,RainStrength);
                    break;
                case WeatherType.Snow:
                    WeatherTrackID = IndieTracksPlayAudio("ѩ", true,SnowStrength);
                    break;
                default:
                    AudioSource NewTrack = this.gameObject.AddComponent<AudioSource>();
                    NewTrack.loop = true;
                    NewTrack.clip = GetAudio("��");
                    NewTrack.Stop();
                    break;
            }
        }
        else
        {
            if (NewWeather == LatestWeather) return;
            LatestWeather = NewWeather;
            switch (NewWeather)
            {
                case WeatherType.Rain:
                    ChangeOneTrackClip(WeatherTrackID, "��", true,1);
                    break;
                case WeatherType.Snow:
                    ChangeOneTrackClip(WeatherTrackID, "ѩ", true,1);
                    break;
                default:
                    ChangeToSilent(WeatherTrackID);
                    break;
            }
        }
    }
}
