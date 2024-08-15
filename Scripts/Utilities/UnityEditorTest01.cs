using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
public class UnityEditorTest01 : MonoBehaviour
{
    /*
     * 李纪甫
     * 邹力恒
     * 高天骥
     * 高青骐
     * 冯翌宸
     * 河山
     * 谢知语
     */ 
    [ValueDropdown("TestFunc2"), OnValueChanged("DebugSelf")]
   public string func1;
   private void DebugSelf()
    {
        Debug.Log(func1);
    }

    private static IEnumerable TestFunc1=new ValueDropdownList<int>()
    {
        { "Small", 256 },
        { "Medium", 512 },
        { "Large", 1024 },
    };
#if UNITY_EDITOR
    private static IEnumerable TestFunc2()
    {
        List<string> stringList = new List<string>(); //新列表（空）
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                //scene.enabled 是否在中启用此场景，以获取如何使用此类的示例。另请参见：EditorBuildSettingsScene、EditorBuildSettings.scenes
                string str1 = scene.path.Substring(scene.path.LastIndexOf('/') + 1); //获取尾地址场景名字（xxx.asset）
                string str2 = str1.Substring(0, str1.Length - 6); //只取除.asset 6个字符以外的名字
                stringList.Add(str2);
            }
        }

        return stringList;
    }
#endif
}
