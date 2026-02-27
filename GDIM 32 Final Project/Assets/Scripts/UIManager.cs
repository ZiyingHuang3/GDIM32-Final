using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private float startSeconds = 300f;
    [SerializeField] private Player player;

    public int hp = 3;
    private float timeLeft;
    private bool timerRunning;
    private bool gameStarted;
    private void Start()
    {
        if (startPanel != null) startPanel.SetActive(true);

        gameOverPanel.SetActive(false);

        SetHealth(hp);

        timeLeft = startSeconds;

        UpdateTimerText();

        timerRunning = false;

        gameStarted = false;

        if (player != null) player.enabled = false;

        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(RestartGame);

    }

    private void Update()
    {
        if (!timerRunning) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            timerRunning = false;
            UpdateTimerText();
            ShowGameOver(); // time up
            return;
        }

        UpdateTimerText();
    }

    public void SetHealth(int hp)
    {
        healthText.text = "HP: " + hp;
    }

    public void ShowGameOver()
    {
        timerRunning = false;
        gameOverPanel.SetActive(true);
    }

    public void StartGame()
    {
        gameStarted = true;
        timerRunning = true;
        if (startPanel != null) startPanel.SetActive(false);
        timeLeft = startSeconds;
        UpdateTimerText();
        gameOverPanel.SetActive(false);
        if (player != null) player.enabled = true;

    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
