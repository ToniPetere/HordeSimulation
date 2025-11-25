using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SurvivorStats")]

public class SurvivorStats : ScriptableObject
{
    [SerializeField] public float RangedDamage;
    [SerializeField] public float RangedRange;
}
