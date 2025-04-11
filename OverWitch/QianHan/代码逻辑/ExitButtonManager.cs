using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace OpenCover
{
    public class ExitButtonManager : MonoBehaviour
    {
        public Button exitButton;  // �˳���ť
        public VideoPlayer exitVideoPlayer;  // �˳���Ƶ������

        void Start()
        {
            // ȷ����ť�¼��Ѱ�
            if (exitButton != null)
            {
                exitButton.onClick.AddListener(OnExitButtonClick); // ��ť����¼�
            }
            else
            {
                Debug.LogError("Exit Button is not assigned.");
            }

            // ȷ����Ƶ��������
            if (exitVideoPlayer != null)
            {
                exitVideoPlayer.loopPointReached += OnVideoEnd; // ������Ƶ���Ž����¼�
            }
            else
            {
                Debug.LogError("Exit VideoPlayer is not assigned.");
            }
        }

        // ��ť����󲥷��˳���Ƶ���ر���Ϸ
        void OnExitButtonClick()
        {
            PlayExitVideo();
        }

        // �����˳���Ƶ
        private void PlayExitVideo()
        {
            if (exitVideoPlayer != null)
            {
                exitVideoPlayer.Play(); // ������Ƶ
            }
        }

        // ��Ƶ���Ž������˳���Ϸ
        private void OnVideoEnd(VideoPlayer vp)
        {
            OnApplicationQuit();
            // �˳���Ϸ
            Application.Quit();

            // �༭ģʽ��Ҳ�ܲ���
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        private void OnApplicationQuit()
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
