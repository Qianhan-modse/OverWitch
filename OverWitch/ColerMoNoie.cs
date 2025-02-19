using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColerMoNoie : MonoBehaviour
{
    // Start is called before the first frame update
        public float colorChangeSpeed = 1f; // 控制颜色变化速度
        public int updateTickRate = 10;  // 控制更新的tick频率
        public Text text;

        private Color[] colors = new Color[] { Color.green, Color.blue, Color.red, Color.yellow, Color.white }; // 颜色数组
        private int Tick = 0;
        private int currentColorIndex = 0;  // 当前颜色的索引

        void Start()
        {
            StartCoroutine(ChangeTextColor());
        }

        public IEnumerator ChangeTextColor()
        {
            while (true)
            {
                Tick++; // 每帧更新tick

                if (Tick >= updateTickRate) // 达到设定的更新周期
                {
                    Tick = 0; // 重置tick

                    // 计算颜色过渡的比例 t
                    float t = Mathf.PingPong(Time.time * colorChangeSpeed, 1f);

                    // 当前颜色和下一个颜色
                    Color currentColor = colors[currentColorIndex];
                    Color nextColor = colors[(currentColorIndex + 1) % colors.Length];

                    // 平滑过渡
                    text.color = Color.Lerp(currentColor, nextColor, t);

                    // 当过渡到下一个颜色时，更新颜色索引
                    if (t == 0)
                    {
                        currentColorIndex = (currentColorIndex + 1) % colors.Length; // 切换到下一个颜色
                    }
                }

                yield return null; // 每帧检查，但只有在tick达到指定值时更新颜色
            }
        }
}
