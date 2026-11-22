
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    public bool GameOver { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Time.timeScale = 1f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IsGameOver()
    {
        GameOver = true;
        Time.timeScale = 0f;

        Debug.Log("Partida detenida");
    }

    public void ResetGame()
    {
        GameOver = false;
        Time.timeScale = 1f;
    }
}
