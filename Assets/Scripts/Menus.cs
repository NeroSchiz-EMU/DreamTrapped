using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject creditsText;
    [SerializeField] private Animator ditherTransition;
    private bool creditsActive;

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
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

    IEnumerator StartGameCoroutine()
    {
        ditherTransition.SetTrigger("startGame");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Level");
    }

}
