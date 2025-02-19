using System.Collections;
using System.Collections.Generic;
using Colorine;
using UnityEngine;
using UnityEngine.UI;
//Õâ¸öÀýÍâ
public class NewBehaviourScript : MonoBehaviour
{
    public float colorChangeSpeed = 1f;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeTextColor());
    }

    public IEnumerator ChangeTextColor()
    {
        while (true)
        {
            float t = Mathf.PingPong(Time.time * colorChangeSpeed, 1f);
            text.color = ColorHelper.Overite(t, Color.green, Color.blue, Color.red, Color.yellow, Color.white);
            yield return null;
        }
    }
}
