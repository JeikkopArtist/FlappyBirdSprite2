
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

    void Update()
    {
        // Comprobar si la partida ha terminado
        if (GameManager.Instance != null && GameManager.Instance.GameOver)
            return;

        // Movimiento constante a la izquierda
        transform.Translate(
            Vector3.left * speed * Time.deltaTime,
            Space.World
        );

        // Comprobar límite izquierdo
        if (transform.position.x < despawnX)
        {
            RecycleTuberia();
        }
    }

    private void RecycleTuberia()
    {
        // Nueva altura aleatoria
        float randomY = Random.Range(minY, maxY);

        // Reposicionar al extremo derecho
        transform.position = new Vector3(
            respawnX,
            randomY,
            transform.position.z
        );
    }

    private void OnDrawGizmosSelected()
    {
        // Visualización del punto de aparición
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            new Vector3(respawnX, transform.position.y, transform.position.z),
            0.2f
        );

        // Visualización del punto de desaparición
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            new Vector3(despawnX, transform.position.y, transform.position.z),
            0.2f
        );
    }
}
