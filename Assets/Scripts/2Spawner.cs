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
        // Isi pool awal
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

        // Random sprite bisa ditambahkan di sini (lihat script Spawner sebelumnya)

        newObj.SetActive(true);

        // Jadwalkan spawn berikutnya
        Invoke(nameof(Spawn), Random.Range(minTime, maxTime));
    }

    private GameObject GetPooledObject()
    {
        // Cari yang tidak aktif di pool
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        // Jika semua aktif, buat baru dan tambahkan ke pool
        GameObject newObj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        pool.Enqueue(newObj);
        return newObj;
    }
}