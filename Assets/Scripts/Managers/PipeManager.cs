using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour, IObserver
{
    [SerializeField] private Subject gameManagerSubject;

    [Header("Behaviour")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float spawnInterval = 2f;

    [Header("Obstacles")]
    [SerializeField] private GameObject pipe;

    private float spawnPointX;
    private float minHeight;
    private float maxHeight;

    private List<GameObject> spawnedPipes = new List<GameObject>();
    private float spawnTimer;
    private bool move = true;

    private void Start()
    {
        CalculateScreenBounds();
    }

    void CalculateScreenBounds()
    {
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Camera.main.aspect;

        spawnPointX = screenWidth * 0.5f + 1f;
        maxHeight = screenHeight * 0.25f;
        minHeight = -maxHeight;
    }

    public void OnNotify(Events @event, int value)
    {
        if (@event == Events.Die)
        {
            move = false;
        }
    }

    void Update()
    {
        if (move)
        {
            SpawnTimer();
            MoveAndDestroy();
        }
    }


    void SpawnTimer()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnPipe();
            spawnTimer = 0f;
        }
    }

    void SpawnPipe()
    {
        Vector3 spawnPoint = GetSpawnPoint();

        var spawnedPipe = Instantiate(pipe, spawnPoint, Quaternion.identity);
        spawnedPipe.transform.SetParent(transform);
        spawnedPipes.Add(spawnedPipe);
    }

    Vector3 GetSpawnPoint()
    {
        float height = Random.Range(minHeight, maxHeight);
        return new Vector3(spawnPointX, height);
    }

    void MoveAndDestroy()
    {
        for (int i = spawnedPipes.Count - 1; i >= 0; i--)
        {
            GameObject pipe = spawnedPipes[i];
            pipe.transform.Translate(Vector2.left * speed * Time.deltaTime);

            if (pipe.transform.position.x < -5)
            {
                Destroy(pipe);
                spawnedPipes.RemoveAt(i);
            }
        }
    }

    private void OnEnable()
    {
        gameManagerSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        gameManagerSubject.RemoveObserver(this);
    }
}
