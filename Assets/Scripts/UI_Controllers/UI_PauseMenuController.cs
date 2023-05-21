using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_PauseMenuController : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private Button btn_BackToMainMenu;
    [SerializeField] private Button btn_Continue;
    
    private void Start() {
        btn_BackToMainMenu.onClick.AddListener(() => {
            UI_Menu_Controller.Instance.PauseMenu_Close();
            SceneManager.LoadScene(mainMenuSceneName);
        });
        btn_Continue.onClick.AddListener(() => {
            UI_Menu_Controller.Instance.PauseMenu_Close();
        });
    }
}
