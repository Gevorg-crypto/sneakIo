using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject peasantPrefab; // Префаб крестьян
    public int numberOfPeasantsToSpawn = 10; // Количество крестьян для спавна
    public float respawnDelay = 5f; // Задержка перед респавном

    private readonly List<GameObject> _spawnedPeasants = new List<GameObject>();
    private float _areaWidth;
    private float _areaHeight;
    private Vector3 _spawnAreaPosition;

    void Start()
    {
        // Установка размеров области спавна
        _areaWidth = GetComponent<BoxCollider2D>().size.x;
        _areaHeight = GetComponent<BoxCollider2D>().size.y;
        _spawnAreaPosition = transform.position;

        SpawnPeasants();
    }

    void SpawnPeasants()
    {
        for (int i = 0; i < numberOfPeasantsToSpawn; i++)
        {
            SpawnPeasant();
        }
    }

    void SpawnPeasant()
    {
        float randomX = Random.Range(_spawnAreaPosition.x - _areaWidth / 2, _spawnAreaPosition.x + _areaWidth / 2);
        float randomY = Random.Range(_spawnAreaPosition.y - _areaHeight / 2, _spawnAreaPosition.y + _areaHeight / 2);

        Vector3 spawnPosition = new Vector3(randomX, randomY, 0);
        GameObject peasant = Instantiate(peasantPrefab, spawnPosition, Quaternion.identity);
        _spawnedPeasants.Add(peasant);

        // Запускаем корутину для респавна крестьян после задержки
        StartCoroutine(RespawnPeasantAfterDelay(peasant));
    }

    IEnumerator RespawnPeasantAfterDelay(GameObject peasant)
    {
        yield return new WaitForSeconds(respawnDelay);
        if (peasant == null)
        {
            SpawnPeasant();
        }
    }

    // Метод для респавна всех крестьян, если потребуется
    public void RespawnAllPeasants()
    {
        foreach (var peasant in _spawnedPeasants)
        {
            if (peasant == null)
            {
                SpawnPeasant();
            }
        }
    }
}
