using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void PlayMatch()        => PlaySfx(soundProfile.matchSfx);
    public void PlayMismatch()     => PlaySfx(soundProfile.mismatchSfx);
    public void PlayGameOver()     => PlaySfx(soundProfile.gameOverSfx);
    public void PlayWarning10s()   => PlaySfx(soundProfile.warning10sSfx);

    private void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
