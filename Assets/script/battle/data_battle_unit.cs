public class data_battle_unit
{
    public string UnitName;
    public int CurrentHP;
    public int MaxHP;
    public int Attack;
    public int Speed;

    public data_battle_unit(data_battle_player data)
    {
        UnitName = data.playerName;
        MaxHP = data.maxHP;
        CurrentHP = data.maxHP;
        Attack = data.attack;
        Speed = data.speed;
    }

    public data_battle_unit(data_battle_character data)
    {
        UnitName = data.characterName;
        MaxHP = data.maxHP;
        CurrentHP = MaxHP;
        Attack = data.attack;
        Speed = data.speed;
    }
}