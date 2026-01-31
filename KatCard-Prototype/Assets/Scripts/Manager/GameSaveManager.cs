using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance { get; private set; }

    private const string KEY_SCORE = "SAVE_SCORE";
    private const string KEY_COMBO = "SAVE_COMBO";
    private const string KEY_ROWS = "SAVE_BOARD_ROWS";
    private const string KEY_COLS = "SAVE_BOARD_COLS";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveLayout(Vector2Int layout)
    {
        PlayerPrefs.SetInt(KEY_ROWS, layout.x);
        PlayerPrefs.SetInt(KEY_COLS, layout.y);
        PlayerPrefs.Save();

        Debug.Log($"[SAVE] Layout {layout.x}x{layout.y}");
    }

    public bool TryLoadLayout(out Vector2Int layout)
    {
        if (!PlayerPrefs.HasKey(KEY_ROWS) ||
            !PlayerPrefs.HasKey(KEY_COLS))
        {
            layout = Vector2Int.zero;
            return false;
        }

        layout = new Vector2Int(
            PlayerPrefs.GetInt(KEY_ROWS),
            PlayerPrefs.GetInt(KEY_COLS)
        );

        return true;
    }


    public void SaveHighestScore(int score)
    {
        PlayerPrefs.SetInt(KEY_SCORE, score);
        PlayerPrefs.Save();
        Debug.Log($"[SAVE] Score={score}");
    }

    public void SaveHighestCombo(int combo)
    {
        PlayerPrefs.SetInt(KEY_COMBO, combo);
        PlayerPrefs.Save();

        Debug.Log($"[SAVE] Combo={combo}");
    }

    public void LoadScore(out int score, out int combo)
    {
        score = PlayerPrefs.GetInt(KEY_SCORE, 0);
        combo = PlayerPrefs.GetInt(KEY_COMBO, 0);
    }

    public void ClearAll()
    {
        PlayerPrefs.DeleteKey(KEY_SCORE);
        PlayerPrefs.DeleteKey(KEY_COMBO);
        PlayerPrefs.DeleteKey(KEY_ROWS);
        PlayerPrefs.DeleteKey(KEY_COLS);
    }
}
