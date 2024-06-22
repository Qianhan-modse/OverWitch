using Assets.OverWitch.Qianhan.Entity.PlayerEntity;
using EntityLivingBaseing;
using UnityEngine;
namespace EntityPlayering
{
    public class EntityPlayer : EntityLivingBase
    {
        public new float MinDamage = 100f;
        public new float MaxDamage = 120f;
        public bool isplayer;
        public PlayerCapabilities capabilities=new PlayerCapabilities();

        public Vector3 position { get; set; }
        public Vector3 velocity { get; set; } = Vector3.zero;
        public Vector3 rotation { get; set; }
        public float speed { get; set; }
        public float rotationSpeed { get; set; }
        public float AttackDamage { get; set; }
        public float AttackSpeed { get; set; }

        private EntityPlayer(float minDamage, float maxDamage, bool isplayer, PlayerCapabilities capabilities, Vector3 position, Vector3 velocity, Vector3 rotation, float speed, float rotationSpeed, float attackDamage, float attackSpeed, float hEALTH)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            this.isplayer = isplayer;
            this.capabilities = capabilities;
            this.position = position;
            this.velocity = velocity;
            this.rotation = rotation;
            this.speed = speed;
            this.rotationSpeed = rotationSpeed;
            AttackDamage = attackDamage;
            AttackSpeed = attackSpeed;
        }

        public override void onUpdate()
        {
            base.onUpdate();

        }
    }
}