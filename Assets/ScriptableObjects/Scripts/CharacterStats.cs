using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Characters", order = 1)]
public class CharacterStats : ScriptableObject
{
    public string Name = "Name?";
    public int Attack = 5;
    public int MaximumHealth = 20;
    public int CurrentHealth = 0;
    public int Speed = 10;
    public GameObject Prefab;
    public bool IsPlayerCharacter = false;
}
