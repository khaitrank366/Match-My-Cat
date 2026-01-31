using System;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    public event Action OnMatch;
    public event Action OnMismatch;
    public event Action<int> OnComboChanged;

    [SerializeField] private Card firstCard;
    [SerializeField] private Card secondCard;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void CheckMatch()
    {
        bool match = firstCard.dataSO.cardId == secondCard.dataSO.cardId;

        if (match)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();
            firstCard = secondCard = null;
            OnMatch?.Invoke();
        }
        else
        {
            firstCard.SetUnmatched();
            secondCard.SetUnmatched();
            firstCard = secondCard = null;
            OnMismatch?.Invoke();
        }

    }

    public void NotifyComboChanged(int combo)
    {
        OnComboChanged?.Invoke(combo);
    }

    public void HandleCardFlipped(Card card)
    {
        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            CheckMatch();
        }
    }
}
