using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenCover
{
    public class GameManager : MonoBehaviour
    {
        public void OnApplicationQuit()
        {
            // �����ڴ�
            System.GC.Collect();
            Resources.UnloadUnusedAssets();

            // ����������ݣ�����У�
            PlayerPrefs.Save();

            // ��¼��־
            Debug.Log("��Ϸ�˳���ִ������ͱ������");
        }
    }
}
