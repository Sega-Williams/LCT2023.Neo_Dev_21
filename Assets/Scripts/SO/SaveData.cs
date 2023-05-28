using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO", fileName ="Save Data")]
public class SaveData : ScriptableObject
{
    public int currentLevel = 1;
    public string namePlayer = "Player";
    public Material skinPlayer;
}
