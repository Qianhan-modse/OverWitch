using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenCover
{
    public class GameManager : MonoBehaviour
    {
        public void OnApplicationQuit()
        {
            // 清理内存
            System.GC.Collect();
            Resources.UnloadUnusedAssets();

            // 保存玩家数据（如果有）
            PlayerPrefs.Save();

            // 记录日志
            Debug.Log("游戏退出，执行清理和保存操作");
        }
    }
}
