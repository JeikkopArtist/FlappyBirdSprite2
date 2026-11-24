using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PipePoolManager : MonoBehaviour
{
    
    [SerializeField] private Transform pipesContainer;
    [SerializeField] private GameObject pipePrefab;
    [SerializeField] private int poolSize = 4;
    [SerializeField] private float distanceBetweenPipes = 5.0f;
    [SerializeField] private float despawnX = -10.0f;
    [SerializeField] private float respawnX = 10.0f;
    [SerializeField] private float minY = -2.0f;
    [SerializeField] private float maxY = 2.5f;

    public GameObject[] pipePool { get; private set; }

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        // clear pool
        try
        {
            if (pipePool != null)
            {
                foreach (GameObject go in pipePool)
                {
                    Destroy(go);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error while clearing pipe pool: " + e.Message);
        }
        

        // create pool
        pipePool = new GameObject[poolSize];

        // distancia entre pipes calculada = (respawn - despawn) / poolSize
        distanceBetweenPipes = (respawnX - despawnX) / poolSize;
        for (int i = 0; i < poolSize; i++)
        {
            // Posicionamiento inicial escalonado en X
            float spawnX = respawnX + (i * distanceBetweenPipes);
            float randomY = Random.Range(minY, maxY);
            
            Vector3 spawnPosition = new Vector3(spawnX, randomY, 0f);
            
            GameObject go = Instantiate(pipePrefab, spawnPosition, Quaternion.identity, pipesContainer);
            PairPipesMovement pairPipesMovement = go.GetComponent<PairPipesMovement>();
            pairPipesMovement.Setup(minY, maxY, despawnX, respawnX);
            pipePool[i] = go;
        }
    }
}