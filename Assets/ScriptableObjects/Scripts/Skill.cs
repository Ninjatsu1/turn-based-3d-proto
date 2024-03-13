using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Skills", order = 1)]

public class Skill : ScriptableObject
{
    public int DamageAmount = 1;
    public Button SkillButton;
}