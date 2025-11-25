using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour, IDamageable
{
    // Setup:
    #region HealthSystem
    [SerializeField] private UI_Bar healthbar;
    [SerializeField] private float currentHealth;
    private float maxHealth;
    public float CurrentHealth { get{ return currentHealth; } set { currentHealth = value; } }
    public float MaxHealth { get { return maxHealth; } set {  maxHealth = value; } }

    public void TakeDamage(float _value, Transform _attacker)
    {
        currentHealth -= _value;
        CheckForDeath();

        if(Target == null) // if the defending Character hasnt already a Target, then the attacker will be its Target
            Target = _attacker;


        if(healthbar != null)
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
        if (transform.CompareTag("Player"))
        {
            Debug.Log("Player took damage!");
        } 
        else
        {
            Destroy(this.gameObject);
        }
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

    public bool IsEnemyInRange {  get { return isEnemyInRange; } set { isEnemyInRange = value; } }
    public bool IsDefensive 
    {
        get { return isDefensive; } 
        set 
        {
            if (value == false && EnemysInRange.Count > 0)
            {
                // baiscally run the code that was previously blocked by this bool, incase a enemy is in its range already
                IsEnemyInRange = true;
                Target = EnemysInRange[0];
            }

            isDefensive = value; 
        } 
    }
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
            if(value != null)
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

    public GameObject SwordGameObject { get { return meleeWeaponGameObject; } }
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
    [HideInInspector] public float SquareDistanceToEnemy; // Will be set in the States(Chase and Attacks)
    [HideInInspector] public bool HasWalkPoint;
    [HideInInspector] public Vector3 WalkPoint;

    [Header("MeleeAttack State")]
    [SerializeField, Tooltip("not necessary needed! Just in case the user has a Weapon")] private GameObject meleeWeaponGameObject;
    //[SerializeField] private GameObject meleeAttackPrefab; // just needed for the MeleeAttack Hitbox Attack
    [HideInInspector] public float MeleeRange;
    private float meleeDamage;
    // public float MeleeAttackSpeed;
    #endregion
    private void Awake()
    {
        maxHealth = soCharacterStats.MaxHealth;
        currentHealth = maxHealth;

        meleeDamage = soCharacterStats.MeleeDamage;
        MeleeRange = soCharacterStats.MeleeRange;

        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
    }

    // Could be merged in one CheckForInRange Funktion, with 2 parameters
    private bool CalculateIfEnemyIsInRange()
    {
        if(SquareDistanceToEnemy <= VisionSquareRange) return true;
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
        if(targetHealth != null)
        {
            targetHealth.TakeDamage(meleeDamage, this.transform);
        }
        else
        {
            Debug.Log("Target: " + Target + " cant be Damaged!");
        }
    }
    //private void Update()
    //{
    //   // Debug.Log(this.gameObject.name + " MeleeDamage is: " + meleeDamage);
    //}

    /*
    public void SpawnMeleeAttack() // Method gets called per Animation! Gets Called in the GetCharacterAnimationMethods Script!
    {
        // MeleeAttack with a Hitbox

        if(Target == null)
        {
            IsEnemyInRange = false;
            return;
        }

        GameObject meleeAttack =  Instantiate(meleeAttackPrefab, this.transform.position + (Target.position - this.transform.position), this.transform.rotation);
        MeleeAttackScript meleeAttackScript = meleeAttack.GetComponent<MeleeAttackScript>();

        meleeAttackScript.TargetTag = enemyTag;
        meleeAttackScript.Damage = meleeDamage;
        meleeAttackScript.Attacker = this.transform;
    }
    */
}
