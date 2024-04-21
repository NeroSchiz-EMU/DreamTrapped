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
    [SerializeField] private SpriteRenderer proceedPrompt;

    [SerializeField] private GameObject healthInvMap;

    [Header("Cutscene Data")]
    [SerializeField] private string[] speakers;
    [SerializeField][TextArea] private string[] bodyTexts;
    [SerializeField] private Sprite[] faces;
    [SerializeField] private Sprite[] faceBackgrounds;

    private bool dialogueActivated;
    private int step;
    private bool openingCutsceneStarted;
    private bool openingCutsceneComplete;
    private bool doorUnlockCutsceneStarted;
    
    [SerializeField] private Animator explosion;
    [SerializeField] private SpriteRenderer explosionSprite;
    [SerializeField] private AudioSource explosionSound;

    private float readSpeed = 0.02f;
    private int index;
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = true;

    private SpriteRenderer dialoguePrompt;

    private Menus menu;

    private Player player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private RuntimeAnimatorController dreamerIdle;

    // Start is called before the first frame update
    void Start()
    {
        dialoguePrompt = GameObject.Find("Dialogue Prompt").GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").GetComponent<Player>();
        
        menu = GameObject.Find("Canvas").GetComponent<Menus>();

        //Automatically start opening cutscene when its DialogueHandler is active
        if (gameObject.name == "OpeningDialogueHandler")
        {
            dialogueActivated = true;
            healthInvMap.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If input for dialogue is given, OR it's the opening cutscene which starts automatically
        if ((Input.GetButtonDown("Melee") && dialogueActivated && canContinueToNextLine && !menu.getPaused() && !menu.getDead() && !menu.getMapOpen()) ||
            (gameObject.name == "OpeningDialogueHandler" && !openingCutsceneStarted && !menu.getPaused() && !menu.getDead() && !menu.getMapOpen()))
        {
            openingCutsceneStarted = true;

            if(gameObject.name == "DoorEndingDialogueHandler")
            {
                playerAnimator.runtimeAnimatorController = dreamerIdle;
            }

            if(gameObject.name == "ExplosionDialogueHandler" && step == 3)
            {
                explosionSprite.enabled = true;
                explosion.enabled = true;
                explosion.Rebind();
                explosion.Update(0f);
                explosionSound.Play();
                StartCoroutine(StopExplosion());
            }

            //Ends cutscene if the amount of steps reaches the set amount
            if (step >= speakers.Length)
            {
                StartCoroutine(EndCutscene());
                step = 0;

                if(gameObject.name == "OpeningDialogueHandler")
                {
                    healthInvMap.SetActive(true);
                }

                //Disables specified cutscene from replaying again
                if (gameObject.name == "OpeningDialogueHandler" ||
                    gameObject.name == "DoorEndingDialogueHandler")
                {
                    openingCutsceneComplete = true;
                    player.setInCutscene(false);
                    dialogueActivated = false;
                    gameObject.SetActive(false);
                }
                if(gameObject.name == "DoorEndingDialogueHandler")
                {
                    player.setInCutscene(true);
                    doorUnlockCutsceneStarted = true;
                }
                
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

            //keeps the Opening and End Door from showing the "Speak" prompt
            if(gameObject.name == "DoorDialogueHandler" ||
                gameObject.name == "DoorEndingDialogueHandler" ||
                gameObject.name == "OpeningDialogueHandler")
            {
                dialoguePrompt.enabled = false;
                dialogueActivated = true;
            }
            //keeps the power cutscenes from replaying if the player already has the respective power
            else if ((gameObject.name == "SwordDialogueHandler" && player.getHasSword() == true) ||
                (gameObject.name == "GunDialogueHandler" && player.getHasGun() == true) ||
                (gameObject.name == "BootsDialogueHandler" && player.getHasBoots() == true) ||
                (gameObject.name == "DashDialogueHandler" && player.getHasDash() == true))
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

            if (dialogueWindow != null)
            {
                dialogueWindow.SetActive(false);
            }

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

        canContinueToNextLine = false;
        proceedPrompt.enabled = false;

        foreach (char letter in line.ToCharArray())
        {
            //Skipping to show full text
            /*if (Input.GetButtonDown("Melee"))
            {
                dialogueText.text = bodyTexts[step];
                break;
            }
            */

            dialogueText.text += letter;
            yield return new WaitForSeconds(readSpeed);
        }

        canContinueToNextLine = true;
        proceedPrompt.enabled = true;
    }

    IEnumerator StopExplosion()
    {
        yield return new WaitForSeconds(2);
        explosion.enabled = false;
        explosionSprite.enabled = false;
    }

    public bool getDoorUnlockCutsceneStarted()
    {
        return doorUnlockCutsceneStarted;
    }

    public bool setDoorUnlockCutsceneStarted(bool b)
    {
        doorUnlockCutsceneStarted = b;
        return doorUnlockCutsceneStarted;
    }

    public bool getOpeningCutsceneComplete()
    {
        return openingCutsceneComplete;
    }
}
