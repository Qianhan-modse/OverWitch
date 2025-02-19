using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能基类，因为是玩家控制所以继承自玩家类
/// </summary>
public class SkilController : EntityPlayer
{
    public EntityPlayer player;
    protected int skillTime = 0;
    protected float SkillDamage = 300.0F;//技能基础伤害
    // Start is called before the first frame update
    public override void Start()
    {
        SkilDamage = 300.0F;//玩家的基础伤害数值
        player= GetComponent<EntityPlayer>();
        SkillDamage = this.SkilDamage;//技能伤害等于玩家的基础伤害数值/技能
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
    //生命值流失
    public void HealthADD(float value)
    {
        this.isSkil = false;//当前技能不是必中类型的技能
        //未完
    }
}
