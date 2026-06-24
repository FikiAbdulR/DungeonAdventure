using UnityEngine;
public enum CharacterType
{
    Companion,
    Enemy
}

[CreateAssetMenu(
    fileName = "Character",
    menuName = "RPG/Battle/Character"
)]
public class data_battle_character : ScriptableObject
{
    [Header("Info")]
    public string characterName;
    public CharacterType characterType;
    public string typeID;

    [Header("Stats")]
    public int maxHP;
    public int attack;
    public int speed;
    public int xpReward;

    [Header("Visual")]
    public GameObject modelPrefab;
}