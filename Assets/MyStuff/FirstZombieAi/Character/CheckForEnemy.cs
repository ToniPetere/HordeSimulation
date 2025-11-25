using System.Collections.Generic;
using UnityEngine;

public class CheckForEnemy : MonoBehaviour
{
    private Character character;

    private List<Transform> enemysInRange = new List<Transform>();

    private void Start()
    {
        character = GetComponentInParent<Character>();
        character.EnemysInRange = enemysInRange;
    }
    //private void Update()
    //{
    //    Is now in the get Funktion from the character.Target
    //    Tracking if the Target died:

    //    if (character.Target == null) // null = dead
    //    {
    //        character.IsEnemyInRange = false;

    //        if (enemysInRange.Count > 0)
    //        {
    //            List<Transform> deadOpponents = new List<Transform>();

    //            foreach (Transform enemy in enemysInRange)
    //            {
    //                //Identify All DeadOpponents until an alive was found
    //                if (enemy == null)
    //                {
    //                    deadOpponents.Add(enemy);
    //                }
    //                else
    //                {
    //                    character.Target = enemy;
    //                    character.IsEnemyInRange = true;
    //                    break;
    //                }
    //            }

    //            //Remove all DeadOpponents from the List
    //            foreach (Transform deadOpponent in deadOpponents)
    //            {
    //                enemysInRange.Remove(deadOpponent);
    //            }
    //        }
    //    }
    //    --> In der Get methode vom Target
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(character.EnemyTag)) return;

        // Debug.Log($"Found {character.EnemyTag}!");
        Transform EnemyTransform = other.GetComponent<Transform>();
        enemysInRange.Add(EnemyTransform);

        if (character.IsDefensive) return; // If the character is in Defense Mode, then just dont set its target now

        character.IsEnemyInRange = true;
        character.Target = enemysInRange[0];
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag(character.EnemyTag)) return;

        enemysInRange.Remove(other.GetComponent<Transform>());
        if (enemysInRange.Count > 0)
        {
            character.Target = enemysInRange[0];
        }
        else
        {
            character.IsEnemyInRange = false;
        }
        // Debug.Log($"is {character.EnemyTag} in Range: " + character.IsEnemyInRange);
    }
}
