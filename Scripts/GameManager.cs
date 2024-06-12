using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalNPCs = 1;
    private int remainingNPCs;

    void Start()
    {
        remainingNPCs = totalNPCs;
    }

    public void NPCKilled()
    {
        remainingNPCs--;
        if (remainingNPCs <= 0)
        {
            LogVictory();
        }
    }

    public void LogDefeat()
    {
        Debug.Log("Поражение! Игрок был убит.");
    }

    public void LogVictory()
    {
        Debug.Log("Победа! Все противники убиты.");
    }
}
