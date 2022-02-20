using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject _obstacle;
    public int obsGroupCount = 10;
    public int obsGroupDistance = 10;
    [SerializeField] Vector2 Xposition;
    private float[] xPosition = new float[2];
    void Start()
    {
        StartCoroutine(obsSpawn());
    }

    IEnumerator obsSpawn()
    {
        xPosition[0] = Xposition.x;
        xPosition[1] = Xposition.y;

        for (int x = 1; x <= obsGroupCount; x++)
        {
            int xIndex = Random.Range(0, 2);
            float randomX = xPosition[xIndex];
            float randomZ = x * obsGroupDistance;
            var obstacle = Instantiate(_obstacle, transform.position + new Vector3(randomX, 0, randomZ), Quaternion.identity);
            obstacle.transform.rotation = Quaternion.Euler(0, 90, 0);
            obstacle.transform.parent = gameObject.transform;
        }
        yield return null;
    }
}
