using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlappyBirdSimulator : MonoBehaviour
{
    
    
    [Header("Parámetros Físicos")]
    [SerializeField] private float impulso = 5.0f;
    [SerializeField] private float gravityScale = 1.5f;
    [SerializeField] private InputActionReference flapAction;
    [SerializeField] private float maxUpwardRotation = 30.0f;
    [SerializeField] private float maxDownwardRotation = -90.0f; 
    [SerializeField] private float rotationSpeed = 5.0f;
     [SerializeField] private float groundY = 0.0f;
    [SerializeField] private float ceilY = 0.0f;
    [SerializeField] private Vector2 pipeSize =  Vector2.one;

    [Header("Hitbox")]
    [SerializeField] private float birdRadius = 0.35f;
    
    
    private Vector2 velocity = Vector2.zero;
    private const float  FuerzaGravity = -9.8f;
    
    private bool  isDead = false;
    
   
    void Start()
    {
        if (flapAction == null)
        {
            Debug.Log("Debe asociar una acción");
        }
    }
    
    private void OnEnable()
    {
        flapAction.action.Enable();
    }
    private void OnDisable()
    {
        flapAction.action.Disable();
    }
    
    private void TriggerGameOver(string colisiónFueraODentroDeRango)
    {
        Debug.Log("Game Over");
        isDead = true;
    }

   
    void Update()
    {
       
        if (isDead)
        {
            return;
        }

        // if ((Input.GetKeyDown(KeyCode.Space)) || Input.GetMouseButton(0))
        if (flapAction.action.WasPressedThisFrame())
        {
            velocity.y = impulso;
        }  //Sobrescribe la velocidad
        
        
        
        //2.Aplicar la fuerza de la gravedad
        velocity.y += FuerzaGravity * Time.deltaTime;
        
        //3. Actualizar posición
        transform.Translate (velocity * Time.deltaTime, Space.World);

       
        
        //4.Orientación del sprite
        
        
        // if (velocity.y > 0)
        // {
        //     transform.rotation = Quaternion.Euler(0, 0, maxUpwardRotation );
        // }
        // else if (velocity.y < 0)
        // {
        //     transform.rotation = Quaternion.Euler(0, 0, maxDownwardRotation);
        // }
        //
        
        
        
        float targetAngle = velocity.y > 0 ? maxUpwardRotation : maxDownwardRotation;
        float currentAngle = transform.eulerAngles.z;
        
        if (currentAngle > 180) currentAngle -= 360;
        
        float lerpedAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * rotationSpeed);
        transform.rotation= Quaternion.Euler(0, 0, lerpedAngle);
        
        //5. Detección de colisiones
    
        //5.1. Construir la hitbox actual del personaje
        CustomCircle birdCircle = new CustomCircle(transform.position , birdRadius);
     
        //5.2. Chequeo contra límites verticales
        if ((birdCircle.center.y - birdRadius < groundY) && (birdCircle.center.y + birdRadius > groundY))
        {
            TriggerGameOver("Colisión fuera o dentro de rango");
            return;
        }
        

        TuberiaScript[] tuberias = FindObjectsByType<TuberiaScript>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        
        //4. Bucle de colisión contra tuberías con descarte rápido (Broadphase)

        if (tuberias.Length > 0)
        {
            Vector2 halfpipeSize = pipeSize / 2;
            foreach (TuberiaScript tuberia in tuberias)
            {
                GameObject go = tuberia.gameObject;
                if (!go.activeInHierarchy) ;
                Transform pipe = go.transform;
                
                Vector2 pipePos = pipe.position;

                if (Mathf.Abs(birdCircle.center.x - pipePos.x) > (birdRadius + halfpipeSize.x))
                {
                    continue;
                }
                
                
                CustomAABB pipeBox = new CustomAABB (pipePos - halfpipeSize, pipePos + halfpipeSize );
                if (CustomCollision2D.CheckCircleAABB(birdCircle, pipeBox))
                {
                    TriggerGameOver($"Colisión con tuberia:{go.name}");
                    break;
                }
                
            }
        }

    }

    
    
    


    private void OnDrawGizmos()
    {
        //color del gizmo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, birdRadius);
        
        //dibuja el suelo
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2 (-10, groundY), new Vector2 (10, groundY));
        
        //dibuja el techo
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector2 (-10, ceilY), new Vector2 (10, ceilY));
        
        
    }
    
    //Dibujar techo y suelo
   
    
}
