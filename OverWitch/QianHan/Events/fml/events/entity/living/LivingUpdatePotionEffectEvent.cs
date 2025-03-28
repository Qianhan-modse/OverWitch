using Assets.OverWitch.QianHan.Events.fml.common.eventhandler;
using Assets.OverWitch.QianHan.PotionEffects;

namespace Assets.OverWitch.QianHan.Events.fml.events.entity.living
{
    [Cancelable]
    public class LivingUpdatePotionEffectEvent:LivingEvent
    {
        private PotionEffect effect;
        public LivingUpdatePotionEffectEvent(EntityLivingBase entity, PotionEffect effect) : base(entity)
        {
            this.effect = effect;
        }
        public PotionEffect getEffect()
        {
            return effect;
        }
    }
}
