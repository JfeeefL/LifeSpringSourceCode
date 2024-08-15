using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    public GameObject HideMask;
    public string NextLevelName = "TestScene1";
    public float WaitTimes=10;
    public float InitialTime = 1f;
    public bool isEnd;

    private bool Flag = false;
    public bool isPressedAny;
    public void StartGame()
    {
        StartCoroutine(Loadlevel(WaitTimes));
    }

    public void QuitGame()
    {
        Debug.Log("游戏结束");
        Application.Quit();
    }

    private void Start()
    {
        StartCoroutine(InitialWait(InitialTime));
    }

    private void Update()
    {
        if(isPressedAny)
            if (Input.anyKey&&Flag)
            {
                StartGame();
                Flag = false;
            }
    }
    /// <summary>
    /// 加载关卡方式
    /// </summary>
    public void LoadLevelMethod()
    {
        Debug.Log("执行");
        if (isEnd)
            SceneManager.LoadSceneAsync(NextLevelName);
        else
            isEnd = !isEnd;
    }

    public void ReloadScene()
    {
        NextLevelName=SceneManager.GetActiveScene().name;
        StartGame();
    }

    IEnumerator Loadlevel(float WaitTime)
    {
        HideMask.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(NextLevelName);
        operation.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(WaitTime);
        operation.allowSceneActivation = true;
    }

    IEnumerator InitialWait(float WaitTime)
    {
        yield return new WaitForSecondsRealtime(WaitTime);
        Flag = true;
    }
}
