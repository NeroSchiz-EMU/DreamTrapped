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

    private float readSpeed = 0.02f;
    private int index;
    private Coroutine displayLineCoroutine;

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
                StartCoroutine(EndCutscene());
                step = 0;
            }
            else
            {
                dialogueWindow.SetActive(true);
                player.setInCutscene(true);

                speakerText.text = speakers[step];
                faceSprite.sprite = faces[step];
                faceBackground.sprite = faceBackgrounds[step];

                //Set text for the current dialogue line
                if (displayLineCoroutine != null)
                {
                    StopCoroutine(displayLineCoroutine);
                }
                displayLineCoroutine = StartCoroutine(DisplayLine(bodyTexts[step]));

                step++;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //keeps the End Door from showing the "Speak" prompt
            if(gameObject.name == "DialogueHandler")
            {
                dialoguePrompt.enabled = true;

            }
            //keeps the sword cutscene from replaying if the player already has the sword
            if (gameObject.name == "SwordDialogueHandler" && player.getHasSword() == true)
            {
                dialoguePrompt.enabled = false;
            }
            else
            {
                dialogueActivated = true;
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

    IEnumerator EndCutscene()
    {
        //Tiny delay so that the player doesn't detect the attack input and instantly attack after the cutscene
        yield return new WaitForSeconds(0.01f);
        dialogueWindow.SetActive(false);
        player.setInCutscene(false);
        step = 0;
    }

    IEnumerator DisplayLine(string line)
    {
        dialogueText.text = null;

        foreach(char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(readSpeed);
        }
    }
}
