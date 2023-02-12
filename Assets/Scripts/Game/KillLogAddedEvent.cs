using System;

public delegate void KillLogAddedEvent(KillLogEventAddedArgs e);

public class KillLogEventAddedArgs : EventArgs
{

    private KillContext killContext;

    public KillLogEventAddedArgs(KillContext killContext)
    {
        this.killContext = killContext;
    }

    public KillContext GetContext() { return killContext; }

}