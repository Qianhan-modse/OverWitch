using System;
using System.Collections;
using DamageSourceine;
using Entitying;
using UnityEngine;
using Assets.OverWitch.Qianhan.Damage.util.text.translation;

namespace DeBuff
{
    public class Debuff
    {
        private I18n l18;
        private static float value;
        public float TimeDebuff = value;
        public DamageSource Source;

        private bool isWither;
        public Entity entity;
        public float debuffDamage = value;
        public float level = value;

        private bool isPoisoned;

        public Debuff()
        {
            debuffDamage = 1;
            TimeDebuff = 200;
            entity = new Entity();
            level = 1;
        }

        private void onEntityDebuffUpdate()
        {
            if (level > 1)
            {
                setDebuffDamage(value);
            }
        }

        private void setDebuffDamage(float damageValue)
        {
            if (!isPoisoned)
            {
                isPoisoned = true;
            }
            else
            {
                level++;
                TimeDebuff = 200f;
            }
            debuffDamage = damageValue;
        }

        public void wither(Entity entity, DamageSource source)
        {
            if (entity.isEntity && isWither == false)
            {
                isWither = true;
                source = new DamageSource("wither");
                source.setDifficultyScaled().setDamageIsAbsolute();
                l18.StartCoroutine(WitherEffect(entity));
                entity.currentHealth = value;
                if (entity.currentHealth == 0 && TimeDebuff > 0)
                {
                    entity.onDeath(new DamageSource("wither"));
                }
            }

        }

        public void poisoning(Entity entity, DamageSource source)
        {
            if (entity.isEntity && isPoisoned == false)
            {
                isPoisoned = true;
                source = new DamageSource("poisoning");
                source.setDamageBypassesArmor().setDamageIsAbsolute().setDifficultyScaled();
                l18.StartCoroutine(PoisonEffect(entity));
                entity.currentHealth = value;
                if (entity.currentHealth == 0 && TimeDebuff > 0)
                {
                    entity.isDeath(entity);
                }
                else { entity.setDeath(); }
            }
        }

        private IEnumerator WitherEffect(Entity entity)
        {
            while (TimeDebuff > 0.0F)
            {
                yield return new WaitForSeconds(1f);
                TimeDebuff--;
                entity.getHealth();
                entity.GetCurrentHealth();
                entity.currentHealth -= level;
                if (entity.currentHealth == 0)
                {
                    entity.onDeath(new DamageSource("wither"));
                    yield break;
                }
            }
            TimeDebuff = 0;
            entity.isClearDebuff = true;
        }

        private IEnumerator PoisonEffect(Entity entity)
        {
            while (TimeDebuff > 0.0F)
            {
                yield return new WaitForSeconds(1f);
                TimeDebuff--;
                entity.GetCurrentHealth();
                entity.currentHealth -= level;

                if (entity.currentHealth < 0 || (entity.isDamage == true))
                {
                    entity.setDamage(value);
                    entity.setDeath();
                    yield break;
                }
            }
            TimeDebuff = 0;
            entity.isClearDebuff = true;
        }
    }
}
