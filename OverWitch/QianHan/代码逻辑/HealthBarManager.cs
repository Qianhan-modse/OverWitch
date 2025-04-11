using PlayerEntity;
using UnityEngine;
using UnityEngine.UI;
namespace HealthManager
{
    [RequireComponent(typeof(Text))]
    public class HealthBarManager : MonoBehaviour
    {
        public EntityPlayer player;      // ͨ��Inspector��ֵ�����ʵ��
        public Text healthBarText;       // ��ʾѪ���� Text

        public float UpdateSpeed = 2.0F; // �����ٶȣ�ƽ�����ɣ�
        public float TextUpdateSpeed = 2.0F; // �ı����µ�ƽ���ٶ�

        private float currentHealthPercentage; // ��ǰѪ������
        private float targetHealthPercentage;  // Ŀ��Ѫ������

        private float currentHealth;         // ��ǰѪ��
        private float targetHealth;          // Ŀ��Ѫ��

        void Start()
        {
            if (player == null)
            {
                Debug.LogError("Player reference is not assigned in the inspector!");
                return;
            }

            if (healthBarText == null)
                healthBarText = GameObject.Find("HealthBarText").GetComponent<Text>();

            // ��ʼ��
            currentHealth = player.getHealth();
            targetHealth = currentHealth;
            currentHealthPercentage = currentHealth / player.getMaxHealth();
            targetHealthPercentage = currentHealthPercentage;
        }

        void Update()
        {
            if (player != null)
            {
                // ÿ֡����Ѫ���ı�
                UpdateHealthText();
            }
        }

        // ����Ѫ���ı���ƽ������
        private void UpdateHealthText()
        {
            if (healthBarText == null || player == null) return;

            // ��ȡ��ǰѪ��
            currentHealth = player.getHealth();

            // ƽ�������ı��е�Ѫ����ֵ
            targetHealth = currentHealth; // Ŀ��Ѫ���ǵ�ǰѪ��
            float currentTextHealth = Mathf.Lerp(currentHealth, targetHealth, TextUpdateSpeed * Time.deltaTime);

            // �����ı���ʾ
            healthBarText.text = $"{(int)currentTextHealth}/{(int)player.getMaxHealth()}";
        }

        // ǿ�Ƹ���Ѫ���������ֶ����ã�
        public void ForceUpdate()
        {
            UpdateHealthText();
        }
    }
}
