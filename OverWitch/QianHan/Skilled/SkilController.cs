using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ܻ��࣬��Ϊ����ҿ������Լ̳��������
/// </summary>
public class SkilController : MonoBehaviour
{
    public EntityPlayer player;
    protected int skillTime = 0;
    protected float SkillDamage = 300.0F;//���ܻ����˺�
    // Start is called before the first frame update
    public void Start()
    {
        player = GetComponent<EntityPlayer>();
        player.SkilDamage = 300.0F;//��ҵĻ����˺���
        SkillDamage = player.SkilDamage;//�����˺�������ҵĻ����˺���ֵ/����
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
    //����ֵ��ʧ
    public void HealthADD(float value)
    {
        player.isSkil = false;//��ǰ���ܲ��Ǳ������͵ļ���
        //δ��
    }
}
