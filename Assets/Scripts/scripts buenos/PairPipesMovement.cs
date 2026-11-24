using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PairPipesMovement : MonoBehaviour
{
    [Header("Parámetros de Scroll")]
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float despawnX = -10.0f;
    [SerializeField] private float respawnX = 10.0f;

    [Header("Variación de Altura")]
    [SerializeField] private float minY = -2.0f;
    [SerializeField] private float maxY = 2.5f;
    [SerializeField] private float coefienteReduccionVertical = 0.10f;
    
    private bool useGameManager;
    
    // distancia entre pipes, variable con el tiempo (se reduce en cada respawn)
    private float _pipesDistance;
    Transform _pipeUp, _pipeDown;

    private void Start()
    {
        // selección del GameManager a utilizar
        if (GameManager.Instance != null)
        {
            useGameManager = true;
        }
        
        
        // obtiene las refrencias a la pipe superior e inferior
        _pipeUp = transform.GetChild(0).transform;
        _pipeDown = transform.GetChild(1).transform;
        
        // calcula la distancia inicial entre el pipes superior e inferior
        float pipeDownBorderUp = _pipeDown.transform.position.y + _pipeDown.transform.localScale.y / 2;
        float pipeUpBorderDown = _pipeUp.transform.position.y - _pipeUp.transform.localScale.y / 2;
        _pipesDistance = Mathf.Abs(pipeDownBorderUp - pipeUpBorderDown);
    }

    void Update()
    {
        if (useGameManager)
        {
            if (GameManager.Instance.GameOver) return;
        }
        
        
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
        // 3. Cálculo de la nueva altura aleatoria
        float randomY = Random.Range(minY, maxY);

        // 4. Reposicionamiento instantáneo al extremo derecho
        transform.position = new Vector3(respawnX, randomY, transform.position.z);
        
        // 5. Desplazamiento de la pipeUp para reducir el espacio intermedio
        _pipesDistance *= (1 - coefienteReduccionVertical);
        float pipeUpCenterY = _pipeDown.transform.position.y + _pipeDown.transform.localScale.y / 2
            + _pipesDistance + _pipeUp.transform.localScale.y / 2;
        _pipeUp.transform.position = new Vector3(_pipeUp.transform.position.x, pipeUpCenterY, _pipeUp.transform.position.z);
    }

    public void Setup(float minY, float maxY, float despawnX, float respawnX)
    {
        this.minY = minY;
        this.maxY = maxY;
        this.despawnX = despawnX;
        this.respawnX = respawnX;
    }
}