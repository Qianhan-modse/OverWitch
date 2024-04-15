using System;
using System.Collections;
using System.Collections.Generic;
using EntityLivingBaseing;
using UnityEngine;
public class EntityPlayer : EntityLivingBase
{
    public new float MinDamage=100f;
    public new float MaxDamage=120f;
    public bool isplayer { get; set; }

    public Vector3 position{get;set;}

    public override void setDamage(int value)
    {
        if (isplayer == true)
        {
            this.setDamage(value);
        }
        else
        {
            this.isplayer = false;
            this.setDamage(value);
        }
        this.Damage(MinDamage,MaxDamage);
    }
}