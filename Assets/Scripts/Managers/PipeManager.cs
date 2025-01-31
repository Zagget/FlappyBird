using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour, IObserver
{
    [SerializeField] Subject gameManagerSubject;

    [Header("Behaviour")]
    [SerializeField] float speed = 5f;
    [SerializeField] float spawnInterval = 2f;

    [Header("Obstacles")]
    [SerializeField] GameObject pipe;


    float SPAWNPOINTX = 9.5f;

    float maxHeight = 3f;
    float minHeight = -1.6f;

    Vector3 spawnPoint;
    List<GameObject> spawnedPipes = new List<GameObject>();
    float spawnTimer;

    bool move = true;

    public void OnNotify(Events @event, int value)
    {
        if (@event == Events.Die)
        {
            move = false;
        }
    }

    // Update is called once per frame
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

    void SpawnPipe()
    {
        UpdateSpawnPoint();

        var spawnedPipe = Instantiate(pipe, spawnPoint, Quaternion.identity);
        spawnedPipes.Add(spawnedPipe);
        Debug.Log($"Spawned pipe at:  {spawnPoint.x} , {spawnPoint.y}");
    }

    Vector3 UpdateSpawnPoint()
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
        spawnPoint = new Vector3(SPAWNPOINTX, height);

        return spawnPoint;
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