using UnityEngine;

[CreateAssetMenu(menuName = "Game/Sound Profile")]
public class SoundProfileSO : ScriptableObject
{
    public AudioClip matchSfx;
    public AudioClip mismatchSfx;
    public AudioClip gameOverSfx;
    public AudioClip warning10sSfx;
}
