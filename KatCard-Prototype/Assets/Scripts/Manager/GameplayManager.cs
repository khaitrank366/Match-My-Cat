using System;
using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    // Events
    public event Action<Card, Card> OnMatch;
    public event Action<Card, Card> OnMismatch;
    public event Action<int> OnComboChanged;

    [SerializeField] private Card firstCard;
    [SerializeField] private Card secondCard;

    [SerializeField] private int combo = 0;
    [SerializeField] private float comboResetTime = 3f;
    [SerializeField] private float lastMatchTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void HandleCardFlipped(Card card)
    {
        if (firstCard == null)
        {
            firstCard = card;
            // Debug.Log("First card selected: " + card.dataSO.cardId);
        }
        else if (secondCard == null)
        {
            secondCard = card;
            // Debug.Log("Second card selected: " + card.dataSO.cardId);
            CheckMatch();
        }
    }

    private void CheckMatch()
    {
        if (firstCard.dataSO.cardId == secondCard.dataSO.cardId)
        {
            // Update combo
            //Debug.Log($"Cards Matched! Combo: {combo}");
            float now = Time.time;
            combo = (now - lastMatchTime <= comboResetTime) ? combo + 1 : 1;
            lastMatchTime = now;

            firstCard.SetMatched();
            secondCard.SetMatched();

            OnMatch?.Invoke(firstCard, secondCard);
            OnComboChanged?.Invoke(combo);

            firstCard = secondCard = null;
        }
        else
        {
            //Debug.Log("Cards Mismatched!");
            combo = 0;
            OnComboChanged?.Invoke(combo);
            OnMismatch?.Invoke(firstCard, secondCard);

            firstCard.SetUnmatched();
            secondCard.SetUnmatched();

            firstCard = secondCard = null;
        }

    }

    // Optional: reset combo manually
    public void ResetCombo()
    {
        combo = 0;
        OnComboChanged?.Invoke(combo);
    }
}
