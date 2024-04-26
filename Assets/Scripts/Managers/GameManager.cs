using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
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

    public UIManager ui;

    public bool IsPaused { get; private set; }
    public bool IsGameOver { get; private set; }
    private int score;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            ui.score.text = $"score : {score}";
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
        StartCoroutine(CoRestart(5f));
        
    }

    public IEnumerator CoRestart(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        IsGameOver = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void PauseGame()
    {
        ui.pausePanel.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }
    public void ResumeGame()
    {
        ui.pausePanel.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }
}
