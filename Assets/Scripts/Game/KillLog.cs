using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KillLog
{

    public static List<KillContext> killLog = new List<KillContext>();

    // events
    public static event KillLogAddedEvent OnKillAdded;

    public static void AddKill(KillContext killContext)
    {
        killLog.Add(killContext);

        OnKillAdded?.Invoke(new KillLogEventAddedArgs(killContext));
    }

}