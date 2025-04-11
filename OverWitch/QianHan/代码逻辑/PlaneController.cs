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
        // ��� Animator ���
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true; // ���ô�����

        // �ж��Ƿ��ȡ�� Animator ���
        isAnimator = animator != null;

        // ȷ����ť�¼���
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
        // ֻ���ڶ�����ʼ��Ž�������߼�
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
                    SceneManager.LoadScene("������");
                }
            }
        }
    }
    private void StartFlight()
    {
        if (isAnimator)
        {
            animator.SetTrigger(animatorTriggerName);
            isFlying = true; // ���Ϊ���ڷ���
            tickCount = 0; // ���ü�����
        }
        else
        {
            Debug.LogError("Animator component is missing.");
        }
    }
}