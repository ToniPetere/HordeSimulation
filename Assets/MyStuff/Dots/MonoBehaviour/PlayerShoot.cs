using Unity.Entities;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private int damage;

    private EntityManager entityManager;

    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * range, Color.red, 5f);
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit, range))
            {
                // Debug:
                // Debug.Log("Raycast hit!");
                // Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * range, Color.red, 5f);
                // Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.position + (Camera.main.transform.forward * range), Color.red, 3f);

                if(hit.collider != null)
                {
                    Debug.Log("Hit: " + hit.collider.GetComponentInParent<Transform>().name);
                }


                if (hit.collider.TryGetComponent(out ZombieView entityRef))
                    {
                    entityManager.AddComponentData(entityRef.Entity,
                        new PendingDamage { value = damage });
                    Debug.Log("Damaged Entity with " + damage + " damage!");
                }
            }
        }
    }
}
