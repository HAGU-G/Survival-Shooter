using System.Collections;
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


        if(IsGameOver)
        {
            //TODO 게임오버 연출
        }

    }

    public void GameOver()
    {
        //TODO 게임오버 연출
        IsGameOver = true;
        uiManager.gameover.enabled = true;
    }

    public IEnumerator CoRestart(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        IsGameOver = false;
        uiManager.gameover.enabled = false;
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
