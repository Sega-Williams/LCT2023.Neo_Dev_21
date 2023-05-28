using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class StartRace : MonoBehaviour
{
    float startTimer = 3;
    [SerializeField] private GameObject startTimerText;
    [SerializeField] private SaveData saveData;
    private P_Controller p_Controller;
    private NavMeshAgent[] botMovements;
    private void Start()
    {
        p_Controller = FindObjectOfType<P_Controller>();
        botMovements = FindObjectsOfType<NavMeshAgent>();
    }
    void Update()
    {
        Wait();
    }

    void Wait()
    {
        p_Controller.enabled = false;
        foreach (var botMove in botMovements)
        {
            botMove.speed = 0;
        }
        if (startTimer > 0)
        {
            int startTimerInt = (int)startTimer;
            startTimer -= Time.deltaTime;
            startTimerText.GetComponent<TextMeshProUGUI>().text = startTimerInt.ToString();
            return;
        }
        startTimer = 0;
        startTimerText.SetActive(false);
        p_Controller.enabled = true;
        foreach (var botMove in botMovements)
        {
            botMove.speed = saveData.currentLevel;
        }
    }

}
