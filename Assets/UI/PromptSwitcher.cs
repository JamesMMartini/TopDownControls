using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PromptSwitcher : MonoBehaviour
{
    [SerializeField] GameObject ControllerPrompt;
    [SerializeField] GameObject KeyboardPrompt;

    PlayerInput input;

    // Start is called before the first frame update
    void Start()
    {
        input = GameObject.FindObjectOfType<PlayerBallController>().GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.currentControlScheme == "Gamepad" && !ControllerPrompt.activeInHierarchy)
        {
            ControllerPrompt.SetActive(true);
            KeyboardPrompt.SetActive(false);
        }
        else if (input.currentControlScheme == "Keyboard&Mouse" && !KeyboardPrompt.activeInHierarchy)
        {
            ControllerPrompt.SetActive(false);
            KeyboardPrompt.SetActive(true);
        }
    }
}
