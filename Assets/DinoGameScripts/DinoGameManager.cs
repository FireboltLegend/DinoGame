using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DinoGameManager : MonoBehaviour
{
    public static DinoGameManager Instance { get; private set; }

    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public static float gameSpeed { get; set; }

    // public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    // public Button retryButton;
    // public Button gameMenuButton;

    //private DinoGamePlayer player;
    private DinoGameSpawner spawner;

    private static float score;

    public static void setScore(float newScore)
    {
        score = newScore;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        //player = FindObjectOfType<DinoGamePlayer>();
        spawner = FindObjectOfType<DinoGameSpawner>();
        
        NewGame();
    }

    public void NewGame()
    {
        DinoGameObstacle[] obstacles = FindObjectsOfType<DinoGameObstacle>();

        foreach(var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        score = 0f;
        gameSpeed = initialGameSpeed;
        enabled = true;

        //player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        // gameOverText.gameObject.SetActive(false);
        // retryButton.gameObject.SetActive(false);
        // gameMenuButton.gameObject.SetActive(false);

        UpdateHiscore();
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;

        //player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        // gameOverText.gameObject.SetActive(true);
        // retryButton.gameObject.SetActive(true);
        // gameMenuButton.gameObject.SetActive(true);

        UpdateHiscore();
    }

    private void Update()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("GameMenu");
        }
    }

    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if(score > hiscore)
        {
            hiscore = score;
           // PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }
}
