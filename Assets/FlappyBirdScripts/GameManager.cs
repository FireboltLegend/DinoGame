using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public FlappyBirdPlayer player;
    public FlappyBirdAgent agent;
    public Text scoreText;
    public GameObject playButton;
    public GameObject mainMenuButton;
    public GameObject gameOver;
    public GameObject scoreUI;
    private int score;

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
        scoreUI.SetActive(true);

        Time.timeScale = 1f;
        agent.enabled = true;
        //player.enabled = true;

        FlappyBirdPipes[] pipes = FindObjectsOfType<FlappyBirdPipes>();

        for (int i = 0; i < pipes.Length; i++) {
            Destroy(pipes[i].gameObject);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        agent.enabled = false;
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        playButton.SetActive(true);
        mainMenuButton.SetActive(true);
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
