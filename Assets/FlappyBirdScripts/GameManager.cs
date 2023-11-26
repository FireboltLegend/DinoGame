using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public FlappyBirdPlayer player;
    public FlappyBirdAgent agent;
    public Text scoreText;
    public Text highScoreText;
    public GameObject playButton;
    public GameObject mainMenuButton;
    public GameObject gameOver;
    public GameObject scoreUI;
    public GameObject highScoreUI;
    private int score;
    private static int highScore;
    public FlappyBirdOutput script;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        playButton.SetActive(true);
        mainMenuButton.SetActive(true);
        Pause();
    }

    public void Play()
    {
        //disable gamemanager object if training
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        mainMenuButton.SetActive(false);
        gameOver.SetActive(false);
        highScoreUI.SetActive(false);
        scoreUI.SetActive(true);

        Time.timeScale = 1f;
        agent.enabled = true;
        //player.enabled = true;

        FlappyBirdPipes[] pipes = FindObjectsOfType<FlappyBirdPipes>();

        for (int i = 0; i < pipes.Length; i++) {
            Destroy(pipes[i].gameObject);
        }

        script.OutputFile("fPlay");
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        agent.enabled = false;
    }

    public void GameOver()
    {
        if (score > highScore) {
            highScore = score;
            highScoreText.text = "New High Score: " + highScore.ToString();
            script.gotHighScore = true;
        }
        else {
            highScoreText.text = "High Score: " + highScore.ToString();
            script.gotHighScore = false;
        }
        gameOver.SetActive(true);
        playButton.SetActive(true);
        highScoreUI.SetActive(true);
        mainMenuButton.SetActive(true);
        script.OutputFile("fDied");
        Pause();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("GameMenu");
        }
    }
}
