using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoGameManager : MonoBehaviour
{
    public static DinoGameManager Instance { get; private set; }

    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }

    private DinoGamePlayer player;
    private DinoGameSpawner spawner;

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
        player = FindObjectOfType<DinoGamePlayer>();
        spawner = FindObjectOfType<DinoGameSpawner>();
        
        NewGame();
    }

    private void NewGame()
    {
        DinoGameObstacle[] obstacles = FindObjectsOfType<DinoGameObstacle>();

        foreach(var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        gameSpeed = initialGameSpeed;
        enabled = true;

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
    }

    private void Update()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
    }
}
