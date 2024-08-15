using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMainCamOr : MonoBehaviour
{
   private Camera mainCam;

   private void Start()
   {
      mainCam = Camera.main;
   }

   private void LateUpdate()
   {
      GetComponent<Camera>().orthographicSize =  mainCam.orthographicSize;
   }
}
