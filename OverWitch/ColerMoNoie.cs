using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColerMoNoie : MonoBehaviour
{
    // Start is called before the first frame update
        public float colorChangeSpeed = 1f; // ������ɫ�仯�ٶ�
        public int updateTickRate = 10;  // ���Ƹ��µ�tickƵ��
        public Text text;

        private Color[] colors = new Color[] { Color.green, Color.blue, Color.red, Color.yellow, Color.white }; // ��ɫ����
        private int Tick = 0;
        private int currentColorIndex = 0;  // ��ǰ��ɫ������

        void Start()
        {
            StartCoroutine(ChangeTextColor());
        }

        public IEnumerator ChangeTextColor()
        {
            while (true)
            {
                Tick++; // ÿ֡����tick

                if (Tick >= updateTickRate) // �ﵽ�趨�ĸ�������
                {
                    Tick = 0; // ����tick

                    // ������ɫ���ɵı��� t
                    float t = Mathf.PingPong(Time.time * colorChangeSpeed, 1f);

                    // ��ǰ��ɫ����һ����ɫ
                    Color currentColor = colors[currentColorIndex];
                    Color nextColor = colors[(currentColorIndex + 1) % colors.Length];

                    // ƽ������
                    text.color = Color.Lerp(currentColor, nextColor, t);

                    // �����ɵ���һ����ɫʱ��������ɫ����
                    if (t == 0)
                    {
                        currentColorIndex = (currentColorIndex + 1) % colors.Length; // �л�����һ����ɫ
                    }
                }

                yield return null; // ÿ֡��飬��ֻ����tick�ﵽָ��ֵʱ������ɫ
            }
        }
}
