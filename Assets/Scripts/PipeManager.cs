using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float spawnInterval = 2f;
    [SerializeField] float pipeLifeTime = 8f;

    [SerializeField] GameObject pipeHigh;
    [SerializeField] GameObject pipeMedium;
    [SerializeField] GameObject pipeLow;

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

            // Destroy pipe if it exceeds its lifetime
            if (pipe.transform.position.x < -10f) // Adjust this position threshold as needed
            {
                Destroy(pipe);
                spawnedPipes.RemoveAt(i);
            }
        }
    }

    void SpawnPipe()
    {
        GameObject pipe = GetRandomPipe();

        if (pipe == null)
        {
            Debug.Log("pipe is null");
        }

        var spawnedPipe = Instantiate(pipe);

        spawnedPipes.Add(spawnedPipe);
    }


    GameObject GetRandomPipe()
    {
        int randomPipe = Random.Range(1, 4);

        switch (randomPipe)
        {
            case 1: return pipeHigh;
            case 2: return pipeMedium;
            case 3: return pipeLow;
            default: return null;
        }
    }

}