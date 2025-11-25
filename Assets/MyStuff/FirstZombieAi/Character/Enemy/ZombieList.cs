using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieList : MonoBehaviour
{
    public List<Zombie> Zombies = new List<Zombie>();

    public static ZombieList Instance;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
