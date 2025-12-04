using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Zombie : MonoBehaviour, IDamageable
{
    // Setup:
    #region HealthSystem
    [SerializeField] private UI_Bar healthbar;
    [SerializeField] private float currentHealth;
    private float maxHealth;
    public float CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

    public void TakeDamage(float _value, Transform _attacker)
    {
        currentHealth -= _value;
        CheckForDeath();

        if (Target == null) // if the defending Character hasnt already a Target, then the attacker will be its Target
            Target = _attacker;


        if (healthbar != null)
        {
            healthbar.UpdateBar(currentHealth, maxHealth);
        }
    }

    private void CheckForOverheal()
    {
        // There currently is no method to heal in the Game!
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
    private void CheckForDeath()
    {
        if (currentHealth <= 0) Die();
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
    #endregion

    #region ZombieListing
    private void Start()
    {
        ZombieList.Instance.Zombies.Add(this);
    }

    private void OnDestroy()
    {
        ZombieList.Instance.Zombies.Remove(this);
    }
    #endregion

    [SerializeField] private CharacterStats soCharacterStats;
    [HideInInspector] public Animator Animator;

    // Enemy Detection:
    [Space, SerializeField] private string enemyTag;
    private Transform target;
    private bool isEnemyInRange;
    private bool isDefensive;

    // Properties:
    #region Properties
    public string EnemyTag { get { return enemyTag; } }

    public bool IsEnemyInRange { get { return isEnemyInRange; } set { isEnemyInRange = value; } }

    public Transform Target
    {
        get
        {
            // Tracking if the Target died:

            if (target == null) // null = dead
            {
                IsEnemyInRange = false;

                if (EnemysInRange.Count > 0)
                {
                    List<Transform> deadOpponents = new List<Transform>();

                    foreach (Transform enemy in EnemysInRange)
                    {
                        //Identify All DeadOpponents until an alive was found
                        if (enemy == null)
                        {
                            deadOpponents.Add(enemy);
                        }
                        else
                        {
                            target = enemy;
                            IsEnemyInRange = true;
                            break;
                        }
                    }

                    //Remove all DeadOpponents from the List
                    foreach (Transform deadOpponent in deadOpponents)
                    {
                        EnemysInRange.Remove(deadOpponent);
                    }
                }
            }
            return target;
        }
        set
        {
            if (value != null)
            {
                // If the Character has a Target, then add it to the EnemysInRange and calculate its distance to decide if the scriptholder "sees" it
                EnemysInRange.Add(value);
                SquareDistanceToEnemy = (transform.position - Target.position).sqrMagnitude;
                IsEnemyInRange = CalculateIfEnemyIsInRange();
            }
            else
            {
                EnemysInRange.Remove(value);
                IsEnemyInRange = false;
            }

            target = value;
        }
    }
    #endregion

    // AI Stuff
    [HideInInspector] public NavMeshAgent Agent;
    public List<Transform> EnemysInRange;

    #region States
    [Header("Idle State")]
    [SerializeField] public float MinIdleTime;
    [SerializeField] public float MaxIdleTime;
    [HideInInspector] public float IdleTime;

    [Header("Wander/Chase State")]
    // [SerializeField] public float SprintMultiplier = 1f; // not implemented yet
    [SerializeField] public float VisionSquareRange = 100f; // For the enemy detection, when he is hit
    [SerializeField] public float WalkpointRange;
    [HideInInspector] public float SquareDistanceToEnemy; // Will be set in the States(Chase and Attacks)
    [HideInInspector] public bool HasWalkPoint;
    protected Vector3 walkPoint;
    public virtual Vector3 WalkPoint
    {
        get { return walkPoint; }
        set 
        {
            walkPoint = value;
            WalkToWalkpoint();
        }
    }

    [Header("MeleeAttack State")]
    [SerializeField, Tooltip("not necessary needed! Just in case the user has a Weapon")] private GameObject meleeWeaponGameObject;
    //[SerializeField] private GameObject meleeAttackPrefab; // just needed for the MeleeAttack Hitbox Attack
    [HideInInspector] public float MeleeRange;
    private float meleeDamage;
    // public float MeleeAttackSpeed;
    #endregion

     private void Awake()
    {
        #region SettingReferenzes
        maxHealth = soCharacterStats.MaxHealth;
        currentHealth = maxHealth;

        meleeDamage = soCharacterStats.MeleeDamage;
        MeleeRange = soCharacterStats.MeleeRange;

        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        #endregion

    }

    // Could be merged in one CheckForInRange Funktion, with 2 parameters
    private bool CalculateIfEnemyIsInRange()
    {
        if (SquareDistanceToEnemy <= VisionSquareRange) return true;
        else return false;
    }
    public bool CheckForEnemyInMeleeRange()
    {
        if (SquareDistanceToEnemy <= MeleeRange) return true;
        else return false;
    }


    public void RunMeleeAttack()  // Method gets called per Animation! Gets Called in the GetCharacterAnimationMethods Script!
    {
        // MeleeAttack without a Hitbox
        // currently always hitting
        if (Target == null)
        {
            IsEnemyInRange = false;
            return;
        }
        IDamageable targetHealth = target.GetComponent<IDamageable>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(meleeDamage, this.transform);
        }
        else
        {
            Debug.Log("Target: " + Target + " cant be Damaged!");
        }
    }

    //NEW STUFF:

    #region ControllType
    // public event Action<Zombie, EZombieControllType> OnControlTypeChanged; //not needed rn
    private EZombieControllType controllType = EZombieControllType.None;
    public EZombieControllType ControllType
    {
        get { return controllType; }
        set
        { 
            //if (controllType == value) return; // idk if this is smart or bad

            controllType = value;
            // OnControlTypeChanged?.Invoke(this, controllType); //not needed rn
        }
    }
    #endregion


    public void WalkToWalkpoint()
    {
        if (ControllType != EZombieControllType.HordeDriven)
            return;



        Animator.SetBool("IsIdle", false);

        //Code from the WalkState(OnEnter):
        Agent.isStopped = false;
        Animator.SetBool("IsWalking", true);
        Agent.SetDestination(WalkPoint);

        //Stop if the Walkpoint is reached:
        StartCoroutine(CheckForWalkpointReached());
    }

    private IEnumerator CheckForWalkpointReached()
    {
        while (true)
        {
            if ((transform.position - WalkPoint).sqrMagnitude < 1f)
            {
                Debug.Log("Walkpoint Reached!");

                // Code from the WalkState(OnExit):
                Animator.SetBool("IsWalking", false);
                HasWalkPoint = false;

                Animator.SetBool("IsIdle", true);

                yield break; // Stops the Coroutine
            }

            yield return null;
        }
    }
}