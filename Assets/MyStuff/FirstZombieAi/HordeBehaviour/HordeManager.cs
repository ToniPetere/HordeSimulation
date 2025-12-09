using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeManager : MonoBehaviour
{
    [SerializeField] private List<Zombie> zombiesInHorde = new List<Zombie>(); // RN Manual adding of Zombies to the List
    [SerializeField] private float intervallForNewWaypoint = 5f; // Is now a fixed number, maybe make it more felxible and depending on the Hordesize. This is not a good Solution!
    [SerializeField] private float NewWaypointRange = 5f; // Is now a fixed number, maybe make it more felxible and depending on the Hordesize. This is not a good Solution!

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

            foreach (Zombie zombie in zombiesInHorde)
            {
                if (zombie.ControllType != EZombieControllType.HordeDriven || zombie.HasWalkPoint)
                    continue;

                zombie.WalkPoint = GenerateNewWalkpoint(NewWaypointRange); // no check if the Walkpoint is Valid (For example: not in a Wall)!
                zombie.HasWalkPoint = true;


                //Debug:
                //Debug.Log(zombiesInHorde[0].name + " now Moves to: " + zombie.WalkPoint);
                var marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                marker.transform.position = zombie.WalkPoint;
                marker.GetComponent<Collider>().enabled = false;
                Destroy(marker, intervallForNewWaypoint);
            }
        }
    }


    private Vector3 ConvertWalkpointIntoRelativDirection(Vector3 _walkpoint) // Not good, as Zombies will just walk around the whole map, and generate Walkpoint in Invalid locations!
    {
        return _walkpoint - transform.position;
    }
    private Vector3 GenerateNewWalkpoint(float _range)
    {
        float randomX = Random.Range(-_range, _range);
        float randomZ = Random.Range(-_range, _range);

        Vector3 Walkpoint = this.transform.position + new Vector3(randomX, 0f, randomZ);
        return Walkpoint;
    }
}
