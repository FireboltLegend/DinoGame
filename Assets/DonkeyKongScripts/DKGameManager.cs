using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DKGameManager : MonoBehaviour
{
    private int lives;
    private int score;

    private void Start() {
        NewGame();
    }

    private void NewGame() {
        lives = 3;
        score = 0;

        //Load level...
    }

    public void LevelComplete() {
        score += 1000;

        // Load next level...
    }

    public void LevelFailed() {
        lives--;
        
        if(lives <= 0) {
            NewGame();
        } else {
            //Reload current level
        }
    }

}