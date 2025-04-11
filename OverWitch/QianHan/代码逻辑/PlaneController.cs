using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class PlaneController : MonoBehaviour
{
    public Animator animator;
    public Button startButton;
    public string animatorTriggerName;
    private bool isFlying;
    public int tickCount;
    private bool isAnimator;
    public int tickThreshold = 2000;

    // Start is called before the first frame update
    void Start()
    {
        tickCount = 0;
        // 检查 Animator 组件
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true; // 设置触发器

        // 判断是否获取到 Animator 组件
        isAnimator = animator != null;

        // 确保按钮事件绑定
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartFlight);
        }
        else
        {
            Debug.LogError("Button component is missing.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 只有在动画开始后才进入飞行逻辑
        if (isFlying)
        {
            if(isFlying)
            {
                //tickCount += (int)Mathf.FloorToInt(Time.deltaTime * 250);
                tickCount++;
                if(tickCount>=tickThreshold)
                {
                    tickCount = 0;
                    isFlying = false;
                    SceneManager.LoadScene("主场景");
                }
            }
        }
    }
    private void StartFlight()
    {
        if (isAnimator)
        {
            animator.SetTrigger(animatorTriggerName);
            isFlying = true; // 标记为正在飞行
            tickCount = 0; // 重置计数器
        }
        else
        {
            Debug.LogError("Animator component is missing.");
        }
    }
}