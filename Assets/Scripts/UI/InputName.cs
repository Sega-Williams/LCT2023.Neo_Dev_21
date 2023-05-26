using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputName : MonoBehaviour
{
    [SerializeField] private SaveData saveData;
    private TMP_InputField _inputField;
    public TMP_InputField InputField => _inputField;
    private void Start()
    {
        if (FindObjectsOfType<TMP_InputField>().Length != 0) 
        {
            _inputField = GetComponent<TMP_InputField>();
            _inputField.onEndEdit.AddListener(NameInputField);
        }
    }
    private void NameInputField(string currentInput)
    {
        if (currentInput != "")
        {
            saveData.namePlayer = currentInput;
            _inputField.gameObject.SetActive(false);
        }
    }

}
