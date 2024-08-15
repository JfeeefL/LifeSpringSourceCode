using System;
using System.Collections;
using System.Collections.Generic;
using Core.Core;
using UnityEngine;
using UnityEngine.Playables;

public class WinLoseManager : Singleton<WinLoseManager>
{
    public PlayableDirector endingPlayable;
    public StartController startController;
    private void Start()
    {
        endingPlayable=GetComponent<PlayableDirector>();
        startController = GameObject.Find("StartAnimation").GetComponent<StartController>();
    }

    private void OnEnable()
    {
        EventHandler.OnPlantDied += OnPlantDied;
        EventHandler.OnPlantRipe += OnPlantRipe;
    }

    private void OnDisable()
    {
         EventHandler.OnPlantDied-=OnPlantDied;
         EventHandler.OnPlantRipe-=OnPlantRipe;
    }

    private void OnPlantRipe(float obj)
    {
        startController.NextLevelName = "StartScene";
        startController.isEnd = false;
        PlayEndingAnimation();
    }

    private void OnPlantDied(float obj)
    {
        startController.NextLevelName = "LoseScene";
        startController.isEnd = true;
        PlayEndingAnimation();
    }

    /// <summary>
    /// 游戏终止动画播放
    /// </summary>
    public void PlayEndingAnimation()
    {
        endingPlayable.Play();
    }
}
