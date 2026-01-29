using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class ZombieView : MonoBehaviour
{
    public Entity Entity;
    public EntityManager EntityManager;

    private LocalTransform localTransform;


    private Animator animator;
    private EZombieAnimationState currentState;
    private EZombieAnimationState lastState;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        // Does the according entity to this Visuals exist?
        if (!EntityManager.Exists(Entity))
        {
            //Note that this happens 1 Frame delayed to the entity death! With fixedUpdate maybe even more frames
            Destroy(this.gameObject);
            return;
        }

        // Sync Positions
        localTransform = EntityManager.GetComponentData<LocalTransform>(Entity); // localTransform is not a Refrence, it needs to be set every frame!
        transform.position = localTransform.Position;
        transform.rotation = localTransform.Rotation;
        Physics.SyncTransforms(); // This is an expensive Methode, that shouldnt be called in a Update Methode!!!!!!!!!!!! However its my only solution rn...

        if (!EntityManager.HasComponent<ZombieAnimationState>(Entity)) // Check if the entity holds the data to read the state
        {
            return;
        }

        //Get the current State from the Zombie:
        currentState = EntityManager.GetComponentData<ZombieAnimationState>(Entity).Value;
        if(currentState == lastState) // if nothing has changed, nothing has to be done
        { 
            return;
        }

        // Set the values in the Animator
        animator.SetBool("IsIdle", currentState == EZombieAnimationState.Idle);
        animator.SetBool("IsWalking", currentState == EZombieAnimationState.Walking);
        animator.SetBool("IsAttackingMelee", currentState == EZombieAnimationState.Attacking);

        lastState = currentState;
    }
}
