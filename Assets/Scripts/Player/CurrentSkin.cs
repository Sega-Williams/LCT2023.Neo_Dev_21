using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSkin : MonoBehaviour
{
    [SerializeField] private SaveData saveData;
    SkinnedMeshRenderer skinnedMesh;
    void Start()
    {
        skinnedMesh = GetComponent<SkinnedMeshRenderer>();
        if (saveData.skinPlayer!=null)
        {
            skinnedMesh.material = saveData.skinPlayer;
        }
    }
}
