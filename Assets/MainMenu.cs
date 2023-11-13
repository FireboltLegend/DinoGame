using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDinoGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("DinoGame");
    }

    public void PlayFlappy()
    {
        SceneManager.LoadScene("FlappyBird");
    }
    public void PlayDK()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }
    public void PlayAsteroid()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Asteroids");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameMenu");
    }
}
