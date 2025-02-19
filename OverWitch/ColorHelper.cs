using UnityEngine;

namespace Colorine
{
    public class ColorHelper
    {
        public static Color Overite(float t, params Color[] colors)
        {
            if (colors == null || colors.Length < 1)
            {
                return Color.white;
            }
            if (colors.Length == 1)
            {
                return colors[0];
            }

            float step = 1f / (colors.Length - 1);

            int startIndex = Mathf.FloorToInt(t / step);
            float subT = (t - startIndex * step) / step;

            if (startIndex >= colors.Length - 1)
            {
                return colors[colors.Length - 1];
            }
            else if (startIndex < 0)
            {
                return colors[0];
            }
            else
            {
                return Color.Lerp(colors[startIndex], colors[startIndex + 1], subT);
            }
        }
    }
}
