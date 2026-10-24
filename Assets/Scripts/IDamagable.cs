using UnityEngine;

public interface IDamagable
{
    float Health { get; }
    bool IsDead { get; set; }

    void TakeDamage(float damage, GameAgent sender);
}
