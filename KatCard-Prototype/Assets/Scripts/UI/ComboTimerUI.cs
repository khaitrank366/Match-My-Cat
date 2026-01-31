using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ComboTimerUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image timerFill;
    [SerializeField] private Text timerText;
    [SerializeField] private Text multiplierText;

    [Header("Animation")]
    [SerializeField] private float punchScale = 1.4f;
    [SerializeField] private float punchDuration = 0.2f;

    [SerializeField] private float comboResetTime = 0f;

    private float timer;
    private bool isRunning;
    private Coroutine scaleRoutine;
    private Vector3 originalScale;

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
        originalScale = multiplierText.transform.localScale;
        gameObject.SetActive(false);
    }

    public void StartTimer(int multiplier)
    {
        timer = comboResetTime;
        isRunning = true;

        UpdateUI();
        gameObject.SetActive(true);
        UpdateMultiplier(multiplier);
    }

    public void StopTimer()
    {
        isRunning = false;
        timer = 0f;
        multiplierText.text = "";
        UpdateUI();
        gameObject.SetActive(false);
    }
    private void UpdateMultiplier(int multiplier)
    {
        if (multiplierText == null) return;

        multiplierText.text = $"x{multiplier}";
        PlayScaleAnimation();
    }

    private void PlayScaleAnimation()
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(ScalePunch());
    }

    private IEnumerator ScalePunch()
    {
        Transform t = multiplierText.transform;
        t.localScale = originalScale * punchScale;

        float time = 0f;
        while (time < punchDuration)
        {
            time += Time.deltaTime;
            t.localScale = Vector3.Lerp(
                originalScale * punchScale,
                originalScale,
                time / punchDuration
            );
            yield return null;
        }

        t.localScale = originalScale;
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
