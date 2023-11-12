using UnityEngine;

public class FlappyBirdSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float spawnRate = 1.3f; //default 1
    public float minHeight = -1f; //default -1
    public float maxHeight = 2.5f; //default 1

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }
    private void Spawn()
    {
        GameObject pipes = Instantiate(prefab, transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
    }
}
