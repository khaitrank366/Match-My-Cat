using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutSelectUI : MonoBehaviour
{
    public Dropdown layoutDropdown;
    public GameObject boardCanvas;
    public BoardManager boardManager;
    private List<Vector2Int> layouts = new();

    void Start()
    {
        GenerateLayouts();
        SetupDropdown();
    }

    private void GenerateLayouts()
    {
        layouts.Clear();

        int maxPairs = boardManager.allCards.Count;
        int maxCells = maxPairs * 2;

        int maxRowOrCol = Mathf.FloorToInt(Mathf.Sqrt(maxCells));

        for (int r = 2; r <= maxRowOrCol; r++)
        {
            for (int c = 2; c <= maxRowOrCol; c++)
            {
                int totalCells = r * c;

                if (totalCells % 2 != 0)
                    continue;

                if (totalCells > maxCells)
                    continue;

                layouts.Add(new Vector2Int(r, c));
            }
        }
    }

    void SetupDropdown()
    {
        layoutDropdown.ClearOptions();

        var options = new List<string>();
        foreach (var l in layouts)
            options.Add($"{l.x} x {l.y}");

        layoutDropdown.AddOptions(options);
    }

    public void OnConfirmClick()
    {
        int index = layoutDropdown.value;
        Vector2Int selected = layouts[index];

        boardManager.InitBoard(selected);

        UIManager.Instance.StartGame();
    }
}
