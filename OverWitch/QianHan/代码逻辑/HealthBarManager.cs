using PlayerEntity;
using UnityEngine;
using UnityEngine.UI;
namespace HealthManager
{
    [RequireComponent(typeof(Text))]
    public class HealthBarManager : MonoBehaviour
    {
        public EntityPlayer player;      // 通过Inspector赋值的玩家实例
        public Text healthBarText;       // 显示血量的 Text

        public float UpdateSpeed = 2.0F; // 更新速度（平滑过渡）
        public float TextUpdateSpeed = 2.0F; // 文本更新的平滑速度

        private float currentHealthPercentage; // 当前血量比例
        private float targetHealthPercentage;  // 目标血量比例

        private float currentHealth;         // 当前血量
        private float targetHealth;          // 目标血量

        void Start()
        {
            if (player == null)
            {
                Debug.LogError("Player reference is not assigned in the inspector!");
                return;
            }

            if (healthBarText == null)
                healthBarText = GameObject.Find("HealthBarText").GetComponent<Text>();

            // 初始化
            currentHealth = player.getHealth();
            targetHealth = currentHealth;
            currentHealthPercentage = currentHealth / player.getMaxHealth();
            targetHealthPercentage = currentHealthPercentage;
        }

        void Update()
        {
            if (player != null)
            {
                // 每帧更新血量文本
                UpdateHealthText();
            }
        }

        // 更新血量文本的平滑过渡
        private void UpdateHealthText()
        {
            if (healthBarText == null || player == null) return;

            // 获取当前血量
            currentHealth = player.getHealth();

            // 平滑更新文本中的血量数值
            targetHealth = currentHealth; // 目标血量是当前血量
            float currentTextHealth = Mathf.Lerp(currentHealth, targetHealth, TextUpdateSpeed * Time.deltaTime);

            // 更新文本显示
            healthBarText.text = $"{(int)currentTextHealth}/{(int)player.getMaxHealth()}";
        }

        // 强制更新血条（可以手动调用）
        public void ForceUpdate()
        {
            UpdateHealthText();
        }
    }
}
