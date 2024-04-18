using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameRiule{
public class GameRules
{
    private Dictionary<String,bool>rules;

    public GameRules()
    {
        rules=new Dictionary<string, bool>();
    }

    public void setRule(string ruleName,bool value)
    {
        rules[ruleName]=value;
    }

    public bool getBool(string ruleName)
    {
        if(rules.ContainsKey(ruleName))
        {
            return rules[ruleName];
        }
        else
        {
            return false;
        }
    }
}

    public class WorldInfo
    {

        private GameRules gameRules;
        public GameRules getGameRulesInstance()
        {
            return this.gameRules;
        }
    }
}