using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private GameObject background;

    [Space(20)]
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject timeSelectPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject pausePanel;


    [Space(20)]
    [Header("Texts")]
    [SerializeField] private TMP_Text ingameScoreText;
    [SerializeField] private TMP_Text remainingTime;
    [SerializeField] private TMP_Text endScoreText;

    private GameManager gameManager;
    private bool gameHasStarted = false;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();

        background.SetActive(true);
        mainPanel.SetActive(true);
    }

    public void OpenTimePanel()
    {
        mainPanel.SetActive(false);
        timeSelectPanel.SetActive(true);
    }

    public void TimePanelBack()
    {
        timeSelectPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void SetTime(int time)
    {
        SetGameplayPanel(time * 60);
    }

    public void SetGameplayPanel(int time)
    {
        gameHasStarted = true;

        gameplayPanel.SetActive(true);
        timeSelectPanel.SetActive(false);
        background.SetActive(false);

        gameManager.StartGame(time);
    }

    public void SetTimerText(int time)
    {
        remainingTime.text = time.ToString();
    }

    public void SetScoreText(int score)
    {
        ingameScoreText.text = score.ToString();
    }

    public void EndGame(int score)
    {
        gameHasStarted = false;
        endPanel.SetActive(true);
        gameplayPanel.SetActive(false);
        endScoreText.text = score.ToString();
    }

    public void PlayAgain()
    {
        endPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        mainPanel.SetActive(true);
        background.SetActive(true);
        SoundManager.instance.PlayBGM();
    }

    public void PauseGame()
    {
        if (gameHasStarted)
        {
            pausePanel.SetActive(!pausePanel.activeSelf);
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
