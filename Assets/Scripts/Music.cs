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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (openingDialogue.getOpeningCutsceneComplete() && !levelThemeStarted)
        {
            levelThemeStarted = true;
            audioSource.Stop();
            audioSource.PlayOneShot(levelTheme);
        }
    }
}
