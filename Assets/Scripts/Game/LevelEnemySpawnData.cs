[System.Serializable]
public class LevelEnemySpawnData
{

    public CharacterData enemyData;
    public int count;

    public LevelEnemySpawnData(CharacterData enemyData, int count)
    {
        this.enemyData = enemyData;
        this.count = count;
    }

}