using UnityEngine;

public interface IDamageable
{
    float CurrentHealth { get; set; }
    float MaxHealth {  get; set; }

    void TakeDamage(float _value, Transform _attacker);
    void Die();
}
