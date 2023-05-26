using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO", fileName ="Save Data")]
public class SaveData : ScriptableObject
{
    public int currentLevel = 0;
    public string namePlayer;
}
