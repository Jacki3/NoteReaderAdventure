using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserIndex : MonoBehaviour
{
    private TMPro.TMP_InputField inputField;

    void Awake()
    {
        inputField = GetComponent<TMPro.TMP_InputField>();
    }

    public bool IndexFilled()
    {
        if (inputField.text.Length == inputField.characterLimit)
        {
            UIController
                .UpdateTextUI(UIController.UITextComponents.indexWarningText,
                "");
            CoreGameElements.i.gameSave.userIndex = inputField.text;
            return true;
        }
        else
        {
            UIController
                .UpdateTextUI(UIController.UITextComponents.indexWarningText,
                "Please Fill Out User Index!");
            return false;
        }
    }
}
