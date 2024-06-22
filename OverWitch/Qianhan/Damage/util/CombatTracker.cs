using System;
using System.Collections.Generic;
using Assets.OverWitch.Qianhan.Damage.util;
using Assets.OverWitch.Qianhan.FML.Common.collect;
using EntityLivingBaseing;

public class CombatTracker
{
    private List<CombatEntry> combatEntries = List.newArrayList();
    private EntityLivingBase fighter;
    private int lastDamageTime;
    private int combatStartTime;
    private int combatEndTime;
    private bool inCombat;
    private bool takingDamage;
    private String fallSuffix;

    public CombatTracker(EntityLivingBase entityLivingBase)
    {
        this.fighter = entityLivingBase;
    }

}
