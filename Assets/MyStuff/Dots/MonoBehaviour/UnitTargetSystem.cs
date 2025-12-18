using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class UnitTargetSystem : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 playerPosition = Camera.main.transform.position; // The player hold the Main Camera, so that indicates where the player is
            playerPosition.y = 0;

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager; 
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover>().Build(entityManager);

            NativeArray<UnitMover> unitMoverArray = entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp); //Dont use ToComponentArray! Its for when you work with classes and not IComponents!
            for (int i = 0; i < unitMoverArray.Length; i++)
            {
                UnitMover unitMover = unitMoverArray[i];
                unitMover.targetPosition = playerPosition;
                unitMoverArray[i] = unitMover;
            }
            entityQuery.CopyFromComponentDataArray(unitMoverArray);
        }
    }

}
