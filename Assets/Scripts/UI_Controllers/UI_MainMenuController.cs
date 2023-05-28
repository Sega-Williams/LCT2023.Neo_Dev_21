using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private Transform pref_SettingsWindow;
    [SerializeField] private Transform pref_TutorialWindow;
    [SerializeField] private Transform pref_CreditsWindow;

    [Header("Buttons")]
    [SerializeField] private Button btn_StartGame;
    [SerializeField] private Button btn_Wardrobe;
    [SerializeField] private Button btn_Settings;
    [SerializeField] private Button btn_Tutorial;
    [SerializeField] private Button btn_Credits;
    [SerializeField] private Button btn_Exit;
    
    private void Start() {
        btn_StartGame.onClick.AddListener(() => {
            SceneManager.LoadScene(gameSceneName);
        });
        btn_Wardrobe.onClick.AddListener(() => {
            SceneManager.LoadScene("Wardrobe");
        });
        btn_Settings.onClick.AddListener(() => {
            Instantiate(pref_SettingsWindow, FindObjectOfType<Canvas>().transform);
        });
        btn_Tutorial.onClick.AddListener(() => {
            Debug.Log("btn_Tutorial");
        });
        btn_Credits.onClick.AddListener(() => {
            Debug.Log("btn_Credits");
        });
        btn_Exit.onClick.AddListener(()=>{
            Application.Quit();
        });
    }
}
