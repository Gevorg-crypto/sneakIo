using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Transform target;
    public GameObject peasantPrefab;
    public List<Transform> squad = new List<Transform>();
    public GameManager gameManager;

    void Start()
    {
        squad.Add(transform); // Добавляем самого NPC в начало списка
    }

    void Update()
    {
        FindNearestPeasant();
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            MoveTowardsTarget();
        }
    }

    void FindNearestPeasant()
    {
        Peasant[] peasants = FindObjectsOfType<Peasant>();
        float minDistance = Mathf.Infinity;
        Transform nearestPeasant = null;

        foreach (Peasant peasant in peasants)
        {
            if (peasant.GetComponent<PeasantFollow>().target != null)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, peasant.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPeasant = peasant.transform;
            }
        }

        target = nearestPeasant;
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.CompareTag("Peasant"))
        {
            Transform peasant = other.transform;
            peasant.GetComponent<PeasantFollow>().target = squad[squad.Count - 1];
            squad.Add(peasant);
            peasant.tag = "NPCBody"; // Назначаем тег NPCBody

            FindNearestPeasant();
        }
        else if (other.CompareTag("PlayerBody"))
        {
            DissolveSquad();
            gameManager.NPCKilled();
        }
    }

    void DissolveSquad()
    {
        // Удалить лидера и создать на его месте крестьянина
        Instantiate(peasantPrefab, squad[0].position, Quaternion.identity);
        Destroy(squad[0].gameObject);

        // Переназначить лидера, если есть еще члены отряда
        if (squad.Count > 1)
        {
            Transform newLeader = squad[1];
            squad.RemoveAt(1);

            for (int i = 0; i < squad.Count; i++)
            {
                Instantiate(peasantPrefab, squad[i].position, Quaternion.identity);
                Destroy(squad[i].gameObject);
            }
            squad.Clear();
            squad.Add(newLeader);
            newLeader.tag = "Peasant";
        }
        else
        {
            squad.Clear();
        }
    }
}
