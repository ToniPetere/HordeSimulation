using Unity.Entities;
using UnityEngine;

public class MeleeAttackAuthoring : MonoBehaviour
{
    public float timerMax;
    public int damageAmount;
    public float attackRange;


    public class Baker : Baker<MeleeAttackAuthoring>
    {
        public override void Bake(MeleeAttackAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MeleeAttack
            {
                timerMax = authoring.timerMax,
                damageAmount = authoring.damageAmount,
                attackRange = authoring.attackRange,
            });
        }
    }
}

public struct MeleeAttack : IComponentData
{
    public int damageAmount;
    public float attackRange;

    public float timerMax;
    public float timer;
}