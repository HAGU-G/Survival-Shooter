using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public UIManager uiManager;
    public AudioManager audioManager;

    public bool IsPaused { get; private set; }
    public bool IsGameOver { get; private set; }
    private int score;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            uiManager.score.text = $"score : {score}";
        }
    }

    private void Start()
    {
        if (Instance != this)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPaused)
                PauseGame();
            else
                ResumeGame();
        }

    }

    public void Gameover()
    {
        //TODO 게임오버 연출
        IsGameOver = true;
        uiManager.gameover.SetActive(true);
    }

    public void Restart()
    {
        IsGameOver = false;
        uiManager.gameover.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void PauseGame()
    {
        uiManager.pausePanel.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }
    public void ResumeGame()
    {
        uiManager.pausePanel.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }
}
