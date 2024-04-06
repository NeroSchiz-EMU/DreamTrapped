using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCollectible : MonoBehaviour
{
    private SpriteRenderer swordPrompt;
    private SpriteRenderer bootsPrompt;
    private SpriteRenderer gunPrompt;
    private SpriteRenderer dashPrompt;

    [SerializeField] private SpriteRenderer mySprite;
    [SerializeField] private Player player;

    [SerializeField] private Sprite swordStoneEmpty;
    [SerializeField] private Sprite bootsMannequinEmpty;
    [SerializeField] private Sprite gunMannequinEmpty;
    [SerializeField] private Sprite dashMannequinEmpty;

    // Start is called before the first frame update
    void Start()
    {
        swordPrompt = GameObject.Find("Sword Prompt").GetComponent<SpriteRenderer>();
        bootsPrompt = GameObject.Find("Boots Prompt").GetComponent<SpriteRenderer>();
        gunPrompt = GameObject.Find("Gun Prompt").GetComponent<SpriteRenderer>();
        dashPrompt = GameObject.Find("Dash Prompt").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Obtain the collectible, depending on which source it was from
        if (swordPrompt.enabled && Input.GetButtonDown("Melee"))
        {
            player.setHasSword(true);
            if (gameObject.name == "Sword Stone")
            {
                mySprite.sprite = swordStoneEmpty;
                swordPrompt.enabled = false;
            }

        }
        if (bootsPrompt.enabled && Input.GetButtonDown("Melee"))
        {
            player.setHasBoots(true);
            if (gameObject.name == "Boots Mannequin")
            {
                mySprite.sprite = bootsMannequinEmpty;
                bootsPrompt.enabled = false;
            }
        }
        if (gunPrompt.enabled && Input.GetButtonDown("Melee"))
        {
            player.setHasGun(true);
            if (gameObject.name == "Gun Mannequin")
            {
                mySprite.sprite = gunMannequinEmpty;
                gunPrompt.enabled = false;
            }
        }
        if (dashPrompt.enabled && Input.GetButtonDown("Melee"))
        {
            player.setHasDash(true);
            if (gameObject.name == "Dash Mannequin")
            {
                mySprite.sprite = dashMannequinEmpty;
                dashPrompt.enabled = false;
            }
        }
    }

    //Enable prompt depending on which collectible it is
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && gameObject.name == "Sword Stone")
        {
            if(player.getHasSword() == false) swordPrompt.enabled = true;
        }
        if (collision.tag == "Player" && gameObject.name == "Boots Mannequin")
        {
            if (player.getHasBoots() == false) bootsPrompt.enabled = true;
        }
        if (collision.tag == "Player" && gameObject.name == "Gun Mannequin")
        {
            if (player.getHasGun() == false) gunPrompt.enabled = true;
        }
        if (collision.tag == "Player" && gameObject.name == "Dash Mannequin")
        {
            if (player.getHasDash() == false) dashPrompt.enabled = true;
        }
    }

    //Disable prompt when player leaves range
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            swordPrompt.enabled = false;
            bootsPrompt.enabled = false;
            gunPrompt.enabled = false;
            dashPrompt.enabled = false;
        }
    }
}
