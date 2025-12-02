using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Card/CardData")]
public class CardData : ScriptableObject
{
    public CardId cardId;          
    public Sprite sprite;
    public AudioClip flipSound;
}
