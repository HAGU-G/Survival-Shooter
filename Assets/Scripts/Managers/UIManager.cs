using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static readonly string scoreFormat = $"SCORE : {0}";

    public TextMeshProUGUI score;
    public GameObject gameover;

    public GameObject pausePanel;

    public Slider musicVol;
    public Slider sfxVol;
    public Toggle soundOn;

    private void Start()
    {
        SetScoreText(0);
        pausePanel.SetActive(false);
        gameover.SetActive(false);
    }

    public void SetScoreText(int score)
    {
        this.score.text = string.Format(scoreFormat, score);
    }



    public void SoundToggle()
    {

    }


}
