using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private SaveData saveData;
    private Finish finish;
    float baseSpeed = 10;
    private void Awake()
    {
        finish = GetComponent<Finish>();
        var allBots = FindObjectsOfType<PathFollower>();
        foreach (var bot in allBots)
        {
            bot.speed = baseSpeed * saveData.currentLevel;
            Debug.Log($"Speed bots on this level = {bot.speed}");
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<P_Controller>(out P_Controller playerNow)) 
        {
            int playerPlace = finish.leaderBoard.FirstOrDefault(x => x.Value == saveData.namePlayer).Key;
            if (playerPlace<=1)
            {
                saveData.currentLevel++;
            }
        }

    }
}
