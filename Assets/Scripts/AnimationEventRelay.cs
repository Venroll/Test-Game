using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    [SerializeField] private WeaponAttack _weaponAttack;
    public void DealDamage()
    {
        _weaponAttack.DealDamage();
    }

    public void AttackFinished()
    {
        _weaponAttack.AttackFinished();
    }
}
