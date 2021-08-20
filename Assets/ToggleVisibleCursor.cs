using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleVisibleCursor : MonoBehaviour
{
    public InputAction toggleButton = new InputAction("toggleKey", InputActionType.Button, "<Keyboard>/tab");
    void Start()
    {
        toggleButton.performed += TogleButton_performed;
        toggleButton.Enable();

        TogleButton_performed();
    }

    private void TogleButton_performed(InputAction.CallbackContext obj = default)
    {
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
