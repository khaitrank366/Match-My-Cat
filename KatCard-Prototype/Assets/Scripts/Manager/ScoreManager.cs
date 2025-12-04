using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private int score = 0;
    private int comboMultiplier = 1;

    private void OnEnable()
    {
        GameplayManager.Instance.OnComboChanged += UpdateScore;
    }

    private void OnDisable()
    {
        GameplayManager.Instance.OnComboChanged -= UpdateScore;
    }

    private void UpdateScore(int comboCount)
    {
        comboMultiplier = Mathf.Clamp(1 + comboCount / 2, 1, 5); // ví dụ: 2 combo → x2, 10 combo → x5 max
        score += 10 * comboMultiplier;
        scoreText.text = $"Score: {score}";
    }
}
