using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private List<CharacterStats> characters = new List<CharacterStats>();
    [SerializeField] private List<CharacterStats> turnOrder = new List<CharacterStats>();

    private void Start()
    {
        GetCharacters();
        SetTurnOrder();
    }

    //Gets player and enemies
    private void GetCharacters() 
    {
        Character[] charactersArray;
        charactersArray = FindObjectsOfType<Character>();
        for (int i = 0; i < charactersArray.Length; i++)
        {
            characters.Add(charactersArray[i].GetComponent<Character>().CharacterStats);
        }
    }

    private void SetTurnOrder()
    {
       turnOrder = characters.OrderByDescending(character => character.Speed).ToList();
    }
}
