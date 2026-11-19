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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IsGameOver()
    {
        GameOver = true;
    }
    
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
