using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float spawnInterval = 2f;
    [SerializeField] float pipeLifeTime = 8f;

    [SerializeField] GameObject pipe;


    float SPAWNPOINTX = 4.5f;

    float maxHeight = 3f;
    float minHeight = -1.6f;

    Vector3 spawnPoint;
    List<GameObject> spawnedPipes = new List<GameObject>();
    float spawnTimer;


    // Update is called once per frame
    void Update()
    {
        SpawnTimer();

        MoveAndDestroy();
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

}