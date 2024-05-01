using DamageSourceine;
using Entitying;
using EntityLivingBaseing;
using Items;
public class TrialHalberd : Item
{
    private EntityPlayer player;
    private Entity entity;
    public static string Name="青天化戟";
    private DamageSource source;

    private bool hitEntity()
    {
        if(!entity.isEntity)
        {
            return false;
        }
        if(entity.isEntity)
        {
            entity.getEntity().getHealth();
            if(entity is EntityPlayer)
            {
                player.isplayer=true;
                entity.setDamage(0);
                return false;
            }
            if(entity is EntityMob||entity is EntityLivingBase)
            {
                player.isEntityAlive();
                source.setDamageBypassesArmor().setDamageIsAbsolute();
                player.setDamage(5000);
                player.getHealth();
                player.setHealth(0);
                player.setDeath();
            }
        }
        return true;
    }
}
