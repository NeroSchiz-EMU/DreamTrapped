using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    //UI References
    [SerializeField] GameObject dialogueWindow;

    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private SpriteRenderer faceSprite;
    [SerializeField] private SpriteRenderer faceBackground;

    [Header("Cutscene Data")]
    [SerializeField] private string[] speakers;
    [SerializeField][TextArea] private string[] bodyTexts;
    [SerializeField] private Sprite[] faces;
    [SerializeField] private Sprite[] faceBackgrounds;

    private bool dialogueActivated;
    private int step;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Melee") && dialogueActivated)
        {
            //Ends cutscene if the amount of steps reaches the set amount
            if (step >= speakers.Length)
            {
                dialogueWindow.SetActive(false);
                step = 0;
            }
            //Continues cutscene
            else
            {
                dialogueWindow.SetActive(true);

                speakerText.text = speakers[step];
                dialogueText.text = bodyTexts[step];
                faceSprite.sprite = faces[step];
                faceBackground.sprite = faceBackgrounds[step];

                step++;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            dialogueActivated = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            dialogueActivated = false;
            dialogueWindow.SetActive(false);
        }
    }
}
