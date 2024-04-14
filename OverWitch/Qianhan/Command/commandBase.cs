using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class commandBase : ICommand
{
    public int compareTo(ICommand o)
    {
        throw new System.NotImplementedException();
    }

    public List<string> getAliases()
    {
        throw new System.NotImplementedException();
    }

    public string getName()
    {
        throw new System.NotImplementedException();
    }

    public string gtUsage(ICommandSender var1)
    {
        throw new System.NotImplementedException();
    }
}
