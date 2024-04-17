using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDoor : MonoBehaviour
{
    private SpriteRenderer door;
    private SpriteRenderer sword;
    private SpriteRenderer boots;
    private SpriteRenderer gun;
    private SpriteRenderer dash;
    private SpriteRenderer core;

    private SpriteRenderer doorPrompt;
    [SerializeField] private SpriteRenderer escapePrompt;

    [SerializeField] private Sprite[] coreSprites;
    [SerializeField] private Sprite doorFull;

    [SerializeField] private Player player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator fadeToWhite;

    [SerializeField] private RuntimeAnimatorController dreamerIdle;

    [SerializeField] private int abilityAmount;
    private bool swordCounted;
    private bool bootsCounted;
    private bool gunCounted;
    private bool dashCounted;

    private GameObject doorDialogueHandler;
    [SerializeField] private GameObject doorEndingDialogueHandler;
    [SerializeField] private Dialogue dialogueScript;
    private bool doorOpened;
    private bool doorOpening;

    // Start is called before the first frame update
    void Start()
    {
        door = transform.Find("Door").GetComponent<SpriteRenderer>();
        sword = transform.Find("Sword Door").GetComponent<SpriteRenderer>();
        boots = transform.Find("Boots Door").GetComponent<SpriteRenderer>();
        gun = transform.Find("Gun Door").GetComponent<SpriteRenderer>();
        dash = transform.Find("Dash Door").GetComponent<SpriteRenderer>();
        core = transform.Find("Core").GetComponent<SpriteRenderer>();

        doorPrompt = GameObject.Find("Door Prompt").GetComponent<SpriteRenderer>();

        doorDialogueHandler = GameObject.Find("DoorDialogueHandler");
    }

    // Update is called once per frame
    void Update()
    {
        //Show filled powers on the door
        if (player.getHasSword())
        {
            sword.enabled = true;
            if (!swordCounted) abilityAmount++;
            swordCounted = true;
        }
        if (player.getHasBoots())
        {
            boots.enabled = true;
            if (!bootsCounted) abilityAmount++;
            bootsCounted = true;
        }
        if (player.getHasGun())
        {
            gun.enabled = true;
            if (!gunCounted) abilityAmount++;
            gunCounted = true;
        }
        if (player.getHasDash())
        {
            dash.enabled = true;
            if (!dashCounted) abilityAmount++;
            dashCounted = true;
        }

        //Core sprite swapping
        if (abilityAmount == 0)
        {
            core.enabled = false;
        }
        if (abilityAmount == 1)
        {
            core.enabled = true;
            core.sprite = coreSprites[1];
        }
        if (abilityAmount == 2)
        {
            core.enabled = true;
            core.sprite = coreSprites[2];
        }
        if (abilityAmount == 3)
        {
            core.enabled = true;
            core.sprite = coreSprites[3];
        }

        //Door filled
        if (abilityAmount == 4)
        {
            sword.enabled = false;
            boots.enabled = false;
            gun.enabled = false;
            dash.enabled = false;
            core.enabled = false;

            doorDialogueHandler.SetActive(false);
            if(!doorOpening) doorEndingDialogueHandler.SetActive(true);

            door.sprite = doorFull;
        }

        if (dialogueScript.getDoorUnlockCutsceneStarted())
        {
            //playerAnimator.SetBool("doorOpening", true);
            doorOpening = true;
            doorPrompt.enabled = false;
            doorEndingDialogueHandler.SetActive(false);
            animator.enabled = true;
            animator.SetTrigger("unlocked");
            dialogueScript.setDoorUnlockCutsceneStarted(false);
            StartCoroutine(DoorOpen());
        }
        
        if (doorOpened && Input.GetButtonDown("Melee"))
        {
            StartCoroutine(FadeToEnding());
        }

        
    }

    //Enable "???" prompt
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            doorPrompt.enabled = true;
        }
       
    }

    //Disable prompt when player leaves range
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            doorPrompt.enabled = false;
        }
    }

    IEnumerator DoorOpen()
    {
        yield return new WaitForSeconds(9);
        doorOpened = true;
        escapePrompt.enabled = true;
    }

    IEnumerator FadeToEnding()
    {
        fadeToWhite.enabled = true;
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Ending");
    }

    public bool getDoorOpening()
    {
        return doorOpening;
    }

}
