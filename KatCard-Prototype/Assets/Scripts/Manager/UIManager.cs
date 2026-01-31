using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] Canvas gameCanvas;
    [SerializeField] Canvas uiCanvas;

    private void Awake()
    {
    
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    private void Start()
    {
        gameCanvas.gameObject.SetActive(false);
        uiCanvas.gameObject.SetActive(true);
    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        gameCanvas.gameObject.SetActive(true);
        uiCanvas.gameObject.SetActive(false);
    }
}
