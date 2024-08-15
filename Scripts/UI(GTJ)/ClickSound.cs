using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.Instance.PlayAudioOnce("Êó±êµã»÷");
        }
    }
}
