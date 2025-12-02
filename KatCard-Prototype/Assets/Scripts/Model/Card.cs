using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] CardData dataSO;
    [SerializeField] Sprite hiddenSprite;
    public bool isSelected;

    [ContextMenu("Show Card")]
    public void Show()
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

    public void OnClickCard()
    {
        SoundManager.Instance.PlayFlip(dataSO.flipSound);
        Show();
    }
}
