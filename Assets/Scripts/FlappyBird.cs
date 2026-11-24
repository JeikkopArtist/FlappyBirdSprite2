
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class FlappyBird : MonoBehaviour
{
    
    private Vector2 velocidad;
    [SerializeField] private InputActionReference flapAction;
    
    [Header("Parámetros del Salto")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float maxUpwardRotation = 30.0f;
    [SerializeField] private float maxDownwardRotation = -90.0f;
    [SerializeField] private float rotationSpeed = 5.0f;

    [Header("Físicas")]
    [SerializeField] private bool forcePhysic;
    [SerializeField] private bool rotateSimulation;
   
   
   
    
    private GameManager _gameManager;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private AudioSource _audioSource;
    private bool jumpRequested;
    private bool isDead = false;
    

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        // Configuración por código de las restricciones del motor
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }
    


    void Start()
    {
        if (flapAction == null)
        {
            Debug.Log("Debe asociar una acción");
        }
    }

    private void OnEnable()
    {
        if (flapAction != null)
            flapAction.action.Enable();
        _gameManager = GameManager.Instance;
    }

    private void OnDisable()
    {
        if (flapAction != null)
            flapAction.action.Disable();
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (!godMode)
        {
            if (!isDead)
            {
                if (other.tag == "Pipe" || other.tag == "Floor" || other.tag == "Ceil")
                {
                    // Lanza el sonido de Hit
                      // _audioSource.PlayOneShot(clipHit);
                    TriggerGameOver($"Colisión con: {other.name}", other.tag == "Floor");
                    // _gameManager.OnGameOver();
                }
            }
            else
            {
                // parar solamente contra el suelo
                if (other.tag == "Floor")
                {
                    _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
                }
            }
        }
    }
    void Update()
    {
        // 1. Captura de Input en el ciclo de Render
        if (flapAction.action.WasPressedThisFrame())
        {
            jumpRequested = true;
            _animator.SetTrigger("Flap");
        }
        
        _animator.SetFloat("Speed", _rigidbody2D.linearVelocity.y);
        _animator.SetBool("IsDead", isDead);
       
    }
    void FixedUpdate()
    {
        // 3. Aplicación de físicas en el paso de tiempo fijo del motor
        if (jumpRequested)
        {
            if (forcePhysic)
            {
                // Aplicar un impulso físico real teniendo en cuenta la masa (Simulador)
                _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                // Sobrescribir la velocidad vertical para respuesta rápida (Arcade)
                _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, jumpForce);
            }
            jumpRequested = false;
        }
    }
    private void TriggerGameOver(string reason, bool floorCollider = false)
    {
        isDead = true;
        _animator.SetBool("isDead", isDead);
        if (!floorCollider)
        {
            // inicia el descenso
            _rigidbody2D.linearVelocity = Vector2.down;
            // Invoke(nameof(FallAfterHit), clipHit.length);
        }
        else
        {
            //_rigidbody2D.linearVelocity = Vector2.zero;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        Debug.Log($"[GAME OVER] {reason}");
    }
}


