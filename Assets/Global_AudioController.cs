using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Global_AudioController : MonoBehaviour
{
    public static Global_AudioController Instance;

    private void Awake() {
        if(Instance == null){
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    [HideInInspector, SerializeField] 
    private AudioSource globalAudioSource;

    public UnityEvent<float> GlobalVolumeChanged = new UnityEvent<float>();
    private void Start() {
        globalAudioSource = GetComponent<AudioSource>();
        
        GlobalVolumeChanged.AddListener((float volume) => {
            Debug.Log($"Volume = {volume}");

            if(globalAudioSource){
                Debug.Log($"Setted up new volume = {volume}");
                globalAudioSource.volume = volume;
            }
            else Debug.Log("Setup globalAudioSource");
        });
    }
}
