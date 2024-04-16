using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject creditsText;
    [SerializeField] private Animator ditherTransition;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject healthAndInv;
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private Animator logo;
    [SerializeField] private Player player;
    [SerializeField] private EndDoor endDoor;
    private bool creditsActive;
    private bool pauseActive;

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (pauseActive == false)
            {
                pauseMenu.SetActive(true);
                healthAndInv.SetActive(false);
                dialogueWindow.SetActive(false);
                pauseActive = true;

                if (endDoor.getDoorOpening())
                {
                    dialogueWindow.SetActive(false);
                }

                Time.timeScale = 0;
            }
            else if (pauseActive == true)
            {
                Unpause();
            }
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    public void ReloadGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level");
    }

    public void ToggleCredits()
    {
        if (creditsActive == false)
        {
            creditsText.SetActive(true);
            creditsActive = true;
        }
        else if (creditsActive == true)
        {
            creditsText.SetActive(false);
            creditsActive = false;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title Screen");
    }

    public void Unpause()
    {
        pauseMenu.SetActive(false);
        pauseActive = false;
        healthAndInv.SetActive(true);

        if (player.getInCutscene())
        {
            dialogueWindow.SetActive(true);
        }
        if(endDoor.getDoorOpening())
        {
            dialogueWindow.SetActive(false);
        }

        Time.timeScale = 1;
    }

    IEnumerator StartGameCoroutine()
    {
        ditherTransition.SetTrigger("startGame");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Level");
    }

    public bool getPaused()
    {
        return pauseActive;
    }

}
