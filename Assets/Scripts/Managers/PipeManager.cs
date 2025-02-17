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


    private float SPAWNPOINTX = 9.5f;

    private float maxHeight = 3f;
    private float minHeight = -1.6f;

    private List<GameObject> spawnedPipes = new List<GameObject>();
    private float spawnTimer;

    private bool move = true;


    public void OnNotify(PlayerActions action, int value)
    {
        if (action == PlayerActions.Die)
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
        spawnedPipes.Add(spawnedPipe);
    }


    Vector3 GetSpawnPoint()
    {
        int randomSpawnHeight = Random.Range(1, 4);
        float height = 0;
        switch (randomSpawnHeight)
        {
            case 1:
                height = maxHeight;
                break;
            case 2:
                height = minHeight;
                break;
            case 3:
                height = maxHeight + minHeight * 0.5f;
                break;
        }
        return new Vector3(SPAWNPOINTX, height);
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