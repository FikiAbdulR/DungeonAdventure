using UnityEngine;

[CreateAssetMenu(
    fileName = "Player Battle Data",
    menuName = "RPG/Battle/Player"
)]
public class data_battle_player : ScriptableObject
{
    [Header("Info")]
    public string playerName;

    [Header("Stats")]
    public int level;
    public int maxHP;
    public int attack;
    public int speed;

    [Header("Visual")]
    public GameObject modelPrefab;
}