using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    [Header("Prefabs & Data")]
    public GameObject cardPrefab;
    public List<CardData> allCards;

    [Header("UI")]
    public GridLayoutGroup gridLayout;

    public int colTest;
    public int rowTest;
    // ======================================================
    // PUBLIC ENTRY POINT
    // ======================================================
    [ContextMenu("Generate Board")]
    public void GenerateBoardDefault()
    {
        GenerateBoard(rowTest, colTest);
    }

    public void GenerateBoard(int rows, int cols)
    {
        if (!IsLayoutValid(rows, cols)) return;

        List<CardData> boardCards = GenerateCardPairs(rows, cols);
        ShuffleCards(boardCards);
        SpawnCards(boardCards);
        UpdateGridLayout();
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

        // Tạo list index: [0,1,2,3,...]
        List<int> indices = new List<int>();
        for (int i = 0; i < allCards.Count; i++)
        {
            indices.Add(i);
        }

        // Shuffle index list
        for (int i = 0; i < indices.Count; i++)
        {
            int j = Random.Range(i, indices.Count);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }

        // Chọn pairCount index đầu tiên
        List<CardData> selectedCards = new List<CardData>(pairCount);
        for (int i = 0; i < pairCount; i++)
        {
            selectedCards.Add(allCards[indices[i]]);
        }

        // Nhân đôi
        List<CardData> boardCards = new List<CardData>(pairCount * 2);
        for (int i = 0; i < selectedCards.Count; i++)
        {
            boardCards.Add(selectedCards[i]);
            boardCards.Add(selectedCards[i]);
        }

        return boardCards;
    }


    // ======================================================
    // 3. Shuffle danh sách card
    // ======================================================
    private void ShuffleCards(List<CardData> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int j = Random.Range(i, cards.Count);
            var temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }

    // ======================================================
    // 4. Spawn card vào GridLayoutGroup
    // ======================================================
    private void SpawnCards(List<CardData> cards)
    {
        // Xóa board cũ
        foreach (Transform child in gridLayout.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var cardData in cards)
        {
            GameObject cardObj = Instantiate(cardPrefab, gridLayout.transform);
            Card card = cardObj.GetComponent<Card>();
            card.dataSO = cardData;
        }
    }

    // ======================================================
    // 5. Cập nhật GridLayoutGroup theo rows/cols
    // ======================================================
    private void UpdateGridLayout()
    {
        // Tính cell size tự động
        RectTransform rt = gridLayout.GetComponent<RectTransform>();
        float width = rt.rect.width - gridLayout.padding.left - gridLayout.padding.right - (colTest - 1) * gridLayout.spacing.x;
        float height = rt.rect.height - gridLayout.padding.top - gridLayout.padding.bottom - (rowTest - 1) * gridLayout.spacing.y;

        float cellSize = Mathf.Min(width / colTest, height / rowTest);
        gridLayout.cellSize = new Vector2(cellSize, cellSize);
    }
}
