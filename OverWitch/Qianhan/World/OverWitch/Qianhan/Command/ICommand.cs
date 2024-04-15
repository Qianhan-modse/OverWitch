using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valuitem;

public interface ICommand : Comparable<ICommand>
{
    String getName();
    String gtUsage(ICommandSender var1);
    List<String> getAliases();
}

public interface ICommandSender
{
}