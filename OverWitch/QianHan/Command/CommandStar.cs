using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Util;
using PlayerEntity;
using UnityEngine;
using UnityEngine.UI;

namespace Commaand
{
    public class CommandStar : MonoBehaviour
    {
        public InputField inputField;
        public Button button;
        private Entity entity;
        private DamageSource source;
        public EntityPlayer player;
        // Start is called before the first frame update
        public virtual void Start()
        {
            // 绑定按钮点击事件到 ExecuteCommand 方法
            button.onClick.AddListener(OnButtonClick);
            inputField.onEndEdit.AddListener(ExecuteCommand);
            source = new DamageSource();
            player = GetComponent<EntityPlayer>();
        }

        // 当按钮点击时调用
        void OnButtonClick()
        {
            ExecuteCommand(inputField.text);
        }

        public void ExecuteCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                Debug.Log("命令不能为空");
                return;
            }

            if (command.StartsWith("kill"))
            {
                KillCommands(command);
            }
            else
            {
                Debug.Log("未知命令");
            }

            inputField.text = ""; // 清空输入框
        }

        public void KillCommands(string command)
        {
            string[] commandParts = command.Split(' ');
            if (commandParts.Length > 1 && commandParts[1] == "@e")
            {
                Entity[] entities = FindObjectsOfType<Entity>();
                foreach (Entity entity in entities)
                {
                    if (entity is EntityLivingBase)
                    {
                        EntityLivingBase livingBase = (EntityLivingBase)entity;
                        if (entity != livingBase.GetComponent<Entity>())
                        {
                            livingBase = entity as EntityLivingBase;
                            if (livingBase != null&&livingBase.invulnerable)
                            {
                                livingBase.onKillCommands();
                            }
                        }
                    }
                }
                Debug.Log("所有实体已被杀死");
            }
            else if (commandParts.Length > 1 && commandParts[1] == "@p")
            {
                    EntityLivingBase livingBase = (EntityLivingBase)entity;
                    if (livingBase is EntityPlayer)
                    {
                        EntityPlayer entityPlayer = (EntityPlayer)livingBase;
                        if (entityPlayer != null)
                        {
                            entityPlayer.onKillCommands();
                        }
                    }
                    Debug.Log("玩家已被击杀");
            }
            else
            {
                Debug.Log("命令格式错误或未指定目标");
            }
        }
    }
}