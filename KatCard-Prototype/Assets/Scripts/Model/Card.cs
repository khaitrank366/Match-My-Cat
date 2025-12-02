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

    public event Action<Card> OnCardClicked;

    [ContextMenu("Show Card")]
    private void Show()
    {
        icon.sprite = dataSO.sprite;
        isSelected = true;
    }

    [ContextMenu("Hide Card")]
    public void Hide()
    {
        icon.sprite = hiddenSprite;
        isSelected = false;
    }

    public void ClickCard()
    {
        Show();
        OnCardClicked.Invoke(this);
    }
}
