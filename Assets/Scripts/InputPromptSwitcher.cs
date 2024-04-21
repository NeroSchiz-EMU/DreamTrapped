using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputPromptSwitcher : MonoBehaviour
{
    [SerializeField] private SpriteRenderer doorPrompt;
    [SerializeField] private SpriteRenderer dialoguePrompt;
    [SerializeField] private SpriteRenderer escapePrompt;
    [SerializeField] private SpriteRenderer swordPrompt;
    [SerializeField] private SpriteRenderer bootsPrompt;
    [SerializeField] private SpriteRenderer gunPrompt;
    [SerializeField] private SpriteRenderer dashPrompt;
    [SerializeField] private SpriteRenderer mapIcon;
    [SerializeField] private SpriteRenderer dialogueProceedPrompt;

    [SerializeField] private GameObject swordKey;
    [SerializeField] private GameObject bootsKey;
    [SerializeField] private GameObject gunKey;
    [SerializeField] private GameObject dashKey;
    [SerializeField] private GameObject swordButton;
    [SerializeField] private GameObject bootsButton;
    [SerializeField] private GameObject gunButton;
    [SerializeField] private GameObject dashButton;

    [SerializeField] private Sprite doorPromptKey;
    [SerializeField] private Sprite doorPromptButton;
    [SerializeField] private Sprite dialoguePromptKey;
    [SerializeField] private Sprite dialoguePromptButton;
    [SerializeField] private Sprite escapePromptKey;
    [SerializeField] private Sprite escapePromptButton;
    [SerializeField] private Sprite itemPromptKey;
    [SerializeField] private Sprite itemPromptButton;
    [SerializeField] private Sprite mapIconKey;
    [SerializeField] private Sprite mapIconButton;
    [SerializeField] private Sprite dialogueProceedKey;
    [SerializeField] private Sprite dialogueProceedButton;

    [SerializeField] private Player player;

    [SerializeField] private bool usingController;

    void Start()
    {
        CheckForController();
        updatePrompts();
    }

    //Check if the player is using keyboard or controller
    private void CheckForController()
    {
        string[] controllerInputs = Input.GetJoystickNames();

        if (controllerInputs.Length > 0 && !string.IsNullOrEmpty(controllerInputs[0]))
        {
            usingController = true;
        }
        else
        {
            usingController = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool prevUsingController = usingController;

        CheckForController();

        if (usingController != prevUsingController)
        {
            updatePrompts();
        }
    }

    public void updatePrompts()
    {

        if (!usingController)
        {
            doorPrompt.sprite = doorPromptKey;
            dialoguePrompt.sprite = dialoguePromptKey;
            escapePrompt.sprite = escapePromptKey;
            mapIcon.sprite = mapIconKey;
            dialogueProceedPrompt.sprite = dialogueProceedKey;

            swordPrompt.sprite = itemPromptKey;
            bootsPrompt.sprite = itemPromptKey;
            gunPrompt.sprite = itemPromptKey;
            dashPrompt.sprite = itemPromptKey;

            if (player.getHasSword())
            {
                swordKey.SetActive(true);
                swordButton.SetActive(false);
            }
            if (player.getHasBoots())
            {
                bootsKey.SetActive(true);
                bootsButton.SetActive(false);
            }
            if (player.getHasGun())
            {
                gunKey.SetActive(true);
                gunButton.SetActive(false);
            }
            if (player.getHasDash())
            {
                dashKey.SetActive(true);
                dashButton.SetActive(false);
            }
        }
        else if (usingController)
        {
            doorPrompt.sprite = doorPromptButton;
            dialoguePrompt.sprite = dialoguePromptButton;
            escapePrompt.sprite = escapePromptButton;
            mapIcon.sprite = mapIconButton;
            dialogueProceedPrompt.sprite = dialogueProceedButton;

            swordPrompt.sprite = itemPromptButton;
            bootsPrompt.sprite = itemPromptButton;
            gunPrompt.sprite = itemPromptButton;
            dashPrompt.sprite = itemPromptButton;

            if (player.getHasSword())
            {
                swordKey.SetActive(false);
                swordButton.SetActive(true);
            }
            if (player.getHasBoots())
            {
                bootsKey.SetActive(false);
                bootsButton.SetActive(true);
            }
            if (player.getHasGun())
            {
                gunKey.SetActive(false);
                gunButton.SetActive(true);
            }
            if (player.getHasDash())
            {
                dashKey.SetActive(false);
                dashButton.SetActive(true);
            }
        }
    }

    public bool getUsingController()
    {
        return usingController;
    }
}
