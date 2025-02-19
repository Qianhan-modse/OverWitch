using System;
using UnityEngine;
using UnityEditor;

namespace TimeDate
{
    public class TimeManager:MonoBehaviour
    {
        public int timeScale=1;//Ê±¼ä±¶ÂÊ
        public int timeHour;
        public int timeMinute;

        public int timeSecond;

        public void onTimeUpdate()
        {
            DateTime datetime = DateTime.Now;
            timeHour = datetime.Hour;
            timeMinute = datetime.Minute;
            timeSecond = datetime.Second;
            //Debug.Log($"Game Time: {timeHour:D2}:{timeMinute:D2}");
        }
        public void Update()
        {
            onTimeUpdate();
        }
    }
}