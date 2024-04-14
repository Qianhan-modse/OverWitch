using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class commandKill : commandBase
{
    public commandKill()
    {

    }

    public new String getName() { return "kill"; }

    public int getRequiredPermissionLevel() { return 2; }

    public new String gtUsage(ICommandSender commandSender) { return "command.kill.usage"; }
}
