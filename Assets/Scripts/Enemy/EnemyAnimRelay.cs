using UnityEngine;

public class EnemyAnimRelay : MonoBehaviour
{
    [SerializeField] private EnemyCombat _enemyCombat;

    public void DealDamage()
    {
        _enemyCombat?.DealDamage();
    }
    public void AttackFinished()
    {
        _enemyCombat?.AttackFinished();
    }
}
