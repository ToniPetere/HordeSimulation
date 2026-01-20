using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Singleton Strukture:
    public static PlayerHealth Instance
    {
        get { return instance; }
        private set { instance = value; }
    }
    private static PlayerHealth instance;
    private void Awake()
    {
        instance = this;
    }

    // Variables:
    [SerializeField] private int maxHealth;
    private int currentHealth;

    private void Start()
    {
        if(maxHealth == 0)
        {
            Debug.LogWarning("Player spawned with 0 Health!");
        }
        currentHealth = maxHealth;
    }

    public void TakeDamage(int _amount)
    {
        currentHealth -= _amount;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            // Player dead:
            Debug.Log("Player died!");
        }

        Debug.Log("Player Health: " +  currentHealth);
    }
}
