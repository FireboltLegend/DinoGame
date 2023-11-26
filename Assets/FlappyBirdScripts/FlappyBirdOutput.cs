using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FlappyBirdOutput : MonoBehaviour
{
    private string message;
    public string filePath;
    public bool gotHighScore = false;

    void Start()
    {
        filePath += "/output.txt";

        if (File.Exists(filePath)) {
            File.Delete(filePath);
        }
    }

    public void OutputFile(string gameStatus)
    {
        if (File.Exists(filePath)) {
            File.Delete(filePath);
        }

        if (gameStatus == "fPlay") {
            message = "Playing Flappy Bird";
        }
        else if (gameStatus == "fDied") {
            message = "Flappy Bird Death";
        }

        File.WriteAllText(filePath, message);
        if (gotHighScore) {
            File.AppendAllText(filePath, " and Highscore");
        }
    }
}
