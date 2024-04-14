using Entitying;
using UnityEngine;


public class EntityMob : Entity
{
    public EntityPlayer player;
    public GameObject bulletPrefab;
    public float bulletSpeed=10f;

    Entity entity;
    public float moveSpeed = 5f;
    public float attackRange=20f;
    public override void Start()
    {
        entity.Damage(5f,10f);
    }
    public override void onUpdate()
    {
        if(player!=null&&player.isplayer)
        {
            float distanceToPlayer=Vector3.Distance(transform.position,player.position);
            if(distanceToPlayer<=40)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
                if(player.isplayer&&distanceToPlayer<=attackRange)
                {
                    Shoot();
                }
                if(distanceToPlayer<=20f)
                {
                    Attack();
                }
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector3 direction = (player.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
    }

    void Attack()
    {
        int damageToDeal= (int)Random.Range(MinDamage,MaxDamage+1);
        player.GetComponent<EntityPlayer>().TakeDamage(damageToDeal);
    }
}
