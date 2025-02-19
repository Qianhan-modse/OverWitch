using System;
using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Log.network;
using UnityEngine;
using UnityEngine.UI;

namespace Commaand
{
    public class CommandStar : MonoBehaviours
    {
        public InputField inputField;
        public Button button;
        private Entity entity;
        // Start is called before the first frame update
        public override void Start()
        {
            // �󶨰�ť����¼��� ExecuteCommand ����
            button.onClick.AddListener(OnButtonClick);
            inputField.onEndEdit.AddListener(ExecuteCommand);
        }

        // ����ť���ʱ����
        void OnButtonClick()
        {
            ExecuteCommand(inputField.text);
        }

        public void ExecuteCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                Debug.Log("�����Ϊ��");
                return;
            }

            if (command.StartsWith("kill"))
            {
                KillCommands(command);
            }
            else
            {
                Debug.Log("δ֪����");
            }

            inputField.text = ""; // ��������
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
                            if (livingBase != null)
                            {
                                livingBase.onKillCommands();
                            }
                        }
                    }
                }
                Debug.Log("����ʵ���ѱ�ɱ��");
            }


            else if (commandParts.Length > 1 && commandParts[1] == "@p")
            {
                if (entity is EntityLivingBase)
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
                    Debug.Log("����ѱ���ɱ");
                }
            }
            else
            {
                Debug.Log("�����ʽ�����δָ��Ŀ��");
            }
        }
    }
}