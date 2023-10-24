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
        SceneManager.LoadScene("DinoGame");
    }

    public void PlayFlappy()
    {
        SceneManager.LoadScene("FlappyBird");
    }
    public void PlayDK()
    {
        SceneManager.LoadScene("Scene1");
    }
    public void PlayAsteroid()
    {
        SceneManager.LoadScene("Asteroids");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }
}
