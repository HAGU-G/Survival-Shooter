using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private readonly string scoreFormat = $"SCORE : {0}";

    public TextMeshProUGUI score;
    public TextMeshProUGUI gameOver;
    public GameObject pausePanel;

    private void Start()
    {
        SetScoreText(0);
        pausePanel.SetActive(false);
        gameOver.enabled = false;

    }

    public void SetScoreText(int score)
    {
        this.score.text = string.Format(scoreFormat, score);
    }



    public void SoundToggle()
    {
        
    }


}
