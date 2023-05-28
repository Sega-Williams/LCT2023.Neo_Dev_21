using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSkin : MonoBehaviour
{
    [SerializeField] private SaveData saveData;
    MeshRenderer skinnedMesh;
    void Start()
    {
        skinnedMesh = GetComponent<MeshRenderer>();
        if (saveData.skinPlayer!=null)
        {
            skinnedMesh.material = saveData.skinPlayer;
        }
    }
}
