using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentName : MonoBehaviour
{
    [SerializeField] private SaveData saveData;
    void Start()
    {
        gameObject.name = saveData.namePlayer;
    }
}
