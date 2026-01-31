using UnityEngine;
using UnityEngine.UI;

public class ComboTimerUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image timerFill;
    [SerializeField] private Text timerText;

    [Header("Config")]
    [SerializeField] private float comboResetTime = 0f;

    private float timer;
    private bool isRunning;

    void Update()
    {
        if (!isRunning) return;

        timer -= Time.deltaTime;

        UpdateUI();

        if (timer <= 0f)
        {
            StopTimer();
        }
    }

    public void Init(float time)
    {
        comboResetTime = time;
        gameObject.SetActive(false);
    }

    public void StartTimer()
    {
        timer = comboResetTime;
        isRunning = true;
        UpdateUI();
        gameObject.SetActive(true);
    }

    public void StopTimer()
    {
        isRunning = false;
        timer = 0f;
        UpdateUI();
        gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        float t = Mathf.Clamp01(timer / comboResetTime);

        if (timerFill != null)
        {
            timerFill.fillAmount = t;
            timerFill.color = t < 0.3f ? Color.red : Color.white;
        }

        if (timerText != null)
            timerText.text = timer > 0 ? timer.ToString("0.0") : "";
    }
}
