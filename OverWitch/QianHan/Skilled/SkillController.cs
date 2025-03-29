using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ܻ��࣬��Ϊ����ҿ������Լ̳��������
/// </summary>
namespace Assets.OverWitch.QianHan.Skilled.PlayerSkill
{
    public class SkillController : MonoBehaviour
    {
        public EntityPlayer player { get; private set; }
        private SkillBase currentSkill;
        private Dictionary<string, SkillBase> skills = new Dictionary<string, SkillBase>();

        void Awake()
        {
            player = GetComponent<EntityPlayer>();
            if (player != null)
            {
                player.skillDamage = 300.0F;
            }
        }

        void Update()
        {
            currentSkill?.UpdateSkill(Time.deltaTime);
        }

        // **��̬ע�Ἴ��**
        public void AddSkill(SkillBase skill)
        {
            if (skill != null && !skills.ContainsKey(skill.SkillName))
            {
                skills.Add(skill.SkillName, skill);
                skill.SetOwner(player); // �ü���֪���������ĸ����
            }
        }

        // **ѡ����**
        public void SelectSkill(string skillName)
        {
            if (skills.TryGetValue(skillName, out SkillBase skill))
            {
                currentSkill = skill;
                Debug.Log($"�л�����: {skillName}");
            }
        }

        // **�ͷż���**
        public void ActivateSkill()
        {
            currentSkill?.ActivateSkill();
        }
    }
}
