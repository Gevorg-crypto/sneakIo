using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadCollisionHandler : MonoBehaviour
{
    public GameObject peasantPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            Debug.Log(other);
            NPCMovement otherNPC = other.GetComponent<NPCMovement>();
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (otherNPC != null)
            {
                DissolveSquad(otherNPC.squad);
            }
            else if (player != null)
            {
                DissolveSquad(player.squad);
            }
        }
    }

    void DissolveSquad(List<Transform> squad)
    {
        foreach (Transform member in squad)
        {
            if (member != transform)
            {
                Instantiate(peasantPrefab, member.position, Quaternion.identity);
                Destroy(member.gameObject);
            }
        }
        squad.Clear();
    }
}
