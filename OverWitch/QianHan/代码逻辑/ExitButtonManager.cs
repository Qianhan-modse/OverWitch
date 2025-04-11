using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace OpenCover
{
    public class ExitButtonManager : MonoBehaviour
    {
        public Button exitButton;  // 退出按钮
        public VideoPlayer exitVideoPlayer;  // 退出视频播放器

        void Start()
        {
            // 确保按钮事件已绑定
            if (exitButton != null)
            {
                exitButton.onClick.AddListener(OnExitButtonClick); // 按钮点击事件
            }
            else
            {
                Debug.LogError("Exit Button is not assigned.");
            }

            // 确保视频播放器绑定
            if (exitVideoPlayer != null)
            {
                exitVideoPlayer.loopPointReached += OnVideoEnd; // 监听视频播放结束事件
            }
            else
            {
                Debug.LogError("Exit VideoPlayer is not assigned.");
            }
        }

        // 按钮点击后播放退出视频并关闭游戏
        void OnExitButtonClick()
        {
            PlayExitVideo();
        }

        // 播放退出视频
        private void PlayExitVideo()
        {
            if (exitVideoPlayer != null)
            {
                exitVideoPlayer.Play(); // 播放视频
            }
        }

        // 视频播放结束后退出游戏
        private void OnVideoEnd(VideoPlayer vp)
        {
            OnApplicationQuit();
            // 退出游戏
            Application.Quit();

            // 编辑模式下也能测试
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        private void OnApplicationQuit()
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
