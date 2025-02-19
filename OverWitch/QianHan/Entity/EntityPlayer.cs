using Assets.OverWitch.QianHan.Items;
using Assets.OverWitch.QianHan.Util;
using Tyess;

public class EntityPlayer :EntityLivingBase
{
    //这个是玩家类型，目前这个类型还未完成
    public bool isPlayer;
    public int UID = 0;
    public float attackDamage;
    internal float criticalChance;
    internal float criticalDamage;
    internal float defense;
    internal float moveSpeed;
    internal float attackSpeed;
    protected float SkilDamage;

    internal  int CommandLevel;
    public override void Start()
    {
        base.Start();
        CommandLevel = 0;
    }

    public override ItemStack getItemStackFromSlot(EntityEquipmentSlot slotIn)
    {
        return this.getHeldItem(EnumHand.MAIN_HAND);
    }
    public void getValue(int value)
    {
        CommandLevel = value;
    }

    private ItemStack getHeldItem(EnumHand mAIN_HAND, EnumHand mAIN_HAND1)
    {
      return this.getHeldItem(EnumHand.MAIN_HAND, mAIN_HAND);  
    }

    internal ItemStack getHeldItem(EnumHand handIn)
    {
        return this.getHeldItem(EnumHand.MAIN_HAND, handIn);
    }
}
