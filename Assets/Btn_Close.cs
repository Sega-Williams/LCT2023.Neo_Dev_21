using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Btn_Close : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private GameObject gameobjectToClose;

    private void Start() {
        btn.onClick.AddListener(()=>{
            Destroy(gameobjectToClose);
        });
    }
}
