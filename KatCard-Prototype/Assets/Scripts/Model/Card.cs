using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Card : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Button button;
    public CardData dataSO;
    [SerializeField] Sprite hiddenSprite;
    public bool isSelected;

    [SerializeField] private float flipDuration = 0.5f;
    private bool isFlipping = false;

    public event Action<Card> OnCardClicked;

    [ContextMenu("Show Card")]
    private void Show()
    {
        if (!isFlipping && !isSelected)
            StartCoroutine(FlipCardCoroutine(show: true));
    }

    public void Reveal()
    {
        icon.sprite = dataSO.sprite;
    }

    [ContextMenu("Hide Card")]
    public void Hide()
    {
        if (!isFlipping && isSelected)
            StartCoroutine(FlipCardCoroutine(show: false));
    }

    public void ClickCard()
    {
        Show();
    }

    public void SetMatched()
    {
        isSelected = true;

        button.interactable = false;

        var iconColor = icon.color;
        iconColor.a = 0.5f;
        icon.color = iconColor;
    }
    public void EnableCard()
    {
        button.interactable = true;
    }
    public void DisableCard()
    {
        button.interactable = false;
    }

    private IEnumerator FlipCardCoroutine(bool show)
    {
        if (isFlipping) yield break;
        isFlipping = true;

        float halfDuration = flipDuration / 2f;
        float direction = show ? 1f : -1f;

        Quaternion startRotation = transform.rotation;
        Quaternion midRotation = startRotation * Quaternion.Euler(0, 90f * direction, 0);
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 180f * direction, 0);

        // 1️⃣ Quay tới giữa (90°)
        float t = 0f;
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float normalized = t / halfDuration;
            float tEase = normalized * normalized * (3f - 2f * normalized);
            transform.rotation = Quaternion.Slerp(startRotation, midRotation, tEase);

            // Lật sprite khi đi qua 90°
            if (transform.rotation.eulerAngles.y >= 90f && transform.rotation.eulerAngles.y <= 270f)
            {
                Vector3 s = icon.transform.localScale;
                s.x = -Mathf.Abs(s.x); // lật ngang
                icon.transform.localScale = s;
            }
            else
            {
                Vector3 s = icon.transform.localScale;
                s.x = Mathf.Abs(s.x); // trở lại bình thường
                icon.transform.localScale = s;
            }

            yield return null;
        }

        // 2️⃣ Đổi sprite khi card quay giữa
        icon.sprite = show ? dataSO.sprite : hiddenSprite;
        isSelected = show;

        // 3️⃣ Quay tiếp 90° để hoàn tất lật
        t = 0f;
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float normalized = t / halfDuration;
            float tEase = normalized * normalized * (3f - 2f * normalized);
            transform.rotation = Quaternion.Slerp(midRotation, endRotation, tEase);

            // Điều chỉnh lại scale để sprite hướng đúng
            if (transform.rotation.eulerAngles.y >= 90f && transform.rotation.eulerAngles.y <= 270f)
            {
                Vector3 s = icon.transform.localScale;
                s.x = -Mathf.Abs(s.x);
                icon.transform.localScale = s;
            }
            else
            {
                Vector3 s = icon.transform.localScale;
                s.x = Mathf.Abs(s.x);
                icon.transform.localScale = s;
            }

            yield return null;
        }

        transform.rotation = endRotation;
        isFlipping = false;

        if (show)
            OnCardClicked?.Invoke(this);
    }


}
