using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    [SerializeField] public float MaxHealth;
    [SerializeField] public float MeleeDamage;
    [SerializeField] public float MeleeRange;
}
