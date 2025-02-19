using OverWitch.QianHan.Entities;

namespace OverWitch.QianHan.Util
{
    public class DamageSource
    {
        public static DamageSource ANVIL = new DamageSource("anvil");
        public static DamageSource CRAMMING = (new DamageSource("cramming")).setDamageBypassesArmor();
        public static DamageSource DROWN = (new DamageSource("drown")).setDamageBypassesArmor();
        public static DamageSource FALL = (new DamageSource("fall")).setDamageBypassesArmor();
        public static DamageSource FALLING_BLOCK = new DamageSource("fallingBlock");
        public static DamageSource FIREWORKS = (new DamageSource("fireworks")).setExplosion();
        public static DamageSource FLY_INTO_WALL = (new DamageSource("flyIntoWall")).setDamageBypassesArmor();
        public static DamageSource GENERAL = new DamageSource("general");
        public static DamageSource GENERIC = (new DamageSource("generic")).setDamageBypassesArmor();
        public static DamageSource HOT_FLOOR = (new DamageSource("hotFloor")).setFireDamage();

        //伤害来源或者伤害源
        public static DamageSource IN_FIRE = (new DamageSource("inFire")).setFireDamage();
        public static DamageSource IN_WALL = (new DamageSource("inWall")).setDamageBypassesArmor();
        public static DamageSource LAVA = (new DamageSource("lava")).setFireDamage();
        public static DamageSource LIGHTNING_BOLT = new DamageSource("lightningBolt");
        public static DamageSource MAGIC = (new DamageSource("magic")).setDamageBypassesArmor().setDamageIsMagicDamage();
        public static DamageSource ON_FIRE = (new DamageSource("onFire")).setDamageBypassesArmor().setFireDamage();
        public static DamageSource OUT_OF_WORLD = (new DamageSource("outOfWorld")).setDamageBypassesArmor();
        public static DamageSource SKIL = (new DamageSource("skil").setExplosion().setDifficultyScaled());
        public static DamageSource WITHER = (new DamageSource("wither")).setDamageBypassesArmor();
        public bool attackDamage;//是否是攻击伤害
        public string damageType;//伤害类型，以this连用表示当前伤害类型
        public Entity entity;//实体属性
        private float currentDamage;//当前伤害值
                                    //private bool isDamageAllowedInCreativeMode;//是否在操作模式受伤，这个引用自minecraft;
        private bool damageIsAbsolute;//伤害是否是绝对值
        private bool difficultyScaled;//是否随难度变幻伤害
        private bool explosion;//是否为爆炸伤害
        private bool fireDamage;//是否是火焰伤害
        private float hungerDamage = 0.1F;//饥饿伤害值
        private bool IsUnblockable;//是否不可拦截
        private bool magicDamage;//是否为法术伤害
        private bool projectile;//是否是弹射物伤害

        public DamageSource()
        {

        }

        public DamageSource(string v)
        {
            this.v = v;
        }

        public string v { get; set; }
        public string getDamageType()//获取伤害类型
        {
            return this.damageType;
        }
        public float getHungerDamage()//获取饥饿伤害
        {
            return this.hungerDamage;
        }
        public Entity getTrueSource()
        {
            return entity;
        }

        public bool isDamageAbsolute()//判断是否为绝对伤害
        {
            return this.damageIsAbsolute;
        }

        public bool isFireDamage()//判断是否为火焰伤害
        {
            return this.fireDamage;
        }

        public bool isProjectile()//用于判断是否为弹射物伤害
        {
            return this.projectile;
        }
        public bool isUnblockable()//判断是否为不可防御伤害
        {
            return this.IsUnblockable; ;
        }
        public DamageSource setDamageBypassesArmor()//设置伤害无视护甲
        {
            this.IsUnblockable = true;
            this.hungerDamage = 0.0F;
            return this;
        }

        public DamageSource setDamageIsAbsolute()//设置绝对伤害
        {
            this.damageIsAbsolute = true;
            this.hungerDamage = 0.0F;
            return this;
        }
        public DamageSource setDamageIsMagicDamage()
        {
            this.magicDamage = true;
            this.currentDamage = 300.0F;
            return this;
        }
        public DamageSource setDifficultyScaled()//设置伤害随难度改变
        {
            this.difficultyScaled = true;
            return this;
        }
        public DamageSource setExplosion()//设置爆炸伤害
        {
            this.explosion = true;
            return this;
        }

        public DamageSource setFireDamage()//设置火焰伤害
        {
            this.fireDamage = true;
            return this;
        }

        //伤害来源

        public DamageSource setVoidDamage()//设置虚空伤害
        {
            this.difficultyScaled = false;//不允许伤害随难度变化
            this.currentDamage = float.MaxValue;//当前伤害值为float的最大伤害值
            return this;
        }
    }
}