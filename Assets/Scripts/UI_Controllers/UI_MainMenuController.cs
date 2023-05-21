using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private Button btn_StartGame;
    [SerializeField] private Button btn_Exit;
    
    private void Start() {
        btn_StartGame.onClick.AddListener(() => {
            SceneManager.LoadScene(gameSceneName);
        });
        btn_Exit.onClick.AddListener(()=>{
            Application.Quit();
        });
    }
}
