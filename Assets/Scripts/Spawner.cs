using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefab & Timing")]
    public GameObject prefab;
    public float minTime = 2f;
    public float maxTime = 4f;

    [Header("Pooling")]
    public int poolSize = 5;
    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
        Spawn();
    }

    private void Spawn()
    {
        GameObject newObj = GetPooledObject();
        newObj.transform.position = transform.position;
        newObj.transform.rotation = Quaternion.identity;

        newObj.SetActive(true);

        Invoke(nameof(Spawn), Random.Range(minTime, maxTime));
    }

    private GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        GameObject newObj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        pool.Enqueue(newObj);
        return newObj;
    }
}