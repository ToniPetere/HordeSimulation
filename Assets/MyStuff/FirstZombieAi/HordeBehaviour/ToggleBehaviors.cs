using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ToggleBehaviors : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Zombie zombie)) // Is it a Zombie?
        {
            zombie.ControllType = EZombieControllType.BehaviourDriven; // Then activate its StateMachine
            //Debug.Log(zombie.name + ": now uses his FSM!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Zombie zombie)) // Is it a Zombie?
        {
            zombie.ControllType = EZombieControllType.HordeDriven; // Deactivate its FSM -> Activate HordeBehavior
            //Debug.Log(zombie.name + ": now uses HordeBehavior!");
        }
    }
}
