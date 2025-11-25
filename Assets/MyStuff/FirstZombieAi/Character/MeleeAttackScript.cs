using UnityEngine;

public class MeleeAttackScript : MonoBehaviour
{
    private string targetTag;
    private float damage;
    private Transform attacker;

    public string TargetTag { set { targetTag = value; } }
    public float Damage { set { damage = value; } }
    public Transform Attacker { set {  attacker = value; } }

    private void OnTriggerEnter(Collider collision)
    {
        // if there was no target or not the wanted target, than the meleeAttack just gets destroyed
        if (collision == null || !collision.transform.CompareTag(targetTag))
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            // if the wanted Target was in the Trigger, than deal Damage and destroy the meleeAttack
            IDamageable targetHealth = collision.GetComponent<IDamageable>();
            targetHealth.TakeDamage(damage, attacker);

            // Debug.Log($"Dealt {damage} damage with MeleeAttack!");

            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        // Attack should be destroy after a short period of time
        Invoke(nameof(KillThis), 0.1f);
    }
    private void KillThis()
    {
        Destroy(this.gameObject);
    }

}
