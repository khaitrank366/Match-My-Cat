using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image background;
    [SerializeField] Button button;
    public CardData dataSO;
    [SerializeField] Sprite hiddenSprite;
    [SerializeField] private float flipDuration = 0.5f;
    private bool isFlipping = false;

    public event Action<Card> OnCardClicked;

    public void ClickCard()
    {
        // Debug.Log("Card clicked: " + dataSO.cardId);
        if (!isFlipping)
            StartCoroutine(Flip(show: true));
    }

    public void Hide()
    {
        StartCoroutine(Flip(show: false));
    }
    public void Reveal() { icon.sprite = dataSO.sprite; }

    public void ResetVisual()
    {
        isFlipping = false;

        background.color = Color.white;
        icon.sprite = hiddenSprite;

        icon.transform.localRotation = Quaternion.identity;
        icon.transform.localPosition = Vector3.zero;
        icon.transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        button.interactable = true;
    }

    public void SetMatched()
    {
        button.interactable = false;
        background.color = new Color(0f, 1f, 0f, 0.5f);
    }

    public void SetUnmatched()
    {
        StartCoroutine(DoUnmatch());
    }

    private IEnumerator DoUnmatch()
    {
        background.color = Color.red;
        button.interactable = false;

        yield return new WaitForSeconds(0.5f);

        background.color = Color.white;
        button.interactable = true;
        Hide();

    }

    private IEnumerator Flip(bool show)
    {
        if (isFlipping)
            yield break;

        isFlipping = true;
        float halfDuration = flipDuration / 2f;
        float direction = show ? 1f : -1f;

        Quaternion startRotation = transform.rotation;
        Quaternion midRotation = startRotation * Quaternion.Euler(0, 90f * direction, 0);
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 180f * direction, 0);

        float t = 0f; while (t < halfDuration)
        {
            t += Time.deltaTime;
            float normalized = t / halfDuration;
            float tEase = normalized * normalized * (3f - 2f * normalized);
            transform.rotation = Quaternion.Slerp(startRotation, midRotation, tEase);

            if (transform.rotation.eulerAngles.y >= 90f && transform.rotation.eulerAngles.y <= 270f)
            {
                Vector3 s = icon.transform.localScale; s.x = -Mathf.Abs(s.x); // lật ngang 
                icon.transform.localScale = s;
            }
            else
            {
                Vector3 s = icon.transform.localScale; s.x = Mathf.Abs(s.x); // trở lại bình thường 
                icon.transform.localScale = s;
            }
            yield return null;
        }

        icon.sprite = show ? dataSO.sprite : hiddenSprite;

        t = 0f;
        while (t < halfDuration)
        {
            t += Time.deltaTime; float normalized = t / halfDuration;
            float tEase = normalized * normalized * (3f - 2f * normalized);
            transform.rotation = Quaternion.Slerp(midRotation, endRotation, tEase);

            if (transform.rotation.eulerAngles.y >= 90f && transform.rotation.eulerAngles.y <= 270f)
            {
                Vector3 s = icon.transform.localScale;
                s.x = -Mathf.Abs(s.x); icon.transform.localScale = s;
            }
            else
            {
                Vector3 s = icon.transform.localScale;
                s.x = Mathf.Abs(s.x); icon.transform.localScale = s;
            }
            yield return null;
        }

        transform.rotation = endRotation;
        isFlipping = false;

        if (show)
            OnCardClicked?.Invoke(this);
    }
}
