using UnityEngine;

public class TuberiaScript : MonoBehaviour
{

    [Header("Spawn y Despawn")] 
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float despawnX = -10.0f;
    [SerializeField] private float respawnX = 10.0f;
    
    [Header("Variacion")]
    [SerializeField] private float minY = -2.5f;
    [SerializeField] private float maxY = 2.5f;
    
    [SerializeField] private float velocity;
    
    
    void Start()
    {
        
    }

    
    void Update()
    {
        //Movimiento constante a la izquierda 
        transform.Translate (new Vector2 (-velocity, 0) * Time.deltaTime, Space.World);
        
        //Comprobar limite izquierdo
        if (transform.position.x < despawnX)
        {
            RecycleTuberia();
        }
        
        
    }

    private void RecycleTuberia()
    {
        //New altura aleatoria
        float randomY = Random.Range(minY, maxY);
        
        // Reposicionar instantaneo al extremo derecho
        transform.position = new Vector3 (respawnX, randomY, transform.position.z);
    }
}
