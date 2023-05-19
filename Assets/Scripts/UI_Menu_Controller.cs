using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Menu_Controller : MonoBehaviour
{
    public static UI_Menu_Controller Instance;

    private void Awake() {
        if(Instance == null){
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }
    

    [SerializeField] private string gameSceneName = "TestScene";
    [SerializeField] private string menuSceneName = "Menu";
    private void MenuOpen(){
        Time.timeScale = 0;
    }
    private void MenuClose(){
        Time.timeScale = 1;
    }



    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Debug.Log("Esc");
            if(SceneManager.GetSceneByName(menuSceneName) != SceneManager.GetActiveScene()){
                SceneManager.LoadScene(menuSceneName, LoadSceneMode.Additive);
                MenuOpen();
            }
            else{
                SceneManager.UnloadSceneAsync(menuSceneName);
                MenuClose();
            } 
        }
    }

    // called first
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        SceneManager.SetActiveScene(scene);
    }

    // called when the game is terminated
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
