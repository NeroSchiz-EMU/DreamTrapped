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

    private SpriteRenderer dialoguePrompt;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        dialoguePrompt = GameObject.Find("Dialogue Prompt").GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").GetComponent<Player>();
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
                player.setInCutscene(false);
                step = 0;
            }
            //Continues cutscene
            else
            {
                dialogueWindow.SetActive(true);
                player.setInCutscene(true);

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
            
            //keeps the End Door from showing the "Speak" prompt
            if(gameObject.name == "DialogueHandler")
            {
                dialoguePrompt.enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            dialogueActivated = false;
            dialogueWindow.SetActive(false);
            dialoguePrompt.enabled = false;
        }
    }
}
