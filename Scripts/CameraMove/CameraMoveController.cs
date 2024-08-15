using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Gameplay;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMoveController : MonoBehaviour
{
   [Header("基础移动速度")]
   public float moveSpeed;

   [Header("基础缩放速度")]
   public float scrollerSpeed;

   private bool isClicked = false;
   private Vector2 clickedPoint;
   private Camera mainCamera;
   public CinemachineVirtualCamera cm1;
   private void Start()
   {
      Application.targetFrameRate = 60;
      mainCamera=Camera.main;
   }

   private void Update()
   {
      //按下鼠标时
      if (Input.GetKeyDown(KeyCode.Mouse0))
      {
         if (IsPointerOverGameObject(Input.mousePosition))
         {
            isClicked = true;
            clickedPoint = Input.mousePosition;
         }
      }

      //持续按着且是Click的时
      if (isClicked&& Input.GetKey(KeyCode.Mouse0))
      {
       
         var deltaPosition = (Vector2)Input.mousePosition - clickedPoint;
         var deltaPercent = -new Vector2(deltaPosition.x / Screen.width, deltaPosition.y / Screen.height);
         clickedPoint = Input.mousePosition;
         Debug.Log(deltaPercent);
         //mainCamera.transform.position=mainCamera.transform.position+ (Vector3)(deltaPercent*Time.deltaTime*moveSpeed*mainCamera.orthographicSize);
         cm1.transform.position=cm1.transform.position+ (Vector3)(deltaPercent*Time.deltaTime*moveSpeed*cm1.m_Lens.OrthographicSize);
         var cmTransform = cm1.transform;
         if (cmTransform.position.x > EarthManager.Instance.rightest+3)
            cmTransform.position = new Vector3(EarthManager.Instance.rightest+3,cmTransform.position.y,cmTransform.position.z);
         if (cmTransform.position.x < EarthManager.Instance.leftest+3)
            cmTransform.position = new Vector3(EarthManager.Instance.leftest+3,cmTransform.position.y,cmTransform.position.z);
         if (cmTransform.position.y > 9)
            cmTransform.position = new Vector3(cmTransform.position.x,9,cmTransform.position.z);
         if (cmTransform.position.y < EarthManager.Instance.lowest)
            cmTransform.position = new Vector3(cmTransform.position.x,EarthManager.Instance.lowest,cmTransform.position.z);
      }
      
      //滚轴输入
      if (Input.GetAxis("Mouse ScrollWheel") != 0)
      {
         cm1.m_Lens.OrthographicSize = cm1.m_Lens.OrthographicSize - Input.GetAxis("Mouse ScrollWheel") * scrollerSpeed*Time.deltaTime;
         cm1.m_Lens.OrthographicSize = Mathf.Clamp(cm1.m_Lens.OrthographicSize,4, 10);
         mainCamera.transform.GetChild(0).GetComponent<Camera>().orthographicSize =  cm1.m_Lens.OrthographicSize;
      }
      
       
      //抬起鼠标时
      if (Input.GetKeyUp(KeyCode.Mouse0))
      {
         isClicked = false;
      
         
            
      }
   }


   /// <summary>
   /// 鼠标得到几个物体,小于等于2返回真
   /// </summary>
   /// <param name="screenPosition"></param>
   /// <returns></returns>
   public bool IsPointerOverGameObject(Vector2 screenPosition)
   {
      //实例化点击事件
      PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
      //将点击位置的屏幕坐标赋值给点击事件
      eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

      List<RaycastResult> results = new List<RaycastResult>();
      //向点击处发射射线
      EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
      return results.Count <=1;

   }

   public void CameraScrollMove()
   {
      
   }

   public void CameraPositionMove()
   {
      
   }

   public void CameraLimit()
   {
      
   }
}
