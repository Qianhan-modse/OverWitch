using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ܻ��࣬��Ϊ����ҿ������Լ̳��������
/// </summary>
public class SkilController : EntityPlayer
{
    public EntityPlayer player;
    protected int skillTime = 0;
    protected float SkillDamage = 300.0F;//���ܻ����˺�
    // Start is called before the first frame update
    public override void Start()
    {
        SkilDamage = 300.0F;//��ҵĻ����˺���ֵ
        player= GetComponent<EntityPlayer>();
        SkillDamage = this.SkilDamage;//�����˺�������ҵĻ����˺���ֵ/����
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
    //����ֵ��ʧ
    public void HealthADD(float value)
    {
        this.isSkil = false;//��ǰ���ܲ��Ǳ������͵ļ���
        //δ��
    }
}
