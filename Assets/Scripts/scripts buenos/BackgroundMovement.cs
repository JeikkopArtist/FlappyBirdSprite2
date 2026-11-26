using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{

    [Header("Párametros de Scroll")]
   
    [SerializeField] private float speed = 3.0f;
     private float despawnX = -10.0f;
     private float respawnX = 10.0f;
     private float spriteWidth;
     
    
    void Start()
    {
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        despawnX = -spriteWidth;
        respawnX = spriteWidth;
    }
    
    void Update()
    {
        
        if (GameManager.Instance.GameOver) return;
        
        
        
        // 1. Movimiento constante a la izquierda indepediente de los FPS
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

        // 2. Comprobación del límite izquierdo
        if (transform.position.x <= despawnX)
        {
            RecyclePipe();
        }
    }

    private void RecyclePipe()
    {
        

        // 4. Reposicionamiento instantáneo al extremo derecho
        transform.position = new Vector3(respawnX, transform.position.y , transform.position.z);
        
        
    }

   
}
