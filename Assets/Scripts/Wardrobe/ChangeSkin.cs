using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSkin : MonoBehaviour
{
    [SerializeField] MeshRenderer player;
    [SerializeField] private SaveData saveData;
    public void ChangeMesh(Material changeMaterial)
    {
        player.material = changeMaterial;
        saveData.skinPlayer = changeMaterial;
    }
    public void Confirm(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
