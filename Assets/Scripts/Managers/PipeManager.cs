using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour, IObserver
{
    [SerializeField] private Subject gameManagerSubject;

    [Header("Obstacles")]
    [SerializeField] private GameObject pipe;

    [Header("Behaviour")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float spawnInterval = 2f;

    [Range(0, 1)]
    [SerializeField] private float verticalSpawnMargin = 0.3f;

    private List<GameObject> spawnedPipes = new List<GameObject>();

    private float destroyPipePoint;
    private float spawnPointX;
    private float minHeight;
    private float maxHeight;

    private float spawnTimer;
    private bool move = true;

    private void Start()
    {
        CalculateScreenBounds();
    }

    void CalculateScreenBounds()
    {
        float halfScreenHeight = Camera.main.orthographicSize;
        float halfScreenWidth = halfScreenHeight * Camera.main.aspect;

        destroyPipePoint = -halfScreenWidth - 1f;

        spawnPointX = halfScreenWidth + 1f;
        maxHeight = halfScreenHeight * verticalSpawnMargin;
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
        float height = Random.Range(minHeight, maxHeight);
        Vector3 spawnPoint = new Vector3(spawnPointX, height);

        var spawnedPipe = Instantiate(pipe, spawnPoint, Quaternion.identity);
        spawnedPipe.transform.SetParent(transform);
        spawnedPipes.Add(spawnedPipe);
    }

    void MoveAndDestroy()
    {
        for (int i = spawnedPipes.Count - 1; i >= 0; i--)
        {
            GameObject pipe = spawnedPipes[i];
            pipe.transform.Translate(Vector2.left * speed * Time.deltaTime);

            if (pipe.transform.position.x < destroyPipePoint)
            {
                Destroy(pipe);
                spawnedPipes.RemoveAt(i);
            }
        }
    }

    void OnEnable() => gameManagerSubject.AddObserver(this);
    void OnDisable() => gameManagerSubject.RemoveObserver(this);
}