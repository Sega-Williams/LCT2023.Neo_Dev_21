using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputName : MonoBehaviour
{
    [SerializeField] private SaveData saveData;
    private TMP_InputField _inputField;
    private GameObject player;
    public TMP_InputField InputField => _inputField;
    private void Start()
    {

        player = FindObjectOfType<P_Controller>().gameObject;
        if (FindObjectsOfType<TMP_InputField>().Length != 0) 
        {
            _inputField = GetComponent<TMP_InputField>();
            if (saveData.currentLevel > 1)
            {
                _inputField.gameObject.SetActive(false);
                return;
            }
            _inputField.onEndEdit.AddListener(NameInputField);
        }
    }
    private void NameInputField(string currentInput)
    {
        if (currentInput != "")
        {
            saveData.namePlayer = currentInput;
            player.gameObject.name = currentInput;
            _inputField.gameObject.SetActive(false);
        }
    }

}
