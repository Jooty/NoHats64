public class KillContext
{

    public float time;
    public Character killer;
    public Character victim;

    public KillContext(float time, Character killer, Character victim)
    {
        this.killer = killer;
        this.victim = victim;
        this.time = time;
    }

    public bool CompareKiller(Character killer)
    {
        return this.killer == killer;
    }

    public bool CompareVictim(Character victim)
    {
        return this.victim == victim;
    }

    public bool CompareTime(float time)
    {
        return this.time == time;
    }

}