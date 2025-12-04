using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Image icon;
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

    private IEnumerator FlipCardCoroutine(bool show)
    {
        isFlipping = true;

        float halfDuration = flipDuration / 2f;
        Vector3 axis = Vector3.up;

        Quaternion startRotation = transform.rotation;;
        Quaternion endRotation;
        Debug.Log("startRotation: " + startRotation.eulerAngles);
        if (show)
        {
            // Show: bắt đầu rotation hiện tại, kết thúc +180° (lật lên)
            endRotation = startRotation * Quaternion.Euler(axis * 180f);
        }
        else
        {
            // Hide: bắt đầu rotation hiện tại, kết thúc -180° (lật ngược)
            endRotation = startRotation * Quaternion.Euler(axis * -180f);
        }
        Debug.Log("endRotation: " + endRotation.eulerAngles);

        // Midpoint = +90° hoặc -90° so với startRotation
        Quaternion midRotation = startRotation * Quaternion.Euler(axis * (show ? 90f : -90f));
        Debug.Log("midRotation: " + midRotation.eulerAngles);
        // 1️⃣ Quay tới 90° (mặt cũ biến mất)
        float t = 0f;
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float normalized = t / halfDuration;
            float tEase = normalized * normalized * (3f - 2f * normalized); // SmoothStep
            transform.rotation = Quaternion.Slerp(startRotation, midRotation, tEase);
            yield return null;
        }

        // 2️⃣ Đổi sprite khi thẻ quay giữa
        icon.sprite = show ? dataSO.sprite : hiddenSprite;
        isSelected = show;

        // 3️⃣ Quay tiếp 90° để hoàn tất lật
        t = 0f;
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float normalized = t / halfDuration;
            float tEase = normalized * normalized * (3f - 2f * normalized); // SmoothStep
            transform.rotation = Quaternion.Slerp(midRotation, endRotation, tEase);
            yield return null;
        }

        transform.rotation = endRotation;
        isFlipping = false;

        // 4️⃣ Event + âm thanh chỉ khi show
        if (show)
            OnCardClicked?.Invoke(this);
    }

}
