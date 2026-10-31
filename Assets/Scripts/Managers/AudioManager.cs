using UnityEngine;

public class AudioManager : SingletonManager<AudioManager>
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource _sfxSource;

    [Header("Player Sounds")]
    [SerializeField] private AudioClip _playerHitSound;
    [SerializeField] private AudioClip _playerDeathSound;

    [Header("Enemy Sounds")]
    [SerializeField] private AudioClip _enemyHitSound;
    [SerializeField] private AudioClip _enemyDeathSound;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this)
            return;

        if (_sfxSource == null)
            _sfxSource = GetComponent<AudioSource>();
    }

    public void PlayPlayerHit()
    {
        PlaySFX(_playerHitSound);
    }
    public void PlayDeathHit()
    {
        PlaySFX(_playerDeathSound);
    }

    public void PlayEnemyHit(Vector3 position)
    {
        PlaySFXAtPosition(_enemyHitSound, position);
    }

    public void PlayEnemyDeath(Vector3 position)
    {
        PlaySFXAtPosition(_enemyDeathSound, position);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null || _sfxSource == null)
            return;

        _sfxSource.PlayOneShot(clip);
    }

    private void PlaySFXAtPosition(AudioClip clip, Vector3 position)
    {
        if (clip == null)
            return;

        AudioSource.PlayClipAtPoint(clip, position);
    }
}
