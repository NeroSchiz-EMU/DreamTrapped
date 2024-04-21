using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject creditsText;
    [SerializeField] private Animator ditherTransition;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject healthAndInv;
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private Player player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private EndDoor endDoor;
    [SerializeField] private GameObject map;
    [SerializeField] private Animator mapAnimator;
    private bool creditsActive;
    private bool mapActive;
    private bool pauseActive;
    private bool deathActive;
    private bool dyingStarted;

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            //PAUSE SCREEN
            if (pauseMenu != null && pauseActive == false && !deathActive && !mapActive)
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

        //DEATH SCREEN
        if(player != null && player.getHealth() >= 98)
        {
            deathActive = true;
            player.setInCutscene(true);
            if (!dyingStarted)
            {
                playerAnimator.SetBool("dying", true); //Fall to ground animation
                dyingStarted = true;
                StartCoroutine(PlayerDead());
            }
            healthAndInv.SetActive(false);
            map.SetActive(false);
            dialogueWindow.SetActive(false);
            //Time.timeScale = 0;
        }

        //MAP
        if (map != null && Input.GetButtonDown("Map") && !pauseActive)
        {
            Debug.Log("map");
            if (!mapActive && !player.getInCutscene())
            {
                mapActive = true;
                map.SetActive(true);
                mapAnimator.SetBool("active", true);
                player.setInCutscene(true);
                
            }
            else if (mapActive)
            {
                StartCoroutine(CloseMap());
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

    public bool getDead()
    {
        return deathActive;
    }

    public bool getMapOpen()
    {
        return mapActive;
    }

    IEnumerator PlayerDead()
    {
        yield return new WaitForSeconds(1);
        playerAnimator.SetBool("dying", false);
        playerAnimator.SetBool("dead", true); //Looping Z animation
        yield return new WaitForSeconds(2);
        deathScreen.SetActive(true);
    }

    IEnumerator CloseMap()
    {
        mapAnimator.SetBool("active", false);
        yield return new WaitForSeconds(0.4f);
        mapActive = false;
        player.setInCutscene(false);
        map.SetActive(false);
    }

}
