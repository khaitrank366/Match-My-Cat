using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private int score = 0;
    private int combo = 0;

    private float lastMatchTime = 0f;
    private float comboResetTime = 3f;

    private void OnEnable()
    {
        StartCoroutine(Register());
    }

    private IEnumerator Register()
    {
        while (GameplayManager.Instance == null)
            yield return null;

        GameplayManager.Instance.OnMatch += HandleMatch;
        GameplayManager.Instance.OnMismatch += HandleMismatch;
    }

    private void OnDisable()
    {
        GameplayManager.Instance.OnMatch -= HandleMatch;
        GameplayManager.Instance.OnMismatch -= HandleMismatch;
    }

    private void HandleMatch()
    {
        float now = Time.time;

        combo = (now - lastMatchTime <= comboResetTime) ? combo + 1 : 1;
        lastMatchTime = now;

        score += 10 * combo;
        scoreText.text = $"Score: {score}";

        GameplayManager.Instance.NotifyComboChanged(combo);
    }

    private void HandleMismatch()
    {
        combo = 0;
        GameplayManager.Instance.NotifyComboChanged(combo);
    }
}
