using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Wall Manager")]
    [SerializeField] private WallManager wallManager;
    [SerializeField] private GameObject rainEffect;
    [SerializeField] private GameObject snowEffect;

    [Space(20)]
    private UIManager uiManager;
    private int remainingTime = 0;
    private int score = 0;

    private void OnEnable()
    {
        Brick3D.OnScoredPoints += IncrementScore;
    }

    private void OnDisable()
    {
        Brick3D.OnScoredPoints -= IncrementScore;
    }
    private void Start()
    {
        uiManager = GetComponent<UIManager>();
    }

    public void StartGame(int time)
    {
        score = 0;
        uiManager.SetScoreText(score);
        remainingTime = time;
        wallManager.SpawnNewWall();
        SoundManager.instance.PlayGameplayMusic();
        StartCoroutine(GameTimerRoutine());
    }

    IEnumerator GameTimerRoutine()
    {
        while (remainingTime >= 0)
        {
            uiManager.SetTimerText(remainingTime);
            remainingTime--;
            yield return new WaitForSeconds(1);
        }

        wallManager.GameOver();
        SoundManager.instance.PlayGameOverMusic();
        uiManager.EndGame(score);
    }

    private void IncrementScore(int scoredPoints)
    {
        score += scoredPoints;
        uiManager.SetScoreText(score);
    }
}
