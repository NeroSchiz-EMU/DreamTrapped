using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip teddyTheme;
    [SerializeField] private AudioClip levelTheme;

    [SerializeField] private Dialogue openingDialogue;
    private bool levelThemeStarted;

    // Update is called once per frame
    void Update()
    {
        if (openingDialogue.getOpeningCutsceneComplete() && !levelThemeStarted)
        {
            levelThemeStarted = true;
            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = levelTheme;
            audioSource.Play();
        }
    }
}
