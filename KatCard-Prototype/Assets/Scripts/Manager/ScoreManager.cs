using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private ComboTimerUI comboTimerUI;
    [SerializeField] private Text highestScoreText;
    [SerializeField] private Text highestComboText;

    private int score = 0;
    private int combo = 0;

    private int highestScore = 0;
    private int highestCombo = 0;

    private float lastMatchTime = 0f;
    private float comboResetTime = 3f;

    private void OnEnable()
    {
        StartCoroutine(Register());
    }

    private IEnumerator Register()
    {
        InitScore();

        while (GameplayManager.Instance == null)
            yield return null;

        GameplayManager.Instance.OnMatch += HandleMatch;
        GameplayManager.Instance.OnMismatch += HandleMismatch;
        GameplayManager.Instance.OnGameCompleted += HandleGameCompleted;
    }

    private void InitScore()
    {
        comboTimerUI.Init(comboResetTime);

        GameSaveManager.Instance.LoadScore(out highestScore, out highestCombo);
        highestScoreText.text = $"Highest Score: {highestScore}";
        highestComboText.text = $"Highest Combo: {highestCombo}";
    }

    private void OnDisable()
    {
        GameplayManager.Instance.OnMatch -= HandleMatch;
        GameplayManager.Instance.OnMismatch -= HandleMismatch;
        GameplayManager.Instance.OnGameCompleted -= HandleGameCompleted;
    }

    private void HandleMatch()
    {
        float now = Time.time;

        combo = (now - lastMatchTime <= comboResetTime) ? combo + 1 : 1;
        lastMatchTime = now;

        score += 10 * combo;

        HandleScore();
        comboTimerUI.StartTimer(combo);
        GameplayManager.Instance.NotifyComboChanged(combo);
    }

    private void HandleScore()
    {
        scoreText.text = $"Score: {score}";
        if (score > highestScore)
        {
            highestScore = score;
            highestScoreText.text = $"Highest Score: {highestScore}";
            GameSaveManager.Instance.SaveHighestScore(highestScore);
        }
        if (combo > highestCombo)
        {
            highestCombo = combo;
            highestComboText.text = $"Highest Combo: {highestCombo}";
            GameSaveManager.Instance.SaveHighestCombo(highestCombo);
        }
    }

    private void HandleMismatch()
    {
        combo = 0;
        comboTimerUI.StopTimer();
        GameplayManager.Instance.NotifyComboChanged(combo);
    }

    private void HandleGameCompleted()
    {
        Debug.Log("🎉 Player finished the game!");

        comboTimerUI.StopTimer();
    }
}
