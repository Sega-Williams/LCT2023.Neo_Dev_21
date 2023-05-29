using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mission : MonoBehaviour
{
    [SerializeField] private SaveData saveData;
    private void Start()
    {
        StartGame(1);
    }
    public void StartGame(float activeMission)
    {
        var text = GetComponentInChildren<TextMeshProUGUI>().text = $"{saveData.currentLevel} уровень\nМиссия: попасть в тройку лидеров";
        gameObject.SetActive(activeMission == 1);
        //Time.timeScale = activeMission == 1 ? 0 : 1;
    }

}
