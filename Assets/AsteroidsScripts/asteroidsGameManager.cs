using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class asteroidsGameManager : MonoBehaviour
{
    public asteroidsPlayer player;
    public ParticleSystem explosion;
    public float respawnTime = 5.0f;
    public float respawnInvulnerabilityTime = 3.0f;
    public int lives =3;
    public int score = 0;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;

    public void AsteroidDestroyed(asteroidScript asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        if (asteroid.size < 0.75f)
        {
            this.score += 100;
        }
        else if (asteroid.size < 1.2f)
        {
            this.score += 50;
        }
        else
        {
            this.score += 25;
        }
   }

   public void PlayerDied()
   {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;

        if (this.lives <= 0)
        {
            GameOver();
        } else {
            Invoke(nameof(Respawn), this.respawnTime);
        }
   }
    public void Start()
    {
        gameOverText.gameObject.SetActive(false);
    }
    private void Update()
    {
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
        livesText.text = Mathf.FloorToInt(lives).ToString("D1");
    }
    public void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        this.player.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        Invoke(nameof(TurnOnCollisions), this.respawnInvulnerabilityTime);
    }

   private void TurnOnCollisions()
   {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
   }

   private void GameOver()
   {
        this.lives = 3;
        this.score = 0;
        gameOverText.gameObject.SetActive(true);
        Invoke(nameof(Respawn), this.respawnTime);
    }

}
