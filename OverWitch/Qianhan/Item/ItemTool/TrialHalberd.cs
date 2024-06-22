using DamageSourceine;
using Entitying;
using EntityLivingBaseing;
using Items;
using WORLD;
using EntityPlayering;
public class TrialHalberd : Item
{
    private EntityPlayer player;
    private Entity entity;
    private World world;
    private Item item;
    private string name="青天化戟";
    private DamageSource source;

    public override bool hitEntity()
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
                player.isplayer = true;
                entity.setDamage(0);
                return false;
            }
            if (entity is EntityMob || entity is EntityLivingBase || !(entity is EntityPlayer))
            {
                if(!entity.isDead)
                {
                    entity.isDead=true;
                    source.setDamageBypassesArmor().setDamageIsAbsolute();
                    player.getHealth();
                    player.setHealth(0);
                    player.setDeath();
                    player.onDeath(new DamageSource("TrialHalberd"));
                }
                world.removeEntity(entity);
            }
        }
        return true;
    }
    public override void Update()
    {
        base.Update();
        onItemUpdate();

    }
    public override void onItemUpdate()
    {
        if(entity is EntityPlayer)
        {
            player.getHealth();
            if(player.currentHealth<=0&&player.isDead)
            {
                player.setMaxHealth(player.maxHealth);
                player.setHealth(player.maxHealth);
                player.isDead=false;
            }
        }
    }
}
