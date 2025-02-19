using System.Collections;
using UnityEngine;

public class WindEffect : MonoBehaviour, IObserver
{
    [SerializeField] private Subject Player;

    [SerializeField] private GameObject windEffectPrefabs;
    [SerializeField] private float speed;
    [SerializeField] private float fallDuration;
    [SerializeField] float frequency = 3f;
    [SerializeField] float amplitude = 0.5f;

    public void OnNotify(Events @event, int value = 0)
    {
        if (@event == Events.Jump)
        {
            Vector2 spawnPos = transform.position;

            GameObject wind = Instantiate(windEffectPrefabs, spawnPos, windEffectPrefabs.transform.rotation);
            SpriteRenderer sr = wind.GetComponent<SpriteRenderer>();
            StartCoroutine(Move(wind, sr, spawnPos));
        }
    }

    private IEnumerator Move(GameObject wind, SpriteRenderer sr, Vector2 startPos)
    {
        startPos = transform.position;
        Vector2 endPosition = new Vector2(startPos.x, startPos.y - speed);

        StartCoroutine(Fade.FadeInOrOut(sr, 1.5f, 1, 0));

        float elapsedTime = 0f;
        while (elapsedTime < fallDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fallDuration;

            // Smooth downward movement
            float newY = Mathf.Lerp(startPos.y, endPosition.y, t);

            // Sinusoidal side-to-side sway
            float newX = startPos.x + Mathf.Sin(elapsedTime * frequency) * amplitude;

            wind.transform.position = new Vector2(newX, newY);

            yield return null;
        }

        Destroy(wind);
    }


    private void OnEnable()
    {
        Player.AddObserver(this);
    }

    private void OnDisable()
    {
        Player.RemoveObserver(this);
    }
}
