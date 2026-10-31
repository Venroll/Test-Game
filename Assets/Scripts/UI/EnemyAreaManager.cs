using UnityEngine;
using TMPro;

public class EnemyArenaManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _enemyCounterText;
    [SerializeField] private UIPauseMenu _uiPauseMenu;

    private int _totalEnemies;
    private int _remainingEnemies;

    private int _activeCombatEnemies;

    private bool _isVictory;

    private void Start()
    {
        EnemyHealth[] enemies =
            FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);

        _totalEnemies = enemies.Length;
        _remainingEnemies = _totalEnemies;

        foreach (EnemyHealth enemy in enemies)
        {
            if (enemy.IsDead)
            {
                _remainingEnemies--;
                continue;
            }

            enemy.OnDied += HandleEnemyDeath;
        }

        UpdateCounter();
    }

    public void EnemyEnteredCombat()
    {
        if (_isVictory)
            return;

        _activeCombatEnemies++;

        if (_activeCombatEnemies == 1)
            MusicManager.Instance.PlayBattleMusic();
    }

    public void EnemyLeftCombat()
    {
        if (_activeCombatEnemies > 0)
            _activeCombatEnemies--;

        if (_activeCombatEnemies == 0 && !_isVictory)
            MusicManager.Instance.PlayGameplayMusic();
    }

    private void HandleEnemyDeath()
    {
        if (_isVictory)
            return;

        _remainingEnemies--;
        UpdateCounter();

        if (_remainingEnemies <= 0)
        {
            _isVictory = true;

            MusicManager.Instance.PlayVictoryMusic();
            _uiPauseMenu.ActivateWinMenu();
        }
    }

    private void UpdateCounter()
    {
        if (_enemyCounterText == null)
            return;

        _enemyCounterText.text =
            $"Enemies\n{_remainingEnemies} / {_totalEnemies}";
    }
}
