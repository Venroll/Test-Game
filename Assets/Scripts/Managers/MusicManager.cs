
using System.Collections;
using UnityEngine;

public class MusicManager : SingletonManager<MusicManager>
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource _musicSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameplayMusic;
    [SerializeField] private AudioClip _battleMusic;
    [SerializeField] private AudioClip _victoryMusic;
    [SerializeField] private AudioClip _defeatMusic;

    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float _musicVolume = 0.5f;
    [SerializeField] private float _fadeDuration = 1.5f;

    private Coroutine _fadeCoroutine;
    private AudioClip _currentClip;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this)
            return;

        if (_musicSource == null)
            _musicSource = GetComponent<AudioSource>();

        if (_musicSource != null)
        {
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;
            _musicSource.volume = _musicVolume;
        }
    }

    public void PlayMenuMusic()
    {
        PlayMusic(_menuMusic);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(_gameplayMusic);
    }

    public void PlayBattleMusic()
    {
        PlayMusic(_battleMusic);
    }

    public void PlayVictoryMusic()
    {
        PlayMusic(_victoryMusic, false);
    }

    public void PlayDefeatMusic()
    {
        PlayMusic(_defeatMusic, false);
    }

    private void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null || _musicSource == null)
            return;

        if (_currentClip == clip && _musicSource.isPlaying)
            return;

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(ChangeMusic(clip, loop));
    }

    private IEnumerator ChangeMusic(AudioClip newClip, bool loop)
    {
        yield return StartCoroutine(FadeVolume(0f));

        _musicSource.Stop();
        _musicSource.clip = newClip;
        _musicSource.loop = loop;
        _musicSource.Play();

        _currentClip = newClip;

        yield return StartCoroutine(FadeVolume(_musicVolume));

        _fadeCoroutine = null;
    }

    private IEnumerator FadeVolume(float targetVolume)
    {
        float startVolume = _musicSource.volume;
        float elapsedTime = 0f;

        if (_fadeDuration <= 0f)
        {
            _musicSource.volume = targetVolume;
            yield break;
        }

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            _musicSource.volume = Mathf.Lerp(
                startVolume,
                targetVolume,
                elapsedTime / _fadeDuration
            );

            yield return null;
        }

        _musicSource.volume = targetVolume;
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);

        if (_musicSource != null && _fadeCoroutine == null)
            _musicSource.volume = _musicVolume;
    }

    public void StopMusic()
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(StopMusicRoutine());
    }

    private IEnumerator StopMusicRoutine()
    {
        yield return StartCoroutine(FadeVolume(0f));

        _musicSource.Stop();
        _currentClip = null;
        _fadeCoroutine = null;
    }
}
