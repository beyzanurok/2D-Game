using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Game : MonoBehaviour
{
    public int lives, score, highscore;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Transform playerSpawnPoint;
    [SerializeField]
    private Spawner[] spawners;
    [SerializeField]
    private int level;

    [SerializeField]
    private TextMeshProUGUI scoreText, livesText, bestText;

    private void Start()
    {
        highscore = PlayerPrefs.GetInt("Highscore", 0);
        Load();
        bestText.text = "Best: " + highscore;
        UpdateHUD();
    }
    // Called whenever player is hurt
    public void LoseLife()
    {
        if (lives > 0)
        {
            StopAllCoroutines();
            StartCoroutine(Respawn());
        }
        else
        {
            EndGame();
        }
    }

    void EndGame()
    {
        if (score > highscore)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
        StartNewGame();
    }

    // Respawn player with spawn delay
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        lives--;
        Instantiate(player.gameObject, playerSpawnPoint.position, Quaternion.identity);
        UpdateHUD();
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateHUD();
        CheckForLevelCompletion();
    }

    public void AddLife()
    {
        lives++;
        UpdateHUD();
    }

    private void CheckForLevelCompletion()
    {
        Debug.Log("Check for completion");
        if (!FindObjectOfType<Enemy>())
        {
            // no enemies are alive currently
            // spawners could still spawn enemies though
            foreach (Spawner spawner in spawners)
            {
                if (!spawner.completed)
                {
                    return;
                }
            }
            // complete level
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        Debug.Log("Complete Level");
        // increase level by 1
        // load that level
        level++;
        Save();
        if (level <= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(level);
        }
        else
        {
            Debug.Log("Game won! nice!");
            EndGame();
        }
    }

    // save game progress
    private void Save()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Lives", lives);
        PlayerPrefs.SetInt("Level", level);
    }

    // load game progress
    private void Load()
    {
        score = PlayerPrefs.GetInt("Score", 0);
        lives = PlayerPrefs.GetInt("Lives", 3);
        level = PlayerPrefs.GetInt("Level", 0);
    }

    // reset game progress
    void StartNewGame()
    {
        level = 0;
        SceneManager.LoadScene(level);
        PlayerPrefs.DeleteKey("Score");
        PlayerPrefs.DeleteKey("Lives");
        PlayerPrefs.DeleteKey("Level");
    }

    void UpdateHUD()
    {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
    }
}
