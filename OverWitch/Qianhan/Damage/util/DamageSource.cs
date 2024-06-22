using System;
using Entitying;
using EntityPlayering;
using EntityLivingBaseing;
using Assets.OverWitch.Qianhan.Damage.util.text.translation;

namespace DamageSourceine
{
    public class DamageSource
    {
        //属性和字段
        public Entity entity;//实体属性
        public bool attackDamage;//是否是攻击伤害
        private bool IsUnblockable;//是否不可拦截
        private bool isDamageAllowedInCreativeMode;//是否在操作模式受伤，这个引用自minecraft;
        private bool damageIsAbsolute;//伤害是否是绝对值
        private float hungerDamage = 0.1F;//饥饿伤害值
        private bool fireDamage;//是否是火焰伤害
        private bool projectile;//是否是弹射物伤害
        private bool difficultyScaled;//是否随难度变幻伤害
        private bool magicDamage;//是否为法术伤害
        private bool explosion;//是否为爆炸伤害
        public string damageType;//伤害类型，以this连用表示当前伤害类型
        private float currentDamage;//当前伤害值
        //伤害源
        public static DamageSource IN_FIRE = (new DamageSource("inFire")).setFireDamage();//来自火焰的伤害；
        public static DamageSource ENTITY = new DamageSource("entity");//来自实体的伤害；
        public static DamageSource OUT_OF_WORLD = new DamageSource("out_of_world").setDamageBypassesArmor();//来自掉出世界的伤害；
        public static DamageSource INFINITY = new DamageSource("infinity").setDamageIsAbsolute().setInfinityDamage();//来自无限的伤害；
        public static DamageSource LIGHTNING_BOLT = new DamageSource("lightningBolt").setDamageBypassesArmor();//来自闪电的伤害
        public static DamageSource VOID = new DamageSource("void").setVoidDamage().setDamageBypassesArmor();//来自虚空的伤害
        //未完成
        public string v { get; set; }

        //获取死亡信息，这个还未完成
        /*
        public ITextCompnent getDeathMessage(EntityLivingBase entityLivingBase)
        {
            EntityLivingBase Base = entityLivingBase.getAttackingEntity();
            string str = "death.attack." + this.damageType;
            string sing = str + ".player";
            return (ITextCompnent)(Base != null && I18n.canTranslate(sing) ? new TextComponentTranslation(sing, new Object[] { entityLivingBase.getDisplayName(), Base.getDisplayName() }) : new TextComponentTranslation(str, new Object[] { entityLivingBase.getDisplayName() }));
        }*/

        //伤害来源

        public DamageSource setVoidDamage()//设置虚空伤害
        {
            this.difficultyScaled = false;//不允许伤害随难度变化
            this.currentDamage = Float.MAX_VALUE;//当前伤害值为float的最大伤害值
            return this;
        }
        public DamageSource setInfinityDamage()//设置无线伤害
        {
            this.entity.setHealth(this.entity.getHealth());//调用来自entity的setHealth并传入0这个值来归零实体血量
            return this; 
        }
        public bool isProjectile()//用于判断是否为弹射物伤害
        {
            return this.projectile;
        }
        public DamageSource setExplosion()//设置爆炸伤害
        {
            this.explosion = true;
            return this;
        }
        public bool isUnblockable()//判断是否为不可防御伤害
        {
            return this.IsUnblockable; ;
        }
        public float getHungerDamage()//获取饥饿伤害
        {
            return this.hungerDamage;
        }
        public bool canHarmInCreative()//判断是否能在创造模式受伤
        {
            return this.isDamageAllowedInCreativeMode;
        }
        public bool isDamageAbsolute()//判断是否为绝对伤害
        {
            return this.damageIsAbsolute;
        }


        public DamageSource(String v)
        {
            this.damageType = v;
        }

        public static implicit operator DamageSource(bool v)
        {
            throw new NotImplementedException();
        }
        public Entity getImmediateSource()
        {
            return this.getTrueSource();
        }

        public DamageSource setDamageIsAbsolute()//设置绝对伤害
        {
            this.damageIsAbsolute = true;
            this.hungerDamage = 0.0F;
            return this;
        }
        public DamageSource setDamageBypassesArmor()//设置伤害无视护甲
        {
            this.IsUnblockable = true;
            this.hungerDamage = 0.0F;
            return this;
        }

        public DamageSource setFireDamage()//设置火焰伤害
        {
            this.fireDamage=true;
            return this;
        }

        public bool isFireDamage()//判断是否为火焰伤害
        {
            return this.fireDamage;
        }
        public String getDamageType()//获取伤害类型
        {
            return this.damageType;
        }
        public DamageSource setDifficultyScaled()//设置伤害随难度改变
        {
            this.difficultyScaled = true;
            return this;
        }

        public DamageSource setMagicDamage()//设置魔法伤害
        {
            this.magicDamage = true;
            return this;
        }


        public Entity getAttacker()
        {
            throw new NotImplementedException();
        }
        public Entity getTrueSource()
        {
            return null;
        }


        public bool isCreativePlayer()//判断是否为创造模式玩家
        {
            Entity e=this.getTrueSource();
            return e is EntityPlayer && ((EntityPlayer)e).capabilities.isCreativeMode;
        }
    }
}