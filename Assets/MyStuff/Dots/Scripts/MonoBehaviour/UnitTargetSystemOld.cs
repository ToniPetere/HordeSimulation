using Unity.Collections;
using Unity.Entities;
using UnityEngine;


//Alter Ansatz:
public class UnitTargetSystemOld : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Replace this with the target
            Vector3 playerPosition = Camera.main.transform.position; // The player hold the Main Camera, so that indicates where the player is
            playerPosition.y = 0f;

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
//Glaube es war nötig, dass dies ein Monobehavior war, weil die Mausposition/Input nicht so einfach in dots ausgelesen werden kann. Not sure tho