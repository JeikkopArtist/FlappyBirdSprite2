using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI puntos;
    [SerializeField] private GameObject panelGameOver;

        
    private GameManager _gameManager;


    // private void OnEnable()
    // {
    //     _gameManager = GameManager.Instance;
    //     _gameManager.RegistraUIManager(this);
    // }
    //
    

    // private void OnDisable()
    // {
    //     _gameManager.DesregistraUIManager(this);
    // }
    
    

    public void Start()
    {
        Showpoints();
        panelGameOver.SetActive(false);
    }
    
    public void Showpoints()
    {
        if  (GameManager.Instance == null) return;
        
            
        puntos.text = "Puntos: " + GameManager.Instance.Puntos.ToString();
        
    }
    
    

    public void OnGameOver()
    {
       
            panelGameOver.SetActive(true);
        
            
       
    }
    
    public void OnReset()
    {
        panelGameOver.SetActive(false);
        Showpoints();
    }
    
    public void NewGame()
    {
       OnReset();
       GameManager.Instance.InicializarJuego();
    }
}
