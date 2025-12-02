using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource flipSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Sound Profile (SO)")]
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
        sfxSource.PlayOneShot(clip);
    }
    public int CountdownEvent = 0;
    [ContextMenu("Play Combo Test")]
    public void PlayCombo()
    {
        int comboCount = CountdownEvent;
        AudioClip clip = null;

        if (comboCount == 1)
        {
            clip = soundProfile.matchSfx;
        }
        else if (comboCount - 2 < soundProfile.comboSounds.Length)
        {
            clip = soundProfile.comboSounds[comboCount - 2];
        }
        else if (soundProfile.maxComboRandom.Length > 0)
        {
            int randIndex = Random.Range(0, soundProfile.maxComboRandom.Length);
            clip = soundProfile.maxComboRandom[randIndex];
        }
        else
        {
            // fallback
            clip = soundProfile.comboSounds[soundProfile.comboSounds.Length - 1];
        }

        if (clip != null)
            PlaySfx(clip);

        Debug.Log("Current clip: " + clip.name);
    }

}
