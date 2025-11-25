using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Character
{
    [Header("Walkpoint Stuff")]
    [SerializeField] public float WalkpointRange;


    private void Start()
    {
        ZombieList.Instance.Zombies.Add(this);
    }

    private void OnDestroy()
    {
        ZombieList.Instance.Zombies.Remove(this);
    }
}
