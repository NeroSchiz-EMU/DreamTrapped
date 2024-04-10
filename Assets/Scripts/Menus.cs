using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject creditsText;
    private bool creditsActive;

    public void StartGame()
    {
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

}
