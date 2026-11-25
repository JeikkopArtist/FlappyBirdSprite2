
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /** Singleton: instancia estática única del objeto GameManager */
    public static GameManager Instance { get; private set; }
    public int Puntos { get; set; } = 0;
    public bool GameOver { get; private set; }
    private AudioSource  _audioSource;
    [SerializeField] private bool music;
    private PipePoolManager _pipePoolManager;


    private void Awake()
    {
// verifica si existe alguna instancia del GameManager previa
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
// si ya existe una instancia previa, eliminar este objeto (suicidio)
            Destroy(gameObject);
        }

        _pipePoolManager = GetComponent<PipePoolManager>();
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        InicializarJuego();
    }
    
    public void InicializarJuego()
    {
        GameOver = false;
        Puntos = 0;
        _pipePoolManager.Setup();
        if (_audioSource)
        {
            if (_audioSource.isPlaying) _audioSource.Stop();
            if(music) _audioSource.Play();
        }
    }

    public void OnGameOver()
    {
        GameOver = true;
        
    }
    
   
    
    

   
}