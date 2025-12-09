using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource flipSource;
    [SerializeField] private AudioSource sfxSource;

    public SoundProfileSO soundProfile;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        StartCoroutine(Register());
    }

    private void OnDisable()
    {
        if (GameplayManager.Instance == null) return;

        GameplayManager.Instance.OnMatch -= PlayMatchFromEvent;
        GameplayManager.Instance.OnMismatch -= PlayMismatchFromEvent;
        GameplayManager.Instance.OnComboChanged -= PlayCombo;
    }

    private IEnumerator Register()
    {
        while (GameplayManager.Instance == null)
            yield return null;

        GameplayManager.Instance.OnMatch += PlayMatchFromEvent;
        GameplayManager.Instance.OnMismatch += PlayMismatchFromEvent;
        GameplayManager.Instance.OnComboChanged += PlayCombo;
    }

    private void PlayMatchFromEvent() => PlayMatch();
    private void PlayMismatchFromEvent() => PlayMismatch();

    public void PlayFlip(AudioClip clip)
    {
        if (flipSource.isPlaying)
            flipSource.Stop();

        flipSource.PlayOneShot(clip);
    }

    public void PlayMatch() => PlaySfx(soundProfile.matchSfx);
    public void PlayMismatch() => PlaySfx(soundProfile.mismatchSfx);
    public void PlayGameOver() => PlaySfx(soundProfile.gameOverSfx);
    public void PlayWarning10s() => PlaySfx(soundProfile.warning10sSfx);

    private void PlaySfx(AudioClip clip)
    {
        if (flipSource.isPlaying)
            flipSource.Stop();
        sfxSource.PlayOneShot(clip);
    }

    public void PlayCombo(int comboCount)
    {
        if (comboCount < 1) return;

        AudioClip clip = null;

        if (comboCount == 1)
            clip = soundProfile.matchSfx;
        else if (comboCount - 2 < soundProfile.comboSounds.Length)
            clip = soundProfile.comboSounds[comboCount - 2];
        else if (soundProfile.maxComboRandom.Length > 0)
            clip = soundProfile.maxComboRandom[Random.Range(0, soundProfile.maxComboRandom.Length)];
        else
            clip = soundProfile.comboSounds[soundProfile.comboSounds.Length - 1];

        if (clip != null)
            PlaySfx(clip);
    }
}
