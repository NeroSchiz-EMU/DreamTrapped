using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private Image swordIcon;
    private Image bootsIcon;
    private Image gunIcon;
    private Image dashIcon;

    [SerializeField] private InputPromptSwitcher promptSwitcher;

    [SerializeField] private ParticleSystem swordParticles;
    [SerializeField] private ParticleSystem bootsParticles;
    [SerializeField] private ParticleSystem gunParticles;
    [SerializeField] private ParticleSystem dashParticles;

    private bool swordParticlesPlayed;
    private bool bootsParticlesPlayed;
    private bool gunParticlesPlayed;
    private bool dashParticlesPlayed;

    [SerializeField] Player player;

    [Header("Item Sprites")]
    [SerializeField] private Sprite swordFull;
    [SerializeField] private Sprite bootsFull;
    [SerializeField] private Sprite gunFull;
    [SerializeField] private Sprite dashFull;

    // Start is called before the first frame update
    void Start()
    {
        swordIcon = transform.Find("Sword").GetComponent<Image>();
        bootsIcon = transform.Find("Boots").GetComponent<Image>();
        gunIcon = transform.Find("Gun").GetComponent<Image>();
        dashIcon = transform.Find("Dash").GetComponent<Image>();

        /*swordKey = GameObject.Find("Sword-key");
        bootsKey = GameObject.Find("Boots-key");
        gunKey = GameObject.Find("Gun-key");
        dashKey = GameObject.Find("Dash-key");

        swordButton = GameObject.Find("Sword-button");
        bootsButton = GameObject.Find("Boots-button");
        gunButton = GameObject.Find("Gun-button");
        dashButton = GameObject.Find("Dash-button");*/
    }

    // Update is called once per frame
    void Update()
    {
        if (player.getHasSword())
        {
            swordIcon.sprite = swordFull;
            promptSwitcher.updatePrompts();
            if (!swordParticlesPlayed)
            {
                swordParticles.Play();
                swordParticlesPlayed = true;
            }
        }
        if (player.getHasBoots())
        {
            bootsIcon.sprite = bootsFull;
            promptSwitcher.updatePrompts();
            if (!bootsParticlesPlayed)
            {
                bootsParticles.Play();
                bootsParticlesPlayed = true;
            }
        }
        if (player.getHasGun())
        {
            gunIcon.sprite = gunFull;
            promptSwitcher.updatePrompts();
            if (!gunParticlesPlayed)
            {
                gunParticles.Play();
                gunParticlesPlayed = true;
            }
        }
        if (player.getHasDash())
        {
            dashIcon.sprite = dashFull;
            promptSwitcher.updatePrompts();
            if (!dashParticlesPlayed)
            {
                dashParticles.Play();
                dashParticlesPlayed = true;
            }
        }


    }
}
