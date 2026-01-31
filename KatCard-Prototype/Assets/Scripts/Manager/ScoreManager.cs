using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private ComboTimerUI comboTimerUI;
    [SerializeField] private Text highestScoreText;
    [SerializeField] private Text highestComboText;

    [SerializeField] private Text highestScoreTextEndGame;
    [SerializeField] private Text highestComboTextEndGame;

    [SerializeField] private Text yourScoreText;
    [SerializeField] private Text yourComboText;

    [SerializeField] private CanvasGroup inGameCanvasGroup;
    [SerializeField] private CanvasGroup endGameCanvasGroup;

    private int score = 0;
    private int combo = 0;

    private int currentHighestcombo = 0;

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
        SetCanvasGroup(inGameCanvasGroup, true);
        SetCanvasGroup(endGameCanvasGroup, false);

        InitScore();

        while (GameplayManager.Instance == null)
            yield return null;

        GameplayManager.Instance.OnMatch += HandleMatch;
        GameplayManager.Instance.OnMismatch += HandleMismatch;
        GameplayManager.Instance.OnGameCompleted += HandleGameCompleted;
    }

    private void InitScore()
    {
        score = 0;
        combo = 0;
        currentHighestcombo = 0;

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

        if (combo > currentHighestcombo)
            currentHighestcombo = combo;

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
        StartCoroutine(ShowFinalScoresAfterDelay(0.5f));
    }

    IEnumerator ShowFinalScoresAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        scoreText.text = "Score: 0";
        highestComboText.text = "";
        highestScoreText.text = "";
        SetCanvasGroup(inGameCanvasGroup, false);
        highestScoreTextEndGame.text = $"Highest Score: {highestScore}";
        highestComboTextEndGame.text = $"Highest Combo: {highestCombo}";
        yourScoreText.text = $"Your Score: {score}";
        yourComboText.text = $"Your Highest Combo: {currentHighestcombo}";
        SetCanvasGroup(endGameCanvasGroup, true);
    }

    public void Restart()
    {
        SetCanvasGroup(inGameCanvasGroup, true);
        SetCanvasGroup(endGameCanvasGroup, false);

        InitScore();
    }

    void SetCanvasGroup(CanvasGroup cg, bool enabled)
    {
        cg.alpha = enabled ? 1f : 0f;
        cg.interactable = enabled;
        cg.blocksRaycasts = enabled;
    }
}
