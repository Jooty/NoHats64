[System.Serializable]
public class CharacterStats
{

    public float currentHealth;
    public float maximumHealth;
    public float damage;
    public float attackSpeedMultiplier;
    public float attackRange;
    public float moveSpeed;
    public float jumpForce;

    public CharacterStats(CharacterData startingData)
    {
        this.currentHealth         = startingData.baseHealth;
        this.maximumHealth         = startingData.baseHealth;
        this.damage                = startingData.baseDamage;
        this.attackSpeedMultiplier = startingData.baseAttackSpeed;
        this.attackRange           = startingData.baseAttackRange;
        this.moveSpeed             = startingData.baseMoveSpeed;
        this.jumpForce             = startingData.baseJumpForce;
    }

    public float GetSprintSpeed()
    {
        return this.moveSpeed * 1.35f;
    }

}