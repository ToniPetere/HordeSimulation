using System.Collections.Generic;
using UnityEngine;

public class CheckForEnemy : MonoBehaviour
{
    private Zombie zombie;

    private List<Transform> enemysInRange = new List<Transform>();

    private void Start()
    {
        zombie = GetComponentInParent<Zombie>();
        zombie.EnemysInRange = enemysInRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(zombie.EnemyTag)) return;

        Debug.Log($"Found {zombie.EnemyTag}!");
        Transform EnemyTransform = other.GetComponent<Transform>();
        enemysInRange.Add(EnemyTransform);

        zombie.IsEnemyInRange = true;
        zombie.Target = enemysInRange[0];
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag(zombie.EnemyTag)) return;

        enemysInRange.Remove(other.GetComponent<Transform>());
        if (enemysInRange.Count > 0)
        {
            zombie.Target = enemysInRange[0];
        }
        else
        {
            zombie.IsEnemyInRange = false;
        }
        // Debug.Log($"is {zombie.EnemyTag} in Range: " + zombie.IsEnemyInRange);
    }
}
