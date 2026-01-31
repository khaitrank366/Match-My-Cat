using System;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    public event Action OnMatch;
    public event Action OnMismatch;
    public event Action<int> OnComboChanged;

    [SerializeField] private CardUI firstCard;
    [SerializeField] private CardUI secondCard;

    public event Action OnGameCompleted;

    private int totalPairs;
    private int matchedPairs;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void InitGame(int pairCount)
    {
        totalPairs = pairCount;
        matchedPairs = 0;

        firstCard = null;
        secondCard = null;

        Debug.Log($"[GAME] Init with {totalPairs} pairs");
    }

    private void CheckMatch()
    {
        bool match = firstCard.dataSO.cardId == secondCard.dataSO.cardId;

        if (match)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();
            matchedPairs++;
            OnMatch?.Invoke();

            firstCard = secondCard = null;

            Debug.Log($"[MATCH] {matchedPairs}/{totalPairs}");
            if (matchedPairs >= totalPairs)
            {
                Debug.Log("[GAME] Completed!");
                OnGameCompleted?.Invoke();
            }
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

    public void HandleCardFlipped(CardUI card)
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
