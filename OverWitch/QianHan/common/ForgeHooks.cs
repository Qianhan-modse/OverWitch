using Assets.OverWitch.QianHan.Events.fml.common;
using Assets.OverWitch.QianHan.Events.fml.events.entity.living;
using Assets.OverWitch.QianHan.PotionEffects;
using EntityLivingBaseEvent;
using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Util;
using PlayerEntity;

namespace Assets.OverWitch.QianHan.common
{
    public class ForgeHooks
    {
        static Entity e;
        
        public static void onLivingSetAttackTarget(EntityLivingBase entityLiving,EntityLivingBase entity)
        {
            ForgeIronteted.EVENT_BUS.post(new LivingSetAttackTargetEvent(entityLiving, entity));
        }
        public static bool onLivingUpdate(EntityLivingBase livingBase)
        {
            return ForgeIronteted.EVENT_BUS.post(new LivingUpdateEvent(livingBase));
        }
        public static bool onLivingJump(EntityLivingBase livingBase)
        {
            return ForgeIronteted.EVENT_BUS.post(new LivingJumpEvent(livingBase));
        }
        public static bool onLivingAttack(EntityLivingBase entity,DamageSource source,float value)
        {
            return entity as EntityPlayer || !ForgeIronteted.EVENT_BUS.post(new LivingBaseAttackEvent(entity, source, value));
        }
        public static bool onPlayerAttack(EntityLivingBase entity,DamageSource src,float amount)
        {
            return !ForgeIronteted.EVENT_BUS.post(new LivingBaseAttackEvent(entity, src, amount));
        }
        public static float onLivingHurt(EntityLivingBase entity,DamageSource source,float a)
        {
            LivingBaseHurtEvent @event=new LivingBaseHurtEvent(entity, source, a);
            return (ForgeIronteted.EVENT_BUS.post(@event)) ? 0 : @event.getAmount();
        }
        public static bool onLivingDeath(EntityLivingBase entity,DamageSource source)
        {
            return ForgeIronteted.EVENT_BUS.post(new LivingBaseDeathEvent(entity,source));
        }
        public static float onLivingDamage(EntityLivingBase entity,DamageSource source,float ount)
        {
            LivingBaseDamageEvent @event=new LivingBaseDamageEvent(entity, source, ount);
            return(ForgeIronteted.EVENT_BUS.post(@event)) ? 0 : @event.getAmount();
        }
        public static bool onLivingDrops(EntityLivingBase entity, DamageSource source, ArrayList<Entity> drops, int lootionglLevel, bool recentlyHit)
        {
            return ForgeIronteted.EVENT_BUS.post(new LivingBaseDropsEvent(entity, source, drops, lootionglLevel, recentlyHit));
        }
        public static void onLivingFall(EntityLivingBase entity, float distance, float damageMultiplier)
        {
            ForgeIronteted.EVENT_BUS.post(new LivingBaseFallEvent(entity, distance, damageMultiplier));
        }
        public static bool onLivingHeal(EntityLivingBase entity, float amount)
        {
            return ForgeIronteted.EVENT_BUS.post(new LivingBaseHealEvent(entity, amount));
        }
        public static bool onLivingUpdatePotionEffect(EntityLivingBase entity, PotionEffect effect)
        {
            return ForgeIronteted.EVENT_BUS.post(new LivingUpdatePotionEffectEvent(entity, effect));
        }
    }
}
