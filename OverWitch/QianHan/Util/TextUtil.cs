using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace OverWitch.QianHan.Util
{
    /*public class TextUtil
    {
        private static readonly Color[] fabulousness = new Color[]
        {
        Color.red, new Color(1f, 0.65f, 0f), // Gold
        Color.yellow, Color.green, Color.cyan, Color.blue, new Color(0.75f, 0.3f, 0.85f) // Light Purple
        };

        private static readonly Color[] sanic = new Color[]
        {
        Color.blue, Color.blue, Color.blue, Color.blue, Color.white, Color.blue, Color.white, Color.white,
        Color.blue, Color.white, Color.white, Color.blue, Color.red, Color.white, Color.gray
        };

        // Call this method to apply the fabulous color formatting
        public static string MakeFabulous(string input)
        {
            return LudicrousFormatting(input, fabulousness, 80.0f, 1, 1);
        }

        // Call this method to apply the SANIC color formatting
        public static string MakeSanic(string input)
        {
            return LudicrousFormatting(input, sanic, 50.0f, 2, 1);
        }

        // Optimized method using StringBuilder to handle color animation for the text
        private static string LudicrousFormatting(string input, Color[] colours, float delay, int step, int posStep)
        {
            if (delay <= 0.0f)
                delay = 0.001f;

            float timeOffset = Time.time / delay; // Use Unity's Time to control the animation
            int offset = Mathf.FloorToInt(timeOffset) % colours.Length;

            StringBuilder sb = new StringBuilder(input.Length * 15); // Estimate the size to avoid frequent reallocations

            for (int i = 0; i < input.Length; i++)
            {
                int colorIndex = (i * posStep + colours.Length - offset) % colours.Length;
                string colorCode = ColorUtility.ToHtmlStringRGB(colours[colorIndex]);
                sb.Append($"<color=#{colorCode}>{input[i]}</color>");
            }

            return sb.ToString();
        }
    }*/

    public class I18r : MonoBehaviour
    {
        public Text text; // 需要改变颜色的文本
        public bool isColor = true; // 控制颜色循环开关
        private Color[] colors = new Color[] {
        new Color(0.7f, 0.7f, 0.7f), // 白色（柔和）
        new Color(0.2f, 0.2f, 1f),   // 蓝色（柔和）
        new Color(1f, 1f, 0.2f),     // 黄色（柔和）
        new Color(0.2f, 1f, 1f),     // 青色（柔和）
        new Color(0.2f, 1f, 0.2f),   // 绿色（柔和）
        new Color(1f, 0.2f, 0.2f),   // 红色（柔和）
        new Color(1f, 0.2f, 1f),     // 品红色（柔和）
        new Color(0.5f, 0.5f, 0.5f), // 灰色（柔和）
        new Color(0.5f, 0.25f, 0f),  // 深棕色（柔和）
        new Color(1f, 0.5f, 0f),     // 橙色（柔和）
        new Color(0.2f, 0.2f, 1f)    // 浅蓝色（柔和）
    };

        public int tickRate = 40; // 控制每隔多少帧更新一次颜色
        public int frameCounter = 0; // 用于控制更新频率
        private float waveSpeed = 1.0F;

        public virtual void Start()
        {
            // 启动颜色波纹的变化协程
            StartCoroutine(ChangeTextColor());
        }

        private IEnumerator ChangeTextColor()
        {
            int colorIndex = 0; // 从第一个颜色开始
            string textContent = text.text; // 获取当前文本内容

            while (true)
            {
                // 控制每隔一定帧数才更新一次颜色
                if (frameCounter >= tickRate)
                {
                    string coloredText = "";

                    // 控制颜色逐步从左到右进行“波纹”变化
                    for (int i = textContent.Length - 1; i >= 0; i--)
                    {
                        // 根据颜色的进度为每个字符赋不同的颜色
                        Color currentColor = colors[(colorIndex + i) % colors.Length];
                        string colorHex = ColorUtility.ToHtmlStringRGB(currentColor);
                        coloredText += $"<color=#{colorHex}>{textContent[i]}</color>"; // 给字符添加颜色

                        //Color startColor = colors[colorIndex % colors.Length];
                        //Color endColor = colors[(colorIndex + 1) % colors.Length];
                        float lerpFactor = Mathf.PingPong(Time.time * waveSpeed, 1); // PingPong效果让颜色渐变更平滑
                        //Color currentColor = Color.Lerp(startColor, endColor, lerpFactor);

                        //string colorHex = ColorUtility.ToHtmlStringRGB(currentColor);
                        //coloredText += $"<color=#{colorHex}>{textContent[i]}</color>"; // 给字符添加颜色
                    }

                    // 更新文本内容
                    text.text = coloredText;

                    // 更新颜色顺序
                    colorIndex = (colorIndex + 1) % colors.Length; // 循环颜色的顺序
                    frameCounter = 0; // 重置计数器
                }
                else
                {
                    frameCounter++; // 增加计数器
                }

                yield return null; // 等待下一帧
            }
        }
    }
}
