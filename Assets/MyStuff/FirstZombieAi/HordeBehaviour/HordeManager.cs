using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeManager : MonoBehaviour
{
    [SerializeField] private List<Zombie> zombiesInHorde = new List<Zombie>(); // RN Manual adding of Zombies to the List
    [SerializeField] private float intervallForNewWaypoint = 5f; // Is now a fixed number, maybe make it more felxible and depending on the Hordesize. This is not a good Solution!
    private void Awake()
    {
        // Add logic to add Zombies Automatically, if they are close
        // Or add logic to spawn a specific amount of Zombies, what this horde should exist off

        //after all Zombies are added/spawned:
        foreach (Zombie zombie in zombiesInHorde)
        {
            zombie.ControllType = EZombieControllType.HordeDriven;
        }
    }
    private void Start()
    {
        StartCoroutine(GenerateWaypoints());
    }

    private IEnumerator GenerateWaypoints()
    {
        while (true)
        {

            yield return new WaitForSeconds(intervallForNewWaypoint);

            zombiesInHorde[0].WalkPoint = this.transform.position; // test if it works
            zombiesInHorde[0].HasWalkPoint = true;

            Debug.Log(zombiesInHorde[0].name + " now Moves to: " + this.transform.position);
        }
    }
}
