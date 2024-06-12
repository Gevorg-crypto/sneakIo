using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 movement;
    public List<Transform> squad = new List<Transform>();
    public GameObject peasantPrefab;
    public GameManager gameManager;
    void Start()
    {
        squad.Add(transform); // Добавляем самого игрока в начало списка
    }

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Peasant"))
        {
            Transform peasant = other.transform;
            peasant.GetComponent<PeasantFollow>().target = squad[squad.Count - 1];
            squad.Add(peasant);
            peasant.tag = "PlayerBody"; // Назначаем тег PlayerBody
        }
        else if (other.CompareTag("NPCBody"))
        {
            
            DissolveSquad();
            gameManager.LogDefeat(); // Логируем поражение
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
