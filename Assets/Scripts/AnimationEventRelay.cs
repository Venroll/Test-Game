using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    [SerializeField] private UIPauseMenu _pauseMenu;
    [SerializeField] private WeaponAttack _weaponAttack;

    public void ActivateFailMenu()
    {
        _pauseMenu.ActivateFailMenu();
    }
    public void DealDamage()
    {
        _weaponAttack.DealDamage();
    }

    public void AttackFinished()
    {
        _weaponAttack.AttackFinished();
    }
}
