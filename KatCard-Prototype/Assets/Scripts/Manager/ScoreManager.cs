using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private int score = 0;

    private void OnEnable()
    {
        StartCoroutine(WaitAndRegister());
    }

    private IEnumerator WaitAndRegister()
    {
        while (GameplayManager.Instance == null)
            yield return null;

        GameplayManager.Instance.OnComboChanged += AddScore;
    }

    private void OnDisable()
    {
        GameplayManager.Instance.OnComboChanged -= AddScore;
    }

    private void AddScore(int combo)
    {
        if (combo == 0) return;

        score += 10;
        scoreText.text = $"Score: {score}";
    }
}
