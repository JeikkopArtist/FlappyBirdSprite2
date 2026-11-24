
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
    [SerializeField] private Vector2 pipeSize = Vector2.one;

    [Header("Hitbox")]
    [SerializeField] private float birdRadius = 0.35f;

    private Vector2 velocity = Vector2.zero;
    private const float FuerzaGravity = -9.8f;

    private bool isDead = false;

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
    }

    private void OnDisable()
    {
        if (flapAction != null)
            flapAction.action.Disable();
    }

    private void TriggerGameOver(string colisiónFueraODentroDeRango)
    {
        Debug.Log("Game Over: " + colisiónFueraODentroDeRango);

        isDead = true;

        // if (GameManager.Instance != null)
        // {
        //     GameManager.Instance.IsGameOver();
        // }
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        // 1. Lectura del impulso
        if (flapAction != null && flapAction.action.WasPressedThisFrame())
        {
            velocity.y = impulso;
        }

        // 2. Aplicar la fuerza de la gravedad
        velocity.y += FuerzaGravity * gravityScale * Time.deltaTime;

        // 3. Actualizar posición
        transform.Translate(velocity * Time.deltaTime, Space.World);

        // 4. Orientación del sprite
        float targetAngle = velocity.y > 0 ? maxUpwardRotation : maxDownwardRotation;
        float currentAngle = transform.eulerAngles.z;

        if (currentAngle > 180)
            currentAngle -= 360;

        float lerpedAngle = Mathf.Lerp(
            currentAngle,
            targetAngle,
            Time.deltaTime * rotationSpeed
        );

        transform.rotation = Quaternion.Euler(0, 0, lerpedAngle);

        // 5. Detección de colisiones

        // 5.1. Construir la hitbox del personaje
        CustomCircle birdCircle = new CustomCircle(transform.position, birdRadius);

        // 5.2. Comprobar suelo y techo
        if (birdCircle.center.y - birdRadius <= groundY ||
            birdCircle.center.y + birdRadius >= ceilY)
        {
            TriggerGameOver("Colisión con suelo o techo");
            return;
        }

        // 5.3. Buscar tuberías
        TuberiaScript[] tuberias = FindObjectsByType<TuberiaScript>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );

        Vector2 halfpipeSize = pipeSize / 2;

        foreach (TuberiaScript tuberia in tuberias)
        {
            GameObject go = tuberia.gameObject;

            if (!go.activeInHierarchy)
                continue;

            Transform pipe = go.transform;
            Vector2 pipePos = pipe.position;

            // Descarte rápido
            if (Mathf.Abs(birdCircle.center.x - pipePos.x) >
                (birdRadius + halfpipeSize.x))
            {
                continue;
            }

            // Comprobación exacta Círculo contra AABB
            CustomAABB pipeBox = new CustomAABB(
                pipePos - halfpipeSize,
                pipePos + halfpipeSize
            );

            if (CustomCollision2D.CheckCircleAABB(birdCircle, pipeBox))
            {
                TriggerGameOver("Colisión con tubería: " + go.name);
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Hitbox del pájaro
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, birdRadius);

        // Suelo
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            new Vector2(-10, groundY),
            new Vector2(10, groundY)
        );

        // Techo
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            new Vector2(-10, ceilY),
            new Vector2(10, ceilY)
        );

        // Hitboxes de las tuberías
        Gizmos.color = Color.yellow;

        Vector2 halfpipeSize = pipeSize / 2;

        TuberiaScript[] tuberias = FindObjectsByType<TuberiaScript>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );

        foreach (TuberiaScript tuberia in tuberias)
        {
            if (tuberia == null || !tuberia.gameObject.activeInHierarchy)
                continue;

            Gizmos.DrawWireCube(
                tuberia.transform.position,
                pipeSize
            );
        }
    }
}
