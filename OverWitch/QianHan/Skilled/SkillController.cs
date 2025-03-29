using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能基类，因为是玩家控制所以继承自玩家类
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

        // **动态注册技能**
        public void AddSkill(SkillBase skill)
        {
            if (skill != null && !skills.ContainsKey(skill.SkillName))
            {
                skills.Add(skill.SkillName, skill);
                skill.SetOwner(player); // 让技能知道它属于哪个玩家
            }
        }

        // **选择技能**
        public void SelectSkill(string skillName)
        {
            if (skills.TryGetValue(skillName, out SkillBase skill))
            {
                currentSkill = skill;
                Debug.Log($"切换技能: {skillName}");
            }
        }

        // **释放技能**
        public void ActivateSkill()
        {
            currentSkill?.ActivateSkill();
        }
    }
}
