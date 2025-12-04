using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    [Header("Prefabs & Data")]
    public CardPool cardPool;
    public List<CardData> allCards;

    [Header("UI")]
    public GridLayoutGroup gridLayout;

    [Header("Board Size")]
    public int rowTest = 2;
    public int colTest = 5;
    [SerializeField] private List<Card> activeCards = new List<Card>();


    [ContextMenu("Generate Board")]
    public void GenerateBoardDefault()
    {
        GenerateBoard(rowTest, colTest);
    }

    [ContextMenu("Reveal Board")]
    public void ReVealBoard()
    {
        foreach (var card in activeCards)
        {
            card.Reveal();
        }
    }
    public void GenerateBoard(int rows, int cols)
    {
        if (!IsLayoutValid(rows, cols)) return;

        List<CardData> boardCards = GenerateCardPairs(rows, cols);
        ShuffleCards(boardCards);
        SpawnCards(boardCards);
        UpdateGridLayout(rows, cols);
    }

    // ======================================================
    // 1. Kiểm tra layout hợp lệ
    // ======================================================
    private bool IsLayoutValid(int rows, int cols)
    {
        int totalCells = rows * cols;
        if (totalCells % 2 != 0)
        {
            Debug.LogError("Tổng số thẻ phải chẵn!");
            return false;
        }

        if (totalCells / 2 > allCards.Count)
        {
            Debug.LogError("Danh sách card không đủ để tạo board!");
            return false;
        }

        return true;
    }

    // ======================================================
    // 2. Chọn cặp card ngẫu nhiên
    // ======================================================
    private List<CardData> GenerateCardPairs(int rows, int cols)
    {
        int pairCount = (rows * cols) / 2;

        List<int> indices = new List<int>();
        for (int i = 0; i < allCards.Count; i++) indices.Add(i);

        // Shuffle index
        for (int i = 0; i < indices.Count; i++)
        {
            int j = Random.Range(i, indices.Count);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }

        // Chọn pairCount card
        List<CardData> selectedCards = new List<CardData>();
        for (int i = 0; i < pairCount; i++)
            selectedCards.Add(allCards[indices[i]]);

        // Nhân đôi
        List<CardData> boardCards = new List<CardData>();
        foreach (var card in selectedCards)
        {
            boardCards.Add(card);
            boardCards.Add(card);
        }

        return boardCards;
    }

    // ======================================================
    // 3. Shuffle cards
    // ======================================================
    private void ShuffleCards(List<CardData> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int j = Random.Range(i, cards.Count);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }
    }

    // ======================================================
    // 4. Spawn card vào GridLayoutGroup
    // ======================================================
    private void SpawnCards(List<CardData> cards)
    {
        DespawnCards();

        foreach (var cardData in cards)
        {
            GameObject cardObj = cardPool.Get();
            cardObj.transform.SetParent(gridLayout.transform, false);
            cardObj.SetActive(true);

            Card card = cardObj.GetComponent<Card>();
            card.dataSO = cardData;
            card.OnCardClicked += CardClickEvent;
            card.OnCardClicked += GameplayManager.Instance.HandleCardFlipped;

            activeCards.Add(card);

        }
    }

    private void DespawnCards()
    {
        foreach (var card in activeCards)
        {
            card.OnCardClicked -= CardClickEvent;
            card.Hide();
            cardPool.Release(card.gameObject);
        }
        activeCards.Clear();
    }

    private void CardClickEvent(Card card)
    {
        SoundManager.Instance.PlayFlip(card.dataSO.flipSound);
    }

    // ======================================================
    // 5. Cập nhật GridLayoutGroup theo rows/cols nhập
    // ======================================================
    private void UpdateGridLayout(int rows, int cols)
    {
        // RectTransform rt = gridLayout.GetComponent<RectTransform>();

        // // Tính width & height khả dụng
        // float width = rt.rect.width - gridLayout.padding.left - gridLayout.padding.right - gridLayout.spacing.x * (cols - 1);
        // float height = rt.rect.height - gridLayout.padding.top - gridLayout.padding.bottom - gridLayout.spacing.y * (rows - 1);

        // // Chọn cellSize vuông
        // float cellSize = Mathf.Min(width / cols, height / rows);
        // gridLayout.cellSize = new Vector2(cellSize, cellSize);

        // // Căn giữa board nếu dư khoảng trống
        // float boardWidth = cellSize * cols + gridLayout.spacing.x * (cols - 1);
        // float boardHeight = cellSize * rows + gridLayout.spacing.y * (rows - 1);

        // gridLayout.padding.left = Mathf.RoundToInt((rt.rect.width - boardWidth) / 2f);
        // gridLayout.padding.right = gridLayout.padding.left;
        // gridLayout.padding.top = Mathf.RoundToInt((rt.rect.height - boardHeight) / 2f);
        // gridLayout.padding.bottom = gridLayout.padding.top;
    }
}
