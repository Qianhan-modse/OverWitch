using System;
using Assets.OverWitch.QianHan.Items;
using Assets.OverWitch.QianHan.Util;
using OverWitch.QianHan.Items;
using OverWitch.QianHan.Util;
using Tyess;

public class EntityPlayer :EntityLivingBase
{
    //�����������ͣ�Ŀǰ������ͻ�δ���
    public bool isPlayer;
    public int UID = 0;
    public float attackDamage;
    public float criticalChance;
    public float criticalDamage;
    public float defense;
    public float moveSpeed;
    public float attackSpeed;
    public float SkilDamage;

    internal  int CommandLevel;
    public override void Start()
    {
        //base.Start();
        CommandLevel = 0;
    }

    public override ItemStack getItemStackFromSlot(EntityEquipmentSlot slotIn)
    {
        return null;
    }

    internal ItemStack getHeldItem(EnumHand handIn)
    {
        return null;
    }
    public override void Update()
    {
       // onEntityUpdate();
    }
    public override void onEntityUpdate()
    {
        base.onEntityUpdate();
        if(this.getHealth()<=0)
        {
            onDeath(DamageSource.GENERIC);
        }
    }
    //Ϊ�˱����ڴ��޷�����ʱ�ͷ��ѱ�ע��
    /*public override ItemStack getItemStackFromSlot(EntityEquipmentSlot slotIn)
    {
        return this.getHeldItem(EnumHand.MAIN_HAND);
    }
    public void getValue(int value)
    {
        CommandLevel = value;
    }

    private ItemStack getHeldItem(EnumHand mAIN_HAND)
    {
      return this.getHeldItem(EnumHand.MAIN_HAND);  
    }

    internal ItemStack getHeldItem(EnumHand handIn)
    {
        //return this.getHeldItem(EnumHand.MAIN_HAND);
    }*/
}
